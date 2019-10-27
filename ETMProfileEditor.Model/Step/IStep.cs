using System;
using System.Collections.Generic;
using System.Text;

namespace ETMProfileEditor.Model
{
    public interface IStep
    {
        string Description { get; set; }

        int Index { get; set; }
    }
}
