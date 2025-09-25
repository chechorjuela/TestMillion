using FluentAssertions;
using Moq;
using TestMillion.Application.Common.Models;
using TestMillion.Application.Features.Properties.Cqrs.Queries.GetProperties;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Domain.Common.Models;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;
using TestMillion.UnitTests.Features.Properties.Common;

namespace TestMillion.UnitTests.Features.Properties.Queries.GetProperties;

public class GetPropertyAllQueryHandlerTests : PropertyTestBase
{
    private readonly Mock<IBaseRepository<Domain.Entities.PropertyImage>> _mockImageRepo;
    private readonly GetPropertyAllQueryHandler _handler;

    public GetPropertyAllQueryHandlerTests()
    {
        _mockImageRepo = new Mock<IBaseRepository<Domain.Entities.PropertyImage>>();
        _handler = new GetPropertyAllQueryHandler(
            MockPropertyRepo.Object,
            _mockImageRepo.Object,
            MockOwnerRepo.Object,
            MockPropertyMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedResponse_WithProperties()
    {
        // Arrange
        var query = new GetPropertyAllQuery
        {
            Pagination = new PaginationRequestDto { PageNumber = 1, PageSize = 10 },
            Filter = new PropertyFilterDto()
        };

        var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 10 };
        var filterModel = new FilterModel();

        var properties = new List<Property>
        {
            new Property("Property 1", "Address 1", 100000, "CODE1", 2024, "owner-id-1"),
            new Property("Property 2", "Address 2", 200000, "CODE2", 2024, "owner-id-2")
        };

        var propertyResponses = properties.Select(p => new PropertyResponseDto
        {
            Id = "test-id",
            Name = p.Name,
            Address = p.Address,
            Price = p.Price,
            CodeInternal = p.CodeInternal,
            Year = p.Year,
            IdOwner = p.IdOwner
        }).ToList();

        var images = new List<Domain.Entities.PropertyImage>
        {
            new Domain.Entities.PropertyImage("image1.jpg", true, "property-id-1"),
            new Domain.Entities.PropertyImage("image2.jpg", false, "property-id-2")
        };

        var owner = new Owner("Test Owner", "Test Address", DateOnly.FromDateTime(DateTime.Now)) { Photo = "test.jpg" };
        var ownerResponse = new OwnerResponseDto
        {
            Id = "test-owner-id",
            Name = owner.Name,
            Address = owner.Address,
            Birthdate = owner.Birthdate,
            Photo = owner.Photo
        };

        MockPropertyMapper
            .Setup(x => x.Map<PaginationModel>(query.Pagination))
            .Returns(paginationModel);

        MockPropertyMapper
            .Setup(x => x.Map<FilterModel>(query.Filter))
            .Returns(filterModel);

        var expectedResult = (Items: (IEnumerable<Property>)properties, TotalCount: properties.Count);

        MockPropertyRepo
            .Setup(x => x.GetPagedAsync(It.IsAny<PaginationModel>(), It.IsAny<FilterModel>()))
            .ReturnsAsync(expectedResult);

        _mockImageRepo
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(images);

        _mockImageRepo
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(images);

        MockOwnerRepo
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(owner);

        MockPropertyMapper
            .Setup(x => x.Map<List<PropertyResponseDto>>(It.IsAny<List<Property>>()))
            .Returns(propertyResponses);

        MockPropertyMapper
            .Setup(x => x.Map<OwnerResponseDto>(owner))
            .Returns(ownerResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(200);
        result.Message.Should().Be("Properties fetched successfully");
        result.Data.Should().NotBeNull();
        result.Data.Count.Should().Be(2);
        result.Metadata.Should().NotBeNull();
        result.Metadata.TotalCount.Should().Be(2);
        result.Metadata.PageSize.Should().Be(10);
        result.Metadata.CurrentPage.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoPropertiesExist()
    {
        // Arrange
        var query = new GetPropertyAllQuery
        {
            Pagination = new PaginationRequestDto { PageNumber = 1, PageSize = 10 },
            Filter = new PropertyFilterDto()
        };

        var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 10 };
        var filterModel = new FilterModel();

        MockPropertyMapper
            .Setup(x => x.Map<PaginationModel>(query.Pagination))
            .Returns(paginationModel);

        MockPropertyMapper
            .Setup(x => x.Map<FilterModel>(query.Filter))
            .Returns(filterModel);

        MockPropertyMapper
            .Setup(x => x.Map<List<PropertyResponseDto>>(It.IsAny<List<Property>>()))
            .Returns(new List<PropertyResponseDto>());

        var emptyResult = (Items: (IEnumerable<Property>)new List<Property>(), TotalCount: 0);

        MockPropertyRepo
            .Setup(x => x.GetPagedAsync(It.IsAny<PaginationModel>(), It.IsAny<FilterModel>()))
            .ReturnsAsync(emptyResult);

        _mockImageRepo
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Domain.Entities.PropertyImage>());

        var metadata = new PaginationMetadataDto(0, 10, 1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(200);
        result.Message.Should().Be("Properties fetched successfully");
        result.Data.Should().NotBeNull();
        result.Data.Count.Should().Be(0);
        result.Metadata.Should().NotBeNull();
        result.Metadata.TotalCount.Should().Be(0);
        result.Metadata.PageSize.Should().Be(10);
        result.Metadata.CurrentPage.Should().Be(1);
    }
}