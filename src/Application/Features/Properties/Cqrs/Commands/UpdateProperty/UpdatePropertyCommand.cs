using TestMillion.Application.Common.Commands;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Properties.DTOs.Request;

namespace TestMillion.Application.Features.Properties.Cqrs.Commands.UpdateProperty;

public class UpdatePropertyCommand : UpdatePropertyRequestDto, IRequest<ResultResponse<PropertyResponseDto>>
{
  
}