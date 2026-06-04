using System.Runtime.CompilerServices;
using TraderJoe.PricingEngine.MarketDataSimulation.DataExample;
using TraderJoe.PricingEngine.Models;
using TraderJoe.PricingEngine.Services.Interfaces;

namespace TraderJoe.PricingEngine.MarketDataSimulation;

public class SimulatedMarketDataFeed : IMarketDataFeed
{
    private readonly Dictionary<string, decimal> _currentPrices = new();

    public async IAsyncEnumerable<RawPriceData> StreamAsync(IEnumerable<string> symbols, [EnumeratorCancellation] CancellationToken ct)
    {
        foreach (var symbol in symbols)
            _currentPrices[symbol] = SymbolBasePrices.Prices.GetValueOrDefault(symbol, 100m);

        while (!ct.IsCancellationRequested)
        {
            foreach (var symbol in _currentPrices.Keys)
            {
                ct.ThrowIfCancellationRequested();

                var price = GenerateNext(symbol);
                if (price is not null)
                    yield return price;
            }

            //symbols get new price every 500ms, seems industry accurate enough for the simulation purposee
            await Task.Delay(500, ct);
        }
    }

    private RawPriceData? GenerateNext(string symbol)
    {
        var lastPrice = _currentPrices[symbol];

        //used AI to help me determine realistic price movement 
        var priceMovement = (decimal)(Random.Shared.NextDouble() * 0.002 - 0.001);
        var newMid = Math.Round(lastPrice * (1 + priceMovement), 4);

        var spreadPercent = (decimal)(Random.Shared.NextDouble() * 0.0004 + 0.0001);
        var halfSpread = Math.Round(newMid * spreadPercent / 2, 4);
        //end of AI assited logic, I promise :)

        var data = new RawPriceData
        {
            Symbol = symbol,
            BidPrice = newMid - halfSpread,
            AskPrice = newMid + halfSpread,
            Timestamp = DateTime.UtcNow
        };

        if (data.BidPrice <= 0 || data.AskPrice <= data.BidPrice)
            return null;

        _currentPrices[symbol] = newMid;
        return data;
    }
}
