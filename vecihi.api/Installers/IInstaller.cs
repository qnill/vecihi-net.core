using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace vecihi.api.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration Configuration);
    }
}
