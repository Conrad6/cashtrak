using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;
using CashTrak.Annotations;
using CashTrak.Extensions;

namespace CashTrak.App.ViewModels
{
    public abstract class BaseViewModel<TEntity> : INotifyPropertyChanged, INotifyDataErrorInfo
        where TEntity : class, new()
    {
        public NavigationService NavigationService { get; }
        public TEntity Entity { get; }
        private readonly ValidationContext _validationContext;

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected BaseViewModel(NavigationService navigationService)
        {
            NavigationService = navigationService;
            Entity = new TEntity();
            _validationContext = new ValidationContext(this);
//            PropertyChanged += CheckErrors;
        }

        /*private void CheckErrors(object sender, PropertyChangedEventArgs e)
        {
            var errors = GetErrors(e.PropertyName);
            if(errors is {} && errors.GetEnumerator().MoveNext())
                ErrorsChanged(this, new DataErrorsChangedEventArgs(e.PropertyName));
        }*/

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
}