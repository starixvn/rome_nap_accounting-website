<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RacingGroupDaily.aspx.cs"
    Inherits="Report_Racing_RacingGroupDaily" %>

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
    <script src="../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="script/racinggroupdaily.js?v=<%=WebSettings.Version %>" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/jQuery/Template/jquery.tmpl.js"></script>
    <script type="text/javascript" src="../../js/jQuery/Template/jQuery.datalink.js"></script>
    <script src="../../js/jQuery/Chart/highcharts.dev.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Chart/accchart.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <link href="../../js/jQuery/Chart/jchart.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .composeBtn
        {
            display: block;
            height: auto;
            position: relative;
            width: 55px;
        }
        .colorBTK4
        {
            border-color: #96BEDA !important;
        }
        
        .composeHeaderButton
        {
        }
        .textButtonTable
        {
            border-style: solid;
            border-width: 1px 0;
            cursor: pointer;
            display: inline-block;
            float: left;
        }
        .buttonOutter
        {
            border-style: solid;
            border-width: 0 1px;
            display: block;
            height: 19px;
            margin: 0 -1px;
        }
        .colorBTK4
        {
            border-color: #96BEDA !important;
        }
        .colorBT3
        {
            background-color: #DAE7EF;
        }
        .buttonOutterflatRight
        {
            border-right: 1px solid;
            margin-right: 0px;
            width: 60px;
            text-align: right;
        }
        .buttonContent
        {
            min-width: 45px;
            text-align: right;
            width: 100%;
            line-height: normal;
            min-height: 16px;
            padding: 0 5px;
            background-color: transparent;
            border: medium none;
            cursor: pointer;
            display: inline-block;
            height: 103%;
            margin: -1px 0 0;
            overflow: visible;
            position: relative;
        }
        .buttonOutter a, .buttonOutter button
        {
            outline: medium none;
        }
        .fontT2
        {
            font-family: arial,sans-serif;
            font-size: 11px;
            line-height: 13px;
        }
        .compHeaderField
        {
            height: 15px;
            left: -2px;
            margin: 0;
            overflow-y: auto;
            position: relative;
            width: 100%;
        }
        .colorK3
        {
            border-color: #C1CCD3 !important;
        }
        .inputField
        {
            background-color: white;
            border: 1px solid;
            padding: 2px 6px;
            resize: none;
        }
        .buttonLight
        {
        }
    </style>
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
                                <li class="current"><span>Racing Group Daily</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tr>
                                        <td>
                                            From Date:
                                        </td>
                                        <td>
                                            <input type="text" id="fdate" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />
                                        </td>
                                        <td>
                                            To Date:
                                        </td>
                                        <td>
                                            <input type="text" id="tdate" readonly="readonly" class="txtdate" style="width: 100px;
                                                background-color: white;" />
                                        </td>
                                        <td>
                                            <input type="button" onclick="racingGroup.getDataReport()" class="btn" value="REPORT"
                                                name="btnReport" id="btnReport" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="err">
                                </div>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="mainreport">
                                <div id="racinggrid" class="content-warper hidden">
                                    <div class="content-toolbox bgtitle">
                                        <div id="gridtitle" class="gridtitle">
                                        </div>
                                        <div class="btngroup">
                                            <input type="button" onclick="racingGroup.chart.viewByTotal()" value="  " id="btnChart"
                                                class="btnCharting hideIBCExcel" title="View Chart" />
                                            <span id="sendingMailLoading" class="sendingMailStatus" style="display: none">Sending...</span>
                                            <!--  <input type="button" onclick="racingGroup.getMails()" value="  " id="btnSendMail"
                                class="btnSendMail" title="Send Email" />-->
                                            <input type="button" onclick="racingGroup.exportExcel();" value="  " id="btnExport"
                                                class="btnExcel hideIBCExcel" title="Export to Excel" /></div>
                                        <div id="divSendMailContainer" style="display: none">
                                        </div>
                                        <script type="text/html" id="scriptSendMailTemplate">
                               {{each(i, current) mails}}
                                <div>
                                    <span unselectable="on" class="textButtonTable composeHeaderButton colorBTK4" transparent="false" title="Add receiver">
                                        <span class="buttonOutter buttonOutterflatRight colorBT3 colorBTK4">
                                            <div style="vertical-align:middle; height:100%; padding-top:2px; padding-right:3px">To:</div>
                                        </span>
                                    </span>
                                    <input class="compHeaderField inputField colorK3" id="tbxTo" value="${To}" type="text" style="width:500px" readonly  />
                                </div>
                                <div style="margin-top:3px">
                                    <span unselectable="on" class="textButtonTable composeHeaderButton colorBTK4" transparent="false" title="Add receiver">
                                        <span class="buttonOutter buttonOutterflatRight colorBT3 colorBTK4">
                                            <div style="vertical-align:middle; height:100%; padding-top:2px; padding-right:3px">Cc:</div>
                                        </span>
                                    </span>
                                    <input class="compHeaderField inputField colorK3" id="tbxCc" value="${Cc}" type="text" style="width:500px" readonly />
                                </div>
                                {{/each}}
                                <div style="margin-top:3px">
                                    <span unselectable="on" class="textButtonTable composeHeaderButton colorBTK4" transparent="false" title="Add receiver">
                                        <span class="buttonOutter buttonOutterflatRight colorBT3 colorBTK4">
                                            <div style="vertical-align:middle; height:100%; padding-top:2px; padding-right:3px">Subject:</div>
                                        </span>
                                    </span>
                                    <input class="compHeaderField inputField colorK3" id="tbxSubject" value="${Subject}" type="text" style="width:500px" readonly />
                                </div>
                                <div style="margin-top:3px">                                    
                                    <div id="tbxSendMailContent"
                                    style="width:560px; height:150px;vertical-align:top"
                                    class="compHeaderField inputField colorK3">${Content}</div>
                                </div>
                                        </script>
                                    </div>
                                    <div class="content-header clearfix">
                                        <div id="header-frozen" class="header-frozen">
                                            <table class="tbl-header-frozen">
                                                <tr>
                                                    <td colspan="2" class="ibc">
                                                        <div class="col100">
                                                            &nbsp;
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="border-bottom: none 0px;" class="ibc">
                                                        <div class="col100 c">
                                                            Date</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;" class="ibc">
                                                        <div class="col130 c">
                                                            PartnerType</div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="header-scroll" class="header-scroll">
                                            <table class="tbl-header-scroll">
                                                <tr>
                                                    <td colspan="4" class="turnover">
                                                        <div class="c">
                                                            Turnover (RM)</div>
                                                    </td>
                                                    <td colspan="3" class="ibc colspanIBC">
                                                        <div class="c">
                                                            IBC (RM)</div>
                                                    </td>
                                                    <td colspan="3" class="turnover hideIBC">
                                                        <div class="c">
                                                            12Bet (RM)</div>
                                                    </td>
                                                    <td rowspan="2" class="ibc hideIBC" style="border-bottom: none 0px;">
                                                        <div class="col100 c">
                                                            Group Total Income</div>
                                                    </td>
                                                    <td colspan="5" class="turnover">
                                                        <div class="c">
                                                            Partner (RM)</div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <!--Turnover-->
                                                    <td class="turnover">
                                                        <div class="col100 c">
                                                            Agency</div>
                                                    </td>
                                                    <td class="turnover">
                                                        <div class="col100  c">
                                                            IBC</div>
                                                    </td>
                                                    <td class="turnover">
                                                        <div class="col100 c">
                                                            Partner</div>
                                                    </td>
                                                    <td class="turnover">
                                                        <div class="col100 c">
                                                            Total TO</div>
                                                    </td>
                                                    <!--IBC-->
                                                    <td class="ibc hideIBC">
                                                        <div class="col100 c">
                                                            PT</div>
                                                    </td>
                                                    <td class="ibc">
                                                        <div class="col100 c">
                                                            Net Comm</div>
                                                    </td>
                                                    <td class="ibc hideIBC">
                                                        <div class="col100 c">
                                                            Total Income</div>
                                                    </td>
                                                    <!--12Bet-->
                                                    <td class="turnover hideIBC">
                                                        <div class="col100 c">
                                                            PT</div>
                                                    </td>
                                                    <td class="turnover hideIBC">
                                                        <div class="col100">
                                                            Comm</div>
                                                    </td>
                                                    <td class="turnover hideIBC">
                                                        <div class="col100 c">
                                                            Total Income</div>
                                                    </td>
                                                    <!--Group Total Income-->
                                                    <!--Partner-->
                                                    <td class="turnover">
                                                        <div class="col100 c">
                                                            Turnover</div>
                                                    </td>
                                                    <td class="turnover">
                                                        <div class="col100 c">
                                                            Dividend Won</div>
                                                    </td>
                                                    <td class="turnover">
                                                        <div class="col100 c">
                                                            Comm Expense</div>
                                                    </td>
                                                    <td class="turnover">
                                                        <div class="col100 c">
                                                            Fee</div>
                                                    </td>
                                                    <td class="turnover">
                                                        <div class="col100 c">
                                                            Amount Due</div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="content-body clearfix">
                                        <div id="body-frozen" class="body-frozen">
                                            <table id="tblfrozen" class="tbl-body-frozen">
                                            </table>
                                        </div>
                                        <div id="body-scroll" class="body-scroll">
                                            <table id="tblscroll" class="tbl-body-scroll">
                                            </table>
                                        </div>
                                    </div>
                                    <div class="content-footer">
                                        <div id="footer-frozen" class="footer-frozen">
                                            <table id="tblfooterfrozen" class="tbl-footer-frozen">
                                            </table>
                                        </div>
                                        <div id="footer-scroll" class="footer-scroll">
                                            <table class="tbl-footer-scroll" id="tblfooterscroll">
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

    <input type="hidden" id="IsHideIBCPTRacing" value="<%=Fanex.Tarus.Infrastructure.Configurations.WebSettings.IsHideIBCPTRacing %>" />

</body>
<script type="text/javascript">
    var title = '<%=Resources.Language.lblRacingGroupDaily %>'
    var mailContent = '<%=string.Format(Resources.Language.lblRacingGroupDailyContentMail, AppSession.AccUserName, DateTime.Now.ToShortDateString()) %>';
    var toDay = '<%= ToDay %>';
    var fDay = '<%= FDay %>';
    var tDay = '<%= TDay %>';
    $(document).ready(function () {
        var options = {
            showOtherMonths: true,
            selectOtherMonths: true,
            checkMinMax: true
        };
        calendarUtils.renderCalendar("#fdate", null, "#fdate", "", fDay, options);
        calendarUtils.renderCalendar("#tdate", null, "#fdate", "#tdate", tDay, options);
    });
</script>
</html>
