using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using TestMillion.Domain.Entities;
using TestMillion.Infrastructure.Persistence.MongoDB.Serializers;

namespace TestMillion.Infrastructure.Persistence.MongoDB;

public static class MongoConfig
{
    private static bool _initialized;
    private static readonly object _lock = new();

    public static void Initialize()
    {
        if (_initialized) return;

        lock (_lock)
        {
            if (_initialized) return;

            RegisterConventions();
            RegisterClassMaps();
            RegisterSerializers();

            _initialized = true;
        }
    }

    private static void RegisterConventions()
    {
        var conventionPack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true)
        };

        ConventionRegistry.Register("CustomConventions", conventionPack, _ => true);
    }

    private static void RegisterClassMaps()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(PropertyTrace)))
        {
            BsonClassMap.RegisterClassMap<PropertyTrace>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapMember(x => x.DateSale);
                cm.MapMember(x => x.Name);
                cm.MapMember(x => x.Value);
                cm.MapMember(x => x.Tax);
                cm.MapMember(x => x.IdProperty);
            });
        }
    }

    private static void RegisterSerializers()
    {
        if (!BsonSerializer.TryRegisterSerializer(new Serializers.DateOnlySerializer()))
        {
            throw new Exception("Failed to register DateOnly serializer");
        }
    }
}