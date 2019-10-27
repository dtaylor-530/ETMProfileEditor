using System;
using System.Collections.Generic;
using System.Text;

namespace ETMProfileEditor.Contract
{
    public interface IDispatcher
    {
        void InvokeAsync(Action action);
    }
}
