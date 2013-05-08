<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DuplicateTickets.aspx.cs"
    Inherits="Report_Others_DuplicateTickets" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="JS/duplicateticket.js" type="text/javascript"></script>
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
                                <li class="current"><span>Duplicate Ticket</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tr>
                                        <td>
                                            Date:
                                        </td>
                                        <td style="width: 120px">
                                            <input type="text" id="date" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />
                                        </td>
                                        <td align="left">
                                            <input type="button" class="btn" onclick="duplicateticket.btnReport()" value="View Report"
                                                name="btnShow" id="btnShow" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="contentBox" style="">
                            <div id="duplicateticket" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitle" class="gridtitle">
                                    </div>
                                    <div class="btngroup">
                                    </div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Username</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        CustId</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Bettype</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Winlostdate</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Balance Before Bet</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Stake</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Odds</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Status</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Refno(1st ticket)</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Refno(2nd ticket)</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Bettime(1st ticket)</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Bettime(2nd ticket)</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Time Diff(milisecond)</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tblduplicateticket" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
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
        calendarUtils.renderCalendar("#date", limitFromDate, "#date", "", fDay, options);
    });
    //duplicateticket.btnReport();
</script>
</html>
