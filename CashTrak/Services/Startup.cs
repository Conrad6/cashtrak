using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using CashTrak.App;
using CashTrak.App.Pages;
using CashTrak.App.ViewModels;
using CashTrak.Core;
using CashTrak.Data;
using CashTrak.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashTrak.Services
{
    public class Startup
    {
        internal IConfiguration Configuration { get; set; }

        internal void ConfigureServices(IServiceCollection services)
        {
            var builder = new DbContextOptionsBuilder<CashTrakContext>()
                .UseMySql(Configuration["DefaultConnection"]);
            var dbOptions = builder.Options;

            services.AddSingleton(dbOptions);
            services.AddSingleton<NavigationWindow>();
            services.AddTransient(provider => provider.GetService<NavigationWindow>().NavigationService);
            services.AddDbContext<CashTrakContext>(ServiceLifetime.Singleton);
            
            services.AddSingleton<MainApplication>();
            
            services.AddTransient<BudgetHistoryPage>();
            services.AddTransient<NewBudgetEntryPage>();
            
            services.AddTransient<BaseViewModel<MonthlyBudget>, NewBudgetEntryPageViewModel>();
            services.AddTransient<BudgetHistoryPageViewModel>();
        }
    }
}