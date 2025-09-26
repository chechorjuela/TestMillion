using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Features.PropertyTrace.Commands.UpdatePropertyTrace;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.Application.Common.Response.Result;
using TestMillion.UnitTests.Common;

namespace TestMillion.UnitTests.Features.PropertyTrace.Commands.UpdatePropertyTrace;

public class UpdatePropertyTraceCommandHandlerTests : TestBase
{
    private readonly UpdatePropertyTraceCommandHandler _handler;
    private readonly Mock<ILogger<UpdatePropertyTraceCommandHandler>> _logger;

    public UpdatePropertyTraceCommandHandlerTests()
    {
        _logger = MockLogger<UpdatePropertyTraceCommandHandler>();
        _handler = new UpdatePropertyTraceCommandHandler(
            _logger.Object,
            MockPropertyTraceRepository.Object,
            MockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPropertyTraceDoesNotExist()
    {
        // Arrange
        var command = new UpdatePropertyTraceCommand
        {
            Id = "nonExistentId",
            DateSale = DateOnly.FromDateTime(DateTime.Now),
            Name = "Updated Test Trace",
            Value = 200000m,
            Tax = 20m
        };

        MockPropertyTraceRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.PropertyTrace?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Message.Should().Be("Property trace not found");
result.StatusCode.Should().Be(ResultType.NotFound);
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldUpdatePropertyTrace_WhenExists()
    {
        // Arrange
        var propertyTraceId = "testId";
        var command = new UpdatePropertyTraceCommand
        {
            Id = propertyTraceId,
            DateSale = DateOnly.FromDateTime(DateTime.Now),
            Name = "Updated Test Trace",
            Value = 200000m,
            Tax = 20m
        };

        var existingPropertyTrace = new Domain.Entities.PropertyTrace(
            DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
            "Original Test Trace",
            100000m,
            10m,
            "propertyId1");

        var updatedPropertyTrace = new Domain.Entities.PropertyTrace(
            command.DateSale,
            command.Name,
            command.Value,
            command.Tax,
            "propertyId1");

        var property = new Property(
            "Test Property",
            "123 Test St",
            200000m,
            "TEST-001",
            2024,
            "ownerId1");

        var propertyTraceResponseDto = new PropertyTraceResponseDto
        {
            Id = propertyTraceId,
            DateSale = command.DateSale,
            Name = command.Name,
            Value = command.Value,
            Tax = command.Tax,
            IdProperty = "propertyId1",
            Property = new PropertyResponseDto
            {
                Id = property.Id,
                Name = property.Name
            }
        };

        MockPropertyTraceRepository
            .Setup(x => x.GetByIdAsync(propertyTraceId))
            .ReturnsAsync(existingPropertyTrace);

        MockPropertyTraceRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Domain.Entities.PropertyTrace>()))
            .ReturnsAsync(updatedPropertyTrace);

        MockPropertyRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(property);

        MockMapper
            .Setup(x => x.Map<Domain.Entities.PropertyTrace>(command))
            .Returns(updatedPropertyTrace);

        MockMapper
            .Setup(x => x.Map<PropertyTraceResponseDto>(It.IsAny<Domain.Entities.PropertyTrace>()))
            .Returns(propertyTraceResponseDto);

        MockMapper
            .Setup(x => x.Map<PropertyResponseDto>(It.IsAny<Property>()))
            .Returns(propertyTraceResponseDto.Property);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be(command.Name);
        result.Data.Value.Should().Be(command.Value);
        result.Data.Tax.Should().Be(command.Tax);
        result.Data.Property.Should().NotBeNull();
        result.Data.Property.Name.Should().Be("Test Property");
    }

    [Fact]
    public async Task Handle_ShouldHandleException_WhenErrorOccurs()
    {
        // Arrange
        var command = new UpdatePropertyTraceCommand
        {
            Id = "testId",
            DateSale = DateOnly.FromDateTime(DateTime.Now),
            Name = "Updated Test Trace",
            Value = 200000m,
            Tax = 20m
        };

        MockPropertyTraceRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Contain("Error updating property trace");
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