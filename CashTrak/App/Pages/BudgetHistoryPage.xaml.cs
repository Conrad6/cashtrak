using System.Windows.Controls;
using CashTrak.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace CashTrak.App.Pages
{
    [Component(Lifetime = ServiceLifetime.Singleton)]
    public partial class BudgetHistoryPage : Page
    {
        public BudgetHistoryPage()
        {
            InitializeComponent();
        }
    }
}