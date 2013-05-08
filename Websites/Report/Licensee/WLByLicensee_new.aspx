<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WLByLicensee_new.aspx.cs"
    Inherits="Report_Deposit_Deposit" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/Message.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" href="../../js/jQuery/GridScroller/gridscroller.css" type="text/css" />
    <link rel="stylesheet" href="../../js/jQuery/Pager/pagination.css" type="text/css" />
    <link rel="stylesheet" href="Style/Deposit.css" type="text/css" />
    <script src="../../js/jQuery/jquery.js?ver=1" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js?ver=1" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js?ver=1" type="text/javascript"></script>
    <script src="../../js/common.js?ver=1" type="text/javascript"></script>
    <script src="../../js/BlockUI.js?ver=1" type="text/javascript"></script>
    <script src="../../js/jQuery/GridScroller/jquery.gridscroller.1.0.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Pager/jpager.js?ver=1" type="text/javascript"></script>
    <script src="js/LicenseeReportV2.js?ver=1" type="text/javascript"></script>
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
                                <li class="current"><span>Win Loss by Licensee</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tr>
                                        <td style="width: 65px">
                                            <b>Search by</b>
                                        </td>
                                        <td style="width: 50px; text-align: right">
                                            Licensee:
                                        </td>
                                        <td>
                                            <select onchange="SelCurrency();" id="dlSite" style="vertical-align: middle; width: 150px">
                                                <option value="0">Please Choose One !</option>
                                                <%=strSite%>
                                            </select>
                                        </td>
                                        <td style="width: 50px; text-align: right">
                                            Currency:
                                        </td>
                                        <td>
                                            <select id="dlCurrency" style="vertical-align: middle; width: 150px">
                                                <option value="0">All</option>
                                                <option value="-3">Base Currency-USD</option>
                                                <option value="-2">Base Currency-RM</option>
                                                <option value="-6">Base Currency-EUR</option>
                                            </select>
                                        </td>
                                        <td class="r">
                                            Date:
                                        </td>
                                        <td>
                                            <select id="dlDate" style="vertical-align: middle; width: 158px" onchange="GetDateTime(this.value)">
                                                <option value="1">Today</option>
                                                <option value="2">Yesterday</option>
                                                <option value="3">Current Week</option>
                                                <option value="4">Last Week</option>
                                                <option value="5">Current Month</option>
                                                <option value="6">Last Month</option>
                                                <option value="7">Since Last Month</option>
                                                <option value="8">Specific Date</option>
                                            </select>
                                        </td>
                                        <td class="r" style="padding-left: 23px">
                                            From:&nbsp;
                                        </td>
                                        <td>
                                            <input id="fdate" class="txtdate" readonly="readonly" />
                                        </td>
                                        <td>
                                            To:
                                        </td>
                                        <td>
                                            <input id="tdate" class="txtdate" readonly="readonly" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr style="height: 17px;">
                                        <td>
                                            <b>Report by</b>
                                        </td>
                                        <td colspan="2">
                                            <span style="padding-right: 8px">
                                                <input type="radio" name="RdReportBy" id="rdDaily" checked="checked" value="1" /><label
                                                    for="rdDaily">Daily</label></span> <span style="padding-right: 5px">
                                                        <input type="radio" name="RdReportBy" id="rdMonthly" value="2" /><label for="rdMonthly">Monthly</label></span>
                                        </td>
                                        <td colspan="2">
                                            <b>Exclude Testing</b> <span style="padding-right: 10px">
                                                <input type="radio" name="rdExcludeTesting" id="Radio1" checked="checked" value="1" /><label
                                                    for="rdExcludeTestingYes">Yes</label></span> <span style="padding-right: 5px">
                                                        <input type="radio" name="rdExcludeTesting" id="Radio2" checked="checked" value="0" /><label
                                                            for="rdExcludeTestingNo">No</label></span>
                                        </td>
                                        <td class="r">
                                            Type:
                                        </td>
                                        <td>
                                            <select id="ddltype" style="vertical-align: middle; width: 158px">
                                                <option value="1">Normal</option>
                                                <option value="3">No Draw for Turnover only</option>
                                                <option value="2">No Draw</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <span>
                                                <input type="checkbox" id="chbAll" checked="checked" onclick="change(this);" /><label
                                                    for="chbAll">All</label></span> <span>
                                                        <input type="checkbox" id="chbSport" class="chksport" checked="checked" onclick="changeSport();" /><label
                                                            for="chbSport">Sportsbook</label></span> <span>
                                                                <input type="checkbox" id="chbRacing" class="chksport" checked="checked" onclick="changeSport();" /><label
                                                                    for="chbRacing">Racing</label></span> <span>
                                                                        <input type="checkbox" id="chbNG" checked="checked" class="chksport" onclick="changeSport();" /><label
                                                                            for="chbNG">Number Game</label></span> <span>
                                                                                <input type="checkbox" id="chbShowBA" checked="checked" class="chksport" onclick="changeSport();" /><label
                                                                                    for="chbShowBA">BA</label></span> <span>
                                                                                        <input type="checkbox" id="chbLiveCasino" checked="checked" class="chksport" onclick="changeSport();" /><label
                                                                                            for="chbShowBA">Live Casino</label></span>
                                        </td>
                                        <td colspan="2" class="r">
                                            <input id="Button2" name="btnReport" type="button" value="REPORT" class="btn" onclick="ResetValue();licenseeReport.renderMainReport(0);" />
                                            <input id="Button3" name="btnReset" type="button" value="RESET" class="btn" onclick="DoReset()" />
                                        </td>
                                    </tr>
                                    <tr style="height: 17px;">
                                        <td colspan="6">
                                            <div style="font-weight: bold">
                                                * Please note that this report is using OANDA exchange rate for currency conversion.<br />
                                                * For data accuracy,  generate the billing report 5 days after month-end.
                                                </div>
                                        </td>
                                    </tr>
                                </table>
                                <div id="ErrMsg">
                                </div>
                            </div>
                        </div>
                        <div id="mainreport">
                            <div id="winlossmain" class="freegrid hidden">
                                <div class="grid-title">
                                    <div class="grid-title-lable" id="gridtitle">
                                        &nbsp;</div>
                                    <div class="grid-toolbox">
                                        <input type="button" onclick="licenseeReport.goBack();" value="  " name="btnBack"
                                            id="btnBackToMain" class="btnBack" title="Back To Main" />
                                        <input type="button" onclick="licenseeReport.exportExcel();" value="  " name="btnExport"
                                            id="btnExport" class="btnExcel" title="Export to Excel" />
                                    </div>
                                </div>
                                <div class="free-cell free-top-left">
                                    <table class="grid-fixed-table free-cell-top-left">
                                        <thead>
                                            <tr>
                                                <th colspan="4">
                                                    <div class="table-cell" style="height: 17px;">
                                                        &nbsp;</div>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th rowspan="2">
                                                    <div class="table-cell" style="width: 100px; height: 30px;" id="colReportBy">
                                                        Date</div>
                                                </th>
                                                <th rowspan="2">
                                                    <div class="table-cell" style="width: 60px; height: 30px;">
                                                        Currency</div>
                                                </th>
                                                <th rowspan="2">
                                                    <div class="table-cell" style="width: 100px; height: 30px;">
                                                        Turnover</div>
                                                </th>
                                                <th rowspan="2">
                                                    <div class="table-cell" style="width: 70px; height: 30px;">
                                                        Total Trans</div>
                                                </th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                                <div class="row_header_wrapper">
                                    <div class="row_header row-headers-background">
                                        <table id="tbwinlossrowheader" class="grid-fixed-table row_header_content">
                                            <thead>
                                                <tr>
                                                    <th colspan="4">
                                                        <div class="table-cell" style="height: 17px;">
                                                            &nbsp;</div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;" id="colReportBy">
                                                            Date</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 60px; height: 30px;">
                                                            Currency</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            Turnover</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 70px; height: 30px;">
                                                            Total Trans</div>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="col_header row-headers-background scroll-y ">
                                    <div class="header-container">
                                        <table class="grid-fixed-table col_header_content">
                                            <thead>
                                                <tr>
                                                    <th colspan="4">
                                                        <div class="table-cell" style="height: 17px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th colspan="3">
                                                        <div style="height: 17px;">
                                                            Customer Win Loss</div>
                                                    </th>
                                                     <th colspan="2">
                                                        <div style="height: 17px;">
                                                           </div>
                                                    </th>
                                                    <th colspan="3">
                                                        <div  style="height: 17px;">
                                                            Racing Revenue</div>
                                                    </th>
                                                    <th colspan="3">
                                                        <div style="height: 17px;">
                                                            Amount Due to</div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th rowspan="2">
                                                        <div class="" style="width: 100px; height: 30px;" id="colReportBy">
                                                            Date</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="" style="width: 60px; height: 30px;">
                                                            Currency</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="" style="width: 100px; height: 30px;">
                                                            Turnover</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="" style="width: 70px; height: 30px;">
                                                            Total Trans</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            Win Loss</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            Comm</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            Total</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;" id="titleLicensee">
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 80px; height: 30px;">
                                                            BA Comm</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            Win Loss</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            Comm</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            Total</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            BA</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            Racing</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;" id="titleCompany">
                                                        </div>
                                                    </th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                </div>
                                <div class="grid-container container ">
                                    <div class="content-container">
                                        <table id="tbWinloss" class="grid-fixed-table col_header_content">
                                            <thead>
                                                <tr>
                                                    <th colspan="4">
                                                        <div class="table-cell" style="height: 17px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th colspan="3">
                                                        <div class="table-cell" style="height: 17px;">
                                                            &nbsp;</div>
                                                    </th>
                                                     <th colspan="2">
                                                        <div class="table-cell" style="height: 17px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th colspan="3">
                                                        <div class="table-cell" style="height: 17px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th colspan="3">
                                                        <div class="table-cell" style="height: 17px;">
                                                            &nbsp;</div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;" id="colReportBy">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 60px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 70px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            &nbsp;
                                                        </div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 80px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                     <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            &nbsp;</div>
                                                    </th>
                                                    <th rowspan="2">
                                                        <div class="table-cell" style="width: 100px; height: 30px;">
                                                            &nbsp;
                                                        </div>
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
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
    </div>
</body>
<script type="text/javascript">
    var _fdate = '<%=strfdate %>';
    var _tdate = '<%=strtdate %>';
    getLicensees();
</script>
</html>
