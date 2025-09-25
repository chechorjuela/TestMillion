using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.Properties.Cqrs.Queries.GetByIdProperty;

public class GetByIdPropertyQueryHandler : UseCaseHandler,
    IRequestHandler<GetByIdPropertyQuery, ResultResponse<PropertyResponseDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;

    public GetByIdPropertyQueryHandler(
        IPropertyRepository propertyRepository,
        IOwnerRepository ownerRepository,
        IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _ownerRepository = ownerRepository;
        _mapper = mapper;
    }

    public async Task<ResultResponse<PropertyResponseDto>> Handle(GetByIdPropertyQuery request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.Id);
        if (property == null)
        {
            return NotFound<PropertyResponseDto>("Property not found");
        }


        var response = _mapper.Map<PropertyResponseDto>(property);
        return Succeded(response);
    }
}