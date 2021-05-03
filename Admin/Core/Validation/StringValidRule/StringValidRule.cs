using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Admin.Core.Validation
{
    class StringValidRule : LinkedTypeValidRule<IStringValidRule, string>, IStringValidRule
    {
        public StringValidRule(Func<string> value, string modelName) : base(value, modelName)
        {
        }

        public StringValidRule(string modelName) : base(modelName)
        {
        }


        public IStringValidRule LengthMax(int max, string errorMessage = null)
        {
            if (errorMessage == null)
                errorMessage = $"Длина строки свойства '{modelName}' должна быть строго больше значения {max}";

            _predicates.AddPredicate(x => x == null ||
            x.Length < max, errorMessage);

            return this;
        }

        public IStringValidRule LengthMaxEquial(int max, string errorMessage = null)
        {
            if (errorMessage == null)
                errorMessage = $"Длина строки свойства '{modelName}' должна быть больше или равна значению {max}";

            _predicates.AddPredicate(x => x == null || x.Length <= max, errorMessage);
            return this;
        }

        public IStringValidRule LengthMin(int min, string errorMessage = null)
        {
            if (errorMessage == null)
                errorMessage = $"Длина строки свойства '{modelName}' должна быть строго меньше значения {min}";

            _predicates.AddPredicate(x => x != null && x.Length > min, errorMessage);
            return this;
        }

        public IStringValidRule LengthMinEquial(int min, string errorMessage = null)
        {
            if (errorMessage == null)
                errorMessage = $"Длина строки свойства '{modelName}' должна быть меньше или равна значению {min}";

            _predicates.AddPredicate(x => x != null && x.Length >= min, errorMessage);
            return this;
        }

        public IStringValidRule MustBeEmpty(string errorMessage = null)
        {
            if (errorMessage == null)
                errorMessage = $"Значение свойства '{modelName}' должно быть пустой строкой, но не равно значению null";


            _predicates.AddPredicate(x => x != null && x.Length == 0, errorMessage);
            return this;
        }

        public IStringValidRule NotEmpty(string errorMessage = null)
        {
            if (errorMessage == null)
                errorMessage = $"Значение свойства '{modelName}' не должно быть пустой строкой";


            _predicates.AddPredicate(x => x != null && x.Length > 0, errorMessage);
            return this;
        }

        public IStringValidRule Regex(string pattern, string errorMessage = null)
        {
            if (errorMessage == null)
                errorMessage = $"Значение свойства '{modelName}' не подпадает под указанный шаблон: {pattern}";

            var reg = new Regex(pattern);
            _predicates.AddPredicate(x => x != null && reg.IsMatch(x), errorMessage);
            return this;
        }

        public IStringValidRule Regex(Regex reg, string errorMessage = null)
        {
            _predicates.AddPredicate(x => reg.IsMatch(x), errorMessage);
            return this;
        }

    }
}
