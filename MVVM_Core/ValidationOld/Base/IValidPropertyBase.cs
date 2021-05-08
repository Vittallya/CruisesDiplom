namespace MVVM_Core.ValidationOld
{
    public interface IValidPropertyBase<out T> : IValidRule where T: IValidRule
    {
        T Or { get; }
    }

}
