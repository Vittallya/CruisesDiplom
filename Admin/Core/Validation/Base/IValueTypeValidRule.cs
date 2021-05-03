using System;

namespace Admin.Core.Validation
{
    internal interface IValueTypeValidRule<TCl, T>
        where TCl : class
        where T : struct, IComparable
    {
    }
}