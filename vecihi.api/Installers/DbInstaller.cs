using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using vecihi.database;

namespace vecihi.api.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<VecihiDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("VecihiDbConnection"),
                b => b.MigrationsAssembly("vecihi.database")));
        }
    }
}
