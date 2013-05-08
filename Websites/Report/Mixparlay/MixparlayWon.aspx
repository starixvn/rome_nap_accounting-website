<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MixparlayWon.aspx.cs" Theme=""
    Inherits="Report_MixparlayWon" %>

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
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Report/mixparlaywon.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Pager/jpager.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="script/MixParlayWonV2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span>Mix Parlay Won</span></li>
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
                                        <td>
                                            <input type="radio" id="rdbsocceronly" name="sportstype" value="soccer" />
                                        </td>
                                        <td style="width: 40px">
                                            Soccer
                                        </td>
                                        <td>
                                            <input type="radio" id="rdborthersports" name="sportstype" value="other" checked="checked" />
                                        </td>
                                        <td style="width: 80px">
                                            Other Sports
                                        </td>
                                        <td align="left">
                                            <input type="button" class="btn" onclick="mixparlayWonReport.getMixparlayWonReport()"
                                                value="View Report" name="btnShow" id="btnShow" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="err" style="">
                                </div>
                            </div>
                        </div>
                        <div class="contentBox" style="">
                            <div id="mixparlaywon" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitle" class="gridtitle">
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="mixparlayWonReport.exportToExcel();" value="  " name="btnExport"
                                            id="btnExport" class="btnExcel" title="Export to Excel" />
                                    </div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        TransId</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="col160 c">
                                                        UserName</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Currency</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Exchange</div>
                                                </td>
                                                <td colspan="2" style="border-bottom: solid 1px #7C98A4;">
                                                    <div class="c">
                                                        User Currency</div>
                                                </td>
                                                <td colspan="2" style="border-bottom: solid 1px #7C98A4;">
                                                    <div class="c">
                                                        Base Currency</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Selection</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Win Odds</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Stake</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        WinLost</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Stake</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c sorter" id="wlsort" onclick="mixparlayWonReport.baseCurrenySort();">
                                                        WinLost</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tblmixparlay" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer clearfix">
                                    <div class="footer-scroll">
                                        <table class="tbl-footer-scroll">
                                            <tr>
                                                <td>
                                                    <div class="col80">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="col160">
                                                        Total</div>
                                                </td>
                                                <td>
                                                    <div class="col80">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="col80 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="col80 r" id="totalstake">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="col80 r" id="totalwinlost">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="col80 r" id="totalbasestake">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="col80 r" id="totalbasewinlost">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="col80 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="col80 r">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-pagination bgtitle clearfix">
                                    <div class="pagination-scroll">
                                        <table style="width: 100%" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <div id="pagination">
                                                        &nbsp;</div>
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
