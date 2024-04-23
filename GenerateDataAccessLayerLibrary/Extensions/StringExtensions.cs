namespace GenerateDataAccessLayerLibrary.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string pascalCase)
        {
            if (string.IsNullOrWhiteSpace(pascalCase))
                return pascalCase;

            pascalCase = pascalCase.TrimStart();

            // Convert the first character to lowercase and append the rest of the string
            return char.ToLower(pascalCase[0]) + pascalCase.Substring(1);
        }
    }
}
