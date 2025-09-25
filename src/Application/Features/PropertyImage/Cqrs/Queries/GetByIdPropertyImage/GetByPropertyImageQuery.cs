using TestMillion.Application.Common.Queries;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;

namespace TestMillion.Application.Features.PropertyImage.Cqrs.Queries.GetAllPropertyImage;

public class GetByPropertyImageQuery: IQuery<ResultResponse<PropertyImageResponseDto>>
{
  public string Id;
}