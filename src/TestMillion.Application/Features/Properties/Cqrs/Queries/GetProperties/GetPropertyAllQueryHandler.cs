using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;
using TestMillion.Domain.Interfaces.Base;
using TestMillion.Domain.Common.Models;
using TestMillion.Application.Common.Models;

namespace TestMillion.Application.Features.Properties.Cqrs.Queries.GetProperties;

public class GetPropertyAllQueryHandler : UseCaseHandler,
  IRequestHandler<GetPropertyAllQuery, PagedResponse<List<PropertyResponseDto>>>
{
  private readonly IPropertyRepository _propertyRepository;
  private readonly IBaseRepository<Domain.Entities.PropertyImage> _imageRepository;
  private readonly IOwnerRepository _ownerRepository;
  private readonly IMapper _mapper;

  public GetPropertyAllQueryHandler(
    IPropertyRepository propertyRepository,
    IBaseRepository<Domain.Entities.PropertyImage> imageRepository,
    IOwnerRepository ownerRepository,
    IMapper mapper)
  {
    _propertyRepository = propertyRepository;
    _imageRepository = imageRepository;
    _ownerRepository = ownerRepository;
    _mapper = mapper;
  }

  public async Task<PagedResponse<List<PropertyResponseDto>>> Handle(GetPropertyAllQuery request, CancellationToken cancellationToken)
  {
    var paginationModel = _mapper.Map<PaginationModel>(request.Pagination);
    var filterModel = _mapper.Map<FilterModel>(request.Filter);
    var (items, total) = await _propertyRepository.GetPagedAsync(paginationModel, filterModel);
    var images = await _imageRepository.GetAllAsync();
    var enabledImages = images.Where(i => i.Enabled).ToList();

    var propertyDtos = _mapper.Map<List<PropertyResponseDto>>(items.ToList());

    foreach (var (dto, entity) in propertyDtos.Zip(items, (dto, entity) => (dto, entity)))
    {
      dto.MainImage = enabledImages.FirstOrDefault(i => i.IdProperty == dto.Id)?.File;
      var owner = await _ownerRepository.GetByIdAsync(entity.IdOwner);
      dto.Owner = _mapper.Map<Owners.DTOs.Response.OwnerResponseDto>(owner);
    }

    var metadata = new PaginationMetadataDto(total, request.Pagination.PageSize, request.Pagination.PageNumber);
    return PagedResponse<List<PropertyResponseDto>>.Success(propertyDtos, "Properties fetched successfully", metadata);
  }
}