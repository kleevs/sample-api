using Company.SampleApi.Contracts;

namespace Company.SampleApi;

public class UserCommandHandler
{
    protected readonly IUserRepository _users;
    protected readonly IUnitOfWork _unitOfWork;

    public UserCommandHandler(IUserRepository users, IUnitOfWork unitOfWork)
    {
        _users = users;
        _unitOfWork = unitOfWork;
    }
}
