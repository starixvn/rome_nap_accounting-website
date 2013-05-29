<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EarlyKickOff.aspx.cs" Inherits="SpecialDelete_V2_EarlyKickOff" Async="true" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Accounting Special Delete - Kick Off Time</title>
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/SpecialDelete/SpecialDelete.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    
    <script src="../../js/jQuery/jquery.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/stdlib.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery-ui-1.8.14.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.draggable.min.js" type="text/javascript"></script>    
    <script src="../../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/common.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
</head>
<body>
    <script type="text/html" id="betform_tpl">
        <table border="0px" class="table">
            <tbody>
            <tr><td class="l" colspan="2"><span style="color: rgb(216, 78, 31); font-weight: bold; font-size: 9pt;"><div class="alertboxwrn"><div>You can only delete the unsettled tickets and refunded ones</div></div></span></td></tr>
		    <tr>
		        <td colspan="2">
		            <table width="700px" border="0">
		                <tr>
		                    <td style="width:60px" class="b">Match ID:</td>
		                    <td style="width:100px" align="left"><input type="text" size="12" maxlength="12" id="matchid" name="matchid" /></td>
		                    <td style="width:30px"></td>
		                    <td style="width:190px" class="b">Date Starting Time (GMT+8):</td>
			                <td> <input type="text" id="txtdate" class="txtcalendar" readonly style="cursor: pointer;" /></td>			            
			                <td style="width:30px"></td>
                            <td style="width:60px" class="b">Filter by:</td>
                            <td style="width:100px"><select onchange='' name="ddlSAFilter" id="ddlSAFilter"><option value="0" selected="selected">All</option><option value="1">Normal Ticket</option><option value="2">SA Ticket</option></select></td>
		                </tr>
		            </table>
		        </td>
		    </tr> 
		    <tr>
			    <td colspan="2">                
			        <div id="bettype-bidding" style="float:left;width:1005px; background-color:#FFF">
			            <div class="bettype-header" style="float:left;width:1005px; background-color:#B5C4D0;border:1px solid #7C98A4;">
			                <input id="chk-bettype-all" class="chk-bettype" type="checkbox"><span class="check bettype" rel="all"><b>All Bet types</b></span>
			            </div>
			            <div class="bettype-cont" style="float:left;width:1005px;border:1px solid #7C98A4;border-top:none;">
			                {{if Rows.length}}
			                {{each(index, row) Rows}}
			                    <div class="bettype-item {{if (index+1)%5==0}}brnone{{/if}} {{if (index+1)>(5*Math.floor(Count/5))}}bbnone{{/if}}">
			                        <input id="chk-bettype-${row.Key}" class="chk-bettype" value="${row.Key}" type="checkbox">&nbsp;<span class="check bettype" rel="${row.Key}">${row.Value}</span>
			                    </div>
			                {{/each}}
			                {{/if}}
			            </div>
			        </div>
			    </td>
		    </tr>
		    <tr>
			    <td colspan="2">
			        <div id="advance-bidding" style="">
			            <div id="advance-header" class="advance-header" style="" rel="advance-cont">
			                <span class="up">Advanced mode</span>
			            </div>
			            <div id="advance-cont" class="advance-cont" style="display:none;">
			                <div class="advance-item hdp" style="">
			                    <div style="float:left;width:400px;">HDP, 1st HDP, Over/Under, 1st Over/Under, Odd/Even, 1st Odd/Even:</div>
			                    <div style="float:left;width:100px;"><select name="ddlBetTeam1378" id="ddlBetTeam1378" disabled="disabled"><option value="All">All</option><option value="h">Home/Over</option><option value="a">Away/Under</option></select></div>
			                    <div style="float:left;width:100px;">HDP:&nbsp;<input type="text" maxlength="5" size="5" name="hdp" id="hdp" disabled=""></div>
			                </div>
			                <div class="advance-item ft" style="">
			                    <div style="float:left;width:400px;">FT.1X2, 1st 1X2:</div>
			                    <div style="float:left;width:100px;"><select name="ddlBetTeam515" id="ddlBetTeam515" disabled="disabled"><option value="All">All</option><option value="1">1</option><option value="X">X</option><option value="2">2</option></select></div>
			                </div>
			                <div class="advance-item score" style="">
			                    <div class="advance-item-score-head" style="float:left;width:800px;">
			                        <div style="float:left;width:400px;"><input style="margin-left:0px;" type="checkbox" id="chk-score-all" class="chk-score" disabled="disabled" />&nbsp;<span class="check score" rel="all">All Correct Scores</span></div>
			                        <div style="float:left;width:100px;">&nbsp;</div>
			                    </div>
			                    <div class="advance-item-score-cont" style="float:left;width:800px;">
			                        {{tmpl "#correct-score-tpl"}}
			                    </div>
			                </div>
			                <div class="advance-item htft" style="">
			                    <div style="float:left;width:150px;"><input style="margin-left:0px;" type="checkbox" id="chk-htft-all" class="chk-htft" disabled="disabled" value="all"/>&nbsp;<span class="check htft" rel="all">All HT/FT:</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-htft-hh" class="chk-htft" disabled="disabled" value="1:1"/>&nbsp;<span class="check htft" rel="hh">H-H</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-htft-hd" class="chk-htft" disabled="disabled" value="1:0"/>&nbsp;<span class="check htft" rel="hd">H-D</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-htft-ha" class="chk-htft" disabled="disabled" value="1:2"/>&nbsp;<span class="check htft" rel="ha">H-A</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-htft-dh" class="chk-htft" disabled="disabled" value="0:1"/>&nbsp;<span class="check htft" rel="dh">D-H</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-htft-dd" class="chk-htft" disabled="disabled" value="0:0"/>&nbsp;<span class="check htft" rel="dd">D-D</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-htft-da" class="chk-htft" disabled="disabled" value="0:2"/>&nbsp;<span class="check htft" rel="da">D-A</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-htft-ah" class="chk-htft" disabled="disabled" value="2:1"/>&nbsp;<span class="check htft" rel="ah">A-H</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-htft-ad" class="chk-htft" disabled="disabled" value="2:0"/>&nbsp;<span class="check htft" rel="ad">A-D</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-htft-aa" class="chk-htft" disabled="disabled" value="2:2"/>&nbsp;<span class="check htft" rel="aa">A-A</span></div>
			                </div>
			                <div class="advance-item tg" style="">
			                    <div style="float:left;width:150px;"><input style="margin-left:0px;" type="checkbox" id="chk-tg-all" class="chk-tg all" disabled="disabled" value="all"/>&nbsp;<span class="check tg" rel="all">All Total Goal:</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-tg-01" class="chk-tg elm" disabled="disabled" value="0-1"/>&nbsp;<span class="check tg" rel="01">0-1</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-tg-23" class="chk-tg elm" disabled="disabled" value="2-3"/>&nbsp;<span class="check tg" rel="23">2-3</span></div>
			                    <div style="float:left;width:50px;"><input type="checkbox" id="chk-tg-46" class="chk-tg elm" disabled="disabled" value="4-6"/>&nbsp;<span class="check tg" rel="46">4-6</span></div>
			                    <div style="float:left;width:70px;"><input type="checkbox" id="chk-tg-70" class="chk-tg elm" disabled="disabled" value="7-over"/>&nbsp;<span class="check tg" rel="70">7-Over</span></div>
			                </div>
			                <div class="advance-item flg" style="">
			                    <div style="float:left;width:150px;"><input style="margin-left:0px;" type="checkbox" id="chk-flg-all" class="chk-flg" disabled="disabled" value="all"/>&nbsp;<span class="check flg" rel="all">All First Goal/Last Goal:</span></div>
			                    <div style="float:left;width:80px;"><input type="checkbox" id="chk-flg-hf" class="chk-flg" disabled="disabled" value="1:1"/>&nbsp;<span class="check flg" rel="hf">Home First</span></div>
			                    <div style="float:left;width:80px;"><input type="checkbox" id="chk-flg-hl" class="chk-flg" disabled="disabled" value="1:2"/>&nbsp;<span class="check flg" rel="hl">Home Last</span></div>
			                    <div style="float:left;width:80px;"><input type="checkbox" id="chk-flg-af" class="chk-flg" disabled="disabled" value="2:1"/>&nbsp;<span class="check flg" rel="af">Away First</span></div>
			                    <div style="float:left;width:80px;"><input type="checkbox" id="chk-flg-al" class="chk-flg" disabled="disabled" value="2:2"/>&nbsp;<span class="check flg" rel="al">Away Last</span></div>
			                    <div style="float:left;width:80px;"><input type="checkbox" id="chk-flg-ng" class="chk-flg" disabled="disabled" value="0:0"/>&nbsp;<span class="check flg" rel="ng">No Goal</span></div>
			                </div>
			            </div>
			        </div>
			    </td>
			</tr>    		
		    <tr style="display:none;">
			    <td colspan="2" class="c">				                
                    <input type="hidden" id="bettype" name="bettype" />
                    <input type="hidden" id="bettypename" name="bettypename" />
                    <input type="hidden" id="betteam4" name="betteam4" />
			    </td>
		    </tr>
		    </tbody>
	    </table>
    </script>
    <script type="text/html" id="correct-score-tpl">
        <table class="tblcollapse" style="width:800px;  text-align:left;">
            <tbody>
                 <tr>
                    <td><input type="checkbox" id="chk-score-10" class="chk-score" value="1:0" disabled="disabled"/>&nbsp;<span class="check score" rel="10">1-0</span></td>
                    <td><input type="checkbox" id="chk-score-20" class="chk-score" value="2:0" disabled="disabled"/>&nbsp;<span class="check score" rel="20">2-0</span></td>
                    <td><input type="checkbox" id="chk-score-21" class="chk-score" value="2:1" disabled="disabled"/>&nbsp;<span class="check score" rel="21">2-1</span></td>
                    <td><input type="checkbox" id="chk-score-30" class="chk-score" value="3:0" disabled="disabled"/>&nbsp;<span class="check score" rel="30">3-0</span></td>
                    <td><input type="checkbox" id="chk-score-31" class="chk-score" value="3:1" disabled="disabled"/>&nbsp;<span class="check score" rel="31">3-1</span></td>
                    <td><input type="checkbox" id="chk-score-32" class="chk-score" value="3:2" disabled="disabled"/>&nbsp;<span class="check score" rel="32">3-2</span></td>
                    <td><input type="checkbox" id="chk-score-40" class="chk-score" value="4:0" disabled="disabled"/>&nbsp;<span class="check score" rel="40">4-0 UP</span></td>
                    <td><input type="checkbox" id="chk-score-41" class="chk-score" value="4:1" disabled="disabled"/>&nbsp;<span class="check score" rel="41">4-1</span></td>
                    <td><input type="checkbox" id="chk-score-42" class="chk-score" value="4:2" disabled="disabled"/>&nbsp;<span class="check score" rel="42">4-2</span></td>
                    <td><input type="checkbox" id="chk-score-43" class="chk-score" value="4:3" disabled="disabled"/>&nbsp;<span class="check score" rel="43">4-3</span></td>
                    <td><input type="checkbox" id="chk-score-50" class="chk-score" value="5:0" disabled="disabled"/>&nbsp;<span class="check score" rel="50">5-0 UP</span></td>
                    
                    <td rowspan="2"><input type="checkbox" id="chk-score-00" class="chk-score" value="0:0" disabled="disabled"/>&nbsp;<span class="check score" rel="00">0-0</span></td>
                    <td rowspan="2"><input type="checkbox" id="chk-score-11" class="chk-score" value="1:1" disabled="disabled"/>&nbsp;<span class="check score" rel="11">1-1</span></td>
                    <td rowspan="2"><input type="checkbox" id="chk-score-22" class="chk-score" value="2:2" disabled="disabled"/>&nbsp;<span class="check score" rel="22">2-2</span></td>
                    <td rowspan="2"><input type="checkbox" id="chk-score-33" class="chk-score" value="3:3" disabled="disabled"/>&nbsp;<span class="check score" rel="33">3-3</span></td>
                    <td rowspan="2"><input type="checkbox" id="chk-score-44" class="chk-score" value="4:4" disabled="disabled"/>&nbsp;<span class="check score" rel="44">4-4</span></td>
                    
                 </tr>            
                 <tr>
                    <td><input type="checkbox" id="chk-score-01" class="chk-score" value="0:1" disabled="disabled"/>&nbsp;<span class="check score" rel="01">0-1</span></td>
                    <td><input type="checkbox" id="chk-score-02" class="chk-score" value="0:2" disabled="disabled"/>&nbsp;<span class="check score" rel="02">0-2</span></td>
                    <td><input type="checkbox" id="chk-score-12" class="chk-score" value="1:2" disabled="disabled"/>&nbsp;<span class="check score" rel="12">1-2</span></td>
                    <td><input type="checkbox" id="chk-score-03" class="chk-score" value="0:3" disabled="disabled"/>&nbsp;<span class="check score" rel="03">0-3</span></td>
                    <td><input type="checkbox" id="chk-score-13" class="chk-score" value="1:3" disabled="disabled"/>&nbsp;<span class="check score" rel="13">1-3</span></td>
                    <td><input type="checkbox" id="chk-score-23" class="chk-score" value="2:3" disabled="disabled"/>&nbsp;<span class="check score" rel="23">2-3</span></td>
                    <td><input type="checkbox" id="chk-score-04" class="chk-score" value="0:4" disabled="disabled"/>&nbsp;<span class="check score" rel="04">0-4 UP</span></td>
                    <td><input type="checkbox" id="chk-score-14" class="chk-score" value="1:4" disabled="disabled"/>&nbsp;<span class="check score" rel="14">1-4</span></td>
                    <td><input type="checkbox" id="chk-score-24" class="chk-score" value="2:4" disabled="disabled"/>&nbsp;<span class="check score" rel="24">2-4</span></td>
                    <td><input type="checkbox" id="chk-score-34" class="chk-score" value="3:4" disabled="disabled"/>&nbsp;<span class="check score" rel="34">3-4</span></td>
                    <td><input type="checkbox" id="chk-score-05" class="chk-score" value="0:5" disabled="disabled"/>&nbsp;<span class="check score" rel="05">0-5 UP</span></td>
                 </tr>  
            </tbody>
        </table>
    </script>
    <script type="text/html" id="row_bet_tpl">
        {{if NoRow==false}}
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
            <div class="betrow status border-right">S.A</div>
            <div class="betrow option border-right">OPTION</div>
        </div>
        <div id="box_list_scroll"></div>
        {{else}}
        <p align="center">There is no information available</p>
        {{/if}}
    </script>
    <script id="item_bettype_tpl" type="text/html">
        <div class="betrow title border-grid-row" id="betrow_title_${Type}">
            <div class="title_label" style="float:left; width: 850px;cursor: pointer;" rel="bettype_${Type}"><span>&nbsp;</span>${Type}</div>
            <div style="float: right;margin-right: 5px;"> 
                {{if IsAllowOptionDelete==true}}
                <select onchange="earlyko.ddlChange('${Type}');" name="ddl${Type}" id="ddl${Type}" class="sel-partner" partner="${Type}">
                    <option value="0" {{if OptionType == 0}}selected="selected"{{/if}}>Not Delete</option>
                    <option value="1" {{if OptionType == 1}}selected="selected"{{/if}}>Delete All</option>
                    <option value="2" {{if OptionType == 2}}selected="selected"{{/if}}>Optional</option>
                </select>
                {{/if}}
            </div>
        </div>
        <div id="bettype_${Type}" class="bettype"></div>
    </script>
    <script id="item_betrans_tpl" type="text/html">
        <div {{if SaFilter == 0}}class="betrow line border-grid-row" {{else}}class="betrow line border-grid-row highlightSA"{{/if}} rel="${Refno}-${TransId}-${CurrencyId}">
            <div class="betrow index border-right">${Order}</div>
            <div class="betrow account border-right"><span id="account-${TransId}">${Account}</span></div>
            <div class="betrow refno border-right">
                {{if IsMP=="1"}}<a href="javascript:;" class="mixparlay" ref="${Refno}-${SubTransId}-${CurrencyId}-${TransId}-${ParlayType}">${TransId}</a><br/><font size="1"><i>{{if ParlayType==29}}(System Parlay){{else}}(Mix Parlay){{/if}}</i></font>{{else}}${TransId}{{/if}}
            </div>
            <div class="betrow bettype border-right">${Bettype}</div>
            <div class="betrow bet border-right">${Betteam}</div>
            <div class="betrow odds border-right"><strong><span style="color:blue;">${Odds}</span><br>${OddType}</strong></div>
            <div class="betrow amount border-right"><span id="amount-${TransId}">${Amount}</span></div>
            <div class="betrow currency border-right"><span id="currency-${TransId}">${Currency}</span></div>
            <div class="betrow hdp border-right">${Hdp}</div>
            <div class="betrow transtime border-right"><span id="transtime-${TransId}">${TransTime}</span></div>
            <div class="betrow score border-right">${Score}</div>
            <div class="betrow account border-right">${Creator}</div>
            <div class="betrow status border-right">${Status}</div>
            <div {{if SaFilter == 0}} class="betrow status border-right" {{else}}class="betrow SAFilter border-right checkSA"{{/if}}></div>
            <div class="betrow option no-border">
                {{html earlyko.genCheckOpt($item.data, $item.bettype)}}
            </div>
        </div>
    </script>
    <script type="text/html" id="row_bet_mixparlay">
        <table class="tbllayout" cellspacing="1">
            <tbody>
                <tr class="title"><td style="width:80px" class="c">UserName</td><td style="width:150px" class="c">Date</td><td style="width:400px" class="c">Choice</td><td style="width:50px" class="c">Stake</td><td style="width:80px" class="c">Currency</td></tr>
                <tr class="evenrow">
                    <td class="c">${Account}</td>
                    <td class="c">Ref No: ${TransId}<br>${TransTime}</td>
                    <td class="r">
                        ${Choice}
                    </td>
                    <td class="c">${Stake}</td>
                    <td class="c">${Currency}</td>
                </tr>
            </tbody>
        </table>
    </script>
    <script type="text/html" id="row_match">
        {{if MatchCode }}
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
        {{/if}}
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
        <div class="maincontent_box" style="float:left; width:1070px; padding:4px 0px!important">
            <div id="ErrMsg" class="errMsg"></div>
        </div>
        <div class="bg_table" style="float:left;width:1066px;">
            <div class="box_header setting" rel="box_setting"><span>Filter Option</span></div>
            <div id="box_setting" style="padding-top: 3px; float:left;">
                <div id="box_setting_form"></div>
                <div class="box_button list" style="display: none;">
                    <input class="btn showbet" type="button" value="  Show  " style="width: 100px" />
                </div>
            </div>
            <div class="box_clear"></div>
            <div class="box_header list" style="display:none;" rel="box_list_cont:box_match_info"><span>Bet List</span></div>
            <div id="box_match_info" style="display:none;"></div>
            <div id="box_list_cont" style="display:none;"></div>
            <div class="box_clear"></div>
            <div class="box_header message" style="display:none;" rel="box_list_message"><span>Bet Message</span></div>
            <div id="box_list_message" style="display:none;" >
                <form id="frmDeleteTrans"><table border="0px" class="table"><tbody><tr><th bgcolor="#88aaff" colspan="3">Please include "@@" in message content for replacing ticket numbers</th></tr><tr bgcolor="#ADADAD" class="tableBw"><th align="center"><span style="color:Red;">*</span>Message En</th><th align="center"><span style="color:Red;">*</span>Message Ch</th><th align="center">Message Cs</th></tr><tr><td><textarea cols="36" name="men" id="men" rows="10"></textarea></td><td><textarea cols="36" name="mch" id="mch" rows="10"></textarea></td><td><textarea cols="36" name="mcs" id="mcs" rows="10"></textarea></td></tr><tr><th bgcolor="#88aaff" colspan="3"><a href="#" onclick="hideLanguage(this)" id="other_display"><span style="color: Black;">Other Languages</span> [+]</a></th></tr><tr bgcolor="#ADADAD" style="display:none;" id="otherLanguage_title" class="tableBw"><th align="center">Message Th</th><th align="center">Message It</th><th align="center">Message Jp</th></tr><tr style="display:none;" id="otherLanguage_content"><td><textarea cols="36" name="mth" id="mth" rows="10"></textarea></td><td><textarea cols="36" name="mit" id="mit" rows="10"></textarea></td><td><textarea cols="36" name="mjp" id="mjp" rows="10"></textarea></td></tr><tr bgcolor="#ADADAD" style="display:none;" id="otherLanguage_title2" class="tableBw"><th align="center">Message Es</th><th align="center">Message Ko</th><th align="center"></th></tr><tr style="display:none;" id="otherLanguage_content2"><td><textarea cols="36" name="mes" id="mes" rows="10"></textarea></td><td><textarea cols="36" name="mko" id="mko" rows="10"></textarea></td><td></td></tr></tbody></table><table border="0px" class="table"><tbody><tr style="display:none;" id="trMsg2"><td colspan="2"><input type="hidden" name="DelOption" id="DelOption"><input type="hidden" name="transids" id="transids"><input type="hidden" name="DelOption_BD" id="DelOption_BD"><input type="hidden" name="transids_BD" id="transids_BD"><input type="hidden" name="DelOption_12EU" id="DelOption_12EU"><input type="hidden" name="transids_12EU" id="transids_12EU"><input type="hidden" name="DelOption_MHC" id="DelOption_MHC"><input type="hidden" name="transids_MHC" id="transids_MHC"><input type="hidden" value="0" name="frefno"><input type="hidden" value="999999999999" name="trefno"><input type="hidden" value="1155322" id="matchid" name="matchid"><input type="hidden" value="1^2^3^4^5^6^7^8^12^13^14^15^16^20^21^22^24^25^26^27^28^" id="bettype" name="bettype"><input type="hidden" value="All" id="betteam1378" name="betteam1378"><input type="hidden" value="All" id="betteam515" name="betteam515"><input type="hidden" value="" id="betteam4" name="betteam4"><input type="hidden" value="2" id="liveindicator" name="liveindicator"><input type="hidden" value="9999" id="hdp" name="hdp"><input type="hidden" id="IsAllCSType" name="IsAllCSType"></td></tr></tbody></table></form>
            </div>
            <div class="box_clear"></div>
            <div class="box_button delete" style="display:none;">
                <table border="0px" class="table"><tbody>
                    <tr><td>REASON:</td><td><input type="text" id="reason" name="reason" onkeyup="countChar(this,150,'#charNum')" />&nbsp;<span style="color:Red;">(*)</span><i><label id="charNum">Characters Left: 150</label></i></td></tr>
                    <tr><td>USERNAME:</td><td><input type="text" id="username" name="username" disabled="disabled"></td></tr><tr><td>PASSWORD:</td><td><input type="password" id="userpwd" name="userpwd"></td></tr><tr><td colspan="2"><div class="error-message" id="ErrorMessage"></div></td></tr>
                </tbody></table>
                <br/>
                <input type="hidden" id="userId" name="userId" value="<%=AppSession.AccId %>" />
                <input type="button" class="btn delbet" value="&nbsp;&nbsp;Delete Bet&nbsp;&nbsp;" style="width: 100px" />
            </div>
            <div class="box_clear"></div>
            <div class="box_clear"></div>
            <script src="js/setting.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
            <script src="js/early-ko.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
            <script type="text/javascript">
                var AccUserName = '<%=Username %>'
                $(document).ready(function() {
                    earlyko.initForm();
                });
            </script>
        </div>
    </div>
</body>
</html>
