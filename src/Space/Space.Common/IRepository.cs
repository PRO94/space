using System.Linq.Expressions;

namespace Space.Common;

public interface IRepository<T> where T : IEntity
{
    Task<IReadOnlyCollection<T>> GetAllAsync();

    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);

    Task<IReadOnlyCollection<T>> GetManyAsync(int amount);

    Task<IReadOnlyCollection<T>> GetManyAsync(Expression<Func<T, bool>> filter, int amount);

    Task<T> GetAsync(Guid id);

    Task<T> GetAsync(Expression<Func<T, bool>> filter);

    Task CreateAsync(T entity);

    Task UpdateAsync(T entity);

    Task RemoveAsync(Guid id);

    Task RemoveManyAsync(IReadOnlyCollection<Guid> ids);
}