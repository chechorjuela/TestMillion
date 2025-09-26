using FluentAssertions;
using Microsoft.Extensions.Logging;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.Commands.UpdatePropertyImage;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;
using TestMillion.UnitTests.Features.PropertyImage.Common;

namespace TestMillion.UnitTests.Features.PropertyImage.Commands.UpdatePropertyImage;

public class UpdatePropertyImageCommandHandlerTests : PropertyImageTestBase
{
    private readonly UpdatePropertyImageCommandHandler _handler;

    public UpdatePropertyImageCommandHandlerTests()
    {
        _handler = new UpdatePropertyImageCommandHandler(
            MockLogger.Object,
            MockPropertyImageRepo.Object,
            MockPropertyImageMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenImageIsUpdated()
    {
        // Arrange
        var command = new UpdatePropertyImageCommand
        {
            Id = "test-id",
            Name = "Updated Image",
            Description = "Updated Description",
            Enabled = false,
            ImagePath = "updated-image.jpg"
        };

        var existingImage = new Domain.Entities.PropertyImage(
            "original-image.jpg",
            true,
            "test-property-id");

        var updatedImage = new Domain.Entities.PropertyImage(
            "updated-image.jpg",
            false,
            "test-property-id");

        var propertyImageResponse = new PropertyImageResponseDto
        {
            Id = command.Id,
            FileUrl = "updated-image.jpg",
            Property = new PropertyResponseDto(),
            Enabled = command.Enabled
        };

        MockPropertyImageRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync(existingImage);

        MockPropertyImageMapper
            .Setup(x => x.Map(command, existingImage))
            .Returns(updatedImage);

        MockPropertyImageRepo
            .Setup(x => x.UpdateAsync(It.IsAny<Domain.Entities.PropertyImage>()))
            .ReturnsAsync(updatedImage);

        MockPropertyImageMapper
            .Setup(x => x.Map<PropertyImageResponseDto>(It.IsAny<Domain.Entities.PropertyImage>()))
            .Returns(propertyImageResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().NotBeNull();
        result.Data.FileUrl.Should().Be("updated-image.jpg");
        result.Data.Enabled.Should().Be(command.Enabled);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenImageNotFound()
    {
        // Arrange
        var command = new UpdatePropertyImageCommand
        {
            Id = "test-id",
            Name = "Updated Image",
            Description = "Updated Description",
            Enabled = false,
            ImagePath = "updated-image.jpg"
        };

        MockPropertyImageRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync((Domain.Entities.PropertyImage?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Be("Property image not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalid_WhenExceptionOccurs()
    {
        // Arrange
        var command = new UpdatePropertyImageCommand
        {
            Id = "test-id",
            Name = "Updated Image",
            Description = "Updated Description",
            Enabled = false,
            ImagePath = "updated-image.jpg"
        };

        var existingImage = new Domain.Entities.PropertyImage(
            "original-image.jpg",
            true,
            "test-property-id");

        MockPropertyImageRepo
            .Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync(existingImage);

        MockPropertyImageMapper
            .Setup(x => x.Map(command, existingImage))
            .Throws(new Exception("Test exception"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Invalid);
        result.Message.Should().Be("Error updating property image");

        // Verify that the error was logged
        MockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
}