using Company.SampleApi.Contracts;

namespace Company.SampleApi;

public class UpdateUserHandler
{
    private readonly UserService _users;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserHandler(IUserRepository users, IUnitOfWork unitOfWork)
    {
        _users = new UserService(users);
        _unitOfWork = unitOfWork;
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

        await _users.UpdateUser(login, oldPassword, password);
        await _unitOfWork.SaveChangesAsync();
    }
}
