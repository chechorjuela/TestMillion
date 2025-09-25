using FluentAssertions;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.Cqrs.Queries.GetOwnerById;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.UnitTests.Features.Owners.Common;

namespace TestMillion.UnitTests.Features.Owners.Queries.GetOwnerById;

public class GetByIdOwnerQueryHandlerTests : OwnerTestBase
{
    private readonly GetByIdOwnerQueryHandler _handler;

    public GetByIdOwnerQueryHandlerTests()
    {
        _handler = new GetByIdOwnerQueryHandler(
            MockOwnerMapper.Object,
            MockOwnerRepo.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOwnerExists()
    {
        // Arrange
        var query = new GetByIdOwnerQuery("test-id");

        var owner = new Owner(
            "Test Owner", 
            "Test Address",
            DateOnly.FromDateTime(DateTime.Now)
        ) { Photo = "test.jpg" };

        var ownerResponse = new OwnerResponseDto
        {
            Id = "test-id",
            Name = owner.Name,
            Address = owner.Address,
            Birthdate = owner.Birthdate,
            Photo = owner.Photo
        };

        MockOwnerRepo
            .Setup(x => x.GetByIdAsync(query.OwnerId))
            .ReturnsAsync(owner);

        MockOwnerMapper
            .Setup(x => x.Map<OwnerResponseDto>(owner))
            .Returns(ownerResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(ownerResponse.Id);
        result.Data.Name.Should().Be(owner.Name);
        result.Data.Address.Should().Be(owner.Address);
        result.Data.Birthdate.Should().Be(owner.Birthdate);
        result.Data.Photo.Should().Be(owner.Photo);
    }
}