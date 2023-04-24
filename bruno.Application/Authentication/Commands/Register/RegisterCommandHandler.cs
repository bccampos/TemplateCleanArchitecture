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

namespace bruno.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<Result<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            // Validate the user doenst exist
            if (_userRepository.GetByEmail(command.Email) is not null)
            {
                return Result.Fail<AuthenticationResult>(new DuplicateEmailError());
            }

            // Create user (generate unique ID) & persist it
            var user = new User()
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Password = command.Password
            };
            _userRepository.Add(user);

            Guid userId = Guid.NewGuid();

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}
