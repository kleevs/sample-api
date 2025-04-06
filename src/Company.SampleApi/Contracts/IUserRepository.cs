using Company.SampleApi.Entities;

namespace Company.SampleApi.Contracts;

public interface IUserRepository : IQueryable<User>
{
    void Add(User user);
    void Update(User user);
    void Delete(User user);
}
