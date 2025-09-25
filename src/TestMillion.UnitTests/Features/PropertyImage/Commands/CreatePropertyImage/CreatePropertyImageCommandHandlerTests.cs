using FluentAssertions;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.Cqrs.Commands.CreatePropertyImage;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;
using TestMillion.UnitTests.Features.PropertyImage.Common;

namespace TestMillion.UnitTests.Features.PropertyImage.Commands.CreatePropertyImage;

public class CreatePropertyImageCommandHandlerTests : PropertyImageTestBase
{
    private readonly CreatePropertyImageCommandHandler _handler;

    public CreatePropertyImageCommandHandlerTests()
    {
        _handler = new CreatePropertyImageCommandHandler(
            MockPropertyImageRepo.Object,
            MockPropertyImageMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenImageIsCreated()
    {
        // Arrange
        var command = new CreatePropertyImageCommand
        {
            File = "test-image.jpg",
            IdProperty = "test-property-id",
            Enabled = true
        };

        var propertyImage = new Domain.Entities.PropertyImage(
            command.File,
            command.Enabled,
            command.IdProperty);

        var propertyImageResponse = new PropertyImageResponseDto
        {
            Id = "test-id",
            FileUrl = command.File,
            Property = new PropertyResponseDto(),
            Enabled = command.Enabled
        };

        MockPropertyImageMapper
            .Setup(x => x.Map<Domain.Entities.PropertyImage>(command))
            .Returns(propertyImage);

        MockPropertyImageRepo
            .Setup(x => x.AddAsync(propertyImage))
            .ReturnsAsync(propertyImage);

        MockPropertyImageMapper
            .Setup(x => x.Map<PropertyImageResponseDto>(propertyImage))
            .Returns(propertyImageResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(ResultType.Ok);
        result.Data.Should().NotBeNull();
        result.Data.FileUrl.Should().Be(command.File);
        result.Data.Enabled.Should().Be(command.Enabled);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenMappingFails()
    {
        // Arrange
        var command = new CreatePropertyImageCommand
        {
            File = "test-image.jpg",
            IdProperty = "test-property-id",
            Enabled = true
        };

        MockPropertyImageMapper
            .Setup(x => x.Map<Domain.Entities.PropertyImage>(command))
            .Throws(new Exception("Mapping failed"));

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Mapping failed");
    }
}