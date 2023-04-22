using bruno.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bruno.Application.Authentication
{
    public record AuthenticationResult(
        User user,
        string Token);
}
