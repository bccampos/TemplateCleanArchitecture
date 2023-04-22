using bruno.Application.Common.Interfaces.Authentication;
using bruno.Application.Common.Interfaces.Persistence;
using bruno.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bruno.Application.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;   

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;   
        }   

        public AuthenticationResult Register(string firstName, string lastName, string email, string password)
        {
            // Validate the user doenst exist
            if (_userRepository.GetByEmail(email) is not null)
            {
                throw new Exception("User with given email already exists.");
            }

            // Create user (generate unique ID) & persist it
            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };
            _userRepository.Add(user);

            Guid userId = Guid.NewGuid();

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }

        public AuthenticationResult Login(string email, string password)
        {
            // Validate the user doenst exist
            if (_userRepository.GetByEmail(email) is not User user)
            {
                throw new Exception("User with given email already exists.");
            }

            //Validate the passoword is correct
            if (user.Password != password)
            {
                throw new Exception("Invalid Password");
            }

            //Create JWT Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}
