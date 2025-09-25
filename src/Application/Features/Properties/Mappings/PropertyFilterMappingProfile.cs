using AutoMapper;
using TestMillion.Application.Features.Properties.DTOs.Request;
using TestMillion.Domain.Common.Models;

namespace TestMillion.Application.Features.Properties.Mappings;

public class PropertyFilterMappingProfile : Profile
{
    public PropertyFilterMappingProfile()
    {
        CreateMap<PropertyFilterDto, FilterModel>()
            .ForMember(dest => dest.Filters, opt => opt.MapFrom(src => new Dictionary<string, string>
            {
                { "name", src.Name ?? string.Empty },
                { "address", src.Address ?? string.Empty },
                { "minPrice", src.MinPrice.HasValue ? src.MinPrice.Value.ToString() : string.Empty },
                { "maxPrice", src.MaxPrice.HasValue ? src.MaxPrice.Value.ToString() : string.Empty }
            }));
    }
}