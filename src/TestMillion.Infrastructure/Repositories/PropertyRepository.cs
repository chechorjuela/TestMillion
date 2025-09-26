using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TestMillion.Domain.Common.Models;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;
using TestMillion.Infrastructure.Persistence.MongoDB;
using TestMillion.Infrastructure.Repositories.Base;
using MongoDB.Driver.Linq;

namespace TestMillion.Infrastructure.Repositories;

public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
{
    public PropertyRepository(IOptions<MongoDbSettings> settings) : base(settings)
    {
    }

    protected override string GetCollectionName() => "Properties";

    public async Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(string ownerId)
    {
        if (!MongoDB.Bson.ObjectId.TryParse(ownerId, out _))
            return Enumerable.Empty<Property>();
            
        return await FindAsync(p => p.IdOwner == ownerId);
    }

    public override async Task<PaginatedResponse<Property>> GetPagedAsync(PaginationModel pagination, FilterModel filter)
    {
        var query = Collection.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(p => 
                p.Name.ToLower().Contains(filter.SearchTerm.ToLower()) ||
                p.Address.ToLower().Contains(filter.SearchTerm.ToLower()));
        }

        if (filter.Filters.TryGetValue("name", out var name))
        {
            query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));
        }

        if (filter.Filters.TryGetValue("address", out var address))
        {
            query = query.Where(p => p.Address.ToLower().Contains(address.ToLower()));
        }

        if (filter.Filters.TryGetValue("minPrice", out var minPriceStr) && decimal.TryParse(minPriceStr, out var minPrice))
        {
            query = query.Where(p => p.Price >= minPrice);
        }

        if (filter.Filters.TryGetValue("maxPrice", out var maxPriceStr) && decimal.TryParse(maxPriceStr, out var maxPrice))
        {
            query = query.Where(p => p.Price <= maxPrice);
        }

        query = filter.SortBy?.ToLower() switch
        {
            "name" => filter.SortDesc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "price" => filter.SortDesc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "year" => filter.SortDesc ? query.OrderByDescending(p => p.Year) : query.OrderBy(p => p.Year),
            _ => query.OrderBy(p => p.Id)
        };

        var total = await query.CountAsync();
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return PaginatedResponse<Property>.Create(items, total, pagination.PageNumber, pagination.PageSize);
    }

    public async Task<bool> IsCodeInternalUniqueAsync(string codeInternal, string? excludeId = null)
    {
        var filter = Builders<Property>.Filter.Eq(p => p.CodeInternal, codeInternal);
        if (!string.IsNullOrEmpty(excludeId))
        {
            filter = filter & Builders<Property>.Filter.Ne(p => p.Id, excludeId);
        }
        return !await Collection.Find(filter).AnyAsync();
    }
}