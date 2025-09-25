using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.DTOs.Resposnse;

namespace TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;

public class CreatePropertyTraceCommandHandler : UseCaseHandler, IRequestHandler<CreatePropertyTraceCommand, ResultResponse<PropertyTraceResponseDto>>
{
  public Task<ResultResponse<PropertyTraceResponseDto>> Handle(CreatePropertyTraceCommand request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}