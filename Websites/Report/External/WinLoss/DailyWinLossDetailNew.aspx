<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyWinLossDetailNew.aspx.cs"
    Inherits="Report_External_WinLoss_DailyWinLossDetailNew" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Bingo.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/input.css" />
    <link href="../../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="styles/dailywinloss.css" rel="stylesheet" type="text/css" />
    <link href="../../../js/jQuery/monthPicker/monPickr-0.0.1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Win/Loss Detail
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width: 1042px;">
                <table>
                    <tr>
                        <td>
                            <table id="tblDate1">
                                <tr>
                                    <td selectkype="1">
                                        From:
                                    </td>
                                    <td selectkype="1">
                                        <%=strFDate%>
                                    </td>
                                    <td selectkype="1">
                                        To:
                                    </td>
                                    <td selectkype="1">
                                        <%=strTDate%>
                                    </td>
                                </tr>
                            </table>
                            <table id="tblDate2" style="display: none">
                                <tr>
                                    <td selectkype="2">
                                        <label>
                                            View data in month
                                        </label>
                                    </td>
                                    <td selectkype="2">
                                        <div id="monPickr1">
                                        </div>
                                    </td>
                                    <td selectkype="2">
                                        From
                                    </td>
                                    <td selectkype="2">
                                        <div id="fromDay1">
                                        </div>
                                    </td>
                                    <td selectkype="2">
                                        To
                                    </td>
                                    <td selectkype="2">
                                        <div id="toDay1">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <input type="button" onclick="superWinLossReportDetail.getWinLossReport(1)" class="btn"
                                            value="REPORT" name="btnReport" id="btnReport" />
                                        <input type="button" onclick="superWinLossReportDetail.getWinLossReport(2)" class="btn"
                                            value="<%=DateTime.Now.ToShortDateString() %>" name="btnReport" id="btnCurrentDateReport" />
                                        <input type="button" onclick="superWinLossReportDetail.getWinLossReport(3)" class="btn"
                                            value="<%=DateTime.Now.AddDays(-1).ToShortDateString() %>" name="btnReport" id="btnCurrentYesterdayReport" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td>
                            <input type="checkbox" checked value="All" id="chkAll" />
                            <label for="chkAll">
                                <span style="padding-top: 2px">All</span></label>&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <span id="chkItems">
                                <input id="chkItems_0" name="chkItems_0" checked="checked" type="checkbox" /><label
                                    for="chkItems_0">SB</label>&nbsp;&nbsp;&nbsp;<input id="chkItems_1" name="chkItems_1"
                                        checked="checked" type="checkbox" /><label for="chkItems_1">CS</label>&nbsp;&nbsp;&nbsp;<input
                                            id="chkItems_2" name="chkItems_2" checked="checked" type="checkbox" /><label for="chkItems_2">Racing</label>&nbsp;&nbsp;&nbsp;<input
                                                id="chkItems_3" name="chkItems_3" checked="checked" type="checkbox" /><label for="chkItems_3">Finance</label>
                                <%if (BingoSettings.BingoGame.Enabled)
                                  { %>
                                <!--no-bingo-->
                                &nbsp;&nbsp;&nbsp;<input id="chkItems_4" name="chkItems_4" checked="checked" type="checkbox" /><label
                                    for="chkItems_4">Bingo</label>
                                <!--/no-bingo-->
                                <% } %>
                                <%if (BingoSettings.NumberGame.Enabled)
                                  {%>
                                <!--no-numbergame-->
                                &nbsp;&nbsp;&nbsp;<input id="chkItems_5" name="chkItems_5" checked="checked" type="checkbox" /><label
                                    for="chkItems_5">Number Game</label>
                                <!--/no-numbergame-->
                                <% } %>
                                <!--no-p2p-->
                                &nbsp;&nbsp;&nbsp;<input id="chkItems_6" name="chkItems_6" checked="checked" type="checkbox" /><label for="chkItems_6">Poker</label>
                                <!--/no-p2p-->
                                <%if (Sunplus.Accounting.Core.Configurations.WebSettings.EnableLiveCasino)
                                  {%>
                              &nbsp;&nbsp;<input id="chkItems_7" name="chkItems_7" checked="checked" type="checkbox" /><label for="chkItems_7">Live Casino</label>
                              <%} %>
                              &nbsp;&nbsp;<input id="chkItems_8" name="chkItems_8" checked="checked" type="checkbox" /><label for="chkItems_8">Virtual Sports</label>
                            </span>
                        </td>
                    </tr>
                </table>
                <div id="err">
                </div>
                <br />
                <img src="images/more.gif" />
                <span id="spMsg" style="margin-left: 4px; font-family: Tahoma,Arial,helvetica,sans-serif;
                    font-size: 11px; color: #90979A; font-weight: bold">You can see the report data
                    up to 3 months. For more historic data, please click to <a onclick='javascript:SwitchCalendarType(1);'
                        href='#'>View History Report</a></span>
            </div>
            <div id="bg_table" style="margin-top: 2px;">
                <div id="superWLGrid" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="superWLGridTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">
                            <input type="button" onclick="superWinLossReportDetail.exportExcel();" value="  "
                                id="btnSuperExport" class="btnExcel" title="Export to Excel" /></div>
                    </div>
                    <div class="content-header clearfix">
                        <div id="header-frozen" class="header-frozen">
                            <table class="tbl-header-frozen">
                                <tr>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col100  c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'UserName', 1);">
                                            Username</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'Turnover', 1);">
                                            Turnover</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'GrossComm', 1);">
                                            GrossComm</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="header-scroll" class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="3">
                                        <div class="c">
                                            Member</div>
                                    </td>
                                    <td colspan="3">
                                        <div class="c">
                                            Agent</div>
                                    </td>
                                    <td colspan="3">
                                        <div class="c">
                                            Master</div>
                                    </td>
                                    <td colspan="3">
                                        <div class="c">
                                            Super</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'Company', 1);">
                                            Company</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c  sorter" onclick="superWinLossReportDetail.sortingReport(this, 'WinLoss', 1);">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'Comm', 1);">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'Total', 1);">
                                            Total</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'AWinLoss', 1);">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'AComm', 1);">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'ATotal', 1);">
                                            Total</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'MWinLoss', 1);">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'MComm', 1);">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'MTotal', 1);">
                                            Total</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'SWinLoss', 1);">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'SComm', 1);">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="superWinLossReportDetail.sortingReport(this, 'STotal', 1);">
                                            Total</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div id="body-frozen" class="body-frozen">
                            <table id="tblSuperWLfrozen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div id="body-scroll" class="body-scroll">
                            <table id="tblSuperWL" class="tbl-body-scroll">
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div id="footer-frozen" class="footer-frozen">
                            <table id="tblfooterSuperWLfrozen" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div id="footer-scroll" class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblfooterSuperWL">
                            </table>
                        </div>
                    </div>
                </div>
                <div id="masterWLGrid" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="masterWLGridTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">
                            <input type="button" onclick="masterWinLossReportDetail.goback();" value="  " id="btnGoBackSuper"
                                class="btnBack" title="Back To Super" />
                            <input type="button" onclick="masterWinLossReportDetail.exportExcel();" value="  "
                                id="btnMasterExport" class="btnExcel" title="Export to Excel" /></div>
                    </div>
                    <div class="content-header clearfix">
                        <div class="header-frozen">
                            <table class="tbl-header-frozen">
                                <tr>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col100  c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'UserName');">
                                            Username</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'Turnover');">
                                            Turnover</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'GrossComm');">
                                            GrossComm</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="3">
                                        <div class="c">
                                            Member</div>
                                    </td>
                                    <td colspan="3">
                                        <div class="c">
                                            Agent</div>
                                    </td>
                                    <td colspan="3">
                                        <div class="c">
                                            Master</div>
                                    </td>
                                    <td colspan="3">
                                        <div class="c">
                                            Super</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'Company');">
                                            Company</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'WinLoss');">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'Comm');">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'Total');">
                                            Total</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'AWinLoss');">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'AComm');">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'ATotal');">
                                            Total</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'MWinLoss');">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'MComm');">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'MTotal');">
                                            Total</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'SWinLoss');">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'SComm');">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="masterWinLossReportDetail.sortingReport(this, 'STotal');">
                                            Total</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-frozen">
                            <table id="tblMasterWLfrozen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div class="body-scroll">
                            <table id="tblMasterWL" class="tbl-body-scroll">
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div class="footer-frozen">
                            <table id="tblfooterMasterWLfrozen" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblfooterMasterWL">
                            </table>
                        </div>
                    </div>
                </div>
                <div id="agentWLGrid" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="agentWLGridTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">
                            <input type="button" onclick="" value="  " id="btnGoBackMaster" class="btnBack" title="Back To" />
                            <a href="#" onclick="masterWinLossReportDetail.goback();">Super</a>| <a href="#"
                                onclick="agentWinLossReportDetail.goback();">Master</a>
                            <input type="button" onclick="agentWinLossReportDetail.exportExcel();" value="  "
                                id="btnExportAgent" class="btnExcel" title="Export to Excel" /></div>
                    </div>
                    <div class="content-header clearfix">
                        <div class="header-frozen">
                            <table class="tbl-header-frozen">
                                <tr>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col100  c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'UserName');">
                                            Username</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'Turnover');">
                                            Turnover</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'GrossComm');">
                                            GrossComm</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="3">
                                        <div class="c">
                                            Member</div>
                                    </td>
                                    <td colspan="3">
                                        <div class="c">
                                            Agent</div>
                                    </td>
                                    <td colspan="3">
                                        <div class="c">
                                            Master</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'Company');">
                                            Company</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'WinLoss');">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'Comm');">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'Total');">
                                            Total</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'AWinLoss');">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'AComm');">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'ATotal');">
                                            Total</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'MWinLoss');">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'MComm');">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="agentWinLossReportDetail.sortingReport(this, 'MTotal');">
                                            Total</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-frozen">
                            <table id="tblAgentWLfrozen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div class="body-scroll">
                            <table id="tblAgentWL" class="tbl-body-scroll">
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div class="footer-frozen">
                            <table id="tblfooterAgentWLfrozen" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblfooterAgentWL">
                            </table>
                        </div>
                    </div>
                </div>
                <div id="memberWLGrid" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="memberWLGridTitle" class="gridtitle">
                        </div>
                        <div class="btngroup">
                            <input type="button" onclick="" value="  " id="btnGoBackAgent" class="btnBack" title="Back To" />
                            <a href="#" onclick="masterWinLossReportDetail.goback();">Super</a>| <a href="#"
                                onclick="agentWinLossReportDetail.goback();">Master</a>| <a href="#" onclick="memberWinLossReportDetail.goback();">
                                    Agent</a>
                            <input type="button" onclick="memberWinLossReportDetail.exportExcel();" value="  "
                                id="btnExportMember" class="btnExcel" title="Export to Excel" /></div>
                    </div>
                    <div class="content-header clearfix">
                        <div class="header-frozen">
                            <table class="tbl-header-frozen">
                                <tr>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col100  c sorter" onclick="memberWinLossReportDetail.sortingReport(this, 'UserName');">
                                            Username</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="memberWinLossReportDetail.sortingReport(this, 'Turnover');">
                                            Turnover</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="memberWinLossReportDetail.sortingReport(this, 'GrossComm');">
                                            GrossComm</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td colspan="3">
                                        <div class="c">
                                            Member</div>
                                    </td>
                                    <td colspan="3">
                                        <div class="c">
                                            Agent</div>
                                    </td>
                                    <td style="border-bottom: none 0px;" rowspan="2">
                                        <div class="col120 c sorter" onclick="memberWinLossReportDetail.sortingReport(this, 'Company');">
                                            Company</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="memberWinLossReportDetail.sortingReport(this, 'WinLoss');">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="memberWinLossReportDetail.sortingReport(this, 'Comm');">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="memberWinLossReportDetail.sortingReport(this, 'Total');">
                                            Total</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="memberWinLossReportDetail.sortingReport(this, 'AWinLoss');">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="memberWinLossReportDetail.sortingReport(this, 'AComm');">
                                            Comm</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c sorter" onclick="memberWinLossReportDetail.sortingReport(this, 'ATotal');">
                                            Total</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-frozen">
                            <table id="tblMemberWLfrozen" class="tbl-body-frozen">
                            </table>
                        </div>
                        <div class="body-scroll">
                            <table id="tblMemberWL" class="tbl-body-scroll">
                            </table>
                        </div>
                    </div>
                    <div class="content-footer">
                        <div class="footer-frozen">
                            <table id="tblfooterMemberWLfrozen" class="tbl-footer-frozen">
                            </table>
                        </div>
                        <div class="footer-scroll">
                            <table class="tbl-footer-scroll" id="tblfooterMemberWL">
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
</body>
<script src="../../../js/jQuery/jquery-1.7.js" type="text/javascript"></script>
<script src="../../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
<script src="../../../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
<script src="../../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
<script src="../../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
<script src="../../../js/BlockUI.js" type="text/javascript"></script>
<script src="../../../js/common.js" type="text/javascript"></script>
<script src="../../../js/jQuery/gridscroller.js" type="text/javascript"></script>
<script src="js/ExternalDailyWinLossDetail.js?v=<%=WebSettings.Version %>" type="text/javascript"></script>
<script src="../../../js/jQuery/monthPicker/monPickr-0.0.1.js" type="text/javascript"></script>
<script>
    var maxdate = '<%= maxDate %>';
    var serverDate = new Date('<%= DateTime.Now.ToString() %>');
    var clientDate = new Date();
    var diffHour = Math.round((serverDate.getTime() - clientDate.getTime()) / (1000 * 60 * 60));
    
</script>
</html>
