<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyTurnover.aspx.cs" Theme=""
    Inherits="Report_DailyTurnover" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="style/DailyTurnover.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/jQuery/Jlinq/jlinq.js"></script>
    <script type="text/javascript" src="../../js/jQuery/Jlinq/jlinq.jquery.js"></script>
    <script src="../../js/dialogbox.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="script/dailyturnover.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span>Daily Turnover</span></li>
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
                                            <input type="button" class="btn" onclick="dailyTurnover.getReport()" value="View Report"
                                                name="btnShow" id="btnShow" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="dailyturnover" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitle" class="gridtitle">
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="dailyTurnover.exportToExcel();" value="  " name="btnExport"
                                            id="btnExport" class="btnExcel" title="Export to Excel" />
                                    </div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Date</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Total Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Super Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Master Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Agent Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        IBC Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col240 c">
                                                        CNT</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tbldailyturnover" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <br />
                                <span style="cursor: pointer" onclick="detailTurnover.getReportDetailTurnover();"><font
                                    color="blue"><u style="font-weight: bold; font-size: small">View detail Turnover changed</u>
                                </font></span>
                                <br />
                                <div id="txtMess" style="display: none">
                                    <span>The figures shown in this report reflect the changes of Turnover traced from the
                                        day after selected date up to now.</span><br />
                                    <span>Note that these figures are FOR REFERENCE ONLY as the report just covers some
                                        common cases that cause Turnover change, actual changes may be vary.</span>
                                </div>
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <div id="detailTurnover" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitleDetail" class="gridtitle">
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="detailTurnover.exportToExcel();" value="  " name="btnExport"
                                            id="btnExcel" class="btnExcel" title="Export to Excel" />
                                    </div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col140 c">
                                                        Date Time</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col140 c">
                                                        Action</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Username</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Total Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Super Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Master Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Agent Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        IBC Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col140 c">
                                                        CNT</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tbldetailTurnover" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <div id="ActionTurnover" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridTitleAction" class="gridtitle">
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="detailActionTurnover.exportToExcel();" value="  " name="btnExport"
                                            id="Button1" class="btnExcel" title="Export to Excel" />
                                    </div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div id="txtMatch" class="col300 c">
                                                    </div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Total Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Super Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Master Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Agent Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        IBC Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col140 c">
                                                        CNT</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tblActionTurnover" class="tbl-body-scroll">
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
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
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
</script>
</html>
