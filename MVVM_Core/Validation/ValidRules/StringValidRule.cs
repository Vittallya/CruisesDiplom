using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.Validation
{
    public class StringValidRule : BaseValidRule<string, StringValidRule>
    {
        public StringValidRule(string propName, PredicatesBranchCollection<string> predicatesBranch) : base(propName, predicatesBranch)
        {
        }

        public StringValidRule LengthLessThan(int len, string msg = null)
        {
            if (msg == null)
            {
                msg = $"Длина поля '{propName}' должна быть меньше {len} симв.";
            }
            _currentBranch.AddPredicate(s => s == null || s != null && s.Length < len, msg);
            return this;
        }

        public StringValidRule LengthLessEqualThan(int len, string msg = null)
        {
            if (msg == null)
            {
                msg = $"Длина поля '{propName}' должна быть меньше или равна {len} симв.";
            }
            _currentBranch.AddPredicate(s => s == null || s != null && s.Length <= len, msg);
            return this;
        }

        public StringValidRule LengthMoreThan(int len, string msg = null)
        {
            if(msg == null)
            {
                msg = $"Длина поля '{propName}' должна быть больше {len} симв.";
            }
            _currentBranch.AddPredicate(s => s != null && s.Length > len, msg);
            return this;
        }

        public StringValidRule LengthMoreEqualThan(int len, string msg = null)
        {
            if (msg == null)
            {
                msg = $"Длина поля '{propName}' должна быть больше или равна {len} симв.";
            }

            _currentBranch.AddPredicate(s => s != null && s.Length >= len, msg);
            return this;
        }

        public StringValidRule NotEmpty(string msg = null)
        {
            if (msg == null)
            {
                msg = $"Поле '{propName}' не должно быть пустой строкой";
            }

            _currentBranch.AddPredicate(s => s != null && s.Length > 0, msg);
            return this;
        }
    }
}
