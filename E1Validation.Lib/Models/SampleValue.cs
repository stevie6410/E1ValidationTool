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
    
    public partial class SampleValue
    {
        public int Id { get; set; }
        public byte[] RowVersion { get; set; }
        public string TableValue { get; set; }
        public string FieldName { get; set; }
        public int SampleValue_Sample { get; set; }
        public Nullable<int> SampleValue_TableJoin { get; set; }
    
        public virtual Sample Sample { get; set; }
        public virtual TableJoin TableJoin { get; set; }
    }
}
