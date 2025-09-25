using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.Properties.Cqrs.Commands.UpdateProperty
{
  public class UpdatePropertyCommandHandler : UseCaseHandler,
    IRequestHandler<UpdatePropertyCommand, ResultResponse<PropertyResponseDto>>
  {
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public UpdatePropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
      _propertyRepository = propertyRepository;
      _mapper = mapper;
    }

    public async Task<ResultResponse<PropertyResponseDto>> Handle(UpdatePropertyCommand request,
      CancellationToken cancellationToken)
    {
      try
      {
        var existingProperty = await _propertyRepository.GetByIdAsync(request.Id);
        if (existingProperty == null)
        {
          return NotFound<PropertyResponseDto>($"Property with ID {request.Id} not found.");
        }

        var updatedProperty = _mapper.Map(request, existingProperty);
        var result = await _propertyRepository.UpdateAsync(updatedProperty);
        var response = _mapper.Map<PropertyResponseDto>(result);

        return Succeded(response);
      }
      catch (Exception ex)
      {
        return Invalid<PropertyResponseDto>($"An error occurred while updating the property: {ex.Message}");
      }
    }
  }
}