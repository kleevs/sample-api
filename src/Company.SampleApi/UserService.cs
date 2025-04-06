using Company.SampleApi.Contracts;
using Company.SampleApi.Entities;
using Company.SampleApi.Tools;

namespace Company.SampleApi;

public static class UserService
{
    public static async Task<IEnumerable<User>> ToList(IUserRepository users) => await users.ToListAsync();

    public static async Task Create(IUserRepository users, IUnitOfWork unitOfWork, string login, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        if (string.IsNullOrEmpty(login))
        {
            throw new ArgumentNullException(nameof(login));
        }

        var existedUser = users.Where(u => u.Login == login).FirstOrDefault();
        var newUser = (existedUser ?? new User { Login = login }) with
        {
            Password = password
        };

        if (existedUser is not null)
        {
            users.Update(newUser);
        }
        else
        {
            users.Add(newUser);
        }

        await unitOfWork.SaveChangesAsync();
    }

    public static void Update(IUserRepository users, IUnitOfWork unitOfWork, string login, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        if (string.IsNullOrEmpty(login))
        {
            throw new ArgumentNullException(nameof(login));
        }

        var existedUser = users.Where(u => u.Login == login).FirstOrDefault();
        var newUser = (existedUser ?? new User { Login = login }) with
        {
            Password = password
        };

        if (existedUser is not null)
        {
            users.Update(newUser);
        }
        else
        {
            users.Add(newUser);
        }

        unitOfWork.SaveChanges();
    }

    public static async Task Delete(IUserRepository users, IUnitOfWork unitOfWork, string login)
    {
        if (string.IsNullOrEmpty(login))
        {
            throw new ArgumentNullException(nameof(login));
        }

        var existedUser = users.Where(u => u.Login == login).FirstOrDefault();
        if (existedUser is not null)
        {
            users.Delete(existedUser);
        }

        await unitOfWork.SaveChangesAsync();
    }
}
