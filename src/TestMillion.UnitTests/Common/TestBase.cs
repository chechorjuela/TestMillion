using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TestMillion.Domain.Interfaces;

namespace TestMillion.UnitTests.Common;

public abstract class TestBase
{
    protected readonly Mock<IPropertyTraceRepository> MockPropertyTraceRepository;
    protected readonly Mock<IPropertyRepository> MockPropertyRepository;
    protected readonly Mock<IOwnerRepository> MockOwnerRepository;
    protected readonly Mock<IMapper> MockMapper;

    protected TestBase()
    {
        MockPropertyTraceRepository = new Mock<IPropertyTraceRepository>();
        MockPropertyRepository = new Mock<IPropertyRepository>();
        MockOwnerRepository = new Mock<IOwnerRepository>();
        MockMapper = new Mock<IMapper>();
    }

    protected static Mock<ILogger<T>> MockLogger<T>()
    {
        return new Mock<ILogger<T>>();
    }
}