using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess;

public class RulesRepository(TradingEngineDbContext db) : Repository<TradingRulesEntity>(db), IRulesRepository
{
    public async Task<TradingRulesEntity?> GetCurrentAsync() =>
        await _dbSet
            .OrderByDescending(r => r.UpdatedAt)
            .FirstOrDefaultAsync();
}
