using Company.SampleApi.Contracts;
using Company.SampleApi.Entities;
using Company.SampleApi.Tools;

namespace Company.SampleApi;

public class CreateUserHandler : IUserCreateService
{
    private readonly IUserRepository _users;
    private readonly IPasswordValidator _passwordValidator;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserHandler(IUserRepository users, IUnitOfWork unitOfWork, IPasswordValidator passwordValidator)
    {
        _users = users;
        _unitOfWork = unitOfWork;
        _passwordValidator = passwordValidator;
    }

    public async Task CreateUser(string login, string password)
    {
        var existedUser = await _users.Where(u => u.Login == login).FirstOrDefaultAsync();

        if (existedUser is not null)
        {
            throw new Exception("login already exist");
        }

        if (!_passwordValidator.IsValidPassword(password)) 
        {
            throw new Exception("password is invalid");
        }

        var newUser = new User
        {
            Login = login,
            Password = password
        };

        await _users.AddAsync(newUser);
    }

    public async Task HandleAsync(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        if (string.IsNullOrEmpty(login))
        {
            throw new ArgumentNullException(nameof(login));
        }

        await CreateUser(login, password);
        await _unitOfWork.SaveChangesAsync();
    }
}
