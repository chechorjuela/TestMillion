using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;
using TestMillion.Infrastructure.Persistence.MongoDB;
using TestMillion.Infrastructure.Repositories.Base;

namespace TestMillion.Infrastructure.Repositories;

public class OwnerRepository : BaseRepository<Owner>, IOwnerRepository
{
    private readonly IMongoCollection<Owner> _ownerCollection;
    private readonly ILogger<OwnerRepository> _logger;

    public OwnerRepository(IOptions<MongoDbSettings> settings, ILogger<OwnerRepository> logger) : base(settings)
    {
        _ownerCollection = Database.GetCollection<Owner>("Owners");
        _logger = logger;
    }

    protected override string GetCollectionName() => "Owners";

    public async Task<Owner> GetByNameAsync(string name)
    {
        _logger.LogInformation("Getting owner by name: {Name}", name);
        var owner = await _ownerCollection.FindAsync(o => o.Name == name).Result.FirstOrDefaultAsync();
        if (owner == null)
        {
            _logger.LogWarning("Owner with name {Name} not found", name);
        }
        else
        {
            _logger.LogInformation("Found owner with ID {OwnerId} for name {Name}", owner.Id, name);
        }
        return owner;
    }
}