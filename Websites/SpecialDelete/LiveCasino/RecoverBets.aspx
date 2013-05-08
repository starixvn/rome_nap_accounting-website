<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecoverBets.aspx.cs" Inherits="SpecialDelete_LiveCasino_RecoverBets" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Recover Bet</title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/SpecialDelete/SpecialDelete.css" rel="stylesheet" type="text/css" />
    <link href="../../js/Window/dhtmlwindow/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <link href="../V2/css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery-latest.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.queue.js" type="text/javascript"></script>
    <script src="../../js/Window/dhtmlwindow/dhtmlwindow.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Number/jquery.formattyping.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="Scripts/RecoverBet.js?vers=<%=AccCommon.Version() %>" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="hiddenAccID" value="<%=AppSession.AccId %>" />
        <input type="hidden" id="hiddenAccUserName" value="<%=AppSession.AccUserName %>" />
    </form>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Recover Bet
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width: 1209px;">
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <label class="b">
                                    Transid:</label>
                            </td>
                            <td style="padding-left: 4px">
                                <input type="text" id="tbxTransId" />
                            </td>
                            <td style="padding-left: 6px">
                                <input id="btnSearch" type="button" class="btn" value="Search" disabled="disabled" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="error-message" id="diveNoInfo" style="width: 200px; display: none">
                    <div class="alertboxerr">
                        <div>
                            There is no information!</div>
                    </div>
                </div>
                <div id="ErrorMessage" style="display:none"></div>
            </div>
            <div id="divMainContainer" class="bg_table" style="width: 1208px; display: none">
                <div id="divMatchGrid" class="content-warper hidden" style="margin-top: 10px">
                    <div class="content-header clearfix">
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col140 c">
                                            CODE
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col360 c">
                                            MATCH EVENT
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col140 c">
                                            EVENT DATE
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-scroll">
                            <table class="tbl-body-scroll">
                                <tbody id="tobdyMatchGrid">
                                </tbody>
                            </table>
                            <script id="scriptMatchTemplate" type="text/html">
                                <tr><td>
                                        <div class="col140 c b">
                                            ${MatchCode}
                                        </div>
                                    </td><td>
                                        <div class="col360 c b">
                                            ${MatchEvent}
                                        </div>
                                    </td><td>
                                        <div class="col140 c b">
                                            ${EventDate}
                                        </div>
                                    </td>
                                </tr>
                            </script>
                        </div>
                    </div>
                </div>
                <div id="divTicketsGrid" class="content-warper hidden" style="margin-top: 20px">
                    <div class="content-header clearfix">
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col40 c">
                                            #
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col140 c">
                                            USER
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col140 c">
                                            TRANS.TIME
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col100 c">
                                            BETID
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col100 c">
                                            BETTYPE
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col100 c">
                                            STAKE
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col60 c">
                                            STATUS
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col80 c">
                                            SUPER PT
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col80 c">
                                            MASTER PT
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col80 c">
                                            AGENT PT
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col160 c">
                                            CHOICE
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col80 c">
                                            Recover
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-scroll">
                            <table class="tbl-body-scroll">
                                <tbody id="tobdyTicketsGrid">
                                </tbody>
                            </table>
                            <script id="scriptTicketsTemplate" type="text/html">
                                <tr><td>
                                        <div class="col40 c">
                                            ${Index}
                                        </div>
                                    </td><td>
                                        <div class="col140 c">
                                            ${UserName}
                                        </div>
                                    </td><td>
                                        <div class="col140 c">
                                            ${TransTime}
                                        </div>
                                    </td><td>
                                        <div class="col100 c">
                                            ${BetID}
                                        </div>
                                    </td><td>
                                        <div class="col100 c">
                                            ${BetTypeName}
                                        </div>
                                    </td><td>
                                        <div class="col100 r">
                                            ${Stake}<br/>
                                            (${CurrencyName})
                                        </div>
                                    </td><td>
                                        <div class="col60 c">
                                            ${Status}
                                        </div>
                                    </td><td>
                                        <div class="col80 r">
                                            ${SuperPT}
                                        </div>
                                    </td><td>
                                        <div class="col80 r">
                                            ${MasterPT}
                                        </div>
                                    </td><td>
                                        <div class="col80 r">
                                            ${AgentPT}
                                        </div>
                                    </td><td>
                                        <div class="col160 r">
                                            ${ChoiceStr}
                                        </div>
                                    </td><td>
                                        <div class="col80 c">
                                            <img onclick="javascript:recoverBet.recoverOne(${BetID});" style="border:0px" src="../../App_Themes/Images/undo.png" class="hand">
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
</body>
</html>
