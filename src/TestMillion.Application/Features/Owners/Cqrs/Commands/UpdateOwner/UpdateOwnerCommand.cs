using MediatR;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.DTOs.Response;

namespace TestMillion.Application.Features.Owners.Cqrs.Commands.UpdateOwner;

public class UpdateOwnerCommand : IRequest<ResultResponse<OwnerResponseDto>>
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Photo { get; set; }
    public DateOnly Birthdate { get; set; }
}
