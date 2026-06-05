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

        // StreamAsync runs one producer per symbol concurrently (internally)
        await foreach (var rawPrice in feed.StreamAsync(symbols, ct))
        {
            if (!validator.IsValid(rawPrice))
                continue;

            var normalized = normalizer.Normalize(rawPrice);
            await publisher.PublishAsync(normalized, ct);
        }
    }

}
