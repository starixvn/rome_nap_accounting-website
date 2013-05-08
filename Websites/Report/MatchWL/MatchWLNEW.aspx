<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeFile="MatchWLNEW.aspx.cs"
    Inherits="Report_MatchWL" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=Resources.Language.lblAccountingSystem%></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/Message.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Report/reportgrid.css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/dialogbox.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="js/MatchWLNEW.js?v=<%=WebSettings.Version %>" type="text/javascript"></script>
    <style>
        .total1 td
        {
            background-color: #FFEEDD;
        }
        .total2 td
        {
            background-color: #D3E1C8;
        }
        .total8RM td
        {
            background-color: #cbd1d3;
        }
        .grandtotal td
        {
            background-color: #CDDEF3;
        }
    </style>
</head>
<body>
    <form id="form" name="form" method="post">
    <div class="main">
        <table style="width: 100%" border="0" cellpadding="0px" cellspacing="0px">
            <tr>
                <td>
                    <div id="divMainHeader" class="">
                        <div id="left_tab1">
                            <ul>
                                <li class="lefticon"></li>
                                <li class="current"><span>
                                    <%=Resources.Language.lblMathWinloss %></span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox">
                            <div class="filterContent">
                                <div id="headerDatetime" runat="server">
                                </div>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="WLByBettype" class="content-warper hidden">
                                <div class="content-header clearfix">
                                    <div id="header-scroll-detail" class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col120">
                                                        Bet type</div>
                                                </td>
                                                <td colspan="2">
                                                    <div class="c">
                                                        Soccer Live</div>
                                                </td>
                                                <td colspan="2">
                                                    <div class="c">
                                                        Soccer Non-Live</div>
                                                </td>
                                                <td colspan="2">
                                                    <div class="c">
                                                        Other Live</div>
                                                </td>
                                                <td colspan="2">
                                                    <div class="c">
                                                        Other Non-Live</div>
                                                </td>
                                                <td colspan="2">
                                                    <div class="c">
                                                        Line Total</div>
                                                </td>
                                                <td rowspan="2" style="border-bottom: none 0px;">
                                                    <div class="c col80">
                                                        Variance</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Real</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Virtual</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Real</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Virtual</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Real</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Virtual</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Real</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Virtual</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Real</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Virtual</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table class="tbl-body-scroll">
                                            <tbody id="tbWLByBettype">
                                            </tbody>
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
                            <!--MatchWL-->
                            <div id="MatchWL" class="content-warper hidden" style="margin-top: 5px;">
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Bet Type</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Date</div>
                                                </td>
                                                <td colspan="3" style="border-bottom: none 0px;">
                                                    <div class="c" style="width: 266px;">
                                                        Match</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Winloss</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Position</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Comm</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Virtual Comm</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Line Total</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Virtual Line Total</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        Variance</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table class="tbl-body-scroll" id="tblMatchWL">
                                            <tbody id="tblMatchWLDetail">
                                            </tbody>
                                            <tfoot id="tblMatchWL3rd">
                                            </tfoot>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-footer">
                                    <div class="footer-scroll">
                                        <table class="tbl-footer-scroll" id="MatchWLGrandTotal">
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
    </form>
</body>
<script type="text/javascript">
    var dateWL = '<%=strwinlostdate %>';

    $(function () {
        $("#trGetData").hide();
    });

    var today = '<%=today %>';
    var yesterday = '<%=yesterday %>';
    var fDay = '<%= FDay %>';
    var minDay = '<%= MinDay %>';
    var maxDay = '<%= MaxDay %>';

    matchWL2a.isActiveBARM = '<%=_RmReportUtils.IsActive %>';
    matchWL2a.displayNameBARM = '<%=_RmReportUtils.DisplayName %>';
</script>
</html>
