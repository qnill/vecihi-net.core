using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using vecihi.auth;

namespace vecihi.api
{
    public class IdentityClaimsValue
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityClaimsValue(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get { return Guid.Parse(_httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == JWTClaimIdentifier.UserId).Value); }
        }

        public string UserName
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == JWTClaimIdentifier.UserName).Value; }
        }
    }
}
