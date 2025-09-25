using TestMillion.Application.Common.Commands;
using TestMillion.Application.Common.Response.Result;

namespace TestMillion.Application.Features.Properties.Cqrs.Commands.DeleteProperty;

public class DeletePropertyCommand: ICommand<ResultResponse<bool>>
{
  public string Id;
  public DeletePropertyCommand(string id)
  {
    Id = id;
  }
}