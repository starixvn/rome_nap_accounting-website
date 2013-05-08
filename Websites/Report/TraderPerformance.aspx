<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TraderPerformance.aspx.cs" Inherits="Report_TraderPerformance" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/> 
    <link href="../App_Themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/TraderPerformance.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
   
    <script src="../js/TraderPerformance.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>  
</head>
<body>
    <asp:Literal ID="reportHeader" runat="server"></asp:Literal>        
</body>
</html>
