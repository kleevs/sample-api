using Company.SampleApi.Contracts;
using Company.SampleApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Company.SampleApi.Database.UserDb;

internal class UserRepository : ProxyDbSet<User>, IUserRepository
{
    private readonly SampleApiDbContext _context;

    public UserRepository(SampleApiDbContext context) : base(context.Set<User>())
    {
        _context = context;
    }

    public void Add(User user)
    {
        _context.Set<User>().Add(user);
    }

    public void Delete(User user)
    {
        var users = _context.Set<User>();
        var u = users.Where(_ => _.Login == user.Login).FirstOrDefault();
        if (u is not null) 
        {
            users.Remove(u);
        }
    }

    public void Update(User user)
    {
        var users = _context.Set<User>();
        var u = users.Where(_ => _.Login == user.Login).FirstOrDefault();
        if (u is not null) 
        {
            users.Update(u with 
            {
                Password = user.Password
            });
        }
    }
}

public static class UserRepositoryConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(_ => _.Login);
        });
    }

    public static IServiceCollection AddUserRepository(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}