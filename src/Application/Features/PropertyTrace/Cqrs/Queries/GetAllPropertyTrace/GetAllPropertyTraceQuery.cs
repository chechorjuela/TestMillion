using TestMillion.Application.Common.Queries;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.DTOs.Resposnse;

namespace TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetAllPropertyTrace;

public class GetAllPropertyTraceQuery : IQuery<ResultResponse<List<PropertyTraceResponseDto>>>
{
  
}