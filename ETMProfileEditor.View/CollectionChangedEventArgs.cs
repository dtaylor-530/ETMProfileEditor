using System.Collections.Generic;
using System.Windows;

namespace ETMProfileEditor.View
{
    public class CollectionChangeEventArgs : RoutedEventArgs
    {
        public Dictionary<object, int> Dictionary { get; }

        public CollectionChangeEventArgs(RoutedEvent routedEvent, Dictionary<object, int> dictionary) : base(routedEvent)
        {
            this.Dictionary = dictionary;
        }
    }
}