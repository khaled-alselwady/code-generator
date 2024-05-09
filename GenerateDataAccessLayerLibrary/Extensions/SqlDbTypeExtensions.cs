using System;
using System.Data;

namespace GenerateDataAccessLayerLibrary.Extensions
{
    public static class SqlDbTypeExtensions
    {
        public static SqlDbType ToSqlDbType(this string text, SqlDbType defaultType = SqlDbType.VarChar)
        {
            try
            {
                return (SqlDbType)Enum.Parse(typeof(SqlDbType), text, true);
            }
            catch (ArgumentException)
            {
                // Handle cases where parsing fails by returning a default type
                return defaultType;
            }
        }
    }
}
