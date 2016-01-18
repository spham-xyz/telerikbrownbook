<%@ Page Language="C#" MasterPageFile="~/Controls/MasterPage.Master" AutoEventWireup="true" CodeBehind="Spoofing.aspx.cs" Inherits="LW1190.Spoofing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="Label1" runat="server" Text="Enter the Network NT ID you want to spoof"></asp:Label><br />
   <asp:TextBox ID="txtNTID" runat="server" Width="146px"></asp:TextBox>&nbsp;
   <br />
   <asp:Button ID="btnOK" runat="server" Text="Enter Application"
      Width="150px" PostBackUrl="~/Pages/LW1190.aspx" />
   <br />
   <br />
   <br />
   <br />
   <br />
   <br />
   <asp:Label ID="lblWarning" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="Red"
      Width="919px"></asp:Label>
</asp:Content>