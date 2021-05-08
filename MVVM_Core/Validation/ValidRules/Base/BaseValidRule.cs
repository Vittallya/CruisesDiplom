using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.Validation
{
    public abstract class BaseValidRule<TValue, TRule> : IValidRule
            where TRule : class, IValidRule
    {
        public readonly string propName;

        public BaseValidRule(string propName, PredicatesBranchCollection<TValue> predicatesBranch)
        {
            this.propName = propName;
            _currentBranch = predicatesBranch;
        }

        public TRule Predicate(Func<TValue, bool> pred, string msg)
        {
            _currentBranch.AddPredicate(pred, msg);
            return this as TRule;
        }

        protected PredicatesBranchCollection<TValue> _currentBranch;
    }
}
