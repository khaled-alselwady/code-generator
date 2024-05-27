using CodeGeneratorDataAccess;
using System.Data;

namespace CodeGeneratorBusiness
{
    public class clsCodeGenerator
    {
        public static bool DoesTableExist(string tableName, string databaseName)
           => clsCodeGeneratorData.DoesTableExist(tableName, databaseName);

        public static DataTable GetColumnsNameWithInfo(string tableName, string databaseName)
            => clsCodeGeneratorData.GetColumnsNameWithInfo(tableName, databaseName);

        public static bool DoesDataBaseExist(string databaseName)
            => clsCodeGeneratorData.DoesDataBaseExist(databaseName);

        public static DataTable GetAllTablesNameInASpecificDatabase(string databaseName)
            => clsCodeGeneratorData.GetAllTablesNameInASpecificDatabase(databaseName);

        public static DataTable GetAllDatabaseName()
            => clsCodeGeneratorData.GetAllDatabaseName();

        public static bool ExecuteStoredProcedure(string databaseName, string storedProcedures)
            => clsCodeGeneratorData.ExecuteStoredProcedure(databaseName, storedProcedures);
    }
}
