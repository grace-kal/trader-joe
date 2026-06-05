using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using TraderJoe.PricingEngine.MarketDataSimulation.DataExample;
using TraderJoe.PricingEngine.Models;
using TraderJoe.PricingEngine.Services.Interfaces;

namespace TraderJoe.PricingEngine.MarketDataSimulation;

public class SimulatedMarketDataFeed : IMarketDataFeed
{
    private readonly ConcurrentDictionary<string, decimal> _currentPrices = new();

    public async IAsyncEnumerable<RawPriceData> StreamAsync(
        IEnumerable<string> symbols,
        [EnumeratorCancellation] CancellationToken ct)
    {
        var symbolList = symbols.ToList();
        var channel = Channel.CreateUnbounded<RawPriceData>();

        // one producer per symbol — they run concurrently
        var producers = symbolList.Select(s => ProduceForSymbolAsync(s, channel.Writer, ct));

        // complete the channel once all producers stop (on cancellation)
        _ = Task.WhenAll(producers)
            .ContinueWith(_ => channel.Writer.Complete(), TaskScheduler.Default);

        await foreach (var price in channel.Reader.ReadAllAsync(ct))
            yield return price;
    }

    private async Task ProduceForSymbolAsync(
        string symbol, ChannelWriter<RawPriceData> writer, CancellationToken ct)
    {
        _currentPrices[symbol] = SymbolBasePrices.Prices.GetValueOrDefault(symbol, 100m);

        while (!ct.IsCancellationRequested)
        {
            var price = GenerateNext(symbol);
            if (price is not null)
                await writer.WriteAsync(price, ct);

            await Task.Delay(500, ct);
        }
    }

    private RawPriceData? GenerateNext(string symbol)
    {
        var lastPrice = _currentPrices[symbol];

        // used AI to help me determine realistic price movement
        var priceMovement = (decimal)(Random.Shared.NextDouble() * 0.002 - 0.001);
        var newMid = Math.Round(lastPrice * (1 + priceMovement), 4);
        var spreadPercent = (decimal)(Random.Shared.NextDouble() * 0.0004 + 0.0001);
        var halfSpread = Math.Round(newMid * spreadPercent / 2, 4);
        
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
