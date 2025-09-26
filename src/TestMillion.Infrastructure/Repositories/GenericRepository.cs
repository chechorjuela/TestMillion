using System.Linq.Expressions;
using MongoDB.Driver;
using TestMillion.Domain.Common.Entities;
using TestMillion.Infrastructure.Persistence.MongoDB;
using Microsoft.Extensions.Options;
using TestMillion.Domain.Interfaces.Base;
using TestMillion.Domain.Common.Models;
using MongoDB.Driver.Linq;

namespace TestMillion.Infrastructure.Repositories;

public class GenericRepository<T> : IBaseRepository<T> where T : IEntity
{
    protected readonly IMongoCollection<T> _collection;

    public GenericRepository(IOptions<MongoDbSettings> settings)
    {
        var database = new MongoClient(settings.Value.ConnectionString).GetDatabase(settings.Value.DatabaseName);
        _collection = database.GetCollection<T>(typeof(T).Name);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        if (!MongoDB.Bson.ObjectId.TryParse(id, out var objectId))
            return default;

        var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<T?> UpdateAsync(T entity)
    {
        var filter = Builders<T>.Filter.Eq(doc => doc.Id, entity.Id);
        var result = await _collection.ReplaceOneAsync(filter, entity);
        return result.ModifiedCount > 0 ? entity : default;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
        var result = await _collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    public Task<bool> ExistsAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        throw new NotImplementedException();
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

    // New interface implementations
    public async Task<PaginatedResponse<T>> GetPagedAsync(PaginationModel pagination)
    {
        var skip = (pagination.PageNumber - 1) * pagination.PageSize;
        var total = (int)await _collection.CountDocumentsAsync(_ => true);
        var items = await _collection
            .Find(_ => true)
            .Skip(skip)
            .Limit(pagination.PageSize)
            .ToListAsync();
        return PaginatedResponse<T>.Create(items, total, pagination.PageNumber, pagination.PageSize);
    }

    public async Task<PaginatedResponse<T>> GetPagedAsync(PaginationModel pagination, FilterModel filter)
    {
        var query = _collection.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(x => x.ToString()!.ToLower().Contains(filter.SearchTerm!.ToLower()));
        }

        query = filter.SortDesc ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);

        var total = (int)await query.CountAsync();
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return PaginatedResponse<T>.Create(items, total, pagination.PageNumber, pagination.PageSize);
    }
}