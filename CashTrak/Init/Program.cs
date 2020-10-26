using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using CashTrak.App;
using CashTrak.Data;
using CashTrak.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashTrak.Init
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("DefaultConnection",
                        "Database=cashtrak_db;Port=3306;Uid=root;Pwd=;Server=localhost;SslMode=None;")
                });
            var configuration = configurationBuilder.Build();
            var startup = new Startup()
            {
                Configuration = configuration
            };
            var serviceCollection = new ServiceCollection();
            startup.ConfigureServices(serviceCollection);
            ServiceLocator.ServiceProvider = serviceCollection.BuildServiceProvider();
            var _ = ServiceLocator.Resolve<CashTrakContext>();

            var application = ServiceLocator.Resolve<MainApplication>();
            application.Exit += async (o, e) => { await _.Database.CurrentTransaction?.CommitAsync(); };
            application.Startup += (o, e) => application.MainWindow?.Show();
            application.Run();
        }
    }
}