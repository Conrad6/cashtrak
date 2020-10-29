using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;
using CashTrak.Annotations;

namespace CashTrak.App.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public NavigationService NavigationService { get; }
        private readonly ValidationContext _validationContext;
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected BaseViewModel(NavigationService navigationService)
        {
            NavigationService = navigationService;
            _validationContext = new ValidationContext(this);
        }
        

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            var results = new List<ValidationResult>();
            var propertyValue = GetType().GetProperty(propertyName)?.GetValue(this);
            return Validator.TryValidateProperty(propertyValue, _validationContext, results)
                ? results.Select(result => result.ErrorMessage)
                : default;
        }

        public bool HasErrors => Validator.TryValidateObject(this, _validationContext, new List<ValidationResult>());
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add => ErrorsChangedEventManager.AddHandler(this, value);
            remove => ErrorsChangedEventManager.RemoveHandler(this, value);
        }
    }

    public abstract class BaseViewModel<TEntity> : BaseViewModel where TEntity : class, new()
    {
        protected TEntity Entity { get; }
        protected BaseViewModel(NavigationService navigationService):base(navigationService)
        {
            Entity = new TEntity();
        }
    }
}