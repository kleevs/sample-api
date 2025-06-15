using Company.SampleApi;
using Company.SampleApi.Entities;
using Moq;

namespace SampleApi.UnitTests;

public class UpdateUserHandlerUnitTests
{
    [Fact]
    public async Task Should_Update_User()
    {
        var usersList = new List<User> 
        {
            new User { Login = "login", Password = "password" }
        }.AsQueryable();
        var users = new Mock<Company.SampleApi.Contracts.IUserRepository>();
        users.Setup(_ => _.ElementType).Returns(usersList.ElementType);
        users.Setup(_ => _.Expression).Returns(usersList.Expression);
        users.Setup(_ => _.Provider).Returns(usersList.Provider);
        users.Setup(_ => _.GetEnumerator()).Returns(usersList.GetEnumerator());
        var passwordValidator = new Mock<Company.SampleApi.Contracts.IPasswordValidator>();
        passwordValidator.Setup(_ => _.IsValidPassword(It.IsAny<string>())).Returns(true);
        var unitOfWork = new Mock<Company.SampleApi.Contracts.IUnitOfWork>();

        var service = new UpdateUserHandler(users.Object, unitOfWork.Object, passwordValidator.Object);

        await service.HandleAsync("login", "password", "newPassword");

        unitOfWork.Verify(_ => _.SaveChangesAsync(default));
        passwordValidator.Verify(_ => _.IsValidPassword("newPassword"));
        users.Setup(_ => _.UpdateAsync(It.Is<User>(u => u.Password == "newPassword" && u.Login == "login")));
    }
}