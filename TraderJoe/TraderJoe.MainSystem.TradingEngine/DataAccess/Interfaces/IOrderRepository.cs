using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;

public interface IOrderRepository : IRepository<OrderEntity>
{
    Task<IEnumerable<OrderEntity>> GetBySymbolAsync(string symbol);
}
