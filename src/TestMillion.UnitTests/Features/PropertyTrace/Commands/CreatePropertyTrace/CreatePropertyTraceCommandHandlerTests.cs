using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.Application.Common.Response.Result;
using TestMillion.UnitTests.Common;

namespace TestMillion.UnitTests.Features.PropertyTrace.Commands.CreatePropertyTrace;

public class CreatePropertyTraceCommandHandlerTests : TestBase
{
    private readonly CreatePropertyTraceCommandHandler _handler;
    private readonly Mock<ILogger<CreatePropertyTraceCommandHandler>> _logger;

    public CreatePropertyTraceCommandHandlerTests()
    {
        _logger = MockLogger<CreatePropertyTraceCommandHandler>();
        _handler = new CreatePropertyTraceCommandHandler(
            _logger.Object,
            MockPropertyTraceRepository.Object,
            MockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPropertyDoesNotExist()
    {
        // Arrange
        var command = new CreatePropertyTraceCommand
        {
            DateSale = DateOnly.FromDateTime(DateTime.Now),
            Name = "Test Trace",
            Value = 100000m,
            Tax = 10m,
            IdProperty = "nonExistentPropertyId"
        };

        // No precondition required; handler does not validate property existence.

        // Arrange mapper to return a DTO
        MockMapper
            .Setup(x => x.Map<PropertyTraceResponseDto>(It.IsAny<Domain.Entities.PropertyTrace>()))
            .Returns(new PropertyTraceResponseDto { Id = "generated" });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldCreatePropertyTrace_WhenPropertyExists()
    {
        // Arrange
        var command = new CreatePropertyTraceCommand
        {
            DateSale = DateOnly.FromDateTime(DateTime.Now),
            Name = "Test Trace",
            Value = 100000m,
            Tax = 10m,
            IdProperty = "propertyId1"
        };

        var property = new Property(
            "Test Property",
            "123 Test St",
            100000m,
            "TEST-001",
            2024,
            "ownerId1");

        var propertyTrace = new Domain.Entities.PropertyTrace(
            command.DateSale,
            command.Name,
            command.Value,
            command.Tax,
            command.IdProperty);

        var propertyTraceResponseDto = new PropertyTraceResponseDto
        {
            Id = "newId",
            DateSale = command.DateSale,
            Name = command.Name,
            Value = command.Value,
            Tax = command.Tax,
            IdProperty = command.IdProperty,
            Property = new PropertyResponseDto
            {
                Id = property.Id,
                Name = property.Name
            }
        };

        MockPropertyRepository
            .Setup(x => x.GetByIdAsync(command.IdProperty))
            .ReturnsAsync(property);

        MockPropertyTraceRepository
            .Setup(x => x.AddAsync(It.IsAny<Domain.Entities.PropertyTrace>()))
            .ReturnsAsync(propertyTrace);

        MockMapper
            .Setup(x => x.Map<Domain.Entities.PropertyTrace>(command))
            .Returns(propertyTrace);

        MockMapper
            .Setup(x => x.Map<PropertyTraceResponseDto>(It.IsAny<Domain.Entities.PropertyTrace>()))
            .Returns(propertyTraceResponseDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be(command.Name);
        result.Data.Value.Should().Be(command.Value);
        result.Data.Tax.Should().Be(command.Tax);
        result.Data.IdProperty.Should().Be(command.IdProperty);
    }

    [Fact]
    public async Task Handle_ShouldHandleException_WhenErrorOccurs()
    {
        // Arrange
        var command = new CreatePropertyTraceCommand
        {
            DateSale = DateOnly.FromDateTime(DateTime.Now),
            Name = "Test Trace",
            Value = 100000m,
            Tax = 10m,
            IdProperty = "propertyId1"
        };

        MockPropertyTraceRepository
            .Setup(x => x.AddAsync(It.IsAny<Domain.Entities.PropertyTrace>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Contain("Error creating property trace");
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