namespace Company.SampleApi.OAuthServer;

public class ConfigurationEndpointHandler
{
    public Configuration Handle(OAuthServerOptions config)
    {
        return new Configuration 
        {
            Authorization_endpoint = $"https://localhost:7126/{config.AuthorizationEndpoint}"
        };
    }
}

public class Configuration
{
    public required string Authorization_endpoint { get; init; }
}

