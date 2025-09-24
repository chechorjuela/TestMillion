using AutoMapper;
using MediatR;
using TestMillion.Application.Common;
using TestMillion.Application.DTOs;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Application.Properties.Queries.GetPropertyDetail;

public class GetPropertyDetailQueryHandler : IRequestHandler<GetPropertyDetailQuery, PropertyDetailDto>
{
    private readonly IBaseRepository<Property> _propertyRepository;
    private readonly IBaseRepository<PropertyImage> _imageRepository;
    private readonly IBaseRepository<Owner> _ownerRepository;
    private readonly IMapper _mapper;

    public GetPropertyDetailQueryHandler(
        IBaseRepository<Property> propertyRepository,
        IBaseRepository<PropertyImage> imageRepository,
        IBaseRepository<Owner> ownerRepository,
        IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _imageRepository = imageRepository;
        _ownerRepository = ownerRepository;
        _mapper = mapper;
    }

    public async Task<PropertyDetailDto> Handle(GetPropertyDetailQuery request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.Id);
        if (property == null)
        {
            throw new KeyNotFoundException($"Property with ID {request.Id} not found");
        }

        var owner = await _ownerRepository.GetByIdAsync(property.IdOwner);
        var images = await _imageRepository.GetAllAsync();
        var propertyImages = images.Where(i => i.IdProperty == property.Id).ToList();

        var propertyDetailDto = _mapper.Map<PropertyDetailDto>(property);
        propertyDetailDto.MainImage = propertyImages.FirstOrDefault(i => i.Enabled)?.File;
        propertyDetailDto.Owner = owner == null ? null : _mapper.Map<OwnerDto>(owner);
        propertyDetailDto.Images = _mapper.Map<IEnumerable<PropertyImageDto>>(propertyImages);
        
        return propertyDetailDto;
    }
}