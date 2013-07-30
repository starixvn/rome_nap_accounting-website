<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DWMAMonitor.aspx.cs" Inherits="Report_Monitor_DWMAMonitor" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/Message.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Report/reportgrid.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" />
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="style/style.css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Pager/jpager.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="script/Datetime.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <link href="../../js/jQuery/autocomplete/styles.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/autocomplete/jquery.autocomplete_matool_v.01.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Number/autoNumeric-1.4.5.js" type="text/javascript"></script>
    <script src="script/monitor.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span>MA Monitor</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <table cellpadding="2" cellspacing="0" border="0" style="width: 100%;">
                                <tr style="height: 25px">
                                    <td>
                                        <b>Report by</b>
                                        <select class="ddl w120" id="ddlreportby">
                                        </select>
                                        <span class="currentreport" style="padding-left: 22px;"><b>Date from</b>
                                            <input class="txtdate" id="txtfromdate" readonly="readonly" />
                                        </span><span class="historyreport hidden" style="padding-left: 4px;"><b>In month</b>
                                            <select id="ddlinmonth" class="ddl w120" onchange="helper.changeInMonth(this);">
                                            </select>
                                        </span><span class="currentreport"><span style="padding-left: 20px"></span><b>To</b>
                                            <input class="txtdate" id="txttodate" readonly="readonly" /></span> <span style="padding-left: 20px">
                                            </span><span class="historyreport hidden"><b>From</b>
                                                <select class="ddl w40" id="ddlfromday" onchange="helper.changeFromDay();">
                                                </select>
                                                <span style="padding-left: 10px"></span><b>To</b>
                                                <select class="ddl w40" id="ddltoday" max="" onchange="helper.changeToDay();">
                                                </select>
                                            </span>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        <b>Product</b> <span id="productcontainer" style="padding-left: 8px;"></span>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        <b>Sport</b>
                                        <select class="ddl w120 LeagueControl" id="ddlsport" onchange="filterManager.getBettype();"
                                            style="margin-left: 25px">
                                        </select>
                                        <span style="padding-left: 22px"></span><b>Bet type</b>
                                        <select class="ddl w220 LeagueControl" id="ddlbettype" onchange="filterManager.getSelectLeagues();"
                                            style="margin-left: 8px">
                                        </select>
                                        <span style="padding-left: 22px"></span><b>Currency</b>
                                        <select class="ddl w80 LeagueControl" id="ddlcurrency" onchange="filterManager.getSelectLeagues();"
                                            style="">
                                        </select>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        <input type="button" onclick="filterManager.showSelectLeagues();" class="btn LeagueControl"
                                            value="Select Leagues" name="btnReport" id="btnselectleagues" style="width: 100px;" />
                                        <div id="selectleaguespopup" class="hidden">
                                            <div id="gridleagues" class="content-warper">
                                                <div class="content-header clearfix">
                                                    <div class="header-scroll">
                                                        <table class="tbl-header-scroll">
                                                            <tr>
                                                                <td style="border-bottom: none 0px;">
                                                                    <div class="w40 c">
                                                                        <input type="checkbox" checked="checked" id="chkallleagues" onclick="helper.checkAllSelectLeagues();" /></div>
                                                                </td>
                                                                <td style="border-bottom: none 0px;">
                                                                    <div class="w360 c sorter" id="lblselectleagues" onclick="filterManager.sortLeagues('lblselectleagues'); ">
                                                                        Select All Leagues</div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div class="content-body clearfix">
                                                    <div class="body-scroll">
                                                        <table id="tblselectleagues" class="tbl-body-scroll">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <div class="tinyloader" style="width: 403px;">
                                                                        &nbsp;</div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <span style="padding-left: 14px"></span><span id="leaguescontainer" class="LeagueControl">
                                        </span><span style="padding-left: 72px"></span><b>Live status</b> <span class="LeagueControl">
                                            <input type="radio" id="rdall" value="0" name="livetype" checked="checked" onclick="filterManager.getSelectLeagues();" />All&nbsp;&nbsp;<input
                                                type="radio" id="rdnonelive" value="1" name="livetype" onclick="filterManager.getSelectLeagues();" />Non-Live&nbsp;&nbsp;<input
                                                    type="radio" id="rdlive" name="livetype" value="2" onclick="filterManager.getSelectLeagues();" />Live
                                        </span>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        <b>Turnover</b>
                                        <div style="width: 255px; border: solid 1px #818181; text-align: center; background-color: #FFFFFF;
                                            display: inline-block; margin-left: 6px;" title="Enter the amount range. Eg: 100,000- 200,000">
                                            <input type="text" id="txtTurnover1" style="text-align: center; border: hidden; width: 105px;
                                                padding: 2px;" maxlength="23" onkeypress="monitorManager.OnKeyPress(event, 'txtTurnover2');" />
                                            <span><b>-</b></span>
                                            <input type="text" id="txtTurnover2" style="text-align: center; border: hidden; width: 105px;
                                                padding: 2px;" maxlength="23" onkeypress="monitorManager.OnKeyPress(event, 'txtWinLoss1');" />
                                        </div>
                                        <b style="padding-left: 24px; padding-right: 2px">Win Loss</b>
                                        <div style="width: 255px; border: solid 1px #818181; text-align: center; background-color: #FFFFFF;
                                            display: inline-block;" title="Enter the amount range. Eg: 100,000- 200,000">
                                            <input type="text" id="txtWinLoss1" style="text-align: center; border: hidden; width: 105px;
                                                padding: 2px;" maxlength="23" onkeypress="monitorManager.OnKeyPress(event, 'txtWinLoss2');" />
                                            <span><b>-</b></span>
                                            <input type="text" id="txtWinLoss2" style="text-align: center; border: hidden; width: 105px;
                                                padding: 2px;" maxlength="23" onkeypress="monitorManager.OnKeyPress(event, 'ddlreportby');" />
                                        </div>
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        <b>Username</b>
                                        <input type="text" title="Support crossed-level and multiple usernames searching seperated by a comma."
                                            class="autocomplete username" size="8000" maxlength="8000" id="txtusername" style="overflow: hidden;
                                            width: 468px; padding: 2px;" />
                                        <input type="button" onclick="monitorManager.getMonitorReport();" disabled="disabled"
                                            class="btn LeagueControl" value="REPORT" name="btnReport" id="btnReport" />
                                        <input type="button" onclick="filterManager.resetFilter();" class="btn LeagueControl"
                                            value="Reset" name="btnReport" id="Button2" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        <div style="margin-bottom: 5px; margin-top: 10px; padding: 0 2px;" class="warning l"
                                            id="dvMsg">
                                            <ul>
                                                <li><span id="spMsg"><span id="spTitleViewBalanceUpDay">You can see the report data
                                                    from latest Balance day up to yesterday. For more historic data, please click to</span>
                                                    <span id="btnviewhistory" onclick="filterManager.changeReportType();" class="canchange">
                                                        View history report</span></span></li></ul>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="err">
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="gridmonitor" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitlemonitor" class="gridtitle">
                                        MA Monitor
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" value="  " id="btnExport" class="btnExcel" title="Export to Excel"
                                            onclick="monitorManager.exportExcel();" />
                                    </div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div id="colTitle" class="c">
                                                        Date</div>
                                                </td>
                                                <td colspan="3">
                                                    <div class="c">
                                                        Member</div>
                                                </td>
                                                <td colspan="2">
                                                    <div class="c">
                                                        Agent</div>
                                                </td>
                                                <td colspan="2">
                                                    <div class="c">
                                                        Master</div>
                                                </td>
                                                <td colspan="2">
                                                    <div class="c">
                                                        Super</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="w100 c">
                                                        Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="w100 c">
                                                        WinLoss</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="w90 c">
                                                        BetCount</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="w100 c">
                                                        Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="w100 c">
                                                        WinLoss</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="w100 c">
                                                        Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="w100 c">
                                                        WinLoss</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="w100 c">
                                                        Turnover</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="w100 c">
                                                        WinLoss</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tblbodyscrollmonitor" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer clearfix">
                                    <div id="footerscrollmonitor" class="footer-scroll">
                                        <table class="tbl-footer-scroll" id="tblfooterscrollmonitor">
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
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
</body>
<script type="text/javascript">
    $(document).ready(function () {
        $("#txtusername").autocomplete({ serviceUrl: '../../Handlers/QueryUserName.ashx', minChars: 2, delimiter: "," });
        $('.username').mouseover(function () {
            isonauto = true;
        }).mouseout(function () {
            isonauto = false;
        });

        $('#txtTurnover1').autoNumeric();
        $('#txtTurnover2').autoNumeric();
        $('#txtWinLoss1').autoNumeric();
        $('#txtWinLoss2').autoNumeric();
    });
</script>
</html>
