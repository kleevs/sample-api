using Company.SampleApi.Contracts;

namespace Company.SampleApi;

public class UserService : IUserService
{
    private readonly IUserCreateService _newUsers;
    private readonly IUserUpdateService _users;

    public UserService(IUserCreateService newUsers, IUserUpdateService users)
    {
        _newUsers = newUsers;
        _users = users;
    }

    public Task CreateUser(string login, string password) => _newUsers.CreateUser(login, password);

    public Task UpdateUser(string login, string oldPassword, string password) => _users.UpdateUser(login, oldPassword, password);
}
