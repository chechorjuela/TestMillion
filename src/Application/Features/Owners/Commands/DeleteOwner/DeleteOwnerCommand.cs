using MediatR;
using TestMillion.Application.Common.Response.Result;

namespace TestMillion.Application.Features.Owners.Commands.DeleteOwner;

public class DeleteOwnerCommand : IRequest<ResultResponse<bool>>
{
    public required string Id { get; set; }
}