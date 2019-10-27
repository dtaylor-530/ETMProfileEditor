using System.Collections.Generic;

namespace ETMProfileEditor.Model
{
    public class Profile
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<IStep> MapperSteps { get; set; }
    }
}