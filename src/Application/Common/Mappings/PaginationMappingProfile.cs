using AutoMapper;
using TestMillion.Application.Common.Models;
using TestMillion.Domain.Common.Models;

namespace TestMillion.Application.Common.Mappings;

public class PaginationMappingProfile : Profile
{
    public PaginationMappingProfile()
    {
        CreateMap<PaginationRequestDto, PaginationModel>();
        CreateMap<FilterRequestDto, FilterModel>();
        CreateMap<PaginatedFilterRequestDto, PaginatedFilterModel>();
    }
}