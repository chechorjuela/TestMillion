using AutoMapper;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.DTOs.Response;
using TestMillion.Domain.Interfaces;
using MediatR;

namespace TestMillion.Application.Features.PropertyImage.Cqrs.Commands.CreatePropertyImage;

public class CreatePropertyImageCommandHandler : UseCaseHandler, IRequestHandler<CreatePropertyImageCommand, ResultResponse<PropertyImageResponseDto>>
{
    private readonly IPropertyImageRepository _propertyImageRepository;
    private readonly IMapper _mapper;

    public CreatePropertyImageCommandHandler(
        IPropertyImageRepository propertyImageRepository,
        IMapper mapper)
    {
        _propertyImageRepository = propertyImageRepository;
        _mapper = mapper;
    }

    public async Task<ResultResponse<PropertyImageResponseDto>> Handle(CreatePropertyImageCommand request, CancellationToken cancellationToken)
    {
        var propertyImage = _mapper.Map<Domain.Entities.PropertyImage>(request);
        await _propertyImageRepository.AddAsync(propertyImage);

        var response = _mapper.Map<PropertyImageResponseDto>(propertyImage);
        return Succeded(response);
    }
}
