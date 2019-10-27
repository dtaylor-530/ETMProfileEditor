namespace ETMProfileEditor.Contract
{
    public interface IFactory<T>
    {
        T Build(string value);
    }
}