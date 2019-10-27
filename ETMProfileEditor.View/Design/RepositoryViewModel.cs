using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Threading;

namespace ETMProfileEditor.View
{
    public class RepositoryViewModel
    {
        public RepositoryViewModel()
        {
            Items = Enumerable.Range(0, 10).Select(a => new { Key = a.ToString() }).ToArray();
            SelectedItem = Items.First();
        }

        public ICollection<object> Items { get; }

        public object SelectedItem { get; }
    }
}
