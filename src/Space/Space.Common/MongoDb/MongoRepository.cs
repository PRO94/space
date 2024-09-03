using System.Linq.Expressions;
using MongoDB.Driver;

namespace Space.Common.MongoDb;

public class MongoRepository<T> : IRepository<T> where T : IEntity
{
    private readonly IMongoCollection<T> dbCollection;
    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        dbCollection = database.GetCollection<T>(collectionName);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        return await dbCollection.Find(filter).ToListAsync();
    }

    public async Task<IReadOnlyCollection<T>> GetManyAsync(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        return await dbCollection.Find(filterBuilder.Empty).Limit(amount).ToListAsync();
    }

    public async Task<IReadOnlyCollection<T>> GetManyAsync(Expression<Func<T, bool>> filter, int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        return await dbCollection.Find(filter).Limit(amount).ToListAsync();
    }

    public async Task<T> GetAsync(Guid id)
    {
        FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);

        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
    {
        return await dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentException(nameof(entity));
        }

        await dbCollection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentException(nameof(entity));
        }

        FilterDefinition<T> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);

        await dbCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);

        await dbCollection.DeleteOneAsync(filter);
    }

    public async Task RemoveManyAsync(IReadOnlyCollection<Guid> ids)
    {
        var filter = filterBuilder.In(entity => entity.Id, ids);

        await dbCollection.DeleteManyAsync(filter);
    }
}