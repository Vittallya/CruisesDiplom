using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVVM_Core.ValidationOld
{
    public class ValidConfig
    {
        public IStringValidRule ForProperty(Func<string> func, string name)
        {
            var prop = new StringValidRule(func, name);                
            return prop;
        }
        public IStringValidRule ForProperty<TItem>(Func<TItem> func, string name) where TItem : class, new()
        {
            throw new NotImplementedException();
        }
    }




}
