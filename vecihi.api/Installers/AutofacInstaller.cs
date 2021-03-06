﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using vecihi.auth;
using vecihi.infrastructure;

namespace vecihi.api.Installers
{
    public static class AutofacInstaller
    {
        public static IServiceProvider Container(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains(nameof(domain)) || x.FullName.Contains(nameof(auth)))
                .ToArray();

            // Load All Services
            builder.RegisterAssemblyTypes(assemblies)
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork<Guid>>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<HttpContextAccessor>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<IdentityClaimsValue>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<EmailSender>().AsImplementedInterfaces().InstancePerLifetimeScope();

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }
    }
}