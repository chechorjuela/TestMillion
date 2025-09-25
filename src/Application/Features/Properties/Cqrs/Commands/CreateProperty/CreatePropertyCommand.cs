using TestMillion.Application.Common.Commands;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Properties.DTOs.Request;

namespace TestMillion.Application.Features.Properties.Cqrs.Commands.CreateProperty;

public class CreatePropertyCommand : CreatePropertyRequestDto, IRequest<ResultResponse<PropertyResponseDto>>
{

}
