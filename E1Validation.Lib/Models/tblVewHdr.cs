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
    
    public partial class tblVewHdr
    {
        public tblVewHdr()
        {
            this.tblVewFlds = new HashSet<tblVewFld>();
            this.tblVewJons = new HashSet<tblVewJon>();
        }
    
        public string VewId { get; set; }
        public string VewDesc { get; set; }
        public string VewSy { get; set; }
        public string VewSyr { get; set; }
        public Nullable<bool> VewUnion { get; set; }
        public Nullable<bool> VewDistinct { get; set; }
        public string VewPrim { get; set; }
    
        public virtual ICollection<tblVewFld> tblVewFlds { get; set; }
        public virtual ICollection<tblVewJon> tblVewJons { get; set; }
    }
}
