using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TestMillion.Domain.Interfaces;
using TestMillion.UnitTests.Common;

namespace TestMillion.UnitTests.Features.Owners.Common;

public abstract class OwnerTestBase : TestBase
{
    protected readonly Mock<IOwnerRepository> MockOwnerRepo;
    protected readonly Mock<IMapper> MockOwnerMapper;

    protected OwnerTestBase()
    {
        MockOwnerRepo = new Mock<IOwnerRepository>();
        MockOwnerMapper = new Mock<IMapper>();
    }
}