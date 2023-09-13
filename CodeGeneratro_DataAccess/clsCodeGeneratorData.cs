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
        public static bool IsTableExists(string TableName, string DatabaseName)
        {
            bool IsFound = false;

            //clsDataAccessSettings.DatabaseName = DatabaseName;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString(DatabaseName));

            string query = @"SELECT 1 FROM sys.tables WHERE name = @TableName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TableName", TableName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                IsFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static DataTable GetColumnsNameWithInfo(string TableName, string DatabaseName)
        {
            DataTable dt = new DataTable();

            //clsDataAccessSettings.DatabaseName = DatabaseName;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString(DatabaseName));

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

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TableName", TableName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool IsDataBaseExists(string DatabaseName)
        {
            bool IsFound = false;

            //clsDataAccessSettings.DatabaseName = DatabaseName;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString(DatabaseName));

            string query = @"SELECT Found = 1 FROM sys.databases WHERE name = @DatabaseName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DatabaseName", DatabaseName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                IsFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static DataTable GetAllTablesNameInASpecificDatabase(string DatabaseName)
        {
            DataTable dt = new DataTable();

            //clsDataAccessSettings.DatabaseName = DatabaseName;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString(DatabaseName));

            string query = @"SELECT name AS TableName
                                 FROM sys.tables 
                                 WHERE name <> 'sysdiagrams'
                                 Order by name;";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable GetAllDatabaseName()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString());

            string query = @"SELECT name AS DatabaseName
                             FROM sys.databases
                             WHERE database_id > 4
                             ORDER BY create_date DESC";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

    }
}
