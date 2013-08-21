<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MatchWinLossNew.aspx.cs"
    Inherits="Report_NumberGame_MatchWinLossNew" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Report/reportgrid.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/Message.css" />
    <link rel="stylesheet" type="text/css" href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" />
    <link rel="stylesheet" type="text/css" href="../../js/MultipleSelect/jquery.multiselect.css" />
    <link rel="stylesheet" type="text/css" href="Styles/numbergamereport.css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="JS/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/MultipleSelect/jquery.multiselect.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="JS/numbergamewinlossbymatchnew.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span>Number Game Win Loss By Match</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tr>
                                        <td style="padding-right: 4px">
                                            <div>
                                                Company:</div>
                                        </td>
                                        <td>
                                            <div>
                                                <select id="selectCompany" style="width: 100px; height: 18px;" multiple="multiple">
                                                </select>
                                            </div>
                                        </td>
                                        <td style="padding-left: 8px">
                                            <div>
                                                Currency:</div>
                                        </td>
                                        <td style="padding-left: 4px">
                                            <select id="selectCurrency" style="width: 100px; height: 18px;" multiple="multiple">
                                            </select>
                                        </td>
                                        <td style="padding-left: 8px">
                                            <div>
                                                Bettypes:</div>
                                        </td>
                                        <td style="padding-left: 4px">
                                            <select id="selectBettypes" style="width: 140px; height: 18px;" multiple="multiple">
                                            </select>
                                        </td>
                                        <td style="padding-left: 8px">
                                            <div>
                                                Date:
                                            </div>
                                        </td>
                                        <td>
                                            <input type="text" id="date" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />
                                        </td>
                                        <td style="padding-left: 8px">
                                            <input type="button" onclick="numberGameWinLossByMatch.GetDataReport()" class="btn"
                                                value="REPORT" name="btnReport" id="btnReport" />
                                        </td>
                                    </tr>
                                    <tr id="backbtn" style="display: none;">
                                        <td colspan="2">
                                            <a href="#" onclick="backtomain();">Back To Main</a>
                                        </td>
                                    </tr>
                                </table>
                                <div id="err">
                                </div>
                            </div>
                        </div>
                        <div class="contentBox" style="">
                            <div id="matchwinlost" class="content-warper" style="display: none;">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitle" class="gridtitle">
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="numberGameWinLossByMatch.exportExcel();" value="  "
                                            name="btnExport" id="btnExport" class="btnExcel" title="Export to Excel" /></div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr class="bgborder">
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col160 c">
                                                        Match - Bet Type</div>
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
                                                        Commission</div>
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
                                        <table id="tblmatchwinlost" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer">
                                    <div class="footer-scroll">
                                        <table class="tbl-footer-scroll">
                                            <tr>
                                                <td>
                                                    <div class="col160 l">
                                                        Grand Total</div>
                                                </td>
                                                <td>
                                                    <div id="totalWinlost" class="col100 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div id="totalPosition" class="col100 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div id="totalCommission" class="col100 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div id="totalLineTotal" class="col100 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div id="totalTurnOver" class="col100 r">
                                                    </div>
                                                </td>
                                                <td>
                                                    <div id="totalMargin" class="col100 r">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="detailreport">
                        <div id="matchwinlostdetail" class="content-warper " style="display: none;">
                            <div class="content-toolbox bgtitle">
                                <div id="gridtitledetail" class="gridtitle">
                                </div>
                                <div class="btngroup">
                                    <input type="button" onclick="numberGameWinLossByMatchDetail.backtomain();" value="  "
                                        name="btnBack" id="btnBack" class="btnBack" title="Back To Main" />
                                    <input type="button" onclick="numberGameWinLossByMatchDetail.exportExcel();" value="  "
                                        name="btnExportDetail" id="btnExportDetail" class="btnExcel" title="Export to Excel" /></div>
                            </div>
                            <div class="content-header clearfix">
                                <div class="header-frozen">
                                    <table class="tbl-header-frozen">
                                        <tr>
                                            <td>
                                                <div class="col40 c">
                                                    No.</div>
                                            </td>
                                            <td>
                                                <div class="col160 c">
                                                    Username</div>
                                            </td>
                                            <td>
                                                <div class="col100 c">
                                                    Turnover</div>
                                            </td>
                                            <td>
                                                <div class="col100 c">
                                                    Comm.</div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="header-scroll">
                                    <table class="tbl-header-scroll">
                                        <tr>
                                            <td colspan="3">
                                                <div class="col300 c">
                                                    Member</div>
                                            </td>
                                            <td colspan="3">
                                                <div class="col300 c">
                                                    Agent</div>
                                            </td>
                                            <td colspan="3">
                                                <div class="col300 c">
                                                    Master</div>
                                            </td>
                                            <td colspan="3">
                                                <div class="col300 c">
                                                    Super</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 c">
                                                    Winloss</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 c">
                                                    Comm.</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 c">
                                                    Total</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 c">
                                                    Winloss</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 c">
                                                    Comm.</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 c">
                                                    Total</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 c">
                                                    Winloss</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 c">
                                                    Comm.</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 c">
                                                    Total</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100  c">
                                                    Winloss</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 c">
                                                    Comm.</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 c">
                                                    Total</div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="content-body clearfix">
                                <div class="body-frozen">
                                    <table id="tblfrozendetail" class="tbl-body-frozen">
                                    </table>
                                </div>
                                <div class="body-scroll">
                                    <table id="tblscrolldetail" class="tbl-body-scroll">
                                    </table>
                                </div>
                            </div>
                            <div class="content-footer">
                                <div class="footer-frozen">
                                    <table class="tbl-footer-frozen">
                                        <tr style="display: none">
                                            <td>
                                                <div class="col40">
                                                    &nbsp;</div>
                                            </td>
                                            <td>
                                                <div class="col160">
                                                    Total</div>
                                            </td>
                                            <td>
                                                <div class="col100 r">
                                                    &nbsp;</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col100 r">
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td>
                                                <div class="col40">
                                                    &nbsp;</div>
                                            </td>
                                            <td>
                                                <div class="col160">
                                                    Total</div>
                                            </td>
                                            <td>
                                                <div class="col100 r">
                                                    &nbsp;</div>
                                            </td>
                                            <td>
                                                <div class="col100 r">
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="col40">
                                                    &nbsp;</div>
                                            </td>
                                            <td>
                                                <div class="col160">
                                                    Total</div>
                                            </td>
                                            <td>
                                                <div id="detailtotalTurnover" class="col100 r">
                                                    &nbsp;</div>
                                            </td>
                                            <td>
                                                <div id="detailtotalComm" class="col100 r">
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="footer-scroll">
                                    <table class="tbl-footer-scroll">
                                        <tr>
                                            <td>
                                                <div id="mdetailtotalwinloss" class="col100 r">
                                                </div>
                                            </td>
                                            <td>
                                                <div id="mdetailtotalcomm" class="col100 r">
                                                </div>
                                            </td>
                                            <td>
                                                <div id="mdetailtotal" class="col100 r">
                                                </div>
                                            </td>
                                            <td>
                                                <div id="adetailtotalwinloss" class="col100 r">
                                                </div>
                                            </td>
                                            <td>
                                                <div id="adetailtotalcomm" class="col100 r">
                                                </div>
                                            </td>
                                            <td>
                                                <div id="adetailtotal" class="col100 r">
                                                </div>
                                            </td>
                                            <td>
                                                <div id="madetailtotalwinloss" class="col100 r">
                                                </div>
                                            </td>
                                            <td>
                                                <div id="madetailtotalcomm" class="col100 r">
                                                </div>
                                            </td>
                                            <td>
                                                <div id="madetailtotal" class="col100 r">
                                                </div>
                                            </td>
                                            <td>
                                                <div id="sdetailtotalwinloss" class="col100 r">
                                                </div>
                                            </td>
                                            <td>
                                                <div id="sdetailtotalcomm" class="col100 r">
                                                </div>
                                            </td>
                                            <td>
                                                <div id="sdetailtotal" class="col100 r">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
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
</script>
</html>
