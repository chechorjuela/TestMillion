using AutoMapper;
using MediatR;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.PropertyImage.Cqrs.Queries.GetByIdPropertyImage;

public class GetByPropertyImageQueryHandler: UseCaseHandler, IRequestHandler<GetByPropertyImageQuery, ResultResponse<PropertyImageResponseDto>>
{
  private readonly IPropertyImageRepository _repository;
  private readonly IPropertyRepository _propertyRepository;
  private readonly IOwnerRepository _ownerRepository;
  private readonly IMapper _mapper;

  public GetByPropertyImageQueryHandler(
    IPropertyImageRepository repository,
    IPropertyRepository propertyRepository,
    IOwnerRepository ownerRepository,
    IMapper mapper)
  {
    _repository = repository;
    _propertyRepository = propertyRepository;
    _ownerRepository = ownerRepository;
    _mapper = mapper;
  }

  public async Task<ResultResponse<PropertyImageResponseDto>> Handle(GetByPropertyImageQuery request, CancellationToken cancellationToken)
  {
    var propertyImage = await _repository.GetByIdAsync(request.Id);
    if (propertyImage == null)
    {
      return NotFound<PropertyImageResponseDto>("Property image not found");
    }

    var property = await _propertyRepository.GetByIdAsync(propertyImage.IdProperty);
    if (property != null)
    {
    }

    var responseDto = _mapper.Map<PropertyImageResponseDto>(propertyImage);
    responseDto.Property = _mapper.Map<Properties.DTOs.Response.PropertyResponseDto>(property);
    return Succeded(responseDto);
  }
}