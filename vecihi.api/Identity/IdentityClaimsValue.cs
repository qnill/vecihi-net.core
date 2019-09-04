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

        public Guid UserId()
        {
            return Guid.Parse(_httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == JWTClaimIdentifier.UserId).Value);
        }

        public Type UserId<Type>()
        {
            return helper.Convert.ToGenericType<Type>(UserId().ToString());
        }

        public string UserName()
        {
            return _httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == JWTClaimIdentifier.UserName).Value;
        }
    }
}