using AutoMapper;
using TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;
using TestMillion.Application.Features.PropertyTrace.Commands.UpdatePropertyTrace;
using TestMillion.Application.Features.PropertyTrace.DTOs.Request;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;

namespace TestMillion.Application.Features.PropertyTrace.Mappings;

public class PropertyTraceMappingProfile : Profile
{
    public PropertyTraceMappingProfile()
    {
        CreateMap<Domain.Entities.PropertyTrace, DTOs.Response.PropertyTraceResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DateSale, opt => opt.MapFrom(src => src.DateSale))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => src.Tax))
            .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty))
            .ForMember(dest => dest.Property, opt => opt.Ignore());

        CreateMap<CreatePropertyTraceRequestDto, CreatePropertyTraceCommand>();

        CreateMap<UpdatePropertyTraceRequestDto, UpdatePropertyTraceCommand>();

        // Map update command to entity
        CreateMap<UpdatePropertyTraceCommand, Domain.Entities.PropertyTrace>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
