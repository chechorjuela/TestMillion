using TestMillion.Application.Common.Queries;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.DTOs.Response;

namespace TestMillion.Application.Features.Owners.Cqrs.Queries.GetOwnerById;

public class GetByIdOwnerQuery : IQuery<ResultResponse<OwnerResponseDto>>
{
  public string OwnerId { get; }

  public GetByIdOwnerQuery(string ownerId)
  {
    OwnerId = ownerId;
  }
}