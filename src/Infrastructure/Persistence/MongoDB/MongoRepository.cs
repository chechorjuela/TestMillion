using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TestMillion.Domain.Common.Entities;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Infrastructure.Persistence.MongoDB;

public class MongoRepository<T> : IBaseRepository<T> where T : IEntity
{
    private readonly IMongoCollection<T> _collection;
    private readonly IMongoDatabase _database;

    public MongoRepository(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
        _collection = _database.GetCollection<T>(typeof(T).Name.ToLowerInvariant() + "s");
        
        // Ensure indexes for better query performance
        if (typeof(T).Name == "Property")
        {
            var indexKeysDefinition = Builders<T>.IndexKeys
                .Ascending("Name")
                .Ascending("Address")
                .Ascending("Price");
            
            var indexOptions = new CreateIndexOptions { Background = true };
            var indexModel = new CreateIndexModel<T>(indexKeysDefinition, indexOptions);
            
            _collection.Indexes.CreateOne(indexModel);
        }
    }

    public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true)
            .Limit(100)  // Default limit for safety
            .ToListAsync();
    }

    public async Task<(IEnumerable<T> Items, long TotalCount)> GetPagedAsync(
        FilterDefinition<T> filter,
        int page = 1,
        int pageSize = 10,
        string? sortField = null,
        bool ascending = true)
    {
        var query = _collection.Find(filter);
        var totalCount = await query.CountDocumentsAsync();

        if (!string.IsNullOrEmpty(sortField))
        {
            var sort = ascending
                ? Builders<T>.Sort.Ascending(sortField)
                : Builders<T>.Sort.Descending(sortField);
            query = query.Sort(sort);
        }

        var items = await query
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(e => e.Id == id);
    }

    public Task<bool> ExistsAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        throw new NotImplementedException();
    }
}