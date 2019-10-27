using ETMProfileEditor.Contract;
using System;

namespace ETMProfileEditor.View
{
    public class Dispatcher : IDispatcher
    {
        private readonly System.Windows.Threading.Dispatcher dispatcher;

        public Dispatcher(System.Windows.Threading.Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void InvokeAsync(Action action)
        {
            this.dispatcher.InvokeAsync(action);
        }
    }
}