using CodeGeneratorBusiness;
using GenerateBusinessLayerLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace GenerateBusinessLayerLibrary
{
    public static class clsGenerateBusinessLayer
    {
        private static string _tableName;
        private static string _databaseName;
        private static string _tableSingleName;
        private static bool _isLogin;
        private static bool _isGenerateAllMode;
        private static StringBuilder _tempText;
        private static List<List<clsColumnInfoForBusiness>> _columnsInfo;

        static clsGenerateBusinessLayer()
        {
            _tableName = string.Empty;
            _databaseName = string.Empty;
            _tableSingleName = string.Empty;
            _isLogin = false;
            _isGenerateAllMode = false;
            _tempText = new StringBuilder();
            _columnsInfo = new List<List<clsColumnInfoForBusiness>>();
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

        private static string _GetDefaultInitialization(string identifierName, SqlDbType sqlDbType)
        {
            switch (sqlDbType)
            {
                case SqlDbType.Int:
                case SqlDbType.BigInt:
                case SqlDbType.SmallInt:
                    return $"{identifierName} = -1;"; // Default for integer types

                case SqlDbType.Float:
                    return $"{identifierName} = -1F;"; // Default for float

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return $"{identifierName} = -1M;"; // Default for decimal types

                case SqlDbType.TinyInt:
                    return $"{identifierName} = 0;"; // Default for tinyint (byte)

                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                    return $"{identifierName} = string.Empty;"; // Default for variable-length strings

                case SqlDbType.Char:
                case SqlDbType.NChar:
                    return $"{identifierName} = ' ';"; // Default for fixed-length strings

                case SqlDbType.DateTime:
                case SqlDbType.Date:
                case SqlDbType.SmallDateTime:
                case SqlDbType.DateTime2:
                    return $"{identifierName} = DateTime.Now;"; // Default for DateTime

                case SqlDbType.Time:
                    return $"{identifierName} = TimeSpan.Zero;"; // Default for TimeSpan

                case SqlDbType.Bit:
                    return $"{identifierName} = false;"; // Default for boolean

                case SqlDbType.UniqueIdentifier:
                    return $"{identifierName} = Guid.Empty;"; // Default for GUIDs

                case SqlDbType.Binary:
                case SqlDbType.VarBinary:
                case SqlDbType.Timestamp:
                case SqlDbType.Image:
                    return $"{identifierName} = new byte[0];"; // Default for binary data

                case SqlDbType.Xml:
                    return $"{identifierName} = string.Empty;"; // Default for XML data

                case SqlDbType.Real:
                    return $"{identifierName} = -1.0F;"; // Default for Real (single-precision float)

                case SqlDbType.Variant:
                case SqlDbType.Udt:
                case SqlDbType.Structured:
                    return $"{identifierName} = null;"; // Default for Variant, UDT, Structured

                case SqlDbType.DateTimeOffset:
                    return $"{identifierName} = DateTimeOffset.Now;"; // Default for DateTime with timezone

                default:
                    return $"{identifierName} = null;"; // Default for any other types
            }
        }

        private static string _MakeParametersForBusinessLayer()
        {
            StringBuilder Parameters = new StringBuilder();

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName;
                    SqlDbType dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    string csharpType = SqlDbTypeToCSharpTypeMapper.GetCSharpType(dataType);

                    if (i == 0)
                    {
                        Parameters.AppendLine($"public {csharpType}? {columnName} {{ get; set; }}");
                    }
                    else
                    {
                        if (isNullable && csharpType.ToLower() != "string")
                        {
                            Parameters.AppendLine($"public {csharpType}? {columnName} {{ get; set; }}");
                        }
                        else
                        {
                            Parameters.AppendLine($"public {csharpType} {columnName} {{ get; set; }}");
                        }
                    }
                }
            }

            return Parameters.ToString().Trim();
        }

        private static string _MakeParametersForUpdateMethod()
        {
            StringBuilder Parameters = new StringBuilder("(");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();
                    SqlDbType dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    string csharpType = SqlDbTypeToCSharpTypeMapper.GetCSharpType(dataType);

                    if (i == 0)
                    {
                        Parameters.Append(csharpType + "? " + columnName + ", ");
                    }
                    else
                    {
                        if (isNullable && csharpType.ToLower() != "string")
                        {
                            Parameters.Append(csharpType + "? " + columnName + ", ");
                        }
                        else
                        {
                            Parameters.Append(csharpType + " " + columnName + ", ");
                        }
                    }
                }
            }

            // To remove the ", " from the end of the text
            Parameters.Length -= 2;

            Parameters.Append(")");

            return Parameters.ToString().Trim();
        }

        private static string _GetPublicConstructor()
        {
            StringBuilder Constructor = new StringBuilder();

            Constructor.AppendLine($"public cls{_tableSingleName}()");
            Constructor.AppendLine("{");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName;
                    SqlDbType dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    if (i == 0)
                    {
                        Constructor.AppendLine($"    {columnName} = null;");
                    }
                    else
                    {
                        if (isNullable)
                        {
                            Constructor.AppendLine($"    {columnName} = null;");
                        }
                        else
                        {
                            string defaultInit = _GetDefaultInitialization(columnName, dataType);
                            Constructor.AppendLine($"    {defaultInit}");
                        }
                    }
                }
            }

            Constructor.AppendLine();
            Constructor.AppendLine("    Mode = enMode.AddNew;");
            Constructor.AppendLine("}");

            return Constructor.ToString().Trim();
        }

        private static string _GetPrivateConstructor()
        {
            StringBuilder Constructor = new StringBuilder();

            Constructor.AppendLine($"private cls{_tableSingleName}{_MakeParametersForUpdateMethod()}");
            Constructor.AppendLine("{");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName;

                    Constructor.AppendLine($"    {columnName} = {columnName.ToCamelCase()};");
                }
            }

            Constructor.AppendLine();
            Constructor.AppendLine("    Mode = enMode.Update;");
            Constructor.AppendLine("}");

            return Constructor.ToString().Trim();
        }

        private static string _MakeParametersForAddNewMethodInBusinessLayer()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 1; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> secondItem = _columnsInfo[i];

                if (secondItem.Count > 0)
                {
                    parameters.Append($"{secondItem[0].ColumnName}, ");
                }
            }

            // To remove the ", " from the end of the text
            if (parameters.Length >= 2)
            {
                parameters.Remove(parameters.Length - 2, 2);
            }

            parameters.Append(");");

            return parameters.ToString().Trim();
        }

        private static string _GetAddMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"private bool _Add()");
            Text.AppendLine("{");
            Text.AppendLine($"    {_tableSingleName}ID = cls{_tableSingleName}Data.Add{_MakeParametersForAddNewMethodInBusinessLayer()}");
            Text.AppendLine();
            Text.AppendLine($"    return ({_tableSingleName}ID.HasValue);");
            Text.AppendLine("}");

            return Text.ToString().Trim();
        }

        private static string _MakeParametersForUpdateMethodInBusinessLayer()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    parameters.Append($"{firstItem[0].ColumnName}, ");
                }
            }

            // To remove the ", " from the end of the text
            parameters.Remove(parameters.Length - 2, 2);
            parameters.Append(");");

            return parameters.ToString().Trim();
        }

        private static string _GetUpdateMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"private bool _Update()");
            Text.AppendLine("{");
            Text.AppendLine($"return cls{_tableSingleName}Data.Update{_MakeParametersForUpdateMethodInBusinessLayer()}");
            Text.AppendLine("}");

            return Text.ToString().Trim();
        }

        private static string _GetSaveMethod()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine("public bool Save()");
            Text.AppendLine("{");
            Text.AppendLine("switch (Mode)");
            Text.AppendLine("{");
            Text.AppendLine("case enMode.AddNew:");
            Text.AppendLine($"if (_Add())");
            Text.AppendLine("{");
            Text.AppendLine("Mode = enMode.Update;");
            Text.AppendLine("return true;");
            Text.AppendLine("}");
            Text.AppendLine("else");
            Text.AppendLine("{");
            Text.AppendLine("return false;");
            Text.AppendLine("}");
            Text.AppendLine();
            Text.AppendLine("case enMode.Update:");
            Text.AppendLine($"return _Update();");
            Text.AppendLine("}");
            Text.AppendLine();
            Text.AppendLine("return false;");
            Text.AppendLine("}");

            return Text.ToString().Trim();
        }

        private static string _MakeInitialParametersForFindMethodInBusinessLayer()
        {
            StringBuilder variable = new StringBuilder();

            for (int i = 1; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> secondItem = _columnsInfo[i];

                if (secondItem.Count > 0)
                {
                    string columnName = secondItem[0].ColumnName.ToCamelCase();
                    SqlDbType dataType = secondItem[0].DataType;
                    bool isNullable = secondItem[0].IsNullable;

                    string csharpType = SqlDbTypeToCSharpTypeMapper.GetCSharpType(dataType);

                    if (isNullable)
                    {
                        if (csharpType.ToLower() != "string")
                        {
                            variable.AppendLine($"{csharpType}? {columnName} = null;");
                        }
                        else
                        {
                            variable.AppendLine($"{csharpType} {columnName} = null;");
                        }
                    }
                    else
                    {
                        string defaultInit = _GetDefaultInitialization($"{csharpType} {columnName}", dataType);
                        variable.AppendLine($"    {defaultInit}");
                    }
                }
            }

            return variable.ToString().Trim();
        }

        private static string _MakeParametersForFindMethodInBusinessLayer()
        {
            StringBuilder Parameters = new StringBuilder();

            Parameters.Append("(");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();

                    if (i == 0)
                    {
                        Parameters.Append($"{columnName}, ");
                    }
                    else
                    {
                        Parameters.Append($"ref {columnName}, ");
                    }
                }
            }

            // To remove the ", " from the end of the text
            Parameters.Remove(Parameters.Length - 2, 2);
            Parameters.AppendLine(");").AppendLine();

            return Parameters.ToString().Trim();
        }

        private static string _MakeReturnParametersForFindMethodInBusinessLayer()
        {
            StringBuilder parameters = new StringBuilder();

            parameters.Append($"(new cls{_tableSingleName}(");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();

                    parameters.Append($"{columnName}, ");
                }
            }

            // To remove the ", " from the end of the text
            parameters.Remove(parameters.Length - 2, 2);
            parameters.AppendLine("))").AppendLine();

            return parameters.ToString().Trim();
        }

        private static string _MakeParametersForDeleteMethod()
        {
            StringBuilder parameters = new StringBuilder("(");

            List<clsColumnInfoForBusiness> firstItem = _columnsInfo[0];

            if (firstItem.Count > 0)
            {
                string columnName = firstItem[0].ColumnName.ToCamelCase();
                SqlDbType dataType = firstItem[0].DataType;

                string csharpType = SqlDbTypeToCSharpTypeMapper.GetCSharpType(dataType);

                parameters.Append(csharpType)
                      .Append("? ")
                      .Append(columnName)
                      .Append(")");
            }
            return parameters.ToString().Trim();
        }

        private static string _GetFindMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static cls{_tableSingleName} Find{_MakeParametersForDeleteMethod()}");
            Text.AppendLine("{");
            Text.AppendLine(_MakeInitialParametersForFindMethodInBusinessLayer());
            Text.AppendLine();

            Text.AppendLine($"bool isFound = cls{_tableSingleName}Data.GetInfoByID{_MakeParametersForFindMethodInBusinessLayer()}");
            Text.AppendLine();

            Text.Append("return (isFound) ? ")
                .Append(_MakeReturnParametersForFindMethodInBusinessLayer())
                .AppendLine(" : null;")
                .AppendLine("}");

            return Text.ToString().Trim();
        }

        private static string _GetDeleteMethodInBusinessLayer()
        {
            StringBuilder text = new StringBuilder();

            text.AppendLine($"public static bool Delete{_MakeParametersForDeleteMethod()}");
            text.AppendLine($"=> cls{_tableSingleName}Data.Delete({_tableSingleName?.ToCamelCase()}ID);");

            return text.ToString().Trim();
        }

        private static string _GetExistsMethodInBusinessLayer()
        {
            StringBuilder text = new StringBuilder();

            text.AppendLine($"public static bool Exists{_MakeParametersForDeleteMethod()}");
            text.AppendLine($"=> cls{_tableSingleName}Data.Exists({_tableSingleName?.ToCamelCase()}ID);");

            return text.ToString().Trim();
        }

        private static string _GetAllMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static DataTable All()");
            Text.AppendLine($"=> cls{_tableSingleName}Data.All();");

            return Text.ToString().Trim();
        }

        private static string _MakeInitialParametersForFindUsernameMethodInBusinessLayer()
        {
            return _MakeInitialParametersForFindMethodInBusinessLayerWithFilter
                    (columnName =>
                    {
                        return columnName.ToLower() != "username";
                    });
        }

        private static string _MakeInitialParametersForFindUsernameAndPasswordMethodInBusinessLayer()
        {
            return _MakeInitialParametersForFindMethodInBusinessLayerWithFilter
                    (columnName =>
                    {
                        return columnName.ToLower() != "username" &&
                               columnName.ToLower() != "password";
                    });
        }

        private static string _MakeParametersForFindUsernameMethodInBusinessLayer()
        {
            return _MakeParametersForFindMethodInBusinessLayerWithFilter(columnName =>
            {
                return columnName.ToLower() == "username";
            });
        }

        private static string _MakeParametersForFindUsernameAndPasswordMethodInBusinessLayer()
        {
            return _MakeParametersForFindMethodInBusinessLayerWithFilter(columnName =>
            {
                return columnName.ToLower() == "username" ||
                       columnName.ToLower() == "password";
            });
        }

        private static string _MakeInitialParametersForFindMethodInBusinessLayerWithFilter(Predicate<string> filter)
        {
            StringBuilder variable = new StringBuilder();

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();
                    SqlDbType dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    string csharpType = SqlDbTypeToCSharpTypeMapper.GetCSharpType(dataType);

                    if (filter?.Invoke(columnName) ?? false)
                    {
                        if (i == 0)
                        {
                            variable.AppendLine($"{csharpType}? {columnName} = null;");
                            continue;
                        }

                        if (isNullable)
                        {
                            if (csharpType.ToLower() != "string")
                            {
                                variable.Append($"{csharpType}? {columnName} = null;").AppendLine();
                            }
                            else
                            {
                                variable.Append($"{csharpType} {columnName} = null;").AppendLine();
                            }
                        }
                        else
                        {
                            string defaultInit = _GetDefaultInitialization($"{csharpType} {columnName}", dataType);
                            variable.AppendLine($"    {defaultInit}");
                        }
                    }
                }
            }

            return variable.ToString().Trim();
        }

        private static string _MakeParametersForFindMethodInBusinessLayerWithFilter(Predicate<string> filter)
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();

                    if (filter?.Invoke(columnName) ?? false)
                    {
                        parameters.Append($"{columnName}, ");
                    }
                    else
                    {
                        parameters.Append($"ref {columnName}, ");
                    }
                }
            }

            // To remove the ", " from the end of the text
            parameters.Remove(parameters.Length - 2, 2);
            parameters.AppendLine(");");

            return parameters.ToString().Trim();
        }

        private static string _GetFindUsernameMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static cls{_tableSingleName} Find(string username)")
                .AppendLine("{")
                .AppendLine(_MakeInitialParametersForFindUsernameMethodInBusinessLayer()).AppendLine()
                .AppendLine($"    bool isFound = cls{_tableSingleName}Data.GetInfoByUsername{_MakeParametersForFindUsernameMethodInBusinessLayer()}").AppendLine();

            Text.Append("return (isFound) ? ")
                .Append(_MakeReturnParametersForFindMethodInBusinessLayer())
                .AppendLine(" : null;")
                .AppendLine("}");

            return Text.ToString().Trim();
        }

        private static string _GetFindUsernameAndPasswordMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static cls{_tableSingleName} Find(string username, string password)")
                .AppendLine("{")
                .AppendLine(_MakeInitialParametersForFindUsernameAndPasswordMethodInBusinessLayer());

            Text.AppendLine($"    bool isFound = cls{_tableSingleName}Data.GetInfoByUsernameAndPassword{_MakeParametersForFindUsernameAndPasswordMethodInBusinessLayer()}").AppendLine();

            Text.Append("return (isFound) ? ")
                .Append(_MakeReturnParametersForFindMethodInBusinessLayer())
                .AppendLine(" : null;")
                .AppendLine("}");

            return Text.ToString().Trim();
        }

        private static string _GetExistsMethodInBusinessLayerForUsername()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static bool Exists(string username)")
                .AppendLine($"=> cls{_tableSingleName}Data.Exists(username);");

            return Text.ToString().Trim();
        }

        private static string _GetExistsMethodInBusinessLayerForUsernameAndPassword()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static bool Exists(string username, string password)")
                .AppendLine($"=> cls{_tableSingleName}Data.Exists(username, password);");

            return Text.ToString().Trim();
        }

        private static void _CreateBusinessLayer()
        {
            _tempText.AppendLine($"public class cls{_tableSingleName}");
            _tempText.AppendLine("{");
            _tempText.AppendLine("public enum enMode { AddNew = 0, Update = 1 };");
            _tempText.AppendLine("public enMode Mode = enMode.AddNew;").AppendLine();
            _tempText.AppendLine(_MakeParametersForBusinessLayer()).AppendLine();
            _tempText.AppendLine(_GetPublicConstructor()).AppendLine();
            _tempText.AppendLine(_GetPrivateConstructor()).AppendLine();
            _tempText.AppendLine(_GetAddMethodInBusinessLayer()).AppendLine();
            _tempText.AppendLine(_GetUpdateMethodInBusinessLayer()).AppendLine();
            _tempText.AppendLine(_GetSaveMethod()).AppendLine();
            _tempText.AppendLine(_GetFindMethodInBusinessLayer()).AppendLine();

            if (_isLogin)
            {
                _tempText.AppendLine(_GetFindUsernameMethodInBusinessLayer()).AppendLine();
                _tempText.AppendLine(_GetFindUsernameAndPasswordMethodInBusinessLayer()).AppendLine();
            }

            _tempText.AppendLine(_GetDeleteMethodInBusinessLayer()).AppendLine();
            _tempText.AppendLine(_GetExistsMethodInBusinessLayer()).AppendLine();

            if (_isLogin)
            {
                _tempText.AppendLine(_GetExistsMethodInBusinessLayerForUsername()).AppendLine();
                _tempText.AppendLine(_GetExistsMethodInBusinessLayerForUsernameAndPassword()).AppendLine();
            }

            _tempText.AppendLine(_GetAllMethodInBusinessLayer());
            _tempText.Append("}");
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

        private static void _LoadColumnInfo()
        {
            _columnsInfo.Clear();

            DataTable dt = clsCodeGenerator.GetColumnsNameWithInfo(_tableName, _databaseName);

            foreach (DataRow row in dt.Rows)
            {
                var columnInfo = new List<clsColumnInfoForBusiness>
                {
                    new clsColumnInfoForBusiness
                    {
                        ColumnName = row["Column Name"].ToString(),
                        DataType = row["Data Type"].ToString().ToSqlDbType(),
                        IsNullable = row["Is Nullable"].ToString().ToLower() == "yes"
                    }
                };

                _columnsInfo.Add(columnInfo);
            }

            _tableSingleName = _GetSingleColumnName();
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

        public static string Generate(List<List<clsColumnInfoForBusiness>> columnsInfo, string databaseName)
        {
            _tempText.Clear();

            _columnsInfo = columnsInfo;
            _databaseName = databaseName;

            _tableSingleName = _GetSingleColumnName();

            if (!_isGenerateAllMode)
            {
                _isLogin = _DoesTableHaveUsernameAndPassword();
            }

            _tempText.AppendLine($"using {_databaseName}DataAccess;\r\nusing System;\r\nusing System.Data;\r\n\r\nnamespace {_databaseName}Business\r\n{{");


            _CreateBusinessLayer();

            _tempText.Append("\n}");

            return _tempText.ToString();
        }

        public static void GenerateAllToFile(List<string> tablesNames, string path, string databaseName)
        {
            _isGenerateAllMode = true;
            _databaseName = databaseName;

            for (byte i = 0; i < tablesNames.Count; i++)
            {
                _tableName = tablesNames[i];

                _LoadColumnInfo();

                _isLogin = _DoesTableHaveUsernameAndPassword();

                string fullPath = path + $"cls{_tableSingleName}.cs";
                _GenerateAllClasses(fullPath);
            }

            _isGenerateAllMode = false;
        }
    }
}
