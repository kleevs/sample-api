using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Company.SampleApi.OAuthServer;

public class TokenEndpointHandler
{
    private readonly HttpRequest _httpRequest;
    private readonly IDataProtector _dataProtector;
    private readonly string _jwtKey = "azerty1234azerty1234azerty1234azerty1234";
    private readonly string _audience = "audience";
    private readonly string _issuer = "issuer";
    private readonly string[] _authorizedGrantType = ["authorization_code", "refresh_token"];

    public TokenEndpointHandler(IHttpContextAccessor httpRequestAccessor, IDataProtectionProvider dataProtectionProvider)
    {
        _httpRequest = httpRequestAccessor.HttpContext!.Request;
        _dataProtector = dataProtectionProvider.CreateProtector("oauth");
    }

    public async Task<TokenResponse> Handle()
    {
        var body = await _httpRequest.BodyReader.ReadAsync();
        var content = Encoding.UTF8.GetString(body.Buffer) ?? string.Empty;
        var payload = new TokenPayload();
        foreach (var str in content.Split("&")) 
        {
            var splitted = str.Split('=');
            var key = splitted[0];
            var value = splitted[1];
            switch (key) 
            {
                case "grant_type" : payload = payload with { Grant_type = value }; break;
                case "code" : payload = payload with { Code = value }; break;
                case "refresh_token": payload = payload with { Code = value }; break;
                case "code_verifier": payload = payload with { Code_verifier = value }; break;
                case "client_id" : payload = payload with { Client_id = value }; break;
            }
        }

        if (!_authorizedGrantType.Contains(payload.Grant_type)) 
        {
            throw new Exception();
        }

        if (string.IsNullOrEmpty(payload.Code)) 
        {
            throw new Exception();
        }

        if (string.IsNullOrEmpty(payload.Client_id))
        {
            throw new Exception();
        }

        var authCode = JsonSerializer.Deserialize<AuthCode>(_dataProtector.Unprotect(payload.Code));

        if (authCode is null) 
        {
            throw new Exception();
        }

        if (payload.Grant_type == "authorization_code")
        {
            if (string.IsNullOrEmpty(payload.Code_verifier))
            {
                throw new Exception();
            }

            using var sha256 = SHA256.Create();
            var codeChallenge = Base64UrlTextEncoder.Encode(sha256.ComputeHash(Encoding.ASCII.GetBytes(payload.Code_verifier)));

            if (codeChallenge != authCode.CodeChallenge)
            {
                throw new Exception();
            }
        }

        if (DateTime.Now > authCode.Expiry) 
        {
            throw new Exception();
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
        {
            new Claim("sub", "me"), 
            new Claim(ClaimTypes.Upn, authCode.Login)
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
        );

        var profile = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        var accesstoken = new JwtSecurityTokenHandler().WriteToken(token);
        var idtoken = new JwtSecurityTokenHandler().WriteToken(profile);
        var refreshToken = _dataProtector.Protect(JsonSerializer.Serialize(authCode with { Expiry = DateTime.Now.AddHours(1)}));

        return new TokenResponse
        {
            Access_token = accesstoken,
            Id_token = idtoken,
            Refresh_token = refreshToken,
            Token_type = "Bearer",
            Expires_in = 900,
            Scope = authCode.Scope ?? string.Empty
        };
    }
}

public record TokenPayload 
{
    public string? Grant_type { get; init; }
    public string? Code { get; init; }
    public string? Code_verifier { get; init; }
    public string? Client_id { get; init; }
}

public record TokenResponse
{
    public required string Access_token { get; init; }
    public required string Id_token { get; init; }
    public required string Token_type { get; init; }
    public required string Refresh_token { get; init; }
    public long Expires_in { get; init; }
    public required string Scope { get; init; }
}