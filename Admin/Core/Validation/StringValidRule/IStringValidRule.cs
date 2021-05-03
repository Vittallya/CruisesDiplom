using System.Text.RegularExpressions;

namespace Admin.Core.Validation
{


    public interface IStringValidRule : ILinkedTypeValidRule<IStringValidRule, string>
    {
        /// <summary>
        /// Либо значение null, либо длина строки больше 0
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        IStringValidRule NotEmpty(string errorMessage = null);

        /// <summary>
        /// Значение не null и длина строки равна 0
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        IStringValidRule MustBeEmpty(string errorMessage = null);
        IStringValidRule LengthMin(int min, string errorMessage = null);
        IStringValidRule LengthMinEquial(int min, string errorMessage = null);
        IStringValidRule LengthMax(int max, string errorMessage = null);
        IStringValidRule LengthMaxEquial(int max, string errorMessage = null);
        IStringValidRule Regex(string match, string errorMessage = null);
        IStringValidRule Regex(Regex match, string errorMessage = null);
    }
}
