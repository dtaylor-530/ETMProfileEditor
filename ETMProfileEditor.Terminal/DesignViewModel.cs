using Bogus;
using Reactive.Bindings;
using System.Collections.Generic;
using System.Linq;

namespace ETMProfileEditor.Terminal
{
    using ViewModel;

    public class DesignViewModel
    {
        public ICollection<Step> Items { get; }

        public string Key { get; }

        /// <summary>
        /// Step that has been selected by the user.
        /// </summary>
        public ReactiveProperty<Step> Selected { get; }

        /// <summary>
        /// Constructor for the MainWindowViewModel
        /// </summary>
        public DesignViewModel()
        {
            var mapperStepFaker = new Faker<MapperStep>()
                .StrictMode(true)
                .RuleFor(o => o.Description, f => f.Random.Words(10))
.RuleFor(o => o.CameraWindowLoad, f => f.Random.Double(0, 10))
.RuleFor(o => o.TemperatureControlEnabled, f => f.Random.Bool())
.RuleFor(o => o.TemperatureControlProbe, f => f.PickRandom<Model.ControlProbe>())
.RuleFor(o => o.Temperature, f => f.Random.Double(0, 10))
.RuleFor(o => o.IdleSpeed, f => f.Random.Double(0, 10))
.RuleFor(o => o.Index, f => f.IndexVariable);

            var tractionStepFaker = new Faker<TractionStep>()
                .RuleFor(o => o.Description, f => f.Random.Words(10))
                .RuleFor(o => o.TemperatureControlEnabled, f => f.Random.Bool())
                .RuleFor(o => o.TemperatureControlProbe, f => f.PickRandom<Model.ControlProbe>())
                .RuleFor(o => o.IdleSpeed, f => f.Random.Double(0, 10))
                .RuleFor(o => o.Index, f => f.IndexVariable);
            Items = mapperStepFaker.Generate(2).ToArray().Cast<Step>().Concat(tractionStepFaker.Generate(1)).ToArray();
            Key = nameof(MapperStep.Description);
        }
    }
}