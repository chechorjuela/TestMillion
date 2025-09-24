using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;
using TestMillion.Infrastructure.Persistence.MongoDB;
using TestMillion.Infrastructure.Repositories.Base;

namespace TestMillion.Infrastructure.Repositories;

public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
{
    public PropertyRepository(IOptions<MongoDbSettings> settings) : base(settings)
    {
    }

    protected override string GetCollectionName() => "Properties";

    public async Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(string ownerId)
    {
        return await FindAsync(p => p.IdOwner == ownerId);
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