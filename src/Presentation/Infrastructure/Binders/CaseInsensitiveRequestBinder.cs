using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TestMillion.Presentation.Infrastructure.Binders;

public class CaseInsensitiveRequestBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        using var reader = new StreamReader(bindingContext.HttpContext.Request.Body);
        var body = await reader.ReadToEndAsync();

        if (string.IsNullOrEmpty(body))
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        try
        {
            var model = JsonSerializer.Deserialize(body, bindingContext.ModelType, options);
            bindingContext.Result = ModelBindingResult.Success(model);
        }
        catch
        {
            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}