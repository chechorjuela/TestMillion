using FluentAssertions;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.Cqrs.Commands.DeletePropertyImage;
using TestMillion.UnitTests.Features.PropertyImage.Common;

namespace TestMillion.UnitTests.Features.PropertyImage.Commands.DeletePropertyImage;

public class DeletePropertyImageCommandHandlerTests : PropertyImageTestBase
{
    private readonly DeletePropertyImageCommandHandler _handler;

    public DeletePropertyImageCommandHandlerTests()
    {
        _handler = new DeletePropertyImageCommandHandler(
            MockPropertyImageRepo.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenImageIsDeleted()
    {
        // Arrange
        var command = new DeletePropertyImageCommand { Id = "test-id" };

        var propertyImage = new Domain.Entities.PropertyImage(
            "test-image.jpg",
            true,
            "test-property-id");

        MockPropertyImageRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync(propertyImage);

        MockPropertyImageRepo
            .Setup(x => x.DeleteAsync(command.Id))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenImageDoesNotExist()
    {
        // Arrange
        var command = new DeletePropertyImageCommand { Id = "nonexistent-id" };

        MockPropertyImageRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync((Domain.Entities.PropertyImage)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.NotFound);
        result.Message.Should().Be("Property image not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenDeletionFails()
    {
        // Arrange
        var command = new DeletePropertyImageCommand { Id = "test-id" };

        var propertyImage = new Domain.Entities.PropertyImage(
            "test-image.jpg",
            true,
            "test-property-id");

        MockPropertyImageRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync(propertyImage);

        MockPropertyImageRepo
            .Setup(x => x.DeleteAsync(command.Id))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Be("Failed to delete property image");
    }
}