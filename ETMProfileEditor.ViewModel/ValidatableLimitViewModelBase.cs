using ETMProfileEditor.Model;
using MvvmValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETMProfileEditor.ViewModel
{
    using Contract;

    public class ValidatableLimitViewModelBase:ValidatableViewModelBase
    {
        /// <summary>
        /// Needs to be stored in LiteDb database.
        /// </summary>
        public IEnumerable<Limit> Limits { get;  set; }

        public ValidatableLimitViewModelBase(ISelect<Limit> limitRepository)
        {
            this.Limits = limitRepository?.Select();
            ValidatorFactory.ConfigureValidationRules(this, Validator, Limits);
        }


        protected override void ConfigureValidationRules()
        {
            ValidatorFactory.ConfigureValidationRules(this, Validator, Limits);
        }

        class ValidatorFactory
        {
            public static void ConfigureValidationRules(ValidatableLimitViewModelBase mainViewModel, ValidationHelper Validator, IEnumerable<Limit> limits)
            {
                var type = mainViewModel.GetType();
                var props = type.GetProperties();
                if(limits!=null)
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
