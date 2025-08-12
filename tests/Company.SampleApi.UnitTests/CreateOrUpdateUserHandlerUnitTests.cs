using Company.SampleApi;
using Moq;
using Company.SampleApi.Contracts;
using Company.SampleApi.Entities;
using Company.SampleApi.Tools.Testing;

namespace SampleApi.UnitTests;

public class CreateOrUpdateUserHandlerUnitTests
{
    [Fact]
    public async Task Should_Create_User()
    {
        var users = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        users.SetupDefault();

        var service = new CreateOrUpdateUserHandler(users.Object, unitOfWork.Object);

        await service.HandleAsync("login", "password", null);

        unitOfWork.Verify(_ => _.SaveChangesAsync(default));
        users.Verify(_ => _.AddAsync(It.Is<User>((user) => user.Login == "login" && user.Password == "password")));
    }

    [Fact]
    public async Task Should_Update_User()
    {
        var users = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        users.SetupDefault().SetupAddUser("login");

        var service = new CreateOrUpdateUserHandler(users.Object, unitOfWork.Object);

        await service.HandleAsync("login", "newPassword", "password");

        unitOfWork.Verify(_ => _.SaveChangesAsync(default));
        users.Verify(_ => _.UpdateAsync(It.Is<User>(user => user.Login == "login" && user.Password == "newPassword")));
    }
}