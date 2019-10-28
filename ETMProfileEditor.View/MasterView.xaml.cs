using Reactive.Bindings;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF.JoshSmith.ServiceProviders.UI;

namespace ETMProfileEditor.View
{
    /// <summary>
    /// Interaction logic for MasterView.xaml
    /// </summary>
    public partial class MasterView : UserControl
    {
        private ISubject<IEnumerable> ItemChanges = new Subject<IEnumerable>();
        private ISubject<string> KeyChanges = new Subject<string>();
        private ISubject<string> IndexChanges = new Subject<string>();

        public IEnumerable Types
        {
            get { return (IEnumerable)GetValue(TypesProperty); }
            set { SetValue(TypesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Types.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypesProperty =
            DependencyProperty.Register("Types", typeof(IEnumerable), typeof(MasterView), new PropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(MasterView), new PropertyMetadata(null));

        public ICommand RemoveItemCommand { get; } = new ReactiveCommand<object>();

        public ReactiveCollection<IndexedObject> Objects { get; }

        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable), typeof(MasterView), new PropertyMetadata(ItemsChanged));

        private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MasterView).ItemChanges.OnNext((IEnumerable)e.NewValue);
            (d as MasterView).Objects.Clear();
        }

        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Key.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(string), typeof(MasterView), new PropertyMetadata(null, KeyChanged));

        public string Index
        {
            get { return (string)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(string), typeof(MasterView), new PropertyMetadata(null, IndexChanged));

        private static void KeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MasterView).KeyChanges.OnNext((string)e.NewValue);
        }

        private static void IndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MasterView).IndexChanges.OnNext((string)e.NewValue);
        }

        public static readonly RoutedEvent CollectionChangeEvent =
        EventManager.RegisterRoutedEvent("CollectionChange", RoutingStrategy.Bubble,
        typeof(TabsEventHandler), typeof(MasterView));

        public event TabsEventHandler CollectionChange
        {
            add { AddHandler(CollectionChangeEvent, value); }
            remove { RemoveHandler(CollectionChangeEvent, value); }
        }

        protected virtual void RaiseClickEvent()
        {
            foreach (var item in ToolBar1.Items.Cast<Control>())
            {
                (item).GetBindingExpression(Control.IsEnabledProperty)?.UpdateTarget();
            }

            CollectionChangeEventArgs args = new CollectionChangeEventArgs(CollectionChangeEvent, Objects.ToDictionary(a => a.Value, a => a.Index));
            RaiseEvent(args);
        }

        public delegate void TabsEventHandler(object sender, CollectionChangeEventArgs e);

        public delegate void Tabs2EventHandler(object sender, SelectionChangeEventArgs e);

        public static readonly RoutedEvent SelectionChangeEvent = EventManager.RegisterRoutedEvent("SelectionChange", RoutingStrategy.Bubble, typeof(Tabs2EventHandler), typeof(MasterView));

        public event Tabs2EventHandler SelectionChange
        {
            add { AddHandler(SelectionChangeEvent, value); }
            remove { RemoveHandler(SelectionChangeEvent, value); }
        }

        protected virtual void RaiseClickEvent2(bool selected)
        {
            SelectionChangeEventArgs args = new SelectionChangeEventArgs(SelectionChangeEvent, (ListView.SelectedItem as IndexedObject)?.Value, selected);
            RaiseEvent(args);
        }

        //public ICollectionView CollectionView { get; }

        public MasterView()
        {
            InitializeComponent();
            this.Loaded += Window1_Loaded;

            DockPanel.DataContext = this;

            ListView.SelectionChanged += ListView_SelectionChanged;

            // Dictionary<object, object> dictionart = new Dictionary<object, object>();

            ItemChanges.Where(a => a == null).Subscribe(a =>
            {
                Objects.Clear();
                // dictionart = new Dictionary<object, object>();
            });

            var obs = ItemChanges.Where(a => a != null).Select(a => a.Cast<object>().Select((a, i) => (a, i)).ToObservable()).Switch();

            var xd = obs.CombineLatest(KeyChanges.Where(a => a != null), IndexChanges, (a, b, c) =>
            {
                var type = a.a.GetType();
                var pi = type.GetProperty(b);

                int index = c != null ? (int)type.GetProperty(c).GetValue(a.a) : a.i;
                Types = Types ?? ETMProfileEditor.Common.TypeHelper.Filter(type.BaseType).ToArray();

                return new IndexedObject(a.a, index);
                //    var xx = a.Cast<object>()
                //    .GroupBy(b =>
                //    pi.GetValue(b))
                //  .Select(a => a);
                //    List<IndexedObject> tempobjects = new List<IndexedObject>();
                //    foreach (var x in xx)
                //    {
                //        if (dictionart.ContainsKey(x.Key) == false)
                //        {
                //            dictionart.Add(x.Key, x.First());
                //            int index = tempobjects.Count();
                //            tempobjects.Add(new IndexedObject(x.First(), index));
                //        }
                //    }
                //    return tempobjects
                //    .ToObservable();
            });

            Objects = xd.ToReactiveCollection();

            //Objects = xd.SelectMany(a => a).ToReactiveCollection();

            var l = Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(ev => this.Loaded += ev, ev => this.Loaded -= ev);

            //Observable.FromEventPattern<System.Collections.Specialized.NotifyCollectionChangedEventHandler,
            //    System.Collections.Specialized.NotifyCollectionChangedEventArgs>(
            //           ev => Objects.CollectionChanged += ev,
            //           ev => Objects.CollectionChanged -= ev)
            //    .Select(e => e.EventArgs.NewItems.Where(a=>a!=null).Cast<IndexedObject>().First())

            //    .Take(1)
            //    //.CombineLatest(l, (a, b) => a)
            //    .Subscribe(obj =>
            //    {
            //        this.Dispatcher.InvokeAsync(() =>
            //        {
            //            //ListView.SelectedItem = obj;
            //            //RaiseClickEvent2(true);
            //        }, System.Windows.Threading.DispatcherPriority.Background);
            //    });

            //CollectionView = CollectionViewSource.GetDefaultView(Objects);
            //CollectionView.SortDescriptions.Add(new SortDescription("Index", ListSortDirection.Ascending));

            //ListView.Items.IsLiveSorting = true;
            //ListView.ItemsSource = icv;

            (RemoveItemCommand as ReactiveCommand<object>)
                .Subscribe(a =>
                {
                    Remove_Click(null, null);
                });
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            RaiseClickEvent();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var bject = ((IndexedObject)ListView.SelectedItem);

            var index = (ListView.SelectedItem as IndexedObject).Index;
            Objects.Remove(bject);
            if (ListView.Items.Cast<object>().Any())
            {
                ListView.SelectedItem = ListView.Items[0];
            }

            foreach (var x in ListView.Items.Cast<IndexedObject>().Where(a => a.Index > index))
            {
                x.Index -= 1;
            }

            RaiseClickEvent();
        }

        private void ListView_Selected(object sender, RoutedEventArgs e)
        {
            RaiseClickEvent2(true);
            RaiseClickEvent();
        }

        private void ListView_UnSelected(object sender, RoutedEventArgs e)
        {
            RaiseClickEvent2(false);
            RaiseClickEvent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var zz = e.RemovedItems.Cast<IndexedObject>()
                .Select(a => a.Value as INotifyDataErrorInfo)
                .Where(a => a != null)
                .Select(a =>
                {
                    return a;
                }).FirstOrDefault();

            if (zz?.HasErrors ?? false)
            {
                ListView.SelectedIndex = e.RemovedItems.Cast<IndexedObject>().First().Index;
            }

            RaiseClickEvent2(e.AddedItems.Cast<object>().Any());
            RaiseClickEvent();
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
            var bject = ((IndexedObject)ListView.SelectedItem);
            if (bject.Index != 0)
            {
                var index = ListView.Items.IndexOf(bject);
                (ListView.Items[index - 1] as IndexedObject).Index += 1;
                bject.Index -= 1;

                //CollectionView.Refresh();
            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            var bject = ((IndexedObject)ListView.SelectedItem);

            if (bject.Index != ListView.Items.Count - 1)
            {
                var index = ListView.Items.IndexOf(bject);
                (ListView.Items[index + 1] as IndexedObject).Index -= 1;
                bject.Index += 1;
                //CollectionView.Refresh();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var xx = Activator.CreateInstance(Types.Cast<Type>().Single(t => t.Name.Equals(((Button)e.Source).Content)));

            var index = ListView.SelectedIndex;
            foreach (var x in ListView.Items.Cast<IndexedObject>().Where(a => a.Index > index))
            {
                x.Index += 1;
            }
            this.Objects.Add((new IndexedObject(xx, index + 1)));
            //CollectionView.Refresh();
        }

        private ListViewDragDropManager<IndexedObject> dragMgr;
        private ICollectionView collectionView;

        #region Window1_Loaded

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            //// Give the ListView an ObservableCollection of Task
            //// as a data source.  Note, the ListViewDragManager MUST
            //// be bound to an ObservableCollection, where the collection's
            //// type parameter matches the ListViewDragManager's type
            //// parameter (in this case, both have a type parameter of Task).
            //ObservableCollection<Task> tasks = Task.CreateTasks();
            //this.listView.ItemsSource = tasks;

            //this.listView2.ItemsSource = new ObservableCollection<Task>();

            // This is all that you need to do, in order to use the ListViewDragManager.

            // Turn the ListViewDragManager on and off.
            //this.chkManageDragging.Checked += delegate { this.dragMgr.ListView = this.listView; };
            //this.chkManageDragging.Unchecked += delegate { this.dragMgr.ListView = null; };

            //// Show and hide the drag adorner.
            //this.chkDragAdorner.Checked += delegate { this.dragMgr.ShowDragAdorner = true; };
            //this.chkDragAdorner.Unchecked += delegate { this.dragMgr.ShowDragAdorner = false; };

            //// Change the opacity of the drag adorner.
            //this.sldDragOpacity.ValueChanged += delegate { this.dragMgr.DragAdornerOpacity = this.sldDragOpacity.Value; };

            //// Apply or remove the item container style, which responds to changes
            //// in the attached properties of ListViewItemDragState.
            //this.chkApplyContStyle.Checked += delegate { this.listView.ItemContainerStyle = this.FindResource("ItemContStyle") as Style; };
            //this.chkApplyContStyle.Unchecked += delegate { this.listView.ItemContainerStyle = null; };

            //// Use or do not use custom drop logic.
            //this.chkSwapDroppedItem.Checked += delegate { this.dragMgr.ProcessDrop += dragMgr_ProcessDrop; };
            //this.chkSwapDroppedItem.Unchecked += delegate { this.dragMgr.ProcessDrop -= dragMgr_ProcessDrop; };

            //// Show or hide the lower ListView.
            //this.chkShowOtherListView.Checked += delegate { this.listView2.Visibility = Visibility.Visible; };
            //this.chkShowOtherListView.Unchecked += delegate { this.listView2.Visibility = Visibility.Collapsed; };

            // Hook up events on both ListViews to that we can drag-drop
            // items between them.

            this.collectionView = (DockPanel.Resources["collectionView"] as System.Windows.Data.CollectionViewSource).View;
            this.dragMgr = new ListViewDragDropManager<IndexedObject>(this.ListView);
            this.ListView.DragEnter += OnListViewDragEnter;
            this.ListView.DragEnter += OnListViewDragEnter;
            this.ListView.Drop += OnListViewDrop;
            this.ListView.Drop += OnListViewDrop;
        }

        #endregion Window1_Loaded

        #region dragMgr_ProcessDrop

        // Performs custom drop logic for the top ListView.
        private void dragMgr_ProcessDrop(object sender, ProcessDropEventArgs<Task> e)
        {
            // This shows how to customize the behavior of a drop.
            // Here we perform a swap, instead of just moving the dropped item.

            int higherIdx = Math.Max(e.OldIndex, e.NewIndex);
            int lowerIdx = Math.Min(e.OldIndex, e.NewIndex);

            if (lowerIdx < 0)
            {
                // The item came from the lower ListView
                // so just insert it.
                e.ItemsSource.Insert(higherIdx, e.DataItem);
            }
            else
            {
                // null values will cause an error when calling Move.
                // It looks like a bug in ObservableCollection to me.
                if (e.ItemsSource[lowerIdx] == null ||
                    e.ItemsSource[higherIdx] == null)
                    return;

                // The item came from the ListView into which
                // it was dropped, so swap it with the item
                // at the target index.
                e.ItemsSource.Move(lowerIdx, higherIdx);
                e.ItemsSource.Move(higherIdx - 1, lowerIdx);
            }

            // Set this to 'Move' so that the OnListViewDrop knows to
            // remove the item from the other ListView.
            e.Effects = DragDropEffects.Move;
        }

        #endregion dragMgr_ProcessDrop

        #region OnListViewDragEnter

        // Handles the DragEnter event for both ListViews.
        private void OnListViewDragEnter(object sender, DragEventArgs e)
        {
            ListView.ItemsSource = Objects;
            e.Effects = DragDropEffects.Move;
        }

        #endregion OnListViewDragEnter

        #region OnListViewDrop

        // Handles the Drop event for both ListViews.
        private void OnListViewDrop(object sender, DragEventArgs e)
        {
            if (e.Effects == DragDropEffects.None)
                return;

            if (sender == this.ListView)
            {
                if (this.dragMgr.IsDragInProgress)
                {
                    ListView.ItemsSource = collectionView;
                    Reindex();
                    //collectionView.Refresh();
                    return;
                }
            }
        }

        private void Reindex()
        {
            int i = 0;
            foreach (var x in collectionView.SourceCollection.Cast<IndexedObject>())
            {
                x.Index = i; i++;
            }
        }

        #endregion OnListViewDrop
    }
}