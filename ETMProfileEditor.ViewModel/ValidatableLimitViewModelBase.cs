using ETMProfileEditor.Model;
using MvvmValidation;
using System.Collections.Generic;
using System.Linq;

namespace ETMProfileEditor.ViewModel
{
    using Contract;

    public class ValidatableLimitViewModelBase : ValidatableViewModelBase
    {
        private IEnumerable<Limit> limits;

        /// <summary>
        /// Needs to be stored in LiteDb database.
        /// </summary>
        public IEnumerable<Limit> Limits
        {
            get => limits;
            set
            {
                limits = value;
                ValidatorFactory.ConfigureValidationRules(this, Validator, Limits);
            }
        }

        public ValidatableLimitViewModelBase(ISelect<Limit> limitRepository)
        {
            this.Limits = limitRepository?.Select();
            ValidatorFactory.ConfigureValidationRules(this, Validator, Limits);
        }

        protected override void ConfigureValidationRules()
        {
            ValidatorFactory.ConfigureValidationRules(this, Validator, Limits);
        }

        private class ValidatorFactory
        {
            public static void ConfigureValidationRules(ValidatableLimitViewModelBase mainViewModel, ValidationHelper Validator, IEnumerable<Limit> limits)
            {
                var type = mainViewModel.GetType();
                var props = type.GetProperties();
                if (limits != null)
                    foreach (var item in limits.Where(l => l.Type.Equals(type.Name)))
                    {
                        Validator.AddRule(item.Variable,
                            () =>
                                RuleResult.Assert((double)props.SingleOrDefault(p => p.Name == item.Variable).GetValue(mainViewModel) >= item.Minimum, "Value too low")
                            );

                        Validator.AddRule(item.Variable,
                            () =>
                            RuleResult.Assert((double)props.SingleOrDefault(p => p.Name == item.Variable).GetValue(mainViewModel) <= item.Maximum, "Value too High")
                            );
                    }
                //Validator.AddChildValidatable(() => InterestSelectorViewModel);
            }
        }
    }
}