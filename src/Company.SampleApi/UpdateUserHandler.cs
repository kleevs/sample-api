using Company.SampleApi.Contracts;
using Company.SampleApi.Tools;

namespace Company.SampleApi;

public class UpdateUserHandler : IUserUpdateService
{
    private readonly IUserRepository _users;
    private readonly IPasswordValidator _passwordValidator;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserHandler(IUserRepository users, IUnitOfWork unitOfWork, IPasswordValidator passwordValidator)
    {
        _users = users;
        _unitOfWork = unitOfWork;
        _passwordValidator = passwordValidator;
    }

    public async Task UpdateUser(string login, string oldPassword, string password)
    {
        var existedUser = await _users.Where(u => u.Login == login && u.Password == oldPassword).FirstOrDefaultAsync();

        if (existedUser is null)
        {
            throw new Exception($"user or password is invalid");
        }

        if (!_passwordValidator.IsValidPassword(password))
        {
            throw new Exception($"password is invalid");
        }

        var newUser = existedUser with
        {
            Password = password
        };

        await _users.UpdateAsync(newUser);
    }

    public async Task HandleAsync(string login, string oldPassword, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }        
        
        if (string.IsNullOrWhiteSpace(oldPassword))
        {
            throw new ArgumentNullException(nameof(oldPassword));
        }

        if (string.IsNullOrEmpty(login))
        {
            throw new ArgumentNullException(nameof(login));
        }

        await UpdateUser(login, oldPassword, password);
        await _unitOfWork.SaveChangesAsync();
    }
}
