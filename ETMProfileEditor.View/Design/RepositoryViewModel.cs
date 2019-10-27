using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

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