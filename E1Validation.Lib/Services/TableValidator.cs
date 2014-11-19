using E1Validation.Lib.Models;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.WinFormsUtilities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E1Validation.Lib.Services
{
    /// <summary>
    /// Responsible for generating a validation document for users to compare source and converted fields.
    /// Also applies simple rules
    /// </summary>
    public class TableValidator
    {
        //Public Properties
        public List<string> MessageOutput { get; set; }
        public Table Table { get; set; }
        public Site Site { get; set; }
        public List<Sample> Samples { get; set; }
        public List<XRef> Xrefs { get; set; }
        public string mappingDocPath { get; set; }
        public DataTable resultsTable;
        public DataTable resultsTableFormatted;
        public string E1Environemt = "JDEPD1";
        public string E1Library = "JDEDATA";
        public string WorldEnvironemnt = "BEAWBLDTA";

        //Private Properties
        private E1ValidationEntities db;
        private ExcelFile mappingDoc;
        private ExcelWorksheet mappingWs;
        private string tablePrefix;
        private Stopwatch sw1;

        //Constant 
        const string NotRequired = "*NotRequired*";

        //Events
        public delegate void MessageHandler(object myobject, string msg);
        public event MessageHandler OnNewMessage;
        public delegate void ProgessHandler(object myobject, int total, int current);
        public event ProgessHandler OnProgressUpdate;

        //Constructor
        public TableValidator(int tableId, int siteId)
        {
            //Setup the DB connection
            db = new E1ValidationEntities();

            //Setup the Message Output
            MessageOutput = new List<string>();

            //Try and get the conversion and site from the database
            Table table = db.Tables
                            .Include(i => i.Samples.Select(s => s.SampleValues))
                            .Include(i => i.SourceTables.Select(s => s.TableJoins))
                            .Include(i => i.SourceTables.Select(s => s.TableHeader))
                            .Where(x => x.Id == tableId)
                            .ToList()
                            .FirstOrDefault();

            Samples = db.Samples.Include(x => x.SampleValues).Where(x => x.Site.Id == siteId && x.Table.Id == tableId).ToList();

            Xrefs = db.XRefs.ToList();

            var site = db.Sites.Where(x => x.Id == siteId).ToList().FirstOrDefault();

            if (table == null)
                throw new Exception("A valid table could not be found in the database");

            if (site == null)
                throw new Exception("A valid site could not be found in the database");

            //Set the public property
            Site = site;
            Table = table;

            //Set the table prefix
            tablePrefix = ValidationHelper.GetTablePrefix(Table.TableName);

            //Cretae stopwatch
            sw1 = new Stopwatch();

            //Create the results Table
            resultsTable = new DataTable("Results");

            //Set the licence for GemBox
            SpreadsheetInfo.SetLicense("E65I-Y0CT-L0H3-XTH2");
        }

        /// <summary>
        /// Responsible for asyncronusly generating a validation document for users to compare source and converted fields.
        /// Also applies simple rules
        /// </summary>
        public Task<string> ValidateAsync()
        {
            return Task.Run(() =>
                {
                    return Validate();
                });
        }

        /// <summary>
        /// Responsible for generating a validation document for users to compare source and converted fields.
        /// Also applies simple rules
        /// </summary>
        public string Validate()
        {
            OutputMsg("################################################");
            OutputMsg("");
            OutputMsg("Start Validation on Table: " + Table.TableName);

            //Load in the mapping file...
            if (File.Exists(mappingDocPath) == false)
                throw new Exception("The mapping document does not exist. Please try again...");

            mappingDoc = ExcelFile.Load(mappingDocPath);
            mappingWs = mappingDoc.Worksheets.Where(x => x.Name.StartsWith(Table.TableName)).FirstOrDefault();
            if (mappingWs == null)
                throw new Exception("A valid tab was not found in the mapping document fot table " + Table.TableName);
            //Makes sure that there are no frozen panes...
            //if (mappingWs.HasSplitOrFreezePanes)               
            //    throw new Exception("The excel work sheet " + mappingWs.Name + " has frozen panes. Remove the frozen panes to import");                

            DataTable ruleTable = mappingWs.CreateDataTable(new CreateDataTableOptions()
            {
                ColumnHeaders = true,
                StartRow = 0,
                NumberOfRows = mappingWs.Rows.Count - 1,
                Resolution = ColumnTypeResolution.AutoPreferStringCurrentCulture,
                NumberOfColumns = 12,
                ExtractDataOptions = ExtractDataOptions.StopAtFirstEmptyRow
            });
            ValidationHelper.DebugTable(ruleTable);

            //Now that we have the rules available, build the header for the results
            resultsTable.Columns.Add("Type", typeof(string));
            DataRow descRow = resultsTable.Rows.Add();
            //Loop through the rules to build up the column headers
            foreach (DataRow rule in ruleTable.Rows)
            {
                var fieldName = GetValueFromDataRow(rule, 3);
                var fieldDesc = GetValueFromDataRow(rule, 2);

                resultsTable.Columns.Add(fieldName, typeof(string));
                //Strange null error here
                if (fieldName != null)
                    descRow[fieldName] = fieldDesc;
            }

            int sampleCount = Samples.Count;
            if (sampleCount == 0)
                OutputMsg("There is no sample data for table " + Table.TableName);
            //throw new Exception("There is no sample data for table " + Table.TableName);
            int i = 1; //Counter

            //Begin loop of sample data for this table and site
            foreach (Sample sample in Samples)
            {
                //OutputMsg(String.Format("       Processing Sample {0} of {1}", i, Samples.Count));
                OnProgressUpdate(this, sampleCount, i);
                i += 1;
                //We have started a new sample so build a new row in the results table
                var worldResultRow = resultsTable.Rows.Add();
                var ruleResultRow = resultsTable.Rows.Add();
                var e1ResultRow = resultsTable.Rows.Add();
                var resultRow = resultsTable.Rows.Add();
                var blankResultRow = resultsTable.Rows.Add();

                worldResultRow["Type"] = "World";
                ruleResultRow["Type"] = "Rule";
                e1ResultRow["Type"] = "E1";
                resultRow["Type"] = "Result";
                blankResultRow["Type"] = "-";

                //Create a new Dataset to store the erp data
                DataSet erpData = new DataSet("erpData");

                //Download the base record so that we can get the other joint source tables
                //First get the data required for joining the remaining source tables
                //Fetch the defualt E1 table record
                DataTable primarySourceDataTable = new DataTable("Master");
                SourceTable primarySourceTable = Table.SourceTables.Where(s => (s.PrimarySource == true)).FirstOrDefault();
                if (primarySourceTable == null)
                    throw new Exception("An E1 Source Table for " + Table.TableName + " could not be found");

                //Put the data into the table
                //generate the SQL for the primary table
                string prefix = primarySourceTable.TableHeader.FieldPrefix;
                string strSql = string.Empty;
                string whereSql = string.Empty;
                //Build the where statment
                foreach (TableJoin tableJoin in primarySourceTable.TableJoins)
                {
                    //Find the matching UserSampleValue
                    SampleValue sValue = sample.SampleValues.Where(sv => sv.FieldName == prefix + tableJoin.SourceFieldName).FirstOrDefault();
                    if (sValue == null)
                        throw new Exception("The source field name could not find a coresponding sample vlaue of the same name");
                    string fieldValue = sValue.TableValue;

                    //Check to see if the sampleValue type requires a XRef ONLY if the source table is in E1 because all sample data should be World based 
                    if (primarySourceTable.IsE1)
                        fieldValue = CheckForXref(XRefDirection.World_To_E1, tableJoin.SourceFieldName, tableJoin.SourceTableName, fieldValue);
                    whereSql += String.Format("{0}{1} = ''{2}'' AND ", prefix, tableJoin.SourceFieldName, fieldValue.Trim());
                }
                //Build the SQL depending on environment
                if (primarySourceTable.IsE1)
                    strSql = String.Format("SELECT * FROM OPENQUERY({0},'SELECT * FROM {1}.{2} WHERE {3}')", E1Environemt, E1Library, primarySourceTable.SourceTableName, whereSql.TrimLastSqlAND());
                else
                    strSql = String.Format("SELECT * FROM OPENQUERY({0},'SELECT * FROM {1} WHERE {2}')", WorldEnvironemnt, primarySourceTable.SourceTableName, whereSql.TrimLastSqlAND());

                ValidationHelper.FillDataTableFromSQL(ref primarySourceDataTable, strSql);

                //Get the row of data for each source table of the main table
                foreach (SourceTable sTable in Table.SourceTables)
                {
                    strSql = GenerateSQLFromSourceTable(primarySourceDataTable, primarySourceTable, sTable, sample);
                    Debug.WriteLine(strSql);
                    //Download the data into a new DataTable and then add it to the Dataset
                    DataTable dt = new DataTable(sTable.SourceTableName + "_" + ((sTable.IsE1) ? "E1" : "World"));
                    ValidationHelper.FillDataTableFromSQL(ref dt, strSql);
                    erpData.Tables.Add(dt);
                }

                foreach (DataTable dt in erpData.Tables)
                    ValidationHelper.DebugTable(dt);

                foreach (DataRow rule in ruleTable.Rows)
                {
                    //Set the values for this ruleitubes
                    string srcTableName = GetValueFromDataRow(rule, 0);
                    string srcFieldName = GetValueFromDataRow(rule, 1);

                    //If the srcTableName is null then no field has been provide. Assume that the table should be local and match the E1 Alias
                    if (srcTableName == null)
                    {
                        srcTableName = Table.TableName;
                        srcFieldName = GetValueFromDataRow(rule, 3); ; //Set it to use the E1 Alias column
                    }
                    else
                    {
                        //Because the mapping document does not append the prfix to the src field name, we have to add it manually
                        srcFieldName = tablePrefix + srcFieldName;
                    }

                    bool srcE1 = false; //false by default                    
                    //Is this is an e1 srcTable or world?
                    if (srcTableName.EndsWith("E1") || srcTableName.StartsWith("E1"))
                    {
                        srcTableName = srcTableName.Replace("E1", "");
                        srcTableName = srcTableName.Trim();
                        srcE1 = true;
                    }

                    string srcFieldPrefix = ValidationHelper.GetTablePrefix(Table.SourceTables.ToList(), srcTableName);
                    string e1FieldName = GetValueFromDataRow(rule, 3);
                    string e1FieldPrefix = tablePrefix;
                    string ruleName = GetValueFromDataRow(rule, 11);

                    //Get the values from the table
                    string oldValue = string.Empty;
                    string newValue = string.Empty;

                    //Based on the rule decide if we need to fetch the values
                    switch (ruleName.ToUpper())
                    {
                        case "BLANK":
                            {
                                oldValue = NotRequired;
                                newValue = GetValueFromErpDataSet(ref erpData, srcTableName, true, e1FieldName);
                                break;
                            }
                        case "SKIP":
                            {
                                oldValue = NotRequired;
                                newValue = NotRequired;
                                break;
                            }
                        case "MATCH":
                        case "MANUAL":
                        case "UCASE":
                            {
                                oldValue = GetValueFromErpDataSet(ref erpData, Table.TableName, srcE1, srcFieldName);
                                newValue = GetValueFromErpDataSet(ref erpData, Table.TableName, true, e1FieldName);
                                break;
                            }
                        default:
                            {
                                if (ruleName.StartsWith("STRING"))
                                {
                                    oldValue = NotRequired;
                                    newValue = GetValueFromErpDataSet(ref erpData, Table.TableName, true, e1FieldName);
                                }
                                else
                                {
                                    oldValue = GetValueFromErpDataSet(ref erpData, Table.TableName, srcE1, srcFieldName);
                                    newValue = GetValueFromErpDataSet(ref erpData, Table.TableName, true, e1FieldName);
                                }
                                break;
                            }
                    }
                    //Now we have the values. Validate the fields and get the result
                    ValidationRule validationRule = ruleName.ToValidationRule();
                    ValidationResult result = ValidateField(validationRule, ruleName, oldValue, newValue);

                    worldResultRow[e1FieldName] = oldValue;
                    ruleResultRow[e1FieldName] = ruleName;
                    e1ResultRow[e1FieldName] = newValue;
                    resultRow[e1FieldName] = result.ToString();
                }
            }
            //All of the processing is complete, Build the output
            //Make sure that the directory exists
            string rootDir = String.Format(@"\\DC0348\e1\Output\{0}", Site.SiteName);
            string filename = String.Format("{0}-ValidationResuls-{1}.xlsx", Table.TableName, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            string filePath = String.Format(@"{0}\{1}", rootDir, filename);

            if (Directory.Exists(rootDir) == false)
                Directory.CreateDirectory(rootDir);

            OutputMsg("Exporting results to " + filePath);
            OutputMsg("");
            ValidationHelper.ExportValidationResults(ref resultsTable, filePath, Table.TableName);
            return filePath;
        }

        /// <summary>
        /// 
        /// </summary>
        public string GenerateSQLFromSourceTable(DataTable masterSDataTable, SourceTable masterSTable, SourceTable sTable, Sample sample)
        {
            //Get the source table field prefix
            //string prefix = ValidationHelper.GetTablePrefix(sTable.SourceTableName);
            string prefix = sTable.TableHeader.FieldPrefix;
            string strSql = string.Empty;
            string whereSql = string.Empty;

            //Build the where statment
            foreach (TableJoin tableJoin in sTable.TableJoins)
            {
                //Get the value from the masterSTable
                object oValue = masterSDataTable.Rows[0].Field<object>(masterSTable.TableHeader.FieldPrefix + tableJoin.ConvFieldName);
                if (oValue == null)
                    throw new Exception("Could not find");

                string sValue = oValue.ToString();

                //Check to see if the sampleValue type requires a XRef ONLY if the source table is in E1 because all sample data should be World based 
                if (sTable.IsE1)
                    sValue = CheckForXref(XRefDirection.World_To_E1, tableJoin.SourceFieldName, tableJoin.SourceTableName, sValue);

                whereSql += String.Format("{0}{1} = ''{2}'' AND ", prefix, tableJoin.SourceFieldName, sValue.Trim());
            }

            //Build the SQL depending on environment
            if (sTable.IsE1)
                strSql = String.Format("SELECT * FROM OPENQUERY({0},'SELECT * FROM {1}.{2} WHERE {3}')", E1Environemt, E1Library, sTable.SourceTableName, whereSql.TrimLastSqlAND());
            else
                strSql = String.Format("SELECT * FROM OPENQUERY({0},'SELECT * FROM {1} WHERE {2}')", WorldEnvironemnt, sTable.SourceTableName, whereSql.TrimLastSqlAND());

            return strSql;
        }

        /// <summary>
        /// Writes a message to the Message Queue, Debug Sceen and Fires the OnNewMessage Event
        /// </summary>
        private void OutputMsg(string msg)
        {
            Debug.WriteLine(msg);
            MessageOutput.Add(msg);
            OnNewMessage(this, msg);
        }

        /// <summary>
        /// Will look at the new and old values using a given rule and test
        /// </summary>
        private ValidationResult ValidateField(ValidationRule rule, string ruleValue, string oldVlaue, string newValue)
        {
            switch (rule)
            {
                case ValidationRule.BLANK:
                    {
                        if (newValue.Trim() == string.Empty || newValue.Trim() == "" || newValue.Trim() == "0" || newValue.Trim() == "0.0000000")
                            return ValidationResult.Pass;
                        break;
                    }
                case ValidationRule.MATCH:
                    {
                        if (newValue.Equals(oldVlaue))
                            return ValidationResult.Pass;
                        if ((oldVlaue == "0.0000000" && newValue == "0") || (oldVlaue == "0" && newValue == "0.0000000"))
                            return ValidationResult.Pass;
                        break;
                    }
                case ValidationRule.MANUAL:
                    {
                        return ValidationResult.Manual;
                    }
                case ValidationRule.SKIP:
                    {
                        return ValidationResult.Pass;
                    }
                case ValidationRule.STRING:
                    {
                        //Extract the value required
                        string param = ValidationHelper.ExtractFieldName(ruleValue);
                        //Remove surrounding '
                        param = param.Replace("'", "");
                        if (newValue.Equals(param))
                            return ValidationResult.Pass;
                        break;
                    }
                case ValidationRule.UCASE:
                    {
                        if (newValue.Equals(oldVlaue.ToUpper()))
                            return ValidationResult.Pass;
                        break;
                    }
                case ValidationRule.XREF:
                    {   //Dont think this is used
                        break;
                    }
            }
            //If none of these are met. Fail
            return ValidationResult.Fail;
        }

        /// <summary>
        /// Will extract the value required from the ErpDataset
        /// </summary>
        private string GetValueFromErpDataSet(ref DataSet erpData, string tableName, bool srcE1, string fieldName)
        {
            string tableNameExt = String.Format("{0}_{1}", tableName, ((srcE1) ? "E1" : "World"));
            string val = string.Empty;
            DataRow row = null;
            //Look for the correct table in erpData

            DataTable table = erpData.Tables[tableNameExt];
            if (table == null)
                throw new Exception("ErpDataSet did not contain a table named " + tableNameExt);

            //Table has been found
            //Check to see if we have any rows
            if (table.Rows.Count == 0)
                return "*NoRecord*";

            //We have rows
            //Check to see if the fieldname exists
            bool rowExists = true;
            try
            {
                var rowTemp = table.Rows[0][fieldName];
            }
            catch (Exception)
            {
                rowExists = false;
            }

            if (rowExists == false)
                return String.Format("*{0} does not exist*", fieldName);

            row = table.Rows[0]; //Get the first row (there should only be one anyway)
            val = row[fieldName].ToString();

            if (val == null)
                throw new Exception("ErpDataSet found the table named " + tableNameExt + " but could not find a value for the field named " + fieldName);
            return val;

        }

        /// <summary>
        /// Check to see if the combination of fieldname and table name is required to have the indexes x-refenced
        /// </summary>
        /// <param name="fieldName">The Field name to be cross refrenced. MUST NOT HAVE THE PREFIX!</param>
        /// <param name="tableName">Will search to see if there is a table specifi cross reference.</param>
        private string CheckForXref(XRefDirection direction, string fieldName, string tableName, string value)
        {
            //First check to see if there is a table specific Xref
            List<XRef> xRefs = Xrefs.Where(z => z.FieldType == fieldName && z.TableName == tableName).ToList();
            if (xRefs != null)
                return GetXRefFromList(xRefs, direction, fieldName, tableName, value); //We have found a xRef -- narrow it down to the value specific            

            //Now check to see if there is a general xRef available
            xRefs = Xrefs.Where(z => z.FieldType == fieldName).ToList();
            if (xRefs != null)
                return GetXRefFromList(xRefs, direction, fieldName, tableName, value); //We have found a xRef -- narrow it down to the value specific

            //Check for special case
            if (fieldName == "MCU")
            {//This is a generic conversion
                return XrefWorkCenter(value);
            }

            //Nothing was found so just keep the orginial value
            return value;
        }

        /// <summary>
        /// Will get the valid XRef from a given list based on direction and current value
        /// </summary>
        private string GetXRefFromList(List<XRef> xRefs, XRefDirection direction, string fieldName, string tableName, string value)
        {
            var currentValue = value.TrimEnd();
            if (direction == XRefDirection.World_To_E1)
            {
                var newValue = xRefs.Where(z => z.WorldValue == currentValue).FirstOrDefault();
                if (newValue != null)
                    return newValue.E1Value;
            }
            else
            {
                var newValue = xRefs.Where(z => z.E1Value == currentValue).FirstOrDefault();
                if (newValue != null)
                    return newValue.WorldValue;
            }
            return value;
        }

        /// <summary>
        /// Xref the Work Center type 
        /// </summary>
        public string XrefWorkCenter(string WC)
        {
            if (Strings.Trim(WC).StartsWith("2101"))
            {
                WC = Strings.Trim(WC);
                WC = Strings.Right(WC, WC.Length - 4);
                WC = "    2741" + WC;
            }
            return WC;
        }

        /// <summary>
        /// Used to retreive a value from a data row. Get type object and then converts to a string
        /// </summary>
        private string GetValueFromDataRow(DataRow row, int columnIndex)
        {
            var obj = row.Field<object>(columnIndex);
            if (obj == null)
                return null;
            //throw new Exception("Value could not feteched from the Data row provided @ column index " + columnIndex);
            //We have a vlaue so conver it to string and return
            return obj.ToString();
        }

    }
}
