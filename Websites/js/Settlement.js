var gSubmitParams ='';
function ClosePopupSettlement() {
    parent.dhtmlwindow.close(ctrWnd);    
}
function ChangeSport() {    
    var isCal =($('m').value =='CALC');
    if (isCal) {
      
        try{parent.BlockUI();}catch(e){}
        $('sContent').innerHTML = '';
//        $("divcontainer").addClassName("allhidden");
        var sportType =$('ddlSports').options[$('ddlSports').selectedIndex].value;    
        //var _classSuffix = GetDivClassSuffix(sportType);    
        //$('divMatch').className ='divMatch' +_classSuffix;
        //$('frMatch').className ='frMatch' +_classSuffix;    
        var myajax= new Ajax.Request("ChangeSport.ashx",{
            asynchronous:true,
            method:'post',        
            parameters:'fdate=' +$('fdate').value +'&tdate=' +$('tdate').value +'&sportID=' +sportType,
            onComplete:BindLeagues
        });
    }
}

function GetDivClassSuffix(sportType) {
    switch (sportType) {                
        case '8': 
            return '78';
        case '1':
        case '2':
        case '3':        
        case '5': 
        case '6':
        case '9':        
            return '65';
        default: return '58';            
    }
}
function BindLeagues(rs) {    
    $('selLeague').innerHTML =rs.responseText;

    parent.jQuery.unblockUI();
}



function GetSettleMatch() {
    //BeginLoading();
    try{parent.BlockUI();}catch(e){}
    //var isCal =$('chkTypeCal').checked ? 1 : 0;
    if ($('ddlLeagues')) {
        _leagueid = parseInt($('ddlLeagues').options[$('ddlLeagues').selectedIndex].value);
    }
    var isCal =($('m').value =='CALC') ?1 :0;
    //var params ='isCalculate=' +isCal;
    var params ='mode=' +$('m').value;
    params +='&fdate=' +$('fdate').value;    
    params +='&sportID=' +$('ddlSports').options[$('ddlSports').selectedIndex].value;    
    if (isCal ==1) {
        params +='&tdate=' +$('tdate').value;
        params +='&leagueID=' +$('ddlLeagues').options[$('ddlLeagues').selectedIndex].value;
        params +='&isUncal=' +(($('chkUnCal').checked) ? 1 : 0);
        params +='&isUncalMul=' +(($('chkUnCalMultiple').checked) ? 1 : 0);
    }    
    gSubmitParams =params;        
    var myajax= new Ajax.Request("GetSettleMatch.ashx",{
        asynchronous:true,
        method:'post',        
        parameters: params, //isCalculate | fdate | tdate | sportID | leagueID | isUncal | isUncalMul        
        onComplete:BindSettleMatch
    });
}

function BindSettleMatch(rs) {
    $('sContent').innerHTML = rs.responseText;
    //FinishLoading();
    $("divcontainer").removeClassName("allhidden");
    
    initMultipleCalculate();
    
    parent.jQuery.unblockUI();
}

function Refresh() {
    //BeginLoading();
    try{parent.BlockUI();}catch(e){} 
    var myajax= new Ajax.Request("GetSettleMatch.ashx",{
        asynchronous:true,
        method:'post',        
        parameters: gSubmitParams, //isCalculate | fdate | tdate | sportID | leagueID | isUncal | isUncalMul        
        onComplete:BindSettleMatch
    });
}

function CloseDiv(hasUpdated) {    
    if (hasUpdated) Refresh();    
    $('divMatch').style.display ='none';    
    self.frames['frMatch'].$('_sContent').innerHTML ='';
    this.focus();
}

function DoCal(type, params) {    
    //$('frMatch').src ='Calculation.aspx?p=' +params +'&type=' +type;
    //$('divMatch').style.display ='inline';
    var SportID = params.split('|')[1];
    var height = 330;
    if (SportID == 1) height = 520;
    var IsIE7 = navigator.appVersion.indexOf('MSIE 7.0') > 0;
    if (type != 'b') {
        height = 240;
    }
    if (IsIE7) height = height + 50;
    var url = 'Calculation.aspx?p=' + params + '&type=' + type;

    var title = 'Settlement';
    switch (type) {
        case 'r': title = 'Refund'; height += 80; break;
        case 'k': title = 'Key Score'; break;
    }
    var width = 620;
    if (SportID == 8)
        width = 750;
//    if (type=='b' || type=='r')
//    {    
//        ShowPopupSettle(url, title, width, height, 50, 200);
//    }
//    else
//    {
        ShowPopupSettle(url, title, width, height, 50, 200);
    //}
}

function DoCalHR(type, params) 
{  
    var url = 'CalculationHR.aspx?p=' + params + '&type=' + type;
   
    ShowPopup(url, 'Settlement', 700, 340, 50, 200);
}

function DoCalHR1(obj) 
{
    var Placing;
    var SCR=0;
    var TF=0;
    var Win=0;
    var Place=0
    Placing=$(obj.id.replace(/_BtnKey/,"")).options[$(obj.id.replace(/_BtnKey/,"")).selectedIndex].value;
    Win=$(obj.id.replace(/_BtnKey/,"_Win")).value;
    Place=$(obj.id.replace(/_BtnKey/,"_Place")).value;
    SCR=$(obj.id.replace(/_BtnKey/,"_SCR")).checked ? 1 : 0;
    TF=$(obj.id.replace(/_BtnKey/,"_TF")).checked ? 1 : 0;
    alert("Placing: " + Placing + ", SCR: " + SCR + ", TF: " + TF + ", Win: " + Win + ", Place:" + Place );
}

function DoCalFGLG(params) {
    var height = 280;
    var url = 'Calculation.aspx?type=b&p=' + params + '&calType=fglg';
    ShowPopup(url, 'Settlement', 600, height, 50, 200);
}

function DoCalSoccer(matchID, sportID) {
    var url = 'Calculation.aspx?type=b&p=' + matchID + '|' + sportID + '&calType=main';
    var height = 400;
    ShowPopup(url, 'Settlement', 600, height, 50, 200);
}


function DoCal2(type, params) {

    var sConfirm = '';
    switch(type)
    {
        case 'm':
            sConfirm = 'Are you sure you want to CALCULATE MULTIPLE This match?';
            break;
        case 'r':
            sConfirm = 'Are you sure you want to REFUND MULTIPLE This match?';
            break;

        case 'rht':
            sConfirm = 'Are you sure you want to REFUND MULTIPLE HT This match?';
            break;
            
        case 'rft':
            sConfirm = 'Are you sure you want to REFUND MULTIPLE FT This match?';
            break;
        default:
            sConfirm = 'Invalid type, Please try again!!!';
            alert(sConfirm);
            return;            
    }
    if (parseInt(params.split("|")[1])!=0)
    {
        if (!confirm(sConfirm)) return;
    }
    //BeginLoading();
    if (parent.parent.BlockUI) {
        try{parent.parent.BlockUI();}catch(e){}
    }
    else {
        try{parent.BlockUI();}catch(e){}
    }
    if (type=="m")
    {
        var myajax= new Ajax.Request("Multiple.ashx",{
            asynchronous:true,
            method:'post',        
            parameters: 'p=' +params +'&type=' +type,
            onComplete:MultipleResult
        });
    }
    else
    {
         var myajax= new Ajax.Request("Multiple.ashx",{
            asynchronous:true,
            method:'post',        
            parameters: 'p=' +params +'&type=' +type,
            onComplete:RefundResult
        });
    }
}

function MultipleResult(rs) {
    //FinishLoading();
   
    if (parent.parent.jQuery && parent.parent.jQuery.unblockUI) {
                parent.parent.jQuery.unblockUI();
            }
            else {
                parent.jQuery.unblockUI();
            }

    jQuery("#msgResult").html(rs.responseText);
    jQuery("#dialog-result" ).data('result',rs.responseText).dialog("open");
    
    if ($('divMatch')) $('divMatch').hide();

    
    parent.ClosePopup();  
}
function RefundResult(rs) {
    //FinishLoading();
   
    try{parent.parent.jQuery.unblockUI();}catch(e){}
    var result = rs.responseText;

    alert(result);
    if ($('divMatch')) $('divMatch').hide();

    if (result == 'Refund Multiple Successfully!' || result == 'Refund Multiple FT Successfully!' || result == 'Refund Multiple HT Successfully!') { parent.Refresh(); }
    parent.ClosePopup();  
}

function GetMatchSettleLog() {
    $('#spMsg').html('');
    if ($('#txtMatchCode').val().length == 0) {
        showAlertMessage("spMsg", "Please input a match code", 1);
        $('#txtMatchCode').focus();
        return;
    }
    if (parent.parent.BlockUI) {
        try{parent.parent.BlockUI();}catch(e){}
    }
    else {
        try{parent.BlockUI();}catch(e){}
    }
    var param = 'mode=3&matchcode=' + $('#txtMatchCode').val().replace('&', '^');
    param += '&fdate=' + $('#fromdate').val();
    param += '&tdate=' + $('#todate').val();
    param += '&t=' + $('#t').val();
    $.ajax({
        type: 'POST',
        url: '../Settle/ProcessData.ashx',
        async: true,
        data: param,
        success: function(rs) {
            $('#sData').html(rs);
            InitFixedHeaderLog(800);
            if (parent.parent.jQuery && parent.parent.jQuery.unblockUI) {
                parent.parent.jQuery.unblockUI();
            }
            else {
                parent.jQuery.unblockUI();
            }
        }
    });

}


function InitFixedHeaderLog(widthmainpage) {

    if ($('#trFooter').length == 0) return;

    $('#trFooter').show();
    var topHeader = $('#trHeader').position().top;
    var topFooter = $('#trFooter').position().top;
    $('#trFooter').hide();

    var innerHeight = window.innerHeight;
    var delta = 0;
    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 0;
    }

    var RealHeight = $(window).height() - topHeader - 10;
    //alert('topHeader: ' + topHeader + '\ntopFooter: ' + topFooter);
    // alert(RealHeight);

    if ((topFooter - topHeader) > $(window).height()) {
        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }

}

function ShowSettlementLog(matchcode) {
    var now = new Date();
    var dateFormat = (now.getMonth()+1)+'/'+ now.getDate() + '/' + now.getFullYear();
    var fdate = $('#fdate') ? $('#fdate').val() : dateFormat;
    var tdate = $('#tdate') ? $('#tdate').val() : dateFormat;

	matchcode = matchcode.replace('&', '^');
	var title = 'SETTLEMENT LOG';
	var url = 'Settle/GetSettleLog.aspx?matchcode=' + matchcode + '&fdate=' + fdate + '&tdate=' + tdate;
    var width = 950;
    var height = 600;
    parent.OpenPopup(url, title, width, height, true, true, window, 2000, null, null);
}

function CloseDiv() {
    ibcWnd.close();
}

function DisplayMatchInfoSettleLog(matchid) {
    var matchinfo = document.getElementById(matchid);
    matchinfo.className = (matchinfo.className.length == 0) ? 'hide' : '';
}

function refundNumberGame(matchId) {
    numgamerefund.confirm(matchId, 'ht');
    return;

    if (!confirm('Are you sure you want to Refund this match?')) return;
    var url = 'CalculationNumberGame.ashx?action=refund' + '&matchid=' + matchId;
    try { parent.BlockUI(); } catch (e) { }
    var myajax = new Ajax.Request(url, {
        asynchronous: true,
        method: 'post',
        parameters: '',
        onComplete: refundNumberGameResult
    });
}

function refundNumberGameFt(matchId) {
    numgamerefund.confirm(matchId, 'ft');
    return;

    if (!confirm('Are you sure you want to Refund this match?')) return;
    var url = 'CalculationNumberGame.ashx?action=refundft' + '&matchid=' + matchId;
    try { parent.BlockUI(); } catch (e) { }
    var myajax = new Ajax.Request(url, {
        asynchronous: true,
        method: 'post',
        parameters: '',
        onComplete: refundNumberGameResult
    });
}

function loadNumberGameResult() {
    parent.jQuery.unblockUI();
    GetSettleMatch();

}

function refundNumberGameResult(rs) {
    //FinishLoading();
    ClosePopup();
    setTimeout('loadNumberGameResult()', 1000);
}
///
/// Number game functons
///
var numgamerefund = {
    curMatchId: 0,
    message: { title: "Number Game - Refund{type} confirmation",
        errorMatch: "Number Game: Match invalid.",
        errorMessage: "Number Game: Refund message invalid."
    },
    confirm: function (matchId, type) {
        if (parseInt(matchId) > 0) {
            this.curMatchId = parseInt(matchId);
            var title = "", width = 520, height = 100;
            if (type.toUpperCase() == 'FT') {
                title = this.message.title.replace(/{type}/g, " " + type.toUpperCase());
            } else {
                title = this.message.title.replace(/{type}/g, "");
            }

            var template = jQuery("#numbergame_templaterf").html();
            template = template.replace(/{matchid}/gi, matchId);
            template = template.replace(/{type}/gi, type);
            // global popup 
            var left = (jQuery('body').width() / 2) - (width / 2);
            ctrWnd = dhtmlwindow.open('numbergame_box', 'inline', template, title, 'width=' + width + 'px,height=' + height + 'px,left=' + left + 'px,top=150px,resize=0,scrolling=1');
            jQuery("div.drag-contentarea .message").hide();
        }
        else {
            alert(this.message.errorMatch);
        }
        return false;
    },
    isCheckSend: function (chkElm) {
        var isSend = null, isChecked;
        if (typeof chkElm == 'undefined') {
            var chkElm = jQuery("input#chkissend" + this.curMatchId);
            isChecked = jQuery(chkElm).attr("checked");
            isSend = !isChecked;
            jQuery(chkElm).attr("checked", isSend);
        }
        else {
            isSend = jQuery(chkElm).attr("checked");
        }
        if (isSend) {
            jQuery("div.message." + this.curMatchId).show();
            jQuery("div.drag-contentarea").css({ height: '300px' });

        } else {
            jQuery("div.message." + this.curMatchId).hide();
            jQuery("textarea." + this.curMatchId).val('');
            jQuery("div.drag-contentarea").css({ height: '100px' });
        }
        return false;
    },
    refundSubmit: function (matchId, type) {
        var validSend = this.validSend();
        if (validSend > 0) {
            type = (type == 'ft') ? 'refundft' : 'refund';
            var url = 'CalculationNumberGame.ashx';
            var params = { action: type, matchid: matchId, isMsg: 0 };
            if (validSend == 2) {
                params = jQuery.extend(params, this.getMessage());
            }
            try { parent.BlockUI(); } catch (e) { }
            jQuery.post(url, params, function (resp) {
                refundNumberGameResult(resp);
            });
        } else {
            alert(this.message.errorMessage);
        }
    },
    validSend: function () {
        var validSend = 0, isSend = jQuery("input#chkissend" + this.curMatchId).attr("checked");
        if (isSend) {
            var elmMessage = jQuery("textarea.ereacontent." + this.curMatchId);
            for (var i = 0, len = elmMessage.length; i < len; i++) {
                var areaText = jQuery(elmMessage[i]).val();
                if (jQuery.trim(areaText) != '') {
                    validSend = 2; break;
                }
            }
        } else {
            validSend = 1; // NOT MESSAGE
        }
        return validSend;
    },
    getMessage: function () {
        var message = { isMsg: 1, msgen: '', msgch: '', msgcs: '' };
        var elmMessage = jQuery("textarea.ereacontent." + this.curMatchId);
        for (var i = 0, len = elmMessage.length; i < len; i++) {
            var areaText = jQuery(elmMessage[i]).val();
            if (jQuery.trim(areaText) != '') {
                switch (i) {
                    case 0:
                        message.msgen = jQuery.trim(areaText);
                        break;
                    case 1:
                        message.msgch = jQuery.trim(areaText);
                        break;
                    case 2:
                        message.msgcs = jQuery.trim(areaText);
                        break;

                    default:
                        break;
                }
            }
        }
        return message;
    }
};

