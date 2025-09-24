using TestMillion.Application.Common.Commands;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.DTOs.Request;
using TestMillion.Application.Features.Owners.DTOs.Response;

namespace TestMillion.Application.Features.Owners.Commands.CreateOwner;

public class CreateOwnerCommand : IRequest<ResultResponse<OwnerResponseDto>>
{
  public required string Name { get; set; }
  public required string Address { get; set; }
  public required string Photo { get; set; }
  public DateOnly Birthdate { get; set; }
}
