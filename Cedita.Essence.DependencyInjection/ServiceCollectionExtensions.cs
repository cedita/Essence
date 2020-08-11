// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Cedita.Essence.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Automatically Inject services from specific assemblies.
        /// </summary>
        /// <param name="services">Dependency Injection Service Collection.</param>
        /// <param name="assemblies">Assemblies to be scanned.</param>
        /// <returns>Result of Auto Injection.</returns>
        public static AutoInjectResult AutoInjectForAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            return AutoInjectForAssemblies(services, t => true, assemblies);
        }

        /// <summary>
        /// Automatically Inject services from specific assemblies.
        /// </summary>
        /// <param name="services">Dependency Injection Service Collection.</param>
        /// <param name="filter">Filter for automatic injection types.</param>
        /// <param name="assemblies">Assemblies to be scanned.</param>
        /// <returns>Result of Auto Injection.</returns>
        public static AutoInjectResult AutoInjectForAssemblies(this IServiceCollection services, Func<Type, bool> filter, params Assembly[] assemblies)
        {
            if (assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            var typesToInject = assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsNested && filter(t))
                .Select(t => new
                {
                    Type = t,
                    AutoInject = t.GetCustomAttribute<AutoInjectAttribute>(),
                })
                .Where(t => t.AutoInject != null)
                .Select(t => new AutoInjectType(t.Type, t.AutoInject));

            var injectionResult = new AutoInjectResult(services, assemblies, typesToInject);

            foreach (var typeDescriptor in typesToInject)
            {
                IEnumerable<Type> interfacesToImplement;

                // Do we need to override these interfaces?
                if (typeDescriptor.Implements != null && typeDescriptor.Implements.Count() > 0)
                {
                    interfacesToImplement = typeDescriptor.Implements;
                }
                else
                {
                    interfacesToImplement = typeDescriptor.Type.GetTypeInfo().ImplementedInterfaces.Where(t => t != typeof(IDisposable) && t.IsPublic && !t.IsNested);
                }

                // If there are no interfaces, self-implement
                if (interfacesToImplement.Count() == 0)
                {
                    interfacesToImplement = new[] { typeDescriptor.Type };
                }

                typeDescriptor.Implements = interfacesToImplement;

                foreach (var interfaceType in interfacesToImplement)
                {
                    services.Add(new ServiceDescriptor(interfaceType, typeDescriptor.Type, typeDescriptor.Lifetime));
                }

                typeDescriptor.Injected = true;
            }

            return injectionResult;
        }
    }
}
