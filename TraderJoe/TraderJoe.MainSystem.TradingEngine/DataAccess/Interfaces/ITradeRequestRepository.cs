using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;

public interface ITradeRequestRepository : IRepository<TradeRequestEntity>
{
    Task<IEnumerable<TradeRequestEntity>> GetFilteredAsync(string? symbol, DateTime? from, DateTime? to, string? status);
    Task<TradeRequestEntity?> GetByIdempotencyKeyAsync(Guid idempotencyKey);
}
