using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CashTrak.App.Pages;
using CashTrak.Data;
using CashTrak.Services;

namespace CashTrak.App
{
    public class MainApplication : Application
    {
        public MainApplication(CashTrakContext cashTrakContext, Window mainWindow)
        {
            Page startupPage;
            if (cashTrakContext.MonthlyBudgets.Any())
            {
                startupPage = ServiceLocator.Resolve<BudgetHistoryPage>();
            }
            else
            {
                startupPage = ServiceLocator.Resolve<NewBudgetEntryPage>();
            }

            if (mainWindow is NavigationWindow navWindow)
            {
                navWindow.ShowsNavigationUI = false;
                navWindow.Navigated += NavWindowOnNavigated;
                MainWindow = navWindow;
                navWindow.NavigationService.Navigate(startupPage);
            }
        }

        private void NavWindowOnNavigated(object sender, NavigationEventArgs e)
        {
            var navigationWindow = sender as NavigationWindow;
            var page = navigationWindow?.Content as Page;
            if (navigationWindow is {})
            {
                navigationWindow.Title = !string.IsNullOrEmpty(page?.Title) ? $"{page.Title} - CashTrak" : "CashTrak";
            }
        }
    }
}