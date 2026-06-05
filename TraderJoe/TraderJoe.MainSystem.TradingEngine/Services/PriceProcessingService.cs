using Mapster;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using TraderJoe.MainSystem.TradingEngine.Models.Price;
using TraderJoe.MainSystem.TradingEngine.Services.Interfaces;
using TraderJoe.SharedNuget.Models;

namespace TraderJoe.MainSystem.TradingEngine.Services;

public class PriceProcessingService(
    IPriceStateStore priceStateStore,
    IPriceStateRepository priceStateRepository,
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
            UpdatedAt = DateTime.UtcNow
        };

        //in-memory fast update for quick access by trading logic
        priceStateStore.Update(newPriceState);

        //persist in db as well
        var entity = newPriceState.Adapt<SymbolPriceStateEntity>();
        await priceStateRepository.UpsertAsync(entity);
        await priceStateRepository.SaveChangesAsync();

        var rules = await rulesService.GetCurrentRulesAsync();
        var autoOrder = autoTradingEngine.GenerateOrder(newPriceState, rules);

        if (autoOrder is not null)
            await orderService.ProcessAsync(autoOrder);
    }

}
