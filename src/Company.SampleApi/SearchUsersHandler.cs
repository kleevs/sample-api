using Company.SampleApi.Contracts;
using Company.SampleApi.Entities;
using Company.SampleApi.Tools;

namespace Company.SampleApi;

public class SearchUsersHandler : UserRequestHandler
{
    public SearchUsersHandler(IUserRepository users) : base(users)
    {
    }

    public async Task<IEnumerable<User>> HandleAsync() => await _users.ToListAsync();
}
