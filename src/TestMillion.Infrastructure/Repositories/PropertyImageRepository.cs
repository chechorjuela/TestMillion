using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;
using TestMillion.Infrastructure.Persistence.MongoDB;
using TestMillion.Infrastructure.Repositories.Base;

namespace TestMillion.Infrastructure.Repositories;

public class PropertyImageRepository : BaseRepository<PropertyImage>, IPropertyImageRepository
{
    public PropertyImageRepository(IOptions<MongoDbSettings> settings) : base(settings)
    {
    }

    protected override string GetCollectionName() => "PropertyImages";

    public async Task<IEnumerable<PropertyImage>> GetImagesByPropertyAsync(string propertyId)
    {
        return await FindAsync(p => p.IdProperty == propertyId);
    }

    public async Task<PropertyImage?> GetMainImageAsync(string propertyId)
    {
        return await Collection
            .Find(p => p.IdProperty == propertyId && p.Enabled)
            .FirstOrDefaultAsync();
    }
}