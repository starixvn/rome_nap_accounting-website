<%@ Page Language="C#" MasterPageFile="~/App_Templates/Default/Report.master" AutoEventWireup="true" CodeFile="AllCustWinLost.aspx.cs" Inherits="Report_AllCustWinLost" Title="Untitled Page" %>
<asp:Content ID="CustWinLostHeader" ContentPlaceHolderID="ReportTitle" Runat="Server">
    <asp:Literal ID="CustWinLostHeaderStr" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="CustWinLostContent" ContentPlaceHolderID="ReportContent" Runat="Server">
    <asp:Literal ID="CustWinLostContentHtml" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ReportFooter" Runat="Server">
</asp:Content>

