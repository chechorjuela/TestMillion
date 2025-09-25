using AutoMapper;
using Microsoft.Extensions.Logging;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.Commands.UpdatePropertyTrace;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.PropertyTrace.Commands.UpdatePropertyTrace;

public class UpdatePropertyTraceCommandHandler : UseCaseHandler, IRequestHandler<UpdatePropertyTraceCommand, ResultResponse<PropertyTraceResponseDto>>
{
    private readonly ILogger<UpdatePropertyTraceCommandHandler> _logger;
    private readonly IPropertyTraceRepository _propertyTraceRepository;
    private readonly IMapper _mapper;

    public UpdatePropertyTraceCommandHandler(
        ILogger<UpdatePropertyTraceCommandHandler> logger,
        IPropertyTraceRepository propertyTraceRepository,
        IMapper mapper)
    {
        _logger = logger;
        _propertyTraceRepository = propertyTraceRepository;
        _mapper = mapper;
    }

    public async Task<ResultResponse<PropertyTraceResponseDto>> Handle(UpdatePropertyTraceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existing = await _propertyTraceRepository.GetByIdAsync(request.Id!);
            if (existing == null)
            {
                return Invalid<PropertyTraceResponseDto>("Property trace not found");
            }

            _mapper.Map(request, existing);

            var updated = await _propertyTraceRepository.UpdateAsync(existing);
            var result = _mapper.Map<PropertyTraceResponseDto>(updated);
            return Succeded(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating property trace");
            return Invalid<PropertyTraceResponseDto>("Error updating property trace");
        }
    }
}
