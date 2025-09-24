using TestMillion.Application.Common.Queries;
using TestMillion.Application.DTOs;
using TestMillion.Application.Properties.DTOs.Request;

namespace TestMillion.Application.Properties.Queries;

public class GetPropertiesQuery : IQuery<IEnumerable<PropertyDto>>
{
    public PropertyFilterRequest Filter { get; set; } = null!;
}