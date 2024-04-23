using CodeGeneratorBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GenerateStoredProcedureLibrary
{
    public static class clsGenerateStoredProcedures
    {
        private static string _tableName;
        private static string _databaseName;
        private static string _tableSingleName;
        private static bool _isLogin;
        private static bool _isGenerateAllMode;
        private static StringBuilder _tempText;
        private static List<List<clsColumnInfoForStoredProcedure>> _columnsInfo;

        static clsGenerateStoredProcedures()
        {
            _tableName = string.Empty;
            _databaseName = string.Empty;
            _tableSingleName = string.Empty;
            _isLogin = false;
            _isGenerateAllMode = false;
            _tempText = new StringBuilder();
            _columnsInfo = new List<List<clsColumnInfoForStoredProcedure>>();
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

        private static void _CreateGetInfoByID_SP()
        {
            _tempText.AppendLine($"create procedure SP_Get{_tableSingleName}InfoByID");
            _tempText.AppendLine($"@{_tableSingleName}ID int");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"select * from {_tableName} where {_tableSingleName}ID = @{_tableSingleName}ID");
            _tempText.AppendLine("end;");

            if (!_isGenerateAllMode)
                _tempText.AppendLine("go");
        }

        private static string _GetLengthOfTheColumn(string Column)
        {
            for (int i = 0; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForStoredProcedure> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    string columnName = firstItem[0].ColumnName;
                    string dataType = firstItem[0].DataType;
                    int? maxLength = firstItem[0].MaxLength;

                    if (columnName.ToLower() == Column.ToLower())
                    {
                        if (!maxLength.HasValue) // there is no length (the data type is not nvarchar or varchar..)
                            return dataType;
                        else
                        {
                            if (maxLength == -1) // in case the max length is MAX, so it will be -1
                                return dataType + "(MAX)";
                            else
                                return dataType + "(" + maxLength + ")";
                        }

                    }
                }
            }

            return "";
        }

        private static void _CreateGetInfoByUsername_SP()
        {
            _tempText.AppendLine($"create procedure SP_Get{_tableSingleName}InfoByUsername");
            _tempText.AppendLine($"@Username {_GetLengthOfTheColumn("username")}");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"select * from {_tableName} where Username = @Username");
            _tempText.AppendLine("end;");

            if (!_isGenerateAllMode)
                _tempText.AppendLine("go");
        }

        private static void _CreateGetInfoByUsernameAndPassword_SP()
        {
            _tempText.AppendLine($"create procedure SP_Get{_tableSingleName}InfoByUsernameAndPassword");
            _tempText.AppendLine($"@Username {_GetLengthOfTheColumn("username")},");
            _tempText.AppendLine($"@Password {_GetLengthOfTheColumn("password")}");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"select * from {_tableName} where Username = @Username and Password = @Password");
            _tempText.AppendLine("end;");

            if (!_isGenerateAllMode)
                _tempText.AppendLine("go");
        }

        private static string _GetParameters(byte StartIndex = 0)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = StartIndex; i < _columnsInfo.Count; i++)
            {
                List<clsColumnInfoForStoredProcedure> firstItem = _columnsInfo[i];

                if (firstItem.Count > 0)
                {
                    sb.AppendLine($"@{firstItem[0].ColumnName} {_GetLengthOfTheColumn(firstItem[0].ColumnName)},");
                }
            }

            if (sb.Length > 0 && StartIndex != 1)
            {
                // Remove the ", " from the end of the query in case update SP
                return _GetQueryAfterRemovingTheTrailingCommaAndSpaces(sb);
            }

            return sb.ToString();
        }

        private static string _GetExistenceCheck()
        {
            return $"if not Exists (select found = 1 from {_tableName} where Username = @Username)";
        }

        private static string _BuildInsertColumns()
        {
            var query = new StringBuilder();
            query.Append("insert into ").Append(_tableName).Append(" (");

            for (int i = 1; i < _columnsInfo.Count; i++)
            {
                var columnInfo = _columnsInfo[i];
                if (columnInfo.Count > 0)
                {
                    query.Append(columnInfo[0].ColumnName).Append(", ");
                }
            }

            // Remove the trailing comma and space
            query.Length -= 2;
            query.Append(")");

            return query.ToString();
        }

        private static string _BuildValues()
        {
            var query = new StringBuilder();
            query.Append("values (");

            for (int i = 1; i < _columnsInfo.Count; i++)
            {
                var columnInfo = _columnsInfo[i];
                if (columnInfo.Count > 0)
                {
                    query.Append("@").Append(columnInfo[0].ColumnName).Append(", ");
                }
            }

            return $"{_GetQueryAfterRemovingTheTrailingCommaAndSpaces(query)})";
        }

        private static string _GetScopeIdentity()
        {
            return $"set @New{_tableSingleName}ID = scope_identity()";
        }

        private static string _GetQueryForAddNew()
        {
            var query = new StringBuilder();

            if (_isLogin)
            {
                query.AppendLine(_GetExistenceCheck());
                query.AppendLine("begin");
            }

            query.AppendLine(_BuildInsertColumns());
            query.AppendLine(_BuildValues());

            if (_isLogin)
            {
                query.AppendLine(_GetScopeIdentity());
                query.AppendLine("end");
            }
            else
            {
                query.Append(_GetScopeIdentity());
            }

            return query.ToString();
        }

        private static void _CreateAddNew_SP()
        {
            _tempText.AppendLine($"create procedure SP_AddNew{_tableSingleName}");
            _tempText.Append($"{_GetParameters(1)}");
            _tempText.AppendLine($"@New{_tableSingleName}ID int output");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"{_GetQueryForAddNew()}");
            _tempText.AppendLine("end;");

            if (!_isGenerateAllMode)
                _tempText.AppendLine("go");
        }

        private static string _BuildSetClause()
        {
            var query = new StringBuilder();

            for (int i = 1; i < _columnsInfo.Count; i++)
            {
                var columnInfo = _columnsInfo[i];
                if (columnInfo.Count > 0)
                {
                    var columnName = columnInfo[0].ColumnName;
                    query.Append($"{columnName} = @{columnName}, ").AppendLine();
                }
            }

            return _GetQueryAfterRemovingTheTrailingCommaAndSpaces(query);
        }

        private static string _GetQueryAfterRemovingTheTrailingCommaAndSpaces(StringBuilder query)
        {
            // Remove the trailing comma and space
            int lastCommaIndex = query.ToString().LastIndexOf(",");
            if (lastCommaIndex != -1)
            {
                return query.ToString().Remove(lastCommaIndex);
            }

            return query.ToString();
        }

        private static string _BuildWhereClause()
        {
            return $"where {_tableSingleName}ID = @{_tableSingleName}ID";
        }

        private static string _GetQueryForUpdate()
        {
            var query = new StringBuilder();

            query.Append($"Update {_tableName}")
                 .AppendLine()
                 .Append("set ")
                 .Append(_BuildSetClause())
                 .AppendLine()
                 .Append(_BuildWhereClause());

            return query.ToString();
        }

        private static void _CreateUpdate_SP()
        {
            _tempText.AppendLine($"create procedure SP_Update{_tableSingleName}");
            _tempText.AppendLine($"{_GetParameters()}");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"{_GetQueryForUpdate()}");
            _tempText.AppendLine("end;");

            if (!_isGenerateAllMode)
                _tempText.AppendLine("go");
        }

        private static void _CreateDelete_SP()
        {
            _tempText.AppendLine($"create procedure SP_Delete{_tableSingleName}");
            _tempText.AppendLine($"@{_tableSingleName}ID int");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"delete {_tableName} where {_tableSingleName}ID = @{_tableSingleName}ID");
            _tempText.AppendLine("end;");

            if (!_isGenerateAllMode)
                _tempText.AppendLine("go");
        }

        private static void _CreateDoesExist_SP()
        {
            _tempText.AppendLine($"create procedure SP_Does{_tableSingleName}Exist");
            _tempText.AppendLine($"@{_tableSingleName}ID int");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"if exists(select top 1 found = 1 from {_tableName} where {_tableSingleName}ID = @{_tableSingleName}ID)");
            _tempText.AppendLine("return 1");
            _tempText.AppendLine("else");
            _tempText.AppendLine("return 0");
            _tempText.AppendLine("end;");

            if (!_isGenerateAllMode)
                _tempText.AppendLine("go");
        }

        private static void _CreateDoesExistForUsername_SP()
        {
            _tempText.AppendLine($"create procedure SP_Does{_tableSingleName}ExistByUsername");
            _tempText.AppendLine($"@Username {_GetLengthOfTheColumn("username")}");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"if exists(select top 1 found = 1 from {_tableName} where Username = @Username)");
            _tempText.AppendLine("return 1");
            _tempText.AppendLine("else");
            _tempText.AppendLine("return 0");
            _tempText.AppendLine("end;");

            if (!_isGenerateAllMode)
                _tempText.AppendLine("go");
        }

        private static void _CreateDoesExistForUsernameAndPassword_SP()
        {
            _tempText.AppendLine($"create procedure SP_Does{_tableSingleName}ExistByUsernameAndPassword");
            _tempText.AppendLine($"@Username {_GetLengthOfTheColumn("username")},");
            _tempText.AppendLine($"@Password {_GetLengthOfTheColumn("password")}");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"if exists(select top 1 found = 1 from {_tableName} where Username = @Username and Password = @Password)");
            _tempText.AppendLine("return 1");
            _tempText.AppendLine("else");
            _tempText.AppendLine("return 0");
            _tempText.AppendLine("end;");

            if (!_isGenerateAllMode)
                _tempText.AppendLine("go");
        }

        private static void _CreateGetAll_SP()
        {
            _tempText.AppendLine($"create procedure SP_GetAll{_tableName}");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"select * from {_tableName}");
            _tempText.AppendLine("end;");

            if (!_isGenerateAllMode)
                _tempText.AppendLine("go");
        }

        private static void _CreateStoredProcedures()
        {
            _CreateGetInfoByID_SP();
            _tempText.AppendLine("------------------------")
                     .AppendLine("------------------------");

            if (_isLogin)
            {
                _CreateGetInfoByUsername_SP();
                _tempText.AppendLine("------------------------")
                         .AppendLine("------------------------");

                _CreateGetInfoByUsernameAndPassword_SP();
                _tempText.AppendLine("------------------------")
                         .AppendLine("------------------------");
            }

            _CreateAddNew_SP();
            _tempText.AppendLine("------------------------")
                     .AppendLine("------------------------");

            _CreateUpdate_SP();
            _tempText.AppendLine("------------------------")
                     .AppendLine("------------------------");

            _CreateDelete_SP();
            _tempText.AppendLine("------------------------")
                     .AppendLine("------------------------");

            _CreateDoesExist_SP();
            _tempText.AppendLine("------------------------")
                     .AppendLine("------------------------");

            if (_isLogin)
            {
                _CreateDoesExistForUsername_SP();
                _tempText.AppendLine("------------------------")
                         .AppendLine("------------------------");

                _CreateDoesExistForUsernameAndPassword_SP();
                _tempText.AppendLine("------------------------")
                         .AppendLine("------------------------");
            }

            _CreateGetAll_SP();
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

        public static string Generate(List<List<clsColumnInfoForStoredProcedure>> columnsInfo, string databaseName, string tableName)
        {
            _tempText.Clear();

            // in case the table has only one column, so I don't create a stored procedure to it.
            if (columnsInfo.Count <= 1)
                return "";

            _columnsInfo = columnsInfo;
            _tableName = tableName;
            _databaseName = databaseName;

            _tableSingleName = _GetSingleColumnName();

            _isLogin = _DoesTableHaveUsernameAndPassword();

            _CreateStoredProcedures();

            return _tempText.ToString();
        }

        public static bool GenerateForOneTableToDatabase(List<List<clsColumnInfoForStoredProcedure>> columnsInfo, string databaseName, string tableName)
        {
            _isGenerateAllMode = true;
            _tableName = tableName;

            Generate(columnsInfo, databaseName, tableName);

            _isGenerateAllMode = false;
            return clsCodeGenerator.ExecuteStoredProcedure(databaseName, _tempText.ToString());
        }

        private static void _LoadColumnInfo()
        {
            _columnsInfo.Clear();

            DataTable dt = clsCodeGenerator.GetColumnsNameWithInfo(_tableName, _databaseName);

            foreach (DataRow row in dt.Rows)
            {

                var columnInfo = new List<clsColumnInfoForStoredProcedure>
                    {
                        new clsColumnInfoForStoredProcedure
                        {
                            ColumnName = row["Column Name"].ToString(),
                            DataType = row["Data Type"].ToString(),
                            IsNullable = row["Is Nullable"].ToString().ToLower() == "yes",
                            MaxLength = string.IsNullOrWhiteSpace(row["Max Length"].ToString()) ? null : (int?)Convert.ToInt32(row["Max Length"].ToString())
                        }
                    };

                _columnsInfo.Add(columnInfo);
            }

            _tableSingleName = _GetSingleColumnName();
        }

        public static bool GenerateAllToDatabase(List<string> tablesNames, string databaseName)
        {
            _isGenerateAllMode = true;
            _databaseName = databaseName;

            for (byte i = 0; i < tablesNames.Count; i++)
            {
                _tableName = tablesNames[i];

                _LoadColumnInfo();

                _isLogin = _DoesTableHaveUsernameAndPassword();

                if (!GenerateForOneTableToDatabase(_columnsInfo, _databaseName, _tableName))
                    return false;
            }

            _isGenerateAllMode = false;

            return true;
        }
    }
}
