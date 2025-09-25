using TestMillion.Application.Common.Queries;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.DTOs.Response;

namespace TestMillion.Application.Features.Owners.Cqrs.Queries.GetAllOwner;

public class GetAllOwnerQuery : IQuery<ResultResponse<List<OwnerResponseDto>>>
{
  
}