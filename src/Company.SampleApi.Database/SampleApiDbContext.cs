using Company.SampleApi.Contracts;
using Company.SampleApi.Database.UserDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Company.SampleApi.Database;

public class SampleApiDbContext : DbContext, IUnitOfWork
{
    public SampleApiDbContext(DbContextOptions<SampleApiDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        UserRepositoryConfiguration.Configure(modelBuilder);
    }
}

public static class SampleApiDbContextConfiguration
{
    public static IServiceCollection AddSampleApiDbContext(this IServiceCollection services)
    {
        services.AddUserRepository();
        services.AddScoped<IUnitOfWork>(_ => _.GetRequiredService<SampleApiDbContext>());
        return services.AddDbContext<SampleApiDbContext>(options => options.UseInMemoryDatabase("user"));
    }
}