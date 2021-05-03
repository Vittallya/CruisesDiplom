using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Core.Validation
{

    abstract class LinkedTypeValidRule<TCl, TModel> : ValidRule<TCl, TModel>, ILinkedTypeValidRule<TCl, TModel>
        where TCl : class, IValidRule
        where TModel : class
    {
        public LinkedTypeValidRule(Func<TModel> value, string modelName) : base(value, modelName)
        {
        }
        
        public LinkedTypeValidRule(string modelName) : base( modelName)
        {
        }

        public TCl NotNull(string errMsg = null)
        {
            if (errMsg == null)
                errMsg = $"Значение свойства '{modelName}' не должно иметь значение null";

            _predicates.AddPredicate(x => x != null, errMsg);
            return this as TCl;
        }

        public TCl MustBeNull(string errMsg = null)
        {
            if (errMsg == null)
                errMsg = $"Значение свойства '{modelName}' должно иметь значение null";

            _predicates.AddPredicate(x => x == null, errMsg);
            return this as TCl;
        }
    }
}
