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
    
    public partial class Conversion
    {
        public Conversion()
        {
            this.Tables = new HashSet<Table>();
        }
    
        public int Id { get; set; }
        public byte[] RowVersion { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Owner { get; set; }
    
        public virtual ICollection<Table> Tables { get; set; }
    }
}
