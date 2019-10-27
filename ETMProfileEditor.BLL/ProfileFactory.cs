using ETMProfileEditor.Contract;
using System.Linq;

namespace ETMProfileEditor.BLL
{
    using ViewModel;

    public class ProfileFactory : IFactory<Profile>
    {
        private readonly ISelect<ETMProfileEditor.Model.Limit> limits;

        public ProfileFactory(ISelect<ETMProfileEditor.Model.Limit> limits)
        {
            this.limits = limits;
        }

        public Profile Build(string value)
        =>
            new ViewModel.Profile()
            {
                Name = value,
                Description = "This is a description",
                Steps = new Step[] { new MapperStep(0, "This is a Mapper description", limits), new TractionStep(1, "This is a description One", limits), new TractionStep(2, "This is a description Two", limits) }.Cast<Step>().ToArray()
            };
    }
}