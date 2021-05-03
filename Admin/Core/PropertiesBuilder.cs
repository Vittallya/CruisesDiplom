using Admin.Core;
using Admin.Core.Interfaces;
using Admin.Core.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Admin
{
    public class PropertiesBuilder<T> where T : class, new()
    {
        public bool IsEdit { get; private set; }
        public IEnumerable<IPropertyControl<T>> PropertyControls => propertyBuilders.Select(x => x.GetPropertyControl());

        private ICollection<IPropertyBuilder<T>> propertyBuilders = new List<IPropertyBuilder<T>>();

        public void AutoGenerate()
        {
        }

        //private void AddProperty(string propName, FrameworkElement element, IValidRule rule, string displayName = null)
        //{
        //    if (displayName == null)
        //        displayName = propName;

        //    PropertyControls.Add(new PropertyControl<T>(element, displayName, rule, propName));

        //}


        public IPropertyBuilder<T> AddValueTypeProperty<TProp>(Expression<Func<T, TProp>> propGetter, 
            string displayName = null, Func<IValueTypeValidRule<TProp>, IValueTypeValidRule<TProp>> validRule = null)

            where TProp: IComparable, IEquatable<TProp>
        {
            string propName = Helper.GetPropertyName(propGetter);

            IValueTypeValidRule<TProp> rule = null;

            if (validRule != null)
            {
                rule = new ValueTypeValidRule<TProp>(displayName);
                rule = validRule?.Invoke(rule);
            }

            var _currentBuilder = new PropertyBuilder<T>(propName, displayName, rule);
            propertyBuilders.Add(_currentBuilder);
            return _currentBuilder;
        }

        public IPropertyBuilder<T> AddStringProperty(Expression<Func<T, string>> propGetter, string displayName = null, Func<IStringValidRule, IStringValidRule> validRule = null)
        {
            string propName = Helper.GetPropertyName(propGetter);

            IStringValidRule rule = null;

            if (validRule != null)
            {
                rule = new StringValidRule(displayName);
                rule = validRule?.Invoke(rule);
            }

            var _currentBuilder = new PropertyBuilder<T>(propName, displayName, rule);
            propertyBuilders.Add(_currentBuilder);
            return _currentBuilder;
        }

        internal void Clear()
        {
            propertyBuilders?.Clear();
        }
    }
}