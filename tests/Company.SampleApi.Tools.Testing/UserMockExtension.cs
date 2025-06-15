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

    public static Mock<IUserRepository> SetupWithOneUser(this Mock<IUserRepository> users)
    {
        users.SetupQueryable(new List<User>
        {
            new User { Login = "login", Password = "password" }
        });

        return users;
    }
}
