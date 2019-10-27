using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ETMProfileEditor.View
{
    public class SelectionChangeEventArgs : RoutedEventArgs
    {
        public object Selection { get; }

        public bool IsSelected { get; }

        public SelectionChangeEventArgs(RoutedEvent routedEvent, object selection, bool isSelected) : base(routedEvent)
        {
            Selection = selection;
            IsSelected = isSelected;
        }
    }

}
