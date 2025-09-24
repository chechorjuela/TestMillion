using TestMillion.Application.Common.Commands;
using TestMillion.Application.Features.Properties.DTOs.Response;
using TestMillion.Application.Properties.DTOs.Request;

namespace TestMillion.Application.Properties.Commands.CreateProperty;

public class CreatePropertyCommand : ICommand<PropertyResponseDto>
{
    public CreatePropertyRequestDto PropertyRequest { get; }

    public CreatePropertyCommand(CreatePropertyRequestDto propertyRequest)
    {
        PropertyRequest = propertyRequest;
    }
}
