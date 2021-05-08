namespace MVVM_Core.Validation
{
    public static class ValidRulesExtension
    {
        public static TRule NotNull<TValue, TRule>(this BaseValidRule<TValue, TRule> obj, string msg = null)
            where TValue: class
            where TRule : class, IValidRule
        {
            if (msg == null)
                msg = $"Значение поля '{obj.propName}' не должно иметь значения 'null'";

            return obj.Predicate(x => x != null, msg);
        }

        public static TRule MustBeNull<TValue, TRule>(this BaseValidRule<TValue, TRule> obj, string msg = null)
            where TValue: class
            where TRule : class, IValidRule
        {
            if (msg == null)
                msg = $"Значение поля '{obj.propName}' должно иметь значения 'null'";

            return obj.Predicate(x => x == null, msg);
        }
    }
}
