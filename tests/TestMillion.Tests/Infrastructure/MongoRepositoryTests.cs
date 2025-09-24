using FluentAssertions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using TestMillion.Domain.Entities;
using TestMillion.Infrastructure.Persistence.MongoDB;

namespace TestMillion.Tests.Infrastructure;

public class MongoRepositoryTests
{
    private readonly Mock<IMongoCollection<Property>> _collectionMock;
    private readonly Mock<IMongoDatabase> _databaseMock;
    private readonly Mock<IMongoClient> _clientMock;
    private MongoRepository<Property> _repository;
    private MongoDbSettings _settings;

    [SetUp]
    public void Setup()
    {
        _settings = new MongoDbSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "TestMillionTest"
        };

        _collectionMock = new Mock<IMongoCollection<Property>>();
        _databaseMock = new Mock<IMongoDatabase>();
        _clientMock = new Mock<IMongoClient>();

        _clientMock.Setup(c => c
                .GetDatabase(_settings.DatabaseName, null))
            .Returns(_databaseMock.Object);

        _databaseMock.Setup(d => d
                .GetCollection<Property>("properties", null))
            .Returns(_collectionMock.Object);

        var options = Options.Create(_settings);
        _repository = new MongoRepository<Property>(options);
    }

    [Test]
    public async Task GetPagedAsync_WithValidFilter_ReturnsPagedResults()
    {
        // Arrange
        var filter = Builders<Property>.Filter.Empty;
        var properties = new List<Property>
        {
            new()
            {
                Id = "1",
                Name = "Test Property",
                Address = "123 Test St",
                Price = 100000M
            }
        };

        var asyncCursor = new Mock<IAsyncCursor<Property>>();
        asyncCursor.Setup(a => a.Current).Returns(properties);
        asyncCursor
            .SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);

        _collectionMock.Setup(c => c
                .FindAsync(
                    It.IsAny<FilterDefinition<Property>>(),
                    It.IsAny<FindOptions<Property>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursor.Object);

        _collectionMock.Setup(c => c
                .CountDocumentsAsync(
                    It.IsAny<FilterDefinition<Property>>(),
                    It.IsAny<CountOptions>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _repository.GetPagedAsync(
            filter,
            page: 1,
            pageSize: 10,
            sortField: "Name",
            ascending: true);

        // Assert
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
    }
}