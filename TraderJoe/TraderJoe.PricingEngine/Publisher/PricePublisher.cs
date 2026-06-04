using System.Threading.Channels;
using TraderJoe.SharedNuget.Models;

namespace TraderJoe.PricingEngine.Publisher;

public class PricePublisher(ChannelWriter<PriceUpdateEvent> writer) : IPricePublisher
{
    public async ValueTask PublishAsync(PriceUpdateEvent priceUpdate, CancellationToken ct = default)
    {
        await writer.WriteAsync(priceUpdate, ct);
    }
}