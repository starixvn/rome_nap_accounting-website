<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TraderPerformance01.aspx.cs" Inherits="Report_TraderPerformance_TraderPerformance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css"/> 
    <link href="../../App_Themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/TraderPerformance.css" rel="stylesheet" type="text/css" />

    <link href="../../js/Calendar2/css/jscal2.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/jquery-easyui-1.2.1/themes/default/easyui.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/jquery-easyui-1.2.1/themes/icon.css" rel="stylesheet"
        type="text/css" />
    <style type="text/css">
        .Tur{background-color: #CCFFFF  !important;}
        .Win{background-color: #FFFF99 !important;}
        .Com{background-color: #ffffee !important;}
        .Mar{background-color: #DFEAFF  !important;}
        .Liv{background-color: #FFDDFF  !important;}
        .Mat{text-align:center; width:42px;white-space: normal;}
        
    </style>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table cellspacing="0px" cellpadding="0px" border="0" class="maincontent_header">
                <tbody>
                    <tr>
                        <td class="maincontent_headerL">
                        </td>
                        <td class="maincontent_headerC">
                            Trader Performance
                        </td>
                        <td class="maincontent_headerR">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="maincontent_box">
                <div style="padding-bottom: 10px;" id="header_main">
                    <div>
                        <b><span id="soccer" onclick="SwitchViewTrader(this);" class="">SOCCER</span>&nbsp;|&nbsp;<span
                            id="other" onclick="SwitchViewTrader(this);" class="lnk">OTHER SPORT</span>&nbsp;Risk
                            Controller Report</b>&nbsp;|&nbsp;<span onclick="LoadTraderDetail()" class="lnk">DETAIL</span>
                            <span style="padding-left: 10px;">
                                <input type="button" onclick="ExportToExcel();" value="   " name="btnExport" id="btnExportIcon" class="btnExcel" title="Export to Excel" />
                            </span>
                                    <input type="hidden" value="1" name="IsSoccer" id="IsSoccer" />
                                    <input type="submit" style="display: none;" value="export" name="export" id="export" />
                    </div>
                    <div style="clear: both;"></div>
                    <div>
                        <span class="padright b">From:</span> <%=FDate %>
                        <span class="pL20 b">To:</span> <%=TDate%>
                        
                        <span style="padding-left: 5px; vertical-align: middle;">
                            <select id="ddlSelectType">
                                <option value="winlost">WinLost</option>
                                <option value="turnover">TurnOver</option>
                                <option value="livebetcount">Live BetCount</option>
                                <option value="margin">Margin</option>
                                <option value="commission">Commission</option>
                            </select>
                        </span>
                        <span style="padding-left: 5px; vertical-align: middle;">
                            <input type="button" onclick="getTranderPerformanceReport('0');" class="btn" value="REPORT" name="btnReport" id="btnReport" />
                        </span>
                        <input type="hidden" id="sortColName" value=""/>
                        <input type="hidden" id="sortType" value="asc"/>
                    </div>
                    
                </div>
            </div>

            <div class="bg_table">
                <div id="dvTraderPerformance">
                   
                </div>
            </div>
        </div>
    </div>

    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery-ui-1.8.2.custom.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <%--<script src="../../js/stdlib.js" type="text/javascript"></script>--%>
    <script src="../../App_Themes/jquery-easyui-1.2.1/jquery.easyui.min.js" type="text/javascript"></script>
    <%--<script src="../../App_Themes/jquery-easyui-1.2.1/datagrid-groupview.js" type="text/javascript"></script>--%>
    <script src="traderperformance.js" type="text/javascript"></script>

    
</body>
</html>
