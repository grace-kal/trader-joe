using TraderJoe.MainSystem.TradingEngine.Models.Price;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;

namespace TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

public interface IAutoTradingEngine
{
    TradeRequest? EvaluateAutoTradeRequestCreation(PriceState priceState, TradingRules rules);
}
