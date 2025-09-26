using FluentAssertions;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.Cqrs.Commands.CreateProperty;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.UnitTests.Features.Properties.Common;

namespace TestMillion.UnitTests.Features.Properties.Commands.CreateProperty;

public class CreatePropertyCommandHandlerTests : PropertyTestBase
{
    private readonly CreatePropertyCommandHandler _handler;

    public CreatePropertyCommandHandlerTests()
    {
        _handler = new CreatePropertyCommandHandler(
            MockPropertyRepo.Object,
            MockPropertyMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCreated_WhenSuccessful()
    {
        // Arrange
        var command = new CreatePropertyCommand
        {
            Name = "Test Property",
            Address = "Test Address",
            Price = 100000,
            CodeInternal = "TEST001",
            Year = 2024,
            IdOwner = "test-owner-id"
        };

        var property = new Property(
            command.Name,
            command.Address,
            command.Price,
            command.CodeInternal,
            command.Year,
            command.IdOwner);

        var propertyResponse = new PropertyResponseDto
        {
            Id = "1",
            Name = command.Name,
            Address = command.Address,
            Price = command.Price,
            CodeInternal = command.CodeInternal,
            Year = command.Year,
            IdOwner = command.IdOwner
        };

        MockPropertyMapper
            .Setup(x => x.Map<Property>(command))
            .Returns(property);

        MockPropertyRepo
            .Setup(x => x.AddAsync(property))
            .ReturnsAsync(property);

        MockPropertyMapper
            .Setup(x => x.Map<PropertyResponseDto>(property))
            .Returns(propertyResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Created);
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be(command.Name);
        result.Data.Address.Should().Be(command.Address);
        result.Data.Price.Should().Be(command.Price);
        result.Data.CodeInternal.Should().Be(command.CodeInternal);
        result.Data.Year.Should().Be(command.Year);
        result.Data.IdOwner.Should().Be(command.IdOwner);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenRepositoryFailsToCreate()
    {
        // Arrange
        var command = new CreatePropertyCommand
        {
            Name = "Test Property",
            Address = "Test Address",
            Price = 100000,
            CodeInternal = "TEST001",
            Year = 2024,
            IdOwner = "test-owner-id"
        };

        var property = new Property(
            command.Name,
            command.Address,
            command.Price,
            command.CodeInternal,
            command.Year,
            command.IdOwner);

        MockPropertyMapper
            .Setup(x => x.Map<Property>(command))
            .Returns(property);

        MockPropertyRepo
            .Setup(x => x.AddAsync(It.IsAny<Property>()))
            .ReturnsAsync((Property?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Be("Failed to create property.");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenExceptionOccurs()
    {
        // Arrange
        var command = new CreatePropertyCommand
        {
            Name = "Test Property",
            Address = "Test Address",
            Price = 100000,
            CodeInternal = "TEST001",
            Year = 2024,
            IdOwner = "test-owner-id"
        };

        MockPropertyMapper
            .Setup(x => x.Map<Property>(command))
            .Throws(new Exception("Test exception"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Contain("An error occurred while creating the property");
        result.Message.Should().Contain("Test exception");
    }
}