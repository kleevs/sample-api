using Company.SampleApi.Contracts;
using Company.SampleApi.Entities;

namespace Company.SampleApi;

public class CreateUserHandler : UserCommandHandler
{
    public CreateUserHandler(IUserRepository users, IUnitOfWork unitOfWork) : base(users, unitOfWork)
    {
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

        var existedUser = _users.Where(u => u.Login == login).FirstOrDefault();
        var newUser = (existedUser ?? new User { Login = login }) with
        {
            Password = password
        };

        if (existedUser is not null)
        {
            _users.Update(newUser);
        }
        else
        {
            _users.Add(newUser);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
