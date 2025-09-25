using TestMillion.Application.Common.Response;
using TestMillion.Application.DTOs;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;
using PropertyImage = TestMillion.Domain.Entities.PropertyImage;

namespace TestMillion.Application.Features.Properties.Cqrs.Queries.GetPropertyDetail;

public class GetPropertyDetailQueryHandler : UseCaseHandler, IRequestHandler<GetPropertyDetailQuery, PropertyDetailDto>
{
    private readonly IBaseRepository<Property> _propertyRepository;
    private readonly IBaseRepository<Domain.Entities.PropertyImage> _imageRepository;
    private readonly IBaseRepository<Owner> _ownerRepository;
    private readonly IMapper _mapper;

    public GetPropertyDetailQueryHandler(
        IBaseRepository<Property> propertyRepository,
        IBaseRepository<Domain.Entities.PropertyImage> imageRepository,
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