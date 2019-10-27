﻿
using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace Mvvm.Actions
{
    /// <summary>
    /// Sets a specified property to a value when invoked.
    /// </summary>
    public class SetterAction : TargetedTriggerAction<FrameworkElement>
    {
        #region Properties

        #region PropertyName

        /// <summary>
        /// Property that is being set by this setter.
        /// </summary>
        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(SetterAction),
            new PropertyMetadata(String.Empty));

        #endregion



        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(ConverterProperty); }
            set { SetValue(ConverterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Converter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConverterProperty =
            DependencyProperty.Register("Converter", typeof(IValueConverter), typeof(SetterAction), new PropertyMetadata(null));


        public object ConverterParameter
        {
            get { return (object)GetValue(ConverterParameterProperty); }
            set { SetValue(ConverterParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConverterParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConverterParameterProperty =
            DependencyProperty.Register("ConverterParameter", typeof(object), typeof(SetterAction), new PropertyMetadata(null));



        #region Value

        /// <summary>
        /// Property value that is being set by this setter.
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(SetterAction),
            new PropertyMetadata(null));

        #endregion

        #endregion

        #region Overrides

        protected override void Invoke(object parameter)
        {
            var target = TargetObject ?? AssociatedObject;

            var targetType = target.GetType();

            var property = targetType.GetProperty(PropertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            if (property == null)
                throw new ArgumentException(String.Format("Property not found: {0}", PropertyName));

            if (property.CanWrite == false)
                throw new ArgumentException(String.Format("Property is not settable: {0}", PropertyName));

            object convertedValue = null;


            var valueType = Value?.GetType();
            var propertyType = property.PropertyType;

            if (Converter != null)
            {
                convertedValue = Converter.Convert(parameter, null, ConverterParameter, null);
            }
            else if (valueType == propertyType)
            {
                convertedValue = Value;
            }
            else
            {
                var propertyConverter = TypeDescriptor.GetConverter(propertyType);

                if (propertyConverter.CanConvertFrom(valueType))
                    convertedValue = propertyConverter.ConvertFrom(Value);

                else if (valueType.IsSubclassOf(propertyType))
                    convertedValue = Value;

                else
                    throw new ArgumentException(String.Format("Cannot convert type '{0}' to '{1}'.", valueType, propertyType));
            }



            property.SetValue(target, convertedValue);
        }

        #endregion
    }
}

