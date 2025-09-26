using FluentAssertions;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Features.Properties.Cqrs.Queries.GetPropertyDetail;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;
using TestMillion.UnitTests.Features.Properties.Common;
using Moq;

namespace TestMillion.UnitTests.Features.Properties.Queries.GetPropertyDetail;

public class GetPropertyDetailQueryHandlerTests : PropertyTestBase
{
    private readonly Mock<IBaseRepository<Property>> _mockPropertyRepo;
    private readonly Mock<IBaseRepository<Domain.Entities.PropertyImage>> _mockImageRepo;
    private readonly Mock<IBaseRepository<Owner>> _mockOwnerRepo;
    private readonly GetPropertyDetailQueryHandler _handler;

    public GetPropertyDetailQueryHandlerTests()
    {
        _mockPropertyRepo = new Mock<IBaseRepository<Property>>();
        _mockImageRepo = new Mock<IBaseRepository<Domain.Entities.PropertyImage>>();
        _mockOwnerRepo = new Mock<IBaseRepository<Owner>>();

        _handler = new GetPropertyDetailQueryHandler(
            _mockPropertyRepo.Object,
            _mockImageRepo.Object,
            _mockOwnerRepo.Object,
            MockPropertyMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPropertyDetailDto_WhenPropertyExists()
    {
        // Arrange
        var query = new GetPropertyDetailQuery { Id = "test-id" };

        var property = new Property(
            "Test Property",
            "Test Address",
            100000,
            "TEST001",
            2024,
            "owner-id");

        var owner = new Owner(
            "Test Owner",
            "Owner Address",
            DateOnly.FromDateTime(DateTime.Now)) { Photo = "owner.jpg" };

        var images = new List<Domain.Entities.PropertyImage>
        {
            new Domain.Entities.PropertyImage("image1.jpg", true, property.Id),
            new Domain.Entities.PropertyImage("image2.jpg", false, property.Id)
        };

        var ownerDto = new TestMillion.Application.Features.Owners.DTOs.Response.OwnerResponseDto
        {
            Id = "owner-id",
            Name = owner.Name,
            Address = owner.Address,
            Birthdate = owner.Birthdate,
            Photo = owner.Photo
        };

        var imagesDtos = images.Select(i => new TestMillion.Application.Features.PropertyImage.DTOs.Response.PropertyImageResponseDto
        {
            Id = i.Id,
            FileUrl = i.File,
            Enabled = i.Enabled,
            Property = default
        }).ToList();

        var propertyDetailDto = new PropertyDetailDto
        {
            Id = query.Id,
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            CodeInternal = property.CodeInternal,
            Year = property.Year,
            IdOwner = property.IdOwner,
            MainImage = "image1.jpg",
            Owner = ownerDto
        };

        _mockPropertyRepo
            .Setup(x => x.GetByIdAsync(query.Id))
            .ReturnsAsync(property);

        _mockOwnerRepo
            .Setup(x => x.GetByIdAsync(property.IdOwner))
            .ReturnsAsync(owner);

        _mockImageRepo
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(images);

        MockPropertyMapper
            .Setup(x => x.Map<PropertyDetailDto>(property))
            .Returns(propertyDetailDto);

        MockPropertyMapper
            .Setup(x => x.Map<TestMillion.Application.Features.Owners.DTOs.Response.OwnerResponseDto>(owner))
            .Returns(ownerDto);

        MockPropertyMapper
            .Setup(x => x.Map<IEnumerable<TestMillion.Application.Features.PropertyImage.DTOs.Response.PropertyImageResponseDto>>(images))
            .Returns(imagesDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(query.Id);
        result.Name.Should().Be(property.Name);
        result.Address.Should().Be(property.Address);
        result.Price.Should().Be(property.Price);
        result.CodeInternal.Should().Be(property.CodeInternal);
        result.Year.Should().Be(property.Year);
        result.MainImage.Should().Be("image1.jpg");
        result.Owner.Should().NotBeNull();
        result.Owner.Name.Should().Be(owner.Name);
        result.Owner.Address.Should().Be(owner.Address);
        result.Images.Should().NotBeNull();
        result.Images.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenPropertyDoesNotExist()
    {
        // Arrange
        var query = new GetPropertyDetailQuery { Id = "nonexistent-id" };

        _mockPropertyRepo
            .Setup(x => x.GetByIdAsync(query.Id))
            .ReturnsAsync((Property?)null);


        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldReturnPropertyDetailWithoutOwner_WhenOwnerDoesNotExist()
    {
        // Arrange
        var query = new GetPropertyDetailQuery { Id = "test-id" };

        var property = new Property(
            "Test Property",
            "Test Address",
            100000,
            "TEST001",
            2024,
            "owner-id");

        var images = new List<Domain.Entities.PropertyImage>();

        var propertyDetailDto = new PropertyDetailDto
        {
            Id = query.Id,
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            CodeInternal = property.CodeInternal,
            Year = property.Year,
            IdOwner = property.IdOwner
        };

        _mockPropertyRepo
            .Setup(x => x.GetByIdAsync(query.Id))
            .ReturnsAsync(property);

        _mockOwnerRepo
            .Setup(x => x.GetByIdAsync(property.IdOwner))
            .ReturnsAsync((Owner?)null);

        _mockImageRepo
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(images);

        MockPropertyMapper
            .Setup(x => x.Map<PropertyDetailDto>(property))
            .Returns(propertyDetailDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(query.Id);
        result.Owner.Should().BeNull();
        result.Images.Should().NotBeNull();
        result.Images.Should().BeEmpty();
    }
}