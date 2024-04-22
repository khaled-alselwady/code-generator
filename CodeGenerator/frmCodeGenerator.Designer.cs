namespace Code_Generator
{
    partial class frmCodeGenerator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.listviewTablesName = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblNumberOfTablesRecords = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tcMode = new Guna.UI2.WinForms.Guna2TabControl();
            this.tbNormal = new System.Windows.Forms.TabPage();
            this.btnGenerateStoredProcedure = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnCopy = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnGenerateDateAccessLayer = new Guna.UI2.WinForms.Guna2GradientButton();
            this.txtData = new System.Windows.Forms.TextBox();
            this.lblNumberOfColumnsRecords = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.listviewColumnsInfo = new System.Windows.Forms.ListView();
            this.listColumnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listDataType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listAllowNull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listMaxLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbAdvanced = new System.Windows.Forms.TabPage();
            this.guna2GroupBox4 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.txtAppConfigPath = new System.Windows.Forms.TextBox();
            this.btnGenerateAppConfig = new Guna.UI2.WinForms.Guna2GradientButton();
            this.label7 = new System.Windows.Forms.Label();
            this.guna2GroupBox3 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.btnGenerateStoredProceduresToAllTables = new Guna.UI2.WinForms.Guna2GradientButton();
            this.brnGenerateStoredProceduresToSelectedTable = new Guna.UI2.WinForms.Guna2GradientButton();
            this.guna2GroupBox2 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.txtBusinessPath = new System.Windows.Forms.TextBox();
            this.btnGenerateBusiness = new Guna.UI2.WinForms.Guna2GradientButton();
            this.label4 = new System.Windows.Forms.Label();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.txtDataAccessPath = new System.Windows.Forms.TextBox();
            this.btnGenerateDataAccess = new Guna.UI2.WinForms.Guna2GradientButton();
            this.label2 = new System.Windows.Forms.Label();
            this.comboDatabaseName = new Guna.UI2.WinForms.Guna2ComboBox();
            this.guna2GroupBox5 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.btnGenerateBusinessLayer = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnGenerateDataAccessSettings = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnGenerateErrorLogger = new Guna.UI2.WinForms.Guna2GradientButton();
            this.GenerateLogHandler = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnGenerateDataAccessHelper = new Guna.UI2.WinForms.Guna2GradientButton();
            this.tcMode.SuspendLayout();
            this.tbNormal.SuspendLayout();
            this.tbAdvanced.SuspendLayout();
            this.guna2GroupBox4.SuspendLayout();
            this.guna2GroupBox3.SuspendLayout();
            this.guna2GroupBox2.SuspendLayout();
            this.guna2GroupBox1.SuspendLayout();
            this.guna2GroupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 39.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MediumBlue;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1582, 58);
            this.label1.TabIndex = 0;
            this.label1.Text = "Code Generator";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.BackColor = System.Drawing.SystemColors.WindowText;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnReset.Location = new System.Drawing.Point(2, 894);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 39);
            this.btnReset.TabIndex = 13;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(8, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Database Name:";
            // 
            // listviewTablesName
            // 
            this.listviewTablesName.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listviewTablesName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listviewTablesName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listviewTablesName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listviewTablesName.FullRowSelect = true;
            this.listviewTablesName.GridLines = true;
            this.listviewTablesName.HideSelection = false;
            this.listviewTablesName.LabelEdit = true;
            this.listviewTablesName.Location = new System.Drawing.Point(82, 132);
            this.listviewTablesName.Name = "listviewTablesName";
            this.listviewTablesName.Size = new System.Drawing.Size(231, 600);
            this.listviewTablesName.TabIndex = 15;
            this.listviewTablesName.UseCompatibleStateImageBehavior = false;
            this.listviewTablesName.View = System.Windows.Forms.View.Details;
            this.listviewTablesName.SelectedIndexChanged += new System.EventHandler(this.listviewTablesName_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Table Name";
            this.columnHeader1.Width = 225;
            // 
            // lblNumberOfTablesRecords
            // 
            this.lblNumberOfTablesRecords.AutoSize = true;
            this.lblNumberOfTablesRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumberOfTablesRecords.ForeColor = System.Drawing.Color.Fuchsia;
            this.lblNumberOfTablesRecords.Location = new System.Drawing.Point(233, 735);
            this.lblNumberOfTablesRecords.Name = "lblNumberOfTablesRecords";
            this.lblNumberOfTablesRecords.Size = new System.Drawing.Size(20, 24);
            this.lblNumberOfTablesRecords.TabIndex = 17;
            this.lblNumberOfTablesRecords.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(78, 736);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(158, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "# Of Tables Records:";
            // 
            // tcMode
            // 
            this.tcMode.Controls.Add(this.tbNormal);
            this.tcMode.Controls.Add(this.tbAdvanced);
            this.tcMode.ItemSize = new System.Drawing.Size(180, 40);
            this.tcMode.Location = new System.Drawing.Point(383, 88);
            this.tcMode.Name = "tcMode";
            this.tcMode.SelectedIndex = 0;
            this.tcMode.Size = new System.Drawing.Size(1190, 845);
            this.tcMode.TabButtonHoverState.BorderColor = System.Drawing.Color.Empty;
            this.tcMode.TabButtonHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.tcMode.TabButtonHoverState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tcMode.TabButtonHoverState.ForeColor = System.Drawing.Color.White;
            this.tcMode.TabButtonHoverState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.tcMode.TabButtonIdleState.BorderColor = System.Drawing.Color.Transparent;
            this.tcMode.TabButtonIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.tcMode.TabButtonIdleState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tcMode.TabButtonIdleState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.tcMode.TabButtonIdleState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.tcMode.TabButtonSelectedState.BorderColor = System.Drawing.Color.Empty;
            this.tcMode.TabButtonSelectedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(37)))), ((int)(((byte)(49)))));
            this.tcMode.TabButtonSelectedState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tcMode.TabButtonSelectedState.ForeColor = System.Drawing.Color.White;
            this.tcMode.TabButtonSelectedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
            this.tcMode.TabButtonSize = new System.Drawing.Size(180, 40);
            this.tcMode.TabIndex = 26;
            this.tcMode.TabMenuBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.tcMode.TabMenuOrientation = Guna.UI2.WinForms.TabMenuOrientation.HorizontalTop;
            this.tcMode.SelectedIndexChanged += new System.EventHandler(this.tcMode_SelectedIndexChanged);
            // 
            // tbNormal
            // 
            this.tbNormal.BackColor = System.Drawing.Color.White;
            this.tbNormal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbNormal.Controls.Add(this.guna2GroupBox5);
            this.tbNormal.Controls.Add(this.btnGenerateStoredProcedure);
            this.tbNormal.Controls.Add(this.btnCopy);
            this.tbNormal.Controls.Add(this.btnGenerateDateAccessLayer);
            this.tbNormal.Controls.Add(this.btnGenerateBusinessLayer);
            this.tbNormal.Controls.Add(this.txtData);
            this.tbNormal.Controls.Add(this.lblNumberOfColumnsRecords);
            this.tbNormal.Controls.Add(this.label6);
            this.tbNormal.Controls.Add(this.listviewColumnsInfo);
            this.tbNormal.Location = new System.Drawing.Point(4, 44);
            this.tbNormal.Name = "tbNormal";
            this.tbNormal.Padding = new System.Windows.Forms.Padding(3);
            this.tbNormal.Size = new System.Drawing.Size(1182, 797);
            this.tbNormal.TabIndex = 0;
            this.tbNormal.Text = "Normal";
            // 
            // btnGenerateStoredProcedure
            // 
            this.btnGenerateStoredProcedure.Animated = true;
            this.btnGenerateStoredProcedure.BorderRadius = 22;
            this.btnGenerateStoredProcedure.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateStoredProcedure.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateStoredProcedure.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateStoredProcedure.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateStoredProcedure.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerateStoredProcedure.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateStoredProcedure.ForeColor = System.Drawing.Color.White;
            this.btnGenerateStoredProcedure.Location = new System.Drawing.Point(858, 684);
            this.btnGenerateStoredProcedure.Name = "btnGenerateStoredProcedure";
            this.btnGenerateStoredProcedure.Size = new System.Drawing.Size(304, 45);
            this.btnGenerateStoredProcedure.TabIndex = 33;
            this.btnGenerateStoredProcedure.Text = "Generate Stored Procedure";
            this.btnGenerateStoredProcedure.Click += new System.EventHandler(this.btnShowStoredProcedure_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Animated = true;
            this.btnCopy.BorderRadius = 22;
            this.btnCopy.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCopy.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCopy.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCopy.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCopy.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCopy.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.ForeColor = System.Drawing.Color.White;
            this.btnCopy.Location = new System.Drawing.Point(1069, 748);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(109, 45);
            this.btnCopy.TabIndex = 32;
            this.btnCopy.Text = "Copy";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnGenerateDateAccessLayer
            // 
            this.btnGenerateDateAccessLayer.Animated = true;
            this.btnGenerateDateAccessLayer.BorderRadius = 22;
            this.btnGenerateDateAccessLayer.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateDateAccessLayer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateDateAccessLayer.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateDateAccessLayer.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateDateAccessLayer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerateDateAccessLayer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnGenerateDateAccessLayer.ForeColor = System.Drawing.Color.White;
            this.btnGenerateDateAccessLayer.Location = new System.Drawing.Point(858, 582);
            this.btnGenerateDateAccessLayer.Name = "btnGenerateDateAccessLayer";
            this.btnGenerateDateAccessLayer.Size = new System.Drawing.Size(304, 45);
            this.btnGenerateDateAccessLayer.TabIndex = 31;
            this.btnGenerateDateAccessLayer.Text = "Generate Data Access Layer";
            this.btnGenerateDateAccessLayer.Click += new System.EventHandler(this.btnShowDateAccessLayer_Click);
            // 
            // txtData
            // 
            this.txtData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtData.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtData.Location = new System.Drawing.Point(500, 14);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.ReadOnly = true;
            this.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtData.Size = new System.Drawing.Size(643, 502);
            this.txtData.TabIndex = 29;
            // 
            // lblNumberOfColumnsRecords
            // 
            this.lblNumberOfColumnsRecords.AutoSize = true;
            this.lblNumberOfColumnsRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumberOfColumnsRecords.ForeColor = System.Drawing.Color.Fuchsia;
            this.lblNumberOfColumnsRecords.Location = new System.Drawing.Point(180, 521);
            this.lblNumberOfColumnsRecords.Name = "lblNumberOfColumnsRecords";
            this.lblNumberOfColumnsRecords.Size = new System.Drawing.Size(20, 24);
            this.lblNumberOfColumnsRecords.TabIndex = 28;
            this.lblNumberOfColumnsRecords.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 522);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(173, 20);
            this.label6.TabIndex = 27;
            this.label6.Text = "# Of Columns Records:";
            // 
            // listviewColumnsInfo
            // 
            this.listviewColumnsInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listviewColumnsInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.listColumnName,
            this.listDataType,
            this.listAllowNull,
            this.listMaxLength});
            this.listviewColumnsInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listviewColumnsInfo.FullRowSelect = true;
            this.listviewColumnsInfo.GridLines = true;
            this.listviewColumnsInfo.HideSelection = false;
            this.listviewColumnsInfo.LabelEdit = true;
            this.listviewColumnsInfo.Location = new System.Drawing.Point(10, 14);
            this.listviewColumnsInfo.Name = "listviewColumnsInfo";
            this.listviewColumnsInfo.Size = new System.Drawing.Size(474, 502);
            this.listviewColumnsInfo.TabIndex = 26;
            this.listviewColumnsInfo.UseCompatibleStateImageBehavior = false;
            this.listviewColumnsInfo.View = System.Windows.Forms.View.Details;
            this.listviewColumnsInfo.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listviewColumnsInfo_AfterLabelEdit);
            // 
            // listColumnName
            // 
            this.listColumnName.Text = "Column Name";
            this.listColumnName.Width = 220;
            // 
            // listDataType
            // 
            this.listDataType.Text = "Data Type";
            this.listDataType.Width = 140;
            // 
            // listAllowNull
            // 
            this.listAllowNull.Text = "Allow Null";
            this.listAllowNull.Width = 110;
            // 
            // listMaxLength
            // 
            this.listMaxLength.Text = "Max Length";
            this.listMaxLength.Width = 1;
            // 
            // tbAdvanced
            // 
            this.tbAdvanced.AutoScroll = true;
            this.tbAdvanced.BackColor = System.Drawing.Color.White;
            this.tbAdvanced.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbAdvanced.Controls.Add(this.guna2GroupBox4);
            this.tbAdvanced.Controls.Add(this.guna2GroupBox3);
            this.tbAdvanced.Controls.Add(this.guna2GroupBox2);
            this.tbAdvanced.Controls.Add(this.guna2GroupBox1);
            this.tbAdvanced.Location = new System.Drawing.Point(4, 44);
            this.tbAdvanced.Name = "tbAdvanced";
            this.tbAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tbAdvanced.Size = new System.Drawing.Size(1182, 797);
            this.tbAdvanced.TabIndex = 1;
            this.tbAdvanced.Text = "Advanced";
            // 
            // guna2GroupBox4
            // 
            this.guna2GroupBox4.BorderColor = System.Drawing.Color.Gainsboro;
            this.guna2GroupBox4.Controls.Add(this.txtAppConfigPath);
            this.guna2GroupBox4.Controls.Add(this.btnGenerateAppConfig);
            this.guna2GroupBox4.Controls.Add(this.label7);
            this.guna2GroupBox4.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox4.ForeColor = System.Drawing.Color.Black;
            this.guna2GroupBox4.Location = new System.Drawing.Point(6, 415);
            this.guna2GroupBox4.Name = "guna2GroupBox4";
            this.guna2GroupBox4.Size = new System.Drawing.Size(1164, 185);
            this.guna2GroupBox4.TabIndex = 28;
            this.guna2GroupBox4.Text = "App.config";
            // 
            // txtAppConfigPath
            // 
            this.txtAppConfigPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAppConfigPath.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAppConfigPath.Location = new System.Drawing.Point(55, 54);
            this.txtAppConfigPath.Multiline = true;
            this.txtAppConfigPath.Name = "txtAppConfigPath";
            this.txtAppConfigPath.Size = new System.Drawing.Size(1096, 71);
            this.txtAppConfigPath.TabIndex = 24;
            // 
            // btnGenerateAppConfig
            // 
            this.btnGenerateAppConfig.Animated = true;
            this.btnGenerateAppConfig.BorderRadius = 22;
            this.btnGenerateAppConfig.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateAppConfig.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateAppConfig.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateAppConfig.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateAppConfig.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerateAppConfig.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnGenerateAppConfig.ForeColor = System.Drawing.Color.White;
            this.btnGenerateAppConfig.Location = new System.Drawing.Point(967, 135);
            this.btnGenerateAppConfig.Name = "btnGenerateAppConfig";
            this.btnGenerateAppConfig.Size = new System.Drawing.Size(188, 43);
            this.btnGenerateAppConfig.TabIndex = 25;
            this.btnGenerateAppConfig.Text = "Generate";
            this.btnGenerateAppConfig.Click += new System.EventHandler(this.btnGenerateAppConfig_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(3, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 21);
            this.label7.TabIndex = 23;
            this.label7.Text = "Path:";
            // 
            // guna2GroupBox3
            // 
            this.guna2GroupBox3.BorderColor = System.Drawing.Color.Gainsboro;
            this.guna2GroupBox3.Controls.Add(this.btnGenerateStoredProceduresToAllTables);
            this.guna2GroupBox3.Controls.Add(this.brnGenerateStoredProceduresToSelectedTable);
            this.guna2GroupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox3.ForeColor = System.Drawing.Color.Black;
            this.guna2GroupBox3.Location = new System.Drawing.Point(6, 626);
            this.guna2GroupBox3.Name = "guna2GroupBox3";
            this.guna2GroupBox3.Size = new System.Drawing.Size(1164, 126);
            this.guna2GroupBox3.TabIndex = 28;
            this.guna2GroupBox3.Text = "Stored Procedures";
            // 
            // btnGenerateStoredProceduresToAllTables
            // 
            this.btnGenerateStoredProceduresToAllTables.Animated = true;
            this.btnGenerateStoredProceduresToAllTables.BorderRadius = 22;
            this.btnGenerateStoredProceduresToAllTables.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateStoredProceduresToAllTables.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateStoredProceduresToAllTables.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateStoredProceduresToAllTables.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateStoredProceduresToAllTables.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerateStoredProceduresToAllTables.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnGenerateStoredProceduresToAllTables.ForeColor = System.Drawing.Color.White;
            this.btnGenerateStoredProceduresToAllTables.Location = new System.Drawing.Point(688, 71);
            this.btnGenerateStoredProceduresToAllTables.Name = "btnGenerateStoredProceduresToAllTables";
            this.btnGenerateStoredProceduresToAllTables.Size = new System.Drawing.Size(375, 43);
            this.btnGenerateStoredProceduresToAllTables.TabIndex = 26;
            this.btnGenerateStoredProceduresToAllTables.Text = "Generate To All Tables";
            this.btnGenerateStoredProceduresToAllTables.Click += new System.EventHandler(this.btnGenerateStoredProceduresToAllTables_Click);
            // 
            // brnGenerateStoredProceduresToSelectedTable
            // 
            this.brnGenerateStoredProceduresToSelectedTable.Animated = true;
            this.brnGenerateStoredProceduresToSelectedTable.BorderRadius = 22;
            this.brnGenerateStoredProceduresToSelectedTable.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.brnGenerateStoredProceduresToSelectedTable.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.brnGenerateStoredProceduresToSelectedTable.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.brnGenerateStoredProceduresToSelectedTable.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.brnGenerateStoredProceduresToSelectedTable.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.brnGenerateStoredProceduresToSelectedTable.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.brnGenerateStoredProceduresToSelectedTable.ForeColor = System.Drawing.Color.White;
            this.brnGenerateStoredProceduresToSelectedTable.Location = new System.Drawing.Point(160, 71);
            this.brnGenerateStoredProceduresToSelectedTable.Name = "brnGenerateStoredProceduresToSelectedTable";
            this.brnGenerateStoredProceduresToSelectedTable.Size = new System.Drawing.Size(375, 43);
            this.brnGenerateStoredProceduresToSelectedTable.TabIndex = 25;
            this.brnGenerateStoredProceduresToSelectedTable.Text = "Generate To The Selected Table";
            this.brnGenerateStoredProceduresToSelectedTable.Click += new System.EventHandler(this.brnGenerateStoredProceduresToSelectedTable_Click);
            // 
            // guna2GroupBox2
            // 
            this.guna2GroupBox2.BorderColor = System.Drawing.Color.Gainsboro;
            this.guna2GroupBox2.Controls.Add(this.txtBusinessPath);
            this.guna2GroupBox2.Controls.Add(this.btnGenerateBusiness);
            this.guna2GroupBox2.Controls.Add(this.label4);
            this.guna2GroupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox2.ForeColor = System.Drawing.Color.Black;
            this.guna2GroupBox2.Location = new System.Drawing.Point(6, 207);
            this.guna2GroupBox2.Name = "guna2GroupBox2";
            this.guna2GroupBox2.Size = new System.Drawing.Size(1164, 185);
            this.guna2GroupBox2.TabIndex = 27;
            this.guna2GroupBox2.Text = "Business Layer";
            // 
            // txtBusinessPath
            // 
            this.txtBusinessPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBusinessPath.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBusinessPath.Location = new System.Drawing.Point(55, 54);
            this.txtBusinessPath.Multiline = true;
            this.txtBusinessPath.Name = "txtBusinessPath";
            this.txtBusinessPath.Size = new System.Drawing.Size(1096, 71);
            this.txtBusinessPath.TabIndex = 24;
            // 
            // btnGenerateBusiness
            // 
            this.btnGenerateBusiness.Animated = true;
            this.btnGenerateBusiness.BorderRadius = 22;
            this.btnGenerateBusiness.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateBusiness.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateBusiness.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateBusiness.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateBusiness.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerateBusiness.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnGenerateBusiness.ForeColor = System.Drawing.Color.White;
            this.btnGenerateBusiness.Location = new System.Drawing.Point(967, 135);
            this.btnGenerateBusiness.Name = "btnGenerateBusiness";
            this.btnGenerateBusiness.Size = new System.Drawing.Size(188, 43);
            this.btnGenerateBusiness.TabIndex = 25;
            this.btnGenerateBusiness.Text = "Generate";
            this.btnGenerateBusiness.Click += new System.EventHandler(this.btnGenerateBusiness_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(3, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 21);
            this.label4.TabIndex = 23;
            this.label4.Text = "Path:";
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BorderColor = System.Drawing.Color.Gainsboro;
            this.guna2GroupBox1.Controls.Add(this.txtDataAccessPath);
            this.guna2GroupBox1.Controls.Add(this.btnGenerateDataAccess);
            this.guna2GroupBox1.Controls.Add(this.label2);
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.Black;
            this.guna2GroupBox1.Location = new System.Drawing.Point(6, 6);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(1164, 185);
            this.guna2GroupBox1.TabIndex = 26;
            this.guna2GroupBox1.Text = "Data Access Layer";
            // 
            // txtDataAccessPath
            // 
            this.txtDataAccessPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDataAccessPath.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDataAccessPath.Location = new System.Drawing.Point(55, 54);
            this.txtDataAccessPath.Multiline = true;
            this.txtDataAccessPath.Name = "txtDataAccessPath";
            this.txtDataAccessPath.Size = new System.Drawing.Size(1096, 71);
            this.txtDataAccessPath.TabIndex = 24;
            // 
            // btnGenerateDataAccess
            // 
            this.btnGenerateDataAccess.Animated = true;
            this.btnGenerateDataAccess.BorderRadius = 22;
            this.btnGenerateDataAccess.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateDataAccess.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateDataAccess.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateDataAccess.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateDataAccess.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerateDataAccess.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnGenerateDataAccess.ForeColor = System.Drawing.Color.White;
            this.btnGenerateDataAccess.Location = new System.Drawing.Point(967, 135);
            this.btnGenerateDataAccess.Name = "btnGenerateDataAccess";
            this.btnGenerateDataAccess.Size = new System.Drawing.Size(188, 43);
            this.btnGenerateDataAccess.TabIndex = 25;
            this.btnGenerateDataAccess.Text = "Generate";
            this.btnGenerateDataAccess.Click += new System.EventHandler(this.btnGenerateDataAccess_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(3, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 21);
            this.label2.TabIndex = 23;
            this.label2.Text = "Path:";
            // 
            // comboDatabaseName
            // 
            this.comboDatabaseName.BackColor = System.Drawing.Color.Transparent;
            this.comboDatabaseName.BorderColor = System.Drawing.Color.Gray;
            this.comboDatabaseName.BorderRadius = 17;
            this.comboDatabaseName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboDatabaseName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDatabaseName.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.comboDatabaseName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.comboDatabaseName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboDatabaseName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.comboDatabaseName.ItemHeight = 30;
            this.comboDatabaseName.Location = new System.Drawing.Point(12, 80);
            this.comboDatabaseName.Name = "comboDatabaseName";
            this.comboDatabaseName.Size = new System.Drawing.Size(301, 36);
            this.comboDatabaseName.TabIndex = 27;
            this.comboDatabaseName.SelectedIndexChanged += new System.EventHandler(this.comboDatabaseName_SelectedIndexChanged);
            this.comboDatabaseName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboDatabaseName_KeyPress);
            this.comboDatabaseName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comboDatabaseName_MouseDown);
            // 
            // guna2GroupBox5
            // 
            this.guna2GroupBox5.Controls.Add(this.btnGenerateDataAccessHelper);
            this.guna2GroupBox5.Controls.Add(this.GenerateLogHandler);
            this.guna2GroupBox5.Controls.Add(this.btnGenerateErrorLogger);
            this.guna2GroupBox5.Controls.Add(this.btnGenerateDataAccessSettings);
            this.guna2GroupBox5.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox5.ForeColor = System.Drawing.Color.Black;
            this.guna2GroupBox5.Location = new System.Drawing.Point(500, 535);
            this.guna2GroupBox5.Name = "guna2GroupBox5";
            this.guna2GroupBox5.Size = new System.Drawing.Size(352, 253);
            this.guna2GroupBox5.TabIndex = 35;
            this.guna2GroupBox5.Text = "Helper Classes For Data Access Layer";
            // 
            // btnGenerateBusinessLayer
            // 
            this.btnGenerateBusinessLayer.Animated = true;
            this.btnGenerateBusinessLayer.BorderRadius = 22;
            this.btnGenerateBusinessLayer.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateBusinessLayer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateBusinessLayer.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateBusinessLayer.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateBusinessLayer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerateBusinessLayer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateBusinessLayer.ForeColor = System.Drawing.Color.White;
            this.btnGenerateBusinessLayer.Location = new System.Drawing.Point(858, 633);
            this.btnGenerateBusinessLayer.Name = "btnGenerateBusinessLayer";
            this.btnGenerateBusinessLayer.Size = new System.Drawing.Size(304, 45);
            this.btnGenerateBusinessLayer.TabIndex = 30;
            this.btnGenerateBusinessLayer.Text = "Generate Business Layer";
            this.btnGenerateBusinessLayer.Click += new System.EventHandler(this.btnShowBusinessLayer_Click);
            // 
            // btnGenerateDataAccessSettings
            // 
            this.btnGenerateDataAccessSettings.Animated = true;
            this.btnGenerateDataAccessSettings.BorderRadius = 22;
            this.btnGenerateDataAccessSettings.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateDataAccessSettings.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateDataAccessSettings.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateDataAccessSettings.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateDataAccessSettings.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerateDataAccessSettings.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateDataAccessSettings.ForeColor = System.Drawing.Color.White;
            this.btnGenerateDataAccessSettings.Location = new System.Drawing.Point(13, 47);
            this.btnGenerateDataAccessSettings.Name = "btnGenerateDataAccessSettings";
            this.btnGenerateDataAccessSettings.Size = new System.Drawing.Size(327, 45);
            this.btnGenerateDataAccessSettings.TabIndex = 31;
            this.btnGenerateDataAccessSettings.Text = "Generate Data Access Settings";
            this.btnGenerateDataAccessSettings.Click += new System.EventHandler(this.btnGenerateDataAccessSettings_Click);
            // 
            // btnGenerateErrorLogger
            // 
            this.btnGenerateErrorLogger.Animated = true;
            this.btnGenerateErrorLogger.BorderRadius = 22;
            this.btnGenerateErrorLogger.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateErrorLogger.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateErrorLogger.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateErrorLogger.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateErrorLogger.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerateErrorLogger.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateErrorLogger.ForeColor = System.Drawing.Color.White;
            this.btnGenerateErrorLogger.Location = new System.Drawing.Point(13, 98);
            this.btnGenerateErrorLogger.Name = "btnGenerateErrorLogger";
            this.btnGenerateErrorLogger.Size = new System.Drawing.Size(327, 45);
            this.btnGenerateErrorLogger.TabIndex = 32;
            this.btnGenerateErrorLogger.Text = "Generate Error Logger";
            this.btnGenerateErrorLogger.Click += new System.EventHandler(this.btnGenerateErrorLogger_Click);
            // 
            // GenerateLogHandler
            // 
            this.GenerateLogHandler.Animated = true;
            this.GenerateLogHandler.BorderRadius = 22;
            this.GenerateLogHandler.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.GenerateLogHandler.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.GenerateLogHandler.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.GenerateLogHandler.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.GenerateLogHandler.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.GenerateLogHandler.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateLogHandler.ForeColor = System.Drawing.Color.White;
            this.GenerateLogHandler.Location = new System.Drawing.Point(13, 149);
            this.GenerateLogHandler.Name = "GenerateLogHandler";
            this.GenerateLogHandler.Size = new System.Drawing.Size(327, 45);
            this.GenerateLogHandler.TabIndex = 33;
            this.GenerateLogHandler.Text = "Generate Log Handler";
            this.GenerateLogHandler.Click += new System.EventHandler(this.GenerateLogHandler_Click);
            // 
            // btnGenerateDataAccessHelper
            // 
            this.btnGenerateDataAccessHelper.Animated = true;
            this.btnGenerateDataAccessHelper.BorderRadius = 22;
            this.btnGenerateDataAccessHelper.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateDataAccessHelper.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerateDataAccessHelper.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateDataAccessHelper.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerateDataAccessHelper.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerateDataAccessHelper.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateDataAccessHelper.ForeColor = System.Drawing.Color.White;
            this.btnGenerateDataAccessHelper.Location = new System.Drawing.Point(13, 200);
            this.btnGenerateDataAccessHelper.Name = "btnGenerateDataAccessHelper";
            this.btnGenerateDataAccessHelper.Size = new System.Drawing.Size(327, 45);
            this.btnGenerateDataAccessHelper.TabIndex = 34;
            this.btnGenerateDataAccessHelper.Text = "Generate Data Access Helper";
            this.btnGenerateDataAccessHelper.Click += new System.EventHandler(this.btnGenerateDataAccessHelper_Click);
            // 
            // frmCodeGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1583, 935);
            this.Controls.Add(this.comboDatabaseName);
            this.Controls.Add(this.tcMode);
            this.Controls.Add(this.lblNumberOfTablesRecords);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listviewTablesName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmCodeGenerator";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code Generetor";
            this.Load += new System.EventHandler(this.frmCodeGenerator_Load);
            this.tcMode.ResumeLayout(false);
            this.tbNormal.ResumeLayout(false);
            this.tbNormal.PerformLayout();
            this.tbAdvanced.ResumeLayout(false);
            this.guna2GroupBox4.ResumeLayout(false);
            this.guna2GroupBox4.PerformLayout();
            this.guna2GroupBox3.ResumeLayout(false);
            this.guna2GroupBox2.ResumeLayout(false);
            this.guna2GroupBox2.PerformLayout();
            this.guna2GroupBox1.ResumeLayout(false);
            this.guna2GroupBox1.PerformLayout();
            this.guna2GroupBox5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listviewTablesName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label lblNumberOfTablesRecords;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2TabControl tcMode;
        private System.Windows.Forms.TabPage tbNormal;
        private System.Windows.Forms.TabPage tbAdvanced;
        private Guna.UI2.WinForms.Guna2GradientButton btnCopy;
        private Guna.UI2.WinForms.Guna2GradientButton btnGenerateDateAccessLayer;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label lblNumberOfColumnsRecords;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListView listviewColumnsInfo;
        private System.Windows.Forms.ColumnHeader listColumnName;
        private System.Windows.Forms.ColumnHeader listDataType;
        private System.Windows.Forms.ColumnHeader listAllowNull;
        private Guna.UI2.WinForms.Guna2GradientButton btnGenerateDataAccess;
        private System.Windows.Forms.TextBox txtDataAccessPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader listMaxLength;
        private Guna.UI2.WinForms.Guna2GradientButton btnGenerateStoredProcedure;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox3;
        private Guna.UI2.WinForms.Guna2GradientButton brnGenerateStoredProceduresToSelectedTable;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox2;
        private System.Windows.Forms.TextBox txtBusinessPath;
        private Guna.UI2.WinForms.Guna2GradientButton btnGenerateBusiness;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2GradientButton btnGenerateStoredProceduresToAllTables;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox4;
        private System.Windows.Forms.TextBox txtAppConfigPath;
        private Guna.UI2.WinForms.Guna2GradientButton btnGenerateAppConfig;
        private System.Windows.Forms.Label label7;
        private Guna.UI2.WinForms.Guna2ComboBox comboDatabaseName;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox5;
        private Guna.UI2.WinForms.Guna2GradientButton btnGenerateBusinessLayer;
        private Guna.UI2.WinForms.Guna2GradientButton GenerateLogHandler;
        private Guna.UI2.WinForms.Guna2GradientButton btnGenerateErrorLogger;
        private Guna.UI2.WinForms.Guna2GradientButton btnGenerateDataAccessSettings;
        private Guna.UI2.WinForms.Guna2GradientButton btnGenerateDataAccessHelper;
    }
}

