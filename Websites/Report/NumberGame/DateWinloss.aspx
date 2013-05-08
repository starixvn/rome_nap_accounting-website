<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DateWinloss.aspx.cs" Theme=""
    Inherits="Report_DateWinloss" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link href="../../js/Calendar2/css/jscal2.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/numbergamewinloss.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>

    <script src="../../js/common.js" type="text/javascript"></script>

    <script src="JS/numbergamewinlostbydate.js" type="text/javascript"></script>
    <script src="JS/matchwinloss.js" type="text/javascript"></script>
    <script type="text/javascript">
        matchwinloss.init();
    </script>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Number Game Win Loss By Date
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <table>
                    <tr>
                        <td style="padding-right: 4px">
                            <div>
                                Company:</div>
                        </td>
                        <td>
                            <div>
                                <select id="selectCompany" class="txt" style="width: 100px;" runat="server">
                                </select>
                                <input type="hidden" id="hiddenCompnayFilter" runat="server" value="0" />
                            </div>
                        </td>
                        <td style="padding-left: 8px">
                            <div>
                                Currency:</div>
                        </td>
                        <td style="padding-left: 4px">
                            <select id="selectCurrency" class="txt" style="width: 100px" runat="server">
                            </select>
                            <input type="hidden" id="hiddenCurrencyFilter" runat="server" value="0" />
                        </td>
                        <td style="padding-left:8px">
                            From Date:
                        </td>
                        <td>
                            <%=strFromDate%>
                        </td>
                        <td>
                            To Date:
                        </td>
                        <td>
                            <%=strToDate%>
                        </td>
                        <td>
                            <input type="button" onclick="GetDataReport()" class="btn" value="REPORT" name="btnReport"
                                id="btnReport">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="bg_table">
                <div id="mainreport">
                    <div id="matchwinlost" class="content-warper" style="display: none;">
                        <div class="content-toolbox">
                            <div id="gridtitle" class="gridtitle">
                            </div>
                            <div class="btngroup">
                                <input type="button" onclick="ExportToExcel('1');" value="  " name="btnExport" id="btnExport"
                                    class="btnExcel" title="Export to Excel"></div>
                        </div>
                        <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table class="tbl-header-scroll">
                                    <tr class="bgborder">
                                        <td>
                                            <div class="col160">
                                                Match - Bet Type</div>
                                        </td>
                                        <td>
                                            <div class="col100 r">
                                                Winloss</div>
                                        </td>
                                        <td>
                                            <div class="col100 r">
                                                Position</div>
                                        </td>
                                        <td>
                                            <div class="col100 r">
                                                Commission</div>
                                        </td>
                                        <td>
                                            <div class="col100 r">
                                                Line Total</div>
                                        </td>
                                        <td>
                                            <div class="col100 r">
                                                Virtual Total</div>
                                        </td>
                                        <td>
                                            <div class="col100 r">
                                                Variance</div>
                                        </td>
                                        <td>
                                            <div class="col100 r">
                                                Turnover</div>
                                        </td>
                                        <td>
                                            <div class="col100 r">
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
                                            <div id="totalVirtualLineTotal" class="col100 r">
                                            </div>
                                        </td>
                                        <td>
                                            <div id="totalVariance" class="col100 r">
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
        </div>
    </div>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
</body>
</html>
