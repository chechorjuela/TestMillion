using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TestMillion.Domain.Interfaces;
using TestMillion.UnitTests.Common;

namespace TestMillion.UnitTests.Features.Properties.Common;

public abstract class PropertyTestBase : TestBase
{
    protected readonly Mock<IPropertyRepository> MockPropertyRepo;
    protected readonly Mock<IOwnerRepository> MockOwnerRepo;
    protected readonly Mock<IMapper> MockPropertyMapper;

    protected PropertyTestBase()
    {
        MockPropertyRepo = new Mock<IPropertyRepository>();
        MockOwnerRepo = new Mock<IOwnerRepository>();
        MockPropertyMapper = new Mock<IMapper>();
    }
}