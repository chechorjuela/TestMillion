using TestMillion.Application.Common.Queries;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.DTOs.Resposnse;

namespace TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetByPropertyTrace;

public class GetByPropertyTraceQuery : IQuery<ResultResponse<PropertyTraceResponseDto>>
{
  public readonly string Id;

  public GetByPropertyTraceQuery(string id)
  {
    Id = id;
  }
}