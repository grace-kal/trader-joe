using TraderJoe.SharedNuget.Models;

namespace TraderJoe.PricingEngine.Publisher;

public interface IPricePublisher
{
    ValueTask PublishAsync(PriceUpdateEvent priceUpdate, CancellationToken ct = default);
}