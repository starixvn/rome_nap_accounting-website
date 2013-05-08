<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RacingDailyReportforTote.aspx.cs"
    Inherits="Report_Racing_RacingDailyReportforTote" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../js/jQuery/Bubblepopup/css/jquery.bubblepopup.v2.3.1.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="style/racingreport.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Bubblepopup/jquery.bubblepopup.v2.3.1.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="script/racingdailyfortote.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span>Racing Daily Report (for Tote)</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tr>
                                        <td>
                                            Date:
                                        </td>
                                        <td>
                                            <input type="text" id="date" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />
                                        </td>
                                        <td>
                                            <input type="button" onclick="racingDailyForTote.GetDataReport()" class="btn" value="REPORT"
                                                name="btnReport" id="btnReport" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="err">
                                </div>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="racingDailyForTote" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitle" class="gridtitle">
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="racingDailyForTote.exportExcel();" value="  " name="btnExport"
                                            id="btnExportExcelDetal" class="btnExcel" title="Export to Excel" /></div>
                                </div>
                                <div class="content-header clearfix">
                                    <div id="header-frozen-detail" class="header-frozen">
                                        <table class="tbl-header-frozen">
                                            <tr>
                                                <td>
                                                    <div class="col160">
                                                        &nbsp;
                                                    </div>
                                                </td>
                                                <td class="ibc">
                                                    <div class="col60">
                                                        &nbsp;
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col160 c">
                                                        Meeting-Currency</div>
                                                </td>
                                                <td style="border-bottom: none 0px;" class="ibc">
                                                    <div class="col60 c">
                                                        Race</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="header-scroll-detail" class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td colspan="6">
                                                    <div class="c">
                                                        Agency</div>
                                                </td>
                                                <td colspan="9" class="ibc">
                                                    <div class="c">
                                                        IBC</div>
                                                </td>
                                                <td colspan="8">
                                                    <div class="c">
                                                        Phumelela</div>
                                                </td>
                                            </tr>
                                            <tr class="bgborder">
                                                <!--Agency-->
                                                <td>
                                                    <div class="col100 c">
                                                        Win TO</div>
                                                </td>
                                                <td>
                                                    <div class="col100  c">
                                                        Place TO</div>
                                                </td>
                                                <td>
                                                    <div class="col100 c">
                                                        W/P TO</div>
                                                </td>
                                                <td>
                                                    <div class="col100 c">
                                                        Total TO</div>
                                                </td>
                                                <td>
                                                    <div class="col100 c">
                                                        Comm Income</div>
                                                </td>
                                                <td>
                                                    <div class="col100 c">
                                                        PT Win/Loss</div>
                                                </td>
                                                <!--IBC-->
                                                <td class="ibc">
                                                    <div class="col100 c">
                                                        Win TO</div>
                                                </td>
                                                <td class="ibc">
                                                    <div class="col100 c">
                                                        Place TO</div>
                                                </td>
                                                <td class="ibc">
                                                    <div class="col100 c">
                                                        W/P TO</div>
                                                </td>
                                                <td class="ibc">
                                                    <div class="col100 c">
                                                        Total TO</div>
                                                </td>
                                                <td class="ibc">
                                                    <div class="col100 c">
                                                        Comm Income</div>
                                                </td>
                                                <td class="ibc">
                                                    <div class="col100 c">
                                                        Comm Expense</div>
                                                </td>
                                                <td class="ibc">
                                                    <div class="col100 c">
                                                        PGI/Amtote Fee</div>
                                                </td>
                                                <td class="ibc">
                                                    <div class="col100 c">
                                                        PT Win/Loss</div>
                                                </td>
                                                <td class="ibc">
                                                    <div class="col100 c">
                                                        Net Income<input type="button" value="  " id="btnNetIncomeHelp" class="btnHelp" title="Formula" /></div>
                                                </td>
                                                <!--Partner-->
                                                <td>
                                                    <div class="col100 c">
                                                        Win TO</div>
                                                </td>
                                                <td>
                                                    <div class="col100 c">
                                                        Place TO</div>
                                                </td>
                                                <td>
                                                    <div class="col100 c">
                                                        W/P TO</div>
                                                </td>
                                                <td>
                                                    <div class="col100 c">
                                                        Total TO</div>
                                                </td>
                                                <td>
                                                    <div class="col100 c">
                                                        Dividend Won</div>
                                                </td>
                                                <td>
                                                    <div class="col100 c">
                                                        Comm Expense</div>
                                                </td>
                                                <td>
                                                    <div class="col100 c">
                                                        PGI/Amote Fee</div>
                                                </td>
                                                <td>
                                                    <div class="col100 c">
                                                        Amount Due<input type="button" value="  " id="btnAmountDueHelp" class="btnHelp" title="Formula" /></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-frozen">
                                        <table id="tblfrozen" class="tbl-body-frozen">
                                        </table>
                                    </div>
                                    <div class="body-scroll">
                                        <table id="tblscroll" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer">
                                    <div class="footer-frozen">
                                        <table id="tblfooterfrozen" class="tbl-footer-frozen">
                                        </table>
                                    </div>
                                    <div class="footer-scroll">
                                        <table class="tbl-footer-scroll" id="tblfooterscroll">
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
    var fromcache = false;
    var title = '<%=Resources.Language.titleRacingDailyReportforTote%>';
    var toDay = '<%= ToDay %>';
    var fDay = '<%= FDay %>';
    var minDay = '<%= MinDay %>';
    var maxDay = '<%= MaxDay %>';
    $(document).ready(function () {
        var limitFromDate = {
            minDate: minDay,
            maxDate: maxDay
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
