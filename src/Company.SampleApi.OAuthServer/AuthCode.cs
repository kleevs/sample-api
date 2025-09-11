namespace Company.SampleApi.OAuthServer;

public record AuthCode
{
    public required string Login { get; init; }
    public string? ClientId { get; init; }
    public string? CodeChallenge { get; init; }
    public string? CodeChallengeMethod { get; init; }
    public string? RedirectUri { get; init; }
    public string? Scope { get; init; }
    public DateTime Expiry { get; init; }
}

