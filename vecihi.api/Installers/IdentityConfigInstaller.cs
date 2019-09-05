using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using vecihi.auth;
using vecihi.database;

namespace vecihi.api.Installers
{
    public class IdentityConfigInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            var identityBuilder = IdentityConfig.Builder(services);

            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole<Guid>), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<VecihiDbContext>().AddDefaultTokenProviders();
        }
    }
}
