using System.Data;

namespace CommonLibrary
{
    public class clsColumnInfo
    {
        public string ColumnName { get; set; }
        public SqlDbType DataType { get; set; }
        public bool IsNullable { get; set; }
        public int? MaxLength { get; set; }
    }
}
