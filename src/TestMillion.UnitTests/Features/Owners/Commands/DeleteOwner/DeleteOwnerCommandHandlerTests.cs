using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.Cqrs.Commands.DeleteOwner;
using TestMillion.Domain.Entities;
using TestMillion.UnitTests.Features.Owners.Common;

namespace TestMillion.UnitTests.Features.Owners.Commands.DeleteOwner;

public class DeleteOwnerCommandHandlerTests : OwnerTestBase
{
    private readonly DeleteOwnerCommandHandler _handler;
    private readonly Mock<ILogger<DeleteOwnerCommandHandler>> _logger;

    public DeleteOwnerCommandHandlerTests()
    {
        _logger = MockLogger<DeleteOwnerCommandHandler>();
        _handler = new DeleteOwnerCommandHandler(
            MockOwnerRepo.Object,
            _logger.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenOwnerDoesNotExist()
    {
        // Arrange
        var command = new DeleteOwnerCommand { Id = "nonexistent" };

        MockOwnerRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync((Owner?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.NotFound);
        result.Message.Should().Be($"Owner with ID {command.Id} not found.");
        
        _logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(command.Id)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenDeleteFails()
    {
        // Arrange
        var command = new DeleteOwnerCommand { Id = "test-id" };
        var owner = new Owner("Test Owner", "Test Address", DateOnly.FromDateTime(DateTime.Now)) { Photo = "test.jpg" };

        MockOwnerRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync(owner);

        MockOwnerRepo
            .Setup(x => x.DeleteAsync(command.Id))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Be("Failed to delete owner.");
        
        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(command.Id)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOwnerIsDeleted()
    {
        // Arrange
        var command = new DeleteOwnerCommand { Id = "test-id" };
        var owner = new Owner("Test Owner", "Test Address", DateOnly.FromDateTime(DateTime.Now)) { Photo = "test.jpg" };

        MockOwnerRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync(owner);

        MockOwnerRepo
            .Setup(x => x.DeleteAsync(command.Id))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().BeTrue();

        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Successfully deleted")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenExceptionOccurs()
    {
        // Arrange
        var command = new DeleteOwnerCommand { Id = "test-id" };

        MockOwnerRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Contain("An error occurred while deleting the owner");
        result.Message.Should().Contain("Test exception");

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