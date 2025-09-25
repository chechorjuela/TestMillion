using TestMillion.Application.Common.Queries;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;

namespace TestMillion.Application.Features.PropertyImage.Cqrs.Queries.GetByIdPropertyImage;

public class GetByPropertyImageQuery: IQuery<ResultResponse<PropertyImageResponseDto>>
{
  public readonly string Id;
  
  public GetByPropertyImageQuery(string id)
  {
    Id = id;
  }
}