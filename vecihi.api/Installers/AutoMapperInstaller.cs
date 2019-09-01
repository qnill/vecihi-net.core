using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using vecihi.infrastructure;

namespace vecihi.api.Installers
{
    public class AutoMapperInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(Profile).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToArray();

            types[types.Count()] = typeof(AutocompleteProfile<Guid>);

            services.AddAutoMapper(types);
        }
    }
}
