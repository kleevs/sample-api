namespace Company.SampleApi.Api.Endpoints;

public static class UsersEndpoint
{
    public static IServiceCollection AddUsers(this IServiceCollection services)
    {
        services.AddScoped<SearchUsersHandler>();
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<UpdateUserHandler>();
        services.AddScoped<DeleteUserHandler>();
        services.AddScoped<CreateOrUpdateUserHandler>();
        return services;
    }

    public static IEndpointRouteBuilder MapUsers(this IEndpointRouteBuilder app) 
    {
        app.MapGet("/users", (SearchUsersHandler h, string? login) => h.HandleAsync(login));
        app.MapPut("/users/{login}", (CreateOrUpdateUserHandler h, string login, string password, string? oldPassword) => h.HandleAsync(login, password, oldPassword));
        app.MapPost("/users/new", (CreateUserHandler h, string login, string password) => h.HandleAsync(login, password));
        app.MapDelete("/users/{login}", (DeleteUserHandler h, string login) => h.HandleAsync(login));
        return app;
    }
}