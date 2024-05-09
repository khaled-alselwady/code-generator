using Code_Generator.Extensions;
using CodeGeneratorBusiness;
using CommonLibrary;
using GenerateAppConfigFileLibrary;
using GenerateBusinessLayerLibrary;
using GenerateDataAccessLayerLibrary;
using GenerateStoredProcedureLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Code_Generator
{
    public partial class frmCodeGenerator : Form
    {
        private string _tableName = string.Empty;
        private bool _isAdvancedMode = false;

        private List<List<clsColumnInfo>> _columnsInfoForDataAccess
            = new List<List<clsColumnInfo>>();

        private List<List<clsColumnInfo>> _columnsInfoForBusiness
            = new List<List<clsColumnInfo>>();

        private List<List<clsColumnInfo>> _columnsInfoForStoredProcedure
            = new List<List<clsColumnInfo>>();

        private List<string> _tableNames = new List<string>();

        public frmCodeGenerator()
        {
            InitializeComponent();
        }

        private void _FillColumnInfoForDataAccessObjectFromListView()
        {
            _columnsInfoForDataAccess.Clear();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    var columnInfo = new List<clsColumnInfo>
                    {
                        new clsColumnInfo
                        {
                            ColumnName = firstItem.SubItems[0].Text,
                            DataType = firstItem.SubItems[1].Text.ToSqlDbType(),
                            IsNullable = firstItem.SubItems[2].Text.ToLower() == "yes"
                        }
                    };

                    _columnsInfoForDataAccess.Add(columnInfo);
                }
            }
        }

        private void _FillColumnInfoForBusinessObjectFromListView()
        {
            _columnsInfoForBusiness.Clear();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    var columnInfo = new List<clsColumnInfo>
                    {
                        new clsColumnInfo
                        {
                            ColumnName = firstItem.SubItems[0].Text,
                            DataType = firstItem.SubItems[1].Text.ToSqlDbType(),
                            IsNullable = firstItem.SubItems[2].Text.ToLower() == "yes"
                        }
                    };

                    _columnsInfoForBusiness.Add(columnInfo);
                }
            }
        }

        private void _FillColumnInfoForStoredProcedureObjectFromListView()
        {
            _columnsInfoForStoredProcedure.Clear();

            for (int i = 0; i < listviewColumnsInfo.Items.Count; i++)
            {
                ListViewItem firstItem = listviewColumnsInfo.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    var columnInfo = new List<clsColumnInfo>
                    {
                        new clsColumnInfo
                        {
                            ColumnName = firstItem.SubItems[0].Text,
                            DataType = firstItem.SubItems[1].Text.ToSqlDbType(),
                            IsNullable = firstItem.SubItems[2].Text.ToLower() == "yes",
                            MaxLength = string.IsNullOrWhiteSpace(firstItem.SubItems[3].Text) ? null : (int?)Convert.ToInt32(firstItem.SubItems[3].Text)
                        }
                    };

                    _columnsInfoForStoredProcedure.Add(columnInfo);
                }
            }
        }

        private void _FillTablesNamesObjectFromListView()
        {
            _tableNames.Clear();

            for (int i = 0; i < listviewTablesName.Items.Count; i++)
            {
                ListViewItem firstItem = listviewTablesName.Items[i];

                if (firstItem.SubItems.Count > 0)
                {
                    _tableNames.Add(firstItem.SubItems[0].Text);
                }
            }
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

            // To show the number of records
            lblNumberOfColumnsRecords.Text = listviewColumnsInfo.Items.Count.ToString();
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

        private void _GenerateHelperClasses(Func<string> generateHelperClass)
        {
            if (generateHelperClass is null)
                return;

            if (!_IsDatabaseSelected())
                return;

            txtData.Text = generateHelperClass?.Invoke();
        }

        private void btnShowDateAccessLayer_Click(object sender, EventArgs e)
        {
            if (!_isAdvancedMode &&
                MessageBox.Show(
                "Please ensure you've added all the necessary helper classes to your data access layer to prevent compilation errors. Would you like to proceed?",
                "Confirm Action",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                return;
            }

            if (int.Parse(lblNumberOfColumnsRecords.Text) <= 0)
            {
                MessageBox.Show("You have to select a column at least!", "Miss Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                txtData.Text = clsGenerateDataAccessLayer.Generate(_columnsInfoForDataAccess, comboDatabaseName.Text, _tableName);
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
                txtData.Text = clsGenerateBusinessLayer.Generate(_columnsInfoForBusiness, comboDatabaseName.Text);
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
                txtData.Text = clsGenerateStoredProcedures
                                .Generate(_columnsInfoForStoredProcedure, comboDatabaseName.Text, _tableName);
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
            _FillTablesNamesObjectFromListView();

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
                _FillColumnInfoForDataAccessObjectFromListView();
                _FillColumnInfoForBusinessObjectFromListView();
                _FillColumnInfoForStoredProcedureObjectFromListView();
            }
        }

        private void comboDatabaseName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // To prevent typing in the ComboBox
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

            clsGenerateDataAccessLayer.GenerateAllToFile(_tableNames, txtDataAccessPath.Text.Trim(), comboDatabaseName.Text);

            MessageBox.Show("Data Access Classes Added Successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGenerateBusiness_Click(object sender, EventArgs e)
        {
            if (!_IsAllDataFilled(txtBusinessPath.Text.Trim(), "business classes"))
                return;

            clsGenerateBusinessLayer.GenerateAllToFile(_tableNames, txtBusinessPath.Text.Trim(), comboDatabaseName.Text);

            MessageBox.Show("Business Classes Added Successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void brnGenerateStoredProceduresToSelectedTable_Click(object sender, EventArgs e)
        {
            if (int.Parse(lblNumberOfColumnsRecords.Text) <= 0)
            {
                MessageBox.Show("You have to select a column at least!", "Miss Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (MessageBox.Show("Are you sure you want to generate stored procedures for this table?",
                "confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                == DialogResult.No)
                return;


            if (clsGenerateStoredProcedures.GenerateForOneTableToDatabase(_columnsInfoForStoredProcedure, comboDatabaseName.Text, _tableName))
            {
                MessageBox.Show("Stored Procedures Saved Successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Stored Procedures Saved Failed!", "Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerateStoredProceduresToAllTables_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to generate stored procedures for all tables?",
                "confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                == DialogResult.No)
                return;

            if (!_IsDatabaseSelected())
                return;

            if (clsGenerateStoredProcedures.GenerateAllToDatabase(_tableNames, comboDatabaseName.Text))
            {
                MessageBox.Show("Stored Procedures Saved Successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Stored Procedures Saved Failed!", "Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmCodeGenerator_Load(object sender, EventArgs e)
        {
            _FillComboBoxWithDatabaseName();
        }

        private void btnGenerateAppConfig_Click(object sender, EventArgs e)
        {
            if (!_IsAllDataFilled(txtAppConfigPath.Text.Trim(), "App.config file"))
                return;

            clsGenerateAppConfigFile.CreateAppConfigFile(txtAppConfigPath.Text.Trim(), comboDatabaseName.Text);

            MessageBox.Show("App.config created successfully.");
        }

        private void btnGenerateDataAccessSettings_Click(object sender, EventArgs e)
        {
            _GenerateHelperClasses(() => clsGenerateHelperClasses.CreateDataAccessSettingsClass(comboDatabaseName.Text));
        }

        private void btnGenerateErrorLogger_Click(object sender, EventArgs e)
        {
            _GenerateHelperClasses(() => clsGenerateHelperClasses.CreateErrorLoggerClass(comboDatabaseName.Text));
        }

        private void GenerateLogHandler_Click(object sender, EventArgs e)
        {
            _GenerateHelperClasses(() => clsGenerateHelperClasses.CreateLogHandlerClass(comboDatabaseName.Text));
        }

        private void btnGenerateDataAccessHelper_Click(object sender, EventArgs e)
        {
            _GenerateHelperClasses(() => clsGenerateHelperClasses.CreateDataAccessHelperClass(comboDatabaseName.Text));
        }
    }
}
