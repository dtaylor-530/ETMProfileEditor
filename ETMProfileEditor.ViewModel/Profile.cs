using MvvmValidation;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ETMProfileEditor.ViewModel
{
    using System;

    public class Profile : ValidatableViewModelBase, IEquatable<Profile>
    {
        private string name;
        private string description;

        public Profile() : base()
        {
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        /// <summary>
        /// Used by LiteDb
        /// </summary>
        public string Id => name;

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public ICollection<Step> Steps { get; set; } = new ObservableCollection<Step>();

        protected override void ConfigureValidationRules()
        {
            ValidatorFactory.ConfigureValidationRules(this, Validator);
        }

        private class ValidatorFactory
        {
            public static void ConfigureValidationRules(Profile mainViewModel, ValidationHelper Validator)
            {
                var type = mainViewModel.GetType();
                var props = type.GetProperties();

                Validator.AddRequiredRule(() => mainViewModel.Name, "Name is required");

                Validator.AddRequiredRule(() => mainViewModel.Description, "Description is required");
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Profile other)
        {
            return this.Name.Equals(other?.Name);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as Profile);
        }

        public override int GetHashCode()
        {
            return Name.Length;
        }
    }
}