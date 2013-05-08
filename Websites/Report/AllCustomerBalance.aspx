<%@ Page Language="C#" MasterPageFile="~/App_Templates/Default/Report.master" AutoEventWireup="true" CodeFile="AllCustomerBalance.aspx.cs" Inherits="Report_AllCustomerBalance" Title="Untitled Page" %>
<asp:Content ID="CustBalanceHeader" ContentPlaceHolderID="ReportTitle" Runat="Server">
    <asp:Literal ID="CustBalanceHeaderStr" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="CustBalanceContent" ContentPlaceHolderID="ReportContent" Runat="Server">
    <asp:Literal ID="CustBalanceContentHtml" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ReportFooter" Runat="Server">
</asp:Content>

