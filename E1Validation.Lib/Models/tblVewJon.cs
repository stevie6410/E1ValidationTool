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
    
    public partial class tblVewJon
    {
        public string VewId { get; set; }
        public short JoinId { get; set; }
        public string TblIdLeft { get; set; }
        public string FldAliasLeft { get; set; }
        public string JoinOperator { get; set; }
        public string JoinType { get; set; }
        public string TblIdRight { get; set; }
        public string FldAliasRight { get; set; }
    
        public virtual tblJonOpr tblJonOpr { get; set; }
        public virtual tblJonTyp tblJonTyp { get; set; }
        public virtual tblVewHdr tblVewHdr { get; set; }
    }
}
