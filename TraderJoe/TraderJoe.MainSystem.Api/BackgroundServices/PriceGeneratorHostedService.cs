using System.ComponentModel.DataAnnotations;
using TraderJoe.PricingEngine.MarketDataSimulation.DataExample;
using TraderJoe.PricingEngine.Models;
using TraderJoe.PricingEngine.Publisher;
using TraderJoe.PricingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.Api.BackgroundServices;

// hosted here only because we use an in-memory Channel
public class PriceGeneratorHostedService(
    IMarketDataFeed feed,
    IPriceValidator validator,
    IPriceNormalizer normalizer,
    IPricePublisher publisher) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var symbols = SymbolBasePrices.Prices.Keys.ToList();

        var tasks = symbols.Select(symbol =>
            GenerateForSymbolAsync(symbol, ct));

        await Task.WhenAll(tasks);
    }

    private async Task GenerateForSymbolAsync(string symbol, CancellationToken ct)
    {
        var currentPrice = SymbolBasePrices.Prices[symbol];

        while (!ct.IsCancellationRequested)
        {
            var rawPrice = new RawPriceData
            {
                Symbol = symbol,
                BidPrice = currentPrice * 0.999m,
                AskPrice = currentPrice * 1.001m,
                Timestamp = DateTime.UtcNow
            };

            if (validator.IsValid(rawPrice))
            {
                var normalized = normalizer.Normalize(rawPrice);
                await publisher.PublishAsync(normalized, ct);
            }

            await Task.Delay(500, ct);
        }
    }

}
