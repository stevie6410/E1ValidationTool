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
    
    public partial class TableFields
    {
        public TableFields()
        {
            this.UserSampleTemplates = new HashSet<UserSampleTemplate>();
            this.SampleTemplates = new HashSet<SampleTemplate>();
        }
    
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FieldDescription { get; set; }
    
        public virtual ICollection<UserSampleTemplate> UserSampleTemplates { get; set; }
        public virtual ICollection<SampleTemplate> SampleTemplates { get; set; }
    }
}
