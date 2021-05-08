using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.ValidationOld
{
    class ValueTypeValidRule<TValue> : ValidRule<IValueTypeValidRule<TValue>, TValue>, IValueTypeValidRule<TValue>
        where TValue : IComparable, IEquatable<TValue>
    {
        public ValueTypeValidRule(Func<TValue> value, string modelName) : base(value, modelName)
        {
        }
        public ValueTypeValidRule(string modelName) : base(modelName)
        {
        }

        public IValueTypeValidRule<TValue> Equial(TValue value, string msg)
        {
            _predicates.AddPredicate(x => x.Equals(value), msg);
            return this;
        }

        public IValueTypeValidRule<TValue> LessThan(TValue value, string msg)
        {
            _predicates.AddPredicate(x => x.CompareTo(value) == -1, msg);
            return this;
        }

        public IValueTypeValidRule<TValue> LessEquialThan(TValue value, string msg)
        {
            _predicates.AddPredicate(x => x.CompareTo(value) == -1 || x.CompareTo(value) == 0, msg);
            return this;
        }

        public IValueTypeValidRule<TValue> MoreThan(TValue value, string msg)
        {
            _predicates.AddPredicate(x => x.CompareTo(value) == 1, msg);
            return this;
        }

        public IValueTypeValidRule<TValue> MoreEquialThan(TValue value, string msg = null)
        {
            if (msg == null)
                msg = $"Значение свойства '{modelName}' должно быть больше или равно {value}";
            _predicates.AddPredicate(x => x.CompareTo(value) == 1 || x.CompareTo(value) == 0, msg);
            return this;
        }

        public IValueTypeValidRule<TValue> NotEquial(TValue value, string msg)
        {
            _predicates.AddPredicate(x => !x.Equals(value), msg);
            return this;
        }
    }
}
