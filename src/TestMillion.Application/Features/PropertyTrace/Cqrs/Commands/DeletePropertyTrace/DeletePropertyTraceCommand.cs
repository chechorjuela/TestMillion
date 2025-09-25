using TestMillion.Application.Common.Commands;
using TestMillion.Application.Common.Response.Result;

namespace TestMillion.Application.Features.PropertyTrace.Commands.DeletePropertyTrace;

public class DeletePropertyTraceCommand: ICommand<ResultResponse<bool>>
{
  public required string Id { get; set; }
}