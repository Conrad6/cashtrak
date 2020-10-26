﻿using System;
using System.Windows.Input;
using CashTrak.Core;

namespace CashTrak.App.Ui.Commands
{
    public class MonthlyBudgetSaveCommand : ICommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;
        private bool __canExecute = false;

        public MonthlyBudgetSaveCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            var temp = _canExecute(parameter);
            if (__canExecute == temp) return __canExecute;
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