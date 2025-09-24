using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestMillion.Application.Common.Models;
using TestMillion.Application.DTOs;
using TestMillion.Application.Properties.Queries.GetProperties;
using TestMillion.Presentation.Controllers;

namespace TestMillion.Tests.Controllers;

public class PropertiesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PropertiesController _controller;

    public PropertiesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PropertiesController(_mediatorMock.Object);
    }

    [Test]
    public async Task GetProperties_WithValidParameters_ReturnsOkResult()
    {
        // Arrange
        var properties = new PagedResponse<PropertyDto>
        {
            Items = new List<PropertyDto>
            {
                new()
                {
                    Id = "1",
                    IdOwner = "owner1",
                    Name = "Test Property",
                    Address = "123 Test St",
                    Price = 100000,
                    MainImage = "image.jpg"
                }
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 1
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetPropertiesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(properties);

        // Act
        var result = await _controller.GetProperties(
            name: "Test",
            address: "123",
            minPrice: 90000,
            maxPrice: 110000,
            page: 1,
            pageSize: 10);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedProperties = okResult.Value.Should().BeOfType<PagedResponse<PropertyDto>>().Subject;
        returnedProperties.Items.Should().HaveCount(1);
        returnedProperties.TotalCount.Should().Be(1);
    }

    [Test]
    public async Task GetProperties_WithInvalidParameters_ReturnsEmptyList()
    {
        // Arrange
        var properties = new PagedResponse<PropertyDto>
        {
            Items = Array.Empty<PropertyDto>(),
            Page = 1,
            PageSize = 10,
            TotalCount = 0
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetPropertiesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(properties);

        // Act
        var result = await _controller.GetProperties(
            name: "NonExistent",
            address: null,
            minPrice: null,
            maxPrice: null);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedProperties = okResult.Value.Should().BeOfType<PagedResponse<PropertyDto>>().Subject;
        returnedProperties.Items.Should().BeEmpty();
        returnedProperties.TotalCount.Should().Be(0);
    }
}