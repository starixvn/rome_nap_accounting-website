<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Mixparlay.aspx.cs" Inherits="SpecialDelete_V2_Mixparlay" Async="true" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Special Delete - Mixparlay</title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css?ver=<%=WebSettings.Version%>" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    
    <script src="../../js/jQuery/jquery.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/stdlib.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery-ui-1.8.14.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/common.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    
    <style type="text/css">
        span.zoom {
            width: 16px;
            /*background:url(../../App_Themes/Images/zoom_out.png) no-repeat top left;*/
            cursor:pointer;
            float:left; 
        }
        span.zoom.in 
        {
           /* background:url(../../App_Themes/Images/zoom_in.png) no-repeat top left;*/
        }
        span.zoom.load {background:url(../../App_Themes/Images/ajax-loader.gif) no-repeat top left;}
        a.txt-expand-all{font-size: 13px;}
        a.txt-expand-all.load{padding-left: 18px;background:url(../../App_Themes/Images/ajax-loader.gif) no-repeat top left;}
    </style>
</head>
<body>
    <script type="text/html" id="betform_tpl">
        <table border="0px" class="table">
            <tbody>
            <tr><td class="l" colspan="2"><span style="color: rgb(216, 78, 31); font-weight: bold; font-size: 9pt;"><div class="alertboxwrn"><div>You can only delete the unsettled tickets and refunded ones</div></div></span></td></tr>
		    <tr>
		        <td colspan="2">
		            <table  border="0"><tbody>
		                <tr>
                            <td class="b">Match ID:</td><td align="left"><input type="text" onkeypress="return CheckDecNumber(event);" name="matchid" id="matchid" maxlength="12" size="12"></td>
                            <td>
                                Type: <select onchange='mixparlay.LiveindicatorChange();' id="ddlLiveIndicator" name="ddlLiveIndicator"><option selected="selected" value="2">All</option><option value="0">Dead Ball</option><option value="1">Live Ball</option></select>
                            </td>
                            <td class='type' style='padding-left:15px;display:none'>Live Score:</td>
                            <td class='type' style='padding-left:5px;display:none'>Home <input style='width:20px' type='text' onkeypress="return CheckDecNumber(event);" id='txtHomeScore' maxlength='2'/></td>
                            <td class='type' style='padding-left:7px;display:none'>Away <input style='width:20px' type='text' onkeypress="return CheckDecNumber(event);" id='txtAwayScore' maxlength='2'/></td>
                            <td class='type' style='padding-left:15px;display:none'>Date Time (GMT +8)</td>
                            <td class='type' style='padding-left:5px;display:none'><span class='p15 type' style='display:none'><input type='checkbox' id='chkDateTime' onclick='mixparlay.eventOnDateTime();'>From<input type="text" id="txtFromdate" class="txtcalendar" readonly style="cursor: pointer;" /></span></td>
                            <td class='type' style='padding-left:5px;display:none'><span class='p15 type' style='display:none'>To<input type="text" id="txtTodate" class="txtcalendar" readonly style="cursor: pointer;" /></span></td>
                            <td class='type' style='padding-left:10px;display:none'><span style='color:red'>(Filtering by Score and Time is optional)</span></td>
                        </tr>
                        <tr>
                            <td><input type='checkbox' id='chkMixparlay' checked='checked' />Mix Parlay</td>
                            <td><input type='checkbox' id='chkSystemparlay' />System Parlay</td>
                        <tr/>
		            </tbody></table>
		        </td>
		    </tr> 
		    <tr>
			    <td colspan="2">                
			        <div id="bettype-bidding" style="float:left;width:1005px;padding:3px 0px;">
			            <div class="bettype-header" style="float:left;width:1005px; background-color:#B5C4D0;border:1px solid #7C98A4;">
			                <input id="chk-bettype-all" class="chk-bettype" type="checkbox"><span class="check bettype" rel="all"><b>All Bet types</b></span>
			            </div>
			            <div class="bettype-cont" style="float:left;width:1005px;border:1px solid #7C98A4;border-top:none;background-color:#FFF">
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
        <div class="betrow head border-grid-header" style="width:1160px">
            <table border="0" cellpadding="0" cellspacing="0" style="width:90%"><tbody><tr>
            <th class="border-right"><div class="betrow index">NO</div></th>
            <th class="border-right"><div class="betrow account">USER</div></th>
            <th class="border-right"><div class="betrow transtime">TRANS.TIME</div></th>
            <th class="border-right"><div class="betrow refno">BETID</div></th>
            <th class="border-right"><div class="betrow refno">BETTYPE</div></th>
            <th class="border-right"><div class="betrow winloss">WINLOSS</div></th>
            <th class="border-right"><div class="betrow odds-mix" style="text-align:center">ODDS</div></th>
            <th class="border-right"><div class="betrow amount">STAKE</div></th>
            <th class="border-right"><div class="betrow status-mix">STATUS</div></th>
            <th class="border-right"><div class="betrow pt">SUPER PT</div></th>
            <th class="border-right"><div class="betrow pt">MASTER PT</div></th>
            <th class="border-right"><div class="betrow pt">AGENT PT</div></th>
            <th class="border-right"><div class="betrow choice">CHOICE</div></th>
            <th class="border-right"><div class="betrow option">OPTION</div></th>
            </tr></tbody></table>
        </div>
        <div id="box_list_scroll" style="margin-left:0px;width:1160px"></div>
        
    </script>
    <script id="item_bettype_tpl" type="text/html">
        {{if Rows.length}}
        <div class="betrow title border-grid-row" id="betrow_title_${Type}" style="display:block;width:1142px;">
            <div class="title_label" style="float:left; width: 850px;cursor: pointer;" rel="bettype_${Type}"><span>&nbsp;</span>${Type}</div>
            <div style="float: left;width: 200px;">
                <a id="expand-all-${Type}" type="${Type}" href="javascript:;" class="txt-expand-all" status="0" style="text-decoration:none;font-weight:bold;color:#FFF;">Expand All</a>
            </div> 
            <div style="float: right;"> 
                <select onchange="mixparlay.ddlChange('${Type}');" name="ddl${Type}" id="ddl${Type}" class="sel-partner" partner="${Type}">
                    <option value="0">Not Delete</option>
                    <option value="1">Delete All</option>
                    <option value="2" selected="selected">Optional</option>
                </select>
            </div>
        </div>
        <div id="bettype_${Type}" class="bettype">
            <table id="tbl_${Type}" border="0" cellpadding="0" cellspacing="0" style="width:90%;"><tbody>
            </tbody></table>
        </div>
        {{/if}}
    </script>
    <script id="item_betrans_tpl" type="text/html">
        <tr class="betrow {{if Liveindicator}}live{{/if}}">
            <td class="border right top"><div class="betrow index">
                ${Order}</div></td>
            <td class="border right top"><div class="betrow account">
                ${Account}</div></td>
            <td class="border right top"><div class="betrow transtime">
                ${TransTime}</div></td>
            <td class="border right top"><div class="betrow refno">
                ${TransId}</div></td>
            <td class="border right top"><div class="betrow refno">
                ${Bettype}</div></td>
            <td class="border right top"><div class="betrow winloss">
                ${WinLoss}</div></td>
            <td class="border right top"><div class="betrow odds-mix">
                <strong><span style="color: blue;">${Odds}</span><br>
                    ${OddType}</strong></div></td>
            <td class="border right top"><div class="betrow amount ">
                ${Amount}<br/>(${Currency})</div></td>
            <td class="border right top"><div class="betrow status-mix">
                ${Status}</div></td>
            <td class="border right top"><div class="betrow pt">
                ${SPT}</div></td>
            <td class="border right top"><div class="betrow pt">
                ${MPT}</div></td>
            <td class="border right top"><div class="betrow pt">
                ${APT}</div></td>
            <td class="border right top">
                <div id="choice-${Refno}" class="betrow choice ${Refno}">
                    <div id="expand-cont-${Refno}" class="expand-cont-${Type}">{{html Choice}}</div>
                    <div id="expand-all-${Refno}" class="expand-all-${Type}"></div>
                </div>
            <td class="border right top"><div class="betrow option">
                <input type="checkbox" value="${SubTransId}" id="${TransId}" name="chk${Type}" class="chk${Type}-${IsDelete}" {{if IsDelete=="0"}}disabled="disabled"{{/if}} refno="${Refno}" /></div></td>
        </tr>
    </script>
    <script type="text/html" id="row_match">
        {{if MatchCode}}
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
                    Special Delete - Mix Parlay
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box" style="width:1182px; padding:4px 0px!important">
            <div id="ErrMsg" class="errMsg">
            </div>
        </div>
        <div class="bg_table" style="width:982px;">
            <div class="box_header setting" rel="box_setting" style="width: 1166px;"><span>Filter Option</span></div>
            <div id="box_setting" style="padding-top: 3px; float:left;">
                <div id="box_setting_form"></div>
                <div class="box_button list" style="display: none;">
                    <input class="btn showbet" type="button" value="  Show  " style="width: 100px" />
                </div>
            </div>
            <div class="box_clear"></div>
            <div class="box_header list" style="display:none; width:1166px;" rel="box_list_cont_mp:box_match_info"><span>Bet List</span></div>
            <div id="box_match_info"></div>
            <div id="box_list_cont" style="display:none; width:1166px;"></div>
            <div class="box_clear"></div>
            <div id="pagination" style="float: left; width: 1161px;display:none;"></div>
            <div class="box_clear"></div>
            <div class="box_header message" style="display:none; width:1166px;" rel="box_list_message"><span class="up">Bet Message</span></div>
            <div id="box_list_message" style="display:none; width:1166px;" >
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
                                <span id="span-msg-en" style="color: Red;display:none;">*</span>Message En
                            </th>
                            <th align="center">
                                <span id="span-msg-ch" style="color: Red;display:none;">*</span>Message Ch
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
                                <a href="javascript:;" onclick="hideLanguage(this)" id="other_display"><span style="color: Black;">
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
                </form>
            </div>
            <div class="box_clear"></div>
            <div class="box_header confirm" style="display:none; width:1166px;" rel="box_list_confirm">
                <span>Confirm Information</span></div>
            <div id="box_list_confirm" class="box_button delete" style="display:none;">
                <div class="box_clear"></div>
                <table border="0px" class="table">
                    <tbody>
                        <tr>
                            <td>REASON:</td>
                            <td>
                                <input type="text" id="reason" name="reason" onkeyup="countChar(this,150,'#charNum')" />&nbsp;<span style="color:Red;">(*)</span><i><label id="charNum">Characters Left: 150</label></i>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                USERNAME:
                            </td>
                            <td>
                                <input type="text" id="username" name="username" disabled="disabled" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PASSWORD:
                            </td>
                            <td>
                                
                                <input type="password" id="userpwd" name="userpwd">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                SEND PERSONAL MESSAGE:
                            </td>
                            <td>
                                <input style="margin-left: 0px;" type="checkbox" id="chk-send-email" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="error-message" id="ErrorMessage">
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <br/>
                <input type="hidden" id="userId" name="userId" value="<%=AccId %>" />
                <input type="button" class="btn delbet" value="&nbsp;&nbsp;Delete Bet&nbsp;&nbsp;" style="width: 100px; float: left;" />
            </div>
            <div class="box_clear"></div>
            <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
            <script src="../../js/jQuery/Pager/jpagergopage.js" type="text/javascript"></script>
            <script src="js/setting.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
            <script src="js/mixparlay.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
            <script type="text/javascript">
                var AccUserName = '<%=Username %>'
                $(document).ready(function() {
                    mixparlay.initForm();
                });
            </script>
        </div>
    </div>
</body>
</html>

