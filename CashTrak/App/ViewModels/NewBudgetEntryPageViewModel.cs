using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using CashTrak.App.Pages;
using CashTrak.App.Ui.Commands;
using CashTrak.Attributes;
using CashTrak.Core;
using CashTrak.Data;
using CashTrak.Services;

namespace CashTrak.App.ViewModels
{
    [Component(ServiceType = typeof(BaseViewModel<MonthlyBudget>))]
    public class NewBudgetEntryPageViewModel : BaseViewModel<MonthlyBudget>
    {
        private readonly CashTrakContext _cashTrakContext;

        private async void SaveMonthlyBudget(MonthlyBudget budget)
        {
            try
            {
                await _cashTrakContext.MonthlyBudgets.AddAsync(budget);
                await _cashTrakContext.SaveChangesAsync();
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
                else
                {
                    NavigationService.Navigate(ServiceLocator.Resolve<BudgetHistoryPage>());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public NewBudgetEntryPageViewModel(CashTrakContext cashTrakContext, NavigationService navigationService) : base(
            navigationService)
        {
            _cashTrakContext = cashTrakContext;
            PropertyChanged += (_, __) => SaveCommand.CanExecute(this);
        }

        [Required(ErrorMessage = "Month value is required")]
        public DateTime? BudgetDate
        {
            get => Entity.BudgetDate;
            set
            {
                if (Entity.BudgetDate == value) return;
                Entity.BudgetDate = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Budget value is required")]
        public double? Budget
        {
            get => Entity.Budget;
            set
            {
                if (Entity.Budget == value) return;
                Entity.Budget = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; } = new MonthlyBudgetSaveCommand(
            o => !(o is null) && Validator.TryValidateObject(o, new ValidationContext(o), new List<ValidationResult>()),
            o =>
            {
                var _this = o as NewBudgetEntryPageViewModel;
                _this?.SaveMonthlyBudget(_this.Entity);
            });
    }
}