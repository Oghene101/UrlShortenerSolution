using System.Linq.Expressions;

namespace Url_Shortener.Data.Abstractions;

public interface IRepository<T>
{
    Task AddAsync(T entity);
    Task<T> FindByIdAsync(string id);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task SaveChangesAsync();

}
