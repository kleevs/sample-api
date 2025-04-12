using Company.SampleApi.Contracts;

namespace Company.SampleApi;

public class DeleteUserHandler : UserCommandHandler
{
    public DeleteUserHandler(IUserRepository users, IUnitOfWork unitOfWork) : base(users, unitOfWork)
    {
    }

    public async Task HandleAsync(string login)
    {
        if (string.IsNullOrEmpty(login))
        {
            throw new ArgumentNullException(nameof(login));
        }

        var existedUser = _users.Where(u => u.Login == login).FirstOrDefault();
        if (existedUser is not null)
        {
            _users.Delete(existedUser);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
