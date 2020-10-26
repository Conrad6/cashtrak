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
    public abstract class BaseViewModel<TEntity> : INotifyPropertyChanged, INotifyPropertyChanging, INotifyDataErrorInfo
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
            PropertyChanging += OnPropertyChanging;
            PropertyChanged += OnPropertyChanged;
            PropertyChanged += CheckErrors;
        }

        private void CheckErrors(object sender, PropertyChangedEventArgs e)
        {
            if (GetErrors(e.PropertyName) != default(IEnumerable))
                ErrorsChanged.Raise(this, new DataErrorsChangedEventArgs(e.PropertyName));
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (GetErrors(e.PropertyName) != default(IEnumerable)) return;
            var propertyValue = GetType().GetProperty(e.PropertyName)?.GetValue(this);
            Entity.GetType().GetProperty(e.PropertyName)?.SetValue(Entity, propertyValue);
        }

        private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            var validationResults = new List<ValidationResult>();
            var propertyValue = GetType().GetProperty(e.PropertyName)?.GetValue(this);

            var propertyIsValid = Validator.TryValidateProperty(propertyValue, _validationContext, validationResults);
            if (propertyIsValid)
                Entity.GetType().GetProperty(e.PropertyName)?.SetValue(Entity, propertyValue);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
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
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event PropertyChangingEventHandler PropertyChanging;
    }
}