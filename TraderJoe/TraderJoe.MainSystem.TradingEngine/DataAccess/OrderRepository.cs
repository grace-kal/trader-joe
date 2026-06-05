using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess;

public class OrderRepository(TradingEngineDbContext db) : Repository<OrderEntity> (db), IOrderRepository
{
    public async Task<IEnumerable<OrderEntity>> GetBySymbolAsync(string symbol) =>
        await _dbSet
            .Where(o => o.Symbol == symbol.ToUpper())
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
}
