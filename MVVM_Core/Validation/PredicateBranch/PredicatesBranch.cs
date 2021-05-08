using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.Validation
{
    public class PredicatesBranch<TValue>: IPredicatesBranch
    {
        private ICollection<KeyValuePair<Func<TValue, bool>, string>> _predicates = new List<KeyValuePair<Func<TValue, bool>, string>>();


        public void AddPredicate(Func<TValue, bool> predicate, string errMsg = null)
        {            
            _predicates.Add(new KeyValuePair<Func<TValue, bool>, string>(predicate, errMsg));
        }

        public string ErrorMessage { get; private set; }

        public string[] ErrorMessages { get; private set; }

        public PredicatesBranch()
        {
        }

        public bool IsCorrectValue(TValue value)
        {
            bool res = _predicates.All(x => x.Key.Invoke(value));

            var nonCorrect = _predicates.
                SkipWhile(x => x.Key.Invoke(value)).
                Select(x => x.Value).
                ToList();

            if (nonCorrect.Count > 0)
            {
                ErrorMessage = nonCorrect.First();
            }
            return res;
        }

    }

    //public class PredicatesBranch
    //{
    //    private ICollection<KeyValuePair<Func<bool>, string>> _predicates = new List<KeyValuePair<Func<bool>, string>>();

    //    public void AddPredicate(Func<bool> predicate, string errMsg = null)
    //    {
    //        _predicates.Add(new KeyValuePair<Func<bool>, string>(predicate, errMsg));
    //    }

    //    public string ErrorMessage { get; private set; }

    //    public string[] ErrorMessages { get; private set; }

    //    private bool CheckCorrect()
    //    {
    //        bool res = _predicates.All(x => x.Key.Invoke());

    //        if (!res)
    //        {
    //            ErrorMessages = _predicates.
    //                Select(x => x.Value).
    //                Where(x => x != null).
    //                ToArray();

    //            ErrorMessage = ErrorMessages.First();
    //        }
    //        return res;
    //    }

    //    public bool IsCorrect => CheckCorrect();
    //}

}
