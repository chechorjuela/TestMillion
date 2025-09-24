using MediatR;
using Microsoft.Extensions.Logging;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.Owners.Commands.DeleteOwner;

public class DeleteOwnerCommandHandler : UseCaseHandler, IRequestHandler<DeleteOwnerCommand, ResultResponse<bool>>
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly ILogger<DeleteOwnerCommandHandler> _logger;

    public DeleteOwnerCommandHandler(IOwnerRepository ownerRepository, ILogger<DeleteOwnerCommandHandler> logger)
    {
        _ownerRepository = ownerRepository;
        _logger = logger;
    }

    public async Task<ResultResponse<bool>> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting delete operation for owner with ID: {OwnerId}", request.Id);
        try
        {
            _logger.LogDebug("Checking if owner exists with ID: {OwnerId}", request.Id);
            var existingOwner = await _ownerRepository.GetByIdAsync(request.Id);
            if (existingOwner == null)
            {
                _logger.LogWarning("Owner not found with ID: {OwnerId}", request.Id);
                return NotFound<bool>($"Owner with ID {request.Id} not found.");
            }
            _logger.LogDebug("Owner found with ID: {OwnerId}, Name: {OwnerName}", request.Id, existingOwner.Name);

            _logger.LogDebug("Checking if owner {OwnerId} has associated properties", request.Id);
      
            // Delete owner
            _logger.LogInformation("Attempting to delete owner {OwnerId}", request.Id);
            var deleted = await _ownerRepository.DeleteAsync(request.Id);
            if (!deleted)
            {
                _logger.LogError("Failed to delete owner {OwnerId}", request.Id);
                return Invalid<bool>("Failed to delete owner.");
            }

            _logger.LogInformation("Successfully deleted owner {OwnerId}", request.Id);
            return Succeded(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting owner {OwnerId}: {ErrorMessage}", request.Id, ex.Message);
            return Invalid<bool>($"An error occurred while deleting the owner: {ex.Message}");
        }
    }
}