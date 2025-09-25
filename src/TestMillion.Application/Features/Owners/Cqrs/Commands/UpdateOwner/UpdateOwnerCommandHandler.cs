using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.Owners.Cqrs.Commands.UpdateOwner;

public class UpdateOwnerCommandHandler : UseCaseHandler, IRequestHandler<UpdateOwnerCommand, ResultResponse<OwnerResponseDto>>
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;

    public UpdateOwnerCommandHandler(IOwnerRepository ownerRepository, IMapper mapper)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
    }

    public async Task<ResultResponse<OwnerResponseDto>> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingOwner = await _ownerRepository.GetByIdAsync(request.Id);
            if (existingOwner == null)
            {
                return NotFound<OwnerResponseDto>($"Owner with ID {request.Id} not found.");
            }

            if (existingOwner.Name != request.Name)
            {
                var ownerWithSameName = await _ownerRepository.GetByNameAsync(request.Name);
                if (ownerWithSameName != null && ownerWithSameName.Id != request.Id)
                {
                    return Invalid<OwnerResponseDto>("An owner with this name already exists.");
                }
            }

            existingOwner.Name = request.Name;
            existingOwner.Address = request.Address;
            existingOwner.Photo = request.Photo;
            existingOwner.Birthdate = request.Birthdate;

            try
            {
                var updatedOwner = await _ownerRepository.UpdateAsync(existingOwner);
                var response = _mapper.Map<OwnerResponseDto>(updatedOwner);
                return Succeded(response);
            }
            catch (Exception ex)
            {
                return Invalid<OwnerResponseDto>($"Failed to update owner: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            return Invalid<OwnerResponseDto>($"An error occurred while updating the owner: {ex.Message}");
        }
    }
}
