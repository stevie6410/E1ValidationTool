using E1Validation.Lib.Data;
using E1Validation.Lib.Models;
using GemBox.Spreadsheet;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E1Validation.Lib.Services
{
    public static class ValidationHelper
    {

        public static int ParseFileNameForConversionId(string filename)
        {
            //Add all of the strings to remove to the list
            List<string> keywords = new List<string>();
            keywords.Add("00110 Coporate ");
            keywords.Add("00110 Corporate ");
            keywords.Add("00000 Financial Reconciliation ");
            keywords.Add("00209 CAS Admin ");
            keywords.Add("00261 Seating Admin ");
            keywords.Add("00262 WestPoint ");
            keywords.Add("00274 Winston Salem SAO ");

            string name = filename;

            foreach (string keyword in keywords)
            {
                name = name.Replace(keyword, "");
            }

            //Get the first 12 chars
            name = name.Left(12);
            //Look for a conversion
            var conv = ConfigurationData.GetConversions().Where(x => x.DisplayName.StartsWith(name)).FirstOrDefault();
            if (conv != null)
                return conv.Id;
            else
                return 0;
        }

        public static void FillDataTableFromSQL(ref DataTable table, string sql)
        {
            Debug.WriteLine(sql);
            string connString = "Server=DC0348;Database=E1ValidationManager;User Id=appuser;Password=Password2;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(sql, conn))
                {
                    adp.Fill(table);
                }
            }
        }

        public static string GetTablePrefix(string tableName)
        {
            using (var db = new E1ValidationEntities())
            {
                TableHeader header = db.TableHeaders.Where(x => x.TableName == tableName).FirstOrDefault();
                if (header == null)
                    throw new Exception("No Table Header for (" + tableName + ") could be found, when searching for a field prefix");
                else
                    return header.FieldPrefix;
            }
        }

        public static string GetTablePrefix(List<SourceTable> sTables, string tableName)
        {
            SourceTable sTable = sTables.Where(x => x.SourceTableName == tableName.ToUpper()).FirstOrDefault();
            if (sTable == null)
                throw new Exception("Table prefix could not be found for table name " + tableName);
            return sTable.TableHeader.FieldPrefix;
        }

        /// <summary>
        /// Responisble for extracting the field name from a string where the fieldname is enclosed in brackets e.g. "Item Number (ILTM)" returns "ILTM"
        /// </summary>
        public static string ExtractFieldName(string str)
        {
            int index = Strings.InStr(str, "(");
            str = Strings.Right(str, str.Length - (index));
            str = Strings.Left(str, str.Length - 1);
            if (str.StartsWith("'") && str.EndsWith("'"))
            {
                str = Strings.Right(str, str.Length - 1);
                str = Strings.Left(str, str.Length - 1);
            }
            if (str.StartsWith("\"") && str.EndsWith("\""))
            {
                str = Strings.Right(str, str.Length - 1);
                str = Strings.Left(str, str.Length - 1);
            }
            return str;
        }

        public static void DebugTable(DataTable table)
        {
            Debug.WriteLine("--- DebugTable(" + Convert.ToString(table.TableName) + ") ---");
            int zeilen = table.Rows.Count;
            int spalten = table.Columns.Count;

            // Header
            for (int i = 0; i <= table.Columns.Count - 1; i++)
            {
                string s = table.Columns[i].ToString();
                Debug.Write(String.Format("{0,-20} | ", s));
            }
            Debug.Write(Environment.NewLine);
            for (int i = 0; i <= table.Columns.Count - 1; i++)
            {
                Debug.Write("---------------------|-");
            }
            Debug.Write(Environment.NewLine);

            // Data
            for (int i = 0; i <= zeilen - 1; i++)
            {
                DataRow row = table.Rows[i];
                //Debug.WriteLine("{0} {1} ", row[0], row[1]);
                for (int j = 0; j <= spalten - 1; j++)
                {
                    string s = row[j].ToString();
                    if (s.Length > 20)
                    {
                        s = s.Substring(0, 17) + "...";
                    }
                    Debug.Write(String.Format("{0,-20} | ", s));
                }
                Debug.Write(Environment.NewLine);
            }
            for (int i = 0; i <= table.Columns.Count - 1; i++)
            {
                Debug.Write("---------------------|-");
            }
            Debug.Write(Environment.NewLine);
        }

        public static void ExportValidationResults(ref DataTable resultsTable, string fileName, string tableName)
        {
                ExcelFile excelFile = new ExcelFile();

                //Insert the first table
                var ws = excelFile.Worksheets.Add(resultsTable.TableName);
                ws.InsertDataTable(resultsTable, new InsertDataTableOptions("A1") { ColumnHeaders = true });

                //Autofit columns
                var colCount = ws.CalculateMaxUsedColumns();
                for (int i = 0; i <= colCount - 1; i++)
                {
                    ws.Columns[i].AutoFit(1, ws.Rows[3], ws.Rows[ws.Rows.Count - 1]);
                }

                //Format the resuls
                ws.Columns[0].Style.Font.Weight = 700;
                ws.Rows[0].Style.Font.Weight = 700;
                ws.Rows[0].Style.FillPattern.SetSolid(Color.DarkBlue);
                ws.Rows[1].Style.FillPattern.SetSolid(Color.LightGray);
                ws.Rows[0].Style.Font.Color = Color.White;
                ws.Rows[1].Style.Font.Color = Color.White;
                ws.Rows[1].Style.WrapText = true;

                foreach (ExcelRow row in ws.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        string cellValue = row.Cells[0].Value.ToString();
                        switch (cellValue)
                        {
                            case "-":
                                row.Style.FillPattern.SetSolid(Color.LightGray);
                                break;
                            case "World":
                                //row.Style.FillPattern.SetSolid(Color.Thistle)
                                row.Style.Font.Color = Color.Purple;
                                break;
                            case "E1":
                                //  row.Style.FillPattern.SetSolid(Color.SkyBlue)
                                row.Style.Font.Color = Color.RoyalBlue;
                                break;
                            case "Rule":
                                break;
                            case "Result":
                                foreach (ExcelCell cell in row.Cells)
                                {
                                    if (Information.IsDBNull(cell.Value) == false && cell.Value != null)
                                    {
                                        switch (cell.Value.ToString().ToUpper())
                                        {
                                            case "SKIPPED":
                                                cell.Style.FillPattern.SetSolid(Color.Moccasin);
                                                cell.Style.Font.Color = Color.Chocolate;
                                                break;
                                            case "PASS":
                                                cell.Style.FillPattern.SetSolid(Color.PaleGreen);
                                                cell.Style.Font.Color = Color.DarkGreen;
                                                break;
                                            case "FAIL":
                                                cell.Style.FillPattern.SetSolid(Color.LightCoral);
                                                cell.Style.Font.Color = Color.DarkRed;
                                                break;
                                            case "MANUAL":
                                                cell.Style.FillPattern.SetSolid(Color.MediumPurple);
                                                cell.Style.Font.Color = Color.Black;
                                                cell.Value = "";
                                                break;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                excelFile.Save(fileName);
        }

        

    }
}
