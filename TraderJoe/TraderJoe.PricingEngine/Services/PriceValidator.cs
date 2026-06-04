using TraderJoe.PricingEngine.Models;
using TraderJoe.PricingEngine.Services.Interfaces;

namespace TraderJoe.PricingEngine.Services;

public class PriceValidator : IPriceValidator
{
    public bool IsValid(RawPriceData data) => data.BidPrice > 0 && data.AskPrice > 0 && data.BidPrice < data.AskPrice && !string.IsNullOrWhiteSpace(data.Symbol);
}
