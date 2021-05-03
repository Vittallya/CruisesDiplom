namespace Admin.Core.Validation
{
    public interface IValidPropertyBase<out T> : IValidRule where T: IValidRule
    {
        T Or { get; }
    }

}
