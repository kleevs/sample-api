using Company.SampleApi.Contracts;
using Company.SampleApi.Entities;
using Company.SampleApi.Tools;

namespace Company.SampleApi;

public class UserService
{
    private readonly IUserRepository _users;
    private readonly PasswordValidator _passwordValidator;

    public UserService(IUserRepository users)
    {
        _users = users;
        _passwordValidator = new PasswordValidator();
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

    public async Task<IEnumerable<User>> SearchUser(string? login, string? password)
    {
        var query = _users.AsQueryable();

        if (!string.IsNullOrEmpty(login))
        {
            query = query.Where(_ => _.Login == login);
        }

        if (!string.IsNullOrEmpty(password))
        {
            query = query.Where(_ => _.Password == password);
        }

        return await query.ToListAsync();
    }
}
