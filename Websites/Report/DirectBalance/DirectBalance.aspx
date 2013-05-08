<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DirectBalance.aspx.cs" Inherits="Report_DirectBalance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <script src="../../js/jQuery/jquery-latest.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-1.3.min.js" type="text/javascript"></script>
    <script src="../../js/Report/DirectBalance/DirectBalance.js?ver=3" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../../js/FrozenHeader.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <style type="text/css">
        .theader
        {
            line-height: 18px;
            background-color: #B5C4D0;
        }
        
        .theader thead
        {
        }
        
        .theader thead tr
        {
        }
        
        .theader thead tr th
        {
            border: 1px solid #7C98A4;
        }
        
        .tcontent
        {
            line-height: 18px;
        }
        .tcontent tbody
        {
        }
        .tcontent tbody tr
        {
        }
        .tcontent tbody tr td
        {
            border-bottom: 1px solid #7C98A4;
            border-left: 1px solid #7C98A4;
            border-right: 1px solid #7C98A4;
            border-top: 0px none;
        }
        
        .tfooter
        {
            line-height: 18px;
        }
        .tfooter tfoot
        {
        }
        
        .tfooter tfoot tr
        {
        }
        
        .tfooter tfoot tr td
        {
            border: 1px solid #7C98A4;
        }
    </style>
</head>
<body>
    <asp:literal id="reportHeader" runat="server"></asp:literal>
</body>
</html>
