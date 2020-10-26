using System;
using System.Linq;
using System.Reflection;
using CashTrak.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace CashTrak.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AssemblyScan(this IServiceCollection serviceCollection, Assembly assembly)
        {
            if (assembly is null) throw new ArgumentNullException(nameof(assembly));

            var componentTypes = assembly.DefinedTypes.Where(type =>
                type.GetCustomAttributes().Any(attribute => attribute is ComponentAttribute));

            var assemblyTypes = componentTypes as TypeInfo[] ?? componentTypes.ToArray();
            if (!assemblyTypes.Any()) return serviceCollection;
            foreach (var assemblyType in assemblyTypes)
            {
                var componentAttribute =
                    assemblyType.GetCustomAttribute(typeof(ComponentAttribute)) as ComponentAttribute;
                var lifetime = componentAttribute?.Lifetime ?? ServiceLifetime.Scoped;
                if (lifetime == ServiceLifetime.Singleton)
                    serviceCollection.AddSingleton(assemblyType);
                else if (lifetime == ServiceLifetime.Transient)
                {
                    if (componentAttribute?.ServiceType is null)
                        throw new NullReferenceException($"{nameof(componentAttribute.ServiceType)} cannot be null");
                    serviceCollection.AddTransient(componentAttribute.ServiceType, assemblyType);
                }
                else
                {
                    if (componentAttribute?.ServiceType is null)
                        throw new NullReferenceException($"{nameof(componentAttribute.ServiceType)} cannot be null");
                    serviceCollection.AddScoped(componentAttribute.ServiceType, assemblyType);
                }
            }

            return serviceCollection;
        }
    }
}