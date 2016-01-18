using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Drawing;
using System.Data.SqlClient;
using Aspose.Cells;
using Telerik.Web.UI;
using ReportTemplate.DataAccess;
using ReportTemplate.Components;
using ReportTemplate.Types;

namespace ReportTemplate.Pages
{
    public partial class ReportTemplate : System.Web.UI.Page
    {
        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) //&& !rcbDistribution.IsCallBack)
            {
                //SecurityCheck();

                //Instantiate an instance of license and set the license file through its path
                Aspose.Cells.License license = new Aspose.Cells.License();
                license.SetLicense("Aspose.Cells.lic");

                //PopulateCurrency();
                //PopulateReportTypes();
                PopulateDistributeBy();
                PopulateLocation();
                //PopulateDistribution(rcbDistributeBy.SelectedValue.ToString().Trim());

                EnableButtons(false);
            }
            else
            {
                this.RadScriptManager1.RegisterPostBackControl(btnExportExcel);
                this.RadScriptManager1.RegisterPostBackControl(btnViewReport);
            }
        }

        protected void SecurityCheck()
        {
            
            string SpooferID = (string)Session["SpoofID"];
            string UserNTID = SessionUtils.CurrentNTIDFromNTLM;
            string SpoofingIDs = ConfigurationManager.AppSettings["SpoofingIDs"].ToString().Trim();

            if (SpooferID == null && SpoofingIDs.Contains(UserNTID) == true)    //It has not visited the spoofing page
                Response.Redirect("~/Spoofing.aspx");
            else
            {
                if (SpooferID != null)  //This is the case the spoofer page has been visited
                {
                    UserNTID = SpooferID;   //The NTID you entered in spoofing page
                }

                if (SessionUtils.EstablishUserPermissionForNTID(UserNTID))
                {
                }
                else
                {
                    Response.Redirect("~/NotAuthorized.aspx");
                }
            }

            //To allow the user to spoof more than one session
            Session["SpoofID"] = null;
        }

        protected void PopulateLocation()
        {
            DataTable dtLocation = DBAccessor.GetLocation();

            for (int row = 0; row < dtLocation.Rows.Count; row++)
            {
                string locCode = dtLocation.Rows[row][0].ToString();
                string locDesc = dtLocation.Rows[row][1].ToString();
                RadComboBoxItem itPeriod = new RadComboBoxItem();
                itPeriod.Value = locCode;
                itPeriod.Text = locCode + " " + locDesc;
                rcbLocation.Items.Add(itPeriod);
            }
            rcbDepartment.SelectedIndex = 0;

        }

        

        //protected void PopulateCurrency()
        //{                                            
        //    this.rcbCurrency.Items.Clear();            
        //    DataTable dtCurrencies = DBAccessor.GetCurrencies();
        //    rcbCurrency.Items.Add(new RadComboBoxItem("USD", "USD"));

        //    //foreach (DataRow row in dtCurrencies.Rows)
        //    //{
        //    //    string strCurrency = row["CURCODE"].ToString().Trim();
        //    //    this.rcbCurrency.Items.Add(new RadComboBoxItem(strCurrency, strCurrency));
        //    //}
        //    this.rcbCurrency.SelectedIndex = 0;                        
        //}

        //protected void PopulateReportTypes()
        //{
        //    UserPermission userPermission = (UserPermission)Session["userPermission"];
            
        //    this.rcbReportType.Items.Clear();
        //    //if (userPermission.IsAccessToAdjust == true)           
        //        this.rcbReportType.Items.Add(new RadComboBoxItem("Adjusted Production", "Adjusted Production"));
        //    //if (userPermission.IsAccessToOrig == true)           
        //        this.rcbReportType.Items.Add(new RadComboBoxItem("Originations", "Originations"));
        //    //if (userPermission.IsAccessToProlif == true)           
        //        this.rcbReportType.Items.Add(new RadComboBoxItem("Proliferations", "Proliferations"));
        //    this.rcbReportType.SelectedIndex = 0;
            
        //}

        protected void PopulateDistributeBy()
        {
            try
            {
                this.rcbDistributeBy.Items.Clear();                
                this.rcbDistributeBy.Items.Add(new RadComboBoxItem("DEPARTMENT", "DEPARTMENT"));
                this.rcbDistributeBy.Items.Add(new RadComboBoxItem("LOCATION", "LOCATION"));                
                this.rcbDistributeBy.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //protected void PopulateDistribution(string strDistributeBy)
        //{                                       
        //    rcbDistribution.Text = string.Empty;
        //    rcbDistribution.Items.Clear();
        //    string strPeriod = rcbPeriod.SelectedItem.Value;

        //    switch (strDistributeBy)
        //    {                
        //        case "ATTORNEY":
        //            DataTable dtItems = DBAccessor.GetAttornyName(strPeriod);
        //            Session.Add("dtAttorneyList", dtItems);

        //            rcbDistribution.Items.Add(new RadComboBoxItem("ALL ATTORNEYS", "00"));
        //            //int i = dtItems.Rows.Count;
        //            foreach (DataRow row in dtItems.Rows)
        //            {
        //                rcbDistribution.Items.Add(new RadComboBoxItem(row["ATTORNEYNUM"].ToString().Trim() + " " + row["ATTORNEYNAME"].ToString().Trim(), row["ATTORNEYNUM"].ToString().Trim()));
        //            }
        //            rcbDistribution.SelectedIndex = 0;

        //            break;
                                
        //    }
           
        //}
                

        #endregion Page Load       

        #region ComboBox Events

        protected void EnableButtons(bool blnEnabled)
        {
            //this.lblConfirmation.Visible = blnEnabled;
            if (blnEnabled == false)
                this.lblConfirmation.Text = string.Empty;

            this.btnViewReport.Enabled = blnEnabled;
            this.btnExportExcel.Enabled = blnEnabled;
        }

        protected void rcbCurrency_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            EnableButtons(false);
        }

        protected void rcbReportType_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            EnableButtons(false);
        }

        protected void rcbDistributeBy_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string strDistributeBy = string.Empty;

            strDistributeBy = this.rcbDistributeBy.SelectedItem.Value.ToString().Trim();

            if (Session["LW159_DistributeBy"] == null)
                Session.Add("LW159_DistributeBy", strDistributeBy);
            else
                Session["LW159_DistributeBy"] = strDistributeBy;

            EnableButtons(false);
            //PopulateDistribution(strDistributeBy);            

            strDistributeBy = string.Empty;
        }

        //protected void rcbDistribution_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        //{
        //    rcbDistribution.Items.Clear();

        //    if (Session["dtAttorneyList"] == null)
        //    {
        //        string strPeriod = rcbPeriod.SelectedItem.Value;
        //        DataTable dtAttys = DBAccessor.GetAttornyName(strPeriod);
        //        Session.Add("dtAttorneyList", dtAttys);
        //    }
            
        //    DataTable dtItems = (DataTable)Session["dtAttorneyList"];           
        //    DataRow[] drItems = dtItems.Select("ATTORNEYNAME LIKE '*" + e.Text + "*' OR ATTORNEYNUM LIKE '" + e.Text + "*'", "ATTORNEYNAME");

        //    int intItemsPerRequest = 15;
        //    int intItemOffset = e.NumberOfItems;
        //    int intEndOffset = intItemOffset + intItemsPerRequest;

        //    if (intEndOffset > drItems.Length)
        //        intEndOffset = drItems.Length;

        //    if (intItemOffset == 0)
        //        rcbDistribution.Items.Add(new RadComboBoxItem("ALL ATTORNEYS", "00"));

        //    for (int i = intItemOffset; i < intEndOffset; i++)
        //        rcbDistribution.Items.Add(new RadComboBoxItem(drItems[i]["ATTORNEYNUM"].ToString() + " " + drItems[i]["ATTORNEYNAME"].ToString(), drItems[i]["ATTORNEYNUM"].ToString()));

        //    if (drItems.Length > 0)
        //    {
        //        e.Message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", intEndOffset.ToString(), drItems.Length.ToString());
        //    }
        //    else
        //    {
        //        e.Message = "No matches";
        //    }

        //}

        //protected void rcbDistribution_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    EnableButtons(false);
        //}

        //protected void rcbPeriod_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    PopulateDistribution(rcbDistributeBy.SelectedValue.ToString().Trim());
        //    EnableButtons(false);
        //}

        #endregion ComboBox Events

        #region Retrieve Data 
       
        protected void btnRetrieveData_Click(object sender, EventArgs e)
        {
        //    // Keep track user's visiting to report.
        //    string strUserNetID = SessionUtils.CurrentNTIDFromNTLM;
        //    DBAccessor.SaveUserInformation(strUserNetID, "ReportTemplate");
            
        //    DataSet dsReport = new DataSet();
        //    DataTable dtReport = new DataTable();
        //    string strConfirmation = "";
        //    string strPeriod = rcbPeriod.SelectedItem.Value;
        //    Session.Add("Period", strPeriod);
        //    string strReportType = Convert.ToString(this.rcbReportType.SelectedItem.Value);
        //    Session.Add("ReportType", strReportType);
        //    string strDistributeBy = rcbDistributeBy.SelectedItem.Value.ToString();
        //    Session.Add("DistributeBy", strDistributeBy);
        //    string strCurrency = rcbCurrency.SelectedItem.Value.ToString();
        //    Session.Add("ReportTemplate_Currency", strCurrency);                                                  
        //    string strAttorney = rcbDistribution.SelectedValue;
        //    string strDistribution = rcbDistribution.Text;
        //    Session.Add("Distribution", strDistribution);
        
        //    dsReport = DBAccessor.GetReportData(strReportType, strPeriod, strAttorney);
        //    dtReport = dsReport.Tables[0];
        //    Session.Add("ReportTemplate_Report", dtReport);                    
                                                  
        //    if (dtReport != null && dtReport.Rows.Count > 0)
        //    {
        //        this.lblConfirmation.Text = "*** Data Successfully Retrieved ***";
        //        this.lblConfirmation.ForeColor = Color.RoyalBlue;
        //        this.EnableButtons(true);
        //    }
        //    else
        //    {
                
        //        strConfirmation = "No records found for this request: " + this.rcbCurrency.Text.Trim();
        //        strConfirmation += ", ";
        //        strConfirmation += this.rcbReportType.Text.Trim();
        //        strConfirmation += ", ";
        //        strConfirmation += this.rcbDistributeBy.Text.Trim();
        //        strConfirmation += ", ";
        //        strConfirmation += this.rcbDistribution.Text.Trim();
                
        //        this.lblConfirmation.Text = strConfirmation;
        //        this.lblConfirmation.ForeColor = Color.RoyalBlue;
        //    }

        //    strConfirmation = string.Empty;

        //    dtReport = null;

        }

        #endregion Retrieve Data

        #region View Report

        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            this.lblConfirmation.Text = "*** Data Successfully Retrieved ***";
            this.lblConfirmation.ForeColor = Color.RoyalBlue;
        }

        #endregion View Report

        #region Export Excel

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
        //    //After page set idle for a while, the license object lost. 
        //    //To avoid the evaluation copy issue, we have to set license on every click
        //    Aspose.Cells.License license = new Aspose.Cells.License();
        //    license.SetLicense("Aspose.Cells.lic");

        //    DataTable dtReport = new DataTable();                                             
        //    dtReport = (DataTable)Session["ReportTemplate_Report"];

        //    string strPeriod = rcbPeriod.SelectedItem.Text;
        //    string strPeriodType = rcbPeriodType.SelectedItem.Text;
        //    string strCurrency = rcbCurrency.SelectedItem.Value;
        //    string strReportType = rcbReportType.SelectedItem.Text;
        //    string strDistributeBy = rcbDistributeBy.SelectedItem.Text;
        //    string strDistributeCode = rcbDistribution.Text;

        //    switch (strReportType)
        //    {
        //        case "Adjusted Production":
        //            ExcelExport_AdjProd("ReportTemplate", dtReport, strCurrency, strReportType, strDistributeBy, strDistributeCode, strPeriod, strPeriodType);
        //            break;
        //        case "Originations":
        //        case "Proliferations":
        //            ExcelExport_Orig("ReportTemplate", dtReport, strCurrency, strReportType, strDistributeBy, strDistributeCode, strPeriod, strPeriodType);
        //            break;
               
        //    }
        }
       
        //protected void ExcelExport_AdjProd(string strExcelFileName, DataTable dtReport, string strCurrency, string strReportType,
        //            string strDistributeBy, string strDistributeCode, string strPeriodName, string strPeriodType)
        //{                    
        //    //Craete a new worksheet
        //    Workbook workbook = new Workbook();
        //    Worksheet worksheet = workbook.Worksheets[0];
        //    Cells cells = worksheet.Cells;

        //    try
        //    {
        //        //Import data                
        //        cells.ImportDataTable(dtReport, true, 9, 0);               

        //        // Populate header                
        //        cells[0, 0].PutValue("Report:");
        //        cells[0, 1].PutValue("ReportTemplate Adjusted Production - " + strCurrency + " CURRENCY");

        //        TimeZoneInfo localZone = TimeZoneInfo.Local;
        //        string strRunDate = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString()
        //                            + " (" + localZone.StandardName + ")";
        //        cells[1, 0].PutValue("Run Date:");
        //        cells[1, 1].PutValue(strRunDate);

        //        cells[2, 0].PutValue("Period:");
        //        cells[2, 1].PutValue(strPeriodName);

        //        cells[3, 0].PutValue("Period Type:");
        //        cells[3, 1].PutValue(strPeriodType);

        //        cells[4, 0].PutValue("Currency:");
        //        cells[4, 1].PutValue(strCurrency);

        //        cells[5, 0].PutValue("Report Type:");
        //        cells[5, 1].PutValue(strReportType);

        //        cells[6, 0].PutValue("Distribute By:");
        //        cells[6, 1].PutValue(strDistributeBy);

        //        cells[7, 0].PutValue("Distribution:");
        //        cells[7, 1].PutValue(strDistributeCode);

        //        string strPeriod = (string)Session["Period"];
        //        string BeginDate = "1/1/20" + strPeriod.Substring(2, 2);
        //        int thisMonth = Convert.ToInt32(strPeriod.Substring(0, 2));
        //        string LastDateofMonth = DBAccessor.GetLastDayOfMonth(thisMonth);
        //        string EndDate = LastDateofMonth + "/20" + strPeriod.Substring(2, 2);
        //        string strPeriodFromTo = "For Bills Processed From " + BeginDate + " Through " + EndDate;
        //        cells[3, 2].PutValue(strPeriodFromTo);
               
        //        //change the heading name
        //        cells[9, 0].PutValue("Client Name");
        //        cells[9, 1].PutValue("Matter Number");                
        //        cells[9, 2].PutValue("Matter Name");
        //        cells[9, 3].PutValue("Attorney No");
        //        cells[9, 4].PutValue("Attorney Name");
        //        cells[9, 5].PutValue("Originating Billing");
        //        cells[9, 6].PutValue("Proliferating Billing");
        //        cells[9, 7].PutValue("Adjusted Production");
        //        cells[9, 8].PutValue("Difference");
                

        //        //Create range and styles for the headings               
        //        Range range = cells.CreateRange("A1", "Z10");
        //        workbook.Styles.Add();
        //        Aspose.Cells.Style style = workbook.Styles[0];
        //        style.VerticalAlignment = TextAlignmentType.Center;
        //        style.Font.Color = Color.Blue;
        //        style.Font.IsBold = true;
        //        style.Font.Name = "Arial";
        //        style.Font.Size = 10;
        //        range.Style = style;

        //        //Format the columns
        //        worksheet.AutoFitColumns();

        //        // Format data for columns
        //        for (byte i = 3; i < 7; i++)
        //        {
        //            cells.Columns[i].Style.Custom = "###,##0.00";                     
        //        }
                
        //        //Save file to the client
        //        strExcelFileName = strExcelFileName + "_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToLongTimeString() + ".xls";
        //        worksheet.Name = "ReportTemplate";
        //        workbook.Save(strExcelFileName, FileFormatType.Default, SaveType.OpenInExcel, this.Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        workbook = null;
        //        worksheet = null;
        //        cells = null;
        //    }
        //}

        //protected void ExcelExport_Orig(string strExcelFileName, DataTable dtReport, string strCurrency, string strReportType,
        //            string strDistributeBy, string strDistributeCode, string strPeriodName, string strPeriodType)
        //{
        //    //Craete a new worksheet
        //    Workbook workbook = new Workbook();
        //    Worksheet worksheet = workbook.Worksheets[0];
        //    Cells cells = worksheet.Cells;

        //    try
        //    {
        //        //Import data                
        //        cells.ImportDataTable(dtReport, true, 9, 0);

        //        // Populate header                
        //        cells[0, 0].PutValue("Report:");
        //        if (strReportType == "Originations")
        //        {
        //            cells[0, 1].PutValue("ReportTemplate Originations - " + strCurrency + " CURRENCY");
        //        }
        //        else
        //        {
        //            cells[0, 1].PutValue("ReportTemplate Proliferations - " + strCurrency + " CURRENCY");
        //        }

        //        TimeZoneInfo localZone = TimeZoneInfo.Local;
        //        string strRunDate = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString()
        //                            + " (" + localZone.StandardName + ")";
        //        cells[1, 0].PutValue("Run Date:");
        //        cells[1, 1].PutValue(strRunDate);

        //        cells[2, 0].PutValue("Period:");
        //        cells[2, 1].PutValue(strPeriodName);

        //        cells[3, 0].PutValue("Period Type:");
        //        cells[3, 1].PutValue(strPeriodType);

        //        cells[4, 0].PutValue("Currency:");
        //        cells[4, 1].PutValue(strCurrency);

        //        cells[5, 0].PutValue("Report Type:");
        //        cells[5, 1].PutValue(strReportType);

        //        cells[6, 0].PutValue("Distribute By:");
        //        cells[6, 1].PutValue(strDistributeBy);

        //        cells[7, 0].PutValue("Distribution:");
        //        cells[7, 1].PutValue(strDistributeCode);

        //        string strPeriod = (string)Session["Period"];
        //        string BeginDate = "1/1/20" + strPeriod.Substring(2, 2);
        //        int thisMonth = Convert.ToInt32(strPeriod.Substring(0, 2));
        //        string LastDateofMonth = DBAccessor.GetLastDayOfMonth(thisMonth);
        //        string EndDate = LastDateofMonth + "/20" + strPeriod.Substring(2, 2);
        //        string strPeriodFromTo = "For Bills Processed From " + BeginDate + " Through " + EndDate;
        //        cells[3, 2].PutValue(strPeriodFromTo);

        //        //change the heading name
        //        cells[9, 0].PutValue("Client Name");
        //        cells[9, 1].PutValue("Matter");
        //        cells[9, 2].PutValue("Matter Name");
        //        cells[9, 3].PutValue("Attorney No");
        //        cells[9, 4].PutValue("Attorney");
        //        cells[9, 5].PutValue("Percent");
        //        cells[9, 6].PutValue("Date");
        //        cells[9, 7].PutValue("Billing Split");
        //        cells[9, 8].PutValue("Total Billing");

        //        //Create range and styles for the headings               
        //        Range range = cells.CreateRange("A1", "Z10");
        //        workbook.Styles.Add();
        //        Aspose.Cells.Style style = workbook.Styles[0];
        //        style.VerticalAlignment = TextAlignmentType.Center;
        //        style.Font.Color = Color.Blue;
        //        style.Font.IsBold = true;
        //        style.Font.Name = "Arial";
        //        style.Font.Size = 10;
        //        range.Style = style;

        //        //Format the columns
        //        worksheet.AutoFitColumns();

        //        // Format data for columns
        //        cells.Columns[5].Style.Custom = "###,##0.00 %";
        //        cells.Columns[6].Style.Custom = "mm/dd/yyyy";
        //        cells.Columns[7].Style.Custom = "###,##0.00";
        //        cells.Columns[8].Style.Custom = "###,##0.00";
        //        cells.HideColumn(9);

        //        //Save file to the client
        //        strExcelFileName = strExcelFileName + "_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToLongTimeString() + ".xls";
        //        worksheet.Name = "ReportTemplate";
        //        workbook.Save(strExcelFileName, FileFormatType.Default, SaveType.OpenInExcel, this.Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        workbook = null;
        //        worksheet = null;
        //        cells = null;
        //    }
        //}
         
        
        #endregion Export Excel
                
    }
}
