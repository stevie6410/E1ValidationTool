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
    
    public partial class SampleTemplate
    {
        public int Id { get; set; }
        public byte[] RowVersion { get; set; }
        public string TableField_TableName { get; set; }
        public string TableField_FieldName { get; set; }
        public int SampleTemplate_Table { get; set; }
    
        public virtual Table Table { get; set; }
        public virtual TableFields TableField { get; set; }
    }
}
