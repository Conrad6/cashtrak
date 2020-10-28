using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CashTrak.App
{
    public class MainApplication : Application
    {
        public MainApplication (NavigationWindow mainWindow)
        {
            if (mainWindow is { })
            {
                mainWindow.ShowsNavigationUI = false;
                mainWindow.Navigated += MainWindow_Navigated;
                mainWindow.SizeToContent = SizeToContent.WidthAndHeight;
                MainWindow = mainWindow;
                Startup += MainApplication_Startup;
            }
            else
            {
                MessageBox.Show("Could not find main window. Application is exiting", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Shutdown();
            }
        }

        private void MainApplication_Startup (object sender, StartupEventArgs e)
        {
            if (sender is Application app)
            {
                app.MainWindow?.Show();
            }
        }

        private void MainWindow_Navigated (object sender, NavigationEventArgs e)
        {
            if (sender is Window window)
            {
                if (window.Content is Page page)
                {
                    window.Title = !string.IsNullOrEmpty(page.Title) ? $"{page.Title} - CashTrak" : "CashTrak";
                }
                else
                    window.Title = "CashTrak";
            }
        }
    }
}
