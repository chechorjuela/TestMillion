using FluentAssertions;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.Cqrs.Queries.GetByIdPropertyImage;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.UnitTests.Features.PropertyImage.Common;

namespace TestMillion.UnitTests.Features.PropertyImage.Queries.GetByIdPropertyImage;

public class GetByPropertyImageQueryHandlerTests : PropertyImageTestBase
{
    private readonly GetByPropertyImageQueryHandler _handler;

    public GetByPropertyImageQueryHandlerTests()
    {
        _handler = new GetByPropertyImageQueryHandler(
            MockPropertyImageRepo.Object,
            MockPropertyRepo.Object,
            MockOwnerRepo.Object,
            MockPropertyImageMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenImageExists()
    {
        // Arrange
        var query = new GetByPropertyImageQuery("test-id");

        var propertyImage = new Domain.Entities.PropertyImage(
            "test-image.jpg",
            true,
            "property-id");

        var property = new Property(
            "Test Property",
            "Test Address",
            100000,
            "TEST001",
            2024,
            "owner-id");

        var propertyResponse = new PropertyResponseDto
        {
            Id = "property-id",
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            CodeInternal = property.CodeInternal,
            Year = property.Year,
            IdOwner = property.IdOwner
        };

        var propertyImageResponse = new PropertyImageResponseDto
        {
            Id = query.Id,
            FileUrl = propertyImage.File,
            Enabled = propertyImage.Enabled,
            Property = propertyResponse
        };

        MockPropertyImageRepo
            .Setup(x => x.GetByIdAsync(query.Id))
            .ReturnsAsync(propertyImage);

        MockPropertyRepo
            .Setup(x => x.GetByIdAsync(propertyImage.IdProperty))
            .ReturnsAsync(property);

        MockPropertyImageMapper
            .Setup(x => x.Map<PropertyImageResponseDto>(propertyImage))
            .Returns(propertyImageResponse);

        MockPropertyImageMapper
            .Setup(x => x.Map<PropertyResponseDto>(property))
            .Returns(propertyResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(query.Id);
        result.Data.FileUrl.Should().Be(propertyImage.File);
        result.Data.Enabled.Should().Be(propertyImage.Enabled);
        result.Data.Property.Should().NotBeNull();
        result.Data.Property.Name.Should().Be(property.Name);
        result.Data.Property.Address.Should().Be(property.Address);
        result.Data.Property.Price.Should().Be(property.Price);
        result.Data.Property.CodeInternal.Should().Be(property.CodeInternal);
        result.Data.Property.Year.Should().Be(property.Year);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenImageDoesNotExist()
    {
        // Arrange
        var query = new GetByPropertyImageQuery("nonexistent-id");

        MockPropertyImageRepo
            .Setup(x => x.GetByIdAsync(query.Id))
            .ReturnsAsync((Domain.Entities.PropertyImage)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.NotFound);
        result.Message.Should().Be("Property image not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessWithoutProperty_WhenPropertyDoesNotExist()
    {
        // Arrange
        var query = new GetByPropertyImageQuery("test-id");

        var propertyImage = new Domain.Entities.PropertyImage(
            "test-image.jpg",
            true,
            "property-id");

        var propertyImageResponse = new PropertyImageResponseDto
        {
            Id = query.Id,
            FileUrl = propertyImage.File,
            Enabled = propertyImage.Enabled
        };

        MockPropertyImageRepo
            .Setup(x => x.GetByIdAsync(query.Id))
            .ReturnsAsync(propertyImage);

        MockPropertyRepo
            .Setup(x => x.GetByIdAsync(propertyImage.IdProperty))
            .ReturnsAsync((Property)null);

        MockPropertyImageMapper
            .Setup(x => x.Map<PropertyImageResponseDto>(propertyImage))
            .Returns(propertyImageResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(query.Id);
        result.Data.FileUrl.Should().Be(propertyImage.File);
        result.Data.Enabled.Should().Be(propertyImage.Enabled);
        result.Data.Property.Should().BeNull();
    }
}