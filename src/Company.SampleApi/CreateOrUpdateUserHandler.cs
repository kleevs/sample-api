using Company.SampleApi.Contracts;

namespace Company.SampleApi;

public class CreateOrUpdateUserHandler
{
    private readonly UserService _users;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrUpdateUserHandler(IUserRepository users, IUnitOfWork unitOfWork)
    {
        _users = new UserService(users);
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(string login, string password, string? oldPassword)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        if (string.IsNullOrEmpty(login))
        {
            throw new ArgumentNullException(nameof(login));
        }

        if (string.IsNullOrEmpty(oldPassword)) 
        {
            await _users.CreateUser(login, password);
        }
        else 
        {
            await _users.UpdateUser(login, oldPassword, password);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
