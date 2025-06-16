using Company.SampleApi.Contracts;
using Moq;

namespace Company.SampleApi.Tools.Testing;

public static class PasswordValidatorMockExtension
{
    public static Mock<IPasswordValidator> SetupIsValidPasswordTrue(this Mock<IPasswordValidator> mock)
    {
        mock.Setup(_ => _.IsValidPassword(It.IsAny<string>())).Returns(true);
        return mock;
    }

    public static Mock<IPasswordValidator> SetupIsValidPasswordFalse(this Mock<IPasswordValidator> mock)
    {
        mock.Setup(_ => _.IsValidPassword(It.IsAny<string>())).Returns(false);
        return mock;
    }
}
