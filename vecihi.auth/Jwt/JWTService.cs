using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace vecihi.auth
{
    public interface IJWTService
    {
        ClaimsIdentity GenerateClaimsIdentity(string userId, string userName);
        Task<object> GenerateJwt(ClaimsIdentity identity, string userName);
    }

    public class JWTService : IJWTService
    {
        private readonly JwtOptions _jwtOptions;

        public JWTService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            JWTHelper.ThrowIfInvalidOptions(_jwtOptions);
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

        public async Task<object> GenerateJwt(ClaimsIdentity identity, string userName)
        {
            var response = new
            {
                access_token = await GenerateEncodedToken(userName, identity),
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                token_type = identity.AuthenticationType,
                user_id = identity.Claims.Single(c => c.Type == JWTClaimIdentifier.UserId).Value,
                user_name = identity.Claims.Single(c => c.Type == JWTClaimIdentifier.UserName).Value
            };

            return response;
        }
    }
}
