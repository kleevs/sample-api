using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Company.SampleApi.OAuthServer;

public static class OAuthServerConfiguration
{
    public static IServiceCollection AddOAuthServer(this IServiceCollection services)
    {
        var config = new OAuthServerOptions
        {
            ConfigurationEndpoint = "oauth/.well-known/openid-configuration",
            AuthorizationEndpoint = "oauth/authorize",
            TokenEndpoint = "oauth/token",
            LoginPageEndpoint = "oauth/login",
            LoginEndpoint = "oauth/login"
        };
        services.AddHttpContextAccessor();
        services.AddDataProtection();
        services.AddScoped<AuthorizationEndpointHandler>();
        services.AddScoped<TokenEndpointHandler>();
        services.AddScoped<ConfigurationEndpointHandler>();
        services.AddScoped<LoginPageEndpointHandler>();
        services.AddScoped<LoginEndpointHandler>();
        services.AddAuthentication("oauth_cookie").AddCookie("oauth_cookie", c => c.LoginPath = $"/{config.LoginPageEndpoint}");
        services.AddAuthorization();
        services.AddAntiforgery();
        services.AddSingleton(() => config);
        return services;
    }

    public static T UseOAuthServer<T>(this T app) where T : IEndpointRouteBuilder, IApplicationBuilder
    {
        var config = app.ServiceProvider.GetRequiredService<OAuthServerOptions>();
        app.UseAntiforgery();
        app.MapGet(config.LoginPageEndpoint, (LoginPageEndpointHandler h) => h.Handle());
        app.MapPost(config.LoginEndpoint, (LoginEndpointHandler h, LoginPayload payload) => h.Handle(payload));
        app.MapGet(config.AuthorizationEndpoint, (AuthorizationEndpointHandler h) => h.Handle()).RequireAuthorization();
        app.MapGet(config.TokenEndpoint, (TokenEndpointHandler h) => h.Handle()).RequireAuthorization();
        app.MapGet(config.ConfigurationEndpoint, (ConfigurationEndpointHandler h) => h.Handle(config)).AllowAnonymous();
        return app;
    }
}

public class OAuthServerOptions
{
    public required string ConfigurationEndpoint { get; init; }
    public required string AuthorizationEndpoint { get; init; }
    public required string TokenEndpoint { get; init; }
    public required string LoginPageEndpoint { get; init; }
    public required string LoginEndpoint { get; init; }
}

