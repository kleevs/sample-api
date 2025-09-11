using Microsoft.AspNetCore.Http;

namespace Company.SampleApi.OAuthServer;

public class ConfigurationEndpointHandler
{
    private readonly HttpRequest _request;

    public ConfigurationEndpointHandler(IHttpContextAccessor httpRequestAccessor)
    {
        _request = httpRequestAccessor.HttpContext!.Request;
    }

    public Configuration Handle(OAuthServerOptions config)
    {
        string baseUrl = $"{_request.Scheme}://{_request.Host}{_request.PathBase.Value?.Trim('/')}";

        return new Configuration 
        {
            Authorization_endpoint = $"{baseUrl}/{config.AuthorizationEndpoint}",
            Token_endpoint = $"{baseUrl}/{config.TokenEndpoint}"
        };
    }
}

public class Configuration
{
    public required string Authorization_endpoint { get; init; }
    public required string Token_endpoint { get; init; }
}

