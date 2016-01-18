<%@ Page Language="C#" MasterPageFile="~/Controls/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="ReportTemplate.aspx.cs" Inherits="ReportTemplate.Pages.ReportTemplate"
    Title="ReportTemplate Originations, Proliferations, and Adjusted Production" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdatePanelsRenderMode="Inline">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbPeriod">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbDistributeBy" />
                    <telerik:AjaxUpdatedControl ControlID="rcbDistribution" LoadingPanelID="AjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="btnViewReport" />
                    <telerik:AjaxUpdatedControl ControlID="btnExportExcel" />
                    <telerik:AjaxUpdatedControl ControlID="lblConfirmation" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbCurrency">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnViewReport" />
                    <telerik:AjaxUpdatedControl ControlID="btnExportExcel" />
                    <telerik:AjaxUpdatedControl ControlID="lblConfirmation" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbReportType">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnViewReport" />
                    <telerik:AjaxUpdatedControl ControlID="btnExportExcel" />
                    <telerik:AjaxUpdatedControl ControlID="lblConfirmation" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbDistributeBy">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbDistribution" LoadingPanelID="AjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="btnViewReport" />
                    <telerik:AjaxUpdatedControl ControlID="btnExportExcel" />
                    <telerik:AjaxUpdatedControl ControlID="lblConfirmation" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbDistribution">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnViewReport" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="btnExportExcel" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="lblConfirmation" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <br />
    <table border="0" width="700">
        <tr>
            <td style="width: 200px">
                <asp:Label ID="lblDistributeBy" runat="server" Text="Distribute By:" Font-Bold="True"
                    Font-Names="Verdana" Font-Size="Small" Width="134px"></asp:Label>
            </td>
            <td style="width: 500px">
                <telerik:RadComboBox ID="rcbDistributeBy" runat="server" Skin="Vista" Width="455px"
                    AutoPostBack="True" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged"
                    OnSelectedIndexChanged="rcbDistributeBy_SelectedIndexChanged">
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td style="width: 200px">
                <asp:Label ID="lblDepartment" runat="server" Text="Department:" Font-Bold="True"
                    Font-Names="Verdana" Font-Size="Small" Width="162px"></asp:Label>
            </td>
            <td style="width: 500px">
                <telerik:RadComboBox ID="rcbDepartment" runat="server" Skin="Vista" Width="455px">
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td style="width: 200px">
                <asp:Label ID="lblLocation" runat="server" Text="Location:" Font-Bold="True" Font-Names="Verdana"
                    Font-Size="Small" Width="162px"></asp:Label>
            </td>
            <td style="width: 500px">
                <telerik:RadComboBox ID="rcbLocation" runat="server" Skin="Vista" Width="455px">
                </telerik:RadComboBox>
            </td>
        </tr>
    </table>
    <br />
    <asp:Button ID="btnRetrieveData" runat="server" Text="Retrieve Data" Font-Bold="False"
        Width="100px" OnClick="btnRetrieveData_Click" />&nbsp;
    <asp:Button ID="btnViewReport" runat="server" Text="View Report" Font-Bold="False"
        Width="100px" OnClientClick="window.open('ReportLoading.aspx', null, 'height=650, width=900, top=50, left=50, resizable=1')"
        OnClick="btnViewReport_Click" />&nbsp;
    <asp:Button ID="btnExportExcel" runat="server" Font-Bold="False" Text="Export Excel"
        Width="100px" OnClick="btnExportExcel_Click" />
    <br />
    <br />
    <asp:Label ID="lblConfirmation" runat="server" Font-Bold="False" Font-Names="Verdana"
        Font-Size="Small" Width="532px" EnableViewState="False" ForeColor="RoyalBlue"></asp:Label><br />
</asp:Content>
