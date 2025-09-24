using TestMillion.Domain.Entities;
using TestMillion.Domain.Interfaces.Base;

namespace TestMillion.Domain.Interfaces;

public interface IOwnerRepository : IBaseRepository<Owner>
{
    Task<bool> HasPropertiesAsync(string ownerId);
}