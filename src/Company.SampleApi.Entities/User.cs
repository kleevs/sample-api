namespace Company.SampleApi.Entities;

public record User
{
    public string Password { get; init; } = string.Empty;
    public string Login { get; init; } = string.Empty;
}