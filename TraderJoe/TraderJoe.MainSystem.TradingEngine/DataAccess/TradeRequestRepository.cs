using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using TraderJoe.MainSystem.TradingEngine.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess;

public class TradeRequestRepository(TradingEngineDbContext db) : Repository<TradeRequestEntity>(db), ITradeRequestRepository
{

    public async Task<IEnumerable<TradeRequestEntity>> GetFilteredAsync(string? symbol, DateTime? from, DateTime? to, string? status)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(symbol))
            query = query.Where(r => r.Symbol == symbol.ToUpper());

        if (from.HasValue)
            query = query.Where(r => r.SubmittedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(r => r.SubmittedAt <= to.Value);

        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
            query = query.Where(r => r.Status == parsedStatus);

        return await query
            .OrderByDescending(r => r.SubmittedAt)
            .ToListAsync();
    }

    public async Task<TradeRequestEntity?> GetByIdempotencyKeyAsync(Guid idempotencyKey) =>
    await _dbSet.FirstOrDefaultAsync(r => r.IdempotencyKey == idempotencyKey);
}
