using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.ValidationOld
{
    public interface IValidRule
    {
        string[] Messages { get; }
        bool IsCorrect { get; }
        bool IsCorrectValue(object value);
    }
}
