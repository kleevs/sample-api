using Company.SampleApi.Entities;

namespace Company.SampleApi.Contracts;

public interface IUserRepository : IQueryable<User>
{
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}
