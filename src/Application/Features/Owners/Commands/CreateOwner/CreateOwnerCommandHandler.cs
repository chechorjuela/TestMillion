using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Owners.DTOs.Response;
using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.Owners.Commands.CreateOwner;

public class CreateOwnerCommandHandler : UseCaseHandler, IRequestHandler<CreateOwnerCommand, ResultResponse<OwnerResponseDto>>
{
  private readonly IOwnerRepository _ownerRepository;
  private readonly IMapper _mapper;

  public CreateOwnerCommandHandler(IOwnerRepository ownerRepository, IMapper mapper)
  {
    _ownerRepository = ownerRepository;
    _mapper = mapper;
  }

  public async Task<ResultResponse<OwnerResponseDto>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
  {
    var entity = _mapper.Map<Owner>(request);
    var objectResponse = await _ownerRepository.AddAsync(entity);
    var response = _mapper.Map<OwnerResponseDto>(objectResponse);
    
    return Succeded(response);
  }
}