using ETMProfileEditor.Model;
using MvvmValidation;

namespace ETMProfileEditor.ViewModel
{
    using Contract;
    using PropertyTools.DataAnnotations;

    public class MapperStep : ViewModel.Step
    {
        private double cameraWindowLoad = 0;
        private double idleSpeed = 0;
        private double temperature = 0;
        private ControlProbe temperatureControlProbe;
        private bool temperatureControlEnabled;

        public bool TemperatureControlEnabled
        {
            get => temperatureControlEnabled;
            set => SetProperty(ref temperatureControlEnabled, value);
        }

        public Model.ControlProbe TemperatureControlProbe
        {
            get => temperatureControlProbe;
            set => SetProperty(ref temperatureControlProbe, value);
        }

        [Spinnable(LargeChange = 10, SmallChange = 0.1, Maximum = 10000000, Minimum = -10000000)]
        [FormatString("0.00")]
        public double CameraWindowLoad
        {
            get => cameraWindowLoad;
            set => SetProperty(ref cameraWindowLoad, value);
        }

        [Spinnable(LargeChange = 10, SmallChange = 0.1, Maximum = 10000000, Minimum = -10000000)]
        [FormatString("0.00")]
        public double Temperature
        {
            get => temperature;
            set => SetProperty(ref temperature, value);
        }

        [Spinnable(LargeChange = 10, SmallChange = 0.1, Maximum = 10000000, Minimum = -10000000)]
        [FormatString("0.00")]
        public double IdleSpeed
        {
            get => idleSpeed;
            set => SetProperty(ref idleSpeed, value);
        }

        protected override void ConfigureValidationRules()
        {
            ValidatorFactory.ConfigureValidationRules(this, Validator);
        }

        public MapperStep(int index, string description, ISelect<Limit> limitRepository) : base(index, description, limitRepository)
        {
            ValidatorFactory.ConfigureValidationRules(this, Validator);
        }

        public MapperStep() : base(default, default, default)
        {
        }

        private class ValidatorFactory
        {
            public static void ConfigureValidationRules(MapperStep mainViewModel, ValidationHelper Validator)
            {
                Validator.AddRequiredRule(() => mainViewModel.Temperature, "Temperature is required");
            }
        }
    }
}