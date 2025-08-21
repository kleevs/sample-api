using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Web;

namespace Company.SampleApi.OAuthServer;

public class AuthorizationEndpointHandler
{
    private readonly HttpRequest _httpRequest;
    private readonly IDataProtector _dataProtector;

    public AuthorizationEndpointHandler(IHttpContextAccessor httpRequestAccessor, IDataProtectionProvider dataProtectionProvider)
    {
        _httpRequest = httpRequestAccessor.HttpContext!.Request;
        _dataProtector = dataProtectionProvider.CreateProtector("oauth");
    }

    public IResult Handle()
    {
        _httpRequest.Query.TryGetValue("response_type", out var responseType);
        _httpRequest.Query.TryGetValue("client_id", out var clientId);
        _httpRequest.Query.TryGetValue("code_challenge", out var codeChallenge);
        _httpRequest.Query.TryGetValue("code_challenge_method", out var codeChallengeMethod);
        _httpRequest.Query.TryGetValue("redirect_uri", out var redirectUri);
        _httpRequest.Query.TryGetValue("scope", out var scope);
        _httpRequest.Query.TryGetValue("state", out var state);

        var authCode = new AuthCode
        {
            ClientId = clientId,
            CodeChallenge = codeChallenge,
            CodeChallengeMethod = codeChallengeMethod,
            RedirectUri = redirectUri,
            Expiry = DateTime.Now.AddMinutes(5)
        };

        var code = _dataProtector.Protect(JsonSerializer.Serialize(authCode));

        return Results.Redirect($"{redirectUri}?code={code}&state={state}&iss={HttpUtility.UrlEncode("MyServer")}");
    }
}

