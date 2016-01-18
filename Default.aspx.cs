using LW1190.Components;
using System;
using System.Text.RegularExpressions;

namespace LW1190
{
   public partial class _Default : System.Web.UI.Page
   {
      protected void Page_Load(object sender, EventArgs e)
      {
         string ntID = SessionUtils.CurrentNTIDFromNTLM;
         string spoofers = System.Configuration.ConfigurationManager.AppSettings["SpoofingIDs"].ToString();

         if (Regex.IsMatch(spoofers, string.Format(@"(\b{0}\b)", ntID), RegexOptions.IgnoreCase))
            Response.Redirect("~/Spoofing.aspx", false);
         else
            SessionUtils.RedirectToStartPage(ntID);

      }
   }
}
