using bruno.Contracts.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bruno.Application.Authentication
{
    public interface IAuthenticationService
    {
        AuthenticationResult Register  (string firstName, string LastName, string email, string password);

        AuthenticationResult Login(string email, string password);
    }
}
