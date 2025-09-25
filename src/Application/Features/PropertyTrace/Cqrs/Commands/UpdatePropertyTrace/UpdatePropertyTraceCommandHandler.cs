using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;

namespace TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;

public class UpdatePropertyTraceCommandHandler : UseCaseHandler, IRequestHandler<UpdatePropertyTraceCommand, ResultResponse<PropertyResponseDto>>
{
  public Task<ResultResponse<PropertyResponseDto>> Handle(UpdatePropertyTraceCommand request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}