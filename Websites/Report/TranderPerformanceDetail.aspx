<%@ Page Language="C#" MasterPageFile="~/App_Templates/Default/Report.master" AutoEventWireup="true" CodeFile="TranderPerformanceDetail.aspx.cs" Inherits="Report_TranderPerformanceDetail" %>

<asp:Content ID="ctnHeader" ContentPlaceHolderID="ReportTitle" Runat="Server">
    <asp:Literal ID="ltrHeaderStr" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="ctnContent" ContentPlaceHolderID="ReportContent" Runat="Server">
    <asp:Literal ID="ltrContentHtml" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="ctnFooter" ContentPlaceHolderID="ReportFooter" Runat="Server">
</asp:Content>