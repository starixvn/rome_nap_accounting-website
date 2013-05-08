<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllCustomerTurnOverBySite.aspx.cs"
    Inherits="Report_AllCustomerTurnOverBySite" Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script type="text/javascript">
        function reLoadPage(paras) {
            paras += '&fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val();
            location.href = 'AllCustomerTurnOverBySite.aspx?reload=1&' + paras;
        }
    </script>
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
                                <li class="current"><span>All Customer Turn Over </span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox">
                            <div class="filterContent">
                                <asp:literal id="SearchStandard" runat="server"></asp:literal>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="sData">
                                <asp:literal id="content" runat="server"></asp:literal>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</body>
<script type="text/javascript">
    var toDay = '<%= ToDay %>';
    var fDay = '<%= FDay %>';
    var tDay = '<%= TDay %>';
    var minDay = '<%= MinDay %>';
    $(document).ready(function () {
        var limitFromDate = {
            minDate: minDay,
            maxDate: toDay
        };
        var options = {
            showOtherMonths: true,
            selectOtherMonths: true,
            checkMinMax: true
        };
        calendarUtils.renderCalendar("#fdate", limitFromDate, "#fdate", "", fDay, options);
        calendarUtils.renderCalendar("#tdate", limitFromDate, "#fdate", "#tdate", tDay, options);
    });
</script>
</html>
