using TraderJoe.MainSystem.TradingEngine.Models.Trading;

namespace TraderJoe.MainSystem.TradingEngine.Services.Interfaces;

public interface IOrderService
{
    Task<TradeRequest> ProcessAsync(TradeRequest request);
}
