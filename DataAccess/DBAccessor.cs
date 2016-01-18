using LW1190.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace LW1190.DataAccess
{
   public static class DBAccessor
   {

      public static DataTable GetPeriod()
      {
         using (SQLUtility oUtil = new SQLUtility())
            return oUtil.ExecStoredProc("dbo.uspLW1190_SELECT_Periods");
      }

      public static DataTable GetReportType()
      {
         using (SQLUtility oUtil = new SQLUtility())
            return oUtil.ExecStoredProc("dbo.uspLW1190_SELECT_ReportTypes");
      }

      public static DataTable GetDepartment()
      {
         using (SQLUtility oUtil = new SQLUtility())
            return oUtil.ExecStoredProc("dbo.uspLW1190_SELECT_Depts");
      }

      public static DataTable GetLocation(int iSecLvl)
      {
         using (SQLUtility oUtil = new SQLUtility())
         {
            oUtil.AddParameters("SecLvl", iSecLvl);
            return oUtil.ExecStoredProc("dbo.uspLW1190_SELECT_Locations");
         }
      }

      public static DataTable GetRegion()
      {
         using (SQLUtility oUtil = new SQLUtility())
            return oUtil.ExecStoredProc("dbo.uspLW1190_SELECT_Regions");
      }

      public static DataTable GetGroup(string sPeriod, int iIPAC)
      {
         using (SQLUtility oUtil = new SQLUtility())
         {
            oUtil.AddParameters("Per", sPeriod);
            oUtil.AddParameters("excom", iIPAC);

            return oUtil.ExecStoredProc("dbo.uspLW1190_SELECT_Groups");
         }
      }

      public static DataTable GetTimekeeper(string sGroups, string sPeriod)
      {
         using (SQLUtility oUtil = new SQLUtility())
         {
            oUtil.AddParameters("Groups", sGroups);
            oUtil.AddParameters("Period", sPeriod);

            return oUtil.ExecStoredProc("dbo.uspLW1190_SELECT_Timekeepers");
         }
      }

      public static DataTable GetReportData(string sPeriod, string sDistBy, string sLoc, string sDep, string sGrp, string sTk, int iPBKMFlag, int iSecLvl, int iDepSort = 2)
      {
         using (SQLUtility oUtil = new SQLUtility())
         {
            oUtil.AddParameters("Period", sPeriod);
            oUtil.AddParameters("DistBy", sDistBy);
            oUtil.AddParameters("Loc", sLoc);
            oUtil.AddParameters("Dep", sDep);
            oUtil.AddParameters("Grp", sGrp);
            oUtil.AddParameters("Tk", sTk);
            oUtil.AddParameters("PBKMFlag", iPBKMFlag);
            oUtil.AddParameters("SecLvl", iSecLvl);
            oUtil.AddParameters("DepSort", iDepSort);

            return oUtil.ExecStoredProc("dbo.uspLW1190_SELECT_Data");
         }
      }

      public static string GetDataAsOf(string sPeriod)
      {
         using (SQLUtility oUtil = new SQLUtility())
         {
            oUtil.AddParameters("Period", sPeriod);
            DataTable dt = oUtil.ExecStoredProc("uspLW1190_SELECT_DataAsOf");

            if (dt.Rows.Count > 0)
               return Convert.ToDateTime(dt.Rows[0][0]).ToShortDateString();
            else
               return String.Empty;
         }
      }

      public static void SaveUserInformation(string strUserNetID, string strReportID)
      {
         using (SQLUtility oUtil = new SQLUtility())
         {
            oUtil.AddParameters("UserNetID", strUserNetID);
            oUtil.AddParameters("ReportID", strReportID);
            oUtil.ExecActionStoredProc("dbo.uspReportHistory_INSERT_UserInformation");
         }
      }

      /// Returns the user permission information for a specific ntid
      public static User GetUserPermission(string ntid)
      {
         using (SQLUtility oUtil = new SQLUtility())
         {
            User usr = new User();

            /*
            //The hierarchy is IPAC, ALL, and then A
            string strSQL = "SELECT	TOP 1 g.nt_id, e.ers_udf1, " +
                                    "CASE RTRIM(e.ers_udf1) " +
                                    "  WHEN 'IPAC' THEN 1 " +
                                    "  WHEN 'ALL' THEN 2 " +
                                    "  WHEN 'A' THEN 3 " +
                                    "  ELSE 4 " +
                                    "END AS [sort] " +
                              "FROM	tblSecGroups g " +
                              "JOIN	ERStblSecurity e  ON g.group_name = e.ers_group_name " +
                              "WHERE	e.ers_reportID IN('LW1190') " +
                              "AND		e.ers_udf1 != 'ME' " +
                              "AND	   Nt_Id = '" + ntid + "' " +
                              "ORDER	BY sort";
            */
            string sSQL = "DECLARE @temp NVARCHAR(250) " +
                           "SELECT  @temp = COALESCE(@temp + ',', '') + UPPER(RTRIM(e.ers_udf1)) + ISNULL(e.ers_udf2,'') " +
                           "FROM	   tblSecGroups g " +
                           "JOIN	   ERStblSecurity e  ON g.group_name = e.ers_group_name " +
                           "WHERE	e.ers_reportID = 'LW1190' " +
                           "AND		g.nt_id = '" + ntid + "' " +
                           "ORDER	BY e.ers_udf1, e.ers_udf2 " +
                           "SELECT @temp";

            DataTable dtPermission = oUtil.ExecSQL(sSQL, Global.CONN_STR_NAME_ADSI);
            usr.Access = dtPermission.Rows[0][0].ToString().Trim();

            if (!String.IsNullOrEmpty(usr.Access))
            {
               // *** Obsolete ***
               // • When user is only setup with the “ADMIN” value, they should see the not authorized page.
               // • ADMIN & ME access codes are not used for authorization to LW1190.aspx.
               // usr.IsAuthorized = usr.Access.Split(',').Except(new[] { "ADMIN", "ME" }).Any();
               // ****************

               // Pat: If a user does not have access to the non-ME and non-Portal items, then that user should not have access to the full-blown version of the LW1190
               // [--> You’re defined as Admin if you have any of the following codes: A,ALL,IPAC <--]
               usr.IsAuthorized = usr.Access.Split(',').Intersect(new[] { "A", "ALL", "IPAC" }).Any();

               usr.IsAuthorizedME1 = usr.Access.Split(',').Intersect(new[] { "ME1" }).Any();       //LW1190Grp.aspx
               usr.IsAuthorizedME2 = usr.Access.Split(',').Intersect(new[] { "ME2" }).Any();       //Dis=GROUP
               usr.IsAuthorizedME3 = usr.Access.Split(',').Intersect(new[] { "ME3" }).Any();       //Dis=DEPARTMENT[?Srt=2]
               usr.IsAuthorizedME4 = usr.Access.Split(',').Intersect(new[] { "ME4" }).Any();       //Dis=DEPARTMENT?Srt=1
            }

            return usr;
         }
      }

      public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
      {
         DataTable dtReturn = new DataTable();

         // column names 
         PropertyInfo[] oProps = null;

         if (varlist == null) return dtReturn;

         foreach (T rec in varlist)
         {
            // Use reflection to get property names, to create table, Only first time, others will follow 
            if (oProps == null)
            {
               oProps = ((Type)rec.GetType()).GetProperties();
               foreach (PropertyInfo pi in oProps)
               {
                  Type colType = pi.PropertyType;

                  if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                  {
                     colType = colType.GetGenericArguments()[0];
                  }

                  dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
               }
            }

            DataRow dr = dtReturn.NewRow();

            foreach (PropertyInfo pi in oProps)
            {
               dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
            }

            dtReturn.Rows.Add(dr);
         }
         return dtReturn;
      }

   }
}
