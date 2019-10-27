//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace ETMProfileEditor.Terminal
//{
//    class TypeHelper
//    {
//        public static IEnumerable<Type> Filter<T>()=>

//        from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
//        let type = typeof(T)
//        from assemblyType in domainAssembly.GetTypes()
//        where type.IsAssignableFrom(assemblyType)
//              && type != (assemblyType)
//        select assemblyType;
//    }
//}
