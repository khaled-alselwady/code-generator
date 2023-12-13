using CodeGenerator_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Code_Generator
{
    public partial class Form1 : Form
    {
        // Table Info
        private string _TableName = string.Empty;
        private string _TableSingleName = string.Empty;
        private bool _IsLogin = false;

        private StringBuilder _TempText = new StringBuilder();

        public Form1()
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

            DataTable dt = clsCodeGenerator.GetColumnsNameWithInfo(_TableName, comboDatabaseName.Text);

            // Loop through the rows in the DataTable and add them to the ListView
            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row["Column Name"].ToString()); // Column1 represents the first column in your DataTable
                item.SubItems.Add(row["Data Type"].ToString()); // Column2 represents the second column in your DataTable
                item.SubItems.Add(row["Is Nullable"].ToString()); // Column3 represents the third column in your DataTable
                                                                  // Add more sub-items as needed for additional columns

                listviewColumnsInfo.Items.Add(item);
            }

            // To get the single column name
            if (listviewColumnsInfo.Items.Count > 0)
            {
                string FirstValue = listviewColumnsInfo.Items[0].Text;
                _TableSingleName = FirstValue.Remove(FirstValue.Length - 2);
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

        #region Data Access Layer

        private string _GetConnectionString()
        {
            return "SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString)";
        }

        private string _CreateCatchBlockWithIsFound()
        {
            return "catch (SqlException ex)\r\n{\r\n    IsFound = false;\r\n\r\n    clsLogError.LogError(\"Database Exception\", ex);\r\n}\r\ncatch (Exception ex)\r\n{\r\n    IsFound = false;\r\n\r\n    clsLogError.LogError(\"General Exception\", ex);\r\n}";
        }

        private string _CreateCatchBlockWithoutIsFound()
        {
            return "catch (SqlException ex)\r\n{\r\n    clsLogError.LogError(\"Database Exception\", ex);\r\n}\r\ncatch (Exception ex)\r\n{\r\n    clsLogError.LogError(\"General Exception\", ex);\r\n}";
        }

        private string _MakeParametersForFindMethod()
        {
            StringBuilder Parameters = new StringBuilder();

            Parameters.Append("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;
                    string DataType = firstItem.SubItems[1].Text;
                    string IsNullable = firstItem.SubItems[2].Text;

                    if (i == 0)
                    {
                        Parameters.Append(_GetDataTypeCSharp(DataType) + "? " + ColumnName + ", ");
                    }
                    else
                    {
                        if (IsNullable.ToUpper() == "YES" && !_IsDataTypeString(DataType))
                        {
                            Parameters.Append("ref " + _GetDataTypeCSharp(DataType) + "? " + ColumnName + ", ");
                        }
                        else
                        {
                            Parameters.Append("ref " + _GetDataTypeCSharp(DataType) + " " + ColumnName + ", ");
                        }
                    }
                }
            }

            // To remove the ", " from the end of the text
            Parameters.Length -= 2;

            Parameters.Append(")");

            return Parameters.ToString().Trim();
        }

        private string _FillTheVariableWithDataThatComingFromDatabase()
        {
            StringBuilder Text = new StringBuilder();

            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem SecondItem = listviewColumnsInfo.Items[i];

                if (SecondItem.SubItems.Count > 0)
                {
                    string ColumnName = SecondItem.SubItems[0].Text;
                    string DataType = SecondItem.SubItems[1].Text;
                    string IsNullable = SecondItem.SubItems[2].Text;

                    if (IsNullable.ToUpper() == "YES")
                    {
                        if (!_IsDataTypeString(DataType))
                        {
                            Text.Append($"{ColumnName} = (reader[\"{ColumnName}\"] != DBNull.Value) ? ({_GetDataTypeCSharp(DataType)}?)reader[\"{ColumnName}\"] : null;");
                        }
                        else
                        {
                            Text.Append($"{ColumnName} = (reader[\"{ColumnName}\"] != DBNull.Value) ? ({_GetDataTypeCSharp(DataType)})reader[\"{ColumnName}\"] : null;");
                        }

                        Text.AppendLine();
                    }
                    else
                    {
                        Text.AppendLine($"{ColumnName} = ({_GetDataTypeCSharp(DataType)})reader[\"{ColumnName}\"];");
                    }
                }
            }

            return Text.ToString().Trim();
        }

        private void _CreateFindMethod()
        {
            _TempText.Append($"public static bool Get{_TableSingleName}InfoByID{_MakeParametersForFindMethod()}");
            _TempText.AppendLine();
            _TempText.AppendLine("{");

            _TempText.AppendLine("    bool IsFound = false;");
            _TempText.AppendLine();

            _TempText.AppendLine("    try");
            _TempText.AppendLine("    {");
            _TempText.AppendLine($"        using ({_GetConnectionString()})");
            _TempText.AppendLine("        {");
            _TempText.AppendLine("            connection.Open();");
            _TempText.AppendLine();

            _TempText.AppendLine($"    string query = @\"select * from {_TableName} where {_TableSingleName}ID = @{_TableSingleName}ID\";");
            _TempText.AppendLine();

            _TempText.AppendLine("            using (SqlCommand command = new SqlCommand(query, connection))");
            _TempText.AppendLine("            {");
            _TempText.AppendLine($"                command.Parameters.AddWithValue(\"@{_TableSingleName}ID\", (object){_TableSingleName}ID ?? DBNull.Value);");
            _TempText.AppendLine();

            _TempText.AppendLine("                using (SqlDataReader reader = command.ExecuteReader())");
            _TempText.AppendLine("                {");
            _TempText.AppendLine("                    if (reader.Read())");
            _TempText.AppendLine("                    {");
            _TempText.AppendLine("                        // The record was found");
            _TempText.AppendLine("                        IsFound = true;");
            _TempText.AppendLine();

            _TempText.AppendLine(_FillTheVariableWithDataThatComingFromDatabase());
            _TempText.AppendLine("                    }");
            _TempText.AppendLine("                    else");
            _TempText.AppendLine("                    {");
            _TempText.AppendLine("                        // The record was not found");
            _TempText.AppendLine("                        IsFound = false;");
            _TempText.AppendLine("                    }");
            _TempText.AppendLine("                }");
            _TempText.AppendLine("            }");
            _TempText.AppendLine("        }");
            _TempText.AppendLine("    }");
            _TempText.AppendLine(_CreateCatchBlockWithIsFound());
            _TempText.AppendLine();
            _TempText.AppendLine("    return IsFound;");
            _TempText.AppendLine("}");
        }

        private string _MakeParametersForFindMethodForUsername()
        {
            StringBuilder Parameters = new StringBuilder("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;
                    string DataType = firstItem.SubItems[1].Text;
                    string IsNullable = firstItem.SubItems[2].Text;

                    if (i == 0)
                    {
                        Parameters.Append("ref ").Append(_GetDataTypeCSharp(DataType)).Append("? ").Append(ColumnName).Append(", ");
                    }
                    else
                    {
                        if (ColumnName.ToLower() == "username")
                        {
                            Parameters.Append(_GetDataTypeCSharp(DataType)).Append(" ").Append(ColumnName).Append(", ");
                        }
                        else
                        {
                            if (IsNullable.ToUpper() == "YES" && !_IsDataTypeString(DataType))
                            {
                                Parameters.Append("ref ").Append(_GetDataTypeCSharp(DataType)).Append("? ").Append(ColumnName).Append(", ");
                            }
                            else
                            {
                                Parameters.Append("ref ").Append(_GetDataTypeCSharp(DataType)).Append(" ").Append(ColumnName).Append(", ");
                            }
                        }
                    }
                }
            }

            // To remove the ", " from the end of the text
            Parameters.Length -= 2;

            Parameters.Append(")");

            return Parameters.ToString().Trim();
        }

        private string _FillTheVariableWithDataThatComingFromDatabaseForUsername()
        {
            StringBuilder Text = new StringBuilder();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem FirstValue = listviewColumnsInfo.Items[i];

                if (FirstValue.SubItems.Count > 0)
                {
                    string ColumnName = FirstValue.SubItems[0].Text;
                    string DataType = FirstValue.SubItems[1].Text;
                    string IsNullable = FirstValue.SubItems[2].Text;

                    if (ColumnName.ToLower() != "username")
                    {
                        if (i == 0)
                        {
                            Text.Append(ColumnName)
                                .Append(" = ")
                                .Append("(reader[\"")
                                .Append(ColumnName)
                                .Append("\"] != DBNull.Value) ? (")
                                .Append(_GetDataTypeCSharp(DataType))
                                .Append("?)reader[\"")
                                .Append(ColumnName)
                                .Append("\"] : null;")
                                .AppendLine();
                            continue;
                        }

                        if (IsNullable == "YES")
                        {
                            if (!_IsDataTypeString(DataType))
                            {
                                Text.Append(ColumnName)
                                    .Append(" = ")
                                    .Append("(reader[\"")
                                    .Append(ColumnName)
                                    .Append("\"] != DBNull.Value) ? (")
                                    .Append(_GetDataTypeCSharp(DataType))
                                    .Append("?)reader[\"")
                                    .Append(ColumnName)
                                    .Append("\"] : null;")
                                    .AppendLine();
                            }
                            else
                            {
                                Text.Append(ColumnName)
                                    .Append(" = ")
                                    .Append("(reader[\"")
                                    .Append(ColumnName)
                                    .Append("\"] != DBNull.Value) ? (")
                                    .Append(_GetDataTypeCSharp(DataType))
                                    .Append(")reader[\"")
                                    .Append(ColumnName)
                                    .Append("\"] : null;")
                                    .AppendLine();
                            }
                        }
                        else
                        {
                            Text.Append(ColumnName)
                                .Append(" = ")
                                .Append("(")
                                .Append(_GetDataTypeCSharp(DataType))
                                .Append(")reader[\"")
                                .Append(ColumnName)
                                .Append("\"];")
                                .AppendLine();
                        }
                    }
                }
            }

            return Text.ToString().Trim();
        }

        private void _CreateFindMethodForUsername()
        {
            _TempText.AppendLine();
            _TempText.AppendLine($"public static bool Get{_TableSingleName}InfoByUsername{_MakeParametersForFindMethodForUsername()}");
            _TempText.AppendLine("{");
            _TempText.AppendLine("    bool IsFound = false;");
            _TempText.AppendLine();

            _TempText.AppendLine("    try");
            _TempText.AppendLine("    {");
            _TempText.AppendLine($"        using ({_GetConnectionString()})");
            _TempText.AppendLine("        {");
            _TempText.AppendLine("            connection.Open();");
            _TempText.AppendLine();

            _TempText.AppendLine($"            string query = @\"select * from {_TableName} where Username = @Username COLLATE SQL_Latin1_General_CP1_CS_AS\";");
            _TempText.AppendLine();

            _TempText.AppendLine("            using (SqlCommand command = new SqlCommand(query, connection))");
            _TempText.AppendLine("            {");
            _TempText.AppendLine($"                command.Parameters.AddWithValue(\"@Username\", Username);");
            _TempText.AppendLine();

            _TempText.AppendLine("                using (SqlDataReader reader = command.ExecuteReader())");
            _TempText.AppendLine("                {");
            _TempText.AppendLine("                    if (reader.Read())");
            _TempText.AppendLine("                    {");
            _TempText.AppendLine("                        // The record was found");
            _TempText.AppendLine("                        IsFound = true;");
            _TempText.AppendLine(_FillTheVariableWithDataThatComingFromDatabaseForUsername());
            _TempText.AppendLine("                    }");
            _TempText.AppendLine("                    else");
            _TempText.AppendLine("                    {");
            _TempText.AppendLine("                        // The record was not found");
            _TempText.AppendLine("                        IsFound = false;");
            _TempText.AppendLine("                    }");
            _TempText.AppendLine("                }");
            _TempText.AppendLine("            }");
            _TempText.AppendLine("        }");
            _TempText.AppendLine("    }");
            _TempText.AppendLine(_CreateCatchBlockWithIsFound());
            _TempText.AppendLine();
            _TempText.AppendLine("    return IsFound;");
            _TempText.AppendLine("}");
        }

        private string _MakeParametersForFindMethodForUsernameAndPassword()
        {
            StringBuilder Parameters = new StringBuilder();

            Parameters.Append("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;
                    string DataType = firstItem.SubItems[1].Text;
                    string IsNullable = firstItem.SubItems[2].Text;

                    if (i == 0)
                    {
                        Parameters.Append("ref ")
                            .Append(_GetDataTypeCSharp(DataType))
                            .Append("? ")
                            .Append(ColumnName)
                            .Append(", ");
                        continue;
                    }

                    if (ColumnName.ToLower() == "username" || ColumnName.ToLower() == "password")
                    {
                        Parameters.Append(_GetDataTypeCSharp(DataType))
                            .Append(" ")
                            .Append(ColumnName)
                            .Append(", ");
                    }
                    else
                    {
                        if (IsNullable.ToUpper() == "YES" && !_IsDataTypeString(DataType))
                        {
                            Parameters.Append("ref ")
                                .Append(_GetDataTypeCSharp(DataType))
                                .Append("? ")
                                .Append(ColumnName)
                                .Append(", ");
                        }
                        else
                        {
                            Parameters.Append("ref ")
                                .Append(_GetDataTypeCSharp(DataType))
                                .Append(" ")
                                .Append(ColumnName)
                                .Append(", ");
                        }
                    }
                }
            }

            // To remove the ", " from the end of the text
            Parameters.Length -= 2;

            Parameters.Append(")");

            return Parameters.ToString().Trim();
        }

        private string _FillTheVariableWithDataThatComingFromDatabaseForUsernameAndPassword()
        {
            StringBuilder Text = new StringBuilder();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem FirstValue = listviewColumnsInfo.Items[i];

                if (FirstValue.SubItems.Count > 0)
                {
                    string ColumnName = FirstValue.SubItems[0].Text;
                    string DataType = FirstValue.SubItems[1].Text;
                    string IsNullable = FirstValue.SubItems[2].Text;

                    if (ColumnName.ToLower() != "username" && ColumnName.ToLower() != "password")
                    {
                        if (i == 0)
                        {
                            Text.Append(ColumnName)
                                .Append(" = (reader[\"")
                                .Append(ColumnName)
                                .Append("\"] != DBNull.Value) ? (")
                                .Append(_GetDataTypeCSharp(DataType))
                                .Append("?)reader[\"")
                                .Append(ColumnName)
                                .Append("\"] : null;")
                                .AppendLine();

                            continue;
                        }

                        if (IsNullable == "YES")
                        {
                            if (!_IsDataTypeString(DataType))
                            {
                                Text.Append(ColumnName)
                                    .Append(" = (reader[\"")
                                    .Append(ColumnName)
                                    .Append("\"] != DBNull.Value) ? (")
                                    .Append(_GetDataTypeCSharp(DataType))
                                    .Append("?)reader[\"")
                                    .Append(ColumnName)
                                    .Append("\"] : null;")
                                    .AppendLine();
                            }
                            else
                            {
                                Text.Append(ColumnName)
                                    .Append(" = (reader[\"")
                                    .Append(ColumnName)
                                    .Append("\"] != DBNull.Value) ? (")
                                    .Append(_GetDataTypeCSharp(DataType))
                                    .Append(")reader[\"")
                                    .Append(ColumnName)
                                    .Append("\"] : null;")
                                    .AppendLine();
                            }
                        }
                        else
                        {
                            Text.Append(ColumnName)
                                .Append(" = (")
                                .Append(_GetDataTypeCSharp(DataType))
                                .Append(")reader[\"")
                                .Append(ColumnName)
                                .Append("\"];")
                                .AppendLine();
                        }
                    }
                }
            }

            return Text.ToString().Trim();
        }

        private void _CreateFindMethodForUsernameAndPassword()
        {
            _TempText.AppendLine();
            _TempText.AppendLine($"public static bool Get{_TableSingleName}InfoByUsernameAndPassword{_MakeParametersForFindMethodForUsernameAndPassword()}");
            _TempText.AppendLine("{");
            _TempText.AppendLine("    bool IsFound = false;");
            _TempText.AppendLine();

            _TempText.AppendLine("    try");
            _TempText.AppendLine("    {");
            _TempText.AppendLine($"        using ({_GetConnectionString()})");
            _TempText.AppendLine("        {");
            _TempText.AppendLine("            connection.Open();");
            _TempText.AppendLine();

            _TempText.AppendLine($"            string query = @\"select * from {_TableName} where Username = @Username COLLATE SQL_Latin1_General_CP1_CS_AS AND Password = @Password COLLATE SQL_Latin1_General_CP1_CS_AS\";");
            _TempText.AppendLine();

            _TempText.AppendLine("            using (SqlCommand command = new SqlCommand(query, connection))");
            _TempText.AppendLine("            {");
            _TempText.AppendLine($"                command.Parameters.AddWithValue(\"@Username\", Username);");
            _TempText.AppendLine($"                command.Parameters.AddWithValue(\"@Password\", Password);");
            _TempText.AppendLine();

            _TempText.AppendLine("                using (SqlDataReader reader = command.ExecuteReader())");
            _TempText.AppendLine("                {");
            _TempText.AppendLine("                    if (reader.Read())");
            _TempText.AppendLine("                    {");
            _TempText.AppendLine("                        // The record was found");
            _TempText.AppendLine("                        IsFound = true;");
            _TempText.AppendLine(_FillTheVariableWithDataThatComingFromDatabaseForUsernameAndPassword());
            _TempText.AppendLine("                    }");
            _TempText.AppendLine("                    else");
            _TempText.AppendLine("                    {");
            _TempText.AppendLine("                        // The record was not found");
            _TempText.AppendLine("                        IsFound = false;");
            _TempText.AppendLine("                    }");
            _TempText.AppendLine("                }");
            _TempText.AppendLine("            }");
            _TempText.AppendLine("        }");
            _TempText.AppendLine("    }");
            _TempText.AppendLine(_CreateCatchBlockWithIsFound());
            _TempText.AppendLine();
            _TempText.AppendLine("    return IsFound;");
            _TempText.AppendLine("}");
        }

        private string _MakeParametersForAddNewMethod()
        {
            StringBuilder Parameters = new StringBuilder("(");

            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem SecondRow = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (SecondRow.SubItems.Count > 0)
                {
                    string ColumnName = SecondRow.SubItems[0].Text;
                    string DataType = SecondRow.SubItems[1].Text;
                    string IsNullable = SecondRow.SubItems[2].Text;

                    if (IsNullable.ToUpper() == "YES" && !_IsDataTypeString(DataType))
                    {
                        Parameters.Append(_GetDataTypeCSharp(DataType)).Append("? ").Append(ColumnName).Append(", ");
                    }
                    else
                    {
                        Parameters.Append(_GetDataTypeCSharp(DataType)).Append(" ").Append(ColumnName).Append(", ");
                    }
                }
            }

            // To remove the ", " from the end of the text
            if (Parameters.Length >= 2)
            {
                Parameters.Length -= 2;
            }

            Parameters.Append(")");

            return Parameters.ToString().Trim();
        }

        private string _GetQueryForAddNewMethod()
        {
            StringBuilder query = new StringBuilder();

            if (_IsLogin)
            {
                query.AppendLine($"string query = @\"if not Exists (select found = 1 from {_TableName} where Username = @Username)");
                query.AppendLine("begin");
                query.Append($"insert into {_TableName} (");
            }
            else
            {
                query.Append($"string query = @\"insert into {_TableName} (");
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

            if (_IsLogin)
            {
                query.AppendLine("select scope_identity()");
                query.Append("end\";");
            }
            else
            {
                query.Append("select scope_identity()\";");
            }

            return query.ToString();
        }

        private string _FillParametersInTheCommand()
        {
            StringBuilder Text = new StringBuilder();

            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem secondItem = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (secondItem.SubItems.Count > 0)
                {
                    string columnName = secondItem.SubItems[0].Text;
                    string isNullable = secondItem.SubItems[2].Text;

                    Text.Append($"command.Parameters.AddWithValue(\"@{columnName}\", ");

                    if (isNullable == "YES")
                    {
                        Text.Append($"(object){columnName} ?? DBNull.Value");
                    }
                    else
                    {
                        Text.Append(columnName);
                    }

                    Text.AppendLine(");");
                }
            }

            return Text.ToString().Trim();
        }

        private void _CreateAddNewMethod()
        {
            _TempText.AppendLine();
            _TempText.AppendLine($"public static int? AddNew{_TableSingleName}{_MakeParametersForAddNewMethod()}");
            _TempText.AppendLine("{");
            _TempText.AppendLine("// This function will return the new person id if succeeded and null if not");
            _TempText.AppendLine($"    int? {_TableSingleName}ID = null;");
            _TempText.AppendLine();

            _TempText.AppendLine("    try");
            _TempText.AppendLine("    {");
            _TempText.AppendLine($"        using ({_GetConnectionString()})");
            _TempText.AppendLine("        {");
            _TempText.AppendLine("            connection.Open();");
            _TempText.AppendLine();

            _TempText.AppendLine(_GetQueryForAddNewMethod());
            _TempText.AppendLine();

            _TempText.AppendLine("            using (SqlCommand command = new SqlCommand(query, connection))");
            _TempText.AppendLine("            {");
            _TempText.AppendLine(_FillParametersInTheCommand());
            _TempText.AppendLine();

            _TempText.AppendLine("                object result = command.ExecuteScalar();");
            _TempText.AppendLine();

            _TempText.AppendLine("                if (result != null && int.TryParse(result.ToString(), out int InsertID))");
            _TempText.AppendLine("                {");
            _TempText.AppendLine($"                    {_TableSingleName}ID = InsertID;");
            _TempText.AppendLine("                }");
            _TempText.AppendLine("            }");
            _TempText.AppendLine("        }");
            _TempText.AppendLine("    }");
            _TempText.AppendLine(_CreateCatchBlockWithoutIsFound());
            _TempText.AppendLine();
            _TempText.AppendLine($"    return {_TableSingleName}ID;");
            _TempText.AppendLine("}");
        }

        private string _MakeParametersForUpdateMethod()
        {
            StringBuilder Parameters = new StringBuilder("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem secondRow = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (secondRow.SubItems.Count > 0)
                {
                    string columnName = secondRow.SubItems[0].Text;
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

        private string _GetQueryForUpdateMethod()
        {
            StringBuilder query = new StringBuilder();

            query.Append("string query = ")
                 .Append($"@\"Update {_TableName}")
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
                 .Append($"where {_TableSingleName}ID = @{_TableSingleName}ID\";");

            return query.ToString();
        }

        private void _CreateUpdateMethod()
        {
            _TempText.AppendLine();
            _TempText.AppendLine($"public static bool Update{_TableSingleName}{_MakeParametersForUpdateMethod()}");
            _TempText.AppendLine("{");
            _TempText.AppendLine("    int RowAffected = 0;");
            _TempText.AppendLine();

            _TempText.AppendLine("    try");
            _TempText.AppendLine("    {");
            _TempText.AppendLine($"        using ({_GetConnectionString()})");
            _TempText.AppendLine("        {");
            _TempText.AppendLine("            connection.Open();");
            _TempText.AppendLine();

            _TempText.AppendLine(_GetQueryForUpdateMethod());
            _TempText.AppendLine();

            _TempText.AppendLine("            using (SqlCommand command = new SqlCommand(query, connection))");
            _TempText.AppendLine("            {");
            _TempText.AppendLine($"                command.Parameters.AddWithValue(\"@{_TableSingleName}ID\", (object){_TableSingleName}ID ?? DBNull.Value);");
            _TempText.AppendLine(_FillParametersInTheCommand());
            _TempText.AppendLine();

            _TempText.AppendLine("                RowAffected = command.ExecuteNonQuery();");
            _TempText.AppendLine("            }");
            _TempText.AppendLine("        }");
            _TempText.AppendLine("    }");
            _TempText.AppendLine(_CreateCatchBlockWithoutIsFound());
            _TempText.AppendLine();
            _TempText.AppendLine($"    return (RowAffected > 0);");
            _TempText.AppendLine("}");
        }

        private string _MakeParametersForDeleteMethod()
        {
            StringBuilder parameters = new StringBuilder("(");

            ListViewItem secondRow = listviewColumnsInfo.Items[0]; // Access the second row (index 0)

            if (secondRow.SubItems.Count > 0)
            {
                string columnName = secondRow.SubItems[0].Text;
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
            _TempText.AppendLine();
            _TempText.AppendLine($"public static bool Delete{_TableSingleName}{_MakeParametersForDeleteMethod()}");
            _TempText.AppendLine("{");
            _TempText.AppendLine("    int RowAffected = 0;");
            _TempText.AppendLine();

            _TempText.AppendLine("    try");
            _TempText.AppendLine("    {");
            _TempText.AppendLine($"        using ({_GetConnectionString()})");
            _TempText.AppendLine("        {");
            _TempText.AppendLine("            connection.Open();");
            _TempText.AppendLine();

            _TempText.AppendLine($"            string query = @\"delete {_TableName} where {_TableSingleName}ID = @{_TableSingleName}ID\";");
            _TempText.AppendLine();

            _TempText.AppendLine("            using (SqlCommand command = new SqlCommand(query, connection))");
            _TempText.AppendLine("            {");
            _TempText.AppendLine($"                command.Parameters.AddWithValue(\"@{_TableSingleName}ID\", (object){_TableSingleName}ID ?? DBNull.Value);");
            _TempText.AppendLine();

            _TempText.AppendLine("                RowAffected = command.ExecuteNonQuery();");
            _TempText.AppendLine("            }");
            _TempText.AppendLine("        }");
            _TempText.AppendLine("    }");
            _TempText.AppendLine(_CreateCatchBlockWithoutIsFound());
            _TempText.AppendLine();
            _TempText.AppendLine($"    return (RowAffected > 0);");
            _TempText.AppendLine("}");
        }

        private void _CreateDoesExistMethod()
        {
            _TempText.AppendLine();
            _TempText.AppendLine($"public static bool Does{_TableSingleName}Exist{_MakeParametersForDeleteMethod()}");
            _TempText.AppendLine("{");
            _TempText.AppendLine("    bool IsFound = false;");
            _TempText.AppendLine();

            _TempText.AppendLine("    try");
            _TempText.AppendLine("    {");
            _TempText.AppendLine($"        using ({_GetConnectionString()})");
            _TempText.AppendLine("        {");
            _TempText.AppendLine("            connection.Open();");
            _TempText.AppendLine();

            _TempText.AppendLine($"            string query = @\"select found = 1 from {_TableName} where {_TableSingleName}ID = @{_TableSingleName}ID\";");
            _TempText.AppendLine();

            _TempText.AppendLine("            using (SqlCommand command = new SqlCommand(query, connection))");
            _TempText.AppendLine("            {");
            _TempText.AppendLine($"                command.Parameters.AddWithValue(\"@{_TableSingleName}ID\", (object){_TableSingleName}ID ?? DBNull.Value);");
            _TempText.AppendLine();

            _TempText.AppendLine("                IsFound = (command.ExecuteScalar() != null);");
            _TempText.AppendLine("            }");
            _TempText.AppendLine("        }");
            _TempText.AppendLine("    }");
            _TempText.AppendLine(_CreateCatchBlockWithIsFound());
            _TempText.AppendLine();
            _TempText.AppendLine("    return IsFound;");
            _TempText.AppendLine("}");
        }

        private void _CreateDoesExistMethodForUsername()
        {
            _TempText.AppendLine();
            _TempText.AppendLine($"public static bool Does{_TableSingleName}Exist(string Username)");
            _TempText.AppendLine("{");
            _TempText.AppendLine("    bool IsFound = false;");
            _TempText.AppendLine();

            _TempText.AppendLine("    try");
            _TempText.AppendLine("    {");
            _TempText.AppendLine($"        using ({_GetConnectionString()})");
            _TempText.AppendLine("        {");
            _TempText.AppendLine("            connection.Open();");
            _TempText.AppendLine();

            _TempText.AppendLine($"            string query = @\"select found = 1 from {_TableName} where Username = @Username COLLATE SQL_Latin1_General_CP1_CS_AS\";");
            _TempText.AppendLine();

            _TempText.AppendLine("            using (SqlCommand command = new SqlCommand(query, connection))");
            _TempText.AppendLine("            {");
            _TempText.AppendLine($"                command.Parameters.AddWithValue(\"@Username\", Username);");
            _TempText.AppendLine();

            _TempText.AppendLine("                IsFound = (command.ExecuteScalar() != null);");
            _TempText.AppendLine("            }");
            _TempText.AppendLine("        }");
            _TempText.AppendLine("    }");
            _TempText.AppendLine(_CreateCatchBlockWithIsFound());
            _TempText.AppendLine();
            _TempText.AppendLine("    return IsFound;");
            _TempText.AppendLine("}");
        }

        private void _CreateDoesExistMethodForUsernameAndPassword()
        {
            _TempText.AppendLine();
            _TempText.AppendLine($"public static bool Does{_TableSingleName}Exist(string Username, string Password)");
            _TempText.AppendLine("{");
            _TempText.AppendLine("    bool IsFound = false;");
            _TempText.AppendLine();

            _TempText.AppendLine("    try");
            _TempText.AppendLine("    {");
            _TempText.AppendLine($"        using ({_GetConnectionString()})");
            _TempText.AppendLine("        {");
            _TempText.AppendLine("            connection.Open();");
            _TempText.AppendLine();

            _TempText.AppendLine($"            string query = @\"select found = 1 from {_TableName} where Username = @Username COLLATE SQL_Latin1_General_CP1_CS_AS and Password = @Password COLLATE SQL_Latin1_General_CP1_CS_AS\";");
            _TempText.AppendLine();

            _TempText.AppendLine("            using (SqlCommand command = new SqlCommand(query, connection))");
            _TempText.AppendLine("            {");
            _TempText.AppendLine($"                command.Parameters.AddWithValue(\"@Username\", Username);");
            _TempText.AppendLine($"                command.Parameters.AddWithValue(\"@Password\", Password);");
            _TempText.AppendLine();

            _TempText.AppendLine("                IsFound = (command.ExecuteScalar() != null);");
            _TempText.AppendLine("            }");
            _TempText.AppendLine("        }");
            _TempText.AppendLine("    }");
            _TempText.AppendLine(_CreateCatchBlockWithIsFound());
            _TempText.AppendLine();
            _TempText.AppendLine("    return IsFound;");
            _TempText.AppendLine("}");
        }

        private void _CreateGetAllMethod()
        {
            _TempText.AppendLine();
            _TempText.AppendLine($"public static DataTable GetAll{_TableName}()");
            _TempText.AppendLine("{");
            _TempText.AppendLine("    DataTable dt = new DataTable();");
            _TempText.AppendLine();

            _TempText.AppendLine("    try");
            _TempText.AppendLine("    {");
            _TempText.AppendLine($"        using ({_GetConnectionString()})");
            _TempText.AppendLine("        {");
            _TempText.AppendLine("            connection.Open();");
            _TempText.AppendLine();

            _TempText.AppendLine($"            string query = @\"select * from {_TableName}\";");
            _TempText.AppendLine();

            _TempText.AppendLine("            using (SqlCommand command = new SqlCommand(query, connection))");
            _TempText.AppendLine("            {");
            _TempText.AppendLine("                using (SqlDataReader reader = command.ExecuteReader())");
            _TempText.AppendLine("                {");
            _TempText.AppendLine("                    if (reader.HasRows)");
            _TempText.AppendLine("                    {");
            _TempText.AppendLine("                        dt.Load(reader);");
            _TempText.AppendLine("                    }");
            _TempText.AppendLine("                }");
            _TempText.AppendLine("            }");
            _TempText.AppendLine("        }");
            _TempText.AppendLine("    }");
            _TempText.AppendLine(_CreateCatchBlockWithoutIsFound());
            _TempText.AppendLine();
            _TempText.AppendLine("    return dt;");
            _TempText.AppendLine("}");
        }

        private void _DataAccessAsLoginInfo()
        {
            _CreateFindMethod();
            _CreateFindMethodForUsername();
            _CreateFindMethodForUsernameAndPassword();
            _CreateAddNewMethod();
            _CreateUpdateMethod();
            _CreateDeleteMethod();
            _CreateDoesExistMethod();
            _CreateDoesExistMethodForUsername();
            _CreateDoesExistMethodForUsernameAndPassword();
            _CreateGetAllMethod();
        }

        private void _DataAccessAsNormal()
        {
            _CreateFindMethod();
            _CreateAddNewMethod();
            _CreateUpdateMethod();
            _CreateDeleteMethod();
            _CreateDoesExistMethod();
            _CreateGetAllMethod();
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

            Constructor.AppendLine($"public cls{_TableSingleName}()");
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
                        Constructor.AppendLine($"    this.{ColumnName} = null;");
                    }
                    else
                    {
                        if (IsNullable.ToUpper() == "YES")
                        {
                            Constructor.AppendLine($"    this.{ColumnName} = null;");
                        }
                        else
                        {
                            switch (DataType.ToLower())
                            {
                                case "int":
                                case "bigint":
                                    Constructor.AppendLine($"    this.{ColumnName} = -1;");
                                    break;

                                case "float":
                                    Constructor.AppendLine($"    this.{ColumnName} = -1F;");
                                    break;

                                case "decimal":
                                case "money":
                                case "smallmoney":
                                    Constructor.AppendLine($"    this.{ColumnName} = -1M;");
                                    break;

                                case "tinyint":
                                    Constructor.AppendLine($"    this.{ColumnName} = 0;");
                                    break;

                                case "smallint":
                                    Constructor.AppendLine($"    this.{ColumnName} = -1;");
                                    break;

                                case "nvarchar":
                                case "varchar":
                                case "char":
                                    Constructor.AppendLine($"    this.{ColumnName} = string.Empty;");
                                    break;

                                case "datetime":
                                case "date":
                                case "smalldatetime":
                                case "datetime2":
                                    Constructor.AppendLine($"    this.{ColumnName} = DateTime.Now;");
                                    break;

                                case "time":
                                    Constructor.AppendLine($"    this.{ColumnName} = DateTime.Now.TimeOfDay;");
                                    break;

                                case "bit":
                                    Constructor.AppendLine($"    this.{ColumnName} = false;");
                                    break;
                            }
                        }
                    }
                }
            }

            Constructor.AppendLine("    Mode = enMode.AddNew;");
            Constructor.AppendLine("}");

            return Constructor.ToString().Trim();
        }

        private string _GetPrivateConstructor()
        {
            StringBuilder Constructor = new StringBuilder();

            Constructor.AppendLine($"private cls{_TableSingleName}{_MakeParametersForUpdateMethod()}");
            Constructor.AppendLine("{");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;
                    string DataType = firstItem.SubItems[1].Text;
                    string IsNullable = firstItem.SubItems[2].Text;

                    Constructor.AppendLine($"    this.{ColumnName} = {ColumnName};");
                }
            }

            Constructor.AppendLine("    Mode = enMode.Update;");
            Constructor.AppendLine("}");

            return Constructor.ToString().Trim();
        }

        private string _MakeParametersForAddNewMethodInBusinessLayer()
        {
            StringBuilder Parameters = new StringBuilder("(");

            for (int i = 1; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem SecondRow = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (SecondRow.SubItems.Count > 0)
                {
                    string ColumnName = SecondRow.SubItems[0].Text;

                    Parameters.Append($"this.{ColumnName}, ");
                }
            }

            // To remove the ", " from the end of the text
            if (Parameters.Length >= 2)
            {
                Parameters.Remove(Parameters.Length - 2, 2);
            }

            Parameters.Append(");");

            return Parameters.ToString().Trim();
        }

        private string _GetAddNewInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"private bool _AddNew{_TableSingleName}()");
            Text.AppendLine("{");
            Text.AppendLine($"    this.{_TableSingleName}ID = cls{_TableSingleName}Data.AddNew{_TableSingleName}{_MakeParametersForAddNewMethodInBusinessLayer()}");
            Text.AppendLine();
            Text.AppendLine($"    return (this.{_TableSingleName}ID.HasValue);");
            Text.AppendLine("}");

            return Text.ToString().Trim();
        }

        private string _MakeParametersForUpdateMethodInBusinessLayer()
        {
            StringBuilder Parameters = new StringBuilder("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem FirstRow = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (FirstRow.SubItems.Count > 0)
                {
                    string ColumnName = FirstRow.SubItems[0].Text;
                    Parameters.Append($"this.{ColumnName}, ");
                }
            }

            // To remove the ", " from the end of the text
            Parameters.Remove(Parameters.Length - 2, 2);
            Parameters.Append(");");

            return Parameters.ToString().Trim();
        }

        private string _GetUpdateInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"private bool _Update{_TableSingleName}()");
            Text.AppendLine("{");
            Text.AppendLine($"return cls{_TableSingleName}Data.Update{_TableSingleName}{_MakeParametersForUpdateMethodInBusinessLayer()}");
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
            Text.AppendLine($"if (_AddNew{_TableSingleName}())");
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
            Text.AppendLine($"return _Update{_TableSingleName}();");
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
                    string ColumnName = SecondRow.SubItems[0].Text;
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
                    string ColumnName = firstItem.SubItems[0].Text;

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
            StringBuilder Parameters = new StringBuilder();

            Parameters.Append($"return new cls{_TableSingleName}(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;

                    Parameters.Append($"{ColumnName}, ");
                }
            }

            // To remove the ", " from the end of the text
            Parameters.Remove(Parameters.Length - 2, 2);
            Parameters.AppendLine(");").AppendLine();

            return Parameters.ToString().Trim();
        }

        private string _GetFindMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static cls{_TableSingleName} Find{_MakeParametersForDeleteMethod()}");
            Text.AppendLine("{");
            Text.AppendLine(_MakeInitialParametersForFindMethodInBusinessLayer());
            Text.AppendLine();

            Text.AppendLine($"bool IsFound = cls{_TableSingleName}Data.Get{_TableSingleName}InfoByID{_MakeParametersForFindMethodInBusinessLayer()}");
            Text.AppendLine();

            Text.AppendLine("if (IsFound)");
            Text.AppendLine("{");
            Text.AppendLine(_MakeReturnParametersForFindMethodInBusinessLayer());
            Text.AppendLine("}");
            Text.AppendLine("else");
            Text.AppendLine("{");
            Text.AppendLine("return null;");
            Text.AppendLine("}");
            Text.AppendLine("}");

            return Text.ToString().Trim();
        }

        private string _GetDeleteMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static bool Delete{_TableSingleName}{_MakeParametersForDeleteMethod()}");
            Text.AppendLine("{");
            Text.AppendLine($"return cls{_TableSingleName}Data.Delete{_TableSingleName}({_TableSingleName}ID);");
            Text.AppendLine("}");

            return Text.ToString().Trim();
        }

        private string _GetDoesExistMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static bool Does{_TableSingleName}Exist{_MakeParametersForDeleteMethod()}");
            Text.AppendLine("{");
            Text.AppendLine($"return cls{_TableSingleName}Data.Does{_TableSingleName}Exist({_TableSingleName}ID);");
            Text.AppendLine("}");

            return Text.ToString().Trim();
        }

        private string _GetAllMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static DataTable GetAll{_TableName}()");
            Text.AppendLine("{");
            Text.AppendLine($"return cls{_TableSingleName}Data.GetAll{_TableName}();");
            Text.AppendLine("}");

            return Text.ToString().Trim();
        }

        private string _MakeInitialParametersForFindUsernameMethodInBusinessLayer()
        {
            StringBuilder Variable = new StringBuilder();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem FirstRow = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (FirstRow.SubItems.Count > 0)
                {
                    string ColumnName = FirstRow.SubItems[0].Text;
                    string DataType = FirstRow.SubItems[1].Text;
                    string IsNullable = FirstRow.SubItems[2].Text;

                    if (ColumnName.ToLower() != "username")
                    {
                        if (i == 0)
                        {
                            Variable.Append(_GetDataTypeCSharp(DataType) + "? " + ColumnName + " = null;").AppendLine();
                            continue;
                        }

                        if (IsNullable.ToUpper() == "YES")
                        {
                            if (!_IsDataTypeString(DataType))
                            {
                                Variable.Append(_GetDataTypeCSharp(DataType) + "? " + ColumnName + " = null;").AppendLine();
                            }
                            else
                            {
                                Variable.Append(_GetDataTypeCSharp(DataType) + " " + ColumnName + " = null;").AppendLine();
                            }
                        }
                        else
                        {
                            switch (DataType.ToLower())
                            {
                                case "int":
                                case "bigint":
                                    Variable.Append(_GetDataTypeCSharp(DataType) + " " + ColumnName + " = -1;").AppendLine();
                                    break;

                                case "float":
                                    Variable.Append(_GetDataTypeCSharp(DataType) + " " + ColumnName + " = -1F;").AppendLine();
                                    break;

                                case "decimal":
                                case "money":
                                case "smallmoney":
                                    Variable.Append(_GetDataTypeCSharp(DataType) + " " + ColumnName + " = -1M;").AppendLine();
                                    break;

                                case "tinyint":
                                    Variable.Append(_GetDataTypeCSharp(DataType) + " " + ColumnName + " = 0;").AppendLine();
                                    break;

                                case "smallint":
                                    Variable.Append(_GetDataTypeCSharp(DataType) + " " + ColumnName + " = -1;").AppendLine();
                                    break;

                                case "nvarchar":
                                case "varchar":
                                case "char":
                                    Variable.Append(_GetDataTypeCSharp(DataType) + " " + ColumnName + " = string.Empty;").AppendLine();
                                    break;

                                case "datetime":
                                case "date":
                                case "smalldatetime":
                                case "datetime2":
                                    Variable.Append(_GetDataTypeCSharp(DataType) + " " + ColumnName + " = DateTime.Now;").AppendLine();
                                    break;

                                case "time":
                                    Variable.Append(_GetDataTypeCSharp(DataType) + " " + ColumnName + " = DateTime.Now.TimeOfDay;").AppendLine();
                                    break;

                                case "bit":
                                    Variable.Append(_GetDataTypeCSharp(DataType) + " " + ColumnName + " = false;").AppendLine();
                                    break;
                            }
                        }
                    }
                }
            }

            return Variable.ToString().Trim();
        }

        private string _MakeParametersForFindUsernameMethodInBusinessLayer()
        {
            StringBuilder Parameters = new StringBuilder("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;

                    if (ColumnName.ToLower() == "username")
                    {
                        Parameters.Append(ColumnName + ", ");
                    }
                    else
                    {
                        Parameters.Append("ref " + ColumnName + ", ");
                    }
                }
            }

            // To remove the ", " from the end of the text
            Parameters.Remove(Parameters.Length - 2, 2);
            Parameters.Append(");").AppendLine();

            return Parameters.ToString().Trim();
        }

        private string _GetFindUsernameMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static cls{_TableSingleName} Find(string Username)")
                .AppendLine("{")
                .AppendLine(_MakeInitialParametersForFindUsernameMethodInBusinessLayer()).AppendLine()
                .AppendLine($"    bool IsFound = cls{_TableSingleName}Data.Get{_TableSingleName}InfoByUsername{_MakeParametersForFindUsernameMethodInBusinessLayer()}").AppendLine();

            Text.AppendLine("    if (IsFound)").AppendLine("    {").AppendLine()
                .AppendLine(_MakeReturnParametersForFindMethodInBusinessLayer()).AppendLine("    }").AppendLine()
                .AppendLine("    else").AppendLine("    {").AppendLine()
                .AppendLine("        return null;").AppendLine("    }").AppendLine()
                .AppendLine("}");

            return Text.ToString().Trim();
        }

        private string _MakeInitialParametersForFindUsernameAndPasswordMethodInBusinessLayer()
        {
            StringBuilder Variable = new StringBuilder();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem FirstRow = listviewColumnsInfo.Items[i]; // Access the second row (index 0)

                if (FirstRow.SubItems.Count > 0)
                {
                    string ColumnName = FirstRow.SubItems[0].Text;
                    string DataType = FirstRow.SubItems[1].Text;
                    string IsNullable = FirstRow.SubItems[2].Text;

                    if (ColumnName.ToLower() != "username" && ColumnName.ToLower() != "password")
                    {
                        if (i == 0)
                        {
                            Variable.AppendLine($"{_GetDataTypeCSharp(DataType)}? {ColumnName} = null;");
                            continue;
                        }

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
                                case "char":
                                    Variable.AppendLine($"{_GetDataTypeCSharp(DataType)} {ColumnName} = string.Empty;");
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
            }

            return Variable.ToString().Trim();
        }

        private string _MakeParametersForFindUsernameAndPasswordMethodInBusinessLayer()
        {
            StringBuilder Parameters = new StringBuilder("(");

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i]; // Access the first row (index 0)

                if (firstItem.SubItems.Count > 0)
                {
                    string ColumnName = firstItem.SubItems[0].Text;

                    if (ColumnName.ToLower() == "username" || ColumnName.ToLower() == "password")
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
            Parameters.AppendLine(");");

            return Parameters.ToString().Trim();
        }

        private string _GetFindUsernameAndPasswordMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static cls{_TableSingleName} Find(string Username, string Password)")
                .AppendLine("{")
                .AppendLine(_MakeInitialParametersForFindUsernameAndPasswordMethodInBusinessLayer());

            Text.AppendLine($"    bool IsFound = cls{_TableSingleName}Data.Get{_TableSingleName}InfoByUsernameAndPassword{_MakeParametersForFindUsernameAndPasswordMethodInBusinessLayer()}");

            Text.AppendLine("    if (IsFound)")
                .AppendLine("    {")
                .AppendLine(_MakeReturnParametersForFindMethodInBusinessLayer())
                .AppendLine("    }")
                .AppendLine("    else")
                .AppendLine("    {")
                .AppendLine("        return null;")
                .AppendLine("    }")
                .AppendLine("}");

            return Text.ToString().Trim();
        }

        private string _GetDoesUsernameExistMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static bool Does{_TableSingleName}Exist(string Username)")
                .AppendLine("{")
                .AppendLine($"    return cls{_TableSingleName}Data.Does{_TableSingleName}Exist(Username);")
                .AppendLine("}");

            return Text.ToString().Trim();
        }

        private string _GetDoesUsernameAndPasswordExistMethodInBusinessLayer()
        {
            StringBuilder Text = new StringBuilder();

            Text.AppendLine($"public static bool Does{_TableSingleName}Exist(string Username, string Password)")
                .AppendLine("{")
                .AppendLine($"    return cls{_TableSingleName}Data.Does{_TableSingleName}Exist(Username, Password);")
                .AppendLine("}");

            return Text.ToString().Trim();
        }

        private void _CreateBusinessLayer()
        {
            _TempText.AppendLine($"public class cls{_TableSingleName}");
            _TempText.AppendLine("{");
            _TempText.AppendLine("public enum enMode { AddNew = 0, Update = 1 };");
            _TempText.AppendLine("public enMode Mode = enMode.AddNew;").AppendLine();
            _TempText.AppendLine(_MakeParametersForBusinessLayer()).AppendLine();
            _TempText.AppendLine(_GetPublicConstructor()).AppendLine();
            _TempText.AppendLine(_GetPrivateConstructor()).AppendLine();
            _TempText.AppendLine(_GetAddNewInBusinessLayer()).AppendLine();
            _TempText.AppendLine(_GetUpdateInBusinessLayer()).AppendLine();
            _TempText.AppendLine(_GetSaveMethod()).AppendLine();
            _TempText.AppendLine(_GetFindMethodInBusinessLayer()).AppendLine();

            if (_IsLogin)
            {
                _TempText.AppendLine(_GetFindUsernameMethodInBusinessLayer()).AppendLine();
                _TempText.AppendLine(_GetFindUsernameAndPasswordMethodInBusinessLayer()).AppendLine();
            }

            _TempText.AppendLine(_GetDeleteMethodInBusinessLayer()).AppendLine();
            _TempText.AppendLine(_GetDoesExistMethodInBusinessLayer()).AppendLine();

            if (_IsLogin)
            {
                _TempText.AppendLine(_GetDoesUsernameExistMethodInBusinessLayer()).AppendLine();
                _TempText.AppendLine(_GetDoesUsernameAndPasswordExistMethodInBusinessLayer()).AppendLine();
            }

            _TempText.AppendLine(_GetAllMethodInBusinessLayer()).AppendLine();
            _TempText.AppendLine("}");
        }

        #endregion

        private void btnShowDateAccessLayer_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblNumberOfColumnsRecords.Text) <= 0)
            {
                MessageBox.Show("You have to add the row at least!", "Miss Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _TempText = new StringBuilder();
                txtData.Clear();

                _TempText.Append($"public class cls{_TableSingleName}Data");
                _TempText.AppendLine();
                _TempText.AppendLine("{");

                if (_IsLogin)
                {
                    _DataAccessAsLoginInfo();
                }
                else
                {
                    _DataAccessAsNormal();
                }

                _TempText.Append("}");

                txtData.Text = _TempText.ToString();
            }
        }

        private void btnShowBusinessLayer_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblNumberOfColumnsRecords.Text) <= 0)
            {
                MessageBox.Show("You have to add the row at least!", "Miss Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _TempText = new StringBuilder();
                txtData.Clear();

                _CreateBusinessLayer();

                txtData.Text = _TempText.ToString();
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
            listviewColumnsInfo.Items.Clear();

            listviewTablesName.Items.Clear();

            txtData.Clear();

            lblNumberOfColumnsRecords.Text = "0";

            lblNumberOfTablesRecords.Text = "0";

            comboDatabaseName.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _FillComboBoxWithDatabaseName();
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
                _TableName = listviewTablesName.SelectedItems[0].SubItems[0].Text;

                _FillListViewWithColumnsData();

                _IsLogin = _DoesTableHaveUsernameAndPassword();
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
    }
}
