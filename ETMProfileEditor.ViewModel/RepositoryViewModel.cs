using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;


namespace ETMProfileEditor.ViewModel
{
    using Contract;

    public class RepositoryViewModel
    {
        private readonly IDispatcher dispatcher;

        public ICommand UndoCommand { get; }

        public ICommand AddCommand { get; }
        public ICommand NameChangedCommand { get; }


        public RepositoryViewModel(IDispatcher dispatcher, IFactory<Profile> profileFactory)
        {
            UndoCommand = new ReactiveCommand();
            AddCommand = new ReactiveCommand<bool>();
            NameChangedCommand = new ReactiveCommand<string>();

            this.dispatcher = dispatcher;
            //Items = new ReactiveCollection<SelectDeleteItem>(Enumerable.Range(0, 10).Select(a => new SelectDeleteItem()).ToObservable()); ;

            //var undos = Items.ToCollectionChanged().Select(a => a.Value.Undo.Where(a => a).Select(b => new KeyValuePair<string, SelectDeleteItem>("Undo", a.Value))).SelectMany(a => a);
            var deletes = Items.ToCollectionChanged().Select(a => a.Value.Delete.Where(c => c).Select(b => new KeyValuePair<string, SelectDeleteItem>("Delete", a.Value))).SelectMany(a => a);
            var selects = Items.ToCollectionChanged().Select(a => a.Value.Select/*.Where(a => a)*/.Select(b => new KeyValuePair<bool, SelectDeleteItem>(b, a.Value))).SelectMany(a => a);

            var selects2 = Items.ToCollectionChanged().Where(a => Items.Count == 1).Select(a => Items.Single()).Select(b => new KeyValuePair<bool, SelectDeleteItem>(true, b));
            var selects3 = Items.ToCollectionChanged().Where(a => Items.Count == 0 && a.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                .Select(a => a.Value).Select(b => new KeyValuePair<bool, SelectDeleteItem>(true, b));

            //selects2.DelaySubscription(TimeSpan.FromSeconds(1)).Subscribe(a =>
            //{

            //});

            //selects3.Subscribe(a =>
            //{

            //});

            this.Deletes = deletes.Select(a => a.Value).ToReactiveCollection();

            deletes.Select(a => a.Value).Subscribe(a => Items.Remove(a));

            (UndoCommand as ReactiveCommand).Subscribe(a =>
            {
                if (Deletes.Any())
                {
                    var last = Deletes.Last();
                    Deletes.Remove(last);
                    last.Delete.Value = false;
                    last.Select.Value = Items.Count() == 0;
                    Items.Add(last);
                }
            });

            //(AddCommand as ReactiveCommand<bool>).Subscribe(a =>
            //{
            //    bool parameter = (bool)a;
            //    if (!Equals(parameter, true)) return;

            //    //if (!string.IsNullOrWhiteSpace(FruitTextBox.Text))
            //    //    FruitListBox.Items.Add(FruitTextBox.Text.Trim());
            //});        

            (AddCommand as ReactiveCommand<bool>)
                .Where(b=>b)
                .Zip((NameChangedCommand as ReactiveCommand<string>), (bl,text) => text)
                .Subscribe(text =>
                 {
                     Items.Add(new SelectDeleteItem(
                                new
                                {
                                    Profile = profileFactory.Build(text),
                                    Key = nameof(Step.Description),
                                    Types = Common.TypeHelper.Filter<ViewModel.Step>().ToArray()
                 }));


               
                 });
           
            SelectedItem = selects.Merge(selects2).Where(a => a.Key).Select(a => a.Value).ToReactiveProperty();
            //mode: ReactivePropertyMode.DistinctUntilChanged
            selects.Subscribe(a =>
            {
                if (a.Key)
                {
                    foreach (var x in Items.Where(b => b != a.Value))
                    {
                        x.Select.Value = false;
                    }
                }
                else if (a.Key == false && SelectedItem.Value == a.Value && Items.Count != 1)
                {
                    this.SelectedItem.Value = null;
                }
            });

            SelectedItem.Subscribe(a =>
            {
                //this.dispatcher.InvokeAsync(() => this.SelectedItem.Value = a);
            });

            // For some reason doesn't pick up initial selecteditem
            SelectedItem.Where(a => a != null).Take(1).Subscribe(a =>
            {
                 this.dispatcher.InvokeAsync(() => this.SelectedItem.Value = a);
            });
        }
        public ReactiveProperty<SelectDeleteItem> SelectedItem { get; }

        public ReactiveCollection<SelectDeleteItem> Items { get; } = new ReactiveCollection<SelectDeleteItem>();

        public ReactiveCollection<SelectDeleteItem> Deletes { get; }
    }


}

