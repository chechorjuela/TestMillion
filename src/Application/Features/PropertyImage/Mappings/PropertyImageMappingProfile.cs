using AutoMapper;
using TestMillion.Application.Features.PropertyImage.Cqrs.Commands.CreatePropertyImage;
using TestMillion.Application.Features.PropertyImage.Commands.UpdatePropertyImage;
using TestMillion.Application.Features.PropertyImage.DTOs.Request;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;

namespace TestMillion.Application.Features.PropertyImage.Mappings;

public class PropertyImageMappingProfile : Profile
{
    public PropertyImageMappingProfile()
    {
        CreateMap<Domain.Entities.PropertyImage, DTOs.Response.PropertyImageResponseDto>()
            .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.File));

        CreateMap<CreatePropertyImageRequestDto, CreatePropertyImageCommand>()
            .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty));

        CreateMap<CreatePropertyImageCommand, Domain.Entities.PropertyImage>()
            .ConstructUsing(src => new Domain.Entities.PropertyImage(
                src.File,
                src.Enabled,
                src.IdProperty
            ));

        CreateMap<UpdatePropertyImageRequestDto, UpdatePropertyImageCommand>();

        CreateMap<UpdatePropertyImageCommand, Domain.Entities.PropertyImage>()
            .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.ImagePath))
            .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled));
    }
}
