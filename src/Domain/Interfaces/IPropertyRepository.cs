using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Domain.Interfaces;

public interface IPropertyRepository : IBaseRepository<Property>
{
    Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(string ownerId);
    Task<bool> IsCodeInternalUniqueAsync(string codeInternal, string? excludeId = null);
}