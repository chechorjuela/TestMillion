using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;

namespace TestMillion.Application.Features.PropertyImage.Cqrs.Queries.GetAllPropertyImage;

public class GetByPropertyImageQueryHandler: UseCaseHandler, IRequestHandler<GetByPropertyImageQuery, ResultResponse<PropertyImageResponseDto>>
{
  public Task<ResultResponse<PropertyImageResponseDto>> Handle(GetByPropertyImageQuery request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}