using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.Owners.Cqrs.Queries.GetOwnerById;

public class GetByIdOwnerQueryHandler : UseCaseHandler, IRequestHandler<GetByIdOwnerQuery, ResultResponse<OwnerResponseDto>>
{
  private readonly IOwnerRepository _ownerRepository;
  private readonly IMapper _mapper;
  
  public GetByIdOwnerQueryHandler(
    IMapper mapper,
    IOwnerRepository ownerRepository)
  {
    _mapper = mapper;
    _ownerRepository = ownerRepository;
  }
  
  public async Task<ResultResponse<OwnerResponseDto>> Handle(GetByIdOwnerQuery request, CancellationToken cancellationToken)
  {
    var owner = await  _ownerRepository.GetByIdAsync(request.OwnerId);
    var response = this._mapper.Map<OwnerResponseDto>(owner);
    return Succeded(response);
  }
}