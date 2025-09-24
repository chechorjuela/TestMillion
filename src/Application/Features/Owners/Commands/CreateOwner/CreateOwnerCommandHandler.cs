using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.Owners.Commands.CreateOwner;

public class CreateOwnerCommandHandler : UseCaseHandler, IRequestHandler<CreateOwnerCommand, ResultResponse<OwnerResponseDto>>
{
  private readonly IOwnerRepository _ownerRepository;
  private readonly IMapper _mapper;

  public CreateOwnerCommandHandler(IOwnerRepository ownerRepository, IMapper mapper)
  {
    _ownerRepository = ownerRepository;
    _mapper = mapper;
  }

  public async Task<ResultResponse<OwnerResponseDto>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
  {
    try
    {
      var entity = _mapper.Map<Owner>(request);
      
      // Check if owner with same name exists
      var existingOwner = await _ownerRepository.GetByNameAsync(request.Name);
      if (existingOwner != null)
      {
        return Invalid<OwnerResponseDto>("An owner with this name already exists.");
      }
      
      var objectResponse = await _ownerRepository.AddAsync(entity);
      if (objectResponse == null)
      {
        return Invalid<OwnerResponseDto>("Failed to create owner.");
      }
      
      var response = _mapper.Map<OwnerResponseDto>(objectResponse);
      return Created(response);
    }
    catch (Exception ex)
    {
      return Invalid<OwnerResponseDto>($"An error occurred while creating the owner: {ex.Message}");
    }
  }
}