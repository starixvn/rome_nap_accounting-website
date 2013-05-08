<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeleteBets.aspx.cs" Inherits="SpecialDelete_LiveCasino_DeleteBets" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delete bets</title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/SpecialDelete/SpecialDelete.css" rel="stylesheet" type="text/css" />    
    <link href="../V2/css/style.css" rel="stylesheet" type="text/css" />
    <link href="Style/Style.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery-latest.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.queue.js" type="text/javascript"></script>    
    <script src="../../js/jQuery/Number/jquery.formattyping.js" type="text/javascript"></script>
    <script src="../V2/js/setting.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="Scripts/DeleteBets.js?vers=<%=AccCommon.Version() %>" type="text/javascript"></script>
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
                        Live Casino
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width: 1209px;">
                <span style="color: rgb(216, 78, 31); font-weight: bold; font-size: 9pt;"><div class="alertboxwrn"><div>You can only delete the refunded tickets</div></div></span>
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <label class="b">
                                    Match ID:</label>
                            </td>
                            <td style="padding-left: 4px">
                                <input type="text" id="tbxMatchID" />
                            </td>
                            <td style="padding-left: 15px;">
                                <label class="b">
                                    From Refno:</label>
                            </td>
                            <td style="padding-left: 4px">
                                <input type="text" id="tbxFromRefNo" />
                            </td>
                            <td style="padding-left: 15px;">
                                <label class="b">
                                    To Refno:</label>
                            </td>
                            <td style="padding-left: 4px">
                                <input type="text" id="tbxToRefNo" />
                            </td>
                            <td style="padding-left: 6px">
                                <input id="btnShow" type="button" class="btn" value="Show" disabled="disabled" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="error-message" id="diveNoInfo" style="width:200px; display:none">
                    <div class="alertboxerr"><div id="divContentMsg">There is no information!</div></div>
                </div>
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
                                        <div class="col20 c">
                                            #
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col100 c">
                                            User
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col140 c">
                                            Trans.Time
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col100 c">
                                            BetID
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col100 c">
                                            BetType
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col60 c">
                                            Stake
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col80 c">
                                            Win Loss
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col60 c">
                                            Status
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col80 c">
                                            Super PT
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col80 c">
                                            Master PT
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col80 c">
                                            Agent PT
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col160 c">
                                            Choice
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title">
                                        <div class="col50 c">
                                            Option
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
                        </div>
                    </div>
                </div>
                <div id="divSendMsg" style="display: none; margin-top: 4px;">
                    <div class="box_header message" style="padding: 0px; width: 1208px">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        <input type="checkbox" id="cbxSendMsg" checked="checked" />
                                    </td>
                                    <td>
                                        <div class="b">
                                            Send Message</div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="box_list_message">
                        <form id="frmDeleteTrans">
                        <table border="0px" class="table">
                            <tbody>
                                <tr>
                                    <th bgcolor="#88aaff" colspan="3">
                                        Please include "@@" in message content for replacing ticket numbers
                                    </th>
                                </tr>
                                <tr bgcolor="#ADADAD" class="tableBw">
                                    <th align="center">
                                        <span style="color: Red;">*</span>Message En
                                    </th>
                                    <th align="center">
                                        <span style="color: Red;">*</span>Message Ch
                                    </th>
                                    <th align="center">
                                        Message Cs
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <textarea cols="36" name="men" id="men" rows="10"></textarea>
                                    </td>
                                    <td>
                                        <textarea cols="36" name="mch" id="mch" rows="10"></textarea>
                                    </td>
                                    <td>
                                        <textarea cols="36" name="mcs" id="mcs" rows="10"></textarea>
                                    </td>
                                </tr>
                                <tr>
                                    <th bgcolor="#88aaff" colspan="3">
                                        <a href="#" onclick="javascript:hideLanguage(this);" id="other_display"><span style="color: Black;">
                                            Other Languages</span> [+]</a>
                                    </th>
                                </tr>
                                <tr bgcolor="#ADADAD" style="display: none;" id="otherLanguage_title" class="tableBw">
                                    <th align="center">
                                        Message Th
                                    </th>
                                    <th align="center">
                                        Message It
                                    </th>
                                    <th align="center">
                                        Message Jp
                                    </th>
                                </tr>
                                <tr style="display: none;" id="otherLanguage_content">
                                    <td>
                                        <textarea cols="36" name="mth" id="mth" rows="10"></textarea>
                                    </td>
                                    <td>
                                        <textarea cols="36" name="mit" id="mit" rows="10"></textarea>
                                    </td>
                                    <td>
                                        <textarea cols="36" name="mjp" id="mjp" rows="10"></textarea>
                                    </td>
                                </tr>
                                <tr bgcolor="#ADADAD" style="display: none;" id="otherLanguage_title2" class="tableBw">
                                    <th align="center">
                                        Message Es
                                    </th>
                                    <th align="center">
                                        Message Ko
                                    </th>
                                    <th align="center">
                                    </th>
                                </tr>
                                <tr style="display: none;" id="otherLanguage_content2">
                                    <td>
                                        <textarea cols="36" name="mes" id="mes" rows="10"></textarea>
                                    </td>
                                    <td>
                                        <textarea cols="36" name="mko" id="mko" rows="10"></textarea>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table border="0px" class="table">
                            <tbody>
                                <tr style="display: none;" id="trMsg2">
                                    <td colspan="2">
                                        
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        </form>
                    </div>
                </div>
                <div id="divLogin" class="box_button delete">
                    <table border="0px" class="table">
                        <tbody>
                            <tr>
                                <td>
                                    USERNAME:
                                </td>
                                <td>
                                    <input value="<%=AppSession.AccUserName %>" type="text" id="username" disabled="disabled" style="width:120px">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    PASSWORD:
                                </td>
                                <td>
                                    <input type="password" id="userpwd" style="width:120px">
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="error-message" id="ErrorMessage"></div>
                    <br />
                    <input id="btnDeleteBets" type="button" class="btn delbet" value="Delete Bet" style="width: 100px" />
                </div>
                <div class="error-message" id="ErrorMessage2" style="display:none"></div>
            </div>
        </div>
    </div>
    </div>
</body>
</html>
