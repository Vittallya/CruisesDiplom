using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.Validation
{
    public interface IPredicatesBranchCollection
    {
        string ErrorMessage { get; }
        string[] ErrorMessages { get; }
        bool IsCorrect { get; }
        bool IsCorrectValue(object param);
    }
}
