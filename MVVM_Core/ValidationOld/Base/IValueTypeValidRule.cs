using System;

namespace MVVM_Core.ValidationOld
{
    internal interface IValueTypeValidRule<TCl, T>
        where TCl : class
        where T : struct, IComparable
    {
    }
}