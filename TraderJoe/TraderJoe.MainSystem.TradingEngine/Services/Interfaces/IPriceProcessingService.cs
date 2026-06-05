using TraderJoe.SharedNuget.Models;

namespace TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

public interface IPriceProcessingService
{
    Task ProcessPriceUpdateAsync(PriceUpdateEvent priceUpdate);
}
