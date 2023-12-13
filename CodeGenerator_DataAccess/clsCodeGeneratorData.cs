using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator_DataAccess
{
    public class clsCodeGeneratorData
    {
        public static bool DoesTableExist(string TableName, string DatabaseName)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString(DatabaseName)))
                {
                    connection.Open();

                    string query = @"SELECT 1 FROM sys.tables WHERE name = @TableName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TableName", TableName);

                        IsFound = (command.ExecuteScalar() != null);
                    }
                }
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsLogError.LogError("Database Exception", ex);
            }
            catch (Exception ex)
            {
                IsFound = false;
                clsLogError.LogError("General Exception", ex);
            }

            return IsFound;
        }

        public static DataTable GetColumnsNameWithInfo(string TableName, string DatabaseName)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString(DatabaseName)))
                {
                    connection.Open();

                    string query = @"SELECT
                                 COLUMN_NAME AS 'Column Name',
                                 DATA_TYPE AS 'Data Type',
                                 IS_NULLABLE AS 'Is Nullable'
                             FROM
                                 INFORMATION_SCHEMA.COLUMNS
                             WHERE
                                 TABLE_NAME = @TableName
                             ORDER BY
                                 ORDINAL_POSITION;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TableName", TableName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogError.LogError("Database Exception", ex);
            }
            catch (Exception ex)
            {
                clsLogError.LogError("General Exception", ex);
            }

            return dt;
        }

        public static bool DoesDataBaseExist(string DatabaseName)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString(DatabaseName)))
                {
                    connection.Open();

                    string query = @"SELECT Found = 1 FROM sys.databases WHERE name = @DatabaseName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DatabaseName", DatabaseName);

                        IsFound = (command.ExecuteScalar() != null);
                    }
                }
            }
            catch (SqlException ex)
            {
                IsFound = false;
                clsLogError.LogError("Database Exception", ex);
            }
            catch (Exception ex)
            {
                IsFound = false;
                clsLogError.LogError("General Exception", ex);
            }

            return IsFound;
        }

        public static DataTable GetAllTablesNameInASpecificDatabase(string DatabaseName)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString(DatabaseName)))
                {
                    connection.Open();

                    string query = @"SELECT name AS TableName
                                 FROM sys.tables 
                                 WHERE name <> 'sysdiagrams'
                                 Order by name;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogError.LogError("Database Exception", ex);
            }
            catch (Exception ex)
            {
                clsLogError.LogError("General Exception", ex);
            }

            return dt;
        }

        public static DataTable GetAllDatabaseName()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString()))
                {
                    connection.Open();

                    string query = @"SELECT name AS DatabaseName
                             FROM sys.databases
                             WHERE database_id > 4
                             ORDER BY create_date DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                clsLogError.LogError("Database Exception", ex);
            }
            catch (Exception ex)
            {
                clsLogError.LogError("General Exception", ex);
            }

            return dt;
        }
    }
}
