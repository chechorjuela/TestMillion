using FluentAssertions;
using TestMillion.Application.Common.Models;
using TestMillion.Application.Features.Owners.Cqrs.Queries.GetAllOwner;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Domain.Common.Models;
using TestMillion.Domain.Entities;
using TestMillion.UnitTests.Features.Owners.Common;

namespace TestMillion.UnitTests.Features.Owners.Queries.GetAllOwner;

public class GetAllOwnerQueryHandlerTests : OwnerTestBase
{
    private readonly GetAllOwnerCommandHandler _handler;

    public GetAllOwnerQueryHandlerTests()
    {
        _handler = new GetAllOwnerCommandHandler(
            MockOwnerMapper.Object,
            MockOwnerRepo.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedResponse_WithOwners()
    {
        // Arrange
        var query = new GetAllOwnerQuery
        {
            Pagination = new PaginationRequestDto { PageNumber = 1, PageSize = 10 },
            Filter = new FilterRequestDto()
        };

        var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 10 };
        var filterModel = new FilterModel();

        var owners = new List<Owner>
        {
            new Owner("Owner 1", "Address 1", DateOnly.FromDateTime(DateTime.Now)) { Photo = "photo1.jpg" },
            new Owner("Owner 2", "Address 2", DateOnly.FromDateTime(DateTime.Now)) { Photo = "photo2.jpg" }
        };

        var ownerResponses = owners.Select(o => new OwnerResponseDto
        {
            Id = "test-id",
            Name = o.Name,
            Address = o.Address,
            Birthdate = o.Birthdate,
            Photo = o.Photo
        }).ToList();

        MockOwnerMapper
            .Setup(x => x.Map<PaginationModel>(query.Pagination))
            .Returns(paginationModel);

        MockOwnerMapper
            .Setup(x => x.Map<FilterModel>(query.Filter))
            .Returns(filterModel);

        MockOwnerRepo
            .Setup(x => x.GetPagedAsync(It.IsAny<PaginationModel>(), It.IsAny<FilterModel>()))
            .ReturnsAsync(PaginatedResponse<Owner>.Create(owners, owners.Count, 1, 10));

        MockOwnerMapper
            .Setup(x => x.Map<List<OwnerResponseDto>>(It.IsAny<List<Owner>>()))
            .Returns(ownerResponses);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(200);
        result.Message.Should().Be("Owners fetched successfully");
        result.Data.Should().NotBeNull();
        result.Data.Count.Should().Be(2);
        result.Metadata.Should().NotBeNull();
        result.Metadata.TotalCount.Should().Be(2);
        result.Metadata.PageSize.Should().Be(10);
        result.Metadata.CurrentPage.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoOwnersExist()
    {
        // Arrange
        var query = new GetAllOwnerQuery
        {
            Pagination = new PaginationRequestDto { PageNumber = 1, PageSize = 10 },
            Filter = new FilterRequestDto()
        };

        var paginationModel = new PaginationModel { PageNumber = 1, PageSize = 10 };
        var filterModel = new FilterModel();

        MockOwnerMapper
            .Setup(x => x.Map<PaginationModel>(query.Pagination))
            .Returns(paginationModel);

        MockOwnerMapper
            .Setup(x => x.Map<FilterModel>(query.Filter))
            .Returns(filterModel);

        var emptyList = new List<Owner>();

        MockOwnerRepo
            .Setup(x => x.GetPagedAsync(It.IsAny<PaginationModel>(), It.IsAny<FilterModel>()))
            .ReturnsAsync(PaginatedResponse<Owner>.Create(emptyList, 0, 1, 10));

        MockOwnerMapper
            .Setup(x => x.Map<List<OwnerResponseDto>>(It.IsAny<List<Owner>>()))
            .Returns(new List<OwnerResponseDto>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(200);
        result.Message.Should().Be("Owners fetched successfully");
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEmpty();
        result.Metadata.Should().NotBeNull();
        result.Metadata.TotalCount.Should().Be(0);
        result.Metadata.PageSize.Should().Be(10);
        result.Metadata.CurrentPage.Should().Be(1);
    }
}