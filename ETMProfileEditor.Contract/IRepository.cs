using System.Collections.Generic;

namespace ETMProfileEditor.Contract
{
    public interface IRepository<T, Tr> : ISelect<T>
    {
        T Find(Tr tr);

        void UpSert(T profile);

        void Delete(T profile);
    }

    public interface ISelect<T>
    {
        IEnumerable<T> Select();
    }
}