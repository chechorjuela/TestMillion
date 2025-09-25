using TestMillion.Application.Features.Properties.Cqrs.Commands.CreateProperty;
using TestMillion.Application.Features.Properties.Cqrs.Commands.UpdateProperty;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Properties.DTOs.Request;
using TestMillion.Domain.Entities;

namespace TestMillion.Application.Features.Properties.Mappings;

public class PropertyMappingProfile : Profile
{
  public PropertyMappingProfile()
  {
    // Create mappings
    CreateMap<CreatePropertyRequestDto, CreatePropertyCommand>();
    CreateMap<CreatePropertyCommand, Property>();
        
    // Update mappings
    CreateMap<UpdatePropertyRequestDto, UpdatePropertyCommand>();
    CreateMap<UpdatePropertyCommand, Property>();
        
    // Response mappings
    CreateMap<Property, PropertyResponseDto>()
      .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner));
  }
}