<%@ Page Language="C#" MasterPageFile="~/App_Templates/Default/Report.master" AutoEventWireup="true" CodeFile="WinLostMultipleBetOnly.aspx.cs" Inherits="Report_WinLostMultipleBetOnly" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ReportTitle" Runat="Server">
    <asp:Literal ID ="header" runat ="server"></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ReportContent" Runat="Server">
    <script type="text/javascript">
        function ShowPopup(url)
        {
            var wnd=window.open(url,"popup","width=300,height=300");
        }
    </script>
    <asp:Literal ID="content" runat="server"></asp:Literal>
</asp:Content>



