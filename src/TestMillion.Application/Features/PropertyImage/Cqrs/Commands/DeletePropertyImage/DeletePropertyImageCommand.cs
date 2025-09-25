using MediatR;
using TestMillion.Application.Common.Commands;
using TestMillion.Application.Common.Response.Result;

namespace TestMillion.Application.Features.PropertyImage.Commands.DeletePropertyImage;

public class DeletePropertyImageCommand : ICommand<ResultResponse<bool>>
{
  public required string Id { get; set; }
}