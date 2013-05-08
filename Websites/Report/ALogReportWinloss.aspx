<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ALogReportWinloss.aspx.cs"
    Theme="" Inherits="Report_ALogReportWinloss" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="ReportTitle" runat="server"></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Report/M88WinLoss.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>

    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>

    <script src="../js/jQuery/jquery.wresize.js" type="text/javascript"></script>

    <script src="../js/CommonFxHeader.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>

    <script src="../js/ALogReportWinLoss.js?ver=1" type="text/javascript"></script>

</head>
<body onload="ResetAll();">
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        ALog Win Lost
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <asp:Literal ID="reportHeader" runat="server"></asp:Literal>
            </div>
            <div class="bg_table">
                <div id="sData">
                </div>
            </div>
        </div>
    </div>
</body>
</html>
