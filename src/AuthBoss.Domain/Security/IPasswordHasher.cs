namespace AuthBoss.Domain.Security;
public interface IPasswordHasher
{
    string Generate(string password);
    bool Verify(string password, string hash);
}
