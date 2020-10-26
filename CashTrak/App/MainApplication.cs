using System.Windows;
using CashTrak.App.Windows;

namespace CashTrak.App
{
    public class MainApplication:Application
    {
        public MainApplication(BudgetHistory mainWindow)
        {
            MainWindow = mainWindow;
            MainWindow?.Show();
        }
    }
}