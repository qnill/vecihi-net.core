﻿using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using vecihi.domain.Modules;

namespace vecihi.auth
{
    public interface IJWTService
    {
        ClaimsIdentity GenerateClaimsIdentity(string userId, string userName);
        Task<object> GenerateJwt(string userId, string userName);
    }

    public class JWTService : IJWTService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IEmployeeService _employeeService;

        public JWTService(IOptions<JwtOptions> jwtOptions, IEmployeeService employeeService)
        {
            _jwtOptions = jwtOptions.Value;
            JWTHelper.ThrowIfInvalidOptions(_jwtOptions);
            _employeeService = employeeService;
        }

        private async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, JWTHelper.ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(JWTClaimIdentifier.UserId),
                 identity.FindFirst(JWTClaimIdentifier.UserName)
             };

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userId, string userName)
        {
            var claims = new ClaimsIdentity(new GenericIdentity(userName, "bearer"), new[]
            {
                new Claim(JWTClaimIdentifier.UserId, userId),
                new Claim(JWTClaimIdentifier.UserName, userName)
            });

            return claims;
        }

        public async Task<object> GenerateJwt(string userId, string userName)
        {
            ClaimsIdentity identity = GenerateClaimsIdentity(userId, userName);

            // Get Employee Info
            var employeeInfo = await _employeeService.InfoForJwt(Guid.Parse(userId));

            string employeeId = null;
            if (employeeInfo != null)
                employeeId = employeeInfo.Id.ToString();

            var response = new
            {
                access_token = await GenerateEncodedToken(userName, identity),
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                token_type = identity.AuthenticationType,
                user_id = userId,
                user_name = userName,
                employee_id = employeeId
            };

            return response;
        }
    }
}
