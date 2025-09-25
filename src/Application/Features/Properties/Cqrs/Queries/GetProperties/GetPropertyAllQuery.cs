using TestMillion.Application.Common.Queries;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Properties.DTOs.Request;

namespace TestMillion.Application.Features.Properties.Cqrs.Queries.GetProperties;

public class GetPropertyAllQuery : IQuery<ResultResponse<List<PropertyResponseDto>>>
{
  public PropertyFilterRequestDto Filter { get; set; } = null!;
}