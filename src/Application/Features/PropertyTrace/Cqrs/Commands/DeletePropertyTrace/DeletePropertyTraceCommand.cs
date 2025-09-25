using TestMillion.Application.Common.Commands;
using TestMillion.Application.Common.Response.Result;

namespace TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;

public class DeletePropertyTraceCommand: ICommand<ResultResponse<bool>>
{
  public string Id { get; set; }
}