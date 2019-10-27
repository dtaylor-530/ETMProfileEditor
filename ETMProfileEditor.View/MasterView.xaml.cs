using Reactive.Bindings;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System;
using System.Windows.Input;

namespace ETMProfileEditor.View
{
    /// <summary>
    /// Interaction logic for MasterView.xaml
    /// </summary>
    public partial class MasterView : UserControl
    {
        private ISubject<IEnumerable> ItemChanges = new Subject<IEnumerable>();
        private ISubject<string> KeyChanges = new Subject<string>();





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


        private ReactiveCollection<IndexedObject> objects;

        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable), typeof(MasterView), new PropertyMetadata(ItemsChanged));

        private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MasterView).ItemChanges.OnNext((IEnumerable)e.NewValue);
        }

        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Key.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(string), typeof(MasterView), new PropertyMetadata(null, KeyChanged));

        private static void KeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MasterView).KeyChanges.OnNext((string)e.NewValue);
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

            CollectionChangeEventArgs args = new CollectionChangeEventArgs(CollectionChangeEvent, objects.ToDictionary(a => a.Value, a => a.Index));
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





        public ICollectionView CollectionView { get; }


        public MasterView()
        {
            InitializeComponent();
            DockPanel.DataContext = this;

            ListView.SelectionChanged += ListView_SelectionChanged;

            Dictionary<object, object> dictionart = new Dictionary<object, object>();

            ItemChanges.Where(a => a == null).Subscribe(a =>
            {
                objects.Clear();
                dictionart = new Dictionary<object, object>();
            });

            var xd = ItemChanges.Where(a => a != null).CombineLatest(KeyChanges.Where(a => a != null), (a, b) =>
        {
            var type = a.Cast<object>().First().GetType();
            var pi = type.GetProperty(b);
            Types = Types ?? ETMProfileEditor.Common.TypeHelper.Filter(type).ToArray();

            var xx = a.Cast<object>()
            .GroupBy(b =>
            pi.GetValue(b))
          .Select(a => a);
            List<IndexedObject> tempobjects = new List<IndexedObject>();
            foreach (var x in xx)
            {
                if (dictionart.ContainsKey(x.Key) == false)
                {
                    dictionart.Add(x.Key, x.First());
                    int index = tempobjects.Count();
                    tempobjects.Add(new IndexedObject(x.First(), index));
                }
            }
            return tempobjects
            .ToObservable();


            //.Scan((0,default),(a,b)=>((a.Item1 += 1,a.Item2.Single(),b)))
            //.Scan((0, default(object)), (a, b) => (a.Item1 + 1, b))
            //.Select(a => new IndexedObject(a.Item2, a.Item1));
        });






            objects = xd.SelectMany(a => a).ToReactiveCollection();

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



            CollectionView = CollectionViewSource.GetDefaultView(objects);
            CollectionView.SortDescriptions.Add(new SortDescription("Index", ListSortDirection.Ascending));

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
            objects.Remove(bject);
            if (ListView.Items.Cast<object>().Any())
            {
                ListView.SelectedItem = ListView.Items[0];
            }

            foreach (var x in ListView.Items.Cast<IndexedObject>().Where(a => a.Index > index))
            {
                x.Index -= 1;
            }
            CollectionView.Refresh();
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

                CollectionView.Refresh();
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
                CollectionView.Refresh();
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
            this.objects.Add((new IndexedObject(xx, index + 1)));
            CollectionView.Refresh();
        }
    }
}