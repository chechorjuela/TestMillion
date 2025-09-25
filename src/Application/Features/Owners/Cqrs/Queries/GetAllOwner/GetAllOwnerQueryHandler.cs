using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.Owners.Cqrs.Queries.GetAllOwner;

public class GetAllOwnerCommandHandler : UseCaseHandler, IRequestHandler<GetAllOwnerQuery, ResultResponse<List<OwnerResponseDto>>>
{
  public readonly IOwnerRepository _ownerRepository;
  public readonly IMapper _mapper;
  
  public GetAllOwnerCommandHandler(
    IMapper mapper,
    IOwnerRepository ownerRepository)
  {
    _mapper = mapper;
    _ownerRepository = ownerRepository;
  }
  
  public async Task<ResultResponse<List<OwnerResponseDto>>> Handle(GetAllOwnerQuery request, CancellationToken cancellationToken)
  {
    var owners = await _ownerRepository.GetAllAsync();
    var result = this._mapper.Map<List<OwnerResponseDto>>(owners);
    return Succeded(result);
  }
}