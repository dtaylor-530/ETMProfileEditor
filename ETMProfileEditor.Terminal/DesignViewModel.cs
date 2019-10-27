using Bogus;
using Reactive.Bindings;
using System.Collections.Generic;
using System.Linq;

namespace ETMProfileEditor.Terminal
{
    using ViewModel;

    public class DesignViewModel
    {
        //public ReactiveProperty<bool> IsValid { get;  }

        public ICollection<Step> Items { get; }

        //public ICollection<Type> Types { get; }

        public string Key { get; }

        /// <summary>
        /// Track that has been selected by the user.
        /// </summary>
        public ReactiveProperty<Step> Selected { get; }

        /// <summary>
        /// Constructor for the MainWindowViewModel
        /// </summary>
        public DesignViewModel()
        {
            var testOrders = new Faker<MapperStep>()
//Ensure all properties have rules. By default, StrictMode is false
//Set a global policy by using Faker.DefaultStrictMode
.StrictMode(true)
//OrderId is deterministic
.RuleFor(o => o.Description, f => f.Random.Words(10))
//Pick some fruit from a basket
.RuleFor(o => o.CameraWindowLoad, f => f.Random.Double(0, 10))
//A random quantity from 1 to 10
.RuleFor(o => o.TemperatureControlEnabled, f => f.Random.Bool())
//A nullable int? with 80% probability of being null.
//The .OrNull extension is in the Bogus.Extensions namespace.
.RuleFor(o => o.TemperatureControlProbe, f => f.PickRandom<Model.ControlProbe>())
.RuleFor(o => o.Temperature, f => f.Random.Double(0, 10))
.RuleFor(o => o.IdleSpeed, f => f.Random.Double(0, 10))
.RuleFor(o => o.Index, f => f.IndexVariable);

            var testOrders2 = new Faker<TractionStep>()
//Ensure all properties have rules. By default, StrictMode is false
//Set a global policy by using Faker.DefaultStrictMode

//OrderId is deterministic
.RuleFor(o => o.Description, f => f.Random.Words(10))
//Pick some fruit from a basket
//A random quantity from 1 to 10
.RuleFor(o => o.TemperatureControlEnabled, f => f.Random.Bool())
//A nullable int? with 80% probability of being null.
//The .OrNull extension is in the Bogus.Extensions namespace.
.RuleFor(o => o.TemperatureControlProbe, f => f.PickRandom<Model.ControlProbe>())
.RuleFor(o => o.IdleSpeed, f => f.Random.Double(0, 10))

.RuleFor(o => o.Index, f => f.IndexVariable);
            Items = testOrders.Generate(2).ToArray().Cast<Step>().Concat(testOrders2.Generate(1)).ToArray();
            Key = nameof(MapperStep.Description);
        }
    }
}