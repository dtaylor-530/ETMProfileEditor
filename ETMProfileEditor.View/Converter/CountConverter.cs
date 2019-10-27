using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Linq;
using System.Windows;

namespace ETMProfileEditor.View
{
    internal class CountConverter : IValueConverter
    {
        public bool Invert { get; set; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            return ((value as IEnumerable).Cast<object>().Count() <= System.Convert.ToInt32(parameter)) ^ Invert;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}