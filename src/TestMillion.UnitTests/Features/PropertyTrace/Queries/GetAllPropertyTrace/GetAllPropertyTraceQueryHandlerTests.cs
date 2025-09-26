using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestMillion.Application.Common.Models;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetAllPropertyTrace;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;
using TestMillion.Domain.Common.Models;
using TestMillion.Domain.Entities;
using TestMillion.UnitTests.Common;

namespace TestMillion.UnitTests.Features.PropertyTrace.Queries.GetAllPropertyTrace;

public class GetAllPropertyTraceQueryHandlerTests : TestBase
{
    private readonly GetAllPropertyTraceQueryHandler _handler;
    private readonly Mock<ILogger<GetAllPropertyTraceQueryHandler>> _logger;

    public GetAllPropertyTraceQueryHandlerTests()
    {
        _logger = MockLogger<GetAllPropertyTraceQueryHandler>();
        _handler = new GetAllPropertyTraceQueryHandler(
            _logger.Object,
            MockPropertyTraceRepository.Object,
            MockPropertyRepository.Object,
            MockOwnerRepository.Object,
            MockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoPropertiesExist()
    {
        // Arrange
        var query = new GetAllPropertyTraceQuery
        {
            Pagination = new PaginationRequestDto { PageNumber = 1, PageSize = 10 },
            Filter = new FilterRequestDto()
        };

        var emptyList = new List<Domain.Entities.PropertyTrace>();
        MockPropertyTraceRepository
            .Setup(x => x.GetPagedAsync(It.IsAny<PaginationModel>(), It.IsAny<FilterModel>()))
            .ReturnsAsync(PaginatedResponse<Domain.Entities.PropertyTrace>.Create(emptyList, 0, 1, 10));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().BeEmpty();
        result.Message.Should().Be("No property traces found");
result.Status.Should().Be(200);
        result.Metadata.Should().NotBeNull();
        result.Metadata!.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldReturnPropertyTraces_WhenPropertiesExist()
    {
        // Arrange
        var query = new GetAllPropertyTraceQuery
        {
            Pagination = new PaginationRequestDto { PageNumber = 1, PageSize = 10 },
            Filter = new FilterRequestDto()
        };

        var propertyTraces = new List<Domain.Entities.PropertyTrace>
        {
            new(DateOnly.FromDateTime(DateTime.Now), "Test Trace", 100000m, 10m, "propertyId1")
        };

        var property = new Property("Test Property", "123 Test St", 100000m, "TEST-001", 2024, "ownerId1");

        var propertyTraceResponseDto = new PropertyTraceResponseDto
        {
            Id = "1",
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
            .Setup(x => x.GetPagedAsync(It.IsAny<PaginationModel>(), It.IsAny<FilterModel>()))
            .ReturnsAsync(PaginatedResponse<Domain.Entities.PropertyTrace>.Create(propertyTraces, propertyTraces.Count, 1, 10));

        MockPropertyRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(property);

        MockMapper
            .Setup(x => x.Map<List<PropertyTraceResponseDto>>(It.IsAny<List<Domain.Entities.PropertyTrace>>()))
            .Returns(new List<PropertyTraceResponseDto> { propertyTraceResponseDto });

        MockMapper
            .Setup(x => x.Map<PropertyResponseDto>(It.IsAny<Property>()))
            .Returns(propertyTraceResponseDto.Property);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeEmpty();
        result.Data.Should().HaveCount(1);
        result.Status.Should().Be(200);
        result.Message.Should().Be("Property traces fetched successfully");
        result.Metadata.Should().NotBeNull();
        result.Metadata!.TotalCount.Should().Be(1);

        var firstItem = result.Data.First();
        firstItem.Name.Should().Be("Test Trace");
        firstItem.Value.Should().Be(100000m);
        firstItem.Tax.Should().Be(10m);
        firstItem.Property.Should().NotBeNull();
        firstItem.Property.Name.Should().Be("Test Property");
    }

    [Fact]
    public async Task Handle_ShouldHandleException_WhenErrorOccurs()
    {
        // Arrange
        var query = new GetAllPropertyTraceQuery
        {
            Pagination = new PaginationRequestDto { PageNumber = 1, PageSize = 10 },
            Filter = new FilterRequestDto()
        };

        MockPropertyTraceRepository
            .Setup(x => x.GetPagedAsync(It.IsAny<PaginationModel>(), It.IsAny<FilterModel>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(400);
        result.Message.Should().Contain("Error getting all property traces");
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