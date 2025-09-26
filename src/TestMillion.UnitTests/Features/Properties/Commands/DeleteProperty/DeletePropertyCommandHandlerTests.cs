using AutoMapper;
using FluentAssertions;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.Cqrs.Commands.DeleteProperty;
using TestMillion.Domain.Entities;
using TestMillion.UnitTests.Features.Properties.Common;

namespace TestMillion.UnitTests.Features.Properties.Commands.DeleteProperty;

public class DeletePropertyCommandHandlerTests : PropertyTestBase
{
    private readonly DeletePropertyCommandHandler _handler;

    public DeletePropertyCommandHandlerTests()
    {
        _handler = new DeletePropertyCommandHandler(
            Mock.Of<IMapper>(),
            MockPropertyRepo.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenPropertyIsDeleted()
    {
        // Arrange
        var command = new DeletePropertyCommand("test-id");

        var existingProperty = new Property(
            "Test Property",
            "Test Address",
            100000,
            "TEST001",
            2024,
            "test-owner-id");

        MockPropertyRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync(existingProperty);

        MockPropertyRepo
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
    public async Task Handle_ShouldReturnNotFound_WhenPropertyDoesNotExist()
    {
        // Arrange
        var command = new DeletePropertyCommand("nonexistent-id");

        MockPropertyRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync((Property?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.NotFound);
        result.Message.Should().Be($"Property with ID {command.Id} not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenDeletionFails()
    {
        // Arrange
        var command = new DeletePropertyCommand("test-id");

        var existingProperty = new Property(
            "Test Property",
            "Test Address",
            100000,
            "TEST001",
            2024,
            "test-owner-id");

        MockPropertyRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync(existingProperty);

        MockPropertyRepo
            .Setup(x => x.DeleteAsync(command.Id))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Be("Failed to delete Property.");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenExceptionOccurs()
    {
        // Arrange
        var command = new DeletePropertyCommand("test-id");

        MockPropertyRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Contain("An error occurred while deleting the Property");
        result.Message.Should().Contain("Test exception");
    }
}