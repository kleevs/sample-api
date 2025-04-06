
namespace Company.SampleApi.Api.Endpoints;

public static class UsersEndpoint
{
    public static IEndpointRouteBuilder MapUsers(this IEndpointRouteBuilder app) 
    {
        app.MapGet("/users", UserService.ToList);
        app.MapPut("/users/{login}", UserService.Create);
        app.MapPost("/users/{login}", UserService.Update);
        app.MapDelete("/users/{login}", UserService.Delete);
        return app;
    }
}