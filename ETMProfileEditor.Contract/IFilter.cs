using System.Collections.Generic;

namespace ETMProfileEditor.Contract
{
    public interface IFilter<T, Tr>
    {
        IEnumerable<Tr> Filter(T t);
    }
}