using System;

namespace LW1190
{
   public partial class NotAuthorized : System.Web.UI.Page
   {
      protected void Page_Load(object sender, EventArgs e)
      {
         switch (Request.Url.Query)
         {
            case "?1":
               lblStatus.Text = "No data available";
               break;
            case "?2":
               lblStatus.Text = "No defined parameters";
               break;
            default:
               //<Do nothing>
               break;
         }
      }
   }
}
