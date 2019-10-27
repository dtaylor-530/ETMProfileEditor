using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace ETMProfileEditor.View
{
    /// <summary>
    /// Interaction logic for RepositoryView.xaml
    /// </summary>
    public partial class RepositoryView : UserControl
    {
        //private readonly SnackbarMessage undo;

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(RepositoryView), new PropertyMetadata(null, ItemTemplateChanged));

        private static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RepositoryView).ContentPresenter1.ContentTemplate = (DataTemplate)e.NewValue;
        }

        public static readonly RoutedEvent CollectionChangeEvent =
EventManager.RegisterRoutedEvent("CollectionChange", RoutingStrategy.Bubble,
typeof(TabsEventHandler), typeof(RepositoryView));

        public event TabsEventHandler CollectionChange
        {
            add { AddHandler(CollectionChangeEvent, value); }
            remove { RemoveHandler(CollectionChangeEvent, value); }
        }

        public delegate void TabsEventHandler(object sender, CollectionChangeEventArgs e);

        public RepositoryView()
        {
            InitializeComponent();
        }

        private void Undo_ActionClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonsDemoChip_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonsDemoChip_OnDeleteClick(object sender, RoutedEventArgs e)
        {
            SnackbarTwo.IsActive = true;
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            SnackbarTwo.IsActive = false;
        }

        private void Sample1_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            FruitTextBox.Text = string.Empty;
        }
    }
}