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
    
    public partial class IndexHeader
    {
        public IndexHeader()
        {
            this.tblIdxFlds = new HashSet<IndexField>();
        }
    
        public string TblId { get; set; }
        public string IdxId { get; set; }
        public Nullable<bool> IdxPrimary { get; set; }
        public Nullable<bool> IdxUnique { get; set; }
        public string IdxDesc { get; set; }
        public Nullable<short> IdxSeq { get; set; }
    
        public virtual ICollection<IndexField> tblIdxFlds { get; set; }
        public virtual TableHeader tblTblHdr { get; set; }
    }
}
