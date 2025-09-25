using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;

namespace TestMillion.Application.Features.PropertyImage.Cqrs.Queries.GetAllPropertyImage;

public class GetAllPropertyImageQueryHandler: UseCaseHandler, IRequestHandler<GetAllPropertyImageQuery, ResultResponse<List<PropertyImageResponseDto>>>
{
  public Task<ResultResponse<List<PropertyImageResponseDto>>> Handle(GetAllPropertyImageQuery request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}