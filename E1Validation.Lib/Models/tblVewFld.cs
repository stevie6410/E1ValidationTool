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
    
    public partial class tblVewFld
    {
        public string VewId { get; set; }
        public short FldPos { get; set; }
        public string TblId { get; set; }
        public string FldAlias { get; set; }
        public Nullable<short> FldIdx { get; set; }
    
        public virtual FieldDetail tblFldDet { get; set; }
        public virtual tblVewHdr tblVewHdr { get; set; }
    }
}
