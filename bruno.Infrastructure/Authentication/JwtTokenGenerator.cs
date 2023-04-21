﻿using bruno.Application.Common.Interfaces.Authentication;
using bruno.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace bruno.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IDateTimeProvider _dateTimeProvider; 
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions) 
        { 
            _dateTimeProvider = dateTimeProvider;
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(Guid userId, string firstName, string lastName)
        {
            var siginingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, firstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var segurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                claims: claims,
                signingCredentials : siginingCredentials);


            return new JwtSecurityTokenHandler().WriteToken(segurityToken);
        }
    }
}
