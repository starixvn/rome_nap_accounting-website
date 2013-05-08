<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EarlyKickOff.aspx.cs" Inherits="SpecialDelete_Index"
    Async="true" Title="Search Transaction" Theme="" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script type="text/javascript">
        window.location.href = "../V2/EarlyKickOff.aspx";
    </script>
    <title>Accounting Special Delete - Kick Off Time</title>
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/SpecialDelete/SpecialDelete.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jQuery/jquery.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../js/stdlib.js" type="text/javascript"></script>
    
    <script src="../../js/jQuery/jquery-ui-1.8.2.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/jQuery/Template/jquery.tmpl.js"></script>
    <script src="../../js/common.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/EarlyKickOffGridRender.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/EarlyKickOffDeleteBets.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    
    <style type="text/css">
        input[type=checkbox]
        {
            cursor: pointer;
        }
        div.match
        {
            background-color: #FFFFAA!important;
        }
        div.cur
        {
            background-color: #FFFFAA;
        }
        div.box_header{
            width: 1050px;
            background:url(../../App_Themes/Images/L_banner/box_02header.jpg) repeat-x top left;
            margin-top: 3px;
            border: solid 1px #7C98A4;
            padding: 3px 5px;
            cursor: pointer;
            float: left;
        }
        div.box_header span {
            background:url(../../App_Themes/Images/icon_down.gif) no-repeat top left;
            padding-left: 20px;
            font-weight: bold;
        }
        div.box_header span.up {
            background:url(../../App_Themes/Images/icon_up.gif) no-repeat top left!important;
        }
        div.title_label span {
            background:url(../../App_Themes/Images/icon_down.gif) no-repeat top left;
            margin-left: 5px;
            font-weight: bold;
            width: 12px;
            float: left;
        }
        div.title_label span.up {
            background:url(../../App_Themes/Images/icon_up.gif) no-repeat top left!important;
        }
        div.box_button {
            margin-top: 10px;
            width: 1061px;
            padding: 3px 0px;
        }
        div#box_list_cont {
            margin-top: 3px;
            margin-bottom: 10px;
            float: left;
            background-color: #FFF;
            width: 1061px;
        }
        div#box_list_message {
            margin-top: 3px;
            width: 1061px;
            float: left;
            overflow: hidden;
        }
        div#box_list_scroll {
            float: left;
            width: 1061px;
            overflow-x: hidden;
            overflow-y: auto;
            border-right: solid 1px #7C98A4;
            border-left: solid 1px #7C98A4;
            border-bottom: solid 1px #7C98A4;
            max-height: 600px;
        }
         div#box_match_info {
            margin-top: 3px;
            width: 973px;
            float: left;
         }
        /*bet grid*/
        .border-grid-header { border: solid 1px #7C98A4; }
        .border-grid-row { border-top: solid 1px #7C98A4; }
        .border-right { border-right: solid 1px #7C98A4; }
        .no-border { border: none!important; }
        .betrow { float: left;}
        .betrow.head { font-weight: bold; background-color: #CDDEF3; color: #000; width: 1061px; }
        .betrow.head div {text-align: center; padding: 3px 0px;}
        .betrow.line { border-right: solid 1px #7C98A4;}
        .betrow.line div { text-align: center; padding: 3px 0px; height: 25px}
        .betrow.title { padding: 3px 0px; border-right: solid 1px #7C98A4; text-align: center; background-color: #6495ED; width: 1044px; color: #FFF!important; margin-bottom: 5px; display: none;}
        .betrow.index { width: 40px; }
        .betrow.account { width: 90px; word-wrap: break-word;}
        .betrow.refno { width: 100px; word-wrap: break-word;}
        .betrow.bettype { width: 80px; word-wrap: break-word;}
        .betrow.bet { width: 50px; }
        .betrow.odds { width: 50px; text-align: right!important;}
        .betrow.amount { width: 70px; }
        .betrow.currency { width: 70px; }
        .betrow.hdp { width: 70px; }
        .betrow.transtime { width: 120px;word-wrap: break-word; }
        .betrow.score { width: 70px; }
        .betrow.status { width: 70px; }
        .betrow.option { width: 60px; }
        .betrow.scroll { width: 8px; }
        div.bettype{float: left;}
        .box_clear {float: left; width:1061px; height: 10px;}
    </style>
</head>
<body>
    <script type="text/html" id="row_bet_tpl">
        {{if Bettyes.length}}
        <div class="betrow head border-grid-header">
            <div class="betrow index border-right">NO</div>
            <div class="betrow account border-right">ACCOUNT</div>
            <div class="betrow refno border-right">REFNO</div>
            <div class="betrow bettype border-right">BET TYPE</div>
            <div class="betrow bet border-right">BET</div>
            <div class="betrow odds border-right">ODDS</div>
            <div class="betrow amount border-right">AMOUNT</div>
            <div class="betrow currency border-right">CURRENCY</div>
            <div class="betrow hdp border-right">HDP</div>
            <div class="betrow transtime border-right">TRANS.TIME</div>
            <div class="betrow score border-right">SCORE</div>
            <div class="betrow account border-right">CREATOR</div>
            <div class="betrow status border-right">STATUS</div>
            <div class="betrow option border-right">OPTION</div>
        </div>
        <div id="box_list_scroll" style="display:;">
            {{each(index, bettype) Bettyes}}
            {{if index % 2 ==0}}
            <div class="betrow title border-grid-row" id="betrow_title_${index}">
                {{html specialdelete.genBetTitle(index, bettype)}}
                <div style="float: right;margin-right: 5px;"> 
                    {{if bettype.IsAllowOptionDelete==true}}
                    <select onchange="ddl_change('${bettype.Type}');" name="ddl${bettype.Type}" id="ddl${bettype.Type}">
                        <option value="0" {{if bettype.OptionType == 0}}selected="selected"{{/if}}>Not Delete</option>
                        <option value="1" {{if bettype.OptionType == 1}}selected="selected"{{/if}}>Delete All</option>
                        <option value="2" {{if bettype.OptionType == 2}}selected="selected"{{/if}}>Optional</option>
                    </select>
                    {{/if}}
                </div>
            </div>
            {{/if}}
            <div id="bettype_${index}" class="bettype">
            {{each(i2, item) bettype.KoItems}}
            <div class="betrow line border-grid-row" rel="${item.Refno}-${item.TransId}-${item.CurrencyId}">
                <div class="betrow index border-right">${item.Order}</div>
                <div class="betrow account border-right">${item.Account}</div>
                <div class="betrow refno border-right">
                    {{if item.IsMP=="1"}}<a href="javascript:;" class="mixparlay" ref="${item.Refno}-${item.TransId}-${item.CurrencyId}">${item.TransId}</a><br/><font size="1"><i>(Mix Parlay)</i></font>{{else}}${item.TransId}{{/if}}
                </div>
                <div class="betrow bettype border-right">${item.Bettype}</div>
                <div class="betrow bet border-right">${item.Betteam}</div>
                <div class="betrow odds border-right"><strong><span style="color:blue;">${item.Odds}</span><br>${item.OddType}</strong></div>
                <div class="betrow amount border-right">${item.Amount}</div>
                <div class="betrow currency border-right">${item.Currency}</div>
                <div class="betrow hdp border-right">${item.Hdp}</div>
                <div class="betrow transtime border-right">${item.TransTime}</div>
                <div class="betrow score border-right">${item.Score}</div>
                <div class="betrow account border-right">${item.Creator}</div>
                <div class="betrow status border-right">${item.Status}</div>
                <div class="betrow option no-border">
                    {{html specialdelete.genCheckOpt(item, bettype)}}
                </div>
            </div>
            {{/each}}
            </div>
            {{/each}}
        </div>
        {{/if}}
    </script>
    <script type="text/html" id="row_bet_mixparlay">
        <table class="tbllayout" cellspacing="1">
            <tbody>
                <tr class="title"><td style="width:80px" class="c">UserName</td><td style="width:150px" class="c">Date</td><td style="width:400px" class="c">Choice</td><td style="width:50px" class="c">Stake</td><td style="width:80px" class="c">Currency</td></tr>
                {{if Rows}}
                {{each(r, row) Rows}}
                <tr class="evenrow">
                    <td class="c">${row.Account}</td>
                    <td class="c">Ref No: ${row.Refno}<br>${row.TransTime}</td>
                    <td class="r">
                        {{each(ind, choice) row.Choices}}
                            {{html choice.Tpl}}
                        {{/each}}
                    </td>
                    <td class="c">${row.Stake}</td>
                    <td class="c">${row.Currency}</td>
                </tr>
                {{/each}}
                {{else}}
                <p align="center">There is no information available</p>
                {{/if}}
            </tbody>
        </table>
    </script>
    <script type="text/html" id="row_match">
        <table style="width:600px;" class="tblcollapse">
            <tbody>
                <tr class="title c">
                    <td nowrap="">CODE</td><td>MATCH EVENT</td><td width="150">EVENT DATE</td>
                </tr>
                <tr class="h3 c">
                    <td id="code">${MatchCode}</td><td nowrap="" id="teams">${HomeName} - vs - ${AwayName}</td><td id="transtime">${EventDate}</td>
                </tr>
            </tbody>
        </table>
    </script>
    <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
            <tr>
                <td class="maincontent_headerL">
                </td>
                <td class="maincontent_headerC">
                    Early Kick-Off Delete Bets
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box" style="width:1066px; padding:4px 0px!important">
            <div id="ErrMsg" class="errMsg">
            </div>
        </div>
        <div class="bg_table" style="width:982px;">
            <div class="box_header setting" rel="box_setting"><span>Filter Option</span></div>
            <div id="box_setting" style="padding-top: 3px; float:left;">
                <%=html %>
                <div class="box_button list">
                    <input class="btn" type="button" onclick="return on_submit();"  value="  Show  " style="width: 100px" />
                </div>
            </div>
            <div class="box_clear"></div>
            <div class="box_header list" style="display:none;" rel="box_list_cont:box_match_info"><span>Bet List</span></div>
            <div id="box_match_info" style="display:none;"></div>
            <div id="box_list_cont" style="display:none;"></div>
            <div class="box_clear"></div>
            <div class="box_header message" style="display:none;" rel="box_list_message"><span>Bet Message</span></div>
            <div id="box_list_message" style="display:none;" >
                <form id="frmDeleteTrans"><table border="0px" class="table"><tbody><tr><th bgcolor="#88aaff" colspan="3">Please include "@@" in message content for replacing ticket numbers</th></tr><tr bgcolor="#ADADAD" class="tableBw"><th align="center"><span style="color:Red;">*</span>Message En</th><th align="center"><span style="color:Red;">*</span>Message Ch</th><th align="center">Message Cs</th></tr><tr><td><textarea cols="36" name="men" id="men" rows="10"></textarea></td><td><textarea cols="36" name="mch" id="mch" rows="10"></textarea></td><td><textarea cols="36" name="mcs" id="mcs" rows="10"></textarea></td></tr><tr><th bgcolor="#88aaff" colspan="3"><a href="#" onclick="hideLanguage(this)" id="other_display"><span style="color: Black;">Other Languages</span> [+]</a></th></tr><tr bgcolor="#ADADAD" style="display:none;" id="otherLanguage_title" class="tableBw"><th align="center">Message Th</th><th align="center">Message It</th><th align="center">Message Jp</th></tr><tr style="display:none;" id="otherLanguage_content"><td><textarea cols="36" name="mth" id="mth" rows="10"></textarea></td><td><textarea cols="36" name="mit" id="mit" rows="10"></textarea></td><td><textarea cols="36" name="mjp" id="mjp" rows="10"></textarea></td></tr><tr bgcolor="#ADADAD" style="display:none;" id="otherLanguage_title2" class="tableBw"><th align="center">Message Es</th><th align="center"></th><th align="center"></th></tr><tr style="display:none;" id="otherLanguage_content2"><td><textarea cols="36" name="mes" id="mes" rows="10"></textarea></td><td></td><td></td></tr></tbody></table><table border="0px" class="table"><tbody><tr style="display:none;" id="trMsg2"><td colspan="2"><input type="hidden" name="DelOption" id="DelOption"><input type="hidden" name="transids" id="transids"><input type="hidden" name="DelOption_BD" id="DelOption_BD"><input type="hidden" name="transids_BD" id="transids_BD"><input type="hidden" name="DelOption_12EU" id="DelOption_12EU"><input type="hidden" name="transids_12EU" id="transids_12EU"><input type="hidden" name="DelOption_MHC" id="DelOption_MHC"><input type="hidden" name="transids_MHC" id="transids_MHC"><input type="hidden" value="0" name="frefno"><input type="hidden" value="999999999999" name="trefno"><input type="hidden" value="1155322" id="matchid" name="matchid"><input type="hidden" value="1^2^3^4^5^6^7^8^12^13^14^15^16^20^21^22^24^25^26^27^28^" id="bettype" name="bettype"><input type="hidden" value="All" id="betteam1378" name="betteam1378"><input type="hidden" value="All" id="betteam515" name="betteam515"><input type="hidden" value="" id="betteam4" name="betteam4"><input type="hidden" value="2" id="liveindicator" name="liveindicator"><input type="hidden" value="9999" id="hdp" name="hdp"><input type="hidden" id="IsAllCSType" name="IsAllCSType"></td></tr></tbody></table></form>
            </div>
            <div class="box_clear"></div>
            <div class="box_button delete" style="display:none;">
                <table border="0px" class="table"><tbody><tr><td>USERNAME:</td><td><input type="text" id="username" name="username"></td></tr><tr><td>PASSWORD:</td><td><input type="password" id="userpwd" name="userpwd"></td></tr><tr><td colspan="2"><div class="error-message" id="ErrorMessage"></div></td></tr></tbody></table>
                <br/>
                <input type="hidden" id="userId" name="userId" value="<%=AppSession.AccId %>" />
                <input type="button" class="btn" onclick="return on_delete();" value="&nbsp;&nbsp;Delete Bet&nbsp;&nbsp;" style="width: 100px" />
            </div>
            <div class="box_clear"></div>
            <div class="box_clear"></div>
            <script type="text/javascript">
                $(document).ready(function() {
                    on_changeBetype(1);
                    $('div.box_header').bind('click', function(e) {
                        var ids = $(this).attr('rel').toString().split(':');
                        $(this).find('span').toggleClass('up');
                        for (i = 0; i < ids.length; i++) {
                            $('div#' + ids[i]).slideToggle();
                        }
                    });
                    // setup starting time
                    var currdate = new Date();
                    SetupCalendar(currdate.format('m/d/Y H:i:s'));
                });
                
            </script>
        </div>
    </div>
</body>
</html>
