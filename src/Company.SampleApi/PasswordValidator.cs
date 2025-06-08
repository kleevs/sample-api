using Company.SampleApi.Contracts;

namespace Company.SampleApi;

public class PasswordValidator : IPasswordValidator
{
    public bool IsValidPassword(string password) 
    {
        if (string.IsNullOrEmpty(password)) 
        {
            return false;
        }

        if (password.Length < 8) 
        {
            return false;
        }

        return true;
    }
}
