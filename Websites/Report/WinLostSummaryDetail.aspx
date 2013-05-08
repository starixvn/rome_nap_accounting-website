<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WinLostSummaryDetail.aspx.cs"
    Inherits="Report_WinLostSummaryDetail" Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
</head>
<body style="margin: 5px 5px 5px 2px">
    <form id="frm" name="frm" method="post" action="WinLostSummaryDetail.aspx">
    <asp:literal id="lblExportToExcel" runat="server"></asp:literal>
    </form>
    <asp:literal id="content" runat="server"></asp:literal>
</body>
</html>
