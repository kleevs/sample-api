using Company.SampleApi.Contracts;
using Company.SampleApi.Entities;
using Moq;

namespace Company.SampleApi.Tools.Testing;

public static class UserMockExtension
{
    public static Mock<IUserRepository> SetupDefault(this Mock<IUserRepository> users) 
    {
        users.SetupQueryable(new List<User>());

        return users;
    }

    public static Mock<IUserRepository> SetupAddUser(this Mock<IUserRepository> users, string login) => users.SetupAddUser(u => u with { Login = login });

    public static Mock<IUserRepository> SetupAddUser(this Mock<IUserRepository> users, Func<User, User> config)
    {
        var list = users.Object.ToList();
        list.Add(config(new User { Login = "Login", Password = "password" }));
        users.SetupQueryable(list);

        return users;
    }
}
