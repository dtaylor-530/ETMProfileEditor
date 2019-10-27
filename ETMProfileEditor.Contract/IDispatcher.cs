using System;

namespace ETMProfileEditor.Contract
{
    public interface IDispatcher
    {
        void InvokeAsync(Action action);
    }
}