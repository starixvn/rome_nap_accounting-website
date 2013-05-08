<%@ Page Language="C#" MasterPageFile="~/App_Templates/Default/Report.master" AutoEventWireup="true" CodeFile="CompanyBalance.aspx.cs" Inherits="Report_CompanyBalance" Title="Untitled Page" %>
<asp:Content ID="CompanyBalanceHeader" ContentPlaceHolderID="ReportTitle" Runat="Server">
    <asp:Literal ID="CompanyBalanceHeaderStr" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="CompanyBalanceContent" ContentPlaceHolderID="ReportContent" Runat="Server">    
    <asp:Literal ID="DebitRunningHtml" runat="server"></asp:Literal><br/>
    <asp:Literal ID="DebitCompletedHtml" runat="server"></asp:Literal><br/>    
    <asp:Literal ID="CreditHtml" runat="server"></asp:Literal><br/>
    <asp:Literal ID="CompanyBalanceTotal" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ReportFooter" Runat="Server">
</asp:Content>

