using bruno.Application.Authentication.Commands.Register;
using bruno.Application.Common.Errors;
using bruno.Application.Common.Interfaces.Authentication;
using bruno.Application.Common.Interfaces.Persistence;
using bruno.Domain.Entities;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bruno.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<Result<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            // Validate the user doenst exist
            if (_userRepository.GetByEmail(query.Email) is not User user)
            {
                return Result.Fail<AuthenticationResult>(new DuplicateEmailError());
            }

            //Validate the passoword is correct
            if (user.Password != query.Password)
            {
                return Result.Fail<AuthenticationResult>("Invalid password.");
            }

            //Create JWT Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}
