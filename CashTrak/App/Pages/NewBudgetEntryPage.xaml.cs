using System.Windows.Controls;
using CashTrak.App.ViewModels;
using CashTrak.Attributes;
using CashTrak.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CashTrak.App.Pages
{
    [Component(Lifetime = ServiceLifetime.Singleton)]
    public partial class NewBudgetEntryPage : Page
    {
        public NewBudgetEntryPageViewModel ViewModel { get; }

        public NewBudgetEntryPage(BaseViewModel<MonthlyBudget> viewModel)
        {
            ViewModel = viewModel as NewBudgetEntryPageViewModel;
            InitializeComponent();
        }
    }
}