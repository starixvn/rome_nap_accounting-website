<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WinLostReportMain.aspx.cs" Inherits="Report_OtherWinLostReports_WinLostReportMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/jquery-ui.css" />
    <link href="../../App_Themes/ui.jqgrid.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        <%=GetReportTitle(Request["site"] != null ? Request["site"] : _defaultTitle)%>
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
    <input type="hidden" id="site" value="<%= Request["site"] != null ? Request["site"] : _defaultTitle %>" />
    <!--Scripts-->
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/Report/OtherReportsWinLoss.js" type="text/javascript"></script>
    <!--/Scripts-->

</body>
</html>
