using System;
using System.Collections.Generic;
using System.Text;

using CashTrak.App;

using Microsoft.Extensions.DependencyInjection;

namespace CashTrak
{
    internal static class ServiceLocator
    {
        private static ServiceProvider _serviceProvider;
        internal static void Initialize (IServiceCollection services) => _serviceProvider = services.BuildServiceProvider();
        public static TService Resolve<TService> () => _serviceProvider.GetService<TService>();
    }

    public class Program
    {
        [STAThread]
        public static void Main ()
        {
            var services = new ServiceCollection();
            var startup = new Startup();

            startup.ConfigureServices(services);

            ServiceLocator.Initialize(services);

            var application = ServiceLocator.Resolve<MainApplication>();
            application?.Run();
        }
    }
}
