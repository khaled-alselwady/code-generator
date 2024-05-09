using CommonLibrary;
using System.Text;

namespace GenerateDataAccessLayerLibrary
{
    public static class clsGenerateHelperClasses
    {
        private static StringBuilder _tempText;

        static clsGenerateHelperClasses()
        {
            _tempText = new StringBuilder();
        }

        public static string CreateDataAccessSettingsClass(string databaseName)
        {
            _tempText.Clear();

            _tempText.Append($@"using System.Configuration;

namespace {databaseName}DataAccess
{{
    static class clsDataAccessSettings
    {{
        public static string ConnectionString = ConfigurationManager.ConnectionStrings[""ConnectionString""].ConnectionString;
    }}
}}");

            return _tempText.ToString();
        }

        public static string CreateLogHandlerClass(string databaseName)
        {
            _tempText.Clear();

            _tempText.Append($@"using System;
using System.Configuration;
using System.Diagnostics;

namespace {databaseName}DataAccess
{{
    public class clsLogHandler
    {{
        public static void LogToEventViewer(string errorType, Exception ex)
        {{
            string sourceName = ConfigurationManager.AppSettings[""ProjectName""];

            // Create the event source if it does not exist
            if (!EventLog.SourceExists(sourceName))
            {{
                EventLog.CreateEventSource(sourceName, ""Application"");
            }}

            string errorMessage = $""{{errorType}} in {{ex.Source}}\\n\\nException Message: {{ex.Message}}\\n\\nException Type: {{ex.GetType().Name}}\\n\\nStack Trace: {{ex.StackTrace}}\\n\\nException Location: {{ex.TargetSite}}"";

            // Log an error event
            EventLog.WriteEntry(sourceName, errorMessage, EventLogEntryType.Error);
        }}
    }}
}}");

            return _tempText.ToString();
        }

        public static string CreateErrorLoggerClass(string databaseName)
        {
            _tempText.Clear();

            _tempText.Append($@"using System;

namespace {databaseName}DataAccess
{{
    public class clsErrorLogger
    {{
        private Action<string, Exception> _logAction;

        public clsErrorLogger(Action<string, Exception> logAction)
        {{
            _logAction = logAction;
        }}

        public void LogError(string errorType, Exception ex)
        {{
            _logAction?.Invoke(errorType, ex);
        }}
    }}
}}");

            return _tempText.ToString();
        }

        private static void _CreateCountMethodHelper()
        {
            _tempText.AppendLine();
            _tempText.AppendLine("public static int Count(string storedProcedureName)");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    int Count = 0;");
            _tempText.AppendLine();
            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({clsGenerateDataAccessLayer.GetConnectionString()})");
            _tempText.AppendLine("        {");
            _tempText.AppendLine("            connection.Open();");
            _tempText.AppendLine();
            _tempText.AppendLine("            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))");
            _tempText.AppendLine("            {");
            _tempText.AppendLine("                command.CommandType = CommandType.StoredProcedure;");
            _tempText.AppendLine();
            _tempText.AppendLine("                object result = command.ExecuteScalar();");
            _tempText.AppendLine();
            _tempText.AppendLine("                if (result != null && int.TryParse(result.ToString(), out int Value))");
            _tempText.AppendLine("                {");
            _tempText.AppendLine("                    Count = Value;");
            _tempText.AppendLine("                }");
            _tempText.AppendLine("            }");
            _tempText.AppendLine("        }");
            _tempText.AppendLine("    }");
            _tempText.AppendLine(clsGenerateDataAccessLayer.CreateCatchBlockWithoutIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return Count;");
            _tempText.AppendLine("}");
        }

        private static void _CreateAllMethodWithNoParameterHelper()
        {
            _tempText.AppendLine();
            _tempText.AppendLine("public static DataTable All(string storedProcedureName)");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    DataTable dt = new DataTable();");
            _tempText.AppendLine();
            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({clsGenerateDataAccessLayer.GetConnectionString()})");
            _tempText.AppendLine("        {");
            _tempText.AppendLine("            connection.Open();");
            _tempText.AppendLine();
            _tempText.AppendLine("            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))");
            _tempText.AppendLine("            {");
            _tempText.AppendLine("                command.CommandType = CommandType.StoredProcedure;");
            _tempText.AppendLine();
            _tempText.AppendLine("                using (SqlDataReader reader = command.ExecuteReader())");
            _tempText.AppendLine("                {");
            _tempText.AppendLine("                    if (reader.HasRows)");
            _tempText.AppendLine("                    {");
            _tempText.AppendLine("                        dt.Load(reader);");
            _tempText.AppendLine("                    }");
            _tempText.AppendLine("                }");
            _tempText.AppendLine("            }");
            _tempText.AppendLine("        }");
            _tempText.AppendLine("    }");
            _tempText.AppendLine(clsGenerateDataAccessLayer.CreateCatchBlockWithoutIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return dt;");
            _tempText.AppendLine("}");
        }

        private static void _CreateAllMethodWithOneParameterHelper()
        {
            _tempText.Append($@"public static DataTable All<T>(string storedProcedureName, string parameterName, T value)
{{
    DataTable dt = new DataTable();

    try
    {{
        using ({clsGenerateDataAccessLayer.GetConnectionString()})
        {{
            connection.Open();

            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {{
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue($""@{{parameterName}}"", (object)value ?? DBNull.Value);

                using (SqlDataReader reader = command.ExecuteReader())
                {{
                    if (reader.HasRows)
                    {{
                        dt.Load(reader);
                    }}
                }}
            }}
        }}
    }}
    {clsGenerateDataAccessLayer.CreateCatchBlockWithoutIsFound()}

    return dt;
}}");
        }

        private static void _CreateAllMethodWithTwoParametersHelper()
        {
            _tempText.Append($@"public static DataTable All<T1, T2>(string storedProcedureName, string parameterName1, T1 value1, string parameterName2, T2 value2)
{{
    DataTable dt = new DataTable();

    try
    {{
        using ({clsGenerateDataAccessLayer.GetConnectionString()})
        {{
            connection.Open();

            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {{
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue($""@{{parameterName1}}"", (object)value1 ?? DBNull.Value);
                command.Parameters.AddWithValue($""@{{parameterName2}}"", (object)value2 ?? DBNull.Value);

                using (SqlDataReader reader = command.ExecuteReader())
                {{
                    if (reader.HasRows)
                    {{
                        dt.Load(reader);
                    }}
                }}
            }}
        }}
    }}
    {clsGenerateDataAccessLayer.CreateCatchBlockWithoutIsFound()}

    return dt;
}}");
        }

        private static void _CreateAllMethodWithMoreTwoParametersHelper()
        {
            _tempText.Append($@"public static DataTable All<T>(string storedProcedureName, Dictionary<string, T> parameters)
{{
    DataTable dt = new DataTable();

    try
    {{
        using ({clsGenerateDataAccessLayer.GetConnectionString()})
        {{
            connection.Open();

            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {{
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters from the dictionary
                if (parameters != null)
                {{
                    foreach (var parameter in parameters)
                    {{
                        command.Parameters.AddWithValue($""@{{parameter.Key}}"", (object)parameter.Value ?? DBNull.Value);
                    }}
                }}

                using (SqlDataReader reader = command.ExecuteReader())
                {{
                    if (reader.HasRows)
                    {{
                        dt.Load(reader);
                    }}
                }}
            }}
        }}
    }}
    {clsGenerateDataAccessLayer.CreateCatchBlockWithoutIsFound()}

    return dt;
}}");
        }

        private static void _CreateAllInPagesMethodHelper()
        {
            _tempText.Append($@"public static DataTable AllInPages(short pageNumber, int rowsPerPage, string storedProcedureName)
{{
    DataTable dt = new DataTable();

    try
    {{
        using ({clsGenerateDataAccessLayer.GetConnectionString()})
        {{
            connection.Open();

            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {{
                command.CommandType = CommandType.StoredProcedure;

                // Add paging parameters to the command
                command.Parameters.AddWithValue(""@PageNumber"", pageNumber);
                command.Parameters.AddWithValue(""@RowsPerPage"", rowsPerPage);

                using (SqlDataReader reader = command.ExecuteReader())
                {{
                    if (reader.HasRows)
                    {{
                        dt.Load(reader);
                    }}
                }}
            }}
        }}
    }}
    {clsGenerateDataAccessLayer.CreateCatchBlockWithoutIsFound()}

    return dt;
}}");
        }

        private static void _CreateHandleExceptionMethodHelper()
        {
            _tempText.Append($@"public static void HandleException(Exception ex)
{{
    if (ex is SqlException sqlEx)
    {{
        var loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
        loggerToEventViewer.LogError(""Database Exception"", sqlEx);
    }}
    else
    {{
        var loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);
        loggerToEventViewer.LogError(""General Exception"", ex);
    }}
}}");
        }

        private static void _CreateDeleteMethodHelper()
        {
            _tempText.Append($@"public static bool Delete<T>(string storedProcedureName, string parameterName, T value)
{{
    int rowAffected = 0;

    try
    {{
        using ({clsGenerateDataAccessLayer.GetConnectionString()})
        {{
            connection.Open();

            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {{
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue($""@{{parameterName}}"", (object)value ?? DBNull.Value);

                rowAffected = command.ExecuteNonQuery();
            }}
        }}
    }}
    {clsGenerateDataAccessLayer.CreateCatchBlockWithoutIsFound()}
    
    return rowAffected > 0;
}}");
        }

        private static void _CreateExistMethodWithOneParameterHelper()
        {
            _tempText.Append($@"public static bool Exists<T>(string storedProcedureName, string parameterName, T value)
{{
    bool isFound = false;

    try
    {{
        using ({clsGenerateDataAccessLayer.GetConnectionString()})
        {{
            connection.Open();

            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {{
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue($""@{{parameterName}}"", (object)value ?? DBNull.Value);

                var returnParameter = new SqlParameter(""@ReturnVal"", SqlDbType.Int)
                {{
                    Direction = ParameterDirection.ReturnValue
                }};
                command.Parameters.Add(returnParameter);

                command.ExecuteNonQuery();

                isFound = (int)returnParameter.Value == 1;
            }}
        }}
    }}
    {clsGenerateDataAccessLayer.CreateCatchBlockWithIsFound()}

    return isFound;
}}");
        }

        private static void _CreateExistMethodWithTowParametersHelper()
        {
            _tempText.Append($@"public static bool Exists<T1, T2>(string storedProcedureName, string parameterName1, T1 value1, string parameterName2, T2 value2)
{{
    bool isFound = false;

    try
    {{
        using ({clsGenerateDataAccessLayer.GetConnectionString()})
        {{
            connection.Open();

            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {{
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue($""@{{parameterName1}}"", (object)value1 ?? DBNull.Value);
                command.Parameters.AddWithValue($""@{{parameterName2}}"", (object)value2 ?? DBNull.Value);

                var returnParameter = new SqlParameter(""@ReturnVal"", SqlDbType.Int)
                {{
                    Direction = ParameterDirection.ReturnValue
                }};
                command.Parameters.Add(returnParameter);

                command.ExecuteNonQuery();

                isFound = (int)returnParameter.Value == 1;
            }}
        }}
    }}
    {clsGenerateDataAccessLayer.CreateCatchBlockWithIsFound()}

    return isFound;
}}");
        }

        public static string CreateDataAccessHelperClass(string databaseName)
        {
            _tempText.Clear();

            _tempText.AppendLine("using System;");
            _tempText.AppendLine("using System.Data;");
            _tempText.AppendLine("using System.Collections.Generic;");
            _tempText.AppendLine("using System.Data.SqlClient;");
            _tempText.AppendLine();

            _tempText.AppendLine($"namespace {databaseName}DataAccess");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    public static class clsDataAccessHelper");
            _tempText.AppendLine("    {");

            // Generate Handle Exception method
            _CreateHandleExceptionMethodHelper();

            _tempText.AppendLine().AppendLine();

            // Generate Delete method
            _CreateDeleteMethodHelper();

            _tempText.AppendLine().AppendLine();

            // Generate Exist methods
            _CreateExistMethodWithOneParameterHelper();
            _tempText.AppendLine().AppendLine();
            _CreateExistMethodWithTowParametersHelper();

            _tempText.AppendLine();

            // Generate Count method
            _CreateCountMethodHelper();

            // Generate GetAll methods
            _CreateAllMethodWithNoParameterHelper();
            _tempText.AppendLine();

            _CreateAllMethodWithOneParameterHelper();
            _tempText.AppendLine().AppendLine();

            _CreateAllMethodWithTwoParametersHelper();
            _tempText.AppendLine().AppendLine();

            _CreateAllMethodWithMoreTwoParametersHelper();
            _tempText.AppendLine().AppendLine();

            _CreateAllInPagesMethodHelper();
            _tempText.AppendLine().AppendLine();

            _tempText.AppendLine("    }");
            _tempText.AppendLine("}");

            return _tempText.ToString();
        }

        public static void GenerateAllToFile(string path, string databaseName)
        {
            _CreateDataAccessSettingsClassToTheFile(path, databaseName);
            _CreateClassesThatRelatedToLoggingErrorsToFile(path, databaseName);
            _CreateDataAccessHelperClassToTheFile(path, databaseName);
        }

        private static void _CreateDataAccessHelperClassToTheFile(string path, string databaseName)
        {
            CreateDataAccessHelperClass(databaseName);

            StringBuilder fullPath = new StringBuilder();

            fullPath.Append(path + "clsDataAccessHelper.cs");

            clsHelperMethods.WriteToFile(fullPath.ToString(), _tempText.ToString());
        }

        private static void _CreateDataAccessSettingsClassToTheFile(string path, string databaseName)
        {
            CreateDataAccessSettingsClass(databaseName);

            StringBuilder fullPath = new StringBuilder();

            fullPath.Append(path + "clsDataAccessSettings.cs");

            clsHelperMethods.WriteToFile(fullPath.ToString(), _tempText.ToString());
        }

        private static void _CreateErrorLoggerClassToTheFile(string path, string databaseName)
        {
            CreateErrorLoggerClass(databaseName);

            StringBuilder fullPath = new StringBuilder();
            fullPath.Append(path + "clsErrorLogger.cs");

            clsHelperMethods.WriteToFile(fullPath.ToString(), _tempText.ToString());
        }

        private static void _CreateClassesThatRelatedToLoggingErrorsToFile(string path, string databaseName)
        {
            // Main class
            _CreateErrorLoggerClassToTheFile(path, databaseName);

            // sub classes to handle where you want to log the errors
            _CreateLogHandlerClassToTheFile(path, databaseName);
        }

        private static void _CreateLogHandlerClassToTheFile(string path, string databaseName)
        {
            CreateLogHandlerClass(databaseName);

            StringBuilder fullPath = new StringBuilder();
            fullPath.Append(path + "clsLogHandler.cs");

            clsHelperMethods.WriteToFile(fullPath.ToString(), _tempText.ToString());
        }
    }
}
