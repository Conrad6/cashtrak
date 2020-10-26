using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using System.Windows.Navigation;
using CashTrak.App.Ui.Commands;
using CashTrak.Attributes;
using CashTrak.Core;
using CashTrak.Data;

namespace CashTrak.App.ViewModels
{
    [Component(ServiceType = typeof(BaseViewModel<MonthlyBudget>))]
    public class NewBudgetEntryPageViewModel : BaseViewModel<MonthlyBudget>
    {
        private readonly CashTrakContext _cashTrakContext;

        private async void SaveMonthlyBudget(MonthlyBudget budget)
        {
            await _cashTrakContext.MonthlyBudgets.AddAsync(budget);
            await _cashTrakContext.SaveChangesAsync();
        }

        public NewBudgetEntryPageViewModel(CashTrakContext cashTrakContext, NavigationService navigationService) : base(
            navigationService)
        {
            _cashTrakContext = cashTrakContext;
        }

        [Required(ErrorMessage = "Month value is required")]
        public Month? Month { get; set; }

        [Required(ErrorMessage = "Budget value is required")]
        public double? Budget { get; set; }

        public ICommand SaveCommand => new MonthlyBudgetSaveCommand(o => HasErrors, o => SaveMonthlyBudget(Entity));
    }
}