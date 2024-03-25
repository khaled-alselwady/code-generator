using CodeGenerator_DataAccess;
using System.Data;

namespace CodeGenerator_Business
{
    public class clsCodeGenerator
    {

        public static bool DoesTableExist(string tableName, string databaseName)
        {
            return clsCodeGeneratorData.DoesTableExist(tableName, databaseName);
        }

        public static DataTable GetColumnsNameWithInfo(string tableName, string databaseName)
        {
            return clsCodeGeneratorData.GetColumnsNameWithInfo(tableName, databaseName);
        }

        public static bool DoesDataBaseExist(string databaseName)
        {
            return clsCodeGeneratorData.DoesDataBaseExist(databaseName);
        }

        public static DataTable GetAllTablesNameInASpecificDatabase(string databaseName)
        {
            return clsCodeGeneratorData.GetAllTablesNameInASpecificDatabase(databaseName);
        }

        public static DataTable GetAllDatabaseName()
        {
            return clsCodeGeneratorData.GetAllDatabaseName();
        }

        public static bool ExecuteStoredProcedure(string databaseName, string storedProcedures)
        {
            return clsCodeGeneratorData.ExecuteStoredProcedure(databaseName, storedProcedures);
        }
    }
}
