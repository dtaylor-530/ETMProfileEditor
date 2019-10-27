﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ETMProfileEditor.DAL.Common
{
    internal class ReflectionHelper
    {
        public static IEnumerable<Type> Filter(Type type) =>

       from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
       from assemblyType in domainAssembly.GetTypes()
       where type.IsAssignableFrom(assemblyType)
             && type != (assemblyType)
       select assemblyType;
    }
}