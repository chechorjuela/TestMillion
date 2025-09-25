using AutoMapper;
using Microsoft.Extensions.Logging;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetByPropertyTrace;

public class GetByPropertyTraceQueryHandler : UseCaseHandler, IRequestHandler<GetByPropertyTraceQuery, ResultResponse<PropertyTraceResponseDto>>
{
    private readonly ILogger<GetByPropertyTraceQueryHandler> _logger;
    private readonly IPropertyTraceRepository _propertyTraceRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;

    public GetByPropertyTraceQueryHandler(
        ILogger<GetByPropertyTraceQueryHandler> logger,
        IPropertyTraceRepository propertyTraceRepository,
        IPropertyRepository propertyRepository,
        IOwnerRepository ownerRepository,
        IMapper mapper)
    {
        _logger = logger;
        _propertyTraceRepository = propertyTraceRepository;
        _propertyRepository = propertyRepository;
        _ownerRepository = ownerRepository;
        _mapper = mapper;
    }

    public async Task<ResultResponse<PropertyTraceResponseDto>> Handle(GetByPropertyTraceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var propertyTrace = await _propertyTraceRepository.GetByIdAsync(request.Id);
            if (propertyTrace == null)
            {
                return NotFound<PropertyTraceResponseDto>("Property trace not found");
            }

            var property = await _propertyRepository.GetByIdAsync(propertyTrace.IdProperty);
            if (property != null)
            {
            }

            var result = _mapper.Map<PropertyTraceResponseDto>(propertyTrace);
            result.Property = _mapper.Map<Properties.DTOs.Response.PropertyResponseDto>(property);
            return Succeded(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting property trace by id");
            return Invalid<PropertyTraceResponseDto>("Error getting property trace");
        }
    }
}
