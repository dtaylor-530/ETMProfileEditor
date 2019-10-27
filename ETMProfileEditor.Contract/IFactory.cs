using System;
using System.Collections.Generic;
using System.Text;

namespace ETMProfileEditor.Contract
{
    public interface IFactory<T>
    {
        T Build(string value);

    }
}
