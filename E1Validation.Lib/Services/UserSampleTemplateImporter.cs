using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using E1Validation.Lib.Data;
using E1Validation.Lib.Models;
using Microsoft.VisualBasic;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.WinFormsUtilities;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace E1Validation.Lib.Services
{
    /// <summary>
    /// Imports data from a User Template and generates the sample data required for validation
    ///     - Deals with many lines per user sample
    ///     - Deals with invalid user samples
    ///     - Deals with duplicated user samples
    ///     - Retruns nothing
    /// </summary>
    public class UserSampleTemplateImporter
    {
        #region Properties
        //Public Properties
        public string UserTemplatePath { get; set; }
        public string UserName { get; set; }
        public Site Site { get; set; }
        public E1Validation.Lib.Models.Conversion Conversion { get; set; }
        public List<string> MessageOutput { get; set; }

        //Private Properties
        private E1ValidationEntities db;
        private string E1Environemt = "JDEPD1";
        private string E1Library = "JDEDATA";
        private string WorldEnvironemnt = "BEAWBLDTA";

        //Events
        public delegate void MessageHandler(object myobject, string msg);
        public event MessageHandler OnNewMessage;

        #endregion

        #region Methods
        /// <summary>
        /// Constructor 
        /// </summary>
        public UserSampleTemplateImporter(int ConversionId, int siteId)
        {
            //Setup the DB connection
            db = new E1ValidationEntities();

            //Setup the Message Output
            MessageOutput = new List<string>();

            //Try and get the conversion and site from the database
            var con = db.Conversions.Where(c => c.Id == ConversionId).FirstOrDefault();
            var site = db.Sites.Where(x => x.Id == siteId).FirstOrDefault();

            if (con == null)
                throw new Exception("A valid conversion could not be found");

            if (site == null)
                throw new Exception("A valid site could not be found");

            //Set the public property
            Site = site;

            //Set the given conversion to the public variable
            Conversion = con;

            //Set the licence for GemBox
            SpreadsheetInfo.SetLicense("E65I-Y0CT-L0H3-XTH2");
        }

        /// <summary>
        /// Will import the document and process the user data into sample data and then store it in the database 
        /// </summary>
        public void Import()
        {
            OutputMsg("Importing..." + UserTemplatePath);
            
            //First check to see if we have a valid file path
            if (File.Exists(UserTemplatePath) == false)
                throw new FileNotFoundException("User Template file could not be found", UserTemplatePath);

            //Get the excel document
            ExcelFile ef = ExcelFile.Load(UserTemplatePath);
            
            int i= 0;
            //Loop through each worksheet which starts with "Template"
            foreach (ExcelWorksheet ws in ef.Worksheets.Where(x => x.Name.StartsWith("Template")))
            {
                i += 1;
                OutputMsg(String.Format("Table {0} of {1}", i , ef.Worksheets.Count));

                //Get the table name and then get the table entity if it is within the provided conversion
                string tableName = ws.Name.Replace("Template ", "");
                //Create the datatable to hold all of the records
                DataTable dt = new DataTable(tableName);

                OutputMsg("Processing table: " + tableName);
                Table t = db.Tables.Where(x => x.TableName == tableName && x.Conversion.Id == Conversion.Id).FirstOrDefault();
                if (t == null)
                    throw new Exception("Table (" + tableName + ") in Converison (" + Conversion.DisplayName + ") could not be found in the database");

                //Now we can begin to step through each row and gather the detail for the sample data

                //Get the table prefix
                string tblPrefix = db.TableHeaders.Where(x => x.TableName == tableName).FirstOrDefault().FieldPrefix;
                string whereSQL = string.Empty;

                int colWidth = ws.CalculateMaxUsedColumns();
                //Use the Worksheet instead
                foreach (ExcelRow row in ws.Rows.Where(x => x.Index > 0 && x.Cells[0].Value != null))   //Ignore the first row
                {
                    //Reset the where clause back to nothing
                    whereSQL = string.Empty;

                    foreach (ExcelColumn col in ws.Columns)
                    {
                        if (col.Index > 1 && col.Index <= colWidth && col.Cells[0].Value != null) //Ignore the first 2 columns and make sure there is a valid title
                        {
                            //Extract the FieldName
                            string colTitle = col.Cells[0].Value.ToString();
                            string fldName = tblPrefix + ValidationHelper.ExtractFieldName(colTitle).Trim();
                            string fldValue = col.Cells[row.Index].Value.ToString().Trim();

                            //Check to see if MCU has 5 padding
                           if (fldName.EndsWith("MCU"))
                           {
                               if (fldValue.StartsWith(" ") == false)
                               {
                                   fldValue = "     " + fldValue;
                               }
                           }
                            whereSQL += String.Format("{0} = '{1}' AND ", fldName, fldValue);
                        }
                    }                    
                    //Trim the last " AND "
                    whereSQL = whereSQL.Left(whereSQL.Length - 5);
                    //Double up the quotes
                    whereSQL = whereSQL.Replace("'", "''");

                    string selectSQL = string.Empty;

                    if ((bool)t.GetSampleDatafromE1)
                    {//E1
                        //Assemble the full select statment
                        selectSQL = String.Format("SELECT * FROM OPENQUERY({0},'SELECT * FROM {1}.{2} WHERE {3}')",E1Environemt,E1Library, tableName, whereSQL);
                    }
                    else
                    {//World
                        //Assemble the full select statment
                        selectSQL = String.Format("SELECT * FROM OPENQUERY({0},'SELECT * FROM {1} WHERE {2}')", WorldEnvironemnt, tableName, whereSQL);
                    }
                                       
                    //Execute the SQL and retreive the datatable
                    ValidationHelper.FillDataTableFromSQL(ref dt, selectSQL);
                    //OutputMsg(selectSQL);
                }

                //Write out the datatable
                ValidationHelper.DebugTable(dt);

                //Delete any exisiting samples for this site and table
                db.Samples.RemoveRange(db.Samples.Where(x => x.Table.Id == t.Id && x.Site.Id == Site.Id));
                db.SaveChanges();

                //Now we can step through each record and add it to the sample data
                foreach (DataRow row in dt.Rows)
                {
                    int sampleID = (int)db.InsertNewSample(tableName, "sampleName", Site.Id).FirstOrDefault();

                    //Instead of using SampleTemplate. Look for the matching table in the SourceTables and then use the Joins. This will avoid duplication
                    //Get the correct source table
                    SourceTable sTable = null;
                    if ((bool)t.GetSampleDatafromE1)
                    {
                        sTable = t.SourceTables.Where(x => x.SourceTableName == t.TableName && x.IsE1 == true).FirstOrDefault();
                    }
                    else
                    {
                        sTable = t.SourceTables.Where(x => x.SourceTableName == t.TableName && x.IsE1 == false).FirstOrDefault();
                    }
                    
                    if (sTable == null)
                        throw new Exception("A valid source table is not setup for " + t.TableName);
                    foreach (TableJoin tJoin in sTable.TableJoins)
                    {
                        string fldName = tJoin.SourceFieldName;
                        string value = row[tblPrefix + fldName].ToString();

                        //Instead of entering each one indivdually put into a staging table and batch upload
                        db.InsertNewSampleValue(value, sampleID, tblPrefix + fldName);
                       // OutputMsg(fldName + " = " + value);
                    }
                }
            }
            OutputMsg("Import Complete!");
        }

        /// <summary>
        /// Will import the document and process the user data into sample data and then store it in the database 
        /// </summary>
        /// <param name="userTemplatePath"></param>
        public void Import(string userTemplatePath)
        {
            UserTemplatePath = userTemplatePath;
            Import();
        }
        
        private void OutputMsg(string msg)
        {
            MessageOutput.Add(msg);
            OnNewMessage(this, msg);
        }
        

        #endregion
    } 

}
