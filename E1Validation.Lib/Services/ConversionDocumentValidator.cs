using E1Validation.Lib.Models;
using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E1Validation.Lib.Services
{
    /// <summary>
    /// Takes a conversion document and validates all of the fields are valid. Also check to see if there is a valid configuration
    /// </summary>
    public class ConversionDocumentValidator
    {

        //Public Properties
        public Conversion Conversion { get; set; }
        public List<string> MessageOutput { get; set; }
        public ExcelFile ConversionDocument { get; set; }

        //Private Properties
        private E1ValidationEntities db;

        //Events
        public delegate void MessageHandler(object myobject, string msg);
        public event MessageHandler OnNewMessage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conversionId"></param>
        /// <param name="conversionDocPath"></param>
        public ConversionDocumentValidator(int conversionId, string conversionDocPath)
        {
            db = new E1ValidationEntities();
            //Try and get the conversion from the database
            Conversion con = db.Conversions.Where(c => c.Id == conversionId).FirstOrDefault();
            if (con == null)
                throw new Exception("A valid conversion could not be found");
            //Set the given conversion to the public variable
            Conversion = con;
            //Setup the Message Output
            MessageOutput = new List<string>();
            //Set the licence for GemBox
            SpreadsheetInfo.SetLicense("E65I-Y0CT-L0H3-XTH2");

            if (File.Exists(conversionDocPath) == false)
                throw new FileNotFoundException("File could not be found", conversionDocPath);
            ConversionDocument = ExcelFile.Load(conversionDocPath);
        }

        /// <summary>
        /// Start the conversion document validation process
        /// </summary>
        public void Validate()
        {
            //Loop through each page tagged as a checklist
            foreach (ExcelWorksheet ws in ConversionDocument.Worksheets.Where(w => w.Name.EndsWith("Checklist")))
            {
                //Get the table
                Table t = Conversion.Tables.Where(x => x.TableName == ws.Name.Replace(" Checklist", "")).FirstOrDefault();
                if (t == null)
                    throw new Exception("There is no valid table for this conversion in the database (" + ws.Name.Replace(" Checklist", "") + ")");

                DataTable table = ws.CreateDataTable(new CreateDataTableOptions()
                {
                    ColumnHeaders = true,
                    ExtractDataOptions = ExtractDataOptions.StopAtFirstEmptyRow,
                    NumberOfColumns = 12,
                    NumberOfRows = ws.Rows.Count - 1
                });

                OutputMsg("Validating worksheet: " + ws.Name);
                //Loop through each row in the table except the first one
                foreach (DataRow row in table.Rows)
                {
                    //Get the values as objects and parse them into strings if they exist
                    string sourceTableName = string.Empty;
                    string sourceFieldName = string.Empty;
                    string e1FieldName = string.Empty;
                    string ruleName = string.Empty;

                    object objSTableName = row.Field<object>(0);
                    object objSFieldName = row.Field<object>(1);
                    object objE1FieldName = row.Field<object>(3);
                    object objRuleName = row.Field<object>(11);

                    if (objSTableName != null)
                        sourceTableName = objSTableName.ToString();
                    if (objSFieldName != null)
                        sourceFieldName = objSFieldName.ToString();
                    if (objE1FieldName != null)
                        e1FieldName = objE1FieldName.ToString();
                    if (objRuleName != null)
                        ruleName = objRuleName.ToString();

                    OutputMsg(String.Format("SrcTableName: {0} SrcFieldName: {1} E1FieldName {2} RuleName {3}", sourceTableName, sourceFieldName, e1FieldName, ruleName));

                    SourceTable srcTable = null;

                    //Check the SourceTable Exixts if listed
                    if (sourceTableName.Trim() != string.Empty)
                    {
                        srcTable = t.SourceTables.Where(x => x.SourceTableName == sourceTableName).FirstOrDefault();
                        if (srcTable == null)
                            throw new Exception("There is no valid Source Table for (" + sourceTableName + ")");
                    }

                    //Check that the field listed is valid for this source table
                    if (sourceFieldName.Trim() != string.Empty)
                    {
                        //Get the field prefix from the sourceTable
                        string prefix = srcTable.TableHeader.FieldPrefix;
                    }
                }
            }
        }

        //Private Functions
        private void OutputMsg(string msg)
        {
            MessageOutput.Add(msg);
            OnNewMessage(this, msg);
        }


    }
}
