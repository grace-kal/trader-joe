using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;

public interface IPriceStateRepository : IRepository<SymbolPriceStateEntity>
{
    Task UpsertAsync(SymbolPriceStateEntity entity);
}
