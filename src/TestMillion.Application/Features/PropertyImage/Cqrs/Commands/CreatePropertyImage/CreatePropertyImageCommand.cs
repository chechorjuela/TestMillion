using TestMillion.Application.Common.Commands;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.DTOs.Request;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;

namespace TestMillion.Application.Features.PropertyImage.Cqrs.Commands.CreatePropertyImage;

public class CreatePropertyImageCommand : CreatePropertyImageRequestDto, ICommand<ResultResponse<PropertyImageResponseDto>>
{
  
}