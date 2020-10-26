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
            services.AddDbContext<CashTrakContext>(options =>
                {
                    options.UseMySql(Configuration["DefaultConnection"]);
                });
            services.AddTransient<BudgetHistory>();
            services.AddSingleton<MainApplication>();
        }
    }
}