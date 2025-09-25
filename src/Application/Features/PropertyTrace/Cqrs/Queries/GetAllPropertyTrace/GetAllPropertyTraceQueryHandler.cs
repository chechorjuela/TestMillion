using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.DTOs.Resposnse;

namespace TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetAllPropertyTrace;

public class GetAllPropertyTraceQueryHandler: UseCaseHandler, 
  IRequestHandler<GetAllPropertyTraceQuery, ResultResponse<List<PropertyTraceResponseDto>>>
{
  public Task<ResultResponse<List<PropertyTraceResponseDto>>> Handle(GetAllPropertyTraceQuery request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}
