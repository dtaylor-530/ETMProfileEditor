using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace ETMProfileEditor.View.Common
{
    internal class DataTemplateFactory
    {
        public DataTemplate Create(Type type)
        {
            StringReader stringReader = new StringReader(
            $@"<DataTemplate         xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">  <TextBlock  Text='{type.Name}'/>   </DataTemplate>");
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as DataTemplate;
        }
    }
}