using FluentAssertions;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.Cqrs.Queries.GetByIdProperty;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.UnitTests.Features.Properties.Common;

namespace TestMillion.UnitTests.Features.Properties.Queries.GetByIdProperty;

public class GetByIdPropertyQueryHandlerTests : PropertyTestBase
{
    private readonly GetByIdPropertyQueryHandler _handler;

    public GetByIdPropertyQueryHandlerTests()
    {
        _handler = new GetByIdPropertyQueryHandler(
            MockPropertyRepo.Object,
            MockOwnerRepo.Object,
            MockPropertyMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenPropertyExists()
    {
        // Arrange
        var query = new GetByIdPropertyQuery("test-id");

        var property = new Property(
            "Test Property",
            "Test Address",
            100000,
            "TEST001",
            2024,
            "test-owner-id");

        var expectedResponse = new PropertyResponseDto
        {
            Id = query.Id,
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            CodeInternal = property.CodeInternal,
            Year = property.Year,
            IdOwner = property.IdOwner
        };

        MockPropertyRepo
            .Setup(x => x.GetByIdAsync(query.Id))
            .ReturnsAsync(property);

        MockPropertyMapper
            .Setup(x => x.Map<PropertyResponseDto>(property))
            .Returns(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(query.Id);
        result.Data.Name.Should().Be(property.Name);
        result.Data.Address.Should().Be(property.Address);
        result.Data.Price.Should().Be(property.Price);
        result.Data.CodeInternal.Should().Be(property.CodeInternal);
        result.Data.Year.Should().Be(property.Year);
        result.Data.IdOwner.Should().Be(property.IdOwner);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenPropertyDoesNotExist()
    {
        // Arrange
        var query = new GetByIdPropertyQuery("nonexistent-id");

        MockPropertyRepo
            .Setup(x => x.GetByIdAsync(query.Id))
            .ReturnsAsync((Property)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.NotFound);
        result.Message.Should().Be("Property not found");
    }
}