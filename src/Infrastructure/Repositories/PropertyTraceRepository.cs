using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;
using TestMillion.Infrastructure.Persistence.MongoDB;
using TestMillion.Infrastructure.Repositories.Base;

namespace TestMillion.Infrastructure.Repositories;

public class PropertyTraceRepository : BaseRepository<PropertyTrace>, IPropertyTraceRepository
{
    public PropertyTraceRepository(IOptions<MongoDbSettings> settings) : base(settings)
    {
    }

    protected override string GetCollectionName() => "PropertiesTrace";

    public Task<IEnumerable<PropertyTrace>> GetPropertiesByOwnerAsync(string ownerId)
    {
        throw new NotImplementedException();
    }
}