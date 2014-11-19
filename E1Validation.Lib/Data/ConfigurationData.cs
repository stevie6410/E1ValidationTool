using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E1Validation.Lib.Models;

namespace E1Validation.Lib.Data
{
    public static class ConfigurationData
    {

        public static List<Conversion> GetConversions()
        {
            //Setup the databse conenction
            using (var db = new E1ValidationEntities())
            {
                //Get the Conversions
                return db.Conversions.ToList();
            }
        }

        public static List<Site> GetSites()
        {
            using (var db = new E1ValidationEntities())
            {
                return db.Sites.ToList();
            }
        }

        public static List<Table> GetTablesByConversion(int conversionId)
        {
            using (var db = new E1ValidationEntities())
            {
                return db.Tables.Where(x => x.Conversion.Id == conversionId).ToList();
            }
        }

    }
}
