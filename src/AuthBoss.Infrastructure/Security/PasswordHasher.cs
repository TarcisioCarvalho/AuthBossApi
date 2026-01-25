using AuthBoss.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthBoss.Infrastructure.Security;
public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
    {
        throw new NotImplementedException();
    }

    public bool Verify(string password, string hash)
    {
        throw new NotImplementedException();
    }
}
