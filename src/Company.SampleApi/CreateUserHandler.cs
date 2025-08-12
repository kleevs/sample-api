using Company.SampleApi.Contracts;

namespace Company.SampleApi;

public class CreateUserHandler
{
    private readonly UserService _users;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserHandler(IUserRepository users, IUnitOfWork unitOfWork)
    {
        _users = new UserService(users);
        _unitOfWork = unitOfWork;
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

        await _users.CreateUser(login, password);
        await _unitOfWork.SaveChangesAsync();
    }
}
