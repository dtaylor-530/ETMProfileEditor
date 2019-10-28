using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;

namespace ETMProfileEditor.ViewModel
{
    public class SelectDeleteItem : IEquatable<SelectDeleteItem>
    {
        //public string Key { get; } = GetString(Guid.NewGuid().ToString().Take(9));

        public ICommand SelectCommand { get; } = new ReactiveCommand();

        public ICommand DeleteCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<bool> Select { get; }

        public ReactiveProperty<bool> Delete { get; }

        public object Value { get; }

        public SelectDeleteItem(object value)
        {
            Value = value;
            //(DeleteCommand as ReactiveCommand).Subscribe(a =>
            //{
            //});

            //(SelectCommand as ReactiveCommand).Subscribe(a =>
            //{
            //});
            //(UndoCommand as ReactiveCommand).Subscribe(a =>
            //{
            //});

            Delete = (DeleteCommand as ReactiveCommand).Select(a => true).ToReactiveProperty();
            Select = (SelectCommand as ReactiveCommand).Select(a => true).Merge(Delete.Select(a => false)).ToReactiveProperty();
        }

        public bool Equals(SelectDeleteItem other)
        {
            return this.Value == other?.Value;
            ;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as SelectDeleteItem);
        }

        public override int GetHashCode()
        {
            return Value.ToString().Length;
        } 
    }
}