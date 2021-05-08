using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.Validation
{
    public class PredicatesBranchCollection<TValue>: IPredicatesBranchCollection
    {
        private ICollection<PredicatesBranch<TValue>> _branches;

        PredicatesBranch<TValue> _currentBranch;


        private Func<TValue> valueGetter;

        public PredicatesBranchCollection(Func<TValue> valueGetter)
        {
            this.valueGetter = valueGetter;
            _branches = new List<PredicatesBranch<TValue>>();
            SwitchToNewBranch();
        }

        public string ErrorMessage { get; private set; }
        public string[] ErrorMessages { get; private set; }

        public bool IsCorrect => CheckCorrect();

        public void SwitchToNewBranch()
        {
            _currentBranch = new PredicatesBranch<TValue>();
            _branches.Add(_currentBranch);
        }
        
        public void AddPredicate(Func<TValue, bool> predicate, string errMsg = null)
        {
            _currentBranch.AddPredicate(predicate, errMsg);
        }


        private bool CheckCorrect()
        {
            if (valueGetter == null)
                throw new ArgumentException();

            return IsCorrectValue(valueGetter());
        }

        public bool IsCorrectValue(object value)
        {
            bool res = false;            

            if (value == default)
                res = _branches.Any(y => y.IsCorrectValue(default));
            else if (value is TValue param)
                res = _branches.Any(y => y.IsCorrectValue(param));
            else
                return false;

            if (!res)
            {
                //Если не сработала ни одна ветка, берем сообщение из первой
                ErrorMessage = _branches.First().ErrorMessage;
            }
            return res;
        }
    }
}
