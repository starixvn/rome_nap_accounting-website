<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResultLiveCasino.aspx.cs" Inherits="LiveCasino_Settlement_ResultLiveCasino" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Settlement Live Casino</title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../js/Window/dhtmlwindow/dhtmlwindow.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jQuery/jquery-latest.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script src="script/Result.js?vers=<%=AccCommon.Version() %>" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.queue.js" type="text/javascript"></script>
    <script src="../../js/Window/dhtmlwindow/dhtmlwindow.js" type="text/javascript"></script>

    <style type="text/css">
        .title1
        {
            text-align: center;
            color: White;
            font-weight: bold;
            background-color: #587281 !important;
        }
        .title2
        {
            text-align: center;
            color: Black;
            font-weight: bold;
            background-color: #B5C4D0 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hiddenServerDate" type="hidden" value="<%= DateTime.Now.ToShortDateString() %>" />
    </form>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Live Casino Result
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <table border="0">
                    <tbody>
                        <tr>
                            <td>
                                <div class="b">
                                    Date:</div>
                            </td>
                            <td>
                                <input id="fdate" type="text" class="txtdate" readonly="readonly" />
                            </td>
                            <td>
                                <div class="b">
                                    Leagues:</div>
                            </td>
                            <td>
                                <div>
                                    <select id="selLeagues">
                                    </select>
                                </div>
                            </td>
                            <td>
                                <input id="btnSubmit" type="button" value="Submit" class="btn" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="divContainer" class="bg_table" style="margin-top: 4px; display: none">
                <div id="divMainGrid" class="content-warper hidden">
                    <div class="content-header clearfix">
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td id="tdtitle1" colspan="8" class="title1">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col140 c">
                                            CODE
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col140 c">
                                            KICK-OFF TIME
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col360 c">
                                            EVENT
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col80 c">
                                            FINAL SCORE
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col120 c">
                                            RESULT
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col120 c">
                                            STATUS
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-scroll">
                            <table class="tbl-body-scroll">
                                <tbody id="tobdyMainGrid">
                                </tbody>
                            </table>
                            <script id="stLeague" type="text/html">
                                <tr><td colspan="3" style="font-weight:bold; color:black;background-color:#E1EFFA">
                                        ${LeagueName}
                                    </td><td style="font-weight:bold; color:black;text-align:center;background-color:#E1EFFA">
                                        ${FinalScore}
                                    </td><td colspan="4" style=";background-color:#E1EFFA">
                                    </td>
                                </tr>
                            </script>
                            <script id="stMatch" type="text/x-jquery-tmpl">
                                <tr
                                    {{if Index % 2 == 0}}
                                        style="background-color:#DFEAFF"
                                    {{/if}}
                                ><td>
                                        <div class="col140 c">
                                            ${MatchCode}
                                        </div>
                                    </td><td>
                                        <div class="col140 c">
                                            ${KickOffTime}
                                        </div>
                                    </td><td>
                                        <div class="col360 l b">
                                            ${ShowTime}
                                        </div>
                                    </td><td>
                                        <div class="col80 c">
                                            {{if BetStatus == ""}}
                                                N/A
                                            {{else}}
                                                <span>${FinalHomeScore} - ${FinalAwayScore}</span>
                                            {{/if}}
                                        </div>
                                    </td><td>
                                        <div class="col120 c">
                                            ${BetStatus}
                                            {{if BetStatus == ""}}
                                                N/A
                                            {{/if}}
                                        </div>
                                    </td><td>
                                        <div class="col120 c">
                                            ${EventStatus}
                                        </div>
                                    </td>
                                </tr>
                            </script>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
    <%= Leagues %>
    </script>
</body>
</html>
