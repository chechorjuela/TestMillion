using AutoMapper;
using TestMillion.Application.DTOs;
using TestMillion.Application.Features.Owners.Commands.CreateOwner;
using TestMillion.Application.Features.Owners.DTOs.Request;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Domain.Entities;

namespace TestMillion.Application.Common.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<CreateOwnerRequestDto, CreateOwnerCommand>();
    CreateMap<CreateOwnerCommand, Owner>();
    CreateMap<Owner, OwnerResponseDto>();
  }
}