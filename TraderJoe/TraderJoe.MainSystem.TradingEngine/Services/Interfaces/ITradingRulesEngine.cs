using TraderJoe.MainSystem.TradingEngine.Models.Price;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;

namespace TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

public interface ITradingRulesEngine
{
    ValidationResult Validate(TradeRequest request, PriceState priceState, TradingRules rules);
}