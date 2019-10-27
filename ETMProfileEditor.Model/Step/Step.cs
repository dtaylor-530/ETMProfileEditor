namespace ETMProfileEditor.Model
{
    public abstract class Step : IStep
    {
        public string Description { get; set; }
        public int Index { get; set; }
    }
}