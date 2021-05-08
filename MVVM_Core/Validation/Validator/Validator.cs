using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.Validation
{
    public class Validator
    {
        public bool IsCorrect => CheckCorrect();

        public string[] ErrorMessages { get; private set; }
        public string ErrorMessage { get; private set; }

        private ICollection<IPredicatesBranchCollection> _branches = new List<IPredicatesBranchCollection>();


        /// <summary>
        /// В том же порядке, что и были заданы правила
        /// </summary>
        /// <param name="values">Значения, которые нужно проверить, соглассно заданным правилам валидации</param>
        /// <returns></returns>
        //public bool IsCorrectValues(params object[] values)
        //{
        //    int min = values.Length <= _branches.Count ? values.Length : _branches.Count;

        //}

        private bool CheckCorrect()
        {
            bool res = _branches.All(y => 
            {
                if (!y.IsCorrect)
                {
                    ErrorMessage = y.ErrorMessage;
                    return false;
                }
                return true;
            });

            return res;
        }


        public StringValidRule ForProperty(Func<string> valueGetter, string displayName)
        {
            var branchColl = new PredicatesBranchCollection<string>(valueGetter);
            _branches.Add(branchColl);
            return new StringValidRule(displayName, branchColl);
        }

        public ComparableValidRule<TValue> ForProperty<TValue>(Func<TValue> valueGetter, string displayName)
            where TValue: struct, IComparable
        {
            var branchColl = new PredicatesBranchCollection<TValue>(valueGetter);
            _branches.Add(branchColl);
            return new ComparableValidRule<TValue>(displayName, branchColl);
        }

    }
}
