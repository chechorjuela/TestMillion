using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;

namespace TestMillion.Application.Features.PropertyImage.Commands.DeletePropertyImage;

public class UpdatePropertyImageCommandHandler : UseCaseHandler, IRequestHandler<UpdatePropertyImageCommand, ResultResponse<PropertyImageResponseDto>>
{
  public Task<ResultResponse<PropertyImageResponseDto>> Handle(UpdatePropertyImageCommand request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}