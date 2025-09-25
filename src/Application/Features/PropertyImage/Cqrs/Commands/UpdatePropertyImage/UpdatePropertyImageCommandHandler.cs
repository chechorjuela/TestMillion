using AutoMapper;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;
using Microsoft.Extensions.Logging;
using TestMillion.Application.Features.PropertyImage.Commands.DeletePropertyImage;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.PropertyImage.Commands.UpdatePropertyImage;

public class UpdatePropertyImageCommandHandler : UseCaseHandler, IRequestHandler<UpdatePropertyImageCommand, ResultResponse<PropertyImageResponseDto>>
{
    private readonly ILogger<UpdatePropertyImageCommandHandler> _logger;
    private readonly IPropertyImageRepository _propertyImageRepository;
    private readonly IMapper _mapper;

    public UpdatePropertyImageCommandHandler(
        ILogger<UpdatePropertyImageCommandHandler> logger,
        IPropertyImageRepository propertyImageRepository,
        IMapper mapper)
    {
        _logger = logger;
        _propertyImageRepository = propertyImageRepository;
        _mapper = mapper;
    }

    public async Task<ResultResponse<PropertyImageResponseDto>> Handle(UpdatePropertyImageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingImage = await _propertyImageRepository.GetByIdAsync(request.Id);
            if (existingImage == null)
            {
                return Invalid<PropertyImageResponseDto>("Property image not found");
            }

            _mapper.Map(request, existingImage);

            var response = await _propertyImageRepository.UpdateAsync(existingImage);

            var result = _mapper.Map<PropertyImageResponseDto>(response);
            return Succeded(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating property image");
            return Invalid<PropertyImageResponseDto>("Error updating property image");
        }
    }
}
