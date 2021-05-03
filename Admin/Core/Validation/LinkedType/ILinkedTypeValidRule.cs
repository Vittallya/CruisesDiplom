using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Core.Validation
{
    public interface ILinkedTypeValidRule<out T, TModel> : IValidPropertyBase<T> where TModel : class where T : IValidRule
    {
        T MustBeNull(string errMsg = null);
        T NotNull(string errMsg = null);
    }
}
