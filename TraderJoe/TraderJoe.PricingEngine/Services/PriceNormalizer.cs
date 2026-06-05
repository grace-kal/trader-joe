using TraderJoe.PricingEngine.Models;
using TraderJoe.PricingEngine.Services.Interfaces;
using TraderJoe.SharedNuget.Models;

namespace TraderJoe.PricingEngine.Services;

public class PriceNormalizer : IPriceNormalizer
{
    public PriceUpdateEvent Normalize(RawPriceData data) =>
        new()
        {
            Symbol = data.Symbol.ToUpper().Trim(),
            BidPrice = Math.Round(data.BidPrice, 4),
            AskPrice = Math.Round(data.AskPrice, 4),
            EventTimestamp = data.Timestamp
        };
}
