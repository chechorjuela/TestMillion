using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Domain.Interfaces;

public interface IPropertyImageRepository : IBaseRepository<PropertyImage>
{
    Task<IEnumerable<PropertyImage>> GetImagesByPropertyAsync(string propertyId);
    Task<PropertyImage?> GetMainImageAsync(string propertyId);
}