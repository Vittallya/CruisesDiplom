using Admin.Core.Interfaces;
using Admin.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Admin.Core
{
    public class PropertyBuilder<T>: IPropertyBuilder<T>
    {
        private readonly string propName;
        private readonly string displayName;
        private readonly IValidRule rule;
        private FrameworkElement control;

        public PropertyBuilder(string propName, string displayName, IValidRule rule)
        {
            this.propName = propName;
            this.displayName = displayName;
            this.rule = rule;
        }

        public IPropertyControl<T> GetPropertyControl()
        {
            if (control == null)
                AutoGenerate();

            return new PropertyControl<T>(control, displayName, rule, propName);
        }

        public void UseControl<TControl>(TControl elem = null, DependencyProperty bindingProp = null) 
            where TControl: FrameworkElement, new()
        {
            if (elem == null)
                elem = new TControl();

            control = elem;



            if (bindingProp == null)
                SetDefaultBinding(elem, propName);
            else
                elem.SetBinding(bindingProp, propName);

        }

        void SetDefaultBinding<TControl>(TControl control, string propName)
            where TControl : FrameworkElement, new()
        {
            if (control is TextBox)
                control.SetBinding(TextBox.TextProperty, propName);

            else if (control is DatePicker)
                control.SetBinding(DatePicker.SelectedDateProperty, propName);
            else if (control is Selector)
            {
                control.SetBinding(Selector.SelectedValueProperty, propName);
            }
        }

        public void AutoGenerate()
        {
            PropertyInfo info = (typeof(T)).GetProperty(propName);

            if(info.PropertyType.IsPrimitive || info.PropertyType.IsValueType || info.PropertyType == typeof(string))
            {
                control = new TextBox();
                SetDefaultBinding(control, propName);
            }
            
            else if(info.PropertyType == typeof(DateTime))
            {
                control = new DatePicker() { DisplayDateStart = new DateTime(1960, 1, 1) };
                SetDefaultBinding(control, propName);
            }

        }

        public void UseCombobox<TControl, TItem>(
            IEnumerable<TItem> items, Expression<Func<TItem, object>> valuePath, Expression<Func<TItem, object>> displayMember) 
            where TControl : Selector, new()
        {
            var combo = new TControl();
            combo.ItemsSource = items;
            combo.DisplayMemberPath = Helper.GetPropertyName(displayMember);
            combo.SelectedValuePath = Helper.GetPropertyName(valuePath);
            combo.SetBinding(Selector.SelectedValueProperty, propName);

            control = combo;
        }
    }
}
