using TraderJoe.PricingEngine.Models;
using TraderJoe.SharedNuget.Models;

namespace TraderJoe.PricingEngine.Services.Interfaces;

public interface IPriceNormalizer
{
    PriceUpdateEvent Normalize(RawPriceData data);
}
