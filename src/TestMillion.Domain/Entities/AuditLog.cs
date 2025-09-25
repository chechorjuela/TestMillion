using System.Text.Json;
using TestMillion.Domain.Common.Entities;

namespace TestMillion.Domain.Entities;

public class AuditLog : IEntity
{
    public string Id { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public Dictionary<string, object?> Changes { get; set; } = new();
    public string? AdditionalInfo { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}