using CodeGenerator_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator_Business
{
    public class clsCodeGenerator
    {

        public static bool IsTableExists(string TableName, string DatabaseName)
        {
            return clsCodeGeneratorData.IsTableExists(TableName, DatabaseName);
        }

        public static DataTable GetColumnsNameWithInfo(string TableName, string DatabaseName)
        {
            return clsCodeGeneratorData.GetColumnsNameWithInfo(TableName, DatabaseName);
        }

        public static bool IsDataBaseExists(string DatabaseName)
        {
            return clsCodeGeneratorData.IsDataBaseExists(DatabaseName);
        }

        public static DataTable GetAllTablesNameInASpecificDatabase(string DatabaseName)
        {
            return clsCodeGeneratorData.GetAllTablesNameInASpecificDatabase(DatabaseName);
        }

        public static DataTable GetAllDatabaseName()
        {
            return clsCodeGeneratorData.GetAllDatabaseName();
        }
    }
}
