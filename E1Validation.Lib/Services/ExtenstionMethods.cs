using E1Validation.Lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E1Validation.Lib.Services
{
    public static class ExtenstionMethods
    {
        public static ValidationRule ToValidationRule(this string ruleValue)
        {
            switch (ruleValue.ToUpper())
            {
                case "BLANK":
                    return ValidationRule.BLANK;
                case "SKIP":
                    return ValidationRule.SKIP;
                case "MATCH":
                    return ValidationRule.MATCH;
                case "MANUAL":
                    return ValidationRule.MANUAL;
                case "UCASE":
                    return ValidationRule.UCASE;
                case "XREF":
                    return ValidationRule.XREF;
            }

            if (ruleValue.ToUpper().StartsWith("STRING"))
                return ValidationRule.STRING;
            else
                return
                ValidationRule.Unknown;
        }
        public static string TrimLastSqlAND(this string sql)
        {
            return sql.Left(sql.Length - 5);
        }

        public static string AppendTimeStamp(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }

        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }
    }


}
