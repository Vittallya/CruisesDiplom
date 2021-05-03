using Admin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Admin.Core
{
    public class BindingBuilder<T, TControl>: IBinidingBuilder<T>
        where TControl: FrameworkElement, new()
    {
        private readonly TControl elem;

        public BindingBuilder(TControl elem)
        {
            this.elem = elem;
        }

        public BindingBuilder<T, TControl> UseBinding(Func<TControl, DependencyProperty> dp,
            Func<T, string> prop)
        {
            string name = prop(default(T));
            elem.SetBinding(dp(elem), name);

            return this;
        }
    }
}
