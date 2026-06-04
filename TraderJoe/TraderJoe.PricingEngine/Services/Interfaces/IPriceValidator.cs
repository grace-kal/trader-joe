using TraderJoe.PricingEngine.Models;

namespace TraderJoe.PricingEngine.Services.Interfaces;

public interface IPriceValidator
{
    bool IsValid(RawPriceData data);
}
