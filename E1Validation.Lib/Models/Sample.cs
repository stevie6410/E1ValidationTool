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
    
    public partial class Sample
    {
        public Sample()
        {
            this.SampleValues = new HashSet<SampleValue>();
        }
    
        public int Id { get; set; }
        public byte[] RowVersion { get; set; }
        public string SampleName { get; set; }
        public bool Active { get; set; }
        public int Sample_Table1 { get; set; }
        public int Sample_SourceTable { get; set; }
        public int Site_Sample { get; set; }
    
        public virtual SourceTable SourceTable { get; set; }
        public virtual Table Table { get; set; }
        public virtual Site Site { get; set; }
        public virtual ICollection<SampleValue> SampleValues { get; set; }
    }
}