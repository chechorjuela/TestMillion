using AutoMapper;
using MediatR;
using TestMillion.Application.Common;
using TestMillion.Application.DTOs;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Application.Properties.Queries.GetProperties;

public class GetPropertiesQueryHandler : IRequestHandler<GetPropertiesQuery, IEnumerable<PropertyDto>>
{
    private readonly IBaseRepository<Property> _propertyRepository;
    private readonly IBaseRepository<PropertyImage> _imageRepository;
    private readonly IMapper _mapper;

    public GetPropertiesQueryHandler(
        IBaseRepository<Property> propertyRepository,
        IBaseRepository<PropertyImage> imageRepository,
        IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _imageRepository = imageRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PropertyDto>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
    {
        var properties = await _propertyRepository.GetAllAsync();
        var filteredProperties = properties.AsQueryable();

        // Apply filters
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

        var propertyDtos = _mapper.Map<IEnumerable<PropertyDto>>(propertyList);
        
        foreach (var dto in propertyDtos)
        {
            dto.MainImage = enabledImages.FirstOrDefault(i => i.IdProperty == dto.Id)?.File;
        }
        
        return propertyDtos;
    }
}