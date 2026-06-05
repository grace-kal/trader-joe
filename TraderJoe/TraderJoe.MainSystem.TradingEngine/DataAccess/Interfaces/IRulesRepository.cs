using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;

public interface IRulesRepository : IRepository<TradingRulesEntity>
{
    Task<TradingRulesEntity?> GetCurrentAsync();
}
