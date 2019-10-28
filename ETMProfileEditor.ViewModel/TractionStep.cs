using ETMProfileEditor.Model;
using MvvmValidation;
using System.Collections.Generic;

namespace ETMProfileEditor.ViewModel
{
    using Contract;
    using PropertyTools.DataAnnotations;

    public class TractionStep : ViewModel.Step
    {
        private double rollingSpeed;
        private double ballLoad;
        private bool unloadAtEnd;
        private bool measureDiscTrackRadius;

        //private List<double> slideRollRatios;
        private bool temperatureControlEnabled;

        private ControlProbe temperatureControlProbe;
        private double temperature;
        private bool waitForTemperatureToStabilise;
        private double idleSpeed;
        private double idleSlideRollRatio;
        private double idleLoad;

        public bool UnloadAtEnd
        {
            get => unloadAtEnd;
            set => SetProperty(ref unloadAtEnd, value);
        }

        public bool MeasureDiscTrackRadius
        {
            get => measureDiscTrackRadius;
            set => SetProperty(ref measureDiscTrackRadius, value);
        }

        [Slidable(LargeChange = 10, SmallChange = 0.1)]
        [FormatString("0.00")]
        public double RollingSpeed
        {
            get => rollingSpeed;
            set => SetProperty(ref rollingSpeed, value);
        }

        [Slidable(LargeChange = 10, SmallChange = 0.1)]
        [FormatString("0.00")]
        public double BallLoad
        {
            get => ballLoad;
            set => SetProperty(ref ballLoad, value);
        }

        [Category("Temperature")]
        public bool WaitForTemperatureToStabilise
        {
            get => waitForTemperatureToStabilise;
            set => SetProperty(ref waitForTemperatureToStabilise, value);
        }

        [Category("Temperature")]
        [DisplayName("Control Probe")]
        public ControlProbe TemperatureControlProbe
        {
            get => temperatureControlProbe;
            set => SetProperty(ref temperatureControlProbe, value);
        }

        [Category("Temperature")]
        [DisplayName("Control Enabled")]
        public bool TemperatureControlEnabled
        {
            get => temperatureControlEnabled;
            set => SetProperty(ref temperatureControlEnabled, value);
        }

        [Category("Temperature")]
        [FormatString("{0:0}°")]
        [Spinnable(LargeChange = 10, SmallChange = 0.1, Maximum = 10000000, Minimum = -10000000)]
        public double Temperature
        {
            get => temperature;
            set => SetProperty(ref temperature, value);
        }

        [Category("Idle")]
        [DisplayName("Speed")]
        [FormatString("0.00")]
        [Spinnable(LargeChange = 10, SmallChange = 0.1, Maximum = 10000000, Minimum = -10000000)]
        public double IdleSpeed
        {
            get => idleSpeed;
            set => SetProperty(ref idleSpeed, value);
        }

        [Browsable(false)]
        public ICollection<double> SlideRollRatios
        {
            get => new double[] { 1.0, 2.7, 10 };
            //set => SetProperty(ref slideRollRatios, value);
        }

        [Category("Idle")]
        [DisplayName("Slide-Roll Ratio")]
        [FormatString("0.00")]
        [Spinnable(LargeChange = 10, SmallChange = 0.1, Maximum = 10000000, Minimum = -10000000)]
        public double IdleSlideRollRatio
        {
            get => idleSlideRollRatio;
            set => SetProperty(ref idleSlideRollRatio, value);
        }

        [Category("Idle")]
        [DisplayName("Load")]
        [FormatString("0.00")]
        [Spinnable(LargeChange = 10, SmallChange = 0.1, Maximum = 10000000, Minimum = -10000000)]
        public double IdleLoad
        {
            get => idleLoad;
            set => SetProperty(ref idleLoad, value);
        }

        protected override void ConfigureValidationRules()
        {
            ValidatorFactory.ConfigureValidationRules(this, Validator);
        }

        public TractionStep(int index, string description, ISelect<Limit> limitRepository) : base(index, description, limitRepository)
        {
            ValidatorFactory.ConfigureValidationRules(this, Validator);
        }

        public TractionStep() : base(default, default, default)
        {
        }

        private class ValidatorFactory
        {
            public static void ConfigureValidationRules(TractionStep mainViewModel, ValidationHelper Validator)
            {
                Validator.AddRequiredRule(() => mainViewModel.Temperature, "Temperature is required");
            }
        }
    }
}