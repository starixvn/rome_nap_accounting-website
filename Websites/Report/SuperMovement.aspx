<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuperMovement.aspx.cs" Inherits="Report_SuperMovement"%>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>      
    <style type="text/css">
        td.cl1{background: #FFDDFF;}
        td.cl2{background: #ffffee;}
    </style>    

    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.wresize.js" type="text/javascript"></script>
    <script src="../js/SuperMovement.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
</head>
<body>   
<asp:Literal ID="reportHeader" runat="server"></asp:Literal>        
</body>
</html>
