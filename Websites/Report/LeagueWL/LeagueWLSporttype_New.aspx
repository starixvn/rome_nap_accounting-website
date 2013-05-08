<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeagueWLSporttype_New.aspx.cs"
    Inherits="Report_LeagueWL_LeagueWLSporttype_New" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="Style/LeagueWLSporttype.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/showhidepopup.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/MultipleSelect/jquery.multiselect.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="js/LeagueWLSporttype.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/LeagueWLBettype.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/LeagueWLBettypeDetail.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span>League Win Loss</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                Date:
                                <input type="text" id="date" readonly="readonly" class="txtdate" style="width: 100px;
                                    background-color: white;" />
                                <input type="button" onclick="leagueWL.getLeagueWLSporttype(true);" class="btn" value="REPORT"
                                    name="btnReport" id="btnReport" />
                                <div id="err" style="margin-top: 3px; display: block;">
                                </div>
                            </div>
                        </div>
                        <div class="contentBox" style="">
                            <div id="leaguewlgrid" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitle" class="gridtitle">
                                        League Win Loss Report
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="leagueWL.leagueWLExportExcel();" value="  " id="btnExport"
                                            class="btnExcel" title="Export to Excel" /></div>
                                </div>
                                <div class="content-header clearfix">
                                    <div id="header-scroll" class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col300 c">
                                                        League</div>
                                                </td>
                                                <td class="ibc" style="border-bottom: none 0px;">
                                                    <div class="col100  c">
                                                        Winloss</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Position</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Comm</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Line Total</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Margin</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div id="body-scroll" class="body-scroll">
                                        <table id="tblscroll" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer">
                                    <div id="footer-scroll" class="footer-scroll">
                                        <table class="tbl-footer-scroll" id="tblfooterscroll">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div id="leaguewlbettype" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="girdWLBettypeTitle" class="gridtitle">
                                        League Win Loss Report
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="leagueWLBettype.leagueWLGoBack();" value="  " name="btnBack"
                                            id="btnBackToMain" class="btnBack" title="Back To Main" />
                                        <input type="button" onclick="leagueWLBettype.leagueWLExportExcel();" value="  "
                                            id="btnExportWLBettype" class="btnExcel" title="Export to Excel" />
                                    </div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Bettype</div>
                                                </td>
                                                <td class="ibc" style="border-bottom: none 0px;">
                                                    <div class="col100  c">
                                                        Winloss</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Position</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Comm</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Line Total</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Margin</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tbWLByBettype" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer">
                                    <div class="footer-scroll">
                                        <table class="tbl-footer-scroll" id="tblfooterWLByBettype">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div id="leaguewlbettypeNoneLive" class="content-warper hidden" style="margin-top: 5px">
                                <div class="content-toolbox bgtitle">
                                    <div id="girdWLBettypeTitleNoneLive" class="gridtitle">
                                        League Win Loss Report
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="leagueWLBettype.leagueWLGoBack();" value="  " name="btnBack"
                                            id="btnBackToMainNoneLive" class="btnBack" title="Back To Main" />
                                        <input type="button" onclick="leagueWLBettype.leagueWLExportExcel();" value="  "
                                            id="btnExportWLBettypeNoneLive" class="btnExcel" title="Export to Excel" />
                                    </div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Bettype</div>
                                                </td>
                                                <td class="ibc" style="border-bottom: none 0px;">
                                                    <div class="col100  c">
                                                        Winloss</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Position</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Comm</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Line Total</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Margin</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tbWLByBettypeNoneLive" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer">
                                    <div class="footer-scroll">
                                        <table class="tbl-footer-scroll" id="tblfooterWLByBettypeNoneLive">
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
    <div id="dialogLeagueWLDetail" class="hidden">
        <div id="leaguewldetailgrid" class="content-warper">
            <div class="content-toolbox bgtitle">
                <div id="leaguewldetailgridtitle" class="gridtitle">
                    League Win Loss Report
                </div>
                <div class="btngroup">
                    <input type="button" onclick="leagueWLBettypeDetail.exportExcel();" value="  " id="btnLeagueWLDetailExport"
                        class="btnExcel" title="Export to Excel" /></div>
            </div>
            <div class="content-header clearfix">
                <div class="header-scroll">
                    <table class="tbl-header-scroll">
                        <tr>
                            <td style="border-bottom: none 0px;">
                                <div class="col100 c">
                                    Code</div>
                            </td>
                            <td class="ibc" style="border-bottom: none 0px;">
                                <div class="col200  c">
                                    Event</div>
                            </td>
                            <td style="border-bottom: none 0px;">
                                <div class="col100 c">
                                    Winloss</div>
                            </td>
                            <td style="border-bottom: none 0px;">
                                <div class="col100 c">
                                    Position</div>
                            </td>
                            <td style="border-bottom: none 0px;">
                                <div class="col100 c">
                                    Comm</div>
                            </td>
                            <td style="border-bottom: none 0px;">
                                <div class="col100 c">
                                    Line Total</div>
                            </td>
                            <td style="border-bottom: none 0px;">
                                <div class="col100 c">
                                    Turnover</div>
                            </td>
                            <td style="border-bottom: none 0px;">
                                <div class="col100 c">
                                    Margin</div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="content-body clearfix">
                <div id="Div3" class="body-scroll">
                    <table id="tblleaguewldetail" class="tbl-body-scroll">
                    </table>
                </div>
            </div>
            <div class="content-footer">
                <div class="footer-scroll">
                    <table class="tbl-footer-scroll" id="tblfooterleaguewldetail">
                    </table>
                </div>
            </div>
        </div>
    </div>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
</body>
<script type="text/javascript">
    leagueWL.BARM.IsActive = '<%=utilsBARM.IsActive %>';
    leagueWL.BARM.DisplayName = '<%=utilsBARM.DisplayName %>';

    leagueWLBettype.BARM.IsActive = '<%=utilsBARM.IsActive %>';
    leagueWLBettype.BARM.DisplayName = '<%=utilsBARM.DisplayName %>';

    leagueWLBettypeDetail.BARM.IsActive = '<%=utilsBARM.IsActive %>';
    leagueWLBettypeDetail.BARM.DisplayName = '<%=utilsBARM.DisplayName %>';

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
