using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E1Validation.Lib.Models;
using System.Diagnostics;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.WinFormsUtilities;
using System.IO;

namespace E1Validation.Lib.Services
{
    public class SampleTemplateGenerator
    {
        //Public Properties
        public Conversion Conversion { get; set; }
        public List<string> MessageOutput { get; set; }

        //Private Properties
        private E1ValidationEntities db;

        //Events
        public delegate void MessageHandler(object myobject, string msg);
        public event MessageHandler OnNewMessage;

        //Contructor
        public SampleTemplateGenerator(int ConversionID)
        {
            //Setup the DB connection
            db = new E1ValidationEntities();
            //Try and get the conversion from the database
            Conversion con = db.Conversions.Where(c => c.Id == ConversionID).FirstOrDefault();
            if (con == null)
                throw new Exception("A valid conversion could not be found");
            //Set the given conversion to the public variable
            Conversion = con;

            //Setup the Message Output
            MessageOutput = new List<string>();

            //Set the licence for GemBox
            SpreadsheetInfo.SetLicense("E65I-Y0CT-L0H3-XTH2");
        }

        //Private Functions
        private void OutputMsg(string msg)
        {
            MessageOutput.Add(msg);
            OnNewMessage(this, msg);
            Debug.WriteLine(msg);
        }

        private ExcelFile CreateExcelFile()
        {
            ExcelFile ef = new ExcelFile();
            return ef;
        }

        private ExcelWorksheet GenerateNewWorkSheet(ExcelFile ef, string tableName)
        {
            //Create the new Worksheet
            ExcelWorksheet ws = ef.Worksheets.Add(String.Format("Template {0}", tableName));
            //Set the standard headers and add two instances of the table name
            ws.Cells["A1"].Value = "TableName";
            ws.Cells["A2"].Value = tableName;
            ws.Cells["A3"].Value = tableName;
            ws.Cells["B1"].Value = "SampleDataName";
            return ws;
        }

        //Public Functions
        public Task<FileInfo> Generate()
        {
            return Task.Run(() =>
                {
                    try
                    {
                        OutputMsg("Generating Sample Data Template for " + Conversion.DisplayName);
                        OutputMsg("");

                        //Create new excel doc
                        ExcelFile ef = new ExcelFile();

                        //First get a list of tables in the conversion
                        foreach (Table t in Conversion.Tables.ToList())
                        {
                            System.Threading.Thread.Sleep(1000);
                            OutputMsg("Table: " + t.TableName);

                            //Create a worksheet in the excel file
                            ExcelWorksheet ws = GenerateNewWorkSheet(ef, t.TableName);

                            //Get the source table which matches the table name for E1
                            SourceTable st = t.SourceTables.Where(x => (x.SourceTableName == t.TableName) && (x.IsE1 == true)).FirstOrDefault();
                            if (st == null)
                                throw new Exception("A valid E1 source table was not found for table " + t.TableName);
                            //OutputMsg("Source Table " + st.SourceTableName + " has been found");

                            //Loop through each tableJoin in the Source Table
                            int i = 2; //2 is the index of the staring column in the woksheet as we have already added the two default columns
                            foreach (TableJoin tj in st.TableJoins.ToList())
                            {
                                //Remove any brackets from the fld desc to avoid conflict with the field name
                                string fldDesc = tj.SourceField.FieldDetails.FldDesc.Replace("(", "").Replace(")", "");

                                //Create a new column header for each index type
                                ws.Columns[i].Cells[0].Value = String.Format("{0} ({1})", fldDesc, tj.SourceFieldName);
                                i++;    //Move to the next 
                                OutputMsg(String.Format("              {0} ({1})", tj.SourceField.FieldDetails.FldDesc, tj.SourceFieldName));
                            }

                            //Autofit the columns
                            int columnCount = ws.CalculateMaxUsedColumns();
                            for (int i2 = 0; i2 < columnCount; i2++)
                            {
                                ws.Columns[i2].AutoFit(1, ws.Rows[1], ws.Rows[ws.Rows.Count - 1]);
                            }

                            foreach (ExcelColumn c in ws.Columns)
                            {
                                c.AutoFit();
                            }
                            OutputMsg("");
                        }

                        OutputMsg("");
                        OutputMsg("Saving the excel docuemnt to the server");
                        //Finally save the document to the server and return the path to the application
                        string path = @"\\Dc0348\e1\SampleDocs\AutoGenerated\" + Conversion.Code + "-" + Conversion.Name + "- Sample Data.xlsx";
                        ef.Save(path);
                        FileInfo fi = new FileInfo(path);
                        return fi;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
        }
    }
}
