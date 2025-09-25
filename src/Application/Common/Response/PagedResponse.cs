using TestMillion.Application.Common.Models;

namespace TestMillion.Application.Common.Response;

public class PagedResponse<T>
{
    public T Data { get; set; } = default!;
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public PaginationMetadataDto? Metadata { get; set; }

    public static PagedResponse<T> Success(T data, string message, PaginationMetadataDto metadata)
    {
        return new PagedResponse<T>
        {
            Data = data,
            Status = 200,
            Message = message,
            Metadata = metadata
        };
    }

    public static PagedResponse<T> Error(string message, int status = 400)
    {
        return new PagedResponse<T>
        {
            Status = status,
            Message = message
        };
    }
}