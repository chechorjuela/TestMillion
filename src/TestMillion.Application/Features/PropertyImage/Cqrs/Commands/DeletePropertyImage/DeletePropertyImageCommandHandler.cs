using MediatR;
using TestMillion.Application.Common.Response;
using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.PropertyImage.Commands.DeletePropertyImage;
using TestMillion.Domain.Interfaces;

namespace TestMillion.Application.Features.PropertyImage.Cqrs.Commands.DeletePropertyImage;

public class DeletePropertyImageCommandHandler : UseCaseHandler, IRequestHandler<DeletePropertyImageCommand, ResultResponse<bool>>
{
    private readonly IPropertyImageRepository _repository;

    public DeletePropertyImageCommandHandler(IPropertyImageRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultResponse<bool>> Handle(DeletePropertyImageCommand request, CancellationToken cancellationToken)
    {
        var propertyImage = await _repository.GetByIdAsync(request.Id);
        if (propertyImage == null)
        {
            return NotFound<bool>("Property image not found");
        }

        return await _repository.DeleteAsync(request.Id) 
            ? Succeded(true) 
            : Invalid<bool>("Failed to delete property image");
    }
}
