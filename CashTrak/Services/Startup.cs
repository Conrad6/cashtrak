using System.Reflection;
using CashTrak.App;
using CashTrak.App.Windows;
using CashTrak.Data;
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
            var dbOptions =  builder.Options;
            
            services.AddSingleton(dbOptions);
            services.AddDbContext<CashTrakContext>();
            services.AddTransient<BudgetHistory>();
            services.AddSingleton<MainApplication>();
        }
    }
}