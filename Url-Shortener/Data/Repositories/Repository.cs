using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UrlShortener.Data.Abstractions;

namespace UrlShortener.Data.Repositories;

public class Repository<T>(AppDbContext dbContext) : IRepository<T> where T : class
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<T> FindByIdAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return (await _dbSet.FirstOrDefaultAsync(predicate) == null) ? false : true;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

}
