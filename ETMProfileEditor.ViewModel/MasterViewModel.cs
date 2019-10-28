using Reactive.Bindings;
using System.Collections.ObjectModel;

namespace ETMProfileEditor.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<Step> Items { get; } = new ObservableCollection<Step>();

        /// <summary>
        /// Track that has been selected by the user.
        /// </summary>
        public ReactiveProperty<Step> Selected { get; }

        public string Key { get; }

        /// <summary>
        /// Constructor for the MainWindowViewModel
        /// </summary>
        public MainViewModel()
        {
        }
    }
}