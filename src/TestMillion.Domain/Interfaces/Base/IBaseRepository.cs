using System.Linq.Expressions;
using TestMillion.Domain.Common.Entities;
using TestMillion.Domain.Common.Models;

namespace TestMillion.Domain.Interfaces.Base;

public interface IBaseRepository<T> where T : IEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(PaginationModel pagination);
    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(PaginationModel pagination, FilterModel filter);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetByIdAsync(string id);
    Task<T> AddAsync(T entity);
    Task<T?> UpdateAsync(T entity);
    Task<bool> DeleteAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null);
}