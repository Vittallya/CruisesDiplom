using Admin.Core.Interfaces;
using Admin.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Admin.Core
{
    class PropertyControl<T> : IPropertyControl<T>
    {
        public PropertyControl(FrameworkElement control, string displayName, IValidRule validRule, string propertyName)
        {
            Control = control;
            DisplayName = displayName;
            ValidRule = validRule;
            PropertyName = propertyName;
        }

        public FrameworkElement Control { get; }

        public string DisplayName { get; }

        public IValidRule ValidRule { get; }

        public string PropertyName { get; }
    }
}
