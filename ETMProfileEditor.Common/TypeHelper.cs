using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETMProfileEditor.Common
{
    public class TypeHelper
    {
        public static IEnumerable<Type> Filter<T>() => Filter(typeof(T));

        public static IEnumerable<Type> Filter(Type type) =>

        from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
        from assemblyType in domainAssembly.GetTypes()
        where type.IsAssignableFrom(assemblyType)
              && type != (assemblyType)
        select assemblyType;
    }
}

