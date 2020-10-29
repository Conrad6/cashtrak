using System.Windows.Controls;
using CashTrak.App.ViewModels;
using CashTrak.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace CashTrak.App.Pages
{
    [Component(Lifetime = ServiceLifetime.Singleton)]
    public partial class BudgetHistoryPage : Page
    {
        public BudgetHistoryPageViewModel ViewModel { get; }

        public BudgetHistoryPage(BudgetHistoryPageViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}