using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Application.Features.Properties.Cqrs.Commands.CreateProperty;

public class CreatePropertyCommandHandler : UseCaseHandler, IRequestHandler<CreatePropertyCommand, ResultResponse<PropertyResponseDto>>
{
  private readonly IPropertyRepository _propertyRepository;
  private readonly IMapper _mapper;

  public CreatePropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper)
  {
    _propertyRepository = propertyRepository;
    _mapper = mapper;
  }

  public async Task<ResultResponse<PropertyResponseDto>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var entity = _mapper.Map<Property>(request);
      var objectResponse = await _propertyRepository.AddAsync(entity);
      if (objectResponse == null)
      {
        return Invalid<PropertyResponseDto>("Failed to create property.");
      }

      var response = _mapper.Map<PropertyResponseDto>(objectResponse);
      return Created(response);
    }
    catch (Exception ex)
    {
      return Invalid<PropertyResponseDto>($"An error occurred while creating the property: {ex.Message}");
    }
  }
}