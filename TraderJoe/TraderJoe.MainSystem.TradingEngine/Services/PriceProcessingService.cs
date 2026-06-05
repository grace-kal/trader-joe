using Mapster;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using TraderJoe.MainSystem.TradingEngine.Models.Price;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;
using TraderJoe.SharedNuget.Models;

namespace TraderJoe.MainSystem.TradingEngine.Services;

public class PriceProcessingService(
    IPriceStateStore priceStateStore,
    IAutoTradingEngine autoTradingEngine,
    IOrderService orderService,
    IRulesService rulesService) : IPriceProcessingService
{
    public async Task ProcessPriceUpdateAsync(PriceUpdateEvent priceUpdate)
    {
        var previousPriceState = priceStateStore.Get(priceUpdate.Symbol);

        var currentMarketPrice = (priceUpdate.AskPrice + priceUpdate.BidPrice) / 2;
        var spread = priceUpdate.AskPrice - priceUpdate.BidPrice;
        var spreadPercentage = Math.Round((spread / currentMarketPrice) * 100, 4);

        var newPriceState = new PriceState
        {
            Symbol = priceUpdate.Symbol,
            BidPrice = priceUpdate.BidPrice,
            AskPrice = priceUpdate.AskPrice,
            CurrentMarketPrice = currentMarketPrice,
            Spread = spread,
            SpreadPercent = spreadPercentage,
            PreviousMarketPrice = previousPriceState?.CurrentMarketPrice,
            EventTimestamp = priceUpdate.EventTimestamp,
            UpdatedAt = DateTime.UtcNow
        };

        //in-memory fast update for quick access by trading logic
        priceStateStore.Update(newPriceState);

        var rules = await rulesService.GetCurrentRulesAsync();
        var autoOrder = autoTradingEngine.EvaluateAutoTradeRequestCreation(newPriceState, rules);

        if (autoOrder is not null)
            await orderService.ProcessAsync(autoOrder);
    }

}
