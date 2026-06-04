using TraderJoe.PricingEngine.Models;

namespace TraderJoe.PricingEngine.Services.Interfaces;

public interface IMarketDataFeed
{
    IAsyncEnumerable<RawPriceData> StreamAsync(IEnumerable<string> symbols, CancellationToken ct);
}
