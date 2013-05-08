<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConcurrentUser.aspx.cs" Inherits="Report_Deposit_ConcurrentUser" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../js/Calendar2/css/jscal2.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/commonIOM.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../MemberStatistics/Style/ActiveMembers.css" rel="stylesheet" type="text/css" />
    <link href="Style/ConcurrentUser.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="js/Concurrent.js?v=<%=WebSettings.Version %>" type="text/javascript"></script>
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
                                <li class="current"><span>Peak Concurrent User Report</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                <div style="display: none;" id="trMsg">
                                    <div id="dvMsg">
                                    </div>
                                </div>
                                <table>
                                    <tr>
                                        <td>
                                            From:
                                        </td>
                                        <td>
                                            <input type="text" id="fdate" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />
                                        </td>
                                        <td>
                                            To:
                                        </td>
                                        <td>
                                            <input type="text" id="tdate" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />
                                        </td>
                                        <td>
                                            Sites:
                                        </td>
                                        <td>
                                            <select id="siteList" style="width: 85px" onchange="ChangeSite()">
                                            </select>
                                        </td>
                                        <td>
                                            <input type="button" id="btnSearch" value="Report" class="btn" onclick="bindingPeak()" />
                                        </td>
                                        <td>
                                            <input type="button" value="" name="btnExport" id="btnExportIcon" class="btnExcel"
                                                style="display: none" onclick="Export2Excel()" title="Export to Excel" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="table_report" class="bg_table" style="width: 490px; display: none">
                                <div id="noData" style="display: none; text-align: center">
                                    <%=Resources.Language.thereisnodata %>
                                </div>
                                <div id="reportMain" style="display: none;">
                                    <div id="peakHeader">
                                        <div class="peakcontentLeft" style="border-top: none; border-bottom: none;">
                                            Date</div>
                                        <div class="peakcontentRight" style="border-top: none; border-bottom: none;">
                                            Peak Concurrent Users</div>
                                    </div>
                                    <div id="peakContent" class="content">
                                    </div>
                                    <div style="" id="peakTotal" class="peakFooter">
                                        <div id="peakTotalelf" class="peakcontentLeft" style="border-top: 1px solid #7C98A4">
                                            Total Peak Concurrent User
                                        </div>
                                        <div id="peakTotalRight" class="peakcontentRight" style="border-top: 1px solid #7C98A4">
                                        </div>
                                    </div>
                                    <div style="" id="peakAverage" class="peakFooter">
                                        <div id="averagePeak" class="peakcontentLeft">
                                            Average Peak Concurrent User
                                        </div>
                                        <div id="totalUser" class="peakcontentRight">
                                        </div>
                                    </div>
                                    <div style="" id="peakMax" class="peakFooter">
                                        <div id="peakMaxLelf" class="peakcontentLeft">
                                            Max Peak Cocurrent User
                                        </div>
                                        <div id="peakMaxRight" class="peakcontentRight">
                                        </div>
                                    </div>
                                    <div style="" id="peakMin" class="peakFooter minPeak">
                                        <div id="peakMinLelf" class="peakcontentLeftLastRow">
                                            Min Peak Concurrent User
                                        </div>
                                        <div id="peakMinRight" class="peakcontentRightLastRow">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <iframe id="iframePeark" src="" frameborder="0" style="display: none"></iframe>
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
        calendarUtils.renderCalendar("#tdate", null, "#fdate", "#tdate", tDay, options);
    });
    renderSite();
</script>
</html>
