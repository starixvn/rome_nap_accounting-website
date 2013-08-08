<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DateWinLossNew.aspx.cs" Inherits="Report_NumberGame_DateWinLossNew" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link href="../../js/Calendar2/css/jscal2.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../js/MultipleSelect/jquery.multiselect.css" rel="stylesheet" type="text/css" />
    <link href="Styles/numbergamereport.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="JS/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/MultipleSelect/jquery.multiselect.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="JS/numbergamewinlossbydatenew.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span>Number Game Win Loss By Date</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tr>
                                        <td style="padding-right: 4px">
                                            <div>
                                                Company:</div>
                                        </td>
                                        <td>
                                            <div>
                                                <select id="selectCompany" style="width: 100px; height: 18px;" multiple="multiple">
                                                </select>
                                            </div>
                                        </td>
                                        <td style="padding-left: 8px">
                                            <div>
                                                Currency:</div>
                                        </td>
                                        <td style="padding-left: 4px">
                                            <select id="selectCurrency" style="width: 100px; height: 18px;" multiple="multiple">
                                            </select>
                                        </td>
                                        <td style="padding-left: 8px">
                                            <div>
                                                Bettypes:</div>
                                        </td>
                                        <td style="padding-left: 4px">
                                            <select id="selectBettypes" style="width: 140px; height: 18px;" multiple="multiple">
                                            </select>
                                        </td>
                                        <td style="padding-left: 8px">
                                            From Date:
                                        </td>
                                        <td>
                                            <input type="text" id="fdate" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />
                                        </td>
                                        <td>
                                            To Date:
                                        </td>
                                        <td>
                                            <input type="text" id="tdate" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />
                                        </td>
                                        <td>
                                            <input type="button" onclick="numberGameWinlossByDate.getReport()" class="btn" value="REPORT"
                                                name="btnReport" id="btnReport" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="err">
                                </div>
                            </div>
                        </div>
                        <div class="contentBox" style="">
                            <div id="matchwinlost" class="content-warper" style="display: none;">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitle" class="gridtitle">
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="numberGameWinlossByDate.exportExcel('1')" value="  "
                                            name="btnExport" id="btnExport" class="btnExcel" title="Export to Excel" /></div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col160">
                                                        Match - Bet Type</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 r">
                                                        Winloss</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 r">
                                                        Position</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 r">
                                                        Commission</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 r">
                                                        Line Total</div>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 r">
                                                        Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 r">
                                                        Margin</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tblmatchwinlost" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer">
                                    <div class="footer-scroll">
                                        <table class="tbl-footer-scroll">
                                            <tr>
                                                <td>
                                                    <div class="col160 l">
                                                        Grand Total</div>
                                                </td>
                                                <td>
                                                    <div id="totalWinlost" class="col100 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div id="totalPosition" class="col100 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div id="totalCommission" class="col100 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div id="totalLineTotal" class="col100 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div id="totalTurnOver" class="col100 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div id="totalMargin" class="col100 r">
                                                    </div>
                                                </td>
                                            </tr>
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
    var tDay = '<%= TDay %>';
    $(document).ready(function () {
        var options = {
            showOtherMonths: true,
            selectOtherMonths: true,
            checkMinMax: true
        };
        calendarUtils.renderCalendar("#fdate", null, "#fdate", "", fDay, options);
        calendarUtils.renderCalendar("#tdate", null, "#tdate", "", fDay, options);
    });
</script>
</html>
