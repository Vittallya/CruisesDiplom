using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.Validation
{
    public class ComparableValidRule<TValue>: BaseValidRule<TValue, ComparableValidRule<TValue>>
        where TValue: IComparable
    {
        public ComparableValidRule(string propName, PredicatesBranchCollection<TValue> branch) : base(propName, branch)
        {
        }

        public ComparableValidRule<TValue> MoreEqualThan(TValue value, string msg = null)
        {
            if (msg == null)
                msg = $"Значение поля '{propName}' должно быть больше или равно значению '{value}'";

            _currentBranch.AddPredicate(x => x.CompareTo(value) >= 0, msg);
            return this;
        }

        public ComparableValidRule<TValue> MoreThan(TValue value, string msg = null)
        {
            if (msg == null)
                msg = $"Значение поля '{propName}' должно быть больше значения '{value}'";

            _currentBranch.AddPredicate(x => x.CompareTo(value) > 0, msg);
            return this;
        }

        public ComparableValidRule<TValue> EqualsTo(TValue value, string msg = null)
        {
            if (msg == null)
                msg = $"Значение поля '{propName}' должно быть равно значению '{value}'";

            _currentBranch.AddPredicate(x => x.CompareTo(value) == 0, msg);
            return this;
        }
        public ComparableValidRule<TValue> NotEqualsTo(TValue value, string msg = null)
        {
            if (msg == null)
                msg = $"Значение поля '{propName}' не должно быть равно значению '{value}'";

            _currentBranch.AddPredicate(x => x.CompareTo(value) != 0, msg);
            return this;
        }
        public ComparableValidRule<TValue> LessThan(TValue value, string msg = null)
        {
            if (msg == null)
                msg = $"Значение поля '{propName}' должно быть меньше или равно значению '{value}'";

            _currentBranch.AddPredicate(x => x.CompareTo(value) < 0, msg);
            return this;
        }
        public ComparableValidRule<TValue> LessEqualThan(TValue value, string msg = null)
        {
            if (msg == null)
                msg = $"Значение поля '{propName}' должно быть меньше значения '{value}'";

            _currentBranch.AddPredicate(x => x.CompareTo(value) <= 0, msg);
            return this;
        }
    }
}
