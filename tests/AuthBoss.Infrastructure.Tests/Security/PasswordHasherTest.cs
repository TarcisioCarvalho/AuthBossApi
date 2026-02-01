using AuthBoss.Infrastructure.Security;
using FluentAssertions;
using Xunit;

namespace AuthBoss.Infrastructure.Tests.Security;
public class PasswordHasherTest
{
    [Fact]
    public void Success()
    {
        string passwordTest = "!abc1234";
        var hash = new PasswordHasher().Generate(passwordTest);
        hash.Should().NotBeNull();
        var parts = hash.Split('$');
        parts.Length.Should().Be(3);
        parts[0].Should().Be("1000");
        Guid.TryParse(parts[1], out var id).Should().BeTrue();
        parts[2].Length.Should().Be(64);

        var isValid = new PasswordHasher().Verify(passwordTest, hash);
        isValid.Should().BeTrue();
    }
}
