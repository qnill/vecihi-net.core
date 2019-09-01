using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace vecihi.api.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services
                .AddMvc(options => { options.OutputFormatters.RemoveType<StringOutputFormatter>(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
    }
}
