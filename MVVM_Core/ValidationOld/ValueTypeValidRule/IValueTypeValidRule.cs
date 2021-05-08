using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core.ValidationOld
{
    public interface IValueTypeValidRule<TValue>: IValidPropertyBase<IValueTypeValidRule<TValue>>
        where TValue: IComparable, IEquatable<TValue>
    {
        IValueTypeValidRule<TValue> LessEquialThan(TValue value, string msg);
        IValueTypeValidRule<TValue> LessThan(TValue value, string msg);
        IValueTypeValidRule<TValue> MoreThan(TValue value, string msg);
        IValueTypeValidRule<TValue> MoreEquialThan(TValue value, string msg = null);
        IValueTypeValidRule<TValue> Equial(TValue value, string msg);
        IValueTypeValidRule<TValue> NotEquial(TValue value, string msg);
    }
}
