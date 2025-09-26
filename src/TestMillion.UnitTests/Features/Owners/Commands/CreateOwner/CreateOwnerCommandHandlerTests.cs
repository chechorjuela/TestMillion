using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.Cqrs.Commands.CreateOwner;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.UnitTests.Features.Owners.Common;

namespace TestMillion.UnitTests.Features.Owners.Commands.CreateOwner;

public class CreateOwnerCommandHandlerTests : OwnerTestBase
{
    private readonly CreateOwnerCommandHandler _handler;

    public CreateOwnerCommandHandlerTests()
    {
        _handler = new CreateOwnerCommandHandler(
            MockOwnerRepo.Object,
            MockOwnerMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenOwnerWithSameNameExists()
    {
        // Arrange
        var command = new CreateOwnerCommand
        {
            Name = "Test Owner",
            Address = "Test Address",
            Birthdate = DateOnly.FromDateTime(DateTime.Now),
            Photo = "test.jpg"
        };

        var existingOwner = new Owner("Test Owner", "Existing Address", DateOnly.FromDateTime(DateTime.Now)) { Photo = "existing.jpg" };
        
        MockOwnerRepo
            .Setup(x => x.GetByNameAsync(command.Name))
            .ReturnsAsync(existingOwner);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Be("An owner with this name already exists.");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenRepositoryFailsToCreate()
    {
        // Arrange
        var command = new CreateOwnerCommand
        {
            Name = "Test Owner",
            Address = "Test Address",
            Birthdate = DateOnly.FromDateTime(DateTime.Now),
            Photo = "test.jpg"
        };

        var owner = new Owner(command.Name, command.Address, command.Birthdate) { Photo = command.Photo };
        
        MockOwnerRepo
            .Setup(x => x.GetByNameAsync(command.Name))
            .ReturnsAsync((Owner?)null);

        MockOwnerMapper
            .Setup(x => x.Map<Owner>(command))
            .Returns(owner);

        MockOwnerRepo
            .Setup(x => x.AddAsync(It.IsAny<Owner>()))
            .ReturnsAsync((Owner?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Be("Failed to create owner.");
    }

    [Fact]
    public async Task Handle_ShouldReturnCreated_WhenSuccessful()
    {
        // Arrange
        var command = new CreateOwnerCommand
        {
            Name = "Test Owner",
            Address = "Test Address",
            Birthdate = DateOnly.FromDateTime(DateTime.Now),
            Photo = "test.jpg"
        };

        var owner = new Owner(command.Name, command.Address, command.Birthdate) { Photo = command.Photo };
        var ownerResponse = new OwnerResponseDto
        {
            Id = "1",
            Name = command.Name,
            Address = command.Address,
            Photo = command.Photo,
            Birthdate = command.Birthdate
        };

        MockOwnerRepo
            .Setup(x => x.GetByNameAsync(command.Name))
            .ReturnsAsync((Owner?)null);

        MockOwnerMapper
            .Setup(x => x.Map<Owner>(command))
            .Returns(owner);

        MockOwnerRepo
            .Setup(x => x.AddAsync(It.IsAny<Owner>()))
            .ReturnsAsync(owner);

        MockOwnerMapper
            .Setup(x => x.Map<OwnerResponseDto>(owner))
            .Returns(ownerResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Created);
        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be(command.Name);
        result.Data.Address.Should().Be(command.Address);
        result.Data.Photo.Should().Be(command.Photo);
        result.Data.Birthdate.Should().Be(command.Birthdate);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenExceptionOccurs()
    {
        // Arrange
        var command = new CreateOwnerCommand
        {
            Name = "Test Owner",
            Address = "Test Address",
            Birthdate = DateOnly.FromDateTime(DateTime.Now),
            Photo = "test.jpg"
        };

        MockOwnerRepo
            .Setup(x => x.GetByNameAsync(command.Name))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Contain("An error occurred while creating the owner");
        result.Message.Should().Contain("Test exception");
    }
}