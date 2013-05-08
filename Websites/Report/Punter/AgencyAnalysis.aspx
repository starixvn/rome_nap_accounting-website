<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgencyAnalysis.aspx.cs" Inherits="Report_Punter_AgencyAnalysis"
    Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=Resources.Language.lblAccountingSystem%></title>
    <meta http-equiv="Cache-Control" content="no-cache" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/commonIOM.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/Report/Punter/AgencyAnalysis.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
</head>
<body>
    <%=html.ToString()%>
    <input type="hidden" id="FullPath" name="FullPath" />
    <input type="hidden" id="chartpath" name="chartpath" />
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
    <img id="displayBox" src="../../App_Themes/Images/waiting7.gif" alt="" style="display: none;" />
</body>
</html>
