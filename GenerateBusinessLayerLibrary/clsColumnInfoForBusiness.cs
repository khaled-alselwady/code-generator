using System.Data;

namespace GenerateBusinessLayerLibrary
{
    public class clsColumnInfoForBusiness
    {
        public string ColumnName { get; set; }
        public SqlDbType DataType { get; set; }
        public bool IsNullable { get; set; }
    }
}
