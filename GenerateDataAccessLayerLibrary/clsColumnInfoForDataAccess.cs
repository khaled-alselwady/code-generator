using System.Data;

namespace GenerateDataAccessLayerLibrary
{
    public class clsColumnInfoForDataAccess
    {
        public string ColumnName { get; set; }
        public SqlDbType DataType { get; set; }
        public bool IsNullable { get; set; }
    }
}
