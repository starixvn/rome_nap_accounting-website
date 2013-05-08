<%@ Page Language="C#" MasterPageFile="~/App_Templates/Default/Report.master" AutoEventWireup="true" CodeFile="StatementAll.aspx.cs" Inherits="Report_StatementAll" Title="All statement report" %>
<asp:Content ID="RepHeader" ContentPlaceHolderID="ReportTitle" runat="server">
    <asp:Literal ID="RepHeaderStr" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="RepContent" ContentPlaceHolderID="ReportContent" Runat="Server">
    <asp:Literal ID="RepContentHtml" runat="server"></asp:Literal>
</asp:Content>

