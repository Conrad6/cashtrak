using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Input;
using System.Windows.Navigation;

using CashTrak.Core;
using CashTrak.Data;
using CashTrak.Resources;
using CashTrak.Ui;

namespace CashTrak.ViewModels
{
    public class NewBudgetEntryPageViewModel : ViewModelBase<MonthlyBudget>
    {
        private readonly CashTrakContext _dbContext;

        private async void SaveBudgetEntry (MonthlyBudget budget)
        {
            await _dbContext.MonthlyBudgets.AddAsync(budget);
            await _dbContext.SaveChangesAsync();
        }

        [Required(ErrorMessageResourceName = nameof(Strings.ValueRequiredErrorMessageTemplate), ErrorMessageResourceType = typeof(Strings))]
        [Display(Name = "Budget Date")]
        public DateTime? BudgetDate
        {
            get => Entity.BudgetDate;
            set => SetPropertyValue(value);
        }
        [Required(ErrorMessageResourceName = nameof(Strings.ValueRequiredErrorMessageTemplate), ErrorMessageResourceType = typeof(Strings))]
        [Display(Name ="Budget Amount")]
        public double? Budget
        {
            get => Entity.Budget;
            set => SetPropertyValue(value);
        }
        public ICommand SaveBudgetEntryCommand { get; }

        public NewBudgetEntryPageViewModel (NavigationService navigationService, CashTrakContext cashTrakContext) : base(navigationService)
        {
            _dbContext = cashTrakContext;
            SaveBudgetEntryCommand = new BaseCommand<MonthlyBudget>(SaveBudgetEntry, o => string.IsNullOrEmpty(Error));
        }
    }
}
