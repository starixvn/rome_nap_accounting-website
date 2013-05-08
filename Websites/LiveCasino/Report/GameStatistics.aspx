<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GameStatistics.aspx.cs" Inherits="LiveCasino_Report_GameStatistics" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link type="text/css" rel="Stylesheet" href="../../js/jQuery/autocomplete/styles.css" />
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="Style/Common.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Pager/jpager.js" type="text/javascript"></script>
    <script src="../../js/jQuery/autocomplete/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="Script/GameStatistics.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span style="width: 192px;">Live Casino Game Statistic
                                    <div id="Span1" class="upCondition" onclick="gameStatistics.showhideSearchCondition(this);"
                                        style="display: inline; width: 14px; margin-left: 10px;">
                                    </div>
                                </span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tr>
                                        <td>
                                            <span><b>Search by:</b></span>
                                        </td>
                                        <td style="width: 103px;">
                                            Time period
                                        </td>
                                        <td style="width: 130px;">
                                            <select id="ddlTimeperiod" onchange="gameStatisticsHelper.setDateFilter(this.value)"
                                                style="width: 120px;">
                                                <option value="1">Today</option>
                                                <option value="2">Yesterday</option>
                                                <option value="3">Current week</option>
                                                <option value="4">Current month</option>
                                                <option value="5">Last month</option>
                                                <option value="6">Since last month</option>
                                                <option value="7">Last 3 months</option>
                                                <option value="8">Last 6 months</option>
                                                <option value="0">Specific period</option>
                                            </select>
                                        </td>
                                        <td style="width: 100px;">
                                            From date
                                        </td>
                                        <td style="width: 120px;">
                                            <input id="txtfromdate" class="txtdate" readonly="readonly" />
                                        </td>
                                        <td style="width: 70px;">
                                            To date
                                        </td>
                                        <td style="width: 120px;">
                                            <input id="txttodate" class="txtdate" readonly="readonly" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span><b>User search:</b></span>
                                        </td>
                                        <td>
                                            Username
                                        </td>
                                        <td>
                                            <input type="text" id="txtUsername" onblur="gameStatisticsHelper.on_Blur()" onfocus="gameStatisticsHelper.on_Focus()"
                                                style="width: 118px;" onkeypress="return KeyPress(event);" value="All" autocomplete="false" />
                                        </td>
                                        <td colspan="4">
                                            <input type="checkbox" id="chkExcludeTestAcc" checked="checked" />Exclude test accounts
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span><b>Game search:</b></span>
                                        </td>
                                        <td>
                                            Company
                                        </td>
                                        <td>
                                            <select id="ddlCompanies" style="width: 120px;">
                                            </select>
                                        </td>
                                        <td>
                                            Game type
                                        </td>
                                        <td>
                                            <select id="ddlGameType" style="width: 110px;">
                                            </select>
                                        </td>
                                        <td>
                                            Game table
                                        </td>
                                        <td>
                                            <select id="ddlGameTable" style="width: 130px;">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            Currency
                                        </td>
                                        <td>
                                            <select id="ddlCurrency" style="width: 120px;">
                                            </select>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            Total bet less than
                                        </td>
                                        <td>
                                            <input class="numbersOnly" type="text" id="txtTotalBetLessThan" style="width: 118px" />
                                        </td>
                                        <td>
                                            Total bet more than
                                        </td>
                                        <td>
                                            <input class="numbersOnly" type="text" id="txtTotalBetMoreThan" style="width: 110px" />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            Total win less than
                                        </td>
                                        <td>
                                            <input class="numbersOnly" type="text" id="txtTotalWinLessThan" style="width: 118px" />
                                        </td>
                                        <td>
                                            Total win more than
                                        </td>
                                        <td>
                                            <input class="numbersOnly" type="text" id="txtTotalWinMoreThan" style="width: 110px" />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span><b>Report by:</b></span>
                                        </td>
                                        <td colspan="6" id="tdReportFilter">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td colspan="6">
                                            <input type="checkbox" id="chkShowRefund" />
                                            Show refund column
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td colspan="6" style="text-align: center">
                                            <input class="btn" type="button" id="btnReport" value="REPORT" onclick="gameStatistics.specialReport=0;gameStatistics.getReportData(false);gameStatistics.setDefaultFilter();" />
                                            <input class="btn" type="button" id="btnToday" value="" onclick="gameStatistics.btnSpecialDateClick(1);gameStatistics.setDefaultFilter();" />
                                            <input class="btn" type="button" id="btnYesterday" value="" onclick="gameStatistics.btnSpecialDateClick(2);gameStatistics.setDefaultFilter();" />
                                            <input type="button" value="  " id="btnExportNew" class="btnExcel" title="Export to Excel"
                                                onclick="gameStatistics.exportExcel(false);" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="err">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="gridgamestatistic" dailyreport="True" class="content-warper hidden" style="margin-top: 6px;">
                        <div class="content-toolbox bgtitle">
                            <div id="gridtitlegamestatistic" class="gridtitle">
                            </div>
                            <div class="btngroup">
                                <input type="button" value="  " id="btnExport" class="btnExcel" title="Export to Excel"
                                    onclick="gameStatistics.exportExcel(true);" />
                            </div>
                        </div>
                        <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table class="tbl-header-scroll" id="tbreportheader">
                                </table>
                            </div>
                        </div>
                        <div class="content-body clearfix">
                            <div class="body-scroll">
                                <table id="tblbodygamestatistic" class="tbl-body-scroll">
                                </table>
                            </div>
                        </div>
                        <div class="content-footer clearfix">
                            <div id="footergamestatistic" class="footer-scroll">
                                <table class="tbl-footer-scroll" id="tblfootergamestatistic">
                                </table>
                            </div>
                        </div>
                        <div class="content-pagination bgtitle clearfix">
                            <div class="pagination-scroll">
                                <table style="width: 100%" cellspacing="0" cellpadding="1">
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
                </td>
            </tr>
        </table>
    </div>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
    <script language="javascript" type="text/javascript">
        gameStatistics.params.movebetsday = '<%=MoveBetDay %>';
        gameStatistics.params.balanceupday = '<%=BalanceUpDay %>';
        var timeoutErrorMessage = 'The Operation has timed out due to intensive processes. Please, reselect filter options appropriatedly to avoid the time-out situation.'

        var _today = '<%=DateTime.Now.ToShortDateString() %>';
        var _yesterday = '<%=DateTime.Now.AddDays(-1).ToShortDateString() %>';
        
    </script>
</body>
</html>
