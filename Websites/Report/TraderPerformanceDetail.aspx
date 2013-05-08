<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TraderPerformanceDetail.aspx.cs" Inherits="Report_TraderPerformanceDetail" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<html>
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/> 
    <%--<link rel="Stylesheet" type ="text/css" href ="../App_Themes/Report/TraderPerformance.css" />--%>
    <style type="text/css">
      /*.a1{background-color: #CCFFFF !important;}
      .a2{background-color: #FFFF99 !important;}
      .a3{background-color: #ffffee !important;}
      .a4{background-color: #FFDDFF !important;}
      .a5{background-color: #ffffff !important;}
      */
      
        .a1{background-color: #CCFFFF !important;}
        .a2{background-color: #FFFF99  !important;}
        .a3{background-color: #ffffee !important;}
        .a6{background-color: #FFDDFF !important;}
        .a4{background-color: #DFEAFF  !important;}
        .a5{background-color: #ffffff  !important;}
    </style>
    <script src="../js/common.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../js/TraderPerformanceDetail.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
</head>
<body style="margin:5px 2px 5px 5px">
    <asp:Literal ID="reportHeader" runat="server"></asp:Literal>
    <div id="sData"></div>    
</body>
</html>



