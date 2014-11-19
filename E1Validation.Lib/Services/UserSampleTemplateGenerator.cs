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
using System.Drawing;

namespace E1Validation.Lib.Services
{
    public class UserSampleTemplateGenerator
    {
        //Public Properties
        public Conversion Conversion { get; set; }
        public Site Site { get; set; }
        public List<string> MessageOutput { get; set; }

        //Private Properties
        private E1ValidationEntities db;

        //Events
        public delegate void MessageHandler(object myobject, string msg);
        public event MessageHandler OnNewMessage;

        //Contructor
        public UserSampleTemplateGenerator(int ConversionID, int SiteID)
        {
            //Setup the DB connection
            db = new E1ValidationEntities();
            //Try and get the conversion from the database
            Conversion con = db.Conversions.Where(c => c.Id == ConversionID).FirstOrDefault();

            if (con == null)
                throw new Exception("A valid conversion could not be found");
            //Set the given conversion to the public variable
            Conversion = con;

            //Try and get the site from the database
            Site site = db.Sites.Where(x => x.Id == SiteID).FirstOrDefault();
            if (site == null)
                throw new Exception("A valid site could not be found");
            Site = site;

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
            ws.Cells["A4"].Value = tableName;
            ws.Cells["A5"].Value = tableName;
            ws.Cells["A6"].Value = tableName;
            ws.Cells["A7"].Value = tableName;
            ws.Cells["A8"].Value = tableName;
            ws.Cells["A9"].Value = tableName;
            ws.Cells["A10"].Value = tableName;
            ws.Cells["A11"].Value = tableName;
            ws.Cells["B1"].Value = "SampleDataName";
            return ws;
        }

        //Public Functions
        public Task<FileInfo> Generate()
        {
            return Task.Run(() =>
                {
                    OutputMsg("Generating Sample Data Template for " + Conversion.DisplayName);
                    OutputMsg("");

                    //Create new excel doc
                    ExcelFile ef = new ExcelFile();

                    //First get a list of tables in the conversion
                    foreach (Table t in Conversion.Tables.ToList())
                    {
                        System.Threading.Thread.Sleep(200);
                        OutputMsg("Table: " + t.TableName);

                        //Create a worksheet in the excel file
                        ExcelWorksheet ws = GenerateNewWorkSheet(ef, t.TableName);

                        int i = 2; //2 is the index of the staring column in the woksheet as we have already added the two default columns
                        //Get the user sample template 
                        foreach (UserSampleTemplate usd in t.UserSampleTemplates)
                        {
                            //Remove any brackets from the fld desc to avoid conflict with the field name
                            string fldDesc = usd.TableField.FieldDescription.Replace("(", "").Replace(")", "");

                            //Create a new column header for each index type
                            ws.Columns[i].Cells[0].Value = String.Format("{0} ({1})", fldDesc, usd.TableField.FieldName);
                            i++;    //Move to the next 
                            OutputMsg(String.Format("              {0} ({1})", usd.TableField.FieldDescription, usd.TableField.FieldName));
                        }

                        //Autofit the columns
                        int columnCount = ws.CalculateMaxUsedColumns();
                        for (int i2 = 0; i2 < columnCount; i2++)
                        {
                            ws.Columns[i2].AutoFit(1, ws.Rows[1], ws.Rows[ws.Rows.Count - 1]);
                        }

                        //Format the top row and autofit columns
                        foreach (ExcelColumn c in ws.Columns)
                        {
                            c.Cells[0].Style.FillPattern.SetSolid(Color.LightBlue);
                            c.AutoFit();
                        }
                        OutputMsg("");
                    }

                    OutputMsg("");
                    OutputMsg("Saving the excel document to the server");
                    //Finally save the document to the server and return the path to the application
                    string rootFolder = @"\\Dc0348\e1\UserSampleDocs\AutoGenerated\" + Site.SiteName;
                    string path = String.Format(@"{0}\{1}", rootFolder, Conversion.Code + "-" + Conversion.Name + "- User Sample Data.xlsx");
                    if (Directory.Exists(rootFolder) == false)
                        Directory.CreateDirectory(rootFolder);

                    ef.Save(path);
                    FileInfo fi = new FileInfo(path);
                    return fi;
                });
        }
    }
}
