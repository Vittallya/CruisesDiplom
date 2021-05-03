using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Core
{
    public class Column
    {
        public Column(string columnName, string bindingPath)
        {
            ColumnName = columnName;
            BindingPath = bindingPath;

            if (bindingPath != null && bindingPath.Length >= 3 && bindingPath.Contains('.'))
                IsComplexProperty = true;
        }

        public string ColumnName { get; set; }

        public string BindingPath { get; set; }
        public bool IsComplexProperty { get; }

        
    }
}
