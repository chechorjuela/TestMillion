using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TestMillion.Domain.Common.Entities;
using TestMillion.Domain.Interfaces.Base;
using TestMillion.Infrastructure.Persistence.MongoDB;

namespace TestMillion.Infrastructure.Repositories.Base;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IEntity
{
    protected readonly IMongoCollection<T> Collection;
    protected readonly IMongoDatabase Database;

    protected BaseRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        Database = client.GetDatabase(settings.Value.DatabaseName);
        Collection = Database.GetCollection<T>(GetCollectionName());
    }

    protected abstract string GetCollectionName();

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Collection.Find(_ => true).ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await Collection.Find(predicate).ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(string id)
    {
        return await Collection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await Collection.InsertOneAsync(entity);
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
    }

    public virtual async Task DeleteAsync(string id)
    {
        await Collection.DeleteOneAsync(e => e.Id == id);
    }

    public virtual async Task<bool> ExistsAsync(string id)
    {
        return await Collection.Find(e => e.Id == id).AnyAsync();
    }

    public virtual async Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate == null
            ? await Collection.CountDocumentsAsync(_ => true)
            : await Collection.CountDocumentsAsync(predicate);
    }
}