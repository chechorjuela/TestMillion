using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.DTOs;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Application.Features.Properties.Cqrs.Queries.GetProperties;

public class GetPropertyAllQueryHandler : UseCaseHandler,
  IRequestHandler<GetPropertyAllQuery, ResultResponse<List<PropertyResponseDto>>>
{
  private readonly IBaseRepository<Property> _propertyRepository;
  private readonly IBaseRepository<Domain.Entities.PropertyImage> _imageRepository;
  private readonly IMapper _mapper;

  public GetPropertyAllQueryHandler(
    IBaseRepository<Property> propertyRepository,
    IBaseRepository<Domain.Entities.PropertyImage> imageRepository,
    IMapper mapper)
  {
    _propertyRepository = propertyRepository;
    _imageRepository = imageRepository;
    _mapper = mapper;
  }

  public async Task<ResultResponse<List<PropertyResponseDto>>> Handle(GetPropertyAllQuery request, CancellationToken cancellationToken)
  {
    var properties = await _propertyRepository.GetAllAsync();
    var filteredProperties = properties.AsQueryable();

    if (!string.IsNullOrWhiteSpace(request.Filter.Name))
    {
      filteredProperties = filteredProperties.Where(p =>
        p.Name.Contains(request.Filter.Name, StringComparison.OrdinalIgnoreCase));
    }

    if (!string.IsNullOrWhiteSpace(request.Filter.Address))
    {
      filteredProperties = filteredProperties.Where(p =>
        p.Address.Contains(request.Filter.Address, StringComparison.OrdinalIgnoreCase));
    }

    if (request.Filter.MinPrice.HasValue)
    {
      filteredProperties = filteredProperties.Where(p => p.Price >= request.Filter.MinPrice.Value);
    }

    if (request.Filter.MaxPrice.HasValue)
    {
      filteredProperties = filteredProperties.Where(p => p.Price <= request.Filter.MaxPrice.Value);
    }

    var propertyList = filteredProperties.ToList();
    var images = await _imageRepository.GetAllAsync();
    var enabledImages = images.Where(i => i.Enabled).ToList();

    var propertyDtos = _mapper.Map<List<PropertyResponseDto>>(propertyList);

    foreach (var dto in propertyDtos)
    {
      dto.MainImage = enabledImages.FirstOrDefault(i => i.IdProperty == dto.Id)?.File;
    }

    return Succeded(propertyDtos);
  }
}