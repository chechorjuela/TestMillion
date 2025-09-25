using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.Commands.DeletePropertyTrace;
using TestMillion.Application.Common.Response.Result;
using TestMillion.UnitTests.Common;

namespace TestMillion.UnitTests.Features.PropertyTrace.Commands.DeletePropertyTrace;

public class DeletePropertyTraceCommandHandlerTests : TestBase
{
    private readonly DeletePropertyTraceCommandHandler _handler;
    private readonly Mock<ILogger<DeletePropertyTraceCommandHandler>> _logger;

    public DeletePropertyTraceCommandHandlerTests()
    {
        _logger = MockLogger<DeletePropertyTraceCommandHandler>();
        _handler = new DeletePropertyTraceCommandHandler(
            _logger.Object,
            MockPropertyTraceRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPropertyTraceDoesNotExist()
    {
        // Arrange
        var command = new DeletePropertyTraceCommand { Id = "nonExistentId" };

        MockPropertyTraceRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.PropertyTrace)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().Be("Property trace not found");
result.StatusCode.Should().Be(ResultType.NotFound);
        result.Data.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldDeletePropertyTrace_WhenExists()
    {
        // Arrange
        var propertyTraceId = "testId";
        var command = new DeletePropertyTraceCommand { Id = propertyTraceId };

        var existingPropertyTrace = new Domain.Entities.PropertyTrace(
            DateOnly.FromDateTime(DateTime.Now),
            "Test Trace",
            100000m,
            10m,
            "propertyId1");

        MockPropertyTraceRepository
            .Setup(x => x.GetByIdAsync(propertyTraceId))
            .ReturnsAsync(existingPropertyTrace);

        MockPropertyTraceRepository
            .Setup(x => x.DeleteAsync(propertyTraceId))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().BeTrue();
        result.Message.Should().Be("Property trace deleted successfully");
    }

    [Fact]
    public async Task Handle_ShouldHandleException_WhenErrorOccurs()
    {
        // Arrange
        var command = new DeletePropertyTraceCommand { Id = "testId" };

        MockPropertyTraceRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Contain("Error deleting property trace");
        result.Data.Should().BeFalse();
        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }
}