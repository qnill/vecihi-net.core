using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using vecihi.auth;
using vecihi.database;

namespace vecihi.api.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("vecihiDbConnection"),
                b => b.MigrationsAssembly("vecihi.auth")));

            services.AddDbContext<VecihiDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("vecihiDbConnection"),
                b => b.MigrationsAssembly("vecihi.database")));
        }
    }
}
