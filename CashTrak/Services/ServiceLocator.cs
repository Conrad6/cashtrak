using Microsoft.Extensions.DependencyInjection;

namespace CashTrak.Services
{
    public static class ServiceLocator
    {
        internal static ServiceProvider ServiceProvider;
        public static T Resove<T>() => ServiceProvider.GetService<T>();
    }
}