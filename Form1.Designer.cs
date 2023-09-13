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
            this.txtDataAccessLayer = new System.Windows.Forms.TextBox();
            this.btnShowDateAccessLayer = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnShowBusinessLayer = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboDatabaseName = new System.Windows.Forms.ComboBox();
            this.listviewTablesName = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblNumberOfTablesRecords = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MediumBlue;
            this.label1.Location = new System.Drawing.Point(551, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(406, 58);
            this.label1.TabIndex = 0;
            this.label1.Text = "Code Generator";
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
            this.listviewColumnsInfo.Size = new System.Drawing.Size(407, 455);
            this.listviewColumnsInfo.TabIndex = 7;
            this.listviewColumnsInfo.UseCompatibleStateImageBehavior = false;
            this.listviewColumnsInfo.View = System.Windows.Forms.View.Details;
            this.listviewColumnsInfo.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listviewColumnsInfo_AfterLabelEdit);
            // 
            // listColumnName
            // 
            this.listColumnName.Text = "Column Name";
            this.listColumnName.Width = 150;
            // 
            // listDataType
            // 
            this.listDataType.Text = "Data Type";
            this.listDataType.Width = 110;
            // 
            // listAllowNull
            // 
            this.listAllowNull.Text = "Allow Null";
            this.listAllowNull.Width = 140;
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
            this.lblNumberOfColumnsRecords.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblNumberOfColumnsRecords.Location = new System.Drawing.Point(617, 589);
            this.lblNumberOfColumnsRecords.Name = "lblNumberOfColumnsRecords";
            this.lblNumberOfColumnsRecords.Size = new System.Drawing.Size(20, 24);
            this.lblNumberOfColumnsRecords.TabIndex = 10;
            this.lblNumberOfColumnsRecords.Text = "0";
            // 
            // txtDataAccessLayer
            // 
            this.txtDataAccessLayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDataAccessLayer.Location = new System.Drawing.Point(993, 131);
            this.txtDataAccessLayer.Multiline = true;
            this.txtDataAccessLayer.Name = "txtDataAccessLayer";
            this.txtDataAccessLayer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDataAccessLayer.Size = new System.Drawing.Size(495, 479);
            this.txtDataAccessLayer.TabIndex = 11;
            // 
            // btnShowDateAccessLayer
            // 
            this.btnShowDateAccessLayer.BackColor = System.Drawing.SystemColors.WindowText;
            this.btnShowDateAccessLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowDateAccessLayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowDateAccessLayer.ForeColor = System.Drawing.Color.LawnGreen;
            this.btnShowDateAccessLayer.Location = new System.Drawing.Point(1288, 616);
            this.btnShowDateAccessLayer.Name = "btnShowDateAccessLayer";
            this.btnShowDateAccessLayer.Size = new System.Drawing.Size(197, 39);
            this.btnShowDateAccessLayer.TabIndex = 9;
            this.btnShowDateAccessLayer.Text = "Show Data Access Layer";
            this.btnShowDateAccessLayer.UseVisualStyleBackColor = false;
            this.btnShowDateAccessLayer.Click += new System.EventHandler(this.btnShowDateAccessLayer_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.SystemColors.WindowText;
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.ForeColor = System.Drawing.Color.LawnGreen;
            this.btnCopy.Location = new System.Drawing.Point(1500, 571);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 39);
            this.btnCopy.TabIndex = 9;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnShowBusinessLayer
            // 
            this.btnShowBusinessLayer.BackColor = System.Drawing.SystemColors.WindowText;
            this.btnShowBusinessLayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowBusinessLayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowBusinessLayer.ForeColor = System.Drawing.Color.LawnGreen;
            this.btnShowBusinessLayer.Location = new System.Drawing.Point(1000, 616);
            this.btnShowBusinessLayer.Name = "btnShowBusinessLayer";
            this.btnShowBusinessLayer.Size = new System.Drawing.Size(197, 39);
            this.btnShowBusinessLayer.TabIndex = 12;
            this.btnShowBusinessLayer.Text = "Show Business Layer";
            this.btnShowBusinessLayer.UseVisualStyleBackColor = false;
            this.btnShowBusinessLayer.Click += new System.EventHandler(this.btnShowBusinessLayer_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.BackColor = System.Drawing.SystemColors.WindowText;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnReset.Location = new System.Drawing.Point(2, 619);
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
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(77, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Database Name:";
            // 
            // comboDatabaseName
            // 
            this.comboDatabaseName.Cursor = System.Windows.Forms.Cursors.Hand;
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
            this.listviewTablesName.Size = new System.Drawing.Size(145, 455);
            this.listviewTablesName.TabIndex = 15;
            this.listviewTablesName.UseCompatibleStateImageBehavior = false;
            this.listviewTablesName.View = System.Windows.Forms.View.Details;
            this.listviewTablesName.SelectedIndexChanged += new System.EventHandler(this.listviewTablesName_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Table Name";
            this.columnHeader1.Width = 140;
            // 
            // lblNumberOfTablesRecords
            // 
            this.lblNumberOfTablesRecords.AutoSize = true;
            this.lblNumberOfTablesRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumberOfTablesRecords.ForeColor = System.Drawing.Color.DarkOrange;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1583, 660);
            this.Controls.Add(this.lblNumberOfTablesRecords);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listviewTablesName);
            this.Controls.Add(this.comboDatabaseName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnShowBusinessLayer);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnShowDateAccessLayer);
            this.Controls.Add(this.txtDataAccessLayer);
            this.Controls.Add(this.lblNumberOfColumnsRecords);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listviewColumnsInfo);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code Generetor";
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.TextBox txtDataAccessLayer;
        private System.Windows.Forms.Button btnShowDateAccessLayer;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnShowBusinessLayer;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboDatabaseName;
        private System.Windows.Forms.ListView listviewTablesName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label lblNumberOfTablesRecords;
        private System.Windows.Forms.Label label5;
    }
}

