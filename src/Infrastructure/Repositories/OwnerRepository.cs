using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;
using TestMillion.Infrastructure.Persistence.MongoDB;
using TestMillion.Infrastructure.Repositories.Base;

namespace TestMillion.Infrastructure.Repositories;

public class OwnerRepository : BaseRepository<Owner>, IOwnerRepository
{
    private readonly IMongoCollection<Property> _propertyCollection;

    public OwnerRepository(IOptions<MongoDbSettings> settings) : base(settings)
    {
        _propertyCollection = Database.GetCollection<Property>("Properties");
    }

    protected override string GetCollectionName() => "Owners";

    public async Task<bool> HasPropertiesAsync(string ownerId)
    {
        return await _propertyCollection.Find(p => p.IdOwner == ownerId).AnyAsync();
    }
}