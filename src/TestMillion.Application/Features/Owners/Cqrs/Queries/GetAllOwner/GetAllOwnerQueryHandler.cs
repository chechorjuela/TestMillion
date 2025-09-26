using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Models;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Domain.Interfaces;
using TestMillion.Domain.Common.Models;

namespace TestMillion.Application.Features.Owners.Cqrs.Queries.GetAllOwner;

public class GetAllOwnerCommandHandler : UseCaseHandler, IRequestHandler<GetAllOwnerQuery, PagedResponse<List<OwnerResponseDto>>>
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
  
  public async Task<PagedResponse<List<OwnerResponseDto>>> Handle(GetAllOwnerQuery request, CancellationToken cancellationToken)
  {
    var paginationModel = _mapper.Map<PaginationModel>(request.Pagination);
    var filterModel = _mapper.Map<FilterModel>(request.Filter);
    var result = await _ownerRepository.GetPagedAsync(paginationModel, filterModel);
    var dtos = this._mapper.Map<List<OwnerResponseDto>>(result.Items);
    var meta = new PaginationMetadataDto(result.TotalCount, result.PageSize, result.CurrentPage);
    return PagedResponse<List<OwnerResponseDto>>.Success(dtos, "Owners fetched successfully", meta);
  }
}