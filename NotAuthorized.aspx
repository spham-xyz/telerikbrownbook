<%@ Page Language="C#" MasterPageFile="~/Controls/MasterPage.Master" AutoEventWireup="true" CodeBehind="NotAuthorized.aspx.cs" Inherits="LW1190.NotAuthorized" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width='100%' border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td align="center">
                <h2>
                    <br />
                    <asp:Label ID="lblStatus" runat="server" Text="You are not authorized to access this page." ForeColor="Red"></asp:Label>
                </h2>
            </td>
        </tr>
    </table>
</asp:Content>
