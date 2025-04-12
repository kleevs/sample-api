
namespace Company.SampleApi.Api.Endpoints;

public static class UsersEndpoint
{
    public static IServiceCollection AddUsersHandler(this IServiceCollection services)
    {
        services.AddScoped<SearchUsersHandler>();
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<UpdateUserHandler>();
        services.AddScoped<DeleteUserHandler>();
        return services;
    }

    public static IEndpointRouteBuilder MapUsers(this IEndpointRouteBuilder app) 
    {
        app.MapGet("/users", (SearchUsersHandler h) => h.HandleAsync());
        app.MapPut("/users/{login}", (CreateUserHandler h, string login, string password) => h.HandleAsync(login, password));
        app.MapPost("/users/{login}", (UpdateUserHandler h, string login, string password) => h.HandleAsync(login, password));
        app.MapDelete("/users/{login}", (DeleteUserHandler h, string login) => h.HandleAsync(login));
        return app;
    }
}