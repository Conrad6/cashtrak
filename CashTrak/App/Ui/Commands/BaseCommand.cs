using System;
using System.Windows.Input;

namespace CashTrak.App.Ui.Commands
{
    public class BaseCommand : ICommand
    {
        public BaseCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }
        
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;
        private bool __canExecute;

        public bool CanExecute(object parameter)
        {
            var temp = _canExecute(parameter);
            if (temp == __canExecute) return __canExecute;

            __canExecute = temp;
            OnCanExecuteChanged();

            return __canExecute;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        private void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;
    }
}