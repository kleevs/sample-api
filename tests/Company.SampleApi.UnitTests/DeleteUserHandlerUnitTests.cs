using Company.SampleApi;
using Moq;
using Company.SampleApi.Contracts;
using Company.SampleApi.Tools.Testing;
using Company.SampleApi.Entities;

namespace SampleApi.UnitTests;

public class DeleteUserHandlerUnitTests
{
    [Fact]
    public async Task Should_Delete_User()
    {
        var users = new Mock<IUserRepository>().SetupDefault().SetupAddUser("login");
        var unitOfWork = new Mock<IUnitOfWork>();

        var service = new DeleteUserHandler(users.Object, unitOfWork.Object);

        await service.HandleAsync("login");

        unitOfWork.Verify(_ => _.SaveChangesAsync(default));
        users.Verify(_ => _.DeleteAsync(It.Is<User>(u => u.Login == "login")));
    }
}