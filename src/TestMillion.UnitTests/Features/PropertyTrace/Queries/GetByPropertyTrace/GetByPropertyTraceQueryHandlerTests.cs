using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetByPropertyTrace;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.Application.Common.Response.Result;
using TestMillion.UnitTests.Common;

namespace TestMillion.UnitTests.Features.PropertyTrace.Queries.GetByPropertyTrace;

public class GetByPropertyTraceQueryHandlerTests : TestBase
{
    private readonly GetByPropertyTraceQueryHandler _handler;
    private readonly Mock<ILogger<GetByPropertyTraceQueryHandler>> _logger;

    public GetByPropertyTraceQueryHandlerTests()
    {
        _logger = MockLogger<GetByPropertyTraceQueryHandler>();
        _handler = new GetByPropertyTraceQueryHandler(
            _logger.Object,
            MockPropertyTraceRepository.Object,
            MockPropertyRepository.Object,
            MockOwnerRepository.Object,
            MockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenPropertyTraceDoesNotExist()
    {
        // Arrange
        var query = new GetByPropertyTraceQuery("nonExistentId");

        MockPropertyTraceRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.PropertyTrace?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().Be("Property trace not found");
result.StatusCode.Should().Be(ResultType.NotFound);
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnPropertyTrace_WhenExists()
    {
        // Arrange
        var propertyTraceId = "testId";
        var query = new GetByPropertyTraceQuery(propertyTraceId);

        var propertyTrace = new Domain.Entities.PropertyTrace(
            DateOnly.FromDateTime(DateTime.Now), 
            "Test Trace", 
            100000m, 
            10m, 
            "propertyId1");

        var property = new Property(
            "Test Property",
            "123 Test St",
            100000m,
            "TEST-001",
            2024,
            "ownerId1");

        var propertyTraceResponseDto = new PropertyTraceResponseDto
        {
            Id = propertyTraceId,
            DateSale = DateOnly.FromDateTime(DateTime.Now),
            Name = "Test Trace",
            Value = 100000m,
            Tax = 10m,
            IdProperty = "propertyId1",
            Property = new PropertyResponseDto
            {
                Id = "propertyId1",
                Name = "Test Property"
            }
        };

        MockPropertyTraceRepository
            .Setup(x => x.GetByIdAsync(propertyTraceId))
            .ReturnsAsync(propertyTrace);

        MockPropertyRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(property);

        MockMapper
            .Setup(x => x.Map<PropertyTraceResponseDto>(It.IsAny<Domain.Entities.PropertyTrace>()))
            .Returns(propertyTraceResponseDto);

        MockMapper
            .Setup(x => x.Map<PropertyResponseDto>(It.IsAny<Property>()))
            .Returns(propertyTraceResponseDto.Property);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be("Test Trace");
        result.Data.Value.Should().Be(100000m);
        result.Data.Tax.Should().Be(10m);
        result.Data.Property.Should().NotBeNull();
        result.Data.Property.Name.Should().Be("Test Property");
    }

    [Fact]
    public async Task Handle_ShouldHandleException_WhenErrorOccurs()
    {
        // Arrange
        var query = new GetByPropertyTraceQuery("testId");

        MockPropertyTraceRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Contain("Error getting property trace");
        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
}