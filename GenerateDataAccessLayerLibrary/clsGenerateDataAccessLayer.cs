using CodeGeneratorBusiness;
using GenerateDataAccessLayerLibrary.Extensions;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace GenerateDataAccessLayerLibrary
{
    public static class clsGenerateDataAccessLayer
    {
        private static string _tableName;
        private static string _databaseName;
        private static string _tableSingleName;
        private static bool _isLogin;
        private static bool _isGenerateAllMode;
        private static StringBuilder _tempText;
        private static List<List<clsColumnInfoForDataAccess>> _columnsInfo;

        static clsGenerateDataAccessLayer()
        {
            _tableName = string.Empty;
            _databaseName = string.Empty;
            _tableSingleName = string.Empty;
            _isLogin = false;
            _isGenerateAllMode = false;
            _tempText = new StringBuilder();
            _columnsInfo = new List<List<clsColumnInfoForDataAccess>>();
        }

        private static bool _DoesTableHaveColumn(string columnName)
        {
            for (int i = 0; i < _columnsInfo.Count; i++)
            {

                var firstItem = _columnsInfo[i]; // Access the first row (index 0)

                if (firstItem.Count > 0)
                {
                    if (firstItem[0].ColumnName.ToLower() == columnName.ToLower())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool _DoesTableHaveUsernameAndPassword()
        {
            return (_DoesTableHaveColumn("username") && _DoesTableHaveColumn("password"));
        }

        private static string _GetDataTypeCSharp(string DataType)
        {
            switch (DataType.ToLower())
            {
                case "int":
                    return "int";

                case "bigint":
                    return "long";

                case "float":
                    return "float";

                case "decimal":
                case "money":
                case "smallmoney":
                    return "decimal";

                case "smallint":
                    return "short";

                case "tinyint":
                    return "byte";

                case "nvarchar":
                case "varchar":
                case "char":
                    return "string";

                case "datetime":
                case "date":
                case "smalldatetime":
                case "datetime2":
                    return "DateTime";

                case "time":
                    return "TimeSpan";

                case "bit":
                    return "bool";

                default:
                    return "string";
            }
        }

        private static bool _IsDataTypeString(string DateType)
        {
            string Result = _GetDataTypeCSharp(DateType);

            return (Result.ToLower() == "string");
        }

        internal static string GetConnectionString()
        {
            return "SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString)";
        }

        internal static string CreateCatchBlockWithIsFound()
        {
            return "catch (Exception ex)\r\n            {\r\n                isFound = false;\r\n                clsDataAccessHelper.HandleException(ex);\r\n            }";
        }

        internal static string CreateCatchBlockWithoutIsFound()
        {
            return "catch (Exception ex)\r\n            {\r\n                clsDataAccessHelper.HandleException(ex);\r\n            }";
        }

        private static string _MakeParametersForFindMethod()
        {
            StringBuilder parameters = new StringBuilder();

            parameters.Append("(");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForDataAccess> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();
                    string dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    if (i == 0)
                    {
                        parameters.Append(_GetDataTypeCSharp(dataType) + "? " + columnName + ", ");
                    }
                    else
                    {
                        if (isNullable && !_IsDataTypeString(dataType))
                        {
                            parameters.Append("ref " + _GetDataTypeCSharp(dataType) + "? " + columnName + ", ");
                        }
                        else
                        {
                            parameters.Append("ref " + _GetDataTypeCSharp(dataType) + " " + columnName + ", ");
                        }
                    }
                }
            }

            // To remove the ", " from the end of the text
            parameters.Length -= 2;

            parameters.Append(")");

            return parameters.ToString().Trim();
        }

        private static string _FillTheVariableWithDataThatComingFromDatabase()
        {
            StringBuilder text = new StringBuilder();

            for (int i = 1; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForDataAccess> secondItem = _columnsInfo[i];

                if (secondItem.Count > 0)
                {
                    string columnName = secondItem[0].ColumnName;
                    string dataType = secondItem[0].DataType;
                    bool isNullable = secondItem[0].IsNullable;

                    if (isNullable)
                    {
                        if (!_IsDataTypeString(dataType))
                        {
                            text.Append($"{columnName.ToCamelCase()} = (reader[\"{columnName}\"] != DBNull.Value) ? ({_GetDataTypeCSharp(dataType)}?)reader[\"{columnName}\"] : null;");
                        }
                        else
                        {
                            text.Append($"{columnName.ToCamelCase()} = (reader[\"{columnName}\"] != DBNull.Value) ? ({_GetDataTypeCSharp(dataType)})reader[\"{columnName}\"] : null;");
                        }

                        text.AppendLine();
                    }
                    else
                    {
                        text.AppendLine($"{columnName.ToCamelCase()} = ({_GetDataTypeCSharp(dataType)})reader[\"{columnName}\"];");
                    }
                }
            }

            return text.ToString().Trim();
        }

        private static void _CreateGetInfoMethod()
        {
            _tempText.Append($"public static bool GetInfoByID{_MakeParametersForFindMethod()}");
            _tempText.AppendLine();
            _tempText.AppendLine("{");

            _tempText.AppendLine("    bool isFound = false;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({GetConnectionString()})");
            _tempText.AppendLine("        {");
            _tempText.AppendLine("            connection.Open();");
            _tempText.AppendLine();

            _tempText.AppendLine($"            using (SqlCommand command = new SqlCommand(\"SP_Get{_tableSingleName}InfoByID\", connection))");
            _tempText.AppendLine("            {");
            _tempText.AppendLine("command.CommandType = CommandType.StoredProcedure;").AppendLine();
            _tempText.AppendLine($"                command.Parameters.AddWithValue(\"@{_tableSingleName}ID\", (object){_tableSingleName.ToCamelCase()}ID ?? DBNull.Value);");
            _tempText.AppendLine();

            _tempText.AppendLine("                using (SqlDataReader reader = command.ExecuteReader())");
            _tempText.AppendLine("                {");
            _tempText.AppendLine("                    if (reader.Read())");
            _tempText.AppendLine("                    {");
            _tempText.AppendLine("                        // The record was found");
            _tempText.AppendLine("                        isFound = true;");
            _tempText.AppendLine();

            _tempText.AppendLine(_FillTheVariableWithDataThatComingFromDatabase());
            _tempText.AppendLine("                    }");
            _tempText.AppendLine("                    else");
            _tempText.AppendLine("                    {");
            _tempText.AppendLine("                        // The record was not found");
            _tempText.AppendLine("                        isFound = false;");
            _tempText.AppendLine("                    }");
            _tempText.AppendLine("                }");
            _tempText.AppendLine("            }");
            _tempText.AppendLine("        }");
            _tempText.AppendLine("    }");
            _tempText.AppendLine(CreateCatchBlockWithIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return isFound;");
            _tempText.AppendLine("}");
        }

        private static string _MakeParametersForFindMethodForUsername()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForDataAccess> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();
                    string dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    if (i == 0)
                    {
                        parameters.Append("ref ").Append(_GetDataTypeCSharp(dataType)).Append("? ").Append(columnName).Append(", ");
                    }
                    else
                    {
                        if (columnName.ToLower() == "username")
                        {
                            parameters.Append(_GetDataTypeCSharp(dataType)).Append(" ").Append(columnName).Append(", ");
                        }
                        else
                        {
                            if (isNullable && !_IsDataTypeString(dataType))
                            {
                                parameters.Append("ref ").Append(_GetDataTypeCSharp(dataType)).Append("? ").Append(columnName).Append(", ");
                            }
                            else
                            {
                                parameters.Append("ref ").Append(_GetDataTypeCSharp(dataType)).Append(" ").Append(columnName).Append(", ");
                            }
                        }
                    }
                }
            }

            // To remove the ", " from the end of the text
            parameters.Length -= 2;

            parameters.Append(")");

            return parameters.ToString().Trim();
        }

        private static string _FillTheVariableWithDataThatComingFromDatabaseForUsername()
        {
            StringBuilder Text = new StringBuilder();

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForDataAccess> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName;
                    string dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    if (columnName.ToLower() != "username")
                    {
                        if (i == 0)
                        {
                            Text.Append(columnName.ToCamelCase())
                                .Append(" = ")
                                .Append("(reader[\"")
                                .Append(columnName)
                                .Append("\"] != DBNull.Value) ? (")
                                .Append(_GetDataTypeCSharp(dataType))
                                .Append("?)reader[\"")
                                .Append(columnName)
                                .Append("\"] : null;")
                                .AppendLine();
                            continue;
                        }

                        if (isNullable)
                        {
                            if (!_IsDataTypeString(dataType))
                            {
                                Text.Append(columnName.ToCamelCase())
                                    .Append(" = ")
                                    .Append("(reader[\"")
                                    .Append(columnName)
                                    .Append("\"] != DBNull.Value) ? (")
                                    .Append(_GetDataTypeCSharp(dataType))
                                    .Append("?)reader[\"")
                                    .Append(columnName)
                                    .Append("\"] : null;")
                                    .AppendLine();
                            }
                            else
                            {
                                Text.Append(columnName.ToCamelCase())
                                    .Append(" = ")
                                    .Append("(reader[\"")
                                    .Append(columnName)
                                    .Append("\"] != DBNull.Value) ? (")
                                    .Append(_GetDataTypeCSharp(dataType))
                                    .Append(")reader[\"")
                                    .Append(columnName)
                                    .Append("\"] : null;")
                                    .AppendLine();
                            }
                        }
                        else
                        {
                            Text.Append(columnName.ToCamelCase())
                                .Append(" = ")
                                .Append("(")
                                .Append(_GetDataTypeCSharp(dataType))
                                .Append(")reader[\"")
                                .Append(columnName)
                                .Append("\"];")
                                .AppendLine();
                        }
                    }
                }
            }

            return Text.ToString().Trim();
        }

        private static void _CreateGetInfoMethodForUsername()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool GetInfoByUsername{_MakeParametersForFindMethodForUsername()}");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    bool isFound = false;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({GetConnectionString()})");
            _tempText.AppendLine("        {");
            _tempText.AppendLine("            connection.Open();");
            _tempText.AppendLine();

            _tempText.AppendLine($"            using (SqlCommand command = new SqlCommand(\"SP_Get{_tableSingleName}InfoByUsername\", connection))");
            _tempText.AppendLine("            {");
            _tempText.AppendLine("command.CommandType = CommandType.StoredProcedure;").AppendLine();
            _tempText.AppendLine($"                command.Parameters.AddWithValue(\"@Username\", username);");
            _tempText.AppendLine();

            _tempText.AppendLine("                using (SqlDataReader reader = command.ExecuteReader())");
            _tempText.AppendLine("                {");
            _tempText.AppendLine("                    if (reader.Read())");
            _tempText.AppendLine("                    {");
            _tempText.AppendLine("                        // The record was found");
            _tempText.AppendLine("                        isFound = true;");
            _tempText.AppendLine(_FillTheVariableWithDataThatComingFromDatabaseForUsername());
            _tempText.AppendLine("                    }");
            _tempText.AppendLine("                    else");
            _tempText.AppendLine("                    {");
            _tempText.AppendLine("                        // The record was not found");
            _tempText.AppendLine("                        isFound = false;");
            _tempText.AppendLine("                    }");
            _tempText.AppendLine("                }");
            _tempText.AppendLine("            }");
            _tempText.AppendLine("        }");
            _tempText.AppendLine("    }");
            _tempText.AppendLine(CreateCatchBlockWithIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return isFound;");
            _tempText.AppendLine("}");
        }

        private static string _MakeParametersForFindMethodForUsernameAndPassword()
        {
            StringBuilder parameters = new StringBuilder();

            parameters.Append("(");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForDataAccess> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();
                    string dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    if (i == 0)
                    {
                        parameters.Append("ref ")
                            .Append(_GetDataTypeCSharp(dataType))
                            .Append("? ")
                            .Append(columnName)
                            .Append(", ");
                        continue;
                    }

                    if (columnName.ToLower() == "username" || columnName.ToLower() == "password")
                    {
                        parameters.Append(_GetDataTypeCSharp(dataType))
                            .Append(" ")
                            .Append(columnName)
                            .Append(", ");
                    }
                    else
                    {
                        if (isNullable && !_IsDataTypeString(dataType))
                        {
                            parameters.Append("ref ")
                                .Append(_GetDataTypeCSharp(dataType))
                                .Append("? ")
                                .Append(columnName)
                                .Append(", ");
                        }
                        else
                        {
                            parameters.Append("ref ")
                                .Append(_GetDataTypeCSharp(dataType))
                                .Append(" ")
                                .Append(columnName)
                                .Append(", ");
                        }
                    }
                }
            }

            // To remove the ", " from the end of the text
            parameters.Length -= 2;

            parameters.Append(")");

            return parameters.ToString().Trim();
        }

        private static string _FillTheVariableWithDataThatComingFromDatabaseForUsernameAndPassword()
        {
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForDataAccess> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName;
                    string dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    if (columnName.ToLower() != "username" && columnName.ToLower() != "password")
                    {
                        if (i == 0)
                        {
                            text.Append(columnName.ToCamelCase())
                                .Append(" = (reader[\"")
                                .Append(columnName)
                                .Append("\"] != DBNull.Value) ? (")
                                .Append(_GetDataTypeCSharp(dataType))
                                .Append("?)reader[\"")
                                .Append(columnName)
                                .Append("\"] : null;")
                                .AppendLine();

                            continue;
                        }

                        if (isNullable)
                        {
                            if (!_IsDataTypeString(dataType))
                            {
                                text.Append(columnName.ToCamelCase())
                                    .Append(" = (reader[\"")
                                    .Append(columnName)
                                    .Append("\"] != DBNull.Value) ? (")
                                    .Append(_GetDataTypeCSharp(dataType))
                                    .Append("?)reader[\"")
                                    .Append(columnName)
                                    .Append("\"] : null;")
                                    .AppendLine();
                            }
                            else
                            {
                                text.Append(columnName.ToCamelCase())
                                    .Append(" = (reader[\"")
                                    .Append(columnName)
                                    .Append("\"] != DBNull.Value) ? (")
                                    .Append(_GetDataTypeCSharp(dataType))
                                    .Append(")reader[\"")
                                    .Append(columnName)
                                    .Append("\"] : null;")
                                    .AppendLine();
                            }
                        }
                        else
                        {
                            text.Append(columnName.ToCamelCase())
                                .Append(" = (")
                                .Append(_GetDataTypeCSharp(dataType))
                                .Append(")reader[\"")
                                .Append(columnName)
                                .Append("\"];")
                                .AppendLine();
                        }
                    }
                }
            }

            return text.ToString().Trim();
        }

        private static void _CreateGetInfoMethodForUsernameAndPassword()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool GetInfoByUsernameAndPassword{_MakeParametersForFindMethodForUsernameAndPassword()}");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    bool isFound = false;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({GetConnectionString()})");
            _tempText.AppendLine("        {");
            _tempText.AppendLine("            connection.Open();");
            _tempText.AppendLine();

            _tempText.AppendLine($"            using (SqlCommand command = new SqlCommand(\"SP_Get{_tableSingleName}InfoByUsernameAndPassword\", connection))");
            _tempText.AppendLine("            {");
            _tempText.AppendLine("command.CommandType = CommandType.StoredProcedure;").AppendLine();
            _tempText.AppendLine($"                command.Parameters.AddWithValue(\"@Username\", username);");
            _tempText.AppendLine($"                command.Parameters.AddWithValue(\"@Password\", password);");
            _tempText.AppendLine();

            _tempText.AppendLine("                using (SqlDataReader reader = command.ExecuteReader())");
            _tempText.AppendLine("                {");
            _tempText.AppendLine("                    if (reader.Read())");
            _tempText.AppendLine("                    {");
            _tempText.AppendLine("                        // The record was found");
            _tempText.AppendLine("                        isFound = true;");
            _tempText.AppendLine(_FillTheVariableWithDataThatComingFromDatabaseForUsernameAndPassword());
            _tempText.AppendLine("                    }");
            _tempText.AppendLine("                    else");
            _tempText.AppendLine("                    {");
            _tempText.AppendLine("                        // The record was not found");
            _tempText.AppendLine("                        isFound = false;");
            _tempText.AppendLine("                    }");
            _tempText.AppendLine("                }");
            _tempText.AppendLine("            }");
            _tempText.AppendLine("        }");
            _tempText.AppendLine("    }");
            _tempText.AppendLine(CreateCatchBlockWithIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return isFound;");
            _tempText.AppendLine("}");
        }

        private static string _MakeParametersForAddNewMethod()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 1; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForDataAccess> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();
                    string dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    if (isNullable && !_IsDataTypeString(dataType))
                    {
                        parameters.Append(_GetDataTypeCSharp(dataType)).Append("? ").Append(columnName).Append(", ");
                    }
                    else
                    {
                        parameters.Append(_GetDataTypeCSharp(dataType)).Append(" ").Append(columnName).Append(", ");
                    }
                }
            }

            // To remove the ", " from the end of the text
            if (parameters.Length >= 2)
            {
                parameters.Length -= 2;
            }

            parameters.Append(")");

            return parameters.ToString().Trim();
        }

        private static string _FillParametersInTheCommand()
        {
            StringBuilder text = new StringBuilder();

            for (int i = 1; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForDataAccess> secondItem = _columnsInfo[i];

                if (secondItem.Count > 0)
                {
                    string columnName = secondItem[0].ColumnName;
                    bool isNullable = secondItem[0].IsNullable;

                    text.Append($"command.Parameters.AddWithValue(\"@{columnName}\", ");

                    if (isNullable)
                    {
                        text.Append($"(object){columnName?.ToCamelCase()} ?? DBNull.Value");
                    }
                    else
                    {
                        text.Append(columnName?.ToCamelCase());
                    }

                    text.AppendLine(");");
                }
            }

            return text.ToString().Trim();
        }

        private static void _CreateAddMethod()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static int? Add{_MakeParametersForAddNewMethod()}");
            _tempText.AppendLine("{");
            _tempText.AppendLine("// This function will return the new person id if succeeded and null if not");
            _tempText.AppendLine($"    int? {_tableSingleName.ToCamelCase()}ID = null;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({GetConnectionString()})");
            _tempText.AppendLine("        {");
            _tempText.AppendLine("            connection.Open();");
            _tempText.AppendLine();

            _tempText.AppendLine($"            using (SqlCommand command = new SqlCommand(\"SP_AddNew{_tableSingleName}\", connection))");
            _tempText.AppendLine("            {");
            _tempText.AppendLine("command.CommandType = CommandType.StoredProcedure;").AppendLine();
            _tempText.AppendLine(_FillParametersInTheCommand());
            _tempText.AppendLine();
            _tempText.AppendLine($"SqlParameter outputIdParam = new SqlParameter(\"@New{_tableSingleName}ID\", SqlDbType.Int)");
            _tempText.AppendLine("{").AppendLine("Direction = ParameterDirection.Output").AppendLine("};");
            _tempText.AppendLine("command.Parameters.Add(outputIdParam);");
            _tempText.AppendLine();

            _tempText.AppendLine("command.ExecuteNonQuery();");
            _tempText.AppendLine();

            _tempText.AppendLine($"{_tableSingleName.ToCamelCase()}ID = (int?)outputIdParam.Value;");
            _tempText.AppendLine("            }");
            _tempText.AppendLine("        }");
            _tempText.AppendLine("    }");
            _tempText.AppendLine(CreateCatchBlockWithoutIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine($"    return {_tableSingleName.ToCamelCase()}ID;");
            _tempText.AppendLine("}");
        }

        private static string _MakeParametersForUpdateMethod()
        {
            StringBuilder Parameters = new StringBuilder("(");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForDataAccess> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();
                    string dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    if (i == 0)
                    {
                        Parameters.Append(_GetDataTypeCSharp(dataType) + "? " + columnName + ", ");
                    }
                    else
                    {
                        if (isNullable && !_IsDataTypeString(dataType))
                        {
                            Parameters.Append(_GetDataTypeCSharp(dataType) + "? " + columnName + ", ");
                        }
                        else
                        {
                            Parameters.Append(_GetDataTypeCSharp(dataType) + " " + columnName + ", ");
                        }
                    }
                }
            }

            // To remove the ", " from the end of the text
            Parameters.Length -= 2;

            Parameters.Append(")");

            return Parameters.ToString().Trim();
        }

        private static void _CreateUpdateMethod()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool Update{_MakeParametersForUpdateMethod()}");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    int rowAffected = 0;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({GetConnectionString()})");
            _tempText.AppendLine("        {");
            _tempText.AppendLine("            connection.Open();");
            _tempText.AppendLine();

            _tempText.AppendLine($"            using (SqlCommand command = new SqlCommand(\"SP_Update{_tableSingleName}\", connection))");
            _tempText.AppendLine("            {");
            _tempText.AppendLine("command.CommandType = CommandType.StoredProcedure;").AppendLine();
            _tempText.AppendLine($"                command.Parameters.AddWithValue(\"@{_tableSingleName}ID\", (object){_tableSingleName?.ToCamelCase()}ID ?? DBNull.Value);");
            _tempText.AppendLine(_FillParametersInTheCommand());
            _tempText.AppendLine();

            _tempText.AppendLine("                rowAffected = command.ExecuteNonQuery();");
            _tempText.AppendLine("            }");
            _tempText.AppendLine("        }");
            _tempText.AppendLine("    }");
            _tempText.AppendLine(CreateCatchBlockWithoutIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine($"    return (rowAffected > 0);");
            _tempText.AppendLine("}");
        }

        private static string _MakeParametersForDeleteMethod()
        {
            StringBuilder parameters = new StringBuilder("(");

            List<clsColumnInfoForDataAccess> firstItem = _columnsInfo[0];

            if (firstItem.Count > 0)
            {
                string columnName = firstItem[0].ColumnName.ToCamelCase();
                string dataType = firstItem[0].DataType;

                parameters.Append(_GetDataTypeCSharp(dataType))
                      .Append("? ")
                      .Append(columnName)
                      .Append(")");
            }
            return parameters.ToString().Trim();
        }

        private static void _CreateDeleteMethod()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool Delete{_MakeParametersForDeleteMethod()}");
            _tempText.AppendLine($"=> clsDataAccessHelper.Delete(\"SP_Delete{_tableSingleName}\", \"{_tableSingleName}ID\", {_tableSingleName?.ToCamelCase()}ID);");
        }

        private static void _CreateExistsMethod()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool Exists{_MakeParametersForDeleteMethod()}");
            _tempText.AppendLine($"=> clsDataAccessHelper.Exists(\"SP_Does{_tableSingleName}Exist\", \"{_tableSingleName}ID\", {_tableSingleName?.ToCamelCase()}ID);");
        }

        private static void _CreateExistsMethodForUsername()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool Exists(string username)");
            _tempText.AppendLine($"=> clsDataAccessHelper.Exists(\"SP_Does{_tableSingleName}ExistByUsername\", \"Username\", username);");
        }

        private static void _CreateExistsMethodForUsernameAndPassword()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool Exists(string username, string password)");
            _tempText.AppendLine($"=> clsDataAccessHelper.Exists(\"SP_Does{_tableSingleName}ExistByUsernameAndPassword\", \"Username\", username, \"Password\", password);");
        }

        private static void _CreateAllMethod()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static DataTable All()");
            _tempText.AppendLine($"=> clsDataAccessHelper.All(\"SP_GetAll{_tableName}\");");
        }

        private static string _DataAccessAsLoginInfo()
        {
            _CreateGetInfoMethod();
            _CreateGetInfoMethodForUsername();
            _CreateGetInfoMethodForUsernameAndPassword();
            _CreateAddMethod();
            _CreateUpdateMethod();
            _CreateDeleteMethod();
            _CreateExistsMethod();
            _CreateExistsMethodForUsername();
            _CreateExistsMethodForUsernameAndPassword();
            _CreateAllMethod();

            return _tempText.ToString();
        }

        private static string _DataAccessAsNormal()
        {
            _CreateGetInfoMethod();
            _CreateAddMethod();
            _CreateUpdateMethod();
            _CreateDeleteMethod();
            _CreateExistsMethod();
            _CreateAllMethod();

            return _tempText.ToString();
        }

        internal static void WriteToFile(string path, string value)
        {
            using (StreamWriter writer = new StreamWriter(path.Trim()))
            {
                writer.Write(value);
            }
        }

        private static void _GenerateAllClasses(string path)
        {
            Generate(_columnsInfo, _databaseName);

            WriteToFile(path.Trim(), _tempText.ToString());
        }

        private static void _LoadColumnInfo()
        {
            _columnsInfo.Clear();

            DataTable dt = clsCodeGenerator.GetColumnsNameWithInfo(_tableName, _databaseName);

            foreach (DataRow row in dt.Rows)
            {
                var columnInfo = new List<clsColumnInfoForDataAccess>
                {
                    new clsColumnInfoForDataAccess
                    {
                        ColumnName = row["Column Name"].ToString(),
                        DataType = row["Data Type"].ToString(),
                        IsNullable = row["Is Nullable"].ToString().ToLower() == "yes"
                    }
                };

                _columnsInfo.Add(columnInfo);
            }

            _tableSingleName = _GetSingleColumnName();
        }

        private static string _GetSingleColumnName()
        {
            if (_columnsInfo.Count > 0)
            {
                string firstValue = _columnsInfo[0][0].ColumnName;
                return firstValue.Remove(firstValue.Length - 2);
            }

            return "";
        }

        public static string Generate(List<List<clsColumnInfoForDataAccess>> columnsInfo, string databaseName)
        {
            _tempText.Clear();

            _columnsInfo = columnsInfo;
            _databaseName = databaseName;

            _tableSingleName = _GetSingleColumnName();

            if (!_isGenerateAllMode)
            {
                _isLogin = _DoesTableHaveUsernameAndPassword();
            }

            _tempText.AppendLine($"using System;\r\nusing System.Data;\r\nusing System.Data.SqlClient;\r\n\r\nnamespace {_databaseName}DataAccess\r\n{{");

            _tempText.Append($"public class cls{_tableSingleName}Data");
            _tempText.AppendLine();
            _tempText.AppendLine("{");

            if (_isLogin)
            {
                _DataAccessAsLoginInfo();
            }
            else
            {
                _DataAccessAsNormal();
            }

            _tempText.Append("}");
            _tempText.Append("\n}");

            return _tempText.ToString();
        }

        public static void GenerateAllToFile(List<string> tablesNames, string path, string databaseName)
        {
            _isGenerateAllMode = true;
            _databaseName = databaseName;

            string fullPath = string.Empty;

            for (byte i = 0; i < tablesNames.Count; i++)
            {
                _tableName = tablesNames[i];

                _LoadColumnInfo();

                _isLogin = _DoesTableHaveUsernameAndPassword();

                fullPath = path + $"cls{_tableSingleName}Data.cs";

                _GenerateAllClasses(fullPath);
            }

            // common classes for all tables
            clsGenerateHelperClasses.GenerateAllToFile(path, databaseName);

            _isGenerateAllMode = false;
        }
    }
}
