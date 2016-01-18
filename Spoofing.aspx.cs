using LW1190.Components;
using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace LW1190
{
   public partial class Spoofing : System.Web.UI.Page
   {
      protected void Page_Load(object sender, EventArgs e)
      {
         string ntID = SessionUtils.CurrentNTIDFromNTLM;
         string spoofers = ConfigurationManager.AppSettings["SpoofingIDs"].ToString().Trim();

         Page.Form.DefaultButton = btnOK.UniqueID;
         Page.SetFocus(txtNTID);

         if (!Regex.IsMatch(spoofers, string.Format(@"(\b{0}\b)", ntID), RegexOptions.IgnoreCase))
         {
            btnOK.Enabled = false;
            txtNTID.Enabled = false;
            lblWarning.Text = "Warning: You do not have permission to use spoofing function.";
         }
      }

      public string Spoofer { get { return txtNTID.Text.Trim(); } }
   }
}
