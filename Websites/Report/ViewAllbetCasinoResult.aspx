<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewAllbetCasinoResult.aspx.cs" Inherits="Report_ViewLiveCasinoResult" Theme="" Async="true" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>All Bet Result</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link rel="Stylesheet" type ="text/css" href ="../App_Themes/BetList/Betlist.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>
    <link rel="stylesheet" type="text/css" href="../Content/Fanex.Result/Allbet/css/table_w.css?v=<%= AccCommon.Version() %>"/>
</head>
<body>
    <div style="margin-bottom:5px;"></div>
    <asp:Literal ID="lResult" runat="server"></asp:Literal>  
</body>
</html>
