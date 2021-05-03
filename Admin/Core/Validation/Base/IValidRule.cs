using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Core.Validation
{
    public interface IValidRule
    {
        string[] Messages { get; }
        bool IsCorrect { get; }
        bool IsCorrectValue(object value);
    }
}
