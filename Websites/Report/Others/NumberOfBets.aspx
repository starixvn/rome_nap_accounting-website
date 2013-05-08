<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NumberOfBets.aspx.cs" Inherits="Report_Deposit_NumberOfBets" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../App_Themes/Public/commonIOM.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="Style/NumberOfBets.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.bk.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="js/NumberofBets.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span>
                                    <%=Resources.Language.lblNumberOfBets %></span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
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
                                            <input type="button" id="btnSearch" value="Report" class="btn" onclick="numberOfBets.getReport()" />
                                        </td>
                                        <td>
                                            <input type="button" value="" name="btnExport" id="btnExportIcon" class="btnExcel"
                                                onclick="numberOfBets.exportExcel()" title="Export to Excel" style="display: none;" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="ErrMsg" style="text-align: center">
                            </div>
                        </div>
                        <div id="mainContentReport" class="contentBox" style="width: 372px; display: none;">
                            <div id="numberofbets" class="countBet-Main">
                                <div id="countbetheader" class="countBet-Header">
                                    <div class="countBet-Header-left">
                                        <%=Resources.Language.Site %></div>
                                    <div class="countBet-Header-right">
                                        <%=Resources.Language.lblBetCount %></div>
                                </div>
                                <div id="countbetdetail" class="countBet-content">
                                </div>
                                <div id="countbetFooter" class="countBet-Header">
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <iframe id="frm" style="display: none;"></iframe>
</body>
<script language="javascript" type="text/javascript">
    var nodataerror = '<%=Resources.Language.thereisnodata %>';
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
</script>
</html>
