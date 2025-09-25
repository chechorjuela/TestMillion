using TestMillion.Application.Common.Commands;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.DTOs.Request;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;

namespace TestMillion.Application.Features.PropertyTrace.Commands.UpdatePropertyTrace;

public class UpdatePropertyTraceCommand : UpdatePropertyTraceRequestDto, ICommand<ResultResponse<PropertyTraceResponseDto>>
{
    // Id is set from the URL route parameter
    public string Id { get; set; } = null!;
}
