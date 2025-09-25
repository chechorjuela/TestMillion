using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;

namespace TestMillion.Application.Features.PropertyImage.Commands.CreatePropertyImage;

public class CreatePropertyImageCommandHandler : UseCaseHandler, IRequestHandler<CreatePropertyImageCommand, ResultResponse<PropertyImageResponseDto>>
{
  public Task<ResultResponse<PropertyImageResponseDto>> Handle(CreatePropertyImageCommand request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}