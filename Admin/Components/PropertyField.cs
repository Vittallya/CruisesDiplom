using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Components
{
    public class PropertyField<T>
    {
        Func<T, object> _property;

        public string BindingPath { get; }



    }
}
