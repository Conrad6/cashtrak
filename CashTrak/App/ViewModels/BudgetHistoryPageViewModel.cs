using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;
using CashTrak.App.Pages;
using CashTrak.App.Ui.Commands;
using CashTrak.Core;
using CashTrak.Data;
using Microsoft.EntityFrameworkCore;

namespace CashTrak.App.ViewModels
{
    public class BudgetHistoryPageViewModel : BaseViewModel
    {
        public NewBudgetEntryPage NewBudgetEntryPage { get; }
        private readonly CashTrakContext _cashTrakContext;
        public ICommand NavigateCommand { get; }
        public ObservableCollection<MonthlyBudget> MonthlyBudgets { get; }

        public BudgetHistoryPageViewModel(NavigationService navigationService,
            CashTrakContext cashTrakContext,
            NewBudgetEntryPage newBudgetEntryPage) : base(navigationService)
        {
            NewBudgetEntryPage = newBudgetEntryPage;
            _cashTrakContext = cashTrakContext;
            MonthlyBudgets = new ObservableCollection<MonthlyBudget>();
            NavigateCommand = new BaseCommand(_ => true, _ => NavigationService.Navigate(_));
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            var budgets = await _cashTrakContext.MonthlyBudgets
                .Include(m => m.Expenses)
                .ToListAsync();
            budgets.ForEach(budget => MonthlyBudgets.Add(budget));
        }
    }
}