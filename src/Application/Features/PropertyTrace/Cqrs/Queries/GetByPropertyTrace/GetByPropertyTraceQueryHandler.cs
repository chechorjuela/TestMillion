using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.DTOs.Resposnse;

namespace TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetByPropertyTrace;

public class GetByPropertyTraceQueryHandler : UseCaseHandler, IRequestHandler<GetByPropertyTraceQuery, ResultResponse<PropertyTraceResponseDto>>
{
  public Task<ResultResponse<PropertyTraceResponseDto>> Handle(GetByPropertyTraceQuery request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}