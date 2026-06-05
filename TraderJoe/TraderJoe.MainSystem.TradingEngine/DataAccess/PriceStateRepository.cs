using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess;

public class PriceStateRepository(TradingEngineDbContext db) : Repository<SymbolPriceStateEntity>(db), IPriceStateRepository
{
    public async Task UpsertAsync(SymbolPriceStateEntity entity)
    {
        var existing = await _dbSet.FindAsync(entity.Symbol);

        if (existing is null)
            await _dbSet.AddAsync(entity);
        else
            _db.Entry(existing).CurrentValues.SetValues(entity);
    }
}
