using TestMillion.Application.Common.Commands;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.DTOs.Request;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;

namespace TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;

public class CreatePropertyTraceCommand : CreatePropertyTraceRequestDto, ICommand<ResultResponse<PropertyTraceResponseDto>>
{
  
}