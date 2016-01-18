using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace LW1190.Models
{
   public class SECContext : DbContext
   {
      static SECContext()
      {
         Database.SetInitializer<SECContext>(null);
      }

      public SECContext()
         : base("Name=" + Global.CONN_STR_NAME_ADSI)
      {
         int iTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);

         var ctx = (this as IObjectContextAdapter).ObjectContext;
         ctx.CommandTimeout = iTimeout;
      }

      public string getUserPermission(string ntid)
      {
         string sSQL = "SELECT TOP 1 e.ers_udf1 " +
                       "FROM	 tblSecGroups g " +
                       "JOIN	 ERStblSecurity e  ON g.group_name = e.ers_group_name " +
                       "WHERE  e.ers_reportID IN('LW1190') " +
                       "AND	 g.nt_id = '" + ntid + "' " +
                       "ORDER	 BY e.ers_udf1";
         return this.Database.SqlQuery<string>(sSQL).SingleOrDefault();
      }

   }
}