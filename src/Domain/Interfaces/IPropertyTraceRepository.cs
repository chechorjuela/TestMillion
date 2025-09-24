using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Domain.Interfaces;

public interface IPropertyTraceRepository : IBaseRepository<PropertyTrace>
{
    Task<IEnumerable<PropertyTrace>> GetPropertiesByOwnerAsync(string ownerId);
}