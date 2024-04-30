using CodeGeneratorBusiness;
using GenerateBusinessLayerLibrary.Extensions;
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

        private static string _MakeParametersForBusinessLayer()
        {
            StringBuilder Parameters = new StringBuilder();

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName;
                    string dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    if (i == 0)
                    {
                        Parameters.AppendLine($"public {_GetDataTypeCSharp(dataType)}? {columnName} {{ get; set; }}");
                    }
                    else
                    {
                        if (isNullable && !_IsDataTypeString(dataType))
                        {
                            Parameters.AppendLine($"public {_GetDataTypeCSharp(dataType)}? {columnName} {{ get; set; }}");
                        }
                        else
                        {
                            Parameters.AppendLine($"public {_GetDataTypeCSharp(dataType)} {columnName} {{ get; set; }}");
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
                    string dataType = firstItem[0].DataType;
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
                            switch (dataType.ToLower())
                            {
                                case "int":
                                case "bigint":
                                    Constructor.AppendLine($"    {columnName} = -1;");
                                    break;

                                case "float":
                                    Constructor.AppendLine($"    {columnName} = -1F;");
                                    break;

                                case "decimal":
                                case "money":
                                case "smallmoney":
                                    Constructor.AppendLine($"    {columnName} = -1M;");
                                    break;

                                case "tinyint":
                                    Constructor.AppendLine($"    {columnName} = 0;");
                                    break;

                                case "smallint":
                                    Constructor.AppendLine($"    {columnName} = -1;");
                                    break;

                                case "nvarchar":
                                case "varchar":
                                case "char":
                                    Constructor.AppendLine($"    {columnName} = string.Empty;");
                                    break;

                                case "datetime":
                                case "date":
                                case "smalldatetime":
                                case "datetime2":
                                    Constructor.AppendLine($"    {columnName} = DateTime.Now;");
                                    break;

                                case "time":
                                    Constructor.AppendLine($"    {columnName} = DateTime.Now.TimeOfDay;");
                                    break;

                                case "bit":
                                    Constructor.AppendLine($"    {columnName} = false;");
                                    break;
                            }
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
                    string dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

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
            StringBuilder Variable = new StringBuilder();

            for (int i = 1; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> secondItem = _columnsInfo[i];

                if (secondItem.Count > 0)
                {
                    string columnName = secondItem[0].ColumnName.ToCamelCase();
                    string dataType = secondItem[0].DataType;
                    bool isNullable = secondItem[0].IsNullable;

                    if (isNullable)
                    {
                        if (!_IsDataTypeString(dataType))
                        {
                            Variable.AppendLine($"{_GetDataTypeCSharp(dataType)}? {columnName} = null;");
                        }
                        else
                        {
                            Variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = null;");
                        }
                    }
                    else
                    {
                        switch (dataType.ToLower())
                        {
                            case "int":
                            case "bigint":
                                Variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = -1;");
                                break;

                            case "float":
                                Variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = -1F;");
                                break;

                            case "decimal":
                            case "money":
                            case "smallmoney":
                                Variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = -1M;");
                                break;

                            case "tinyint":
                                Variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = 0;");
                                break;

                            case "smallint":
                                Variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = -1;");
                                break;

                            case "nvarchar":
                            case "varchar":
                                Variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = string.Empty;");
                                break;

                            case "char":
                                Variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = ' ';");
                                break;

                            case "datetime":
                            case "date":
                            case "smalldatetime":
                            case "datetime2":
                                Variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = DateTime.Now;");
                                break;

                            case "time":
                                Variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = DateTime.Now.TimeOfDay;");
                                break;

                            case "bit":
                                Variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = false;");
                                break;
                        }
                    }
                }
            }

            return Variable.ToString().Trim();
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
                string dataType = firstItem[0].DataType;

                parameters.Append(_GetDataTypeCSharp(dataType))
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
            StringBuilder variable = new StringBuilder();

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();
                    string dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    if (columnName.ToLower() != "username")
                    {
                        if (i == 0)
                        {
                            variable.Append(_GetDataTypeCSharp(dataType) + "? " + columnName + " = null;").AppendLine();
                            continue;
                        }

                        if (isNullable)
                        {
                            if (!_IsDataTypeString(dataType))
                            {
                                variable.Append(_GetDataTypeCSharp(dataType) + "? " + columnName + " = null;").AppendLine();
                            }
                            else
                            {
                                variable.Append(_GetDataTypeCSharp(dataType) + " " + columnName + " = null;").AppendLine();
                            }
                        }
                        else
                        {
                            switch (dataType.ToLower())
                            {
                                case "int":
                                case "bigint":
                                    variable.Append(_GetDataTypeCSharp(dataType) + " " + columnName + " = -1;").AppendLine();
                                    break;

                                case "float":
                                    variable.Append(_GetDataTypeCSharp(dataType) + " " + columnName + " = -1F;").AppendLine();
                                    break;

                                case "decimal":
                                case "money":
                                case "smallmoney":
                                    variable.Append(_GetDataTypeCSharp(dataType) + " " + columnName + " = -1M;").AppendLine();
                                    break;

                                case "tinyint":
                                    variable.Append(_GetDataTypeCSharp(dataType) + " " + columnName + " = 0;").AppendLine();
                                    break;

                                case "smallint":
                                    variable.Append(_GetDataTypeCSharp(dataType) + " " + columnName + " = -1;").AppendLine();
                                    break;

                                case "nvarchar":
                                case "varchar":
                                case "char":
                                    variable.Append(_GetDataTypeCSharp(dataType) + " " + columnName + " = string.Empty;").AppendLine();
                                    break;

                                case "datetime":
                                case "date":
                                case "smalldatetime":
                                case "datetime2":
                                    variable.Append(_GetDataTypeCSharp(dataType) + " " + columnName + " = DateTime.Now;").AppendLine();
                                    break;

                                case "time":
                                    variable.Append(_GetDataTypeCSharp(dataType) + " " + columnName + " = DateTime.Now.TimeOfDay;").AppendLine();
                                    break;

                                case "bit":
                                    variable.Append(_GetDataTypeCSharp(dataType) + " " + columnName + " = false;").AppendLine();
                                    break;
                            }
                        }
                    }
                }
            }

            return variable.ToString().Trim();
        }

        private static string _MakeParametersForFindUsernameMethodInBusinessLayer()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();

                    if (columnName.ToLower() == "username")
                    {
                        parameters.Append(columnName + ", ");
                    }
                    else
                    {
                        parameters.Append("ref " + columnName + ", ");
                    }
                }
            }

            // To remove the ", " from the end of the text
            parameters.Remove(parameters.Length - 2, 2);
            parameters.Append(");").AppendLine();

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

        private static string _MakeInitialParametersForFindUsernameAndPasswordMethodInBusinessLayer()
        {
            StringBuilder variable = new StringBuilder();

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();
                    string dataType = firstItem[0].DataType;
                    bool isNullable = firstItem[0].IsNullable;

                    if (columnName.ToLower() != "username" && columnName.ToLower() != "password")
                    {
                        if (i == 0)
                        {
                            variable.AppendLine($"{_GetDataTypeCSharp(dataType)}? {columnName} = null;");
                            continue;
                        }

                        if (isNullable)
                        {
                            if (!_IsDataTypeString(dataType))
                            {
                                variable.AppendLine($"{_GetDataTypeCSharp(dataType)}? {columnName} = null;");
                            }
                            else
                            {
                                variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = null;");
                            }
                        }
                        else
                        {
                            switch (dataType.ToLower())
                            {
                                case "int":
                                case "bigint":
                                    variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = -1;");
                                    break;

                                case "float":
                                    variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = -1F;");
                                    break;

                                case "decimal":
                                case "money":
                                case "smallmoney":
                                    variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = -1M;");
                                    break;

                                case "tinyint":
                                    variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = 0;");
                                    break;

                                case "smallint":
                                    variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = -1;");
                                    break;

                                case "nvarchar":
                                case "varchar":
                                case "char":
                                    variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = string.Empty;");
                                    break;

                                case "datetime":
                                case "date":
                                case "smalldatetime":
                                case "datetime2":
                                    variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = DateTime.Now;");
                                    break;

                                case "time":
                                    variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = DateTime.Now.TimeOfDay;");
                                    break;

                                case "bit":
                                    variable.AppendLine($"{_GetDataTypeCSharp(dataType)} {columnName} = false;");
                                    break;
                            }
                        }
                    }
                }
            }

            return variable.ToString().Trim();
        }

        private static string _MakeParametersForFindUsernameAndPasswordMethodInBusinessLayer()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForBusiness> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName.ToCamelCase();

                    if (columnName.ToLower() == "username" || columnName.ToLower() == "password")
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
                        DataType = row["Data Type"].ToString(),
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

                string fullPath = path + $"cls{_tableSingleName}Data.cs";
                _GenerateAllClasses(fullPath);
            }

            _isGenerateAllMode = false;
        }
    }
}
