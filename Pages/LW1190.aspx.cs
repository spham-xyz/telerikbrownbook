using Aspose.Cells;
using LW1190.Components;
using LW1190.Models;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace LW1190.Pages
{
   public partial class LW1190 : System.Web.UI.Page
   {
      string ntID = SessionUtils.CurrentNTIDFromNTLM;

      public override void Validate()
      {
         if (tabAtty.FindTabByText("Personal").Selected && txtFullName.Text.Length > 0)
         {
            rfvDepartDate.Enabled = (rcbDeparture.SelectedValue == "Y");
            //rfvIPLevel.Enabled = (rcbType.SelectedValue == "I" && txtIPLevel.Text.Trim() == "");
         }
         base.Validate();
      }

      protected void Page_Load(object sender, EventArgs e)
      {
         if (!Page.IsPostBack)
         {
            SecurityCheck();
            SetAccess();

            //ClientScriptManager script = Page.ClientScript;
            //if (!script.IsStartupScriptRegistered(this.GetType(), "showDivAtty"))
            //{
            //   script.RegisterClientScriptBlock(this.GetType(), "showDivAtty", "showDivAtty();", false);
            //}            
         }
      }

      private void SecurityCheck()
      {
         string spooferID = null;
         string sAccess;

         if (Page.PreviousPage != null) spooferID = ((Spoofing)Page.PreviousPage).Spoofer;
         string spoofers = ConfigurationManager.AppSettings["SpoofingIDs"].ToString().Trim();

         if (spooferID == null && Regex.IsMatch(spoofers, string.Format(@"(\b{0}\b)", ntID), RegexOptions.IgnoreCase))
            Response.Redirect("~/Spoofing.aspx");
         else
         {
            ntID = spooferID ?? ntID;

            using (var sec = new SECContext())
               sAccess = sec.getUserPermission(ntID);

            if (String.IsNullOrEmpty(sAccess))
               Response.Redirect("~/NotAuthorized.aspx");
            else
               ViewState["Access"] = sAccess;
         }
      }

      private void SetAccess()
      {
         RadTab tabPersonal = tabAtty.FindTabByText("Personal");
         RadPageView pgVw = tabPersonal.PageView;

         switch (ViewState["Access"].ToString())
         {
            case "1":
               SqlAtty.SelectParameters["AttyType"].DefaultValue = " ";
               SqlNewAtty.SelectParameters["AttyType"].DefaultValue = " ";

               #region Disable selected controls: Unit Value, Pre-Fund Compensation, Partner Fund & Bonus
               foreach (var ctl in pgVw.Controls.OfType<RadNumericTextBox>())
               {
                  if ("txtUnitValue,txtPreFundComp,txtPrnFund,txtBonus".IndexOf(ctl.ID) != -1)
                  { 
                     ctl.ReadOnly = true;
                     ctl.BackColor = ColorTranslator.FromHtml("#EFEFEF");
                  }
               }
               #endregion
               break;
            case "2":   //Associates ONLY able to modify Date of Birth, Class, Evaluation Class & Committees
               tabMain.FindTabByText("Committee Master Maintenance").Visible = false;

               #region Disable/hide all Personal controls, except for DOB, Class & Evaluation Class
               foreach (var ctl in pgVw.Controls.OfType<RadTextBox>())
               {
                  ctl.ReadOnly = true;
                  ctl.BackColor = ColorTranslator.FromHtml("#EFEFEF");
               }
               foreach (var ctl in pgVw.Controls.OfType<RadMaskedTextBox>())
               {
                  if ("txtClass,txtEvalClass".IndexOf(ctl.ID) == -1)
                  {
                     ctl.ReadOnly = true;
                     ctl.BackColor = ColorTranslator.FromHtml("#EFEFEF");
                  }
               }
               foreach (var ctl in pgVw.Controls.OfType<RadNumericTextBox>())
               {
                  ctl.ReadOnly = true;
                  ctl.BackColor = ColorTranslator.FromHtml("#EFEFEF");

               }
               foreach (var ctl in pgVw.Controls.OfType<RadComboBox>())
               {
                  ctl.Enabled = false;
                  ctl.BackColor = ColorTranslator.FromHtml("#EFEFEF");
               }
               foreach (var ctl in pgVw.Controls.OfType<RadDatePicker>())
               {
                  if (ctl.ID != "txtDOB")
                  {
                     ctl.Enabled = false;
                     ctl.DateInput.BackColor = ColorTranslator.FromHtml("#EFEFEF");
                     ctl.BackColor = ColorTranslator.FromHtml("#EFEFEF");
                  }
               }
               //foreach (var ctl in pgVw.Controls.OfType<ImageButton>())
               //   ((ImageButton)ctl).Visible = false;
               #endregion

               //tabAtty.FindTabByText("Committee").Visible = false;
               tabAtty.FindTabByText("Salary").Visible = false;
               break;
            case "3":   //Read-only
               SqlAtty.SelectParameters["AttyType"].DefaultValue = " ";
               //SqlNewAtty.SelectParameters["AttyType"].DefaultValue = " ";     //Add new record icon is not shown

               tabMain.FindTabByText("Committee Master Maintenance").Visible = false;

               #region Disable/hide all Personal controls
               foreach (var ctl in pgVw.Controls.OfType<RadTextBox>())
               {
                  ctl.ReadOnly = true;
                  ctl.BackColor = ColorTranslator.FromHtml("#EFEFEF");
               }
               foreach (var ctl in pgVw.Controls.OfType<RadMaskedTextBox>())
               {
                  ctl.ReadOnly = true;
                  ctl.BackColor = ColorTranslator.FromHtml("#EFEFEF");
               }
               foreach (var ctl in pgVw.Controls.OfType<RadNumericTextBox>())
               {
                  ctl.ReadOnly = true;
                  ctl.BackColor = ColorTranslator.FromHtml("#EFEFEF");
               }
               foreach (var ctl in pgVw.Controls.OfType<RadComboBox>())
               {
                  ctl.Enabled = false;
                  ctl.BackColor = ColorTranslator.FromHtml("#EFEFEF");
               }
               foreach (var ctl in pgVw.Controls.OfType<RadDatePicker>())
               {
                  ctl.Enabled = false;
                  ctl.DateInput.BackColor = ColorTranslator.FromHtml("#EFEFEF");
                  ctl.BackColor = ColorTranslator.FromHtml("#EFEFEF");
               }
               //foreach (var ctl in pgVw.Controls.OfType<ImageButton>())
               //   ((ImageButton)ctl).Visible = false;
               #endregion

               grdAtty.MasterTableView.GetColumn("DeleteColumn").Visible = false;
               grdAtty.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;

               grdCommMstr.MasterTableView.GetColumn("EditColumn").Visible = false;
               grdCommMstr.MasterTableView.GetColumn("DeleteColumn").Visible = false;
               grdCommMstr.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;

               grdCommittee.MasterTableView.GetColumn("EditColumn").Visible = false;
               grdCommittee.MasterTableView.GetColumn("DeleteColumn").Visible = false;
               grdCommittee.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;

               grdSalary.MasterTableView.GetColumn("EditColumn").Visible = false;
               grdSalary.MasterTableView.GetColumn("DeleteColumn").Visible = false;
               grdSalary.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
               break;
         }
      }

      //http://www.telerik.com/community/forums/aspnet-ajax/grid/radgrid-clear-filters.aspx
      protected void btnShowAll_Click(object sender, System.Web.UI.ImageClickEventArgs e)
      {
         grdAtty.MasterTableView.FilterExpression = string.Empty;             //"([Country] LIKE \'%Germany%\') "       

         foreach (GridColumn column in grdAtty.MasterTableView.RenderColumns)
         {
            if (column is GridBoundColumn)
            {
               GridBoundColumn boundColumn = column as GridBoundColumn;
               boundColumn.CurrentFilterValue = string.Empty;
            }
         }

         grdAtty.MasterTableView.Rebind();
      }


      #region grdAtty Events
      protected void rbAtty_CheckedChanged(object sender, EventArgs e)
      {
         string sAttyId = string.Empty;

         #region Toggle row selection
         RadioButton selRadioBtn = (sender as RadioButton);

         foreach (GridDataItem dataItem in grdAtty.MasterTableView.Items)
         {
            RadioButton rb = (dataItem.FindControl("rbAtty") as RadioButton);

            if (rb != selRadioBtn)
            {
               rb.Checked = false;
               dataItem.Selected = false;
            }
            else
            {
               //http://www.telerik.com/help/aspnet-ajax/grid-accessing-cells-and-rows.html
               if (selRadioBtn.Checked)
               {
                  sAttyId = dataItem["AttyID"].Text;
                  dataItem.Selected = true;
               }
            }
         }

         //(selRadioBtn.NamingContainer as GridItem).Selected = selRadioBtn.Checked;
         #endregion

         if (!String.IsNullOrEmpty(sAttyId))
         {
            //using (var ctx = new DBContext())
            //   ctx.LogUser(ntID, Global.REPORT_ID);

            BindAttyInfo(sAttyId);
         }
      }

      /* Not needed
      protected void grdAtty_ItemDataBound(object sender, GridItemEventArgs e)
      {
         if (e.Item.IsInEditMode && e.Item is GridEditFormItem)      //FormTemplate
         // OR
         //if (e.Item.IsInEditMode && e.Item is GridEditableItem)
         {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            RadGrid grid = (RadGrid)editedItem.FindControl("grdNewAtty");

            if (e.Item.OwnerTableView.IsItemInserted)
            {
               //This event occurs twice - before & after the actual insertion
               //*** item is about to be inserted ***
               //Populate controls in EditForm Template
               grid.DataBind();
            }
            else
            {
               //item is about to be edited
            }
         }
      }
      */

      protected void grdAtty_ItemCommand(object sender, GridCommandEventArgs e)
      {
         switch (e.CommandName)
         {
            //case RadGrid.InitInsertCommandName:             //"InitInsert"
            case RadGrid.FilterCommandName:
            case RadGrid.PageCommandName:
            case RadGrid.CancelCommandName:
               ResetAttyInfo();
               break;
            case RadGrid.PerformInsertCommandName:          //"PerformInsert"
               //GridDataInsertItem editItem = (GridDataInsertItem)grdAtty.MasterTableView.GetInsertItem();
               //string OrderId = (editItem["OrderID"].Controls[0] as TextBox).Text;
               //string ShipName = (editItem["ShipName"].Controls[0] as TextBox).Text;
               //DateTime OrderDate = Convert.ToDateTime((editItem["OrderDate"].Controls[0] as RadDatePicker).SelectedDate);

               /*
               //Automatic EditForms
               // http://www.telerik.com/community/forums/aspnet-ajax/grid/extracting-values-from-edited-items-in-a-radgrid-editform.aspx
               // http://demos.telerik.com/aspnet-ajax/grid/examples/dataediting/templateformupdate/defaultvb.aspx
               GridEditableItem editItem = e.Item as GridEditableItem;

               RadGrid grid = e.Item.FindControl("grdNewAtty") as RadGrid;

               //When using custom edit forms from template or user control, the ExtractValuesFromitem() method won't work for you, 
               //as it can extract values only from auto-generated grid edit forms
               Hashtable newValues = new Hashtable();
               e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editItem);

               string sID = newValues["AttyID"].ToString();
               */

               GridEditFormInsertItem insItem = e.Item as GridEditFormInsertItem;
               GridTableView vw = insItem.FindControl("grdNewAtty").Controls[1] as GridTableView;

               foreach (GridDataItem item in vw.Items)
               {
                  RadioButton rb = (item["rbNewAttyTemplateColumn"].FindControl("rbNewAtty") as RadioButton);

                  if (rb.Checked)
                  {
                     string sType;
                     switch (item["TitleCode"].Text)
                     {
                        case "0":                        //Type: lw_pdsm.tstype
                        case "1":                        //   E:	Equity			   0-1
                           sType = "E";                  //   I:	Income
                           break;                        //   C:	Counsel			   2
                        case "2":                        //   O:	Of Counsel
                           sType = "C";                  //   A:	Associate		   3
                           break;
                        case "3":
                        default:
                           sType = "A";
                           break;
                     }

                     var atty = new Attorney
                     {
                        tstk = item["AttyID"].Text,
                        tsloc = item["Loc"].Text,     //Not nullable       
                        tsunits = 99999,              //Units
                        tsadjunits = 0,               //Adjusted Value
                        tsunitval = 0,                //Unit Value
                        tsfund = 0,                   //Pre-Fund Compensation
                        tspfund = 0,                  //Partner Fund
                        tsbonus = 0,                  //Bonus
                        tstype = sType,               //Type: Must be 'E','I','C','O','A' to be displayed; see uspLW1190_Attys
                        tstatus = "2",                //Rank
                        tsaudit_op = ntID,            //Modified by
                        LastModified = DateTime.Now   //Last Modifed
                     };

                     using (var ctx = new DBContext())
                     {
                        ctx.Attorneys.Add(atty);
                        ctx.SaveChanges();

                        //ctx.LogUser(ntID, Global.REPORT_ID);
                     }

                     Debug.WriteLine("grdAtty_ItemCommand --> ID:{0}, Location:{1}, Last:{2}, First:{3}, Title:{4}", item["AttyID"].Text, item["Loc"].Text, item["LastName"].Text, item["FirstName"].Text, item["Title"].Text);

                     //http://www.telerik.com/help/aspnet-ajax/grid-operate-with-filter-expression-manually.html
                     //Need to set [grdAtty.EnableLinqExpressions = false;] ELSE have to set 
                     //[grdAtty.MasterTableView.FilterExpression = "(it["Atty ID"].ToString().ToUpper().Contains("04819".ToUpper()))";]

                     //grdAtty.MasterTableView.FilterExpression = "([Atty ID] LIKE \'%" + item["AttyID"].Text + "%\')";
                     grdAtty.MasterTableView.FilterExpression = "([Atty ID] = \'" + item["AttyID"].Text + "\')";        //"([Atty ID] = '04592')"

                     GridColumn column = grdAtty.MasterTableView.GetColumnSafe("AttyID");
                     column.CurrentFilterFunction = GridKnownFunction.EqualTo;
                     column.CurrentFilterValue = item["AttyID"].Text;

                     grdAtty.MasterTableView.Rebind();

                     ResetAttyInfo();
                     break;
                  }
               }

               break;
            case RadGrid.DeleteCommandName:
               GridEditableItem editItem = e.Item as GridEditableItem;
               var pId = editItem.GetDataKeyValue("Atty ID").ToString();
               //OR e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Atty ID"]

               using (var ctx = new DBContext())
               {
                  var atty = ctx.Attorneys.Find(pId);
                  // *** For cascade delete, explicitly load committees & salaries related to a given atty. ***
                  //ctx.Entry(atty).Collection(c => c.Committees).Load();    
                  //ctx.Entry(atty).Collection(s => s.Salaries).Load();
                  ctx.Entry(atty).Collection("Committees").Load();
                  ctx.Entry(atty).Collection("Salaries").Load();

                  ctx.Attorneys.Remove(atty);
                  ctx.SaveChanges();

                  //ctx.LogUser(ntID, Global.REPORT_ID);
               }

               //Clear filters
               foreach (GridColumn col in grdAtty.MasterTableView.Columns)
               {
                  col.CurrentFilterFunction = GridKnownFunction.NoFilter;
                  col.CurrentFilterValue = string.Empty;
               }
               grdAtty.MasterTableView.FilterExpression = string.Empty;
               grdAtty.MasterTableView.Rebind();

               ResetAttyInfo();
               break;
         }
      }

      #endregion

      protected void rbNewAtty_CheckedChanged(object sender, EventArgs e)
      {
         RadioButton radio = (sender as RadioButton);

         //http://www.telerik.com/help/aspnet-ajax/grid-structure-overview.html
         GridTableView view = radio.NamingContainer.NamingContainer as GridTableView;

         foreach (GridDataItem item in view.Items)
         {
            RadioButton rb = (item.FindControl("rbNewAtty") as RadioButton);

            if (rb != radio)
            {
               rb.Checked = false;
               item.Selected = false;
            }
            else
            {
               if (radio.Checked)
               {
                  RadGrid grd = radio.NamingContainer.NamingContainer.NamingContainer as RadGrid;     //grdNewAtty
                  GridEditFormInsertItem insItem = grd.NamingContainer as GridEditFormInsertItem;
                  ImageButton imgSave = insItem.FindControl("btnAttySave") as ImageButton;
                  if (imgSave != null)
                     imgSave.Visible = true;

                  item.Selected = true;
                  //string sId = item["AttyID"].Text;      //AttyID (not "Atty ID") --> GridBoundColumn UniqueName
                  //Debug.WriteLine("ID:{0}, Last:{1}, First:{2}, Title:{3}", item["AttyID"].Text, item["LastName"].Text, item["FirstName"].Text, item["Title"].Text);
               }
            }
         }

         //(radio.NamingContainer as GridItem).Selected = radio.Checked;
      }

      protected void btnDtlUpdate_Click(object sender, System.Web.UI.ImageClickEventArgs e)
      {
         //http://stackoverflow.com/questions/12879198/how-can-i-update-a-detached-entity-in-ef-cf-multi-tier-application
         //context.Entry(user).Property(u => u.Body).IsModified = true;
         //DataContext.Entry(entity).State = System.Data.EntityState.Modified;
         //http://msdn.microsoft.com/en-us/data/jj592676.aspx

         if (Page.IsValid)
         {
            RadTab tab = tabAtty.FindTabByText("Personal");
            RadPageView pgVw = tab.PageView;

            string id = ((RadTextBox)pgVw.FindControl("txtAttyID")).Text;

            if (!String.IsNullOrEmpty(id))
            {
               using (var ctx = new DBContext())
               {
                  var atty = ctx.Attorneys.Find(id);

                  atty.tsaudit_op = ntID;
                  atty.LastModified = DateTime.Now;
                  //atty.ProperCaseName = ((RadTextBox)pgVw.FindControl("txtFullName")).Text;
                  //NO need to update location code: tsloc
                  //atty.tspstd = ((RadNumericTextBox)pgVw.FindControl("txtStdPrnRate")).Value;          //Std Partner Rate
                  atty.tsunits = ((RadNumericTextBox)pgVw.FindControl("txtUnits")).Value;                //Units
                  atty.tsbonus = ((RadNumericTextBox)pgVw.FindControl("txtBonus")).Value;                //Bonus

                  atty.tsdeparture = ((RadComboBox)pgVw.FindControl("rcbDeparture")).SelectedValue;      //Departure?
                  atty.tsadjunits = ((RadNumericTextBox)pgVw.FindControl("txtAdjUnits")).Value;          //Adjusted Units
                  atty.tsdob = ((RadDatePicker)pgVw.FindControl("txtDOB")).DateInput.SelectedDate;       //Date of Birth
                  atty.tsdepartdt = (atty.tsdeparture == "Y" ?                                           //Departure Date
                        ((RadDatePicker)pgVw.FindControl("txtDepartDate")).DateInput.SelectedDate : null);
                  atty.tsunitval = ((RadNumericTextBox)pgVw.FindControl("txtUnitValue")).Value;          //Unit Value
                  atty.tstype = ((RadComboBox)pgVw.FindControl("rcbType")).SelectedValue;                //Type
                  if (atty.tstype == "I")                                                                //IP Level - Allowable values: 1-8, but no validation
                  {                                                                                      //[MinValue="1"; MaxValue="8"]
                     int n = 1;
                     if (int.TryParse(((RadNumericTextBox)pgVw.FindControl("txtIPLevel")).Text, out n))
                        atty.IPLevel = n;
                  }
                  else
                  {
                     atty.IPLevel = null;
                  }
                  atty.tsclass = ((RadMaskedTextBox)pgVw.FindControl("txtClass")).Text.Trim();           //Class
                  atty.tsfund = ((RadNumericTextBox)pgVw.FindControl("txtPreFundComp")).Value;           //Pre-Fund Compensation
                  atty.tstatus = ((RadComboBox)pgVw.FindControl("rcbRank")).SelectedValue;               //Rank
                  atty.tsevalclass = ((RadMaskedTextBox)pgVw.FindControl("txtEvalClass")).Text.Trim();   //Evaluation Class
                  atty.tspfund = ((RadNumericTextBox)pgVw.FindControl("txtPrnFund")).Value;              //Partner Fund
                  //atty.tsppgroup = ((RadTextBox)pgVw.FindControl("txtPrinPracGroup")).Text;            //Principal Practice Group [Read-only]
                  //atty.tsepadmindt = ((RadDatePicker)pgVw.FindControl("txtAdminDate")).DateInput.SelectedDate;   //Equity Partner Admission Date [Read-only]
                  //atty.tsBarState = ((RadTextBox)pgVw.FindControl("txtBar")).Text;                     //Bar [Read-only]
                  atty.tscomment = ((RadTextBox)pgVw.FindControl("txtComment")).Text;                    //Comment
                  ctx.SaveChanges();

                  //ctx.LogUser(ntID, Global.REPORT_ID);

                  RadWindowManager1.RadAlert("Personal info updated", 225, 125, "Personal", "");

                  //*** Joanne Joe's request: Upon saving the record, please clear the screen of data ***
                  ResetAttyInfo();
                  //    1. Afterward, clear attorney option selected
                  foreach (GridDataItem dataItem in grdAtty.MasterTableView.Items)
                  {
                     RadioButton rb = (dataItem.FindControl("rbAtty") as RadioButton);
                     rb.Checked = false;
                     dataItem.Selected = false;
                  }
               }
            }
         }
      }

      protected void btnDtlCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
      {
         RadTab tab = tabAtty.FindTabByText("Personal");
         RadPageView pgVw = tab.PageView;

         string id = ((RadTextBox)pgVw.FindControl("txtAttyID")).Text;

         if (!String.IsNullOrEmpty(id))
         {
            valType.ErrorMessage = string.Empty;
            BindAttyInfo(id);
         }
      }

      private void BindAttyInfo(string sAttnId)
      {
         #region Personal
         AttyInfo atty;
         using (var ctx = new DBContext())
         {
            atty = ctx.getAtty(sAttnId);
         }

         RadTab tab = tabAtty.FindTabByText("Personal");
         RadPageView pgVw = tab.PageView;

         ((RadTextBox)pgVw.FindControl("txtAttyID")).Text = atty.tstk;                             //Atty Id
         ((RadDatePicker)pgVw.FindControl("txtLastModified")).SelectedDate = atty.LastModified;    //Last Modified
         ((RadTextBox)pgVw.FindControl("txtModifyBy")).Text = atty.tsaudit_op;                     //Modify by
         ((RadTextBox)pgVw.FindControl("txtFullName")).Text = atty.FullName;                       //Full Name
         ((RadTextBox)pgVw.FindControl("txtLocation")).Text = atty.LocDesc;                        //Location description
         //((RadNumericTextBox)pgVw.FindControl("txtStdPrnRate")).Value = atty.tspstd;             //Std Partner Rate
         ((RadNumericTextBox)pgVw.FindControl("txtUnits")).Value = atty.tsunits;                   //Units
         ((RadNumericTextBox)pgVw.FindControl("txtBonus")).Value = atty.tsbonus;                   //Bonus
         ((RadComboBox)pgVw.FindControl("rcbDeparture")).SelectedValue =                           //Departure?
            (atty.tsdeparture ?? string.Empty).Trim();
         ((RadNumericTextBox)pgVw.FindControl("txtAdjUnits")).Value = atty.tsadjunits;             //Adjusted Units
         ((RadDatePicker)pgVw.FindControl("txtDOB")).SelectedDate = atty.tsdob;                    //Date of Birth
         ((RadDatePicker)pgVw.FindControl("txtDepartDate")).SelectedDate = atty.tsdepartdt;        //Departure Date
         ((RadNumericTextBox)pgVw.FindControl("txtUnitValue")).Value = atty.tsunitval;             //Unit Value
         ((RadComboBox)pgVw.FindControl("rcbType")).SelectedValue =                                //Type
            (atty.tstype ?? string.Empty).Trim();
         ((RadMaskedTextBox)pgVw.FindControl("txtClass")).Text = atty.tsclass;                     //Class

         RadNumericTextBox txtIPLvl = (RadNumericTextBox)pgVw.FindControl("txtIPLevel");           //IP Level
         Label lblIPLvl = (Label)pgVw.FindControl("lblIPLevel");
         if (atty.tstype.Trim() == "I")
         {
            //lblIPLvl.Visible =
            //txtIPLvl.Visible = true;
            lblIPLevel.Style["display"] =
            txtIPLevel.Style["display"] = "block";
            txtIPLvl.Value = atty.IPLevel;
         }
         else
         {
            //lblIPLvl.Visible =
            //txtIPLvl.Visible = false;
            lblIPLevel.Style["display"] =
            txtIPLevel.Style["display"] = "none";
         }
         ((RadNumericTextBox)pgVw.FindControl("txtPreFundComp")).Value = atty.tsfund;              //Pre-Fund Compensation
         ((RadComboBox)pgVw.FindControl("rcbRank")).SelectedValue =                                //Rank
            (atty.tstatus ?? string.Empty).Trim();
         //((RadMaskedTextBox)pgVw.FindControl("txtEvalClass")).Text = atty.tsevalclass;           //Evaluation Class, where A=Associate
         if (atty.tstype.Trim() == "A")
         {
            lblEvalClass.Style["display"] =
            txtEvalClass.Style["display"] = "block";
            txtEvalClass.Text = atty.tsevalclass;
         }
         else
         {
            lblEvalClass.Style["display"] =
            txtEvalClass.Style["display"] = "none";
         }
         ((RadNumericTextBox)pgVw.FindControl("txtPrnFund")).Value = atty.tspfund;                 //Partner Fund
         ((RadTextBox)pgVw.FindControl("txtPrinPracGroup")).Text = atty.tsppgroup;                 //Principal Practice Group
         //((RadDatePicker)pgVw.FindControl("txtAdminDate")).SelectedDate = atty.tsepadmindt;      //Equity Partner Admission Date, where E=Equity
         if (atty.tstype.Trim() == "E")
         {
            lblAdminDate.Style["display"] =
            txtAdminDate.Style["display"] = "block";
            txtAdminDate.SelectedDate = atty.tsepadmindt;
         }
         else
         {
            lblAdminDate.Style["display"] =
            txtAdminDate.Style["display"] = "none";
         }
         ((RadTextBox)pgVw.FindControl("txtBar")).Text = atty.tsBarState;                          //Bar
         ((RadTextBox)pgVw.FindControl("txtComment")).Text = atty.tscomment;                       //Comment

         //Only levels 1 & 2 are allowed to edit
         switch (ViewState["Access"].ToString())
         {
            case "1":      //Edit all attys
               txtUnits.Focus();    //Give focus to 1st editable field 

               foreach (var ctl in pgVw.Controls.OfType<ImageButton>())
                  ((ImageButton)ctl).Visible = true;

               grdCommittee.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Bottom;
               grdSalary.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Bottom;
               break;
            case "2":      //Edit associates only 
               txtDOB.Focus();

               foreach (var ctl in pgVw.Controls.OfType<ImageButton>())
                  ((ImageButton)ctl).Visible = true;

               grdCommittee.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Bottom;
               //grdSalary.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.Bottom;
               break;
         }
         #endregion

         SqlCommittee.SelectParameters["tkinit"].DefaultValue = sAttnId;
         SqlSalary.SelectParameters["TimeKeep"].DefaultValue = sAttnId;

         //If grids were left in "uncommitted" state, 
         //RequiredFieldValidation errors on grdSalary on subsequent atty retrievals
         grdCommittee.MasterTableView.IsItemInserted = false;     
         grdCommittee.Rebind();
         //grdSalary.MasterTableView.ClearEditItems();
         grdSalary.MasterTableView.IsItemInserted = false;
         grdSalary.Rebind();

         //Show "Personal" tab
         tabAtty.SelectedIndex = 0;
         PagePersonal.Selected = true;
      }

      private void ResetAttyInfo()
      {
         #region Personal
         String strDummy = String.Empty;
         Double? dblDummy = null;
         DateTime? dteDummy = null;
         RadTab tab = tabAtty.FindTabByText("Personal");
         RadPageView pgVw = tab.PageView;

         #region Long & tedious way
         /*
         ((RadTextBox)pgVw.FindControl("txtAttyID")).Text = strDummy;                              //Atty Id
         ((RadTextBox)pgVw.FindControl("txtModifyBy")).Text = strDummy;                            //Modify by
         ((RadTextBox)pgVw.FindControl("txtFullName")).Text = strDummy;                            //Full Name
         ((RadTextBox)pgVw.FindControl("txtLocation")).Text = strDummy;                            //Location description
         //((RadNumericTextBox)pgVw.FindControl("txtStdPrnRate")).Value = atty.tspstd;             //Std Partner Rate
         ((RadNumericTextBox)pgVw.FindControl("txtUnits")).Value = dblDummy;                       //Units
         ((RadNumericTextBox)pgVw.FindControl("txtBonus")).Value = dblDummy;                       //Bonus
         ((RadComboBox)pgVw.FindControl("rcbDeparture")).SelectedValue = strDummy;                 //Departure?
         ((RadNumericTextBox)pgVw.FindControl("txtAdjUnits")).Value = dblDummy;                    //Adjusted Units

         ((RadDatePicker)pgVw.FindControl("txtDOB")).SelectedDate = dteDummy;                      //Date of Birth
         ((RadDatePicker)pgVw.FindControl("txtDepartDate")).SelectedDate = dteDummy;               //Departure Date
         ((RadNumericTextBox)pgVw.FindControl("txtUnitValue")).Value = dblDummy;                   //Unit Value
         ((RadComboBox)pgVw.FindControl("rcbType")).SelectedValue = strDummy;                      //Type
         ((RadMaskedTextBox)pgVw.FindControl("txtClass")).Text = strDummy;                         //Class

         RadNumericTextBox txtIPLvl = (RadNumericTextBox)pgVw.FindControl("txtIPLevel");           //IP Level
         Label lblIPLvl = (Label)pgVw.FindControl("lblIPLevel");
         txtIPLvl.Value = dblDummy;
         lblIPLevel.Style["display"] =
         txtIPLevel.Style["display"] = "none";

         ((RadNumericTextBox)pgVw.FindControl("txtPreFundComp")).Value = dblDummy;                 //Pre-Fund Compensation
         ((RadComboBox)pgVw.FindControl("rcbRank")).SelectedValue = strDummy;                      //Rank
         ((RadMaskedTextBox)pgVw.FindControl("txtEvalClass")).Text = strDummy;                     //Evaluation Class
         ((RadNumericTextBox)pgVw.FindControl("txtPrnFund")).Value = dblDummy;                     //Partner Fund
         ((RadTextBox)pgVw.FindControl("txtPrinPracGroup")).Text = strDummy;                       //Principal Practice Group
         ((RadDatePicker)pgVw.FindControl("txtAdminDate")).SelectedDate = dteDummy;                //Equity Partner Admission Date                                //Equity Partner Admission Date
         ((RadTextBox)pgVw.FindControl("txtBar")).Text = strDummy;                                 //Bar
         ((RadTextBox)pgVw.FindControl("txtComment")).Text = strDummy;                            //Comment
         */
         #endregion

         foreach (var ctl in pgVw.Controls.OfType<RadTextBox>())
            ctl.Text = strDummy;
         foreach (var ctl in pgVw.Controls.OfType<RadMaskedTextBox>())
            ctl.Text = strDummy;
         foreach (var ctl in pgVw.Controls.OfType<RadNumericTextBox>())
            ctl.Value = dblDummy;
         foreach (var ctl in pgVw.Controls.OfType<RadComboBox>())
            ctl.SelectedValue = strDummy;
         foreach (var ctl in pgVw.Controls.OfType<RadDatePicker>())
            ctl.SelectedDate = dteDummy;
         foreach (var ctl in pgVw.Controls.OfType<ImageButton>())
            ctl.Visible = false;

         RadNumericTextBox txtIPLvl = (RadNumericTextBox)pgVw.FindControl("txtIPLevel");           //IP Level
         Label lblIPLvl = (Label)pgVw.FindControl("lblIPLevel");
         lblIPLevel.Style["display"] =
         txtIPLevel.Style["display"] = "none";
         #endregion

         SqlCommittee.SelectParameters["action"].DefaultValue = "R";
         SqlCommittee.SelectParameters["tkinit"].DefaultValue = "";
         grdCommittee.Rebind();
         SqlSalary.SelectParameters["action"].DefaultValue = "R";
         SqlSalary.SelectParameters["TimeKeep"].DefaultValue = "";
         grdSalary.Rebind();

         //Show "Personal" tab
         tabAtty.SelectedIndex = 0;
         PagePersonal.Selected = true;

         //Reset(disable) "Departure Date" validation
         rfvDepartDate.Enabled = false;
      }

      protected void grdCommittee_ItemCommand(object sender, GridCommandEventArgs e)
      {
         string id;

         #region Iff all fields are bound by GridBoundColumn
         /*
         if (e.Item.IsInEditMode && e.Item is GridEditableItem)
         {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            //Prepare new dictionary object
            Hashtable newValues = new Hashtable();
            e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

            int newNum = int.Parse(newValues["cnum"].ToString());
            string newChair = newValues["cchair"].ToString();

            switch (e.CommandName)
            {
               case RadGrid.PerformInsertCommandName:
                  RadTab tab = tabAtty.FindTabByText("Personal");
                  RadPageView pgVw = tab.PageView;

                  id = ((RadTextBox)pgVw.FindControl("txtAttyID")).Text;

                  using (var ctx = new DBContext())
                  {
                     var com = new Committee { tkinit = id, cnum = newNum, cchair = newChair };
                     ctx.Committees.Add(com);
                     ctx.SaveChanges();
                  }
                  break;
               case RadGrid.UpdateCommandName:
                  //editedItem.SavedOldValues: dictionary holds the old values
                  id = editedItem.SavedOldValues["tkinit"].ToString();
                  int oldNum = int.Parse(editedItem.SavedOldValues["cnum"].ToString());

                  var comm = new Committee { tkinit = id, cnum = newNum, cchair = newChair };
                  using (var ctx = new DBContext())
                  {
                     ////*** Notes:  You cannot update the primary key through EF. ***
                     //// A referential integrity constraint violation occurred: A primary key property that is a part of 
                     //// referential integrity constraint cannot be changed when the dependent object is Unchanged 
                     //// unless it is being set to the association's principal object. 
                     //// The principal object must be tracked and not marked for deletion.
                     //var com = ctx.Committees.Find(id, oldNum);
                     //var comMas = ctx.CommitteeMasters.Find(newNum);
                     ////com.cnum = newNum;
                     ////com.cnum = comMas.cnum;
                     //com.CommitteeMaster = comMas;
                     //com.cchair = newChair;
                     //ctx.SaveChanges();

                     ctx.updateCom(comm, oldNum);
                  }
                  break;
               //case RadGrid.DeleteCommandName:        //AllowAutomaticDeletes="true"             
               //   break;
            }
         }
         */
         #endregion

         if (e.Item.IsInEditMode && e.Item is GridEditableItem)
         {
            RadComboBox cbChair = (RadComboBox)e.Item.FindControl("rcbChairEdit");
            RadComboBox cbCode = (RadComboBox)e.Item.FindControl("rcbComCodeEdit");

            int newNum = int.Parse(cbCode.SelectedValue);
            string newChair = cbChair.SelectedValue;

            switch (e.CommandName)
            {
               case RadGrid.PerformInsertCommandName:
                  //e.Item as GridDataInsertItem
                  RadTab tab = tabAtty.FindTabByText("Personal");
                  RadPageView pgVw = tab.PageView;

                  id = ((RadTextBox)pgVw.FindControl("txtAttyID")).Text;

                  using (var ctx = new DBContext())
                  {
                     var com = new Committee { tkinit = id, cnum = newNum, cchair = newChair };
                     ctx.Committees.Add(com);
                     ctx.SaveChanges();
                  }

                  break;
               case RadGrid.UpdateCommandName:
                  GridEditableItem editedItem = e.Item as GridEditableItem;

                  //editedItem.SavedOldValues: dictionary holds the old values
                  id = editedItem.SavedOldValues["tkinit"].ToString();
                  int oldNum = int.Parse(editedItem.SavedOldValues["cnum"].ToString());

                  var comm = new Committee { tkinit = id, cnum = newNum, cchair = newChair };
                  using (var ctx = new DBContext())
                     ctx.updCommittee(comm, oldNum);
                  break;
               //case RadGrid.DeleteCommandName:        //AllowAutomaticDeletes="true"             
               //   break;
            }

         }
      }

      protected void grdCommittee_ItemDataBound(object sender, GridItemEventArgs e)
      {
         //if (e.Item.IsInEditMode && e.Item is GridEditFormItem)       //FormTemplate
         if (e.Item.IsInEditMode && e.Item is GridEditableItem)
         {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            RadComboBox cbChair = (RadComboBox)editedItem.FindControl("rcbChairEdit");
            //cbChair.Width = Unit.Pixel(50);      //--> Set in designer
            RadComboBox cbCode = (RadComboBox)editedItem.FindControl("rcbComCodeEdit");
            //cbCode.Width = Unit.Pixel(375);      //--> Set in designer

            using (var ctx = new DBContext())
               cbCode.DataSource = ctx.CommitteeMasters
                                       .OrderBy(x => x.ccode)
                                       .ThenBy(y => y.cdescription)
                                       .Select(p => new
                                       {
                                          ccode = p.ccode.Trim(),
                                          cdescription = p.cdescription.Trim(),
                                          p.cnum
                                       }).ToList();
            cbCode.DataValueField = "cnum";
            cbCode.DataTextField = "ccode";
            cbCode.DataBind();

            if (e.Item.OwnerTableView.IsItemInserted)
            {
               //item is about to be inserted
            }
            else
            {
               //item is about to be edited
               //combo.SelectedValue = ((lw740_bankrate)editedItem.DataItem).Client.ToString();
               DataRowView dataView = editedItem.DataItem as DataRowView;
               DataRow row = dataView.Row;

               cbChair.SelectedValue = row["cchair"].ToString();
               cbCode.SelectedValue = row["cnum"].ToString();
            }
         }

      }

      protected void rcbType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
      {
         /*
         RadTab tab = tabAtty.FindTabByText("Personal");
         RadPageView pgVw = tab.PageView;

         RadNumericTextBox txtIPLvl = (RadNumericTextBox)pgVw.FindControl("txtIPLevel");           //IP Level
         Label lblIPLvl = (Label)pgVw.FindControl("lblIPLevel");

         // *** Visibility of txtIPlevel doesn't work correctly using client-side code: disappear on hovering over it. ***
         if (e.Value == "I")
         {
            lblIPLevel.Style["display"] =
            txtIPLevel.Style["display"] = "block";
            txtIPLevel.Text = string.Empty;
         }
         else
         {
            lblIPLevel.Style["display"] =
            txtIPLevel.Style["display"] = "none";
         }
         */
         
         // Initialize
         lblIPLevel.Style["display"] =
         txtIPLevel.Style["display"] = "none";
         lblEvalClass.Style["display"] =
         txtEvalClass.Style["display"] = "none";
         lblAdminDate.Style["display"] =
         txtAdminDate.Style["display"] = "none";

         switch (e.Value)
         {
            case "A":      //Associate
               lblEvalClass.Style["display"] =
               txtEvalClass.Style["display"] = "block";
               txtEvalClass.Text = string.Empty;
               break;
            case "E":      //Equity
               lblAdminDate.Style["display"] =
               txtAdminDate.Style["display"] = "block";
               txtAdminDate.SelectedDate = null;
               break;
            case "I":      //Income
               lblIPLevel.Style["display"] =
               txtIPLevel.Style["display"] = "block";
               txtIPLevel.Text = string.Empty;
               break;
         }
      }

      protected void grdSalary_ItemDataBound(object sender, GridItemEventArgs e)
      {
         if (e.Item.IsInEditMode && e.Item is GridEditableItem)
         {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            RadComboBox cbCCY = (RadComboBox)editedItem.FindControl("rcbCurrCodeEdit");
            //cbCCY.Width = Unit.Pixel(220);
            RadNumericTextBox tbxSalary = (RadNumericTextBox)editedItem["LocalSalary"].Controls[0];
            tbxSalary.Width = Unit.Pixel(85);
            RadDatePicker dteEff = (RadDatePicker)editedItem["EffectiveDate"].Controls[0];
            dteEff.Width = Unit.Pixel(100);

            using (var ctx = new DBContext())
               cbCCY.DataSource = ctx.getCurrencyCodes();
            cbCCY.DataValueField = "curcode";
            cbCCY.DataTextField = "curcode";
            cbCCY.DataBind();

            if (e.Item.OwnerTableView.IsItemInserted)
            {
               //item is about to be inserted
            }
            else
            {
               //item is about to be edited
               //cbCCY.SelectedValue = ((CurrInfo)editedItem.DataItem).curcode.ToString();

               DataRowView dataView = editedItem.DataItem as DataRowView;
               DataRow row = dataView.Row;
               cbCCY.SelectedValue = row["CurrCode"].ToString();
            }
         }
      }

      protected void grdSalary_ItemCommand(object sender, GridCommandEventArgs e)
      {
         string id, code, comment;
         int seq;
         decimal salary;
         DateTime dteEff;

         if (e.Item.IsInEditMode && e.Item is GridEditableItem)
         {
            RadComboBox cbCCY = (RadComboBox)e.Item.FindControl("rcbCurrCodeEdit");
            code = cbCCY.SelectedValue;

            //Prepare new dictionary object
            Hashtable newValues = new Hashtable();

            switch (e.CommandName)
            {
               case RadGrid.PerformInsertCommandName:
                  if (Page.IsValid)       //Check required fields are populated:  Salary & Eff. Date    
                  {
                     GridDataInsertItem item = e.Item as GridDataInsertItem;

                     item.ExtractValues(newValues);

                     RadTab tab = tabAtty.FindTabByText("Personal");
                     RadPageView pgVw = tab.PageView;

                     id = ((RadTextBox)pgVw.FindControl("txtAttyID")).Text;
                     seq = 0;    //value will be determined by stored proc
                     salary = Decimal.Parse(newValues["LocalSalary"].ToString());
                     dteEff = DateTime.Parse(newValues["EffectiveDate"].ToString());
                     comment = (newValues["Comment"] == null ? String.Empty : newValues["Comment"].ToString());

                     var insSal = new Salary { TimeKeep = id, Seq_no = seq, LocalSalary = salary, CurrCode = code, EffectiveDate = dteEff, Comment = comment };

                     using (var ctx = new DBContext())
                        ctx.modifySalary("C", insSal);
                  }
                  break;
               case RadGrid.UpdateCommandName:
                  GridEditableItem editedItem = e.Item as GridEditableItem;

                  //editedItem.SavedOldValues: dictionary holds the old values
                  e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                  id = newValues["TimeKeep"].ToString();
                  seq = int.Parse(newValues["Seq_no"].ToString());
                  salary = Decimal.Parse(newValues["LocalSalary"].ToString());
                  dteEff = DateTime.Parse(newValues["EffectiveDate"].ToString());
                  comment = (newValues["Comment"] == null ? String.Empty : newValues["Comment"].ToString());

                  var updSal = new Salary { TimeKeep = id, Seq_no = seq, LocalSalary = salary, CurrCode = code, EffectiveDate = dteEff, Comment = comment };

                  using (var ctx = new DBContext())
                     ctx.modifySalary("U", updSal);
                  break;
               //case RadGrid.DeleteCommandName:        //AllowAutomaticDeletes="true"             
               //   break;
            }
         }
      }

      protected void grdNewAtty_ItemCommand(object sender, GridCommandEventArgs e)
      {
         switch (e.CommandName)
         {
            case RadGrid.PageCommandName:
               GridEditFormInsertItem insItem = ((RadGrid)sender).NamingContainer as GridEditFormInsertItem;
               ImageButton imgSave = insItem.FindControl("btnAttySave") as ImageButton;
               if (imgSave != null)
                  imgSave.Visible = false;
               break;
         }
      }

      protected void grdCommMstr_ItemCommand(object sender, GridCommandEventArgs e)
      {
         switch (e.CommandName)
         {
            case RadGrid.PerformInsertCommandName:
            case RadGrid.UpdateCommandName:
            case RadGrid.DeleteCommandName:
               using (var ctx = new DBContext())
                  //ctx.LogUser(ntID, Global.REPORT_ID);
               break;
         }
      }

      protected void grdSalary_ItemCreated(object sender, GridItemEventArgs e)
      {
         if (e.Item is GridEditableItem && e.Item.IsInEditMode)
         {
            GridEditableItem item = e.Item as GridEditableItem;
            RequiredFieldValidator val = new RequiredFieldValidator();

            GridNumericColumnEditor salary = item.EditManager.GetColumnEditor("LocalSalary") as GridNumericColumnEditor;
            TableCell cell = (TableCell)salary.NumericTextBox.Parent;
            val.ControlToValidate = salary.NumericTextBox.ID;
            val.ErrorMessage = "Salary";
            val.Text = "*Required";
            cell.Controls.Add(val);

            GridDateTimeColumnEditor dteEff = item.EditManager.GetColumnEditor("EffectiveDate") as GridDateTimeColumnEditor;
            TableCell parent = (TableCell)dteEff.PickerControl.Parent;
            val = new RequiredFieldValidator();
            val.ControlToValidate = dteEff.PickerControl.ID;
            val.ErrorMessage = "Effective Date";
            val.Text = "*Required";
            parent.Controls.Add(val);
         }
      }
      /*
      protected void rcbDeparture_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
      {

         if (e.Value == "N")
         {
            //txtDepartDate.Enabled = false;
            txtDepartDate.SelectedDate = null;
         }
         //else
         //   txtDepartDate.Enabled = true;
      }
      */
      protected void valType_ServerValidate(object source, ServerValidateEventArgs args)
      {
         if (txtFullName.Text.Length > 0)
         {
            switch (rcbType.SelectedValue)
            {
               case "":
                  {
                     valType.ErrorMessage = "Type";
                     valType.Text = "*Required";
                     args.IsValid = false;
                  }
                  break;
               //case "I":
               //   if (txtIPLevel.Text.Trim() == "") 
               //   {
               //      valType.ErrorMessage = "*IP Level?";
               //      args.IsValid = false;
               //   }
               //   break;
               default:
                  args.IsValid = true;
                  break;
            }
         }
         else
            args.IsValid = true;
      }

      protected void btnExportExcel_Click(object sender, EventArgs e)
      {
         //After page set idle for a while, the license object lost. 
         //To avoid the evaluation copy issue, we have to set license on every click
         Aspose.Cells.License license = new Aspose.Cells.License();
         license.SetLicense("Aspose.Total.lic");

         DataTable dtMain;

         using (var ctx = new DBContext())
         { 
            dtMain = ctx.getExportAttys();
            //ctx.LogUser(ntID, Global.REPORT_ID);
         }

         //Create a new worksheet
         Workbook workbook = new Workbook();
         Worksheet worksheet = workbook.Worksheets[0];
         Cells cells = worksheet.Cells;

         //Import data                
         cells.ImportDataTable(dtMain, true, 5, 0);

         //Create range and styles for the headings               
         Aspose.Cells.Style stl1 = workbook.Styles[workbook.Styles.Add()];
         stl1.VerticalAlignment = TextAlignmentType.Center;
         stl1.Font.Color = Color.Blue;
         stl1.Font.IsBold = true;
         stl1.Font.Name = "Arial";
         stl1.Font.Size = 10;
         StyleFlag flg = new StyleFlag();
         flg.Font = true;

         Range rngHd = cells.CreateRange("A1", "Z6");
         rngHd.ApplyStyle(stl1, flg);

         //Format the columns
         worksheet.AutoFitColumns();

         /*** TESTING ***
         // http://www.aspose.com/docs/display/cellsnet/Setting+Display+Formats+of+Numbers+and+Dates
         Aspose.Cells.Style stl2 = workbook.Styles[workbook.Styles.Add()];
         StyleFlag flg2 = new StyleFlag();
         stl2.Number = 15;
         cells.Columns[6].ApplyStyle(stl2, flg2);
         *** END TESTING ***/

         cells.SetColumnWidth(10, 45);    //Primary Practice Group
         cells.SetColumnWidth(11, 45);    //Bar

         // Populate header                
         cells[0, 0].PutValue("Report:");
         cells[0, 1].PutValue("LW1190 Brown Book Maintenance");

         TimeZoneInfo localZone = TimeZoneInfo.Local;
         string strRunDate = String.Format("{0:MM/dd/yyyy hh:mm tt}", DateTime.Now) +
                  " (" + ((localZone.IsDaylightSavingTime(DateTime.Now)) ? localZone.DaylightName : localZone.StandardName) + ")";
         cells[1, 0].PutValue("Run Date:");
         cells[1, 1].PutValue(strRunDate);

         cells[2, 0].PutValue("Report Type:");
         cells[2, 1].PutValue("Associates");

         //Save file to the client
         string sExcel = "LW1190";
         worksheet.Name = sExcel;
         sExcel += "_" + String.Format("{0:MM_dd_yyyy_hh_mm_ss_tt}", DateTime.Now) + ".xls";
         workbook.Save(this.Response, sExcel, ContentDisposition.Attachment, new XlsSaveOptions(SaveFormat.Excel97To2003));
      }

      /*
      protected void grdCommMstr_ItemCreated(object sender, GridItemEventArgs e)
      {
         if (e.Item is GridEditableItem && e.Item.IsInEditMode)
         {
            GridEditableItem item = e.Item as GridEditableItem;

            // Code
            GridTextBoxColumnEditor editor = item.EditManager.GetColumnEditor("CodeCommMstr") as GridTextBoxColumnEditor;
            TableCell parent = (TableCell)editor.TextBoxControl.Parent;
            RequiredFieldValidator val = new RequiredFieldValidator();
            // ID below is *NULL* if Telerik's Q2 2012 is used on GridBoundColumn.  Worked okay with version Q2 2013.
            val.ControlToValidate = editor.TextBoxControl.ID;         
            val.ErrorMessage = "*Required";
            parent.Controls.Add(val);

            // Description
            GridTextBoxColumnEditor editor1 = item.EditManager.GetColumnEditor("DescrpCommMstr") as GridTextBoxColumnEditor;
            TableCell parent1 = (TableCell)editor1.TextBoxControl.Parent;
            RequiredFieldValidator val1 = new RequiredFieldValidator();
            val1.ControlToValidate = editor1.TextBoxControl.ID;
            val1.ErrorMessage = "*Required";
            parent1.Controls.Add(val1);
         }
      }
      */
      /*
      //http://www.telerik.com/community/forums/aspnet-ajax/grid/accessing-headertext-of-a-gridtemplatecolumn.aspx
      protected void grdNewAtty_ItemDataBound(object sender, GridItemEventArgs e)
      {
         if (e.Item is GridHeaderItem)
         {
            GridHeaderItem header = (GridHeaderItem)e.Item;
            header["rbNewAttyTemplateColumn"].Text = "Select";
         }  
      }
      */
   }

}
