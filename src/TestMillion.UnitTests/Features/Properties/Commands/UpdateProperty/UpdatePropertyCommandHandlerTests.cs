using FluentAssertions;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.Cqrs.Commands.UpdateProperty;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.UnitTests.Features.Properties.Common;

namespace TestMillion.UnitTests.Features.Properties.Commands.UpdateProperty;

public class UpdatePropertyCommandHandlerTests : PropertyTestBase
{
    private readonly UpdatePropertyCommandHandler _handler;

    public UpdatePropertyCommandHandlerTests()
    {
        _handler = new UpdatePropertyCommandHandler(
            MockPropertyRepo.Object,
            MockPropertyMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUpdateSuccessful()
    {
        // Arrange
        var command = new UpdatePropertyCommand
        {
            Id = "test-id",
            Name = "Updated Property",
            Address = "Updated Address",
            Price = 200000,
            CodeInternal = "TEST002",
            Year = 2025,
            IdOwner = "test-owner-id"
        };

        var existingProperty = new Property(
            "Test Property",
            "Test Address",
            100000,
            "TEST001",
            2024,
            "test-owner-id");

        var updatedProperty = new Property(
            command.Name,
            command.Address,
            command.Price,
            command.CodeInternal,
            command.Year,
            command.IdOwner);

        var propertyResponse = new PropertyResponseDto
        {
            Id = command.Id,
            Name = command.Name,
            Address = command.Address,
            Price = command.Price,
            CodeInternal = command.CodeInternal,
            Year = command.Year,
            IdOwner = command.IdOwner
        };

        MockPropertyRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync(existingProperty);

        MockPropertyMapper
            .Setup(x => x.Map(command, existingProperty))
            .Returns(updatedProperty);

        MockPropertyRepo
            .Setup(x => x.UpdateAsync(updatedProperty))
            .ReturnsAsync(updatedProperty);

        MockPropertyMapper
            .Setup(x => x.Map<PropertyResponseDto>(updatedProperty))
            .Returns(propertyResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be(command.Name);
        result.Data.Address.Should().Be(command.Address);
        result.Data.Price.Should().Be(command.Price);
        result.Data.CodeInternal.Should().Be(command.CodeInternal);
        result.Data.Year.Should().Be(command.Year);
        result.Data.IdOwner.Should().Be(command.IdOwner);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenPropertyDoesNotExist()
    {
        // Arrange
        var command = new UpdatePropertyCommand
        {
            Id = "nonexistent-id",
            Name = "Updated Property",
            Address = "Updated Address",
            Price = 200000,
            CodeInternal = "TEST002",
            Year = 2025,
            IdOwner = "test-owner-id"
        };

        MockPropertyRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync((Property)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.NotFound);
        result.Message.Should().Be($"Property with ID {command.Id} not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenExceptionOccurs()
    {
        // Arrange
        var command = new UpdatePropertyCommand
        {
            Id = "test-id",
            Name = "Updated Property",
            Address = "Updated Address",
            Price = 200000,
            CodeInternal = "TEST002",
            Year = 2025,
            IdOwner = "test-owner-id"
        };

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

        MockPropertyMapper
            .Setup(x => x.Map(command, existingProperty))
            .Throws(new Exception("Test exception"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Contain("An error occurred while updating the property");
        result.Message.Should().Contain("Test exception");
    }
}