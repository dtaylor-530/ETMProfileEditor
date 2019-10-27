using MvvmValidation;
using Reactive.Bindings;
using System;
using System.Collections;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ETMProfileEditor.ViewModel
{
    public abstract class ValidatableViewModelBase : Mvvm.BindableBase, IValidatable, INotifyDataErrorInfo
    {
        //public string ValidationErrorsString
        //{
        //    get { return validationErrorsString; }
        //    private set
        //    {
        //        validationErrorsString = value;
        //        OnPropertyChanged(nameof(ValidationErrorsString));
        //    }
        //}

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;

            OnPropertyChanged(propertyName);
            Validator.ValidateAsync(propertyName);
            return true;
        }

        private string validationErrorsString;
        private bool? isValid;

        public ICommand ValidateCommand { get; } = new ReactiveCommand();

        protected ValidationHelper Validator { get; } = new ValidationHelper();

        private NotifyDataErrorInfoAdapter NotifyDataErrorInfoAdapter { get; }

        protected ValidatableViewModelBase()
        {
            NotifyDataErrorInfoAdapter = new NotifyDataErrorInfoAdapter(Validator);

            Observable.FromEventPattern<DataErrorsChangedEventArgs>(
                       ev => NotifyDataErrorInfoAdapter.ErrorsChanged += ev,
                       ev => NotifyDataErrorInfoAdapter.ErrorsChanged -= ev)
                .Select(e => e.EventArgs.PropertyName)
                .Subscribe(OnPropertyChanged);

            var obs = Observable.FromEventPattern<ValidationResultChangedEventArgs>(
                ev => Validator.ResultChanged += ev,
                ev => Validator.ResultChanged -= ev)
                .Select(e => e.EventArgs.NewResult)
    .CombineLatest(IsValid, ValidateCommand as ReactiveCommand, (a, b, c) => (a, b, c))
    .Where((ab) => ab.b == false)
    .Select(ab => ab.a);

            obs.Subscribe(a => IsValid.Value = a.IsValid);
            ValidationErrorsString = obs.Select(a => a.ToString()).ToReadOnlyReactiveProperty();
        }

        protected abstract void ConfigureValidationRules();

        Task<ValidationResult> IValidatable.Validate()
        {
            return Validator.ValidateAllAsync();
        }

        //protected void OnValidationResultChanged(object sender, ValidationResultChangedEventArgs e)
        //{
        //    if (!IsValid.GetValueOrDefault(true))
        //    {
        //        ValidationResult validationResult = Validator.GetResult();

        //        UpdateValidationSummary(validationResult);
        //    }
        //}

        //private void UpdateValidationSummary(ValidationResult validationResult)
        //{
        //    IsValid = validationResult.IsValid;
        //    ValidationErrorsString = validationResult.ToString();
        //}

        public ReadOnlyReactiveProperty<string> ValidationErrorsString { get; }

        public ReactiveProperty<bool> IsValid { get; } = new ReactiveProperty<bool>();
        //{
        //    get { return isValid; }
        //    private set
        //    {
        //        isValid = value;
        //        OnPropertyChanged(nameof(IsValid));
        //    }
        //}

        #region Implementation of INotifyDataErrorInfo

        public IEnumerable GetErrors(string propertyName)
        {
            return NotifyDataErrorInfoAdapter.GetErrors(propertyName);
        }

        public bool HasErrors => NotifyDataErrorInfoAdapter.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        //{
        //    add { NotifyDataErrorInfoAdapter.ErrorsChanged += value; }
        //    remove { NotifyDataErrorInfoAdapter.ErrorsChanged -= value; }
        //}

        #endregion Implementation of INotifyDataErrorInfo
    }
}