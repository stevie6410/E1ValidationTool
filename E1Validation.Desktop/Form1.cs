using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using E1Validation.Lib.Services;
using E1Validation.Lib.Models;
using E1Validation.Lib.Data;
using System.IO;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace E1Validation.Desktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Responsible for setting the default configuration on form load
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            //Setup the Drag and drop
            tabImportSampleData.AllowDrop = true;
            tabImportSampleData.DragEnter += tabImportSampleData_DragEnter;
            tabImportSampleData.DragDrop += tabImportSampleData_DragDrop;

            //Get the data required for the combobxes
            var conversions = ConfigurationData.GetConversions();
            var sites = ConfigurationData.GetSites();

            //Configure Conversion Comboboxes
            ConfigureComboBox(comboBox1, conversions);
            ConfigureComboBox(comboBox2, conversions);
            ConfigureComboBox(comboBox5, conversions);

            //Configure Site Comboboxes
            ConfigureComboBox(comboBox3, sites);
            ConfigureComboBox(comboBox4, sites);
            ConfigureComboBox(cboSite_GenerateTemplate, sites);

            //Configure Table Comboboxes
            ConfigureComboBox(comboBox6, (int)comboBox5.SelectedValue);
        }

        /// <summary>
        /// Configure combobox for Sites. Given a list of sites
        /// </summary>
        private static void ConfigureComboBox(ComboBox cbo, IList<Site> dataSource)
        {
            cbo.DataSource = dataSource;
            cbo.DisplayMember = "SiteName";
            cbo.ValueMember = "Id";
            cbo.SelectedValue = 11;
        }

        /// <summary>
        /// Configure combobox for Conversions. Given a list of conversions 
        /// </summary>
        private static void ConfigureComboBox(ComboBox cbo, IList<Conversion> dataSource)
        {
            cbo.DataSource = dataSource;
            cbo.DisplayMember = "DisplayName";
            cbo.ValueMember = "Id";
            cbo.SelectedValue = 11;
        }

        /// <summary>
        /// Configure a combobox for Tables. Given the conversionID it will fetch the relevant tables
        /// </summary>
        private static void ConfigureComboBox(ComboBox cbo, int conversionId)
        {
            var tables = ConfigurationData.GetTablesByConversion(conversionId);
            cbo.DataSource = tables;
            cbo.DisplayMember = "TableName";
            cbo.ValueMember = "Id";
            cbo.SelectedValue = 11;
        }
        
        /// <summary>
        /// Responsible for getting the first file path of a dropped document and updating the UI
        /// </summary>
        void tabImportSampleData_DragDrop(object sender, DragEventArgs e)
        {
            
            tabImportSampleData.BackColor = Color.Green;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);  
            var path = files[0].ToString();           
            if (File.Exists(path))  //Validate the results
            {
                AutoDetectImportDocument(path);
                System.Threading.Thread.Sleep(2000);
                tabImportSampleData.BackColor = Color.White;    
            }
        }

        /// <summary>
        /// Responsible for updating the UI when a file has been dragged over the drop area
        /// </summary>
        void tabImportSampleData_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            tabImportSampleData.BackColor = Color.Orange;
        }

        /// <summary>
        /// Responsible for validating the sample data docuemnt path and also detect the conversion from the filename
        /// </summary>
        /// <param name="path">Path of the auto generated sample data document</param>
        private void AutoDetectImportDocument(string path)
        {
            WriteLine(Environment.NewLine + "############################################");
            WriteLine("Processing file '" + path + "'");

            //Validate the document
            FileInfo f = new FileInfo(path);
            //Is the file a valid excel file?
            if (f.Extension != ".xlsx")
                throw new Exception("Invalid file format. Should be an XLSX excel file");
            //Populate the textbox with the given path
            txtUserSamplePath.Text = path;
            //Try and detect the conversion from the document name
            int convId = ValidationHelper.ParseFileNameForConversionId(f.Name);
            if (convId > 0)
            {
                comboBox1.SelectedValue = convId;
                WriteLine("Conversion " + comboBox1.Text + " has been identified");
            }
        }

        /// <summary>
        /// Responsible for opening a file dialog and fetching a valid file path
        /// </summary>
        private void btnBrowseSampleData_Click(object sender, EventArgs e)
        {
            //Open a file dialog 
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = "xlsx";
            fd.InitialDirectory = @"\\Dc0348\e1\UserSampleDocs";
            DialogResult result = fd.ShowDialog();
            //Check to see if the result is a valid file path
            if (result == System.Windows.Forms.DialogResult.OK && File.Exists(fd.FileName))
                AutoDetectImportDocument(fd.FileName);
        }

        /// <summary>
        /// Responsible for startting the process of Importing the sample data
        /// </summary>
        private void btnImportUserSampleTemplate_Click(object sender, EventArgs e)
        {
            //try
            //{
            if (File.Exists(txtUserSamplePath.Text))
            {

                StartProgress();
                //Clear any messages in the listbox
                ClearList();
                ControlEnabled(btnImportUserSampleTemplate, false);
                ControlEnabled(comboBox2, false);

                UserSampleTemplateImporter importer = new UserSampleTemplateImporter((int)comboBox2.SelectedValue, (int)comboBox3.SelectedValue);
                importer.UserTemplatePath = txtUserSamplePath.Text;
                importer.OnNewMessage += importer_OnNewMessage;
                importer.Import();
            }
            //}
            //catch (Exception ex)
            //{
            //    WriteLine("");
            //    WriteLine("#####################################################");
            //    WriteLine(ex.Message);
            //    WriteLine("#####################################################");
            //}
            //finally
            //{
            ControlEnabled(btnImportUserSampleTemplate, true);
            ControlEnabled(comboBox2, true);
            EndProgress();
            //}
        }

        /// <summary>
        /// Responsible for executing the code to Generate the Template and open the resulting file
        /// </summary>
         private async void btnGenerateSourceDataTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                StartProgress();
                //Initialise the Generator
                UserSampleTemplateGenerator generator = new UserSampleTemplateGenerator((int)comboBox1.SelectedValue, (int)cboSite_GenerateTemplate.SelectedValue);
                //Setup the message events
                generator.OnNewMessage += generator_OnNewMessage;
                FileInfo fi = await generator.Generate();
                
                string argument = @"/select, " + fi.FullName;
                Process.Start("explorer.exe", argument);
                WriteLine("Complete!");
                dataGridView1.DataSource = generator.MessageOutput;
            }
            catch (Exception ex)
            {
                WriteLine("");
                WriteLine("#####################################################");
                WriteLine(ex.Message);
                WriteLine("#####################################################");
            }
            finally
            {
                EndProgress();
            }
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    StartProgress();

            TableValidator validator = new TableValidator((int)comboBox6.SelectedValue, (int)comboBox4.SelectedValue);
            validator.OnNewMessage += validator_OnNewMessage;
            validator.OnProgressUpdate += validator_OnProgressUpdate;
            validator.mappingDocPath = txtMappingDocPath.Text;
            //string path = await validator.Validate();
            string path = validator.Validate();
            dataGridView1.DataSource = validator.resultsTable;
            Process.Start(path);
            //}
            //catch (Exception ex)
            //{
            //    WriteLine("");
            //    WriteLine("#####################################################");
            //    WriteLine(ex.Message);
            //    WriteLine("#####################################################");
            //}
            //finally
            //{
            //    EndProgress();
            //    WriteLine("");
            //    WriteLine("Processing Complete!");
            //}
        }

        private async void btnValidateConversion_Click(object sender, EventArgs e)
        {
            try
            {
                StartProgress();

                //Loop thru each Table
                foreach (Table table in ConfigurationData.GetTablesByConversion((int)comboBox5.SelectedValue))
                {
                    //Create a new Validation Object
                    TableValidator validator = new TableValidator(table.Id, (int)comboBox4.SelectedValue);
                    validator.OnNewMessage += validator_OnNewMessage;
                    validator.OnProgressUpdate += validator_OnProgressUpdate;
                    validator.mappingDocPath = txtMappingDocPath.Text;
                    //string path = await validator.Validate();
                    string path = await validator.ValidateAsync();
                    //Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                WriteLine("");
                WriteLine("#####################################################");
                WriteLine(ex.Message);
                WriteLine("#####################################################");
            }
            finally
            {
                EndProgress();
                WriteLine("");
                WriteLine("Processing Complete!");
            }
        }

        void validator_OnProgressUpdate(object myobject, int total, int current)
        {
            ///
            UpdateProgressBar(total, current);
        }       

        private void StartProgress()
        {
            this.Invoke((MethodInvoker)delegate
            {
                ProgressBar1.MarqueeAnimationSpeed = 10;
                ProgressBar1.Style = ProgressBarStyle.Continuous;
                ClearList();
                ControlEnabled(btnValidateTable, false);
                ControlEnabled(comboBox4, false);
                ControlEnabled(comboBox5, false);
                ControlEnabled(comboBox6, false);
                ControlEnabled(txtMappingDocPath, false);
                ControlEnabled(btnGenerateSourceDataTemplate, false);
                ControlEnabled(comboBox1, false);
            });
        }

        private void EndProgress()
        {
            this.Invoke((MethodInvoker)delegate
            {
                ProgressBar1.Style = ProgressBarStyle.Continuous;
                ProgressBar1.MarqueeAnimationSpeed = 0;
                ControlEnabled(btnValidateTable, true);
                ControlEnabled(comboBox4, true);
                ControlEnabled(comboBox5, true);
                ControlEnabled(comboBox6, true);
                ControlEnabled(txtMappingDocPath, true);
                ControlEnabled(btnGenerateSourceDataTemplate, true);
                ControlEnabled(comboBox1, true);
            });
        }

        private void UpdateProgressBar(int total, int current)
        {
            this.Invoke((MethodInvoker)delegate
            {
                ProgressBar1.Value = current;
                ProgressBar1.Maximum = total;
            });
        }

        private void ClearList()
        {
            this.Invoke((MethodInvoker)delegate
            {
                //  textBox1.Text = string.Empty;
            });
        }

        private void WriteLine(string msg)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textBox1.AppendText(Environment.NewLine + msg);
            });
        }

        private void ControlEnabled(Control control, bool enabled)
        {
            this.Invoke((MethodInvoker)delegate
            {
                control.Enabled = enabled;
            });
        }

        void generator_OnNewMessage(object myobject, string msg)
        {
            WriteLine(msg);
        }

        void importer_OnNewMessage(object myobject, string msg)
        {
            WriteLine(msg);
        }

        /// <summary>
        /// Responsible for updating the table 
        /// </summary>
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //The conversion has changed...
            //Update the table combobox
            if (comboBox5.SelectedValue != null)
            {
                int conversionID;
                int.TryParse(comboBox5.SelectedValue.ToString(), out conversionID);                
                ConfigureComboBox(comboBox6, conversionID);
            }
        }

        private void btnBrowseMappingDoc_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = "xlsx";
            //fd.InitialDirectory = @"http://beportal.beav.com/Programs/ERP/Team Folders/MDM/Phase 1/Conversion Cycles/M6/Site Data Validation/00274 Winston-Salem SAO";
            DialogResult result = fd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK && File.Exists(fd.FileName))
            {
                txtMappingDocPath.Text = fd.FileName;
                string strCode = fd.SafeFileName.Substring(13, 12);
                int convId = ValidationHelper.ParseFileNameForConversionId(fd.SafeFileName);
                if (convId > 0)
                    comboBox5.SelectedValue = convId;
            }
        }

        void validator_OnNewMessage(object myobject, string msg)
        {
            WriteLine(msg);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            txtMappingDocPath.Text = @"\\beportal.beav.com\DavWWWRoot\Programs\ERP\Team Folders\MDM\Phase 1\Conversion Cycles\M8\Site Validation Files\00110 Corporate\00110 Corporate R2R-CON-0062 Tax Area Conversion Overview.xlsx";

            int convId = ValidationHelper.ParseFileNameForConversionId("00110 Corporate R2R-CON-0062 Tax Area Conversion Overview.xlsx");
            if (convId > 0)
                comboBox5.SelectedValue = convId;
        }

        private void btnValidateDocument_Click(object sender, EventArgs e)
        {
            ConversionDocumentValidator validator = new ConversionDocumentValidator((int)comboBox5.SelectedValue, txtMappingDocPath.Text);
            validator.OnNewMessage += validator_OnNewMessage;
            validator.Validate();
        }

        /// <summary>
        /// Responsible for opening the folder holding the sample data templates 
        /// </summary>
        private void btnOpenSampleDataFolder_Click(object sender, EventArgs e)
        {
            Process.Start(@"\\DC0348\e1\UserSampleDocs\AutoGenerated");
        }
          
    }

}
