//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E1Validation.Lib.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FieldDetail
    {
        public FieldDetail()
        {
            this.tblTblFlds = new HashSet<TableField>();
            this.tblVewFlds = new HashSet<tblVewFld>();
        }
    
        public string FldAlias { get; set; }
        public string FldDesc { get; set; }
        public Nullable<double> FldLength { get; set; }
        public Nullable<int> FldDecml { get; set; }
        public Nullable<short> FldTyp { get; set; }
        public string FldDataItm { get; set; }
        public string FldSys { get; set; }
        public string FldUDC { get; set; }
        public string FldNN { get; set; }
    
        public virtual FieldType tblFldTyp { get; set; }
        public virtual ICollection<TableField> tblTblFlds { get; set; }
        public virtual ICollection<tblVewFld> tblVewFlds { get; set; }
    }
}
