using AutoMapper;
using TestMillion.Application.Features.Owners.Commands.CreateOwner;
using TestMillion.Application.Features.Owners.Commands.UpdateOwner;
using TestMillion.Application.Features.Owners.DTOs.Request;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Domain.Entities;

namespace TestMillion.Application.Features.Owners.Mappings;

public class OwnerMappingProfile : Profile
{
    public OwnerMappingProfile()
    {
        // Create mappings
        CreateMap<CreateOwnerRequestDto, CreateOwnerCommand>();
        CreateMap<CreateOwnerCommand, Owner>();
        
        // Update mappings
        CreateMap<UpdateOwnerRequestDto, UpdateOwnerCommand>();
        CreateMap<UpdateOwnerCommand, Owner>();
        
        // Response mappings
        CreateMap<Owner, OwnerResponseDto>();
    }
}