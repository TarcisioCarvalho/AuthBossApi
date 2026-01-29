using AuthBoss.Infrastructure.Security;
using FluentAssertions;
using Xunit;

namespace AuthBoss.Infrastructure.Tests.Security;
public class PasswordHasherTest
{
    [Fact]
    public void Success()
    {
        string passwordTest = "password";
        var hash = new PasswordHasher().Generate(passwordTest);
        hash.Should().NotBeNull();
    }
}
