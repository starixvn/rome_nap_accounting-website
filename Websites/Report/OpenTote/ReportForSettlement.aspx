<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportForSettlement.aspx.cs"
    Inherits="Report_OptenTote_ReportForSettlement" Theme="" %>

<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Report/Report.css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-1.3.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <link href="../../js/IBCWnd/IBCWnd.css" rel="stylesheet" type="text/css" />
    <script src="../../js/IBCWnd/IBCWnd.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="../../js/Report/OpenToteSettlement/OpenToteSettlement.js" type="text/javascript"></script>
</head>
<body>
    <div class="main">
        <table style="width: 100%" border="0" cellpadding="0px" cellspacing="0px">
            <tr>
                <td>
                    <div id="divMainHeader" class="">
                        <div id="left_tab1">
                            <ul>
                                <li class="lefticon"></li>
                                <li class="current"><span>Racing OpenTote Daily Report</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox">
                            <div class="filterContent">
                                <asp:literal id="headerDatetime" runat="server"></asp:literal>
                                <div id="sErrMsg" class="error-message" style="font-weight: bold">
                                </div>
                            </div>
                        </div>
                        <div class="contentBox" style="">
                            <div id="sContent" style="margin-top: 1px; margin-right: 3px;">
                            </div>
                        </div>
                    </div>
                    <input type="hidden" id="movebetsday" name="movebetsday" value="<%=ReportUtils.BalanceUp.MoveBetsDay %>" />
                    <input type="hidden" id="type" name="type" value="<%=Request["type"] %>" />
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="WinlostHR_OpenToteTitle" name="WinlostHR_OpenToteTitle" />
    <input type="hidden" id="WinlostHR_OpenToteHeader" name="WinlostHR_OpenToteHeader" />
    <input type="hidden" id="WinlostHR_OpenToteContent" name="WinlostHR_OpenToteContent" />
    <input type="hidden" id="winlostdate" name="winlostdate" />
    <input type="hidden" id="db" name="db" />
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
    <script type="text/javascript">
        var toDay = '<%= ToDay %>';
        var fDay = '<%= FDay %>';
        var minDay = '<%= MinDay %>';
        var maxDay = '<%= MaxDay %>';
        $(document).ready(function () {
            var limitFromDate = {
                minDate: minDay,
                maxDate: maxDay
            };
            var options = {
                showOtherMonths: true,
                selectOtherMonths: true,
                checkMinMax: true
            };
            calendarUtils.renderCalendar("#fdate", limitFromDate, "#fdate", "", fDay, options);
        });
        WinlostHR_OpenTote();
    </script>
</body>
</html>
