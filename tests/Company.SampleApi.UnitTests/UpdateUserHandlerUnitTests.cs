using Company.SampleApi;
using Company.SampleApi.Entities;
using Moq;
using Company.SampleApi.Tools.Testing;
using Company.SampleApi.Contracts;

namespace SampleApi.UnitTests;

public class UpdateUserHandlerUnitTests
{
    [Fact]
    public async Task Should_Update_User()
    {
        var users = new Mock<IUserRepository>().SetupDefault().SetupAddUser("login");
        var unitOfWork = new Mock<IUnitOfWork>();

        var service = new UpdateUserHandler(users.Object, unitOfWork.Object);

        await service.HandleAsync("login", "password", "newPassword");

        unitOfWork.Verify(_ => _.SaveChangesAsync(default));
        users.Verify(_ => _.UpdateAsync(It.Is<User>(u => u.Password == "newPassword" && u.Login == "login")));
    }

    [Fact]
    public async Task Should_Throw_Error_Because_Invalid_Password()
    {
        var users = new Mock<IUserRepository>().SetupDefault().SetupAddUser("login");
        var unitOfWork = new Mock<IUnitOfWork>();

        var service = new UpdateUserHandler(users.Object, unitOfWork.Object);

        var error = await Assert.ThrowsAsync<Exception>(async () => await service.HandleAsync("login", "password", "123"));

        Assert.Equal("password is invalid", error.Message);
    }

    [Fact]
    public async Task Should_Throw_Error_Because_Users_Is_Empty()
    {
        var users = new Mock<IUserRepository>().SetupDefault();
        var unitOfWork = new Mock<IUnitOfWork>();

        var service = new UpdateUserHandler(users.Object, unitOfWork.Object);

        var error = await Assert.ThrowsAsync<Exception>(async () => await service.HandleAsync("login", "password", "newPassword"));

        Assert.Equal("user or password is invalid", error.Message);
    }
}