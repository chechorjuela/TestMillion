using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;

namespace TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;

public class DeletePropertyTraceCommandHandler : UseCaseHandler, IRequestHandler<DeletePropertyTraceCommand, ResultResponse<bool>>
{
  public Task<ResultResponse<bool>> Handle(DeletePropertyTraceCommand request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}