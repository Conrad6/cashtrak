using System;

namespace CashTrak.App.Ui.Commands
{
    public class MonthlyBudgetSaveCommand : BaseCommand
    {
        public MonthlyBudgetSaveCommand(Func<object, bool> canExecute, Action<object> execute) : base(canExecute,
            execute)
        {
        }
    }
}