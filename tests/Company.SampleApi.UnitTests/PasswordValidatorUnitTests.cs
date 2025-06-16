using Company.SampleApi;

namespace SampleApi.UnitTests;

public class PasswordValidatorUnitTests
{
    [Fact]
    public void Should_Be_Valid()
    {
        var service = new PasswordValidator();

        var result = service.IsValidPassword("password");

        Assert.True(result);
    }

    [Fact]
    public void Should_Not_Be_Valid()
    {
        var service = new PasswordValidator();

        var result = service.IsValidPassword("123");

        Assert.False(result);
    }
}