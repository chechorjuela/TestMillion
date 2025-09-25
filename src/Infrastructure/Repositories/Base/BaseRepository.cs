using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TestMillion.Domain.Common.Entities;
using TestMillion.Domain.Interfaces.Base;
using TestMillion.Infrastructure.Persistence.MongoDB;
using TestMillion.Domain.Common.Models;
using MongoDB.Driver.Linq;

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
    try
    {
      var data = await Collection.Find(_ => true).ToListAsync();
      Console.WriteLine($"GetAllAsync for {typeof(T).Name}: Found {data.Count} items");
      return data;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error in GetAllAsync for {typeof(T).Name}: {ex.Message}");
      throw;
    }
  }

  public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
  {
    return await Collection.Find(predicate).ToListAsync();
  }

  public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(PaginationModel pagination)
  {
    var skip = (pagination.PageNumber - 1) * pagination.PageSize;
    var total = (int)await Collection.CountDocumentsAsync(_ => true);
    var items = await Collection
      .Find(_ => true)
      .Skip(skip)
      .Limit(pagination.PageSize)
      .ToListAsync();
    return (items, total);
  }

  public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(PaginationModel pagination, FilterModel filter)
  {
    var query = Collection.AsQueryable();

    if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
    {
      // naive string contains search across ToString
      query = query.Where(x => x.ToString()!.ToLower().Contains(filter.SearchTerm!.ToLower()));
    }

    if (!string.IsNullOrWhiteSpace(filter.SortBy))
    {
      // fallback: no dynamic sort binding, can be improved per-entity
      query = filter.SortDesc ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
    }

    var total = await query.CountAsync();
    var items = await query
      .Skip((pagination.PageNumber - 1) * pagination.PageSize)
      .Take(pagination.PageSize)
      .ToListAsync();

    return (items, (int)total);
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

  public virtual async Task<T?> UpdateAsync(T entity)
  {
    var result = await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
    if (result.ModifiedCount > 0)
      return await Collection.Find(e => e.Id == entity.Id).FirstOrDefaultAsync();
    return null;
  }

  public virtual async Task<bool> DeleteAsync(string id)
  {
    var result = await Collection.DeleteOneAsync(e => e.Id == id);
    return result.DeletedCount > 0;
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