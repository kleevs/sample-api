using Company.SampleApi;
using Moq;
using Company.SampleApi.Tools.Testing;
using Company.SampleApi.Contracts;

namespace SampleApi.UnitTests;

public class SearchUsersHandlerUnitTests
{
    [Fact]
    public async Task Should_Return_All_Users()
    {
        var users = new Mock<IUserRepository>().SetupDefault().SetupAddUser("user1").SetupAddUser("user2").SetupAddUser("user3");

        var service = new SearchUsersHandler(users.Object);

        var result = (await service.HandleAsync(default)).ToArray();

        Assert.Equal(3, result.Length);
    }

    [Fact]
    public async Task Should_Return_Just_Bob_User()
    {
        var users = new Mock<IUserRepository>().SetupDefault().SetupAddUser("Marie").SetupAddUser("Bob").SetupAddUser("Dominique");

        var service = new SearchUsersHandler(users.Object);

        var result = (await service.HandleAsync("B")).ToArray();

        Assert.Single(result);
        Assert.Equal("Bob", result.Single().Login);
    }
}