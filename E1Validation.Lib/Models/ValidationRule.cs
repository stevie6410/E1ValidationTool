using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E1Validation.Lib.Models
{
    public enum ValidationRule
    {
        BLANK,
        MATCH,
        SKIP,
        UCASE,
        MANUAL,
        XREF,
        STRING,
        Unknown
    }

    public enum ValidationResult
    {
        Pass,
        Fail, 
        Manual
    }

    public enum XRefDirection
    {
        World_To_E1,
        E1_To_World
    }

}
