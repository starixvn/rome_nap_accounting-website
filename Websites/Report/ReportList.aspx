<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportList.aspx.cs" Inherits="Report_ReportList" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report List</title> 
    <link rel="stylesheet" type ="text/css" href="../App_Themes/Public/common.css"/>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />  
    <link href="../App_Themes/Report/MainReport.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/ReportList.js?ver=<%=WebSettings.Version%>"></script>    
</head>
<body>
<asp:Literal ID="ReportListHtml" runat="server"></asp:Literal>    
</body>
</html>
