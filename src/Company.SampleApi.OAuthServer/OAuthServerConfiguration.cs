using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Company.SampleApi.OAuthServer;

public static class OAuthServerConfiguration
{
    public static IServiceCollection AddOAuthServer(this IServiceCollection services) => AddOAuthServer(services, (e) => { });
    public static IServiceCollection AddOAuthServer(this IServiceCollection services, Action<OAuthServerOptions> configBuilder)
    {
        var config = new OAuthServerOptions
        {
            ConfigurationEndpoint = "oauth/.well-known/openid-configuration",
            AuthorizationEndpoint = "oauth/authorize",
            TokenEndpoint = "oauth/token",
            AuthenticationScheme = "oauth_cookie"
        };
        configBuilder(config);
        services.AddHttpContextAccessor();
        services.AddDataProtection();
        services.AddScoped<AuthorizationEndpointHandler>();
        services.AddScoped<TokenEndpointHandler>();
        services.AddScoped<ConfigurationEndpointHandler>();
        services.AddScoped<LoginPageEndpointHandler>();
        services.AddScoped<LoginEndpointHandler>();
        services.AddAuthorization();
        services.AddSingleton(config);
        return services;
    }

    public static T UseOAuthServer<T>(this T app) where T : IEndpointRouteBuilder, IApplicationBuilder
    {
        var config = app.ServiceProvider.GetRequiredService<OAuthServerOptions>();
        var policy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes(config.AuthenticationScheme).RequireAuthenticatedUser().Build();
        app.MapGet(config.AuthorizationEndpoint, (AuthorizationEndpointHandler h) => h.Handle()).RequireAuthorization(policy);
        app.MapGet(config.TokenEndpoint, (TokenEndpointHandler h) => h.Handle()).RequireAuthorization(policy);
        app.MapGet(config.ConfigurationEndpoint, (ConfigurationEndpointHandler h) => h.Handle(config)).AllowAnonymous();
        return app;
    }
}

public class OAuthServerOptions
{
    public required string ConfigurationEndpoint { get; set; }
    public required string AuthorizationEndpoint { get; set; }
    public required string TokenEndpoint { get; set; }
    public required string AuthenticationScheme { get; set; }
}

