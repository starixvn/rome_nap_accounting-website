var gHasUpdated = false;
var isInputScoreDirectly = false;
function DoExit() {
    parent.CloseDiv(gHasUpdated);
}
function FromKP(e) {
    if (!e) e = window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;
    if (key == 27) { DoExit(); return false; }
}

function OnKP(e, val) {
    if (!e) e = window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;

    if (key == 13) {
        var _type = $('btnSubmit').value;
        if (_type == 'Calculate ALL') { // bill soccer only
            var _qstr = parent.location.search.substring(1);
            var _ext = '';
            if (_qstr == 'm=htoveronly') _ext = 'HT';
            if (_qstr == 'm=overonly') _ext = 'FT';
            DoCalculate('bill', _ext);
            return false;
        }
        if (_type == 'BILL') DoCalculate('bill');
        if (_type == 'KEY SCORE') DoCalculate('keyscore');
        return false;
    }
    if (key == 27) { DoExit(); return false; }
    if ((key < 33) || (key == 37) || (key == 39) || (key == 46)) return true;
    if (/[^0-9]/.test(String.fromCharCode(key))) return false;
    return true;
}

function OnKP_HR(e, val) {
    if (!e) e = window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;


    if (key == 27) { DoExit(); return false; }
    if ((key < 33) || (key == 37) || (key == 39) || (key == 46)) return true;
    //if (/^$/.test(val.value) && /0/.test(String.fromCharCode(key))) return false;
    if (/[^0-9]/.test(String.fromCharCode(key))) return false;
    return true;
}

function CheckNoGoal() {
    var isDisabled = $('chkNoGoal').checked;
    $('chkFgHome').disabled = isDisabled;
    $('chkFgAway').disabled = isDisabled;
    $('chkLgHome').disabled = isDisabled;
    $('chkLgAway').disabled = isDisabled;
    var chkFGLG = Form.getInputs('frmBill', 'radio', 'chkFGLG').find(function(radio) { return radio.checked; }).value;
    if (!isDisabled && chkFGLG == 2) {
        $('chkLgHome').disabled = true;
        $('chkLgAway').disabled = true;
    }
}

function OnKUSoccer() {
    try {
        $('errMsg').style.display = 'none';        
        var _homeH = $('homeH').value.trim();
        var _homeF = $('homeF').value.trim();
        var _awayH = $('awayH').value.trim();
        var _awayF = $('awayF').value.trim();
        if (_homeH == '0') _homeH = '';
        if (_homeF == '0') _homeF = '';
        if (_awayH == '0') _awayH = '';
        if (_awayF == '0') _awayF = '';

        if (_homeH == '' && _homeF == '' && _awayH == '' && _awayF == '') {
            $('chkFgHome').checked = false;
            $('chkLgHome').checked = false;
            $('chkFgAway').checked = false;
            $('chkLgAway').checked = false;
            $('chkFgHome').disabled = 'disable';
            $('chkLgHome').disabled = 'disable';
            $('chkFgAway').disabled = 'disable';
            $('chkLgAway').disabled = 'disable';
        }
        else {
            $('chkFgHome').disabled = '';
            $('chkLgHome').disabled = '';
            $('chkFgAway').disabled = '';
            $('chkLgAway').disabled = '';
        }
    }
    catch (ex) { }
}

//-- Basketball, Football
function OnKUBasketball(e) {
    $('errMsg').style.display = 'none';
    
    var totalHH = 0, totalHA = 0;
    var totalFH = 0, totalFA = 0;
    var i = 1;
    for (var i = 1; i < 5; i++) {
        var _scurH = $('txt' + i + 'qHome').value.trim();
        var _scurA = $('txt' + i + 'qAway').value.trim();
        if (_scurH == '') _scurH = '0';
        if (_scurA == '') _scurA = '0';
        var currH = (/[^0-9]/.test(_scurH)) ? 0 : parseInt(_scurH, 10);
        var currA = (/[^0-9]/.test(_scurA)) ? 0 : parseInt(_scurA, 10);
        if ($('txt' + i + 'qHome').value.trim() != '0') $('txt' + i + 'qHome').value = (currH == 0) ? '' : currH.toString();
        if ($('txt' + i + 'qAway').value.trim() != '0') $('txt' + i + 'qAway').value = (currA == 0) ? '' : currA.toString();
        totalFH += currH;
        totalFA += currA;
        if (i == 2) {
            totalHH = totalFH;
            totalHA = totalFA;
        }
    }
    totalFH += ($('txtOTHome').value == '') ? 0 : parseInt($('txtOTHome').value, 10);
    totalFA += ($('txtOTAway').value == '') ? 0 : parseInt($('txtOTAway').value, 10);

    $('homeH').value = totalHH.toString();
    $('awayH').value = totalHA.toString();
    $('homeF').value = totalFH.toString();
    $('awayF').value = totalFA.toString();

    if ($('IsHTOnly').value == '1') {
        $('homeF').value = '';
        $('awayF').value = '';
    }

    
}

//-- Hockey
function OnKUHockey(e) {
    $('errMsg').style.display = 'none';
    
    var totalFH = 0, totalFA = 0;
    var i = 1;
    for (var i = 1; i < 4; i++) {
        var _scurH = $('txt' + i + 'pHome').value.trim();
        var _scurA = $('txt' + i + 'pAway').value.trim();
        if (_scurH == '') _scurH = '0';
        if (_scurA == '') _scurA = '0';
        var currH = (/[^0-9]/.test(_scurH)) ? 0 : parseInt(_scurH, 10);
        var currA = (/[^0-9]/.test(_scurA)) ? 0 : parseInt(_scurA, 10);
        if ($('txt' + i + 'pHome').value.trim() != '0') $('txt' + i + 'pHome').value = (currH == 0) ? '' : currH.toString();
        if ($('txt' + i + 'pAway').value.trim() != '0') $('txt' + i + 'pAway').value = (currA == 0) ? '' : currA.toString();
        totalFH += currH;
        totalFA += currA;
    }
    totalFH += ($('txtOTHome').value == '') ? 0 : parseInt($('txtOTHome').value, 10);
    totalFA += ($('txtOTAway').value == '') ? 0 : parseInt($('txtOTAway').value, 10);
    $('homeF').value = totalFH.toString();
    $('awayF').value = totalFA.toString();
}

//-- Tennis
function OnKUTennis(e) {
    $('errMsg').style.display = 'none';
    
    var totalGH = 0, totalGA = 0;
    var totalFH = 0, totalFA = 0;
    var i = 1;
    for (var i = 1; i < 6; i++) {
        var _scurH = $('txt' + i + 'sHome').value.trim();
        var _scurA = $('txt' + i + 'sAway').value.trim();
        if (_scurH == '') _scurH = '0';
        if (_scurA == '') _scurA = '0';
        var currH = (/[^0-9]/.test(_scurH)) ? 0 : parseInt(_scurH, 10);
        var currA = (/[^0-9]/.test(_scurA)) ? 0 : parseInt(_scurA, 10);
        if ($('txt' + i + 'sHome').value.trim() != '0') $('txt' + i + 'sHome').value = (currH == 0) ? '' : currH.toString();
        if ($('txt' + i + 'sAway').value.trim() != '0') $('txt' + i + 'sAway').value = (currA == 0) ? '' : currA.toString();
        totalGH += currH;
        totalGA += currA;
        if (currH > currA) {
            totalFH++;
        }
        else if (currH < currA) {
            totalFA++;
        }
    }
    $('txtTGHome').value = totalGH.toString();
    $('txtTGAway').value = totalGA.toString();
    $('txtFSHome').value = totalFH.toString();
    $('txtFSAway').value = totalFA.toString();
}

//-- Badminton, Volleyball
function OnKUBadminton(e) {
    $('errMsg').style.display = 'none';
    
    var totalGH = 0, totalGA = 0;
    var totalFH = 0, totalFA = 0;
    var i = 1;
    for (var i = 1; i < 6; i++) {
        var _scurH = $('txt' + i + 'sHome').value.trim();
        var _scurA = $('txt' + i + 'sAway').value.trim();
        if (_scurH == '') _scurH = '0';
        if (_scurA == '') _scurA = '0';
        var currH = (/[^0-9]/.test(_scurH)) ? 0 : parseInt(_scurH, 10);
        var currA = (/[^0-9]/.test(_scurA)) ? 0 : parseInt(_scurA, 10);
        if ($('txt' + i + 'sHome').value.trim() != '0') $('txt' + i + 'sHome').value = (currH == 0) ? '' : currH.toString();
        if ($('txt' + i + 'sAway').value.trim() != '0') $('txt' + i + 'sAway').value = (currA == 0) ? '' : currA.toString();
        totalGH += currH;
        totalGA += currA;
        if (currH > currA) {
            totalFH++;
        }
        else if (currH < currA) {
            totalFA++;
        }
    }
    $('txtTPHome').value = totalGH.toString();
    $('txtTPAway').value = totalGA.toString();
    $('txtTSHome').value = totalFH.toString();
    $('txtTSAway').value = totalFA.toString();
}

function OnKUBaseBall(e) {
    $('errMsg').style.display = 'none';
    
    var totalFH = 0, totalFA = 0, totalHTH = 0, totalHTA = 0;
    for (var i = 1; i < 10; i++) {
        var _scurH = $('txt' + i + 'Home').value.trim();
        var _scurA = $('txt' + i + 'Away').value.trim();
        if (_scurH == '') _scurH = '0';
        if (_scurA == '') _scurA = '0';
        var currH = (/[^0-9]/.test(_scurH)) ? 0 : parseInt(_scurH, 10);
        var currA = (/[^0-9]/.test(_scurA)) ? 0 : parseInt(_scurA, 10);
        if ($('txt' + i + 'Home').value.trim() != '0') $('txt' + i + 'Home').value = (currH == 0) ? '' : currH.toString();
        if ($('txt' + i + 'Away').value.trim() != '0') $('txt' + i + 'Away').value = (currA == 0) ? '' : currA.toString();
        if (i < 6) {
            totalHTH += currH;
            totalHTA += currA;
        }
        totalFH += currH;
        totalFA += currA;
    }
    totalFH += ($('txtEIHome').value == '') ? 0 : parseInt($('txtEIHome').value, 10);
    totalFA += ($('txtEIAway').value == '') ? 0 : parseInt($('txtEIAway').value, 10);
    $('homeH').value = totalHTH.toString();
    $('awayH').value = totalHTA.toString();
    $('homeF').value = totalFH.toString();
    $('awayF').value = totalFA.toString();
}

function RefundRunningBetDaq(matchid, matchcode) {
    try { parent.parent.BlockUI(); } catch (e) { }
    parent.jQuery.ajax({
        type: "POST",
        url: "Service/SettlementService.svc/RefundRunningBetDaqTickets",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"Matchid":' + matchid + '}',
        success: function (rs) {
            try { parent.parent.jQuery.unblockUI(); } catch (e) { }
            if (rs != "" && rs != null && typeof rs != "undefined") {
                $('errmsg').innerHTML = rs;
                $('errmsg').style.display = '';
            }
            else {
                parent.ClosePopup();
                parent.GetSettleMatch();
            }
        }
    });
}

function DoCalculate(type, ext) {
    $('errMsg').style.display = 'none';
    //MatchID|SportID
    var sportID = parseInt($('sParams').value.split('|')[1], 10);
    //only for bill, soccer and FT or ALL
    if (sportID == 1 && ext != 'HT' && (type == 'calculate' || type == 'bill')) {
        var goalrecord = $('GR').value;
        var homeF = ($('homeF').value.length == 0) ? 0 : parseInt($('homeF').value);
        var awayF = ($('awayF').value.length == 0) ? 0 : parseInt($('awayF').value);
        var homeH = ($('homeH').value.length == 0) ? 0 : parseInt($('homeH').value);
        var awayH = ($('awayH').value.length == 0) ? 0 : parseInt($('awayH').value);

        if (type == 'calculate') {
            if (homeH > homeF || awayH > awayF) {
                homeF = homeH;
                awayF = awayH;
            }
        }


        //alert('homeF: ' + homeF + '\nawayF: ' + awayF);

        var IsExistsNG = $('txtIsExistsNG').value == 'True';
        var IsLive = $('txtIsLive').value == 'True';
        var IsHedge = $('txtMatchCode').value.toLowerCase().indexOf('hedge') >= 0;

        if (!IsHedge && (type == 'calculate' || type == 'bill') && ($('sHasFGLG').value == '1' || IsExistsNG)) {

            if (IsLive) {
                if (((homeF + awayF) != goalrecord.length) || (ReplaceAll(ReplaceAll($('GR').value, 'a', ''), 'A', '').length != homeF)) {
                    $('errMsg').style.display = '';
                    $('errMsg').innerHTML = 'Goal record is not consistent with the score, please correct Goal record before calculating.';
                     try{parent.parent.jQuery.unblockUI();}catch(e){}
                    return;
                }
            }
            else {

                //Case H=0 A=0 -> GR = Empty
                if (homeF + awayF == 0 && goalrecord.length > 0) {
                    $('errMsg').style.display = '';
                    $('errMsg').innerHTML = "Goal record is not consistent with the score, please correct Goal record before calculating";
                    try{parent.jQuery.unblockUI();}catch(e){}
                    return false;
                }
                //Case H=0 A>0 --> GR = A only
                if (homeF == 0 && awayF > 0 && (goalrecord.toLowerCase().indexOf("h") >= 0 || goalrecord.toLowerCase().indexOf("a") < 0)) {
                    $('errMsg').style.display = '';
                    $('errMsg').innerHTML = "Goal record is not consistent with the score, please correct Goal record before calculating";
                     try{parent.parent.jQuery.unblockUI();}catch(e){}
                    return false;
                }
                //Case H>=1 A=0 -->GR = H only

                if (homeF >= 1 && awayF == 0 && (goalrecord.toLowerCase().indexOf("a") >= 0 || goalrecord.toLowerCase().indexOf("h") < 0)) {
                    $('errMsg').style.display = '';
                    $('errMsg').innerHTML = "Goal record is not consistent with the score, please correct Goal record before calculating";
                    try{parent.parent.jQuery.unblockUI();}catch(e){}
                    return false;
                }


                if (homeF == 1 && awayF == 1 && (goalrecord.toLowerCase().indexOf("a") < 0 || goalrecord.toLowerCase().indexOf("h") < 0 || goalrecord.toLowerCase().substring(0, 1) == goalrecord.toLowerCase().substring(goalrecord.length - 1, goalrecord.length))) {
                    $('errMsg').style.display = '';
                    $('errMsg').innerHTML = "Goal record is not consistent with the score, please correct Goal record before calculating";
                    try{parent.parent.jQuery.unblockUI();}catch(e){}
                    return false;
                }
                //Case H=1 A>1 -->GR = Exist A
                if (homeF == 1 && awayF > 1 && goalrecord.toLowerCase().indexOf("a") < 0) {
                    $('errMsg').style.display = '';
                    $('errMsg').innerHTML = "Goal record is not consistent with the score, please correct Goal record before calculating";
                     try{parent.parent.jQuery.unblockUI();}catch(e){}
                    return false;
                }
                //Case H>1 A=1 -->GR = Exist H
                if (homeF > 1 && awayF == 1 && goalrecord.toLowerCase().indexOf("h") < 0) {
                    $('errMsg').style.display = '';
                    $('errMsg').innerHTML = "Goal record is not consistent with the score, please correct Goal record before calculating";
                    try{parent.parent.jQuery.unblockUI();}catch(e){}
                    return false;
                }
                if (homeF + awayF > 0 && goalrecord.length <= 0) {
                    $('errMsg').style.display = '';
                    $('errMsg').innerHTML = "Goal record is not consistent with the score, please correct Goal record before calculating";
                     try{parent.parent.jQuery.unblockUI();}catch(e){}
                    return false;
                }

                if (((homeF + awayF) != goalrecord.length) || (ReplaceAll(ReplaceAll($('GR').value, 'a', ''), 'A', '').length != homeF)) {
                    $('errMsg').style.display = '';
                    $('errMsg').innerHTML = 'Goal record is not consistent with the score, please correct Goal record before calculating.';
                     try{parent.parent.jQuery.unblockUI();}catch(e){}
                    return false;
                }
              
            }
        }
    }

    switch (type) {
        case 'bill':
            if (!isInputScoreDirectly && !CheckInputValue(sportID)) {
                 try{parent.parent.jQuery.unblockUI();}catch(e){}
                return;
            }
            if (typeof (ext) != 'undefined' && ext != '') {
                if (ext == 'HT') {
                    var s = '';

                    if ($('IsAlreadyRefundHT').value.toLowerCase() == 'true') {
                        s = 'This match is already refunded HT. All refund will be rollback if you calculate HT!\n';
                    }
                    s += 'Please double check the score!\nYou will not be able to cancel the FIRST HALF CALCULATION of this match!';

                    if (!confirm("WARNING!\n\n" + s)) {
                         try{parent.parent.jQuery.unblockUI();}catch(e){}
                        return;
                    }
                    if (!confirm("Check again!\nAre you sure you want to CALCULATE FIRST HALF for this match?")) {
                        try{parent.parent.jQuery.unblockUI();}catch(e){}
                        return;
                    }
                    type = 'billHT';
                }
                else if (ext == 'FT') {
                    if (!confirm("WARNING!\n\nPlease double check the score!\nYou will not be able to cancel the FULL TIME CALCULATION of this match!")) {
                        try{parent.parent.jQuery.unblockUI();}catch(e){}
                        return;
                    }
                    if (!confirm("Check again!\nAre you sure you want to CALCULATE only FULL TIME for this match?")) {
                         try{parent.parent.jQuery.unblockUI();}catch(e){}
                        return;

                    }
                    type = 'billFT';
                }
            }
            else {
                if (!confirm("WARNING!\n\nPlease double check the score and confirm this match is OVER!\nYou will not be able to cancel the CALCULATION and REUSE this match!")) {
                     try{parent.parent.jQuery.unblockUI();}catch(e){}
                    return;
                }
                if (!confirm("Check again!\nAre you sure you want to CALCULATE this match and it is OVER?")) {
                     try{parent.parent.jQuery.unblockUI();}catch(e){}
                    return;
                }
            }

            break;
        case 'calculate':
            if (!confirm("Are you sure you want to CALCULATE First Goal and No Goal?")) {
                 try{parent.parent.jQuery.unblockUI();}catch(e){}
                return;
            }
            break;
        case 'keyscore':
            if (!isInputScoreDirectly && !CheckInputValue(sportID)) {
                 try{parent.parent.jQuery.unblockUI();}catch(e){}
                return;
            }
            break;
        case 'unsettle':
            if (!confirm("WARNING!\n\nPlease double check and confirm this match really needs to be UNSETTLED!")) {
                 try{parent.parent.jQuery.unblockUI();}catch(e){}
                return;
            }

            if (!confirm("Check again!\nAre you sure you want to UNSETTLE this match?")) {
                 try{parent.parent.jQuery.unblockUI();}catch(e){}
                return;
            }
            break;
        case 'refund':
            scoreConfirm(type);
            return;
        case 'refundfulltime':
            scoreConfirm(type);
            return;
        case 'refundhalftime':
            scoreConfirm(type);
            return;
        case 'refundfirstnogoal':

            if (!confirm("Are you sure you want to REFUND All FGLG this match?")) {
                 try{parent.parent.jQuery.unblockUI();}catch(e){}
                return;
            }
            break;
        case 'refundalllastgoal':

            if (!confirm("Are you sure you want to REFUND All Last Goal this match?")) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                return;
            }
            break;
    }

    DoCalculate_Submit(type);
}

function DoCalculate_Submit(type) {
    $('sAction').value = type;
    try { parent.parent.BlockUI(); } catch (e) { }
    var myajax = new Ajax.Request("Calculation.ashx", {
        asynchronous: true,
        method: 'post',
        parameters: Form.serialize('frmBill'),
        onComplete: ResultCalculate
    });
};

function DoCalculateHR() {
    $('errMsg').style.display = 'none';
    $('infoMsg').style.display = 'none';

    var checkplacing1_4 = 0;
    for (var i = 1; i < 5; i++) {

        if (document.getElementById('sHName_' + i) != null && document.getElementById('sHName_' + i)[document.getElementById('sHName_' + i).selectedIndex].value == "-") {
            checkplacing1_4 = 1;
        }
        if (i < 4 && document.getElementById('txtPlace_' + i) != null && trim(document.getElementById('txtPlace_' + i).value, ' ') == "") {
            checkplacing1_4 = 1;
        }
    }
    if (trim(document.getElementById('txtWin_1').value, ' ') == "") {
        checkplacing1_4 = 1;
    }
    if (checkplacing1_4 == 1) {
        $('errMsg').style.display = '';
    }
    else {
        $('infoMsg').style.display = '';
    }
}

function ResultCalculate(rs) {
    var IsSuccess = 0;
    var delay = 0;
    
    $('errMsg').style.display = 'none';
    if (rs.responseText == 'success') {
        gHasUpdated = true;
        IsSuccess = 1;
    }
    else if (rs.responseText == 'success-refund') {
        alert('Refund All Successful!');
        gHasUpdated = true;
        IsSuccess = 1;
    }
    else if (rs.responseText == 'success-refundfulltime') {
        alert('Refund FT Successful!');
        gHasUpdated = true;
        IsSuccess = 1;
    }
    else if (rs.responseText == 'FGLGsuccess') {
        alert('In case you have changed the result of First Goal/Last Goal here, please re-calculate the MULTIPLE also to make sure all the tickets are update correctly.');
        gHasUpdated = true;
        IsSuccess = 0
    }
    else if (rs.responseText == 'soccerSuccess') {
        $('infoMsg').style.display = '';
        $('infoMsg').innerHTML = 'SUCCESS!';
        IsSuccess = 1; // that mean no need close the window settlement, 
    }
    else if (rs.responseText == 'success-refundalllastgoal') {
        alert('Refund All Last Goal Successful!');
        gHasUpdated = true;
        IsSuccess = 1;
    }
    else if (rs.responseText == 'Access Denied') {
        $('infoMsg').innerHTML = 'ACCESS IS DENIED!';
        $('infoMsg').style.display = '';
        IsSuccess = 0; // that mean no need close the window settlement, 
    }
    else if (rs.responseText.split('|')[0] == 'no') {
        $('errmsg').innerHTML = '<br/>' + rs.responseText.split('|')[1];
        $('errmsg').style.display = '';
        IsSuccess = 0;
    }
    else if (rs.responseText.split('|')[0] == 'delay')
    {
        IsSuccess=0;
        delay=parseInt(rs.responseText.split('|')[1]);
    }
    else {
        $('_sContent').innerHTML = rs.responseText;
        IsSuccess = 1;
    }

    if (IsSuccess == 1) {
        try{parent.parent.jQuery.unblockUI();}catch(e){}
        parent.ClosePopup();
        parent.GetSettleMatch();

    }
    else
    {
        try{parent.parent.jQuery.unblockUI();}catch(e){}
        if (delay>0)
        {
            SettleDelay(delay);
            //parent.parent.SettleDelay(delay);
        }
    }

}


/*Settlement delay*/
var totaltime=0;
var t;

 function timedCount()
  {
   
    if (totaltime>0)
      {
      document.getElementById("lblduration").innerHTML=totaltime;
      totaltime--;
      t=setTimeout("timedCount()",1000);
      }
    else
    {
       clearTimeout(t);
       overlay("overlaydiv");
       $("errmsg").style.display = 'none';
    }  
  }

function SettleDelay(duration)
 {
    totaltime=duration;
    $("errmsg").style.display = 'inline';
    overlay("overlaydiv");
    timedCount();
   
 }
  function overlay(id) {
    var el = $(id);
    if (el.style.display == 'none') {
       el.style.display='inline';
    }
    else {
         el.style.display='none';
    }
}


function CheckInputValue(sportID) {
    switch (sportID) {
        case 1: // soccer
            return true;
        case 2: // basketball
        case 3: // football
        case 26: // rugby    
        case 32: // netball        
            return IsValidScore(['txt1qHome', 'txt2qHome', 'txt3qHome', 'txt4qHome', 'txt1qAway', 'txt2qAway', 'txt3qAway', 'txt4qAway']);
        case 4: // ice hockey
        case 28: // field hockey
            return IsValidScore(['txt1pHome', 'txt2pHome', 'txt3pHome', 'txt1pAway', 'txt2pAway', 'txt3pAway']);
        case 5: // tennis
            return IsValidScore(['txt1sHome', 'txt2sHome', 'txt3sHome', 'txt4sHome', 'txt5sHome', 'txt1sAway', 'txt2sAway', 'txt3sAway', 'txt4sAway', 'txt5sAway']);
        case 10: // golf
        case 11: // Motorsports
        case 7: // billiard
            return true; //return IsValidScore(['homeF','awayF']);  
        case 8: // baseball
            return IsValidScore(['txt1Home', 'txt2Home', 'txt3Home', 'txt4Home', 'txt5Home', 'txt6Home', 'txt7Home', 'txt8Home', 'txt9Home', 'txt1Away', 'txt2Away', 'txt3Away', 'txt4Away', 'txt5Away', 'txt6Away', 'txt7Away', 'txt8Away', 'txt9Away']);
        case 9: // badminton
        case 6: // volleyball
            return IsValidScore(['txt1sHome', 'txt2sHome', 'txt3sHome', 'txt4sHome', 'txt5sHome', 'txt1sAway', 'txt2sAway', 'txt3sAway', 'txt4sAway', 'txt5sAway']);
        default: return true; //return IsValidScore(['homeH','awayH','homeF','awayF']);                        
    }
    return false;
}

function IsValidScore(arrItem) {
    var len = arrItem.length;
    if (len < 1) return true;
    for (var i = 0; i < len; i++) {
        if (!($(arrItem[i]).value).isNullOrEmpty()) return true;
    }
    $('errMsg').style.display = '';
    return false;
}

function IsValidSoccer() {
    if ($('chkFgHome') != null && $('chkFgHome').disabled == '') {
        try {
            var fg = Form.getInputs('frmBill', 'radio', 'chkFg').find(function(radio) { return radio.checked; }).value;
            var lg = Form.getInputs('frmBill', 'radio', 'chkLg').find(function(radio) { return radio.checked; }).value;
        }
        catch (ex) {
            $('errMsg').innerHTML = 'Please input First Goal / Last Goal!';
            $('errMsg').style.display = '';
            return false;
        }
    }
    return true;
}

window.onload = function() {


    if (!$('sParams')) return;
    //MatchID|SportID
    var sportID = $('sParams').value.split('|')[1];

    switch (parseInt(sportID, 10)) {
        case 1:
        case 2:
        case 3:
        case 9:
        case 32:
            if (parent.$('divMatch') && parent.$('frMatch')) {
                parent.$('divMatch').className = 'divMatch65';
                parent.$('frMatch').className = 'frMatch65';
            }
            break;
        default:
            if (parent.$('divMatch') && parent.$('frMatch')) {
                parent.$('divMatch').className = 'divMatch200';
                parent.$('frMatch').className = 'frMatch200';
            }
            break;
    }

    var IsRefund = $('IsRefund');
    if (IsRefund && IsRefund.value == '1') {

        if (sportID != 9) {
            if (parent.$('divMatch') && parent.$('frMatch')) {
                parent.$('divMatch').className = 'divMatch200';
                parent.$('frMatch').className = 'frMatch200';
            }
        }
        return;
    }

    switch (parseInt(sportID, 10)) {
        case 1: // soccer   
            if ($('homeH') != null) $('homeH').focus();
            else if ($('chkFgHome').disabled == '') $('chkFgHome').focus();
            break;
        case 2: // basketball            
        case 3: // football   
        case 32: // netball   
            $('txt1qHome').focus();
            break;
        case 4: // hockey
            $('txt1pHome').focus();
            break;
        case 5: // tennis
            $('txt1sHome').focus();
            break;
        case 10: // golf
        case 11: // Motorsports
        case 7: // billiard
        case 25: // Dart
        case 33: //     Cycling
        case 34: //     Fencing
        case 35: //     Judo
        case 36: //     Modern Pentathlon
        case 37: //     Rowing
        case 38: //     Sailing
        case 39: //     Shooting
        case 40: //     Taekwondo
        case 41: //     Triathlon
        case 42: //     Wrestling
            $('homeF').focus();
            break;
        case 8: // baseball
            $('txt1Home').focus();
            break;
        case 9: // badminton
        case 6: // volleyball
            $('txt1sHome').focus();
            break;
        default:
            $('homeH').focus();
            break;
    }
}

function ChangeTypeSoccer(calType) {
    //MatchID|SportID
    var mid = $('sParams').value.split('|')[0];
    var sid = $('sParams').value.split('|')[1];
    this.location.replace('Calculation.aspx?type=b&p=' + mid + '|' + sid + '&calType=' + calType);
}

function CheckValidFGLG() {
    if ($('chkRefundAll').checked == false && $('chkNoGoal').checked == false) {
        try {
            var fg = Form.getInputs('frmBill', 'radio', 'chkFg').find(function(radio) { return radio.checked; }).value;
            if ($('chkBillFGNG').checked == false) {
                var lg = Form.getInputs('frmBill', 'radio', 'chkLg').find(function(radio) { return radio.checked; }).value;
            }
        }
        catch (ex) {
            $('errMsg').style.display = '';
            return false;
        }
    }
    return true;
}

function FocusFGAndNoGoal(rdo) {
    $('chkNoGoal').disabled = false;
    if (rdo.id == 'chksBillFGNG') {
        $('chkLgHome').disabled = true;
        $('chkLgAway').disabled = true;
    }
    else if ($('chkNoGoal').checked == false) {
        $('chkLgHome').disabled = false;
        $('chkLgAway').disabled = false;
    }
}

function ChangeInputWay(rdo) {
    //MatchID|SportID
    var sportID = $('sParams').value.split('|')[1];
    ScoreDirect(parseInt(sportID, 10), $('scoreDirect').checked);
}

function ScoreDirect(sportID, val) {
    switch (sportID) {
        case 2: // basketball
        case 3: // football
        case 26: // rugby
        case 32: // netball
            var IsHTOnly = $('IsHTOnly').value;

            if ((IsHTOnly == '1') && val) {
                $('txt1qHome').disabled = true;
                $('txt2qHome').disabled = true;
                $('txt3qHome').disabled = true;
                $('txt4qHome').disabled = true;
                $('txtOTHome').disabled = true;
                $('txt1qAway').disabled = true;
                $('txt2qAway').disabled = true;
                $('txt3qAway').disabled = true;
                $('txt4qAway').disabled = true;
                $('txtOTAway').disabled = true;

                $('homeH').disabled = false;
                $('homeF').disabled = true;
                $('awayH').disabled = false;
                $('awayF').disabled = true;
            }
            else if ((IsHTOnly == '1') && !val) {
                $('txt1qHome').disabled = false;
                $('txt2qHome').disabled = false;
                $('txt3qHome').disabled = true;
                $('txt4qHome').disabled = true;
                $('txtOTHome').disabled = true;
                $('txt1qAway').disabled = false;
                $('txt2qAway').disabled = false;
                $('txt3qAway').disabled = true;
                $('txt4qAway').disabled = true;
                $('txtOTAway').disabled = true;

                $('homeH').disabled = true;
                $('homeF').disabled = true;
                $('awayH').disabled = true;
                $('awayF').disabled = true;
            }
            else if ((IsHTOnly == '0') && !val) {
                $('txt1qHome').disabled = false;
                $('txt2qHome').disabled = false;
                $('txt3qHome').disabled = false;
                $('txt4qHome').disabled = false;
                $('txtOTHome').disabled = false;
                $('txt1qAway').disabled = false;
                $('txt2qAway').disabled = false;
                $('txt3qAway').disabled = false;
                $('txt4qAway').disabled = false;
                $('txtOTAway').disabled = false;

                $('homeH').disabled = true;
                $('homeF').disabled = true;
                $('awayH').disabled = true;
                $('awayF').disabled = true;
            }
            else if ((IsHTOnly == '0') && val) {
                $('txt1qHome').disabled = true;
                $('txt2qHome').disabled = true;
                $('txt3qHome').disabled = true;
                $('txt4qHome').disabled = true;
                $('txtOTHome').disabled = true;
                $('txt1qAway').disabled = true;
                $('txt2qAway').disabled = true;
                $('txt3qAway').disabled = true;
                $('txt4qAway').disabled = true;
                $('txtOTAway').disabled = true;

                $('homeH').disabled = false;
                $('homeF').disabled = false;
                $('awayH').disabled = false;
                $('awayF').disabled = false;
            }


            $('txt1qHome').value = '';
            $('txt2qHome').value = '';
            $('txt3qHome').value = '';
            $('txt4qHome').value = '';
            $('txtOTHome').value = '';
            $('txt1qAway').value = '';
            $('txt2qAway').value = '';
            $('txt3qAway').value = '';
            $('txt4qAway').value = '';
            $('txtOTAway').value = '';
            $('homeH').value = '';
            $('homeF').value = '';
            $('awayH').value = '';
            $('awayF').value = '';
            break;
        case 4: // ice hockey
        case 28: // field hockey 
            $('txt1pHome').disabled = val;
            $('txt2pHome').disabled = val;
            $('txt3pHome').disabled = val;
            $('txtOTHome').disabled = val;
            $('txt1pAway').disabled = val;
            $('txt2pAway').disabled = val;
            $('txt3pAway').disabled = val;
            $('txtOTAway').disabled = val;
            $('homeF').disabled = !val;
            $('awayF').disabled = !val;

            $('txt1pHome').value = '';
            $('txt2pHome').value = '';
            $('txt3pHome').value = '';
            $('txtOTHome').value = '';
            $('txt1pAway').value = '';
            $('txt2pAway').value = '';
            $('txt3pAway').value = '';
            $('txtOTAway').value = '';
            $('homeF').value = '';
            $('awayF').value = '';
            break;
        case 5: // tennis 
            $('txt1sHome').disabled = val;
            $('txt2sHome').disabled = val;
            $('txt3sHome').disabled = val;
            $('txt4sHome').disabled = val;
            $('txt5sHome').disabled = val;
            $('txt1sAway').disabled = val;
            $('txt2sAway').disabled = val;
            $('txt3sAway').disabled = val;
            $('txt4sAway').disabled = val;
            $('txt5sAway').disabled = val;

            $('txtFSHome').disabled = !val;
            $('txtFSAway').disabled = !val;

            $('txt1sHome').value = '';
            $('txt2sHome').value = '';
            $('txt3sHome').value = '';
            $('txt4sHome').value = '';
            $('txt5sHome').value = '';
            $('txt1sAway').value = '';
            $('txt2sAway').value = '';
            $('txt3sAway').value = '';
            $('txt4sAway').value = '';
            $('txt5sAway').value = '';
            $('txtTGHome').value = '';
            $('txtFSHome').value = '';
            $('txtTGAway').value = '';
            $('txtFSAway').value = '';
            break;
        case 8: // baseball
            $('txt1Home').disabled = val;
            $('txt2Home').disabled = val;
            $('txt3Home').disabled = val;
            $('txt4Home').disabled = val;
            $('txt5Home').disabled = val;
            $('txt6Home').disabled = val;
            $('txt7Home').disabled = val;
            $('txt8Home').disabled = val;
            $('txt9Home').disabled = val;
            $('txtEIHome').disabled = val;
            $('txt1Away').disabled = val;
            $('txt2Away').disabled = val;
            $('txt3Away').disabled = val;
            $('txt4Away').disabled = val;
            $('txt5Away').disabled = val;
            $('txt6Away').disabled = val;
            $('txt7Away').disabled = val;
            $('txt8Away').disabled = val;
            $('txt9Away').disabled = val;
            $('txtEIAway').disabled = val;
            $('homeH').disabled = !val;
            $('awayH').disabled = !val;
            $('homeF').disabled = !val;           
            $('awayF').disabled = !val;

            $('txt1Home').value = '';
            $('txt2Home').value = '';
            $('txt3Home').value = '';
            $('txt4Home').value = '';
            $('txt5Home').value = '';
            $('txt6Home').value = '';
            $('txt7Home').value = '';
            $('txt8Home').value = '';
            $('txt9Home').value = '';
            $('txtEIHome').value = '';
            $('txt1Away').value = '';
            $('txt2Away').value = '';
            $('txt3Away').value = '';
            $('txt4Away').value = '';
            $('txt5Away').value = '';
            $('txt6Away').value = '';
            $('txt7Away').value = '';
            $('txt8Away').value = '';
            $('txt9Away').value = '';
            $('txtEIAway').value = '';
            $('homeH').value = '';
            $('awayH').value = '';
            $('homeF').value = '';            
            $('awayF').value = '';
            break;
        case 9: // badminton
        case 6: // volleyball
        case 30: //Squash
            $('txt1sHome').disabled = val;
            $('txt2sHome').disabled = val;
            $('txt3sHome').disabled = val;
            $('txt4sHome').disabled = val;
            $('txt5sHome').disabled = val;
            $('txt1sAway').disabled = val;
            $('txt2sAway').disabled = val;
            $('txt3sAway').disabled = val;
            $('txt4sAway').disabled = val;
            $('txt5sAway').disabled = val;

            $('txtTSHome').disabled = !val;
            $('txtTSAway').disabled = !val;

            $('txt1sHome').value = '';
            $('txt2sHome').value = '';
            $('txt3sHome').value = '';
            $('txt4sHome').value = '';
            $('txt5sHome').value = '';
            $('txtTPHome').value = '';
            $('txt1sAway').value = '';
            $('txt2sAway').value = '';
            $('txt3sAway').value = '';
            $('txt4sAway').value = '';
            $('txt5sAway').value = '';
            $('txtTPAway').value = '';

            $('txtTSHome').value = '';
            $('txtTSAway').value = '';

            break;
        default: break;
    }
    isInputScoreDirectly = val;
    $('isSD').value = (isInputScoreDirectly) ? "1" : "0";
}

function InputGoalRecord(e) {
    var code = '';
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;

    //alert(code);
    //A:65, 97| h:72, 104| back:8| move left:37| move right:39| delete:46
    if (code == 65 || code == 97 || code == 72 || code == 104 || code == 8 || code == 35 || code == 36 || code == 37 || code == 39 || code == 46 || code == 9)
        return true;
    else
        return false;
}

function RePlaceStr(e) {
    $('GR').value = ReplaceAll($('GR').value, '\'', '');

    var code = '';
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;

    if (code == 9 || code == 13) {
        DisplayGoalHistory();
    }
    if ($('sHasFGLG').value == '1') {
        if ($('GR').value == "") {
            $('hfgoal').style.display = 'none';
            $('hlgoal').style.display = 'none';
            $('afgoal').style.display = 'none';
            $('algoal').style.display = 'none';
        }
        else {
            if ($('GR').value.substring(0, 1) == "h" || $('GR').value.substring(0, 1) == "H") {
                $('hfgoal').style.display = "inline";
                $('afgoal').style.display = "none";
            }
            else {
                $('hfgoal').style.display = "none";
                $('afgoal').style.display = "inline";
            }
            if ($('GR').value.substring($('GR').value.length - 1, $('GR').value.length) == "h" || $('GR').value.substring($('GR').value.length - 1, $('GR').value.length) == "H") {
                $('hlgoal').style.display = "inline";
                $('algoal').style.display = "none";
            }
            else {
                $('hlgoal').style.display = "none";
                $('algoal').style.display = "inline";
            }

        }
    }


}

function DisplayGoalHistory() {
    $('errMsg').style.display = 'none';
    var goalrecord = $('GR').value;
    var param = '?mode=1&goalrecord=' + goalrecord;
    var myajax = new Ajax.Request("ProcessData.ashx", {
        asynchronous: true,
        method: 'post',
        parameters: param,
        onComplete: function(rs) {
            var str = rs.responseText;
            $('spGoalHistory').innerHTML = str;
        }
    });
}

function MoveBetdaqTickets(matchid, matchcode) {
    height=getScrollHeight()+ 50;
    parent.ibcWnd.open('SelectMatchMoveBet.aspx?matchid=' + matchid + '&matchcode=' + matchcode, 'Move Betdaq Tickets', 350, height, 270, 170, 'class1', 'class2');

}
