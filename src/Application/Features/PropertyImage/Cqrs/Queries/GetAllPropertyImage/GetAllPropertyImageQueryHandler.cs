using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Models;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;
using TestMillion.Domain.Interfaces;
using TestMillion.Domain.Common.Models;
using MediatR;

namespace TestMillion.Application.Features.PropertyImage.Cqrs.Queries.GetAllPropertyImage;

public class GetAllPropertyImageQueryHandler : UseCaseHandler,
  IRequestHandler<GetAllPropertyImageQuery, PagedResponse<List<PropertyImageResponseDto>>>
{
  private readonly IPropertyImageRepository _propertyImageRepository;
  private readonly IPropertyRepository _propertyRepository;
  private readonly IOwnerRepository _ownerRepository;
  private readonly IMapper _mapper;

  public GetAllPropertyImageQueryHandler(
    IPropertyImageRepository propertyImageRepository,
    IPropertyRepository propertyRepository,
    IOwnerRepository ownerRepository,
    IMapper mapper
  )
  {
    _mapper = mapper;
    _propertyImageRepository = propertyImageRepository;
    _propertyRepository = propertyRepository;
    _ownerRepository = ownerRepository;
  }

  public async Task<PagedResponse<List<PropertyImageResponseDto>>> Handle(GetAllPropertyImageQuery request,
    CancellationToken cancellationToken)
  {
    var paginationModel = _mapper.Map<PaginationModel>(request.Pagination);
    var filterModel = _mapper.Map<FilterModel>(request.Filter);
    var (items, total) = await _propertyImageRepository.GetPagedAsync(paginationModel, filterModel);
    var response = this._mapper.Map<List<PropertyImageResponseDto>>(items);

    foreach (var (dto, entity) in response.Zip(items, (dto, entity) => (dto, entity)))
    {
        var property = await _propertyRepository.GetByIdAsync(entity.IdProperty);
        if (property != null)
        {
            dto.Property = _mapper.Map<Properties.DTOs.Response.PropertyResponseDto>(property);
        }
    }

    var meta = new PaginationMetadataDto(total, request.Pagination.PageSize, request.Pagination.PageNumber);
    return PagedResponse<List<PropertyImageResponseDto>>.Success(response, "Property images fetched successfully", meta);
  }
}