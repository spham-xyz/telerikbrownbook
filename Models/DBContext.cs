using LW1190.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace LW1190.Models
{
   public partial class DBContext : DbContext
   {
      static DBContext()
      {
         Database.SetInitializer<DBContext>(null);
      }

      public DBContext()
         : base("Name=" + Global.CONN_STR_NAME_RPT)
      {
         int iTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);

         var ctx = (this as IObjectContextAdapter).ObjectContext;
         ctx.CommandTimeout = iTimeout;
      }

      public DbSet<Attorney> Attorneys { get; set; }
      public DbSet<Committee> Committees { get; set; }
      public DbSet<CommitteeMaster> CommitteeMasters { get; set; }
      public DbSet<Salary> Salaries { get; set; }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         modelBuilder.Configurations.Add(new AttorneyMap());
         modelBuilder.Configurations.Add(new CommitteeMap());
         modelBuilder.Configurations.Add(new CommitteeMasterMap());
         modelBuilder.Configurations.Add(new SalaryMap());
      }

      public AttyInfo getAtty(string attyId)
      {
         return this.Database.SqlQuery<AttyInfo>("EXEC uspLW1190_AttyInfo @AttyId",
                        new SqlParameter("@AttyId", attyId)).FirstOrDefault();
      }

      public void updCommittee(Committee com, int cnumOld)
      {
         this.Database.ExecuteSqlCommand("EXEC uspLW1190_CRUDCommittee @action, @tkinit, @cnum, @cchair, @cnumOld",
                        new SqlParameter("@action", "U"),
                        new SqlParameter("@tkinit", com.tkinit),
                        new SqlParameter("@cnum", com.cnum),
                        new SqlParameter("@cchair", com.cchair),
                        new SqlParameter("@cnumOld", cnumOld));
      }

      public List<CurrInfo> getCurrencyCodes()
      {
         return this.Database.SqlQuery<CurrInfo>("EXEC uspLW1190_CurrencyCodes").ToList();
      }

      //Only options used: "C" & "U"
      public void modifySalary(string action, Salary sal)
      {
         this.Database.ExecuteSqlCommand("EXEC uspLW1190_CRUDSalary @action, @TimeKeep, @Seq_no, @LocalSalary, @CurrCode, @EffectiveDate, @Comment",
                        new SqlParameter("@action", action),
                        new SqlParameter("@TimeKeep", sal.TimeKeep),
                        new SqlParameter("@Seq_no", sal.Seq_no),
                        new SqlParameter("@LocalSalary", sal.LocalSalary),
                        new SqlParameter("@CurrCode", sal.CurrCode),
                        new SqlParameter("@EffectiveDate", sal.EffectiveDate),
                        new SqlParameter("@Comment", sal.Comment));
      }

      // http://stackoverflow.com/questions/10254272/execute-stored-procedure-in-entity-framework-return-listdatatable-or-dataset
      // http://dyslexicanaboko.blogspot.com/2012/08/entity-framework-executing-sql-strings.html
      public DataTable getExportAttys()
      {
         //var ctx = (this as IObjectContextAdapter).ObjectContext;
         //var sCnn = ((EntityConnection)ctx.Connection).StoreConnection.ConnectionString;      //EF 4.0            
         var sCnn = this.Database.Connection.ConnectionString;

         using (SqlConnection cnn = new SqlConnection(sCnn))
         {
            using (SqlCommand cmd = cnn.CreateCommand())
            {
               cmd.CommandText = "uspLW1190_ExportAttys";
               cmd.CommandType = CommandType.StoredProcedure;

               cnn.Open();
               SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
               DataTable dt = new DataTable();
               dt.Load(dr);

               return dt;
            }
         }
      }

      public void LogUser(string sUsrId, string sRptId)
      {
         this.Database.ExecuteSqlCommand("EXEC dbo.uspReportHistory_INSERT_UserInformation @UserNetID, @ReportID",
                        new SqlParameter("@UserNetID", sUsrId),
                        new SqlParameter("@ReportID", sRptId));
      }
   }
}