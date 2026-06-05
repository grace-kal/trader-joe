using Microsoft.EntityFrameworkCore;
using TraderJoe.MainSystem.TradingEngine.DataAccess.Interfaces;

namespace TraderJoe.MainSystem.TradingEngine.DataAccess;

public class Repository<T>(TradingEngineDbContext db) : IRepository<T> where T : class
{
    protected readonly TradingEngineDbContext _db = db;
    protected readonly DbSet<T> _dbSet = db.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    public async Task UpdateAsync(T entity) =>
        _dbSet.Update(entity);

    public async Task SaveChangesAsync() =>
        await _db.SaveChangesAsync();
}
