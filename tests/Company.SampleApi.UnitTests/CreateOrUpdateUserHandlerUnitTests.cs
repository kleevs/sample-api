using Company.SampleApi;
using Moq;
using Company.SampleApi.Contracts;

namespace SampleApi.UnitTests;

public class CreateOrUpdateUserHandlerUnitTests
{
    [Fact]
    public async Task Should_Create_User()
    {
        var users = new Mock<IUserService>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var service = new CreateOrUpdateUserHandler(users.Object, unitOfWork.Object);

        await service.HandleAsync("login", "password", null);

        unitOfWork.Verify(_ => _.SaveChangesAsync(default));
        users.Verify(_ => _.CreateUser("login", "password"));
    }

    [Fact]
    public async Task Should_Update_User()
    {
        var users = new Mock<IUserService>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var service = new CreateOrUpdateUserHandler(users.Object, unitOfWork.Object);

        await service.HandleAsync("login", "newPassword", "password");

        unitOfWork.Verify(_ => _.SaveChangesAsync(default));
        users.Verify(_ => _.UpdateUser("login", "password", "newPassword"));
    }
}