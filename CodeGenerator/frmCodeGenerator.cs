using Code_Generator.Extensions;
using CodeGeneratorBusiness;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Code_Generator
{
    public partial class frmCodeGenerator : Form
    {
        private string _tableName = string.Empty;
        private string _tableSingleName = string.Empty;
        private bool _isLogin = false;
        private bool _isAdvancedMode = false;
        private bool _generateStoredProceduresInAllTables = false;
        private StringBuilder _tempText = new StringBuilder();

        public frmCodeGenerator()
        {
            InitializeComponent();
        }

        private bool _DoesTableHaveColumn(string Column)
        {
            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {

                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;

                    if (ColumnName.ToLower() == Column.ToLower())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool _DoesTableHaveUsernameAndPassword()
        {
            return (_DoesTableHaveColumn("username") && _DoesTableHaveColumn("password"));
        }

        private void _FillListViewWithTablesName()
        {
            listviewTablesName.Items.Clear();

            DataTable dt = clsCodeGenerator.GetAllTablesNameInASpecificDatabase(comboDatabaseName.Text.Trim());

            // Loop through the rows in the DataTable and add them to the ListView
            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row["TableName"].ToString()); // Column1 represents the first column in your DataTable   

                listviewTablesName.Items.Add(item);
            }

            // To show the number of records
            lblNumberOfTablesRecords.Text = listviewTablesName.Items.Count.ToString();
        }

        private void _FillComboBoxWithDatabaseName()
        {
            DataTable dtDatabase = clsCodeGenerator.GetAllDatabaseName();

            foreach (DataRow row in dtDatabase.Rows)
            {
                comboDatabaseName.Items.Add(row["DatabaseName"]);
            }

        }

        private void _FillListViewWithColumnsData()
        {
            listviewColumnsInfo.Items.Clear();

            DataTable dt = clsCodeGenerator.GetColumnsNameWithInfo(_tableName, comboDatabaseName.Text);

            // Loop through the rows in the DataTable and add them to the ListView
            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row["Column Name"].ToString()); // Column1 represents the first column in your DataTable
                item.SubItems.Add(row["Data Type"].ToString()); // Column2 represents the second column in your DataTable
                item.SubItems.Add(row["Is Nullable"].ToString()); // Column3 represents the third column in your DataTable
                item.SubItems.Add(row["Max Length"].ToString());
                // Add more sub-items as needed for additional columns

                listviewColumnsInfo.Items.Add(item);
            }

            // To get the single column name
            if (listviewColumnsInfo.Items.Count > 0)
            {
                string FirstValue = listviewColumnsInfo.Items[0].Text;
                _tableSingleName = FirstValue.Remove(FirstValue.Length - 2);
            }

            // To show the number of records
            lblNumberOfColumnsRecords.Text = listviewColumnsInfo.Items.Count.ToString();
        }

        private string _GetDataTypeCSharp(string DataType)
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

        private bool _IsDataTypeString(string DateType)
        {
            string Result = _GetDataTypeCSharp(DateType);

            return (Result.ToLower() == "string");
        }

        private void _Reset()
        {
            comboDatabaseName.SelectedIndex = -1;

            listviewColumnsInfo.Items.Clear();

            listviewTablesName.Items.Clear();

            txtData.Clear();

            txtDataAccessPath.Clear();

            txtBusinessPath.Clear();

            txtAppConfigPath.Clear();

            lblNumberOfColumnsRecords.Text = "0";

            lblNumberOfTablesRecords.Text = "0";
        }

        private bool _IsAllDataFilled(string path, string fileType)
        {
            if (MessageBox.Show($"Are you sure you want to generate {fileType} into this path?",
                "confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                == DialogResult.No)
                return false;

            if (!_IsDatabaseSelected())
                return false;

            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("You have to type a path!", "Miss Path",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private bool _IsDatabaseSelected()
        {
            if (string.IsNullOrWhiteSpace(comboDatabaseName.Text))
            {
                MessageBox.Show("You have to select a database first!", "Miss Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void _WriteToFile(string path, string value)
        {
            using (StreamWriter writer = new StreamWriter(path.Trim()))
            {
                writer.Write(value);
            }
        }

        private void _GenerateLayer(Action<string> generateLayerAction, Action showMessageAfterGenerating)
        {
            if (generateLayerAction is null)
                return;

            for (byte i = 0; i < listviewTablesName.Items.Count; i++)
            {
                _tableName = listviewTablesName.Items[i].SubItems[0].Text;

                _FillListViewWithColumnsData();

                _isLogin = _DoesTableHaveUsernameAndPassword();

                generateLayerAction?.Invoke(_tableSingleName);
            }

            showMessageAfterGenerating?.Invoke();
        }

        #region Data Access Layer
        private string _GetConnectionString()
        {
            return "SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString)";
        }

        private string _CreateCatchBlockWithIsFound()
        {
            return "catch (SqlException ex)\r\n            {\r\n                isFound = false;\r\n\r\n                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);\r\n                loggerToEventViewer.LogError(\"Database Exception\", ex);\r\n            }\r\n            catch (Exception ex)\r\n            {\r\n                isFound = false;\r\n\r\n                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);\r\n                loggerToEventViewer.LogError(\"General Exception\", ex);\r\n            }";
        }

        private string _CreateCatchBlockWithoutIsFound()
        {
            return "catch (SqlException ex)\r\n            {\r\n                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);\r\n                loggerToEventViewer.LogError(\"Database Exception\", ex);\r\n            }\r\n            catch (Exception ex)\r\n            {\r\n                clsErrorLogger loggerToEventViewer = new clsErrorLogger(clsLogHandler.LogToEventViewer);\r\n                loggerToEventViewer.LogError(\"General Exception\", ex);\r\n            }";
        }

        private string _MakeParametersForFindMethod()
        {
            StringBuilder parameters = new StringBuilder();

            parameters.Append("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    string columnName = firstItem.SubItems[0].Text.ToCamelCase();
                    string dataType = firstItem.SubItems[1].Text;
                    string isNullable = firstItem.SubItems[2].Text;

                    if (i == 0)
                    {
                        parameters.Append(_GetDataTypeCSharp(dataType) + "? " + columnName + ", ");
                    }
                    else
                    {
                        if (isNullable.ToUpper() == "YES" && !_IsDataTypeString(dataType))
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

        private string _FillTheVariableWithDataThatComingFromDatabase()
        {
            StringBuilder text = new StringBuilder();

            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem secondItem = listviewColumnsInfo.Items[i];

                if (secondItem.SubItems.Count > 0)
                {
                    string columnName = secondItem.SubItems[0].Text;
                    string dataType = secondItem.SubItems[1].Text;
                    string isNullable = secondItem.SubItems[2].Text;

                    if (isNullable.ToUpper() == "YES")
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

        private void _CreateGetInfoMethod()
        {
            _tempText.Append($"public static bool GetInfoByID{_MakeParametersForFindMethod()}");
            _tempText.AppendLine();
            _tempText.AppendLine("{");

            _tempText.AppendLine("    bool isFound = false;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({_GetConnectionString()})");
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
            _tempText.AppendLine(_CreateCatchBlockWithIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return isFound;");
            _tempText.AppendLine("}");
        }

        private string _MakeParametersForFindMethodForUsername()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    string columnName = firstItem.SubItems[0].Text.ToCamelCase();
                    string dataType = firstItem.SubItems[1].Text;
                    string isNullable = firstItem.SubItems[2].Text;

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
                            if (isNullable.ToUpper() == "YES" && !_IsDataTypeString(dataType))
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

        private string _FillTheVariableWithDataThatComingFromDatabaseForUsername()
        {
            StringBuilder Text = new StringBuilder();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstValue = listviewColumnsInfo.Items[i];

                if (firstValue.SubItems.Count > 0)
                {
                    string columnName = firstValue.SubItems[0].Text;
                    string dataType = firstValue.SubItems[1].Text;
                    string isNullable = firstValue.SubItems[2].Text;

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

                        if (isNullable == "YES")
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

        private void _CreateGetInfoMethodForUsername()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool GetInfoByUsername{_MakeParametersForFindMethodForUsername()}");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    bool isFound = false;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({_GetConnectionString()})");
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
            _tempText.AppendLine(_CreateCatchBlockWithIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return isFound;");
            _tempText.AppendLine("}");
        }

        private string _MakeParametersForFindMethodForUsernameAndPassword()
        {
            StringBuilder parameters = new StringBuilder();

            parameters.Append("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    string columnName = firstItem.SubItems[0].Text.ToCamelCase();
                    string dataType = firstItem.SubItems[1].Text;
                    string isNullable = firstItem.SubItems[2].Text;

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
                        if (isNullable.ToUpper() == "YES" && !_IsDataTypeString(dataType))
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

        private string _FillTheVariableWithDataThatComingFromDatabaseForUsernameAndPassword()
        {
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstValue = listviewColumnsInfo.Items[i];

                if (firstValue.SubItems.Count > 0)
                {
                    string columnName = firstValue.SubItems[0].Text;
                    string dataType = firstValue.SubItems[1].Text;
                    string isNullable = firstValue.SubItems[2].Text;

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

                        if (isNullable == "YES")
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

        private void _CreateGetInfoMethodForUsernameAndPassword()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool GetInfoByUsernameAndPassword{_MakeParametersForFindMethodForUsernameAndPassword()}");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    bool isFound = false;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({_GetConnectionString()})");
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
            _tempText.AppendLine(_CreateCatchBlockWithIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return isFound;");
            _tempText.AppendLine("}");
        }

        private string _MakeParametersForAddNewMethod()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem secondRow = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (secondRow.SubItems.Count > 0)
                {
                    string columnName = secondRow.SubItems[0].Text.ToCamelCase();
                    string dataType = secondRow.SubItems[1].Text;
                    string isNullable = secondRow.SubItems[2].Text;

                    if (isNullable.ToUpper() == "YES" && !_IsDataTypeString(dataType))
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

        private string _FillParametersInTheCommand()
        {
            StringBuilder text = new StringBuilder();

            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem secondItem = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (secondItem.SubItems.Count > 0)
                {
                    string columnName = secondItem.SubItems[0].Text;
                    string isNullable = secondItem.SubItems[2].Text;

                    text.Append($"command.Parameters.AddWithValue(\"@{columnName}\", ");

                    if (isNullable == "YES")
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

        private void _CreateAddMethod()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static int? Add{_MakeParametersForAddNewMethod()}");
            _tempText.AppendLine("{");
            _tempText.AppendLine("// This function will return the new person id if succeeded and null if not");
            _tempText.AppendLine($"    int? {_tableSingleName.ToCamelCase()}ID = null;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({_GetConnectionString()})");
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
            _tempText.AppendLine(_CreateCatchBlockWithoutIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine($"    return {_tableSingleName.ToCamelCase()}ID;");
            _tempText.AppendLine("}");
        }

        private string _MakeParametersForUpdateMethod()
        {
            StringBuilder Parameters = new StringBuilder("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem secondRow = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (secondRow.SubItems.Count > 0)
                {
                    string columnName = secondRow.SubItems[0].Text?.ToCamelCase();
                    string dataType = secondRow.SubItems[1].Text;
                    string isNullable = secondRow.SubItems[2].Text;

                    if (i == 0)
                    {
                        Parameters.Append(_GetDataTypeCSharp(dataType) + "? " + columnName + ", ");
                    }
                    else
                    {
                        if (isNullable.ToUpper() == "YES" && !_IsDataTypeString(dataType))
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

        private void _CreateUpdateMethod()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool Update{_MakeParametersForUpdateMethod()}");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    int rowAffected = 0;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({_GetConnectionString()})");
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
            _tempText.AppendLine(_CreateCatchBlockWithoutIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine($"    return (rowAffected > 0);");
            _tempText.AppendLine("}");
        }

        private string _MakeParametersForDeleteMethod()
        {
            StringBuilder parameters = new StringBuilder("(");

            ListViewItem secondRow = listviewColumnsInfo.Items[0]; // Access the second row (index 0)

            if (secondRow.SubItems.Count > 0)
            {
                string columnName = secondRow.SubItems[0].Text.ToCamelCase();
                string dataType = secondRow.SubItems[1].Text;

                parameters.Append(_GetDataTypeCSharp(dataType))
                          .Append("? ")
                          .Append(columnName)
                          .Append(")");
            }

            return parameters.ToString().Trim();
        }

        private void _CreateDeleteMethod()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool Delete{_MakeParametersForDeleteMethod()}");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    int rowAffected = 0;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({_GetConnectionString()})");
            _tempText.AppendLine("        {");
            _tempText.AppendLine("            connection.Open();");
            _tempText.AppendLine();

            _tempText.AppendLine($"            using (SqlCommand command = new SqlCommand(\"SP_Delete{_tableSingleName}\", connection))");
            _tempText.AppendLine("            {");
            _tempText.AppendLine("command.CommandType = CommandType.StoredProcedure;").AppendLine();
            _tempText.AppendLine($"                command.Parameters.AddWithValue(\"@{_tableSingleName}ID\", (object){_tableSingleName?.ToCamelCase()}ID ?? DBNull.Value);");
            _tempText.AppendLine();

            _tempText.AppendLine("                rowAffected = command.ExecuteNonQuery();");
            _tempText.AppendLine("            }");
            _tempText.AppendLine("        }");
            _tempText.AppendLine("    }");
            _tempText.AppendLine(_CreateCatchBlockWithoutIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine($"    return (rowAffected > 0);");
            _tempText.AppendLine("}");
        }

        private void _CreateExistsMethod()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool Exists{_MakeParametersForDeleteMethod()}");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    bool isFound = false;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({_GetConnectionString()})");
            _tempText.AppendLine("        {");
            _tempText.AppendLine("            connection.Open();");
            _tempText.AppendLine();

            _tempText.AppendLine($"            using (SqlCommand command = new SqlCommand(\"SP_Does{_tableSingleName}Exist\", connection))");
            _tempText.AppendLine("            {");
            _tempText.AppendLine("command.CommandType = CommandType.StoredProcedure;").AppendLine();
            _tempText.AppendLine($"                command.Parameters.AddWithValue(\"@{_tableSingleName}ID\", (object){_tableSingleName?.ToCamelCase()}ID ?? DBNull.Value);");
            _tempText.AppendLine();
            _tempText.AppendLine("// @ReturnVal could be any name, and we don't need to add it to the SP, just use it here in the code.");
            _tempText.AppendLine("SqlParameter returnParameter = new SqlParameter(\"@ReturnVal\", SqlDbType.Int)");
            _tempText.AppendLine("{").AppendLine("Direction = ParameterDirection.ReturnValue").AppendLine("};");
            _tempText.AppendLine("command.Parameters.Add(returnParameter);");
            _tempText.AppendLine();

            _tempText.AppendLine("command.ExecuteNonQuery();");
            _tempText.AppendLine();
            _tempText.AppendLine("isFound = (int)returnParameter.Value == 1;");
            _tempText.AppendLine("            }");
            _tempText.AppendLine("        }");
            _tempText.AppendLine("    }");
            _tempText.AppendLine(_CreateCatchBlockWithIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return isFound;");
            _tempText.AppendLine("}");
        }

        private void _CreateExistsMethodForUsername()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool Exists(string username)");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    bool isFound = false;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({_GetConnectionString()})");
            _tempText.AppendLine("        {");
            _tempText.AppendLine("            connection.Open();");
            _tempText.AppendLine();

            _tempText.AppendLine($"            using (SqlCommand command = new SqlCommand(\"SP_Does{_tableSingleName}ExistByUsername\", connection))");
            _tempText.AppendLine("            {");
            _tempText.AppendLine("command.CommandType = CommandType.StoredProcedure;").AppendLine();
            _tempText.AppendLine($"                command.Parameters.AddWithValue(\"@Username\", username);");
            _tempText.AppendLine();
            _tempText.AppendLine("// @ReturnVal could be any name, and we don't need to add it to the SP, just use it here in the code.");
            _tempText.AppendLine("SqlParameter returnParameter = new SqlParameter(\"@ReturnVal\", SqlDbType.Int)");
            _tempText.AppendLine("{").AppendLine("Direction = ParameterDirection.ReturnValue").AppendLine("};");
            _tempText.AppendLine("command.Parameters.Add(returnParameter);");
            _tempText.AppendLine();

            _tempText.AppendLine("command.ExecuteNonQuery();");
            _tempText.AppendLine();
            _tempText.AppendLine("isFound = (int)returnParameter.Value == 1;");
            _tempText.AppendLine("            }");
            _tempText.AppendLine("        }");
            _tempText.AppendLine("    }");
            _tempText.AppendLine(_CreateCatchBlockWithIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return isFound;");
            _tempText.AppendLine("}");
        }

        private void _CreateExistsMethodForUsernameAndPassword()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static bool Exists(string username, string password)");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    bool isFound = false;");
            _tempText.AppendLine();

            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({_GetConnectionString()})");
            _tempText.AppendLine("        {");
            _tempText.AppendLine("            connection.Open();");
            _tempText.AppendLine();

            _tempText.AppendLine($"            using (SqlCommand command = new SqlCommand(\"SP_Does{_tableSingleName}ExistByUsernameAndPassword\", connection))");
            _tempText.AppendLine("            {");
            _tempText.AppendLine("command.CommandType = CommandType.StoredProcedure;").AppendLine();
            _tempText.AppendLine($"                command.Parameters.AddWithValue(\"@Username\", username);");
            _tempText.AppendLine($"                command.Parameters.AddWithValue(\"@Password\", password);");
            _tempText.AppendLine();
            _tempText.AppendLine("// @ReturnVal could be any name, and we don't need to add it to the SP, just use it here in the code.");
            _tempText.AppendLine("SqlParameter returnParameter = new SqlParameter(\"@ReturnVal\", SqlDbType.Int)");
            _tempText.AppendLine("{").AppendLine("Direction = ParameterDirection.ReturnValue").AppendLine("};");
            _tempText.AppendLine("command.Parameters.Add(returnParameter);");
            _tempText.AppendLine();

            _tempText.AppendLine("command.ExecuteNonQuery();");
            _tempText.AppendLine();
            _tempText.AppendLine("isFound = (int)returnParameter.Value == 1;");
            _tempText.AppendLine("            }");
            _tempText.AppendLine("        }");
            _tempText.AppendLine("    }");
            _tempText.AppendLine(_CreateCatchBlockWithIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return isFound;");
            _tempText.AppendLine("}");
        }

        private void _CreateAllMethod()
        {
            _tempText.AppendLine();
            _tempText.AppendLine($"public static DataTable All()");
            _tempText.AppendLine("{");
            _tempText.AppendLine($"return clsDataAccessHelper.All(\"SP_GetAll{_tableName}\");");
            _tempText.AppendLine("}");
        }

        private void _CreateDataAccessSettingsClass()
        {
            _tempText.Clear();

            _tempText.Append($"using System.Configuration;\r\n\r\nnamespace {comboDatabaseName.Text}_DataAccess\r\n{{\r\n    static class clsDataAccessSettings\r\n    {{\r\n        public static string ConnectionString = ConfigurationManager.ConnectionStrings[\"ConnectionString\"].ConnectionString;\r\n    }}\r\n}}");

            StringBuilder path = new StringBuilder();

            path.Append(txtDataAccessPath.Text.Trim() + "clsDataAccessSettings.cs");

            if (_isAdvancedMode)
                _WriteToFile(path.ToString(), _tempText.ToString());
        }

        private void _CreateLogHandlerClass()
        {
            _tempText.Clear();

            _tempText.Append("using System;\r\n" +
                             "using System.Configuration;\r\n" +
                             "using System.Diagnostics;\r\n\r\n" +
                             $"namespace {comboDatabaseName.Text}_DataAccess\r\n{{\r\n" +
                             "public class clsLogHandler\r\n" +
                             "{\r\n" +
                             "public static void LogToEventViewer(string errorType, Exception ex)\r\n" +
                             "{\r\n string sourceName = ConfigurationManager.AppSettings[\"ProjectName\"];\r\n\r\n" +
                             "// Create the event source if it does not exist\r\n" +
                             "if (!EventLog.SourceExists(sourceName))\r\n" +
                             "{\r\n" +
                             "EventLog.CreateEventSource(sourceName, \"Application\");\r\n" +
                             "}\r\n\r\n" +
                             "string errorMessage = $\"{errorType} in {ex.Source}\\n\\nException Message:\"" +
                             " +\r\n$\" {ex.Message}\\n\\nException Type: {ex.GetType().Name}\\n\\nStack Trace:" +
                             "\" +\r\n$\" {ex.StackTrace}\\n\\nException Location: " +
                             "{ex.TargetSite}\";\r\n\r\n" +
                             "// Log an error event\r\n" +
                             "EventLog.WriteEntry(sourceName, errorMessage, EventLogEntryType.Error);\r\n" +
                             "}\r\n}\r\n}");

            StringBuilder path = new StringBuilder();

            path.Append(txtDataAccessPath.Text.Trim() + "clsLogHandler.cs");

            if (_isAdvancedMode)
                _WriteToFile(path.ToString(), _tempText.ToString());
        }

        private void _CreateErrorLoggerClass()
        {
            _tempText.Clear();

            _tempText.Append("using System;\r\n\r\n" +
                             $"namespace {comboDatabaseName.Text}_DataAccess\r\n{{\r\n" +
                             "public class clsErrorLogger\r\n" +
                             "{\r\n" +
                             "private Action<string, Exception> _logAction;\r\n\r\n" +
                             "public clsErrorLogger(Action<string, Exception> logAction)\r\n" +
                             "{\r\n" +
                             "_logAction = logAction;\r\n" +
                             "}\r\n\r\n" +
                             "public void LogError(string errorType, Exception ex)\r\n" +
                             "{\r\n" +
                             "_logAction?.Invoke(errorType, ex);\r\n" +
                             "}\r\n}\r\n}");

            StringBuilder path = new StringBuilder();

            path.Append(txtDataAccessPath.Text.Trim() + "clsErrorLogger.cs");

            if (_isAdvancedMode)
                _WriteToFile(path.ToString(), _tempText.ToString());
        }

        private void _CreateClassesThatRelatedToLoggingErrors()
        {
            // Main class
            _CreateErrorLoggerClass();

            // sub classes to handle where you want to log the errors
            _CreateLogHandlerClass();
        }

        private void _CreateCountMethodHelper()
        {
            _tempText.AppendLine();
            _tempText.AppendLine("public static int Count(string storedProcedureName)");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    int Count = 0;");
            _tempText.AppendLine();
            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({_GetConnectionString()})");
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
            _tempText.AppendLine(_CreateCatchBlockWithoutIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return Count;");
            _tempText.AppendLine("}");
        }

        private void _CreateAllMethodHelper()
        {
            _tempText.AppendLine();
            _tempText.AppendLine("public static DataTable All(string storedProcedureName)");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    DataTable dt = new DataTable();");
            _tempText.AppendLine();
            _tempText.AppendLine("    try");
            _tempText.AppendLine("    {");
            _tempText.AppendLine($"        using ({_GetConnectionString()})");
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
            _tempText.AppendLine(_CreateCatchBlockWithoutIsFound());
            _tempText.AppendLine();
            _tempText.AppendLine("    return dt;");
            _tempText.AppendLine("}");
        }

        private void _CreateDataAccessHelperClass()
        {
            _tempText.AppendLine("using System;");
            _tempText.AppendLine("using System.Data;");
            _tempText.AppendLine("using System.Data.SqlClient;");
            _tempText.AppendLine();

            _tempText.AppendLine($"namespace {comboDatabaseName.Text}_DataAccess");
            _tempText.AppendLine("{");
            _tempText.AppendLine("    public static class clsDataAccessHelper");
            _tempText.AppendLine("    {");

            // Generate Count method
            _CreateCountMethodHelper();

            // Generate GetAll method
            _CreateAllMethodHelper();

            _tempText.AppendLine("    }");
            _tempText.AppendLine("}");
        }

        private void _CreateDataAccessHelperClassToTheFile()
        {
            _tempText.Clear();

            _CreateDataAccessHelperClass();

            StringBuilder path = new StringBuilder();

            path.Append(txtDataAccessPath.Text.Trim() + "clsDataAccessHelper.cs");

            if (_isAdvancedMode)
                _WriteToFile(path.ToString(), _tempText.ToString());
        }

        private void _DataAccessAsLoginInfo()
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
        }

        private void _DataAccessAsNormal()
        {
            _CreateGetInfoMethod();
            _CreateAddMethod();
            _CreateUpdateMethod();
            _CreateDeleteMethod();
            _CreateExistsMethod();
            _CreateAllMethod();
        }

        private void _GenerateAllClassesInDataAccess(string path)
        {
            btnGenerateDateAccessLayer.PerformClick();

            _WriteToFile(path.Trim(), _tempText.ToString());
        }
        #endregion

        #region Business Layer
        private string _MakeParametersForBusinessLayer()
        {
            StringBuilder Parameters = new StringBuilder();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;
                    string DataType = firstItem.SubItems[1].Text;
                    string IsNullable = firstItem.SubItems[2].Text;

                    if (i == 0)
                    {
                        Parameters.AppendLine($"public {_GetDataTypeCSharp(DataType)}? {ColumnName} {{ get; set; }}");
                    }
                    else
                    {
                        if (IsNullable.ToUpper() == "YES" && !_IsDataTypeString(DataType))
                        {
                            Parameters.AppendLine($"public {_GetDataTypeCSharp(DataType)}? {ColumnName} {{ get; set; }}");
                        }
                        else
                        {
                            Parameters.AppendLine($"public {_GetDataTypeCSharp(DataType)} {ColumnName} {{ get; set; }}");
                        }
                    }
                }
            }

            return Parameters.ToString().Trim();
        }

        private string _GetPublicConstructor()
        {
            StringBuilder Constructor = new StringBuilder();

            Constructor.AppendLine($"public cls{_tableSingleName}()");
            Constructor.AppendLine("{");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;
                    string DataType = firstItem.SubItems[1].Text;
                    string IsNullable = firstItem.SubItems[2].Text;

                    if (i == 0)
                    {
                        Constructor.AppendLine($"    {ColumnName} = null;");
                    }
                    else
                    {
                        if (IsNullable.ToUpper() == "YES")
                        {
                            Constructor.AppendLine($"    {ColumnName} = null;");
                        }
                        else
                        {
                            switch (DataType.ToLower())
                            {
                                case "int":
                                case "bigint":
                                    Constructor.AppendLine($"    {ColumnName} = -1;");
                                    break;

                                case "float":
                                    Constructor.AppendLine($"    {ColumnName} = -1F;");
                                    break;

                                case "decimal":
                                case "money":
                                case "smallmoney":
                                    Constructor.AppendLine($"    {ColumnName} = -1M;");
                                    break;

                                case "tinyint":
                                    Constructor.AppendLine($"    {ColumnName} = 0;");
                                    break;

                                case "smallint":
                                    Constructor.AppendLine($"    {ColumnName} = -1;");
                                    break;

                                case "nvarchar":
                                case "varchar":
                                case "char":
                                    Constructor.AppendLine($"    {ColumnName} = string.Empty;");
                                    break;

                                case "datetime":
                                case "date":
                                case "smalldatetime":
                                case "datetime2":
                                    Constructor.AppendLine($"    {ColumnName} = DateTime.Now;");
                                    break;

                                case "time":
                                    Constructor.AppendLine($"    {ColumnName} = DateTime.Now.TimeOfDay;");
                                    break;

                                case "bit":
                                    Constructor.AppendLine($"    {ColumnName} = false;");
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

        private string _GetPrivateConstructor()
        {
            StringBuilder Constructor = new StringBuilder();

            Constructor.AppendLine($"private cls{_tableSingleName}{_MakeParametersForUpdateMethod()}");
            Constructor.AppendLine("{");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;
                    string DataType = firstItem.SubItems[1].Text;
                    string IsNullable = firstItem.SubItems[2].Text;

                    Constructor.AppendLine($"    {ColumnName} = {ColumnName.ToCamelCase()};");
                }
            }

            Constructor.AppendLine();
            Constructor.AppendLine("    Mode = enMode.Update;");
            Constructor.AppendLine("}");

            return Constructor.ToString().Trim();
        }

        private string _MakeParametersForAddNewMethodInBusinessLayer()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem secondRow = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (secondRow.SubItems.Count > 0)
                {
                    string ColumnName = secondRow.SubItems[0].Text;

                    parameters.Append($"{ColumnName}, ");
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

        private string _GetAddMethodInBusinessLayer()
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

        private string _MakeParametersForUpdateMethodInBusinessLayer()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstRow = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstRow.SubItems.Count > 0)
                {
                    string columnName = firstRow.SubItems[0].Text;
                    parameters.Append($"{columnName}, ");
                }
            }

            // To remove the ", " from the end of the text
            parameters.Remove(parameters.Length - 2, 2);
            parameters.Append(");");

            return parameters.ToString().Trim();
        }

        private string _GetUpdateMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"private bool _Update()");
            Text.AppendLine("{");
            Text.AppendLine($"return cls{_tableSingleName}Data.Update{_MakeParametersForUpdateMethodInBusinessLayer()}");
            Text.AppendLine("}");

            return Text.ToString().Trim();
        }

        private string _GetSaveMethod()
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

        private string _MakeInitialParametersForFindMethodInBusinessLayer()
        {
            StringBuilder Variable = new StringBuilder();

            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem SecondRow = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (SecondRow.SubItems.Count > 0)
                {
                    string ColumnName = SecondRow.SubItems[0].Text?.ToCamelCase();
                    string DataType = SecondRow.SubItems[1].Text;
                    string IsNullable = SecondRow.SubItems[2].Text;

                    if (IsNullable.ToUpper() == "YES")
                    {
                        if (!_IsDataTypeString(DataType))
                        {
                            Variable.AppendLine($"{_GetDataTypeCSharp(DataType)}? {ColumnName} = null;");
                        }
                        else
                        {
                            Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = null;");
                        }
                    }
                    else
                    {
                        switch (DataType.ToLower())
                        {
                            case "int":
                            case "bigint":
                                Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = -1;");
                                break;

                            case "float":
                                Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = -1F;");
                                break;

                            case "decimal":
                            case "money":
                            case "smallmoney":
                                Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = -1M;");
                                break;

                            case "tinyint":
                                Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = 0;");
                                break;

                            case "smallint":
                                Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = -1;");
                                break;

                            case "nvarchar":
                            case "varchar":
                                Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = string.Empty;");
                                break;

                            case "char":
                                Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = ' ';");
                                break;

                            case "datetime":
                            case "date":
                            case "smalldatetime":
                            case "datetime2":
                                Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = DateTime.Now;");
                                break;

                            case "time":
                                Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = DateTime.Now.TimeOfDay;");
                                break;

                            case "bit":
                                Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = false;");
                                break;
                        }
                    }
                }
            }

            return Variable.ToString().Trim();
        }

        private string _MakeParametersForFindMethodInBusinessLayer()
        {
            StringBuilder Parameters = new StringBuilder();

            Parameters.Append("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text?.ToCamelCase();

                    if (i == 0)
                    {
                        Parameters.Append($"{ColumnName}, ");
                    }
                    else
                    {
                        Parameters.Append($"ref {ColumnName}, ");
                    }
                }
            }

            // To remove the ", " from the end of the text
            Parameters.Remove(Parameters.Length - 2, 2);
            Parameters.AppendLine(");").AppendLine();

            return Parameters.ToString().Trim();
        }

        private string _MakeReturnParametersForFindMethodInBusinessLayer()
        {
            StringBuilder parameters = new StringBuilder();

            parameters.Append($"(new cls{_tableSingleName}(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string columnName = firstItem.SubItems[0].Text?.ToCamelCase();

                    parameters.Append($"{columnName}, ");
                }
            }

            // To remove the ", " from the end of the text
            parameters.Remove(parameters.Length - 2, 2);
            parameters.AppendLine("))").AppendLine();

            return parameters.ToString().Trim();
        }

        private string _GetFindMethodInBusinessLayer()
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

        private string _GetDeleteMethodInBusinessLayer()
        {
            StringBuilder text = new StringBuilder();

            text.AppendLine($"public static bool Delete{_MakeParametersForDeleteMethod()}");
            text.AppendLine("{");
            text.AppendLine($"return cls{_tableSingleName}Data.Delete({_tableSingleName?.ToCamelCase()}ID);");
            text.AppendLine("}");

            return text.ToString().Trim();
        }

        private string _GetExistsMethodInBusinessLayer()
        {
            StringBuilder text = new StringBuilder();

            text.AppendLine($"public static bool Exists{_MakeParametersForDeleteMethod()}");
            text.AppendLine("{");
            text.AppendLine($"return cls{_tableSingleName}Data.Exists({_tableSingleName?.ToCamelCase()}ID);");
            text.AppendLine("}");

            return text.ToString().Trim();
        }

        private string _GetAllMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static DataTable All()");
            Text.AppendLine("{");
            Text.AppendLine($"return cls{_tableSingleName}Data.All();");
            Text.AppendLine("}");

            return Text.ToString().Trim();
        }

        private string _MakeInitialParametersForFindUsernameMethodInBusinessLayer()
        {
            StringBuilder variable = new StringBuilder();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstRow = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (firstRow.SubItems.Count > 0)
                {
                    string columnName = firstRow.SubItems[0].Text?.ToCamelCase();
                    string dataType = firstRow.SubItems[1].Text;
                    string isNullable = firstRow.SubItems[2].Text;

                    if (columnName.ToLower() != "username")
                    {
                        if (i == 0)
                        {
                            variable.Append(_GetDataTypeCSharp(dataType) + "? " + columnName + " = null;").AppendLine();
                            continue;
                        }

                        if (isNullable.ToUpper() == "YES")
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

        private string _MakeParametersForFindUsernameMethodInBusinessLayer()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string columnName = firstItem.SubItems[0].Text?.ToCamelCase();

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

        private string _GetFindUsernameMethodInBusinessLayer()
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

        private string _MakeInitialParametersForFindUsernameAndPasswordMethodInBusinessLayer()
        {
            StringBuilder variable = new StringBuilder();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstRow = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (firstRow.SubItems.Count > 0)
                {
                    string columnName = firstRow.SubItems[0].Text?.ToCamelCase();
                    string dataType = firstRow.SubItems[1].Text;
                    string isNullable = firstRow.SubItems[2].Text;

                    if (columnName.ToLower() != "username" && columnName.ToLower() != "password")
                    {
                        if (i == 0)
                        {
                            variable.AppendLine($"{_GetDataTypeCSharp(dataType)}? {columnName} = null;");
                            continue;
                        }

                        if (isNullable.ToUpper() == "YES")
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

        private string _MakeParametersForFindUsernameAndPasswordMethodInBusinessLayer()
        {
            StringBuilder parameters = new StringBuilder("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string columnName = firstItem.SubItems[0].Text?.ToCamelCase();

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

        private string _GetFindUsernameAndPasswordMethodInBusinessLayer()
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

        private string _GetExistsMethodInBusinessLayerForUsername()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static bool Exists(string username)")
                .AppendLine("{")
                .AppendLine($"    return cls{_tableSingleName}Data.Exists(username);")
                .AppendLine("}");

            return Text.ToString().Trim();
        }

        private string _GetExistsMethodInBusinessLayerForUsernameAndPassword()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static bool Exists(string username, string password)")
                .AppendLine("{")
                .AppendLine($"    return cls{_tableSingleName}Data.Exists(username, password);")
                .AppendLine("}");

            return Text.ToString().Trim();
        }

        private void _CreateBusinessLayer()
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

        private void _GenerateAllClassesInBusiness(string path)
        {
            btnGenerateBusinessLayer.PerformClick();

            _WriteToFile(path.Trim(), _tempText.ToString());
        }

        #endregion

        #region Stored Procedure
        private void _CreateGetInfoByID_SP()
        {
            _tempText.AppendLine($"create procedure SP_Get{_tableSingleName}InfoByID");
            _tempText.AppendLine($"@{_tableSingleName}ID int");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"select * from {_tableName} where {_tableSingleName}ID = @{_tableSingleName}ID");
            _tempText.AppendLine("end;");

            if (!_isAdvancedMode)
                _tempText.AppendLine("go");
        }

        private string _GetLengthOfTheColumn(string Column)
        {
            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;
                    string DataType = firstItem.SubItems[1].Text;
                    //string IsNullable = firstItem.SubItems[2].Text;
                    string MaxLength = firstItem.SubItems[3].Text;

                    if (ColumnName.ToLower() == Column.ToLower())
                    {
                        if (string.IsNullOrWhiteSpace(MaxLength)) // there is no length (the data type is not nvarchar or varchar..)
                            return DataType;
                        else
                        {
                            if (MaxLength == "-1") // in case the max length is MAX, so it will be -1
                                return DataType + "(MAX)";
                            else
                                return DataType + "(" + MaxLength + ")";
                        }

                    }
                }
            }

            return "";
        }

        private void _CreateGetInfoByUsername_SP()
        {
            _tempText.AppendLine($"create procedure SP_Get{_tableSingleName}InfoByUsername");
            _tempText.AppendLine($"@Username {_GetLengthOfTheColumn("username")}");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"select * from {_tableName} where Username = @Username");
            _tempText.AppendLine("end;");

            if (!_isAdvancedMode)
                _tempText.AppendLine("go");
        }

        private void _CreateGetInfoByUsernameAndPassword_SP()
        {
            _tempText.AppendLine($"create procedure SP_Get{_tableSingleName}InfoByUsernameAndPassword");
            _tempText.AppendLine($"@Username {_GetLengthOfTheColumn("username")},");
            _tempText.AppendLine($"@Password {_GetLengthOfTheColumn("password")}");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"select * from {_tableName} where Username = @Username and Password = @Password");
            _tempText.AppendLine("end;");

            if (!_isAdvancedMode)
                _tempText.AppendLine("go");
        }

        private string _GetParameters(byte StartIndex = 0)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = StartIndex; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;

                    sb.AppendLine($"@{ColumnName} {_GetLengthOfTheColumn(ColumnName)},");
                }
            }

            if (sb.Length > 0 && StartIndex != 1)
                // Remove the ", " from the end of the query in case update SP
                sb.Length -= 3;

            return sb.ToString();
        }

        private string _GetQueryForAddNew()
        {
            StringBuilder query = new StringBuilder();

            if (_isLogin)
            {
                query.AppendLine($"if not Exists (select found = 1 from {_tableName} where Username = @Username)");
                query.AppendLine("begin");
                query.Append($"insert into {_tableName} (");
            }
            else
            {
                query.Append($"insert into {_tableName} (");
            }

            // Print the header of the columns
            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem SecondItem = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (SecondItem.SubItems.Count > 0)
                {
                    string ColumnName = SecondItem.SubItems[0].Text;
                    query.Append(ColumnName).Append(", ");
                }
            }

            // Remove the ", " from the end of the query
            query.Length -= 2;

            query.AppendLine(")");

            query.Append("values (");

            // Print the values
            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem SecondItem = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (SecondItem.SubItems.Count > 0)
                {
                    string ColumnName = SecondItem.SubItems[0].Text;
                    query.Append("@").Append(ColumnName).Append(", ");
                }
            }

            // Remove the ", " from the end of the query
            query.Length -= 2;

            query.AppendLine(")");

            if (_isLogin)
            {
                query.AppendLine($"set @New{_tableSingleName}ID = scope_identity()");
                query.Append("end");
            }
            else
            {
                query.Append($"set @New{_tableSingleName}ID = scope_identity()");
            }

            return query.ToString();
        }

        private void _CreateAddNew_SP()
        {
            _tempText.AppendLine($"create procedure SP_AddNew{_tableSingleName}");
            _tempText.Append($"{_GetParameters(1)}");
            _tempText.AppendLine($"@New{_tableSingleName}ID int output");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"{_GetQueryForAddNew()}");
            _tempText.AppendLine("end;");

            if (!_isAdvancedMode)
                _tempText.AppendLine("go");
        }

        private string _GetQueryForUpdate()
        {
            StringBuilder query = new StringBuilder();

            query.Append($"Update {_tableName}")
                 .AppendLine()
                 .Append("set ");


            // Print the header of the columns
            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem SecondItem = listviewColumnsInfo.Items[i];

                if (SecondItem.SubItems.Count > 0)
                {
                    string ColumnName = SecondItem.SubItems[0].Text;
                    query.Append($"{ColumnName} = @{ColumnName},")
                         .AppendLine();
                }
            }

            // Remove the trailing ", " from the end of the query
            query.Remove(query.Length - 3, 3);

            query.AppendLine()
                 .Append($"where {_tableSingleName}ID = @{_tableSingleName}ID");

            return query.ToString();
        }

        private void _CreateUpdate_SP()
        {
            _tempText.AppendLine($"create procedure SP_Update{_tableSingleName}");
            _tempText.AppendLine($"{_GetParameters()}");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"{_GetQueryForUpdate()}");
            _tempText.AppendLine("end;");

            if (!_isAdvancedMode)
                _tempText.AppendLine("go");
        }

        private void _CreateDelete_SP()
        {
            _tempText.AppendLine($"create procedure SP_Delete{_tableSingleName}");
            _tempText.AppendLine($"@{_tableSingleName}ID int");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"delete {_tableName} where {_tableSingleName}ID = @{_tableSingleName}ID");
            _tempText.AppendLine("end;");

            if (!_isAdvancedMode)
                _tempText.AppendLine("go");
        }

        private void _CreateDoesExist_SP()
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

            if (!_isAdvancedMode)
                _tempText.AppendLine("go");
        }

        private void _CreateDoesExistForUsername_SP()
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

            if (!_isAdvancedMode)
                _tempText.AppendLine("go");
        }

        private void _CreateDoesExistForUsernameAndPassword_SP()
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

            if (!_isAdvancedMode)
                _tempText.AppendLine("go");
        }

        private void _CreateGetAll_SP()
        {
            _tempText.AppendLine($"create procedure SP_GetAll{_tableName}");
            _tempText.AppendLine("as");
            _tempText.AppendLine("begin");
            _tempText.AppendLine($"select * from {_tableName}");
            _tempText.AppendLine("end;");

            if (!_isAdvancedMode)
                _tempText.AppendLine("go");
        }

        private void _CreateStoredProcedures()
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

        #endregion

        #region App.config
        private void _CreateAppConfigFile(string path)
        {
            _tempText.Clear();

            _tempText.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            _tempText.AppendLine("<configuration>");
            _tempText.AppendLine();
            _tempText.AppendLine("\t<startup>");
            _tempText.AppendLine("\t\t<supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.8\" />");
            _tempText.AppendLine("\t</startup>");
            _tempText.AppendLine();
            _tempText.AppendLine("\t<appSettings>");
            _tempText.AppendLine($"\t\t<add key=\"ProjectName\" value=\"{comboDatabaseName.Text}\" />");
            _tempText.AppendLine("\t</appSettings>");
            _tempText.AppendLine();
            _tempText.AppendLine("\t<connectionStrings>");
            _tempText.AppendLine($"\t\t<add name=\"ConnectionString\" connectionString=\"Server=.;Database={comboDatabaseName.Text};Integrated Security=True;\" providerName=\"System.Data.SqlClient\" />");
            _tempText.AppendLine("\t</connectionStrings>");
            _tempText.AppendLine();
            _tempText.AppendLine("</configuration>");

            if (_isAdvancedMode)
                _WriteToFile(path, _tempText.ToString());
        }

        #endregion

        private void btnShowDateAccessLayer_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblNumberOfColumnsRecords.Text) <= 0)
            {
                MessageBox.Show("You have to select a column at least!", "Miss Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _tempText = new StringBuilder();

                if (_isAdvancedMode)
                {
                    _tempText.AppendLine($"using System;\r\nusing System.Data;\r\nusing System.Data.SqlClient;\r\n\r\nnamespace {comboDatabaseName.Text}_DataAccess\r\n{{");
                }

                txtData.Clear();

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

                if (_isAdvancedMode)
                {
                    _tempText.Append("\n}");
                }

                txtData.Text = _tempText.ToString();
            }
        }

        private void btnShowBusinessLayer_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblNumberOfColumnsRecords.Text) <= 0)
            {
                MessageBox.Show("You have to select a column at least!", "Miss Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _tempText = new StringBuilder();

                if (_isAdvancedMode)
                {
                    _tempText.AppendLine($"using {comboDatabaseName.Text}_DataAccess;\r\nusing System;\r\nusing System.Data;\r\n\r\nnamespace {comboDatabaseName.Text}_Business\r\n{{");
                }

                txtData.Clear();

                _CreateBusinessLayer();

                if (_isAdvancedMode)
                {
                    _tempText.Append("\n}");
                }

                txtData.Text = _tempText.ToString();
            }
        }

        private void btnShowStoredProcedure_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblNumberOfColumnsRecords.Text) <= 0)
            {
                MessageBox.Show("You have to select a column at least!", "Miss Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _tempText = new StringBuilder();

                txtData.Clear();

                // in case the table has only one column, so I don't create a stored procedure to it.
                if (listviewColumnsInfo.Items.Count <= 1)
                    return;

                _CreateStoredProcedures();

                if (_isAdvancedMode)
                {
                    if (clsCodeGenerator.ExecuteStoredProcedure(comboDatabaseName.Text, _tempText.ToString()))
                    {
                        if (!_generateStoredProceduresInAllTables)
                            MessageBox.Show("Stored Procedures Saved Successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Stored Procedures Saved Failed!", "Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    return;
                }

                txtData.Text = _tempText.ToString();
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtData.Text))
            {
                // Copy the text to the clipboard
                Clipboard.SetText(txtData.Text);
            }
        }

        private void listviewColumnsInfo_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null) // Check if the label was edited
            {
                // Update the item's text with the edited label
                listviewColumnsInfo.Items[e.Item].Text = e.Label;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _Reset();
        }

        private void comboDatabaseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            _FillListViewWithTablesName();

            listviewColumnsInfo.Items.Clear();

            txtData.Clear();

            lblNumberOfColumnsRecords.Text = "0";
        }

        private void listviewTablesName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listviewTablesName.SelectedItems.Count > 0)
            {
                txtData.Clear();

                // Access the first value (first column) of the selected item
                _tableName = listviewTablesName.SelectedItems[0].SubItems[0].Text;

                _FillListViewWithColumnsData();

                _isLogin = _DoesTableHaveUsernameAndPassword();
            }
        }

        private void comboDatabaseName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // To prevent typing in the combobox
            e.Handled = true;
        }

        private void comboDatabaseName_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                comboDatabaseName.DroppedDown = true;
            }
        }

        private void tcMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _isAdvancedMode = (tcMode.SelectedTab == tbAdvanced);
        }

        private void btnGenerateDataAccess_Click(object sender, EventArgs e)
        {
            if (!_IsAllDataFilled(txtDataAccessPath.Text.Trim(), "data-access classes"))
                return;

            string path = string.Empty;

            _GenerateLayer(
                (singleTableName) =>
                {
                    path = txtDataAccessPath.Text.Trim() + $"cls{singleTableName}Data.cs";
                    _GenerateAllClassesInDataAccess(path);
                }
                ,
                () => MessageBox.Show($"Classes of The Data Access Layer created and added to the file successfully.")
                          );

            // common classes for all tables
            _CreateDataAccessSettingsClass();
            _CreateClassesThatRelatedToLoggingErrors();
            _CreateDataAccessHelperClassToTheFile();

            txtDataAccessPath.Clear();
        }

        private void btnGenerateBusiness_Click(object sender, EventArgs e)
        {
            if (!_IsAllDataFilled(txtBusinessPath.Text.Trim(), "business classes"))
                return;

            string path = string.Empty;

            _GenerateLayer(
                           (singleTableName) =>
                           {
                               path = txtBusinessPath.Text.Trim() + $"cls{singleTableName}.cs";
                               _GenerateAllClassesInBusiness(path);
                           }
                           ,
                           () => MessageBox.Show($"Classes of The Business Layer created and added to the file successfully.")
                          );

            txtBusinessPath.Clear();
        }

        private void brnGenerateStoredProceduresToSelectedTable_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to generate stored procedures for this table?",
                "confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                == DialogResult.No)
                return;

            btnGenerateStoredProcedure.PerformClick();
        }

        private void btnGenerateStoredProceduresToAllTables_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to generate stored procedures for all tables?",
                "confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                == DialogResult.No)
                return;

            if (!_IsDatabaseSelected())
                return;

            _generateStoredProceduresInAllTables = true;

            _GenerateLayer(
                (singleTableName) => btnGenerateStoredProcedure.PerformClick()
                ,
                () => MessageBox.Show("Stored Procedures added Successfully.")
                          );
        }

        private void frmCodeGenerator_Load(object sender, EventArgs e)
        {
            _FillComboBoxWithDatabaseName();
        }

        private void btnGenerateAppConfig_Click(object sender, EventArgs e)
        {
            if (!_IsAllDataFilled(txtAppConfigPath.Text.Trim(), "App.config file"))
                return;

            _CreateAppConfigFile(txtAppConfigPath.Text.Trim());

            MessageBox.Show("App.config created successfully.");

            txtAppConfigPath.Clear();
        }
    }
}
