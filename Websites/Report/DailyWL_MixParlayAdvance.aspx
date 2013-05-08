<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyWL_MixParlayAdvance.aspx.cs"
    Inherits="Report_DailyWL_MixParlayAdvance" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="Styles/MixparlayAdvance.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/BlockUI.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/Datetime.js" type="text/javascript"></script>
    <script src="../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="Scripts/DailyWLMixparlay_Advance.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <div id="headerReport">
        <table>
            <tr>
                <td style="font-weight: bold">
                    Date:
                </td>
                <td>
                    <%=strwinlossdate %>
                </td>
                <td>
                    <input type="button" id="btnReport" name="btnREport" value="REPORT" class="btn" onclick="DailyWLMP.GetMainDetailMP()" />
                </td>
                <td>
                    <input type="button" id="btnToday" name="btnToday" class="btn" onclick="DailyWLMP.viewReportYT(1)" />
                </td>
                <td>
                    <input type="button" id="btnYesterday" name="btnYesterday" class="btn" onclick="DailyWLMP.viewReportYT(2)" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <b><a href="DailyWL_Sporttype.aspx?fdate=<%=fdate %> &db=Rep&mode=">Main </a>&gt; Mix
                        Parlay </b>
                </td>
            </tr>
        </table>
    </div>
    <div id="ErrMsg">
    </div>
    <div id="MixparlayDetails" class="content-warper hidden" style="margin-top: 6px;">
        <div class="content-toolbox bgtitle">
            <div id="gridtitle" class="gridtitle">
            </div>
            <div class="btngroup">
                <input type="button" onclick="DailyWLMP.exportExcel()" value="  " id="btnExport"
                    class="btnExcel" title="Export to Excel"></div>
        </div>
        <div class="content-header clearfix">
            <div class="header-scroll">
                <table class="tbl-header-scroll">
                    <tr>
                        <td style="border-bottom: none 0px;">
                            <div class="col120 c">
                            </div>
                        </td>
                        <td style="border-bottom: none 0px;">
                            <div class="col120 c">
                                Sporttype</div>
                        </td>
                        <td style="border-bottom: none 0px;">
                            <div class="col120 c">
                                Bettype</div>
                        </td>
                        <td style="border-bottom: none 0px;">
                            <div class="col120 c">
                                WinLoss</div>
                        </td>
                        <td style="border-bottom: none 0px;">
                            <div class="col120 c">
                                Position</div>
                        </td>
                        <td style="border-bottom: none 0px;">
                            <div class="col120 c">
                                Comm</div>
                        </td>
                        <td style="border-bottom: none 0px;">
                            <div class="col120 c">
                                Line Total</div>
                        </td>
                        <td style="border-bottom: none 0px;">
                            <div class="col120 c">
                                Turnover</div>
                        </td>
                        <td style="border-bottom: none 0px;">
                            <div class="col120 c">
                                Margin</div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="content-body clearfix">
            <div class="body-scroll">
                <table id="tblMixparlay" class="tbl-body-scroll">
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
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
    <script type="text/javascript">
        var today = '<%=Today %>'
        var yesterday = '<%=Yesterday %>';
        var datewinloss = '<%=datewinloss %>'
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);
        var ispopup = '<%=isPopup %>'
        if (ispopup == "true")
            $('#headerReport').hide();
        $(document).ready(function () {
            DailyWLMP.GetMainDetailMP();
        });
    </script>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
</body>
</html>
