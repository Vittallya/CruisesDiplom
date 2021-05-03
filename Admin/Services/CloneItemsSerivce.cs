using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Services
{
    public class CloneItemsSerivce
    {
        public void Clone<T>(T original, T copy) where T: class
        {
            var type = typeof(T);

            var props = type.GetProperties();

            foreach(var property in props)
            {
                if(property.PropertyType.IsPrimitive || property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                {
                    var origValue = property.GetValue(original);
                    property.SetValue(copy, origValue);
                }
            }

        }
    }
}
