using AuthBoss.Domain.Security;

namespace AuthBoss.Infrastructure.Security;
public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
    {
        var salt = Guid.NewGuid().ToString();
        var passwordWithSalt = password + salt;
        throw new NotImplementedException();
    }

    public bool Verify(string password, string hash)
    {
        throw new NotImplementedException();
    }
}
