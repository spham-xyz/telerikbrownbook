using LW1190.Models;
using LW1190.Types;
using System;
using System.Web;


namespace LW1190.Components
{
   public class SessionUtils
   {
      public static string CurrentNTIDFromNTLM
      {
         get
         {
            try
            {
               return System.Web.HttpContext.Current.User.Identity.Name.Replace("LW\\", "").ToUpper().Trim();
            }
            catch (Exception)
            {
               return null;
            }
         }
      }

      public static void RedirectToStartPage(string ntID)
      {
         if (ntID != null)
         {
            string sAccess;

            using (var sec = new SECContext())
               sAccess = sec.getUserPermission(ntID);

            if (String.IsNullOrEmpty(sAccess))
               HttpContext.Current.Response.Redirect("~/NotAuthorized.aspx", false);
            else
               HttpContext.Current.Response.Redirect("~/Pages/LW1190.aspx", false);
         }
         else
         {
            throw new Exception("Could not determine Network ID (ntid from NTLM was NULL)");
         }
      }

   }
}
