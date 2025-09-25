using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TestMillion.Domain.Interfaces;
using TestMillion.UnitTests.Common;

namespace TestMillion.UnitTests.Features.PropertyImage.Common;

public abstract class PropertyImageTestBase : TestBase
{
    protected readonly Mock<IPropertyImageRepository> MockPropertyImageRepo;
    protected readonly Mock<IPropertyRepository> MockPropertyRepo;
    protected readonly Mock<IOwnerRepository> MockOwnerRepo;
    protected readonly Mock<IMapper> MockPropertyImageMapper;
    protected readonly Mock<ILogger<TestMillion.Application.Features.PropertyImage.Commands.UpdatePropertyImage.UpdatePropertyImageCommandHandler>> MockLogger;

    protected PropertyImageTestBase()
    {
        MockPropertyImageRepo = new Mock<IPropertyImageRepository>();
        MockPropertyRepo = new Mock<IPropertyRepository>();
        MockOwnerRepo = new Mock<IOwnerRepository>();
        MockPropertyImageMapper = new Mock<IMapper>();
        MockLogger = new Mock<ILogger<TestMillion.Application.Features.PropertyImage.Commands.UpdatePropertyImage.UpdatePropertyImageCommandHandler>>();
    }
}