<%@ Page Language="C#" MasterPageFile="~/App_Templates/Default/Report.master" AutoEventWireup="true" CodeFile="AllCustomerTurnOver.aspx.cs" Inherits="AllCustomerTurnOver" Title="Untitled Page" %>
<asp:Content ID="custTurnOverHeader" ContentPlaceHolderID="ReportTitle" Runat="Server">
    <asp:Literal ID="custTurnOverHeaderStr" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="custTurnOverContent" ContentPlaceHolderID="ReportContent" Runat="Server">
    <asp:Literal ID="custTurnOverContentHtml" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ReportFooter" Runat="Server">
</asp:Content>

