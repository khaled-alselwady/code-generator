using System.Data;

namespace GenerateStoredProcedureLibrary
{
    public class clsColumnInfoForStoredProcedure
    {
        public string ColumnName { get; set; }
        public SqlDbType DataType { get; set; }
        public bool IsNullable { get; set; }
        public int? MaxLength { get; set; }
    }
}
