namespace Company.SampleApi.OAuthServer;

public class ConfigurationEndpointHandler
{
    public Configuration Handle(OAuthServerOptions config)
    {
        return new Configuration 
        {
            Authorization_endpoint = $"https://localhost:7126/{config.AuthorizationEndpoint}",
            Token_endpoint = $"https://localhost:7126/{config.TokenEndpoint}",
            Userinfo_endpoint = $"https://localhost:7126/{config.UserinfoEndpoint}"
        };
    }
}

public class Configuration
{
    public required string Authorization_endpoint { get; init; }
    public required string Token_endpoint { get; init; }
    public required string Userinfo_endpoint { get; init; }
}

