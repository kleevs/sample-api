using Company.SampleApi.Contracts;

namespace Company.SampleApi;

public class UserRequestHandler
{
    protected readonly IUserRepository _users;

    public UserRequestHandler(IUserRepository users)
    {
        _users = users;
    }
}
