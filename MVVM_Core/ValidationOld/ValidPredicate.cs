using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.ValidationOld
{
    class ValidPredicate<T>
    {
        public Predicate<T> Predicate { get; }

        public string ErrorMessage { get; }

        internal ValidPredicate(Predicate<T> predicate, string errorMessage)
        {
            Predicate = predicate;
            ErrorMessage = errorMessage;
        }

        public bool Check(T param) => Predicate?.Invoke(param) ?? false;

    }

    class ValidPredicateCollection<T>
    {
        private List<List<ValidPredicate<T>>> _predicates = new List<List<ValidPredicate<T>>>();

        private int currentBranch = 0;

        public void AddPredicate(Predicate<T> predicate, string errorMessage)
        {
            if (_predicates.Count <= currentBranch)
            {
                _predicates.Add(new List<ValidPredicate<T>>());
            }

            _predicates[currentBranch].Add(new ValidPredicate<T>( predicate, errorMessage));
        }
        public void AddOrPredicate(Predicate<T> predicate, string errorMessage)
        {

            AddPredicate(predicate, errorMessage);
        }

        public void SetupNewBranch()
        {
            if (_predicates[currentBranch].Count > 0)
                currentBranch++;            
        }

        public bool TryValid(T value, out string[] messages)
        {
            List<string> messagesList = new List<string>();

            int trueCount = 0;

            bool branchIsCorrect = false;

            foreach(var branch in _predicates)
            {
                branchIsCorrect = true;

                foreach(var p in branch)
                {
                    if (!p.Check(value))
                    {
                        branchIsCorrect = false;
                        messagesList.Add(p.ErrorMessage);
                    }
                }
                trueCount += Convert.ToInt32(branchIsCorrect);
            }

            messages = messagesList.ToArray();
            return trueCount > 0;
        }
    }
}
