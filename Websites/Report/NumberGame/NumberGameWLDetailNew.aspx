<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NumberGameWLDetailNew.aspx.cs"
    Inherits="Report_BettypeWL_NumberGame_NumberGameWLDetailNew" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/numbergamewinloss.css" rel="stylesheet" type="text/css" />
    <link href="../../js/MultipleSelect/jquery.multiselect.css" rel="stylesheet" type="text/css" />
    <link href="Styles/numbergamereport.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="JS/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="../../js/MultipleSelect/jquery.multiselect.js" type="text/javascript"></script>
    <script src="JS/numbergamewldetail.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
</head>
<body>
    <div class="main" style="">
        <table style="width: 100%" border="0" cellpadding="0px" cellspacing="0px">
            <tr>
                <td>
                    <div id="divMainHeader" class="">
                        <div id="left_tab1">
                            <ul>
                                <li class="lefticon"></li>
                                <li class="current"><span>Number Game Win Loss by Bet Type</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table border="0">
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
                                                Date:</div>
                                        </td>
                                        <td style="padding-left: 4px">
                                            <input type="text" id="fdate" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />
                                        </td>
                                        <td>
                                            <input type="button" onclick="numberGameWLByBettype.getReport()" class="btn" value="REPORT"
                                                name="btnReport" id="btnReport" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="err">
                                </div>
                            </div>
                        </div>
                        <div class="contentBox" style="">
                            <div id="ngNoneLiveGrid" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="ngNoneLiveGridTitle" class="gridtitle">
                                        NUMBER GAME "NON - LIVE" PRODUCT WIN/LOSS REPORT
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="numberGameWLByBettype.exportExcel()" value="  " id="btnExportNone"
                                            class="btnExcel" title="Export to Excel" /></div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Bettype</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        WinLoss</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Position</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Comm</div>
                                                </td>
                                                <td colspan="2">
                                                    <div class="c">
                                                        Line Total</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Turnover</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Margin</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Variance</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Real</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Virtual</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tblNgNoneLive" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer">
                                    <div class="footer-scroll">
                                        <table class="tbl-footer-scroll" id="tblFooterNgNoneLive">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div id="ngLiveGrid" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="ngLiveGridTitle" class="gridtitle">
                                        NUMBER GAME "LIVE" PRODUCT WIN/LOSS REPORT
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="numberGameWLByBettype.exportExcel()" value="  " id="btnExport"
                                            class="btnExcel" title="Export to Excel" /></div>
                                </div>
                                <div class="content-header clearfix">
                                    <div id="header-scroll" class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Bettype</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        WinLoss</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Position</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Comm</div>
                                                </td>
                                                <td colspan="2">
                                                    <div class="c">
                                                        Line Total</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Turnover</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Margin</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col100">
                                                        Variance</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Real</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Virtual</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div id="body-scroll" class="body-scroll">
                                        <table id="tblNgLive" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer">
                                    <div id="footer-scroll" class="footer-scroll">
                                        <table class="tbl-footer-scroll" id="tblFooterNgLive">
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
    <div id="dialogNgWLDetail" class="hidden">
        <div id="NgEventWLGrid" class="content-warper">
            <div class="content-toolbox bgtitle">
                <div id="NgEventWLGridTitle" class="gridtitle">
                </div>
                <div class="btngroup">
                    <input type="button" onclick="numberGameEventWinLossDetail.exportExcel()" value=" "
                        id="btnNgEventExport" class="btnExcel" title="Export to Excel" /></div>
            </div>
            <div class="content-header clearfix">
                <div class="header-scroll">
                    <table class="tbl-header-scroll">
                        <tr>
                            <td rowspan="2" style="border-bottom: none 0px;">
                                <div class="c col100">
                                    &nbsp;</div>
                            </td>
                            <td rowspan="2" style="border-bottom: none 0px;">
                                <div class="c col100">
                                    WinLoss</div>
                            </td>
                            <td rowspan="2" style="border-bottom: none 0px;">
                                <div class="c col100">
                                    Position</div>
                            </td>
                            <td rowspan="2" style="border-bottom: none 0px;">
                                <div class="c col100">
                                    Comm</div>
                            </td>
                            <td colspan="2">
                                <div class="c">
                                    Line Total</div>
                            </td>
                            <td rowspan="2" style="border-bottom: none 0px;">
                                <div class="c
    col100">
                                    Turnover</div>
                            </td>
                            <td rowspan="2" style="border-bottom: none 0px;">
                                <div class="c col100">
                                    Margin</div>
                            </td>
                            <td rowspan="2" style="border-bottom: none 0px;">
                                <div class="c col100">
                                    Variance</div>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-bottom: none 0px;">
                                <div class="col100 c">
                                    Real</div>
                            </td>
                            <td style="border-bottom: none
    0px;">
                                <div class="col100 c">
                                    Virtual</div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="content-body clearfix">
                <div class="body-scroll">
                    <table id="tblNgEvent" class="tbl-body-scroll">
                    </table>
                </div>
            </div>
            <div class="content-footer">
                <div class="footer-scroll">
                    <table class="tbl-footer-scroll" id="tblNgFooterEvent">
                    </table>
                </div>
            </div>
        </div>
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
        calendarUtils.renderCalendar("#fdate", limitFromDate, "#fdate", "", fDay, options);
    });
</script>
</html>
