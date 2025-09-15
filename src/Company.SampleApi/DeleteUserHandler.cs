using Company.SampleApi.Contracts;
using System.Linq.Async;

namespace Company.SampleApi;

public class DeleteUserHandler
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserHandler(IUserRepository users, IUnitOfWork unitOfWork)
    {
        _users = users;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(string login)
    {
        if (string.IsNullOrEmpty(login))
        {
            throw new ArgumentNullException(nameof(login));
        }

        var existedUser = await _users.Where(u => u.Login == login).FirstOrDefaultAsync();
        if (existedUser is not null)
        {
            await _users.DeleteAsync(existedUser);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
