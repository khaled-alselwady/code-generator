using System.Data;

namespace CommonLibrary
{
    public static class SqlDbTypeToCSharpTypeMapper
    {
        private static readonly Dictionary<SqlDbType, string> _TypeMapping = new Dictionary<SqlDbType, string>
    {
        { SqlDbType.BigInt, "long" },
        { SqlDbType.Binary, "byte[]" },
        { SqlDbType.Bit, "bool" },
        { SqlDbType.Char, "string" },
        { SqlDbType.DateTime, "DateTime" },
        { SqlDbType.Decimal, "decimal" },
        { SqlDbType.Float, "double" },
        { SqlDbType.Image, "byte[]" },
        { SqlDbType.Int, "int" },
        { SqlDbType.Money, "decimal" },
        { SqlDbType.NChar, "string" },
        { SqlDbType.NText, "string" },
        { SqlDbType.NVarChar, "string" },
        { SqlDbType.Real, "float" },
        { SqlDbType.UniqueIdentifier, "Guid" },
        { SqlDbType.SmallDateTime, "DateTime" },
        { SqlDbType.SmallInt, "short" },
        { SqlDbType.SmallMoney, "decimal" },
        { SqlDbType.Text, "string" },
        { SqlDbType.Timestamp, "byte[]" },
        { SqlDbType.TinyInt, "byte" },
        { SqlDbType.VarBinary, "byte[]" },
        { SqlDbType.VarChar, "string" },
        { SqlDbType.Variant, "object" },
        { SqlDbType.Xml, "string" },
        { SqlDbType.Udt, "object" },
        { SqlDbType.Structured, "object" },
        { SqlDbType.Date, "DateTime" },
        { SqlDbType.Time, "TimeSpan" },
        { SqlDbType.DateTime2, "DateTime" },
        { SqlDbType.DateTimeOffset, "DateTimeOffset" }
    };

        public static string GetCSharpType(SqlDbType sqlDbType)
        {
            if (_TypeMapping.TryGetValue(sqlDbType, out string csharpType))
            {
                return csharpType;
            }

            // Return 'object' for unmapped types or unexpected values
            return "object";
        }
    }
}
