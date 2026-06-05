using TraderJoe.MainSystem.TradingEngine.Models.Enums;
using TraderJoe.MainSystem.TradingEngine.Models.Price;
using TraderJoe.MainSystem.TradingEngine.Models.Trading;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

namespace TraderJoe.MainSystem.TradingEngine.Services;

public class AutoTradingEngine : IAutoTradingEngine
{
    private const decimal PriceAdjustmentPercent = 0.0003m;
    private const decimal AutoOrderNotionalValue = 1000m;
    public TradeRequest? GenerateOrder(PriceState priceState, TradingRules rules)
    {
        if (priceState.SpreadPercent <= rules.AutoTradingSpreadThresholdPercent)
            return null;
        
        if (priceState.PreviousMarketPrice is null)
            return null;

        if (priceState.CurrentMarketPrice == priceState.PreviousMarketPrice)
            return null;

        var side = priceState.CurrentMarketPrice > priceState.PreviousMarketPrice
            ? OrderSide.Sell
            : OrderSide.Buy;

        var price = side == OrderSide.Sell
            ? priceState.AskPrice - (priceState.AskPrice * PriceAdjustmentPercent)
            : priceState.BidPrice + (priceState.BidPrice * PriceAdjustmentPercent);

        price = Math.Round(price, 4);

        var quantity = Math.Round(AutoOrderNotionalValue / priceState.CurrentMarketPrice, 4);

        return new TradeRequest
        {
            Symbol = priceState.Symbol,
            Price = price,
            Quantity = quantity,
            Type = OrderType.Limit,
            Side = side,
            Source = OrderSource.Auto
        };
    }
}
