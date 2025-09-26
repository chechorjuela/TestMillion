using FluentAssertions;
using TestMillion.Application.Common.Models;
using TestMillion.Application.Features.PropertyImage.Cqrs.Queries.GetAllPropertyImage;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;
using TestMillion.Domain.Common.Models;
using TestMillion.Domain.Entities;
using TestMillion.UnitTests.Features.PropertyImage.Common;

namespace TestMillion.UnitTests.Features.PropertyImage.Queries.GetAllPropertyImage;

public class GetAllPropertyImageQueryHandlerTests : PropertyImageTestBase
{
    private readonly GetAllPropertyImageQueryHandler _handler;

    public GetAllPropertyImageQueryHandlerTests()
    {
        _handler = new GetAllPropertyImageQueryHandler(
            MockPropertyImageRepo.Object,
            MockPropertyRepo.Object,
            MockOwnerRepo.Object,
            MockPropertyImageMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedResponse_WithPropertyImages()
    {
        // Arrange
        var query = new GetAllPropertyImageQuery
        {
            Pagination = new PaginationRequestDto { PageNumber = 1, PageSize = 10 },
            Filter = new FilterRequestDto()
        };

        var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 10 };
        var filterModel = new FilterModel();

        var propertyImages = new List<Domain.Entities.PropertyImage>
        {
            new Domain.Entities.PropertyImage("image1.jpg", true, "property-id-1"),
            new Domain.Entities.PropertyImage("image2.jpg", false, "property-id-2")
        };

        var property = new Property(
            "Test Property",
            "Test Address",
            100000,
            "TEST001",
            2024,
            "owner-id");

        var propertyResponse = new PropertyResponseDto
        {
            Id = "property-id-1",
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            CodeInternal = property.CodeInternal,
            Year = property.Year,
            IdOwner = property.IdOwner
        };

        var propertyImageResponses = propertyImages.Select(pi => new PropertyImageResponseDto
        {
            Id = "test-id",
            FileUrl = pi.File,
            Enabled = pi.Enabled,
            Property = propertyResponse
        }).ToList();

        MockPropertyImageMapper
            .Setup(x => x.Map<PaginationModel>(query.Pagination))
            .Returns(paginationModel);

        MockPropertyImageMapper
            .Setup(x => x.Map<FilterModel>(query.Filter))
            .Returns(filterModel);

        MockPropertyImageRepo
            .Setup(x => x.GetPagedAsync(paginationModel, filterModel))
            .ReturnsAsync(PaginatedResponse<Domain.Entities.PropertyImage>.Create(propertyImages, propertyImages.Count, 1, 10));

        MockPropertyRepo
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(property);

        MockPropertyImageMapper
            .Setup(x => x.Map<List<PropertyImageResponseDto>>(propertyImages))
            .Returns(propertyImageResponses);

        MockPropertyImageMapper
            .Setup(x => x.Map<PropertyResponseDto>(property))
            .Returns(propertyResponse);

        var metadata = new PaginationMetadataDto(2, 10, 1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(200);
        result.Message.Should().Be("Property images fetched successfully");
        result.Data.Should().NotBeNull();
        result.Data.Count.Should().Be(2);
        result.Data.Should().AllSatisfy(image =>
        {
            image.Property.Should().NotBeNull();
            image.Property.Name.Should().Be(property.Name);
            image.Property.Address.Should().Be(property.Address);
        });
        result.Metadata.Should().NotBeNull();
        result.Metadata.TotalCount.Should().Be(2);
        result.Metadata.PageSize.Should().Be(10);
        result.Metadata.CurrentPage.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoImagesExist()
    {
        // Arrange
        var query = new GetAllPropertyImageQuery
        {
            Pagination = new PaginationRequestDto { PageNumber = 1, PageSize = 10 },
            Filter = new FilterRequestDto()
        };

        var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 10 };
        var filterModel = new FilterModel();

        MockPropertyImageMapper
            .Setup(x => x.Map<PaginationModel>(query.Pagination))
            .Returns(paginationModel);

        MockPropertyImageMapper
            .Setup(x => x.Map<FilterModel>(query.Filter))
            .Returns(filterModel);

        MockPropertyImageRepo
            .Setup(x => x.GetPagedAsync(paginationModel, filterModel))
            .ReturnsAsync(PaginatedResponse<Domain.Entities.PropertyImage>.Create(new List<Domain.Entities.PropertyImage>(), 0, 1, 10));

        MockPropertyImageMapper
            .Setup(x => x.Map<List<PropertyImageResponseDto>>(It.IsAny<List<Domain.Entities.PropertyImage>>()))
            .Returns(new List<PropertyImageResponseDto>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(200);
        result.Message.Should().Be("Property images fetched successfully");
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEmpty();
        result.Metadata.Should().NotBeNull();
        result.Metadata.TotalCount.Should().Be(0);
        result.Metadata.PageSize.Should().Be(10);
        result.Metadata.CurrentPage.Should().Be(1);
    }
}