using bruno.Application.Authentication;
using bruno.Application.Common.Errors;
using bruno.Contracts.Authentication;
using bruno.WebApi.Filter;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace bruno.WebApi.Controllers
{
    [Route("auth")]
    [ApiController]
    [ErrorHandlingFilter]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            Result<AuthenticationResult> authResult = _authenticationService.Register(request.FirstName, request.LastName, request.Email, request.Password);

            if (authResult.IsSuccess)
            {
                return Ok(MapAuthResult(authResult.Value));
            }

            var firstError = authResult.Errors[0];
            if (firstError is DuplicateEmailError) 
            {
                return Problem(statusCode: StatusCodes.Status409Conflict, detail: firstError.Message);
            }

            return Problem($"Unexpected error: {firstError.Message}");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var authResult = _authenticationService.Login(request.Email, request.Password);

            if (authResult.IsSuccess)
            {
                return Ok(MapAuthResult(authResult.Value));
            }

            var firstError = authResult.Errors[0];
            if (firstError is DuplicateEmailError)
            {
                return Problem(statusCode: StatusCodes.Status409Conflict, detail: firstError.Message);
            }

            return Problem($"Unexpected error: {firstError.Message}");
        }

        private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
        { 
            return new AuthenticationResponse(
                authResult.user.Id,
                authResult.user.FirstName,
                authResult.user.LastName,
                authResult.user.Email,
                authResult.Token);
        }
    }
}
