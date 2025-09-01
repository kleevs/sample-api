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
                case "code_verifier": payload = payload with { Code_verifier = value }; break;
                case "client_id" : payload = payload with { Client_id = value }; break;
                case "redirect_uri": payload = payload with { Redirect_uri = value }; break;
            }
        }

        if (string.IsNullOrEmpty(payload.Code)) 
        {
            throw new Exception();
        }

        if (string.IsNullOrEmpty(payload.Code_verifier))
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

        using var sha256 = SHA256.Create();
        var codeChallenge = Base64UrlTextEncoder.Encode(sha256.ComputeHash(Encoding.ASCII.GetBytes(payload.Code_verifier)));

        if (codeChallenge != authCode.CodeChallenge) 
        {
            throw new Exception(); 
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
        {
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
        );

        var accesstoken = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenResponse
        {
            Access_Token = accesstoken,
            Refresh_token = string.Empty,
            Token_type = "Bearer",
            Expires_in = DateTimeOffset.Now.AddMinutes(15).ToUnixTimeSeconds(),
            Scope = authCode.Scope ?? string.Empty
        };
    }
}

public record TokenPayload 
{
    public string? Grant_type { get; init; }
    public string? Redirect_uri { get; init; }
    public string? Code { get; init; }
    public string? Code_verifier { get; init; }
    public string? Client_id { get; init; }
}

public record TokenResponse
{
    public required string Access_Token { get; init; }
    public required string Token_type { get; init; }
    public required string Refresh_token { get; init; }
    public long Expires_in { get; init; }
    public required string Scope { get; init; }
}