<%@ Page Language="C#" MasterPageFile="~/App_Templates/Default/Report.master" AutoEventWireup="true" CodeFile="AllCustBalanceByCurrency.aspx.cs" Inherits="AllCustBalanceByCurrency" Title="Untitled Page" %>
<asp:Content ID="CustBalanceByCurrencyHeader" ContentPlaceHolderID="ReportTitle" Runat="Server">
    <asp:Literal ID="CustBalanceByCurrencyHeaderStr" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="CustBalanceByCurrencyHeaderContent" ContentPlaceHolderID="ReportContent" Runat="Server">
    <asp:Literal ID="CustBalanceByCurrencyHeaderContentHtml" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ReportFooter" Runat="Server">
</asp:Content>

