using TestMillion.Application.Common.Results;

namespace TestMillion.Application.Common.Response;

public class ValidationErrorResponse
{
    public int Status { get; set; }
    public string Message { get; set; } = "Validation failed";
    public List<ValidationError> Errors { get; set; } = new();
    public DebugInfo? Debug { get; set; }
}

public class ValidationError
{
    public string Field { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class DebugInfo
{
    public string ExceptionType { get; set; } = string.Empty;
    public string StackTrace { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
}