namespace UxCarrier.Models
{
    public static class CommonValidators
    {
        private static char groupDelimiter { get; } = ',';
        private static char qDelimiter { get; } = ':';

        public static bool IsGuid(string bar)
        {
            return Guid.TryParse(bar, out var result);
        }

        public static bool WebApiEnumCheck<T>(string enumName)
        {
            if (string.IsNullOrWhiteSpace(enumName))
                return true;

            var upperFirstCharEnumName = enumName.First().ToString().ToUpper() + enumName.Substring(1);
            return Enum.IsDefined(typeof(T), upperFirstCharEnumName);
        }

        public static bool Checkq(string qs)
        {
            if (string.IsNullOrWhiteSpace(qs))
            {
                return true;
            }

            var q = qs.Split(groupDelimiter);
            if (qs.Length == 0)
                return false;

            foreach (var fieldmap in q)
            {
                var field = fieldmap.Split(qDelimiter);
                if (field.Length != 2)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckSort(PageRequest filter, string sort)
        {
            if (string.IsNullOrWhiteSpace(sort))
            {
                return true;
            }

            var sorts = sort.Split(groupDelimiter);
            if (sorts.Length == 0)
                return false;

            var isValid = true;
            foreach (var s in sorts)
            {
                var validator = new sortOperatorValidator();
                isValid = validator.Validate(s).IsValid;
                if (!isValid)
                {
                    break;
                }
            }

            return isValid;
        }
    }
}
