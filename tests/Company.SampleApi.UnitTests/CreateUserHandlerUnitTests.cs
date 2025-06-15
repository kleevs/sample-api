using Company.SampleApi;
using Company.SampleApi.Database;
using Company.SampleApi.Database.UserDb;
using Company.SampleApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace SampleApi.UnitTests;

public class CreateUserHandlerUnitTests
{
    [Fact]
    public async Task Should_Create_User()
    {
        var options = new DbContextOptionsBuilder<SampleApiDbContext>().UseInMemoryDatabase($"{nameof(CreateUserHandlerUnitTests)}_{nameof(Should_Create_User)}").Options;
        var context = new SampleApiDbContext(options);
        var users = new UserRepository(context);
        var passwordValidator = new PasswordValidator();
        var service = new CreateUserHandler(users, context, passwordValidator);

        await service.HandleAsync("login", "password");

        var result = await context.Set<User>().SingleAsync();

        Assert.Equal("login", result.Login);
        Assert.Equal("password", result.Password);
    }
}