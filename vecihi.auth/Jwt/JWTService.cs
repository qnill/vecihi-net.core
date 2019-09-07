using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using vecihi.domain.Modules;
using vecihi.helper;
using vecihi.helper.Const;

namespace vecihi.auth
{
    public interface IJwtService
    {
        Task<object> GenerateJwt(string userId, string userName);
        Task<ApiResult> GenerateRefreshToken(string token, string refreshToken);
    }

    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IEmployeeService _employeeService;
        private readonly IAuthService _authService;

        public JwtService(IOptions<JwtOptions> jwtOptions, TokenValidationParameters tokenValidationParameters,
            IEmployeeService employeeService, IAuthService authService)
        {
            _jwtOptions = jwtOptions.Value;
            JwtHelper.ThrowIfInvalidOptions(_jwtOptions);
            _employeeService = employeeService;
            _tokenValidationParameters = tokenValidationParameters;
            _authService = authService;
        }

        private async Task<(string encodedJwt, string refreshToken)> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, JwtHelper.ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(JwtClaimIdentifier.UserId),
                 identity.FindFirst(JwtClaimIdentifier.UserName)
             };

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var refreshToken = await _authService.SaveRefreshToken(jwt.Id);

            return (encodedJwt, refreshToken);
        }

        private ClaimsIdentity GenerateClaimsIdentity(string userId, string userName)
        {
            var claims = new ClaimsIdentity(new GenericIdentity(userName, "bearer"), new[]
            {
                new Claim(JwtClaimIdentifier.UserId, userId),
                new Claim(JwtClaimIdentifier.UserName, userName)
            });

            return claims;
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var refreshTokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = _tokenValidationParameters.ValidIssuer,

                    ValidateAudience = true,
                    ValidAudience = _tokenValidationParameters.ValidAudience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _tokenValidationParameters.IssuerSigningKey,

                    RequireExpirationTime = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, refreshTokenValidationParameters, out var validatedToken);

                if (!JwtHelper.IsJwtWithValidSecurityAlgorithm(validatedToken))
                    return null;

                return principal;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<object> GenerateJwt(string userId, string userName)
        {
            ClaimsIdentity identity = GenerateClaimsIdentity(userId, userName);

            var (encodedJwt, refreshToken) = await GenerateEncodedToken(userName, identity);

            // Get Employee Info
            var employeeInfo = await _employeeService.InfoForJwt(Guid.Parse(userId));

            string employeeId = null;
            if (employeeInfo != null)
                employeeId = employeeInfo.Id.ToString();

            var response = new
            {
                access_token = encodedJwt,
                refresh_token = refreshToken,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                token_type = identity.AuthenticationType,
                user_id = userId,
                user_name = userName,
                employee_id = employeeId
            };

            return response;
        }

        public async Task<ApiResult> GenerateRefreshToken(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
                return new ApiResult { Message = ApiResultMessages.AUE0002 };

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var usedRefreshToken = await _authService.UseRefreshToken(refreshToken, jti);

            if (usedRefreshToken.Message != ApiResultMessages.Ok)
                return usedRefreshToken;

            string userId = validatedToken.Claims.Single(x => x.Type == JwtClaimIdentifier.UserId).Value;
            string userName = validatedToken.Claims.Single(x => x.Type == JwtClaimIdentifier.UserName).Value;

            var jwt = await GenerateJwt(userId, userName);

            return new ApiResult { Data = jwt, Message = ApiResultMessages.Ok };
        }
    }
}