<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TraderPerformance.aspx.cs"
    Inherits="Report_TraderPerformance_TraderPerformance" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/jquery-easyui-1.2.1/themes/default/easyui.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/jquery-easyui-1.2.1/themes/icon.css" rel="stylesheet"
        type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Report/TraderPerformance.css?version=<%=WebSettings.Version%>"
        rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/GridScroller/gridscroller.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../App_Themes/jquery-easyui-1.2.1/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/ReportList.js?version=<%=WebSettings.Version%>"></script>
    <script src="../../js/jQuery/GridScroller/jquery.gridscroller.1.0.js?version=<%=WebSettings.Version%>"
        type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="Scripts/TraderPerformance.js?version=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span>Trader Performance</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="width: 100%;">
                        <div class="filterBox">
                            <div class="filterContent">
                                <div style="padding-bottom: 10px;" id="header_main">
                                    <div class="b" style="display: inline-block;">
                                        <%=PermissionFunction%>
                                    </div>
                                    <div style="padding-left: 10px; display: inline-block;">
                                        <b><font color="Black">Risk Controller Report</font></b>&nbsp;|&nbsp;<span onclick="traderPerformanceManager.loadTraderDetail()"
                                            class="lnk" id="reportDetail">DETAIL</span> <span style="padding-left: 10px;">
                                                <input type="button" onclick="traderPerformanceManager.export2Excel();" value="   "
                                                    name="btnExport" id="btnExportIcon" class="btnExcel" title="Export to Excel" />
                                            </span>
                                        <input type="hidden" value="1" name="IsSoccer" id="IsSoccer" />
                                        <input type="submit" style="display: none;" value="export" name="export" id="export" />
                                    </div>
                                </div>
                                <div style="clear: both;">
                                </div>
                                <div>
                                    <span class="padright b">From:</span>
                                    <input type="text" id="fdate" readonly="readonly" class="txtdate" style="width: 100px;
                                        background-color: white;" />
                                    <span class="pL20 b">To:</span>
                                    <input type="text" id="tdate" readonly="readonly" class="txtdate" style="width: 100px;
                                        background-color: white;" />
                                    <span style="padding-left: 5px; vertical-align: middle;">
                                        <input type="button" onclick="traderPerformanceManager.viewReport();" class="btn"
                                            value="REPORT" name="btnReport" id="btnReport" />
                                    </span>
                                    <input type="hidden" id="sortColName" value="" />
                                    <input type="hidden" id="sortType" value="asc" />
                                </div>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="traderPerformanceReport" class="freegrid hidden" style="">
                                <div class="grid-title">
                                    <div class="grid-title-lable" id="gridtitleSummary">
                                        Title</div>
                                </div>
                                <div class="free-cell free-top-left">
                                    <table class="grid-fixed-table free-cell-top-left" style="height: 61px;">
                                        <thead>
                                            <tr>
                                                <th>
                                                    <div class="col30 c table-cell" style="height: 61px;">
                                                        No</div>
                                                </th>
                                                <th>
                                                    <div class="col100 c table-cell" style="height: 61px;">
                                                        Trader</div>
                                                </th>
                                                <th>
                                                    <div class="col40 table-cell" style="height: 61px;">
                                                        Match</div>
                                                </th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                                <div class="row_header_wrapper">
                                    <div class="row_header row-headers-background" id="traderPerformanceReport_RowHeader">
                                        <table class="grid-fixed-table row_header_content">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        <div class="col30 c table-cell" style="height: 61px;">
                                                            No</div>
                                                    </th>
                                                    <th>
                                                        <div class="col100 c table-cell" style="height: 61px;">
                                                            Trader</div>
                                                    </th>
                                                    <th>
                                                        <div class="col40 table-cell" style="height: 61px;">
                                                            Match</div>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="col_header row-headers-background scroll-y">
                                    <div class="header-container">
                                        <table class="grid-fixed-table col_header_content">
                                            <thead>
                                                <tr>
                                                    <th rowspan="2">
                                                        <div class="col30 c table-cell" style="height: 61px;">
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="col100 c table-cell" style="height: 61px;">
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="col40 table-cell" style="height: 61px;">
                                                        </div>
                                                    </th>
                                                    <th colspan="15">
                                                        <div class="colHeaderSoccer c table-cell" style="height: 17px;">
                                                            Winlost</div>
                                                    </th>
                                                    <th colspan="15">
                                                        <div class="colHeaderSoccer c table-cell" style="height: 17px;">
                                                            Turnover</div>
                                                    </th>
                                                    <th colspan="15">
                                                        <div class="colHeaderSoccer c table-cell" style="height: 17px;">
                                                            Live BetCount</div>
                                                    </th>
                                                    <th colspan="15">
                                                        <div class="colHeaderSoccer c table-cell" style="height: 17px;">
                                                            Margin %</div>
                                                    </th>
                                                    <th colspan="15">
                                                        <div class="colHeaderSoccer c table-cell" style="height: 17px;">
                                                            Commission</div>
                                                    </th>
                                                    <th rowspan="2" align="center">
                                                        <div class="col100 c table-cell" style="height: 61px;">
                                                            Trader</div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st 1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            15mins O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            Next Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col90 c table-cell" style="height: 39px;">
                                                            Overall</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st 1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            15mins O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            Next Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            Overall</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st 1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            15mins O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            Next Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            Overall</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st 1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            15mins O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            Next Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            Overall</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st 1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            15mins O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            Next Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            1st O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c table-cell" style="height: 39px;">
                                                            Overall</div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                </div>
                                <div class="grid-container container">
                                    <div class="content-container">
                                        <table id="traderPerformanceReport_Container" class="grid-fixed-table col_header_content">
                                            <thead>
                                                <tr>
                                                    <th rowspan="2">
                                                        <div class="col30 c table-cell" style="height: 61px;">
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="col100 c table-cell" style="height: 61px;">
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="col40 table-cell" style="height: 61px;">
                                                        </div>
                                                    </th>
                                                    <th colspan="15">
                                                        <div class="colHeaderSoccer c table-cell" style="height: 17px;">
                                                            Winlost</div>
                                                    </th>
                                                    <th colspan="15">
                                                        <div class="colHeaderSoccer c table-cell" style="height: 17px;">
                                                            Turnover</div>
                                                    </th>
                                                    <th colspan="15">
                                                        <div class="colHeaderSoccer c table-cell" style="height: 17px;">
                                                            Live BetCount</div>
                                                    </th>
                                                    <th colspan="15">
                                                        <div class="colHeaderSoccer c table-cell" style="height: 17px;">
                                                            Margin %</div>
                                                    </th>
                                                    <th colspan="15">
                                                        <div class="colHeaderSoccer c table-cell" style="height: 17px;">
                                                            Commission</div>
                                                    </th>
                                                    <th rowspan="2" align="center">
                                                        <div class="col100 c table-cell" style="height: 61px;">
                                                            Trader</div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st 1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            15mins O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            Next Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col90 c" style="height: 39px;">
                                                            Overall</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st 1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            15mins O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            Next Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            Overall</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st 1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            15mins O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            Next Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            Overall</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st 1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            15mins O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            Next Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            Overall</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st 1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1x2</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            HDP</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            No. of Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            No. of Yellow Card</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            15mins O/U</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            Next Corner</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            1st O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            O/E</div>
                                                    </th>
                                                    <th>
                                                        <div class="col80 c" style="height: 39px;">
                                                            Overall</div>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="row_footer row-footer-background z999">
                                    <table class="grid-fixed-table row_footer_content">
                                        <thead>
                                        </thead>
                                    </table>
                                </div>
                                <div class="col_footer row-footer-background scroll-y">
                                    <div class="footer-container">
                                        <table class="col_footer_content grid-fixed-table">
                                            <thead>
                                            </thead>
                                        </table>
                                    </div>
                                </div>
                                <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</body>
<script type="text/javascript">
     var hasTraderSoccerPermission = <%=HasTraderSoccerPermission%>;
     var hasTraderOtherSportsPermission = <%=HasTraderOtherSportsPermission%>;
     var toDay = '<%= ToDay %>';
     var fDay = '<%= FDay %>';
     var tDay = '<%= TDay %>';
     var minDay = '<%= MinDay %>';
</script>
</html>
