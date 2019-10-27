using ETMProfileEditor.Contract;

using ETMProfileEditor.ViewModel;
using System;
using System.Linq;

namespace ETMProfileEditor.BLL
{
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
                Steps = new Step[] { new MapperStep(0, "This is a desctription", limits), new TractionStep(1, "This is a desctription", limits), new TractionStep(2, "This is a desctription", limits) }.Cast<Step>().ToArray()
            };
        
    }
}
