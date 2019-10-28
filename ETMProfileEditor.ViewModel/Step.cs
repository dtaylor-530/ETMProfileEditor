namespace ETMProfileEditor.ViewModel
{
    using Contract;
    using Model;
    using MvvmValidation;

    public class Step : ValidatableLimitViewModelBase
    {
        private int index;
        private string description;

        public Step(int index, string description, ISelect<Limit> limitRepository) : base(limitRepository)
        {
            ValidatorFactory.ConfigureValidationRules(this, Validator);
            this.index = index;
            this.description = description;
        }

        public int Index
        {
            get => index;
            set => SetProperty(ref index, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        protected override void ConfigureValidationRules()
        {
            ValidatorFactory.ConfigureValidationRules(this, Validator);
        }

        private class ValidatorFactory
        {
            public static void ConfigureValidationRules(Step mainViewModel, ValidationHelper Validator)
            {
                Validator.AddRequiredRule(() => mainViewModel.Index, "Index is required");

                Validator.AddRequiredRule(() => mainViewModel.Description, "Description is required");
            }
        }
    }
}