using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;

namespace CashTrak.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        public string this[string propertyName]
        {
            get
            {
                var results = new List<ValidationResult>();
                var propertyValue = GetType().GetProperty(propertyName).GetValue(this);
                var isValid = Validator.TryValidateProperty(propertyValue, _validationContext, results);
                return isValid ? string.Empty : results.Select(result => result.ErrorMessage)
                    .Aggregate((_, __) => string.Join(Environment.NewLine, _, __));
            }
        }

        public string Error
        {
            get
            {
                var results = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(this, _validationContext, results);
                return isValid ? string.Empty : results.Select(result => result.ErrorMessage)
                    .Aggregate((_, __) => string.Join(Environment.NewLine, _, __));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected readonly NavigationService _navigationService;

        protected void OnPropertyChanged ([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetPropertyValue(object newValue, [CallerMemberName] string propertyName = null)
        {
            var temp = GetType().GetProperty(propertyName).GetValue(this);
            if (temp == newValue) return;

            GetType().GetProperty(propertyName).SetValue(this, newValue);
            OnPropertyChanged();
        }

        private readonly ValidationContext _validationContext;

        protected ViewModelBase (NavigationService navigationService)
        {
            _validationContext = new ValidationContext(this);
            _navigationService = navigationService;
        }
    }

    public abstract class ViewModelBase<TEntity> : ViewModelBase where TEntity : class, new()
    {
        public TEntity Entity { get; }
        public ViewModelBase (NavigationService navigationService) : base(navigationService)
        {
            Entity = new TEntity();
        }
    }
}
