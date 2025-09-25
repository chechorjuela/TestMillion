using AutoMapper;
using Microsoft.Extensions.Logging;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Models;
using TestMillion.Application.Features.PropertyTrace.DTOs.Response;
using TestMillion.Domain.Interfaces;
using TestMillion.Domain.Common.Models;

namespace TestMillion.Application.Features.PropertyTrace.Cqrs.Queries.GetAllPropertyTrace;

public class GetAllPropertyTraceQueryHandler: UseCaseHandler, 
  IRequestHandler<GetAllPropertyTraceQuery, PagedResponse<List<PropertyTraceResponseDto>>>
{
  private readonly ILogger<GetAllPropertyTraceQueryHandler> _logger;
  private readonly IPropertyTraceRepository _propertyTraceRepository;
  private readonly IPropertyRepository _propertyRepository;
  private readonly IOwnerRepository _ownerRepository;
  private readonly IMapper _mapper;

  public GetAllPropertyTraceQueryHandler(
    ILogger<GetAllPropertyTraceQueryHandler> logger,
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

  public async Task<PagedResponse<List<PropertyTraceResponseDto>>> Handle(GetAllPropertyTraceQuery request, CancellationToken cancellationToken)
  {
    try
    {
      _logger.LogInformation("Getting property traces with pagination: {PageNumber}, {PageSize}", 
        request.Pagination.PageNumber, request.Pagination.PageSize);

      var paginationModel = _mapper.Map<PaginationModel>(request.Pagination);
      var filterModel = _mapper.Map<FilterModel>(request.Filter);
      
      _logger.LogInformation("Fetching property traces from repository");
      var (items, total) = await _propertyTraceRepository.GetPagedAsync(paginationModel, filterModel);
      
      if (!items.Any())
      {
        _logger.LogInformation("No property traces found");
        var emptyMeta = new PaginationMetadataDto(0, request.Pagination.PageSize, request.Pagination.PageNumber);
        return PagedResponse<List<PropertyTraceResponseDto>>.Success(new List<PropertyTraceResponseDto>(), "No property traces found", emptyMeta);
      }

      _logger.LogInformation("Mapping {Count} property traces to DTOs", items.Count());
      var result = _mapper.Map<List<PropertyTraceResponseDto>>(items);

      _logger.LogInformation("Loading related properties for property traces");
      foreach (var (dto, entity) in result.Zip(items, (dto, entity) => (dto, entity)))
      {
          var property = await _propertyRepository.GetByIdAsync(entity.IdProperty);
          if (property != null)
          {
              dto.Property = _mapper.Map<Properties.DTOs.Response.PropertyResponseDto>(property);
          }
      }

      var meta = new PaginationMetadataDto(total, request.Pagination.PageSize, request.Pagination.PageNumber);
      _logger.LogInformation("Successfully retrieved {Count} property traces", result.Count);
      return PagedResponse<List<PropertyTraceResponseDto>>.Success(result, "Property traces fetched successfully", meta);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error getting all property traces: {ErrorMessage}", ex.Message);
      if (ex.InnerException != null)
      {
          _logger.LogError(ex.InnerException, "Inner exception: {ErrorMessage}", ex.InnerException.Message);
      }
      return PagedResponse<List<PropertyTraceResponseDto>>.Error($"Error getting all property traces: {ex.Message}");
    }
  }
}
