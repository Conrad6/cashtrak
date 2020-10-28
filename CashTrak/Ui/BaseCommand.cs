using System;
using System.Windows.Input;

namespace CashTrak.Ui
{
    public class BaseCommand<TParam> : ICommand where TParam : class
    {
        private readonly Action<TParam> _executeAction;
        private readonly Func<TParam, bool> _canExecuteAction;
        public BaseCommand (Action<TParam> executeAction, Func<TParam, bool> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        public BaseCommand (Action<TParam> executeAction) : this(executeAction, null)
        { }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute (object parameter)
        {
            var ans = true;
            if (_canExecuteAction is { })
                ans = _canExecuteAction(parameter as TParam);
            OnCanExecuteChanged();
            return ans;
        }

        protected void OnCanExecuteChanged () => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public void Execute (object parameter)
        {
            _executeAction(parameter as TParam);
        }
    }
}
