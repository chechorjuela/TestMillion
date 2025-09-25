using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;

namespace TestMillion.Application.Features.PropertyImage.Commands.DeletePropertyImage;

public class DeletePropertyImageCommandHandler: UseCaseHandler, IRequestHandler<DeletePropertyImageCommand, ResultResponse<bool>>
{
  public Task<ResultResponse<bool>> Handle(DeletePropertyImageCommand request, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}