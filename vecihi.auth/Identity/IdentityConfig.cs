using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using vecihi.database.model;

namespace vecihi.auth
{
    public static class IdentityConfig
    {
        public static IdentityBuilder Builder(IServiceCollection services)
        {
            var identityBuilder = services.AddIdentityCore<User>(o =>
             {
                 o.User.RequireUniqueEmail = true;
                 o.Password.RequireDigit = true;
                 o.Password.RequireLowercase = true;
                 o.Password.RequireUppercase = true;
                 o.Password.RequireNonAlphanumeric = true;
                 o.Password.RequiredLength = 8;
             });

            return identityBuilder;
        }
    }
}