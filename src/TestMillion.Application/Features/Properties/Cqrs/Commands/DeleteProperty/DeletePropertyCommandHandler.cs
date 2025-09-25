using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.Properties.Cqrs.Commands.DeleteProperty;

public class DeletePropertyCommandHandler: UseCaseHandler, IRequestHandler<DeletePropertyCommand, ResultResponse<bool>>
{
  public readonly IPropertyRepository _propertyRepository;
  
  public DeletePropertyCommandHandler(
    IMapper mapper,
    IPropertyRepository propertyRepository)
  {
    _propertyRepository = propertyRepository;
  }
  public async Task<ResultResponse<bool>> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var existingProperty = await _propertyRepository.GetByIdAsync(request.Id);
      if (existingProperty == null)
      {
        return NotFound<bool>($"Property with ID {request.Id} not found.");
      }
      
      var deleted = await _propertyRepository.DeleteAsync(request.Id);
      if (!deleted)
      {
        return Invalid<bool>("Failed to delete Property.");
      }
      return Succeded(true);
    }
    catch (Exception ex)
    {
      return Invalid<bool>($"An error occurred while deleting the Property: {ex.Message}");
    }
  }
}