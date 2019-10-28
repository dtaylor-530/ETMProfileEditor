namespace ETMProfileEditor.View
{
    public class IndexedObject : Mvvm.BindableBase
    {
        private int index;
        private object value;

        //private bool selected;
        public IndexedObject(object o, int i)
        {
            this.value = o;
            this.index = i;
        }

        public int Index { get => index; set => SetProperty(ref index, value); }

        public object Value { get => value; set => SetProperty(ref this.value, value); }

    }
}