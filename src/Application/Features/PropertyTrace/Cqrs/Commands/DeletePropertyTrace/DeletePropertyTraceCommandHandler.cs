using Microsoft.Extensions.Logging;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.PropertyTrace.Commands.DeletePropertyTrace;

public class DeletePropertyTraceCommandHandler : UseCaseHandler, IRequestHandler<DeletePropertyTraceCommand, ResultResponse<bool>>
{
    private readonly ILogger<DeletePropertyTraceCommandHandler> _logger;
    private readonly IPropertyTraceRepository _propertyTraceRepository;

    public DeletePropertyTraceCommandHandler(
        ILogger<DeletePropertyTraceCommandHandler> logger,
        IPropertyTraceRepository propertyTraceRepository)
    {
        _logger = logger;
        _propertyTraceRepository = propertyTraceRepository;
    }

    public async Task<ResultResponse<bool>> Handle(DeletePropertyTraceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existing = await _propertyTraceRepository.GetByIdAsync(request.Id);
            if (existing == null)
            {
                return NotFound<bool>("Property trace not found");
            }

            await _propertyTraceRepository.DeleteAsync(existing.Id);
            return Succeded(true, "Property trace deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting property trace");
            return Invalid<bool>("Error deleting property trace");
        }
    }
}
