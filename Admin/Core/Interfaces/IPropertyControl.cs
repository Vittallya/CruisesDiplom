using Admin.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Admin.Core.Interfaces
{
    

    public interface IPropertyControl<TModel> 
    {
        FrameworkElement Control { get; }

        string DisplayName { get; }

        string PropertyName { get; }

        IValidRule ValidRule { get; }
    }
}
