namespace Company.SampleApi.Contracts;

public interface IUserCreateService
{
    Task CreateUser(string login, string password);
}

public interface IUserUpdateService
{
    Task UpdateUser(string login, string oldPassword, string password);
}

public interface IUserService : IUserCreateService, IUserUpdateService
{
}

public interface IPasswordValidator 
{
    bool IsValidPassword(string password);
}