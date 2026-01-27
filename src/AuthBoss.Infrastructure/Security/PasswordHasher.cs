using AuthBoss.Domain.Security;
using System.Security.Cryptography;
using System.Text;

namespace AuthBoss.Infrastructure.Security;
public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
    {
        var salt = Guid.NewGuid().ToString();
        var passwordWithSalt = password + salt;
        string passwordHashed = passwordWithSalt;
        long iteractions = 1000;

        for (int i = 0; i < iteractions; i++)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(passwordHashed));
            passwordHashed = Convert.ToHexString(bytes);
        }

        return $"{iteractions}${salt}${passwordHashed}";
    }

    public bool Verify(string password, string hash)
    {
        long iteractions = Convert.ToInt64(hash.Split('$')[0]);
        string salt = hash.Split('$')[1];
        var passwordWithSalt = password + salt;
        string passwordHashed = passwordWithSalt;
        for (int i = 0; i < iteractions; i++)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(passwordHashed));
            passwordHashed = Convert.ToHexString(bytes);
        }
       return passwordHashed == hash.Split('$')[2];
    }
}
