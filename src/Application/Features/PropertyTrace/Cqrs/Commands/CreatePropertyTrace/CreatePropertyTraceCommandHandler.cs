using AutoMapper;
using Microsoft.Extensions.Logging;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.PropertyTrace.Commands.CreatePropertyTrace;

public class CreatePropertyTraceCommandHandler : UseCaseHandler, IRequestHandler<CreatePropertyTraceCommand, ResultResponse<PropertyTraceResponseDto>>
{
    private readonly ILogger<CreatePropertyTraceCommandHandler> _logger;
    private readonly IPropertyTraceRepository _propertyTraceRepository;
    private readonly IMapper _mapper;

    public CreatePropertyTraceCommandHandler(
        ILogger<CreatePropertyTraceCommandHandler> logger,
        IPropertyTraceRepository propertyTraceRepository,
        IMapper mapper)
    {
        _logger = logger;
        _propertyTraceRepository = propertyTraceRepository;
        _mapper = mapper;
    }

    public async Task<ResultResponse<PropertyTraceResponseDto>> Handle(CreatePropertyTraceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var propertyTrace = new Domain.Entities.PropertyTrace(
                request.DateSale,
                request.Name,
                request.Value,
                request.Tax,
                request.IdProperty
            );

            var created = await _propertyTraceRepository.AddAsync(propertyTrace);
            var result = _mapper.Map<PropertyTraceResponseDto>(created);
            return Succeded(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating property trace");
            return Invalid<PropertyTraceResponseDto>("Error creating property trace");
        }
    }
}
