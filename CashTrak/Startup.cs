using System.Windows.Navigation;

using CashTrak.App;
using CashTrak.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CashTrak
{
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CashTrakContext>(options =>
            {
                options.UseMySql("Server=localhost;SslMode=None;Port=3306;Uid=root;Pwd=;");
            });

            services.AddSingleton<NavigationWindow>();
            services.AddSingleton<MainApplication>();
        }
    }
}
