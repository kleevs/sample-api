using Microsoft.AspNetCore.Http;

namespace Company.SampleApi.OAuthServer;

public class LoginEndpointHandler
{
    public void Handle(LoginPayload payload) 
    {
    }
}

public class LoginPayload 
{
    public string? Login { get; init; }
    public string? Password { get; init; }
}
