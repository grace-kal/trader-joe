using Microsoft.EntityFrameworkCore;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Entities;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess;

public class TradingEngineDbContext : DbContext
{
    public TradingEngineDbContext(DbContextOptions<TradingEngineDbContext> options)
       : base(options) { }

    public DbSet<SymbolPriceStateEntity> SymbolPriceStates => Set<SymbolPriceStateEntity>();
    public DbSet<TradeRequestEntity> TradeRequests => Set<TradeRequestEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<TradingRulesEntity> TradingRules => Set<TradingRulesEntity>();
}
