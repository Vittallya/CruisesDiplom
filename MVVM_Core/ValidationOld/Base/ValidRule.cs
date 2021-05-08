using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.ValidationOld
{

    abstract class ValidRule<TClass, TModel> : IValidPropertyBase<TClass>
        where TClass : class, IValidRule
    {
        protected readonly Func<TModel> valueGetter;
        protected readonly string modelName;

        protected ValidPredicateCollection<TModel> _predicates = new ValidPredicateCollection<TModel>();
        private string[] messages;

        protected ValidRule(Func<TModel> value, string modelName): this(modelName)
        {
            this.valueGetter = value;
        }

        protected ValidRule(string modelName)
        {
            this.modelName = modelName;
        }

        public string[] Messages => messages;

        public bool IsCorrect => TryValidate();

        public TClass Or { get { _predicates.SetupNewBranch(); return this as TClass; } }

        public bool IsCorrectValue(object value)
        {
            return _predicates.TryValid((TModel)value, out messages);
        }

        private bool TryValidate()
        {
            return _predicates.TryValid(valueGetter.Invoke(), out messages);
        }
    }
}
