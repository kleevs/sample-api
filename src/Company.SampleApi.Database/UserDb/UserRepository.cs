using Company.SampleApi.Contracts;
using Company.SampleApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Company.SampleApi.Database.UserDb;

public class UserRepository : ProxyDbSet<User>, IUserRepository
{
    private readonly SampleApiDbContext _context;

    public UserRepository(SampleApiDbContext context) : base(context.Set<User>().AsNoTracking())
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        await _context.Set<User>().AddAsync(user);
    }

    public async Task DeleteAsync(User user)
    {
        var users = _context.Set<User>();
        var u = await users.Where(_ => _.Login == user.Login).FirstOrDefaultAsync();
        if (u is not null) 
        {
            users.Remove(u);
        }
    }

    public async Task UpdateAsync(User user)
    {
        var users = _context.Set<User>();
        var u = await users.AsNoTracking().Where(_ => _.Login == user.Login).FirstOrDefaultAsync();
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
        services.AddScoped<IQueryable<User>>(_ => _.GetRequiredService<IUserRepository>());
        return services;
    }
}