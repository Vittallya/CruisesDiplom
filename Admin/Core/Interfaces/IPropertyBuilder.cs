using Admin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Admin.Core.Interfaces
{
    public interface IPropertyBuilder<T>
    {
        IPropertyControl<T> GetPropertyControl();
        void UseControl<TControl>(TControl elem = null, DependencyProperty bindingProp = null)
            where TControl : FrameworkElement, new();

        void UseCombobox<TControl, TItem>(IEnumerable<TItem> items, 
            Expression<Func<TItem, object>> valuePath, Expression<Func<TItem, object>> displayMember )
            where TControl : Selector, new();

        void AutoGenerate();
    }
}
