using TestMillion.Application.Common.Commands;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Features.PropertyTrace.DTOs.Request;

namespace TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;

public class UpdatePropertyTraceCommand : UpdatePropertyTraceRequestDto, ICommand<ResultResponse<PropertyResponseDto>>
{
  
}