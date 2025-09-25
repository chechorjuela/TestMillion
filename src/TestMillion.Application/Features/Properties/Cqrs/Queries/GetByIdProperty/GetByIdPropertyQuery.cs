using TestMillion.Application.Common.Response.Result;
using TestMillion.Application.Features.Properties.DTOs.Response;

namespace TestMillion.Application.Features.Properties.Cqrs.Queries.GetByIdProperty;

public class GetByIdPropertyQuery : IRequest<ResultResponse<PropertyResponseDto>>
{
    public string Id { get; }

    public GetByIdPropertyQuery(string id)
    {
        Id = id;
    }
}