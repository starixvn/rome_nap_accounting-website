<%@ Page Language="C#" MasterPageFile="~/App_Templates/Default/Report.master" AutoEventWireup="true" CodeFile="AllAgentTurnOver.aspx.cs" Inherits="AllAgentTurnOver" Title="Untitled Page" %>
<asp:Content ID="agentTurnOverHeader" ContentPlaceHolderID="ReportTitle" Runat="Server">
    <asp:Literal ID="agentTurnOverHeaderStr" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="agentTurnOverContent" ContentPlaceHolderID="ReportContent" Runat="Server">
    <asp:Literal ID="agentTurnOverContentHtml" runat="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ReportFooter" Runat="Server">
</asp:Content>

