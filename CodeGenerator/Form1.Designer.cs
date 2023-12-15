namespace Code_Generator
{
    partial class Form1
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
            this.listviewColumnsInfo = new System.Windows.Forms.ListView();
            this.listColumnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listDataType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listAllowNull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label6 = new System.Windows.Forms.Label();
            this.lblNumberOfColumnsRecords = new System.Windows.Forms.Label();
            this.txtData = new System.Windows.Forms.TextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboDatabaseName = new System.Windows.Forms.ComboBox();
            this.listviewTablesName = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblNumberOfTablesRecords = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbNormal = new System.Windows.Forms.RadioButton();
            this.rbAdvance = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnGenerate = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnShowBusinessLayer = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnShowDateAccessLayer = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnCopy = new Guna.UI2.WinForms.Guna2GradientButton();
            this.groupBox1.SuspendLayout();
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
            // listviewColumnsInfo
            // 
            this.listviewColumnsInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.listColumnName,
            this.listDataType,
            this.listAllowNull});
            this.listviewColumnsInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listviewColumnsInfo.GridLines = true;
            this.listviewColumnsInfo.HideSelection = false;
            this.listviewColumnsInfo.LabelEdit = true;
            this.listviewColumnsInfo.Location = new System.Drawing.Point(447, 131);
            this.listviewColumnsInfo.Name = "listviewColumnsInfo";
            this.listviewColumnsInfo.Size = new System.Drawing.Size(474, 455);
            this.listviewColumnsInfo.TabIndex = 7;
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
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(443, 590);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(173, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "# Of Columns Records:";
            // 
            // lblNumberOfColumnsRecords
            // 
            this.lblNumberOfColumnsRecords.AutoSize = true;
            this.lblNumberOfColumnsRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumberOfColumnsRecords.ForeColor = System.Drawing.Color.Fuchsia;
            this.lblNumberOfColumnsRecords.Location = new System.Drawing.Point(617, 589);
            this.lblNumberOfColumnsRecords.Name = "lblNumberOfColumnsRecords";
            this.lblNumberOfColumnsRecords.Size = new System.Drawing.Size(20, 24);
            this.lblNumberOfColumnsRecords.TabIndex = 10;
            this.lblNumberOfColumnsRecords.Text = "0";
            // 
            // txtData
            // 
            this.txtData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtData.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtData.Location = new System.Drawing.Point(993, 131);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtData.Size = new System.Drawing.Size(495, 479);
            this.txtData.TabIndex = 11;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.BackColor = System.Drawing.SystemColors.WindowText;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnReset.Location = new System.Drawing.Point(2, 752);
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
            this.label3.Location = new System.Drawing.Point(77, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Database Name:";
            // 
            // comboDatabaseName
            // 
            this.comboDatabaseName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboDatabaseName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDatabaseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboDatabaseName.FormattingEnabled = true;
            this.comboDatabaseName.Location = new System.Drawing.Point(210, 85);
            this.comboDatabaseName.Name = "comboDatabaseName";
            this.comboDatabaseName.Size = new System.Drawing.Size(145, 28);
            this.comboDatabaseName.TabIndex = 14;
            this.comboDatabaseName.SelectedIndexChanged += new System.EventHandler(this.comboDatabaseName_SelectedIndexChanged);
            this.comboDatabaseName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboDatabaseName_KeyPress);
            this.comboDatabaseName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comboDatabaseName_MouseDown);
            // 
            // listviewTablesName
            // 
            this.listviewTablesName.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listviewTablesName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listviewTablesName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listviewTablesName.GridLines = true;
            this.listviewTablesName.HideSelection = false;
            this.listviewTablesName.LabelEdit = true;
            this.listviewTablesName.Location = new System.Drawing.Point(82, 131);
            this.listviewTablesName.Name = "listviewTablesName";
            this.listviewTablesName.Size = new System.Drawing.Size(171, 455);
            this.listviewTablesName.TabIndex = 15;
            this.listviewTablesName.UseCompatibleStateImageBehavior = false;
            this.listviewTablesName.View = System.Windows.Forms.View.Details;
            this.listviewTablesName.SelectedIndexChanged += new System.EventHandler(this.listviewTablesName_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Table Name";
            this.columnHeader1.Width = 180;
            // 
            // lblNumberOfTablesRecords
            // 
            this.lblNumberOfTablesRecords.AutoSize = true;
            this.lblNumberOfTablesRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumberOfTablesRecords.ForeColor = System.Drawing.Color.Fuchsia;
            this.lblNumberOfTablesRecords.Location = new System.Drawing.Point(233, 589);
            this.lblNumberOfTablesRecords.Name = "lblNumberOfTablesRecords";
            this.lblNumberOfTablesRecords.Size = new System.Drawing.Size(20, 24);
            this.lblNumberOfTablesRecords.TabIndex = 17;
            this.lblNumberOfTablesRecords.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(78, 590);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(158, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "# Of Tables Records:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbAdvance);
            this.groupBox1.Controls.Add(this.rbNormal);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.DarkOrange;
            this.groupBox1.Location = new System.Drawing.Point(1099, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 62);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mode";
            // 
            // rbNormal
            // 
            this.rbNormal.AutoSize = true;
            this.rbNormal.ForeColor = System.Drawing.Color.Black;
            this.rbNormal.Location = new System.Drawing.Point(10, 25);
            this.rbNormal.Name = "rbNormal";
            this.rbNormal.Size = new System.Drawing.Size(77, 24);
            this.rbNormal.TabIndex = 0;
            this.rbNormal.Text = "Normal";
            this.rbNormal.UseVisualStyleBackColor = true;
            this.rbNormal.CheckedChanged += new System.EventHandler(this.rbNormal_CheckedChanged);
            // 
            // rbAdvance
            // 
            this.rbAdvance.AutoSize = true;
            this.rbAdvance.Checked = true;
            this.rbAdvance.ForeColor = System.Drawing.Color.Black;
            this.rbAdvance.Location = new System.Drawing.Point(121, 25);
            this.rbAdvance.Name = "rbAdvance";
            this.rbAdvance.Size = new System.Drawing.Size(89, 24);
            this.rbAdvance.TabIndex = 1;
            this.rbAdvance.TabStop = true;
            this.rbAdvance.Text = "Advance";
            this.rbAdvance.UseVisualStyleBackColor = true;
            this.rbAdvance.CheckedChanged += new System.EventHandler(this.rbAdvance_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(329, 677);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 20);
            this.label2.TabIndex = 19;
            this.label2.Text = "Path:";
            // 
            // txtPath
            // 
            this.txtPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPath.Location = new System.Drawing.Point(381, 677);
            this.txtPath.Multiline = true;
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(527, 58);
            this.txtPath.TabIndex = 20;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Animated = true;
            this.btnGenerate.BorderRadius = 22;
            this.btnGenerate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerate.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnGenerate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnGenerate.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.Location = new System.Drawing.Point(544, 742);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(180, 45);
            this.btnGenerate.TabIndex = 22;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnShowBusinessLayer
            // 
            this.btnShowBusinessLayer.Animated = true;
            this.btnShowBusinessLayer.BorderRadius = 22;
            this.btnShowBusinessLayer.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnShowBusinessLayer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnShowBusinessLayer.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnShowBusinessLayer.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnShowBusinessLayer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnShowBusinessLayer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowBusinessLayer.ForeColor = System.Drawing.Color.White;
            this.btnShowBusinessLayer.Location = new System.Drawing.Point(1099, 667);
            this.btnShowBusinessLayer.Name = "btnShowBusinessLayer";
            this.btnShowBusinessLayer.Size = new System.Drawing.Size(242, 45);
            this.btnShowBusinessLayer.TabIndex = 23;
            this.btnShowBusinessLayer.Text = "Show Business Layer";
            this.btnShowBusinessLayer.Click += new System.EventHandler(this.btnShowBusinessLayer_Click);
            // 
            // btnShowDateAccessLayer
            // 
            this.btnShowDateAccessLayer.Animated = true;
            this.btnShowDateAccessLayer.BorderRadius = 22;
            this.btnShowDateAccessLayer.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnShowDateAccessLayer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnShowDateAccessLayer.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnShowDateAccessLayer.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnShowDateAccessLayer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnShowDateAccessLayer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnShowDateAccessLayer.ForeColor = System.Drawing.Color.White;
            this.btnShowDateAccessLayer.Location = new System.Drawing.Point(1064, 616);
            this.btnShowDateAccessLayer.Name = "btnShowDateAccessLayer";
            this.btnShowDateAccessLayer.Size = new System.Drawing.Size(304, 45);
            this.btnShowDateAccessLayer.TabIndex = 24;
            this.btnShowDateAccessLayer.Text = "Show Data Access Layer";
            this.btnShowDateAccessLayer.Click += new System.EventHandler(this.btnShowDateAccessLayer_Click);
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
            this.btnCopy.Location = new System.Drawing.Point(1474, 616);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(109, 45);
            this.btnCopy.TabIndex = 25;
            this.btnCopy.Text = "Copy";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1583, 793);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnShowDateAccessLayer);
            this.Controls.Add(this.btnShowBusinessLayer);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblNumberOfTablesRecords);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listviewTablesName);
            this.Controls.Add(this.comboDatabaseName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.txtData);
            this.Controls.Add(this.lblNumberOfColumnsRecords);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listviewColumnsInfo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code Generetor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listviewColumnsInfo;
        private System.Windows.Forms.ColumnHeader listColumnName;
        private System.Windows.Forms.ColumnHeader listDataType;
        private System.Windows.Forms.ColumnHeader listAllowNull;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblNumberOfColumnsRecords;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboDatabaseName;
        private System.Windows.Forms.ListView listviewTablesName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label lblNumberOfTablesRecords;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbAdvance;
        private System.Windows.Forms.RadioButton rbNormal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPath;
        private Guna.UI2.WinForms.Guna2GradientButton btnGenerate;
        private Guna.UI2.WinForms.Guna2GradientButton btnShowBusinessLayer;
        private Guna.UI2.WinForms.Guna2GradientButton btnShowDateAccessLayer;
        private Guna.UI2.WinForms.Guna2GradientButton btnCopy;
    }
}

