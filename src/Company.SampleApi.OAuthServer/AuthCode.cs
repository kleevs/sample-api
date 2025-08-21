namespace Company.SampleApi.OAuthServer;

public class AuthCode
{
    public string? ClientId { get; init; }
    public string? CodeChallenge { get; init; }
    public string? CodeChallengeMethod { get; init; }
    public string? RedirectUri { get; init; }
    public DateTime Expiry { get; init; }
}

