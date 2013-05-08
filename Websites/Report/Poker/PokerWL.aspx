<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PokerWL.aspx.cs" Inherits="Report_Poker_PokerWL" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server">Poker Win/Loss Detail</title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="style/style.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js?ver=2" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js?ver=2" type="text/javascript"></script>
    <script src="../../js/common.js?ver=2" type="text/javascript"></script>
    <script src="../../js/BlockUI.js?ver=2" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js?ver=2" type="text/javascript"></script>
    <script src="../../js/jQuery/Pager/jpager.js?ver=2" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="js/PokerReport.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span>Poker Win/Loss Detail</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tr>
                                        <td class="r">
                                            From:&nbsp;
                                            <input type="text" id="fdate" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />&nbsp; To:&nbsp;
                                            <input type="text" id="tdate" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />
                                        </td>
                                        <td>
                                            <input id="btnReport" name="btnReport" type="button" value="REPORT" class="btn" onclick="pokerReport.renderMainReport('');" />
                                        </td>
                                        <td style="padding-left: 5px">
                                            <input id="btnReportYesterday" name="btnReport" type="button" value="Yesterday" class="btn"
                                                onclick="pokerReport.renderMainReport('<%=yesterday%>');" />&nbsp;
                                            <input id="btnReportToday" name="btnReport" type="button" value="Today" class="btn"
                                                onclick="pokerReport.renderMainReport('<%=today%>');" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="ErrMsg">
                                </div>
                            </div>
                            <div style="padding-bottom: 3px;">
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="winlossmain" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitle" class="gridtitle">
                                    </div>
                                    <div class="btngroup">
                                        <%-- <input type="button" onclick="pokerReport.goBack();" value="  " name="btnBack"
                                    id="btnBackToMain" class="btnBack" title="Back To Main" />--%>
                                        <input type="button" onclick="pokerReport.exportExcel();" value="  " name="btnExport"
                                            id="btnExport" class="btnExcel" title="Export to Excel" /></div>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Date</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="col100 c">
                                                        Currency</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Gross Comm</div>
                                                </td>
                                                <td colspan="3">
                                                    <div class="c">
                                                        Member</div>
                                                </td>
                                                <td colspan="4">
                                                    <div class="c">
                                                        Comm</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Gross Comm Base (RM)</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Company Comm Base (RM)</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Win/Loss</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Comm</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Total</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Agent</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Master</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Super</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Company</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tblwinlossmain" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer clearfix">
                                    <div class="footer-scroll">
                                        <table class="tbl-footer-scroll" id="tblFooter">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div id="detailreport">
                                <div id="winlossdetail" class="content-warper hidden">
                                    <div class="content-toolbox bgtitle">
                                        <div id="gridtitledetail" class="gridtitle">
                                        </div>
                                        <div class="btngroup">
                                            <input type="button" onclick="pokerReport.goBack();" value="  " name="btnBack" id="btnBackToMain"
                                                class="btnBack" title="Back To Main" />
                                            <input type="button" onclick="pokerReport.exportExcelDetail();" value="  " name="btnExport"
                                                id="btnExportDetail" class="btnExcel" title="Export to Excel" /></div>
                                    </div>
                                    <div class="content-header clearfix">
                                        <div class="header-scroll">
                                            <table class="tbl-header-scroll">
                                                <tr>
                                                    <td rowspan="2" style="border-bottom: none 0px;">
                                                        <div class="col100 c">
                                                            UserName</div>
                                                    </td>
                                                    <td rowspan="2" style="border-bottom: none 0px;">
                                                        <div class="col80 c">
                                                            Gross Comm</div>
                                                    </td>
                                                    <td colspan="3">
                                                        <div class="c">
                                                            Member</div>
                                                    </td>
                                                    <td colspan="4">
                                                        <div class="c">
                                                            Comm</div>
                                                    </td>
                                                    <td rowspan="2" style="border-bottom: none 0px;">
                                                        <div class="col80 c">
                                                            Gross Comm Base (RM)</div>
                                                    </td>
                                                    <td rowspan="2" style="border-bottom: none 0px;">
                                                        <div class="col80 c">
                                                            Company Comm Base (RM)</div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col80 c">
                                                            Win/Loss</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col80 c">
                                                            Comm</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col80 c">
                                                            Total</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col80 c">
                                                            Agent</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col80 c">
                                                            Master</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col80 c">
                                                            Super</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col80 c">
                                                            Company</div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="content-body clearfix">
                                        <div class="body-scroll">
                                            <table id="tblwinlossdetail" class="tbl-body-scroll">
                                            </table>
                                        </div>
                                    </div>
                                    <div class="content-footer clearfix">
                                        <div class="footer-scroll">
                                            <table class="tbl-footer-scroll" id="tblFooterDetail">
                                            </table>
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
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
    <script language="javascript" type="text/javascript">
        var showOnload=<%=showOnload %>;
        var toDay = '<%= ToDay %>';
        var fDay = '<%= FDay %>';
        var tDay = '<%= TDay %>';
    </script>
</body>
</html>
