namespace E1Validation.Desktop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnValidateDocument = new System.Windows.Forms.Button();
            this.btnValidateConversion = new System.Windows.Forms.Button();
            this.btnValidateTable = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowseMappingDoc = new System.Windows.Forms.Button();
            this.txtMappingDocPath = new System.Windows.Forms.TextBox();
            this.tabImportSampleData = new System.Windows.Forms.TabPage();
            this.btnOpenSampleDataFolder = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.btnImportUserSampleTemplate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseSampleData = new System.Windows.Forms.Button();
            this.txtUserSamplePath = new System.Windows.Forms.TextBox();
            this.tab1 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.cboSite_GenerateTemplate = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnGenerateSourceDataTemplate = new System.Windows.Forms.Button();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabImportSampleData.SuspendLayout();
            this.tab1.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar1.Location = new System.Drawing.Point(12, 134);
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(1001, 23);
            this.ProgressBar1.TabIndex = 4;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Lime;
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(987, 421);
            this.textBox1.TabIndex = 5;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(12, 163);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1001, 453);
            this.tabControl2.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(993, 427);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Messages";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dataGridView1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(993, 427);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Results";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(987, 421);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage2.Controls.Add(this.btnValidateDocument);
            this.tabPage2.Controls.Add(this.btnValidateConversion);
            this.tabPage2.Controls.Add(this.btnValidateTable);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.comboBox6);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.comboBox4);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.comboBox5);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.btnBrowseMappingDoc);
            this.tabPage2.Controls.Add(this.txtMappingDocPath);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(993, 90);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Validate Data";
            // 
            // btnValidateDocument
            // 
            this.btnValidateDocument.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidateDocument.Location = new System.Drawing.Point(855, 6);
            this.btnValidateDocument.Name = "btnValidateDocument";
            this.btnValidateDocument.Size = new System.Drawing.Size(132, 23);
            this.btnValidateDocument.TabIndex = 17;
            this.btnValidateDocument.Text = "Validate Conversion Doc";
            this.btnValidateDocument.UseVisualStyleBackColor = true;
            this.btnValidateDocument.Click += new System.EventHandler(this.btnValidateDocument_Click);
            // 
            // btnValidateConversion
            // 
            this.btnValidateConversion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidateConversion.Location = new System.Drawing.Point(878, 61);
            this.btnValidateConversion.Name = "btnValidateConversion";
            this.btnValidateConversion.Size = new System.Drawing.Size(109, 23);
            this.btnValidateConversion.TabIndex = 16;
            this.btnValidateConversion.Text = "Run Conversion";
            this.btnValidateConversion.UseVisualStyleBackColor = true;
            this.btnValidateConversion.Click += new System.EventHandler(this.btnValidateConversion_Click);
            // 
            // btnValidateTable
            // 
            this.btnValidateTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidateTable.Location = new System.Drawing.Point(910, 33);
            this.btnValidateTable.Name = "btnValidateTable";
            this.btnValidateTable.Size = new System.Drawing.Size(77, 23);
            this.btnValidateTable.TabIndex = 14;
            this.btnValidateTable.Text = "Run Table";
            this.btnValidateTable.UseVisualStyleBackColor = true;
            this.btnValidateTable.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(279, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Select a Site:";
            // 
            // comboBox6
            // 
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Location = new System.Drawing.Point(279, 59);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(268, 21);
            this.comboBox6.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(553, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Select a Site:";
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(556, 59);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(270, 21);
            this.comboBox4.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Select a Conversion:";
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(6, 59);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(268, 21);
            this.comboBox5.TabIndex = 8;
            this.comboBox5.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Mapping Document: ";
            // 
            // btnBrowseMappingDoc
            // 
            this.btnBrowseMappingDoc.Location = new System.Drawing.Point(800, 20);
            this.btnBrowseMappingDoc.Name = "btnBrowseMappingDoc";
            this.btnBrowseMappingDoc.Size = new System.Drawing.Size(26, 20);
            this.btnBrowseMappingDoc.TabIndex = 4;
            this.btnBrowseMappingDoc.Text = "...";
            this.btnBrowseMappingDoc.UseVisualStyleBackColor = true;
            this.btnBrowseMappingDoc.Click += new System.EventHandler(this.btnBrowseMappingDoc_Click);
            // 
            // txtMappingDocPath
            // 
            this.txtMappingDocPath.Enabled = false;
            this.txtMappingDocPath.Location = new System.Drawing.Point(7, 20);
            this.txtMappingDocPath.Name = "txtMappingDocPath";
            this.txtMappingDocPath.Size = new System.Drawing.Size(787, 20);
            this.txtMappingDocPath.TabIndex = 3;
            // 
            // tabImportSampleData
            // 
            this.tabImportSampleData.BackColor = System.Drawing.SystemColors.Menu;
            this.tabImportSampleData.Controls.Add(this.btnOpenSampleDataFolder);
            this.tabImportSampleData.Controls.Add(this.label5);
            this.tabImportSampleData.Controls.Add(this.comboBox3);
            this.tabImportSampleData.Controls.Add(this.label4);
            this.tabImportSampleData.Controls.Add(this.comboBox2);
            this.tabImportSampleData.Controls.Add(this.btnImportUserSampleTemplate);
            this.tabImportSampleData.Controls.Add(this.label3);
            this.tabImportSampleData.Controls.Add(this.btnBrowseSampleData);
            this.tabImportSampleData.Controls.Add(this.txtUserSamplePath);
            this.tabImportSampleData.Location = new System.Drawing.Point(4, 22);
            this.tabImportSampleData.Name = "tabImportSampleData";
            this.tabImportSampleData.Size = new System.Drawing.Size(993, 90);
            this.tabImportSampleData.TabIndex = 2;
            this.tabImportSampleData.Text = "Import User Sample Template";
            // 
            // btnOpenSampleDataFolder
            // 
            this.btnOpenSampleDataFolder.Location = new System.Drawing.Point(837, 10);
            this.btnOpenSampleDataFolder.Name = "btnOpenSampleDataFolder";
            this.btnOpenSampleDataFolder.Size = new System.Drawing.Size(153, 23);
            this.btnOpenSampleDataFolder.TabIndex = 8;
            this.btnOpenSampleDataFolder.Text = "Open Sample Data Folder";
            this.btnOpenSampleDataFolder.UseVisualStyleBackColor = true;
            this.btnOpenSampleDataFolder.Click += new System.EventHandler(this.btnOpenSampleDataFolder_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(274, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Select a Site:";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(277, 66);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(270, 21);
            this.comboBox3.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Select a Conversion:";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(3, 66);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(268, 21);
            this.comboBox2.TabIndex = 4;
            // 
            // btnImportUserSampleTemplate
            // 
            this.btnImportUserSampleTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportUserSampleTemplate.Location = new System.Drawing.Point(837, 64);
            this.btnImportUserSampleTemplate.Name = "btnImportUserSampleTemplate";
            this.btnImportUserSampleTemplate.Size = new System.Drawing.Size(153, 23);
            this.btnImportUserSampleTemplate.TabIndex = 3;
            this.btnImportUserSampleTemplate.Text = "Import Sample Data";
            this.btnImportUserSampleTemplate.UseVisualStyleBackColor = true;
            this.btnImportUserSampleTemplate.Click += new System.EventHandler(this.btnImportUserSampleTemplate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "User Sample Template: ";
            // 
            // btnBrowseSampleData
            // 
            this.btnBrowseSampleData.Location = new System.Drawing.Point(768, 26);
            this.btnBrowseSampleData.Name = "btnBrowseSampleData";
            this.btnBrowseSampleData.Size = new System.Drawing.Size(26, 20);
            this.btnBrowseSampleData.TabIndex = 1;
            this.btnBrowseSampleData.Text = "...";
            this.btnBrowseSampleData.UseVisualStyleBackColor = true;
            this.btnBrowseSampleData.Click += new System.EventHandler(this.btnBrowseSampleData_Click);
            // 
            // txtUserSamplePath
            // 
            this.txtUserSamplePath.Enabled = false;
            this.txtUserSamplePath.Location = new System.Drawing.Point(4, 27);
            this.txtUserSamplePath.Name = "txtUserSamplePath";
            this.txtUserSamplePath.Size = new System.Drawing.Size(758, 20);
            this.txtUserSamplePath.TabIndex = 0;
            // 
            // tab1
            // 
            this.tab1.BackColor = System.Drawing.SystemColors.Menu;
            this.tab1.Controls.Add(this.label10);
            this.tab1.Controls.Add(this.cboSite_GenerateTemplate);
            this.tab1.Controls.Add(this.label1);
            this.tab1.Controls.Add(this.comboBox1);
            this.tab1.Controls.Add(this.btnGenerateSourceDataTemplate);
            this.tab1.Location = new System.Drawing.Point(4, 22);
            this.tab1.Name = "tab1";
            this.tab1.Padding = new System.Windows.Forms.Padding(3);
            this.tab1.Size = new System.Drawing.Size(993, 90);
            this.tab1.TabIndex = 0;
            this.tab1.Text = "Generate User Sample Template";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Select a Site:";
            // 
            // cboSite_GenerateTemplate
            // 
            this.cboSite_GenerateTemplate.FormattingEnabled = true;
            this.cboSite_GenerateTemplate.Location = new System.Drawing.Point(6, 61);
            this.cboSite_GenerateTemplate.Name = "cboSite_GenerateTemplate";
            this.cboSite_GenerateTemplate.Size = new System.Drawing.Size(367, 21);
            this.cboSite_GenerateTemplate.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select a Conversion:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 21);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(370, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // btnGenerateSourceDataTemplate
            // 
            this.btnGenerateSourceDataTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateSourceDataTemplate.Location = new System.Drawing.Point(862, 59);
            this.btnGenerateSourceDataTemplate.Name = "btnGenerateSourceDataTemplate";
            this.btnGenerateSourceDataTemplate.Size = new System.Drawing.Size(125, 23);
            this.btnGenerateSourceDataTemplate.TabIndex = 0;
            this.btnGenerateSourceDataTemplate.Text = "Generate Template";
            this.btnGenerateSourceDataTemplate.UseVisualStyleBackColor = true;
            this.btnGenerateSourceDataTemplate.Click += new System.EventHandler(this.btnGenerateSourceDataTemplate_Click);
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tab1);
            this.tabs.Controls.Add(this.tabImportSampleData);
            this.tabs.Controls.Add(this.tabPage2);
            this.tabs.Location = new System.Drawing.Point(12, 12);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1001, 116);
            this.tabs.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1025, 641);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.ProgressBar1);
            this.Controls.Add(this.tabs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "E1 Data Validator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabImportSampleData.ResumeLayout(false);
            this.tabImportSampleData.PerformLayout();
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar ProgressBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnValidateDocument;
        private System.Windows.Forms.Button btnValidateConversion;
        private System.Windows.Forms.Button btnValidateTable;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnBrowseMappingDoc;
        private System.Windows.Forms.TextBox txtMappingDocPath;
        private System.Windows.Forms.TabPage tabImportSampleData;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button btnImportUserSampleTemplate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowseSampleData;
        private System.Windows.Forms.TextBox txtUserSamplePath;
        private System.Windows.Forms.TabPage tab1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnGenerateSourceDataTemplate;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.Button btnOpenSampleDataFolder;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboSite_GenerateTemplate;

    }
}

