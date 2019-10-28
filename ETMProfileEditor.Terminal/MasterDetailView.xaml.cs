using System.Windows;
using System.Windows.Controls;

namespace ETMProfileEditor.Terminal
{
    /// <summary>
    /// Interaction logic for MasterDetailView.xaml
    /// </summary>
    public partial class MasterDetailView : UserControl
    {
        public MasterDetailView()
        {
            InitializeComponent();
        }

        public static readonly RoutedEvent CollectionChangeEvent =
            EventManager.RegisterRoutedEvent("CollectionChange", RoutingStrategy.Bubble,
typeof(TabsEventHandler), typeof(MasterDetailView));

        public event TabsEventHandler CollectionChange
        {
            add { AddHandler(CollectionChangeEvent, value); }
            remove { RemoveHandler(CollectionChangeEvent, value); }
        }

        public delegate void TabsEventHandler(object sender, View.CollectionChangeEventArgs e);

        private void MasterView1_CollectionChange(object sender, View.CollectionChangeEventArgs e)
        {
            RaiseEvent(new View.CollectionChangeEventArgs(CollectionChangeEvent, e.Dictionary));
        }
    }
}