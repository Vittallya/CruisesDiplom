using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.ValidationOld
{
    public interface ILinkedTypeValidRule<out T, TModel> : IValidPropertyBase<T> where TModel : class where T : IValidRule
    {
        T MustBeNull(string errMsg = null);
        T NotNull(string errMsg = null);
    }
}
