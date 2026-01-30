using AuthBoss.Infrastructure.Security;
using FluentAssertions;
using Xunit;

namespace AuthBoss.Infrastructure.Tests.Security;
public class PasswordHasherTest
{
    [Theory]
    [InlineData("Teste")]
    public void Success(string passwordTest)
    {
        var hash = new PasswordHasher().Generate(passwordTest);
        hash.Should().NotBeNull();
    }
}
