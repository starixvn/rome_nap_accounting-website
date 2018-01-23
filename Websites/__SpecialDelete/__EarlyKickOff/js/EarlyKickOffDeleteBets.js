
var msg = new Message();
var p = new ParamObj();

function ParamObj() {
    this.MatchId = '';
    this.FromRefNo = '';
    this.ToRefNo = '';
    this.Hdp = '';
    this.Liveindicator = '';
    this.Bettypes = '';
    this.Betteam1378 = '';
    this.Betteam515 = '';
    this.Betteam4 = '';
    this.KichOffDate = '';
}

function Message() {
    this.Content_en = '';
    this.Content_ch = '';
    this.Content_cs = '';
    this.Content_it = '';
    this.Content_jp = '';
    this.Content_th = '';
    this.Content_es = '';
    this.UserId = 0;
    this.UserName = '';
    this.userPwd = '';
    this.CreatorName = '';
    this.CreatorId = 0;
}

function SetupCalendar(strdatetime) {

    $('#txtdate').datetimepicker({
        showSecond: true,
        timeFormat: 'hh:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond:1,
        hourGrid: 3,
        minuteGrid: 5,
        secondGrid: 5
        
    });
    if (strdatetime != null && strdatetime != "") {
        $('#txtdate').val(strdatetime).datetimepicker('setDate', (new Date(strdatetime)));
    }

}

function on_delete() {
    $('#ErrorMessage').html('');
    $('#trMsg2').hide();
    if ($('#username').val() == "") {
        $('#trMsg2').show();
        showAlertMessage("ErrorMessage", "Please enter username", 1);
        //$('#ErrorMessage').html("*Please enter username");
        $('#username').focus();
        return false;
    }
    else if ($('#userpwd').val() == "") {
        $('#trMsg2').show();
        //$('#ErrorMessage').html("*Please enter password");
        showAlertMessage("ErrorMessage", "Please enter password", 1);
        $('#userpwd').focus();
        return false;
    }
    else if ($('#men').val() == "") {
        $('#trMsg2').show();
        //$('#ErrorMessage').html("*You must enter English message");
        showAlertMessage("ErrorMessage", "You must enter English message", 1);
        $('#men').focus();
        return false;
    }
    else if ($('#men').val().indexOf('@@') == -1) {
        $('#trMsg2').show();
        //$('#ErrorMessage').html("*You must include @@ in  English message for replacing ticket number");
        showAlertMessage("ErrorMessage", "You must include @@ in  English message for replacing ticket number", 1);
        $('#men').focus();
        return false;
    }
    else if ($('#mch').val() == "") {
        $('#trMsg2').show();
        //$('#ErrorMessage').html("*You must enter Chinese message");
        showAlertMessage("ErrorMessage", "You must enter Chinese message", 1);
        $('#mch').focus();
        return false;
    }
    else if ($('#mch').val().indexOf('@@') == -1) {
        $('#trMsg2').show();
        //$('#ErrorMessage').html("*You must include @@ in Chinese message for replacing ticket number");
        showAlertMessage("ErrorMessage", "You must include @@ in Chinese message for replacing ticket number", 1);
        $('#mch').focus();
        return false;
    }
    else if (($('#mcs').val() != "") && ($('#mcs').val().indexOf('@@') == -1)) {
        $('#trMsg2').show();
        //$('#ErrorMessage').html("*You must include @@ in Simple Chinese message for replacing ticket number");
        showAlertMessage("ErrorMessage", "You must include @@ in Simple Chinese message for replacing ticket number", 1);
        $('#mcs').focus();
        return false;
    }
    else if (($('#mth').val() != "") && ($('#mth').val().indexOf('@@') == -1)) {
        $('#trMsg2').show();
        //$('#ErrorMessage').html("*You must include @@ in Thai message for replacing ticket number");
        showAlertMessage("ErrorMessage", "You must include @@ in Thai message for replacing ticket number", 1);
        $('#mcs').focus();
        $('#mth').focus();
        return false;
    }
    else if (($('#mit').val() != "") && ($('#mit').val().indexOf('@@') == -1)) {
        $('#trMsg2').show();
        //$('#ErrorMessage').html("*You must include @@ in Italian message for replacing ticket number");
        showAlertMessage("ErrorMessage", "You must include @@ in Italian message for replacing ticket number", 1);
        $('#mit').focus();
        return false;
    }
    else if (($('#mjp').val() != "") && ($('#mjp').val().indexOf('@@') == -1)) {
        $('#trMsg2').show();
        //$('#ErrorMessage').html("*You must include @@ in Japanese message for replacing ticket number");
        showAlertMessage("ErrorMessage", "You must include @@ in Japanese message for replacing ticket number", 1);
        $('#mjp').focus();
        return false;
    }
    else if (($('#mes').val() != "") && ($('#mes').val().indexOf('@@') == -1)) {
        $('#trMsg2').show();
        //$('#ErrorMessage').html("*You must include @@ in Spanish message for replacing ticket number");
        showAlertMessage("ErrorMessage", "You must include @@ in Spanish message for replacing ticket number", 1);
        $('#mes').focus();
        return false;
    }

    try {
        var transids = [];
        
        // for normal && normal mix parlay
        var bet_normals = [];
        bet_normals = $.merge($('input.chkNormal-1'), $('input.chkNormalMixParlay-1'));
        //alert(bet_normals.length);
        if (bet_normals && bet_normals.length > 0) {
            for (ind = 0; ind < bet_normals.length; ind++) {
                var elm = bet_normals[ind];
                if (elm && $(elm).val() == '1') {
                    transids.push($(elm).attr('id').toString());
                }
            }
        }
        // for deposit && deposit mix parlay
        var DelOption = '0';
        if ($('#ddlDeposit').length > 0 || $('#ddlDepositMixParlay').length > 0) {
            DelOption = $('#ddlDeposit').length > 0 ? $('#ddlDeposit').val() : '0';
            //DelOption += '^' + ($('#ddlDepositMixParlay').length > 0 ? $('#ddlDepositMixParlay').val() : '0');
            var bet_deposits = [];
            bet_deposits = $.merge($('input.chkDeposit-1'), $('input.chkDepositMixParlay-1'));
            if (bet_deposits && bet_deposits.length > 0) {
                $(bet_deposits).each(function(ind, elm) {
                    if ($(elm).attr('checked')) {
                        transids.push($(elm).attr('id'));
                    }
                });
            }
        }
        // for BetDaq && BetDaq mix parlay
        var DelOption_BD = '0';
        var transids_BD = [];
        if ($('#ddlBetDaq').length > 0 || $('#ddlBetDaqMixParlay').length > 0) {
            DelOption_BD = $('#ddlBetDaq').length > 0 ? $('#ddlBetDaq').val() : '0';
            //DelOption_BD += '^' + ($('#ddlBetDaqMixParlay').length > 0 ? $('#ddlBetDaqMixParlay').val() : '0');
            var bet_betdaqs = [];
            bet_betdaqs = $.merge($('input.chkBetDaq-1'), $('input.chkBetDaqMixParlay-1'));
            //alert(bet_betdaqs.length);
            if (bet_betdaqs && bet_betdaqs.length > 0) {
                $(bet_betdaqs).each(function(ind, elm) {
                    if ($(elm).attr('checked')) {
                        transids_BD.push($(elm).attr('id'));
                    }
                });
            }
        }

        //For 12EU
        var DelOption_12EU = '0';
        var transids_12EU = [];
        if ($('#ddl12EU').length > 0 || $('#ddl12EUMixParlay').length > 0) {
            DelOption_12EU = $('#ddl12EU').length > 0 ? $('#ddl12EU').val() : '0';
            //DelOption_12EU += '^' + ($('#ddl12EUMixParlay').length > 0 ? $('#ddl12EUMixParlay').val() : '0');
            var bet_12eus = [];
            bet_12eus = $.merge($('input.chk12EU-1'), $('input.chk12EUMixParlay-1'));
            //alert(bet_12eus.length);
            if (bet_12eus && bet_12eus.length > 0) {
                $(bet_12eus).each(function(ind, elm) {
                    if ($(elm).attr('checked')) {
                        transids_12EU.push($(elm).attr('id'));
                    }
                });
            }
        }

        //For MHC Master
        var DelOption_MHC = '0';
        var transids_MHC = [];
        if ($('#ddlMasterMHC').length > 0 || $('#ddlMasterMHCMixParlay').length > 0) {
            DelOption_MHC = $('#ddlMasterMHC').length > 0? $('#ddlMasterMHC').val() : '0';
            //DelOption_MHC += '^' + ($('#ddlMasterMHCMixParlay').length > 0 ? $('#ddlMasterMHCMixParlay').val() : '0');
            var bet_mhcs = [];
            bet_mhcs = $.merge($('input.chkMHC-1'), $('input.chkMHCMixParlay-1'));
            //alert(bet_mhcs.length);
            if (bet_mhcs && bet_mhcs.length > 0) {
                $(bet_mhcs).each(function(ind, elm) {
                    if ($(elm).attr('checked')) {
                        transids_MHC.push($(elm).attr('id'));
                    }
                });
            }
        }
    } catch (e) {
      
    }
    // set message content
    msg.Content_en = $('#men').val();
    msg.Content_ch = $('#mch').val();
    msg.Content_cs = $('#mcs').val();
    msg.Content_it = $('#mit').val();
    msg.Content_jp = $('#mjp').val();
    msg.Content_th = $('#mth').val();
    msg.Content_es = $('#mes').val();
    msg.UserName = $('#username').val();
    msg.UserId = $('#accId').val();
    msg.userPwd = $('#userpwd').val();
    msg.CreatorId = $('#userId').val();
    msg.CreatorName = $('#username').val();
    
    var bettype = $('#bettype').val();

    if ($('#hdbettype').val() == '10') {
        $('#matchid').val($('#ddlOutright').val());
        bettype = '10';
    }

    
    var betteam4 = '';
    if (typeof (specialdelete) !== "undefined")
        betteam4=specialdelete.betteam4 || '';

    var data =
    {
        'matchId': $('#matchid').val(),
        'bettype': bettype,
        'betteam1378': $('#betteam1378').val(),
        'betteam515': $('#betteam515').val(),
        'hdp': ($('#hdp').val() == '' ? '9999' : $('#hdp').val()),
        'uname': $('#username').val(),
        'userId': $('#userId').val(),
        'pass': $('#userpwd').val(),
        'DelOption': DelOption,
        'transids': transids.join(','),
        //'DelOption': '2',
        //'transids': '246647719,246654687',
        'kickoffdate': $.trim($('#txtdate').val()),
        'DelOption_BD': DelOption_BD,
        'transids_BD': transids_BD.join(','),
        'DelOption_12EU': DelOption_12EU,
        'transids_12EU': transids_12EU.join(','),
        'DelOption_MHC': DelOption_MHC,
        'transids_MHC': transids_MHC.join(','),
        'betteam4': betteam4,
        'message': msg
    };

    //alert(JSON.stringify(data));
    //return;

    $.ajax({
        type: "POST",
        url: "../Services/SpecialDeleteService.svc/DeleteSpecialEarlyKO",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        beforeSend: function() {
            try { parent.BlockUI(); } catch (e) { }
        },
        success: function(rs) {
            parent.jQuery.unblockUI();
            $('#ErrMsg').html(rs.ErrorMessage);
            if (rs.ErrorCode == 0) {
                specialdelete.validAction('complete');
            }
        }
    });

    return false;
}



function ddl_change(type) {
    var ddl = 'ddl';
    ddl = ddl + type;
    
    var checkbox = 'chk';
    checkbox = checkbox + type + '-1';
    
    var DelOption = $('#' + ddl).val();
    var chkAlls = $('input.' + checkbox);
    if (!chkAlls || chkAlls.length==0) return;
    switch (DelOption) {
        case '0': //Not Delete
            $(chkAlls).each(function(i, elm) {
                $(elm).attr('disabled', 'disabled');
                $(elm).attr('checked', false);
            });
            break;
        case '1': //Delete All
            $(chkAlls).each(function(i, elm) {
                if ($(elm).val() == 1) {
                    $(elm).attr('disabled', 'disabled');
                    $(elm).attr('checked', true);
                }
                else {
                    $(elm).attr('disabled', 'disabled');
                    $(elm).attr('checked', false);
                }
            });
            break;
        case '2': //Option
            $(chkAlls).each(function(i, elm) {
                if ($(elm).val() == 1) {
                    $(elm).removeAttr('disabled');
                    $(elm).attr('checked', false);
                } else {
                    $(elm).attr('disabled', 'disabled');
                    $(elm).attr('checked', false);
                }
            });
            break;

    }
}
/*
function on_deposit() {
    if (!obj('ddlDeposit')) return;
    var DelOption = obj('ddlDeposit').value;

    DelOption = '1';
    obj('ddlDeposit').value = '1';

    var chkAlls = document.getElementsByName('chkDeposit');
    if (!chkAlls) return;
    switch (DelOption) {
        case '1': //CheckAll
            for (i = 0; i < chkAlls.length; i++) {
                var id = chkAlls[i].id;
                var IsDelete = id.split('-')[2];
                if (IsDelete == '0') {
                    chkAlls[i].checked = false;
                    chkAlls[i].disabled = true;
                }
                else {
                    chkAlls[i].checked = true;
                    chkAlls[i].disabled = true;
                }
            }
            break;

    }
}*/

var timeoutID = 0;
function hideLanguage(obj) {
    if (String(obj.innerHTML).indexOf("-") != -1) {
        //current opened
        $("#otherLanguage_title").hide();
        $("#otherLanguage_content").hide();
        $("#otherLanguage_title2").hide();
        $("#otherLanguage_content2").hide();
        obj.innerHTML = "<span style=\"color: Black;\">Other Language</span> [+]";
    }
    else {
        //current closed
        $("#otherLanguage_title").show();
        $("#otherLanguage_content").show();
        $("#otherLanguage_title2").show();
        $("#otherLanguage_content2").show();
        obj.innerHTML = "<span style=\"color: Black;\">Other Language</span> [-]";
    }

    window.clearTimeout(timeoutID);
    timeoutID = setTimeout('gotoBottom()', 1);
}

function gotoBottom() {
    window.location = '#men';
}

function on_submit() {

    $('#spErrCS').html('');
    $('#dvContent').html('');

    var chks = document.getElementsByName('chkScore');
    var scores = '';
    for (i = 0; i < chks.length; i++) {
        if (chks[i].checked) {
            scores += chks[i].value + '^';
        }
    }

    if ($('#chk4').is(':checked') && scores.length == 0) {        
         showAlertMessage("spErrCS", "Please select a score!", 1);
        return false;
    }

    if ($('#chkAllScores').is(':checked'))
        scores = 'All';


    //--------------

    if (!isNumberNotNull('matchid')) {
        showAlertMessage("ErrMsg", "Please enter a valid Match Id (number)!", 1);
        $('#matchid').focus();
        return false;
    }
    
    if ($('#hdp').val().length > 0) {
        if (isNaN($('#hdp').val())) {
            showAlertMessage("ErrMsg", "Please enter a valid To HDP Number!", 1);
            $('#hdp').focus();
            return false;
        }
    }
    var chkBettype = document.getElementsByName('chkBettype');
    var strBettype = '', strBettypeName = '';
    for (i = 0; i < chkBettype.length; i++) {
        if (chkBettype[i].checked) {
            strBettype += chkBettype[i].value.split('^')[0] + '^';
            strBettypeName += chkBettype[i].value.split('^')[1] + '^';
        }
    }
    if (strBettype.length == 0) {
        showAlertMessage("ErrMsg", "Please select a bet type!", 1);
        return false;
    }
    $('#bettype').val(strBettype);
    
    p.MatchId = $('#matchid').val();
    p.FromRefNo = '0';
    p.ToRefNo = '999999999999';
    p.Hdp = $('#hdp').val().length == 0 ? '9999' : $('#hdp').val();
    p.Liveindicator = '0';
    p.Bettypes = strBettype;
    p.Betteam1378 = $('#ddlBetTeam1378').val();
    p.Betteam515 = $('#ddlBetTeam515').val();
    p.Betteam4 = scores;
    p.KichOffDate = $('#txtdate').val();

    $('#ErrMsg').empty();
    specialdelete.renderSpecialDeleteGrid(p);

    return true;
}

function on_changeBetype(obj) {
    $('#ErrMsg').html('');
    if (!obj) return;
    var Is1378 = false; //Handicap, 1st Handicap , Over/Under  and 1st Over/Under 
    var Is515 = false; //1X2 and FT.1X2
    var Is4 = false; //Correct Score
    var Is212 = false; //Odd/Even, 1st Odd/Even
    var chkBettype = document.getElementsByName('chkBettype');
    var chkBetteamCS = document.getElementsByName('chkScore');
    var strBettype = '';
    var CountChecked = 0;
    $('#ddlBetTeam1378').attr('disabled', true);
    $('#hdp').attr('disabled', true);
    $('#ddlLiveIndicator').attr('disabled', true);
    $('#ddlBetTeam515').attr('disabled', true);

    Is4 = $('#chk4').is(':checked');
    var IsCSAnyChecked = false;
    var score = document.getElementsByName('chkScore');
    for (i = 0; i < score.length; i++) {
        if (score[i].checked) { IsCSAnyChecked = true; break; }
    }
    if (!IsCSAnyChecked)
        IsEnableCS(true, false);

    $('#chkAllScores').attr('disabled', true);

    for (i = 0; i < chkBettype.length; i++) {
        if (chkBettype[i].checked) {
            strBettype += chkBettype[i].value.split('^')[0] + '^';
            switch (chkBettype[i].value.split('^')[0]) {
                case "1":
                case "3":
                case "7":
                case "8":
                    Is1378 = true;
                    break;
                case "5":
                case "15":
                    Is515 = true;
                    break;
                case "4":
                    Is4 = true;
                    break;
                case "2":
                case "12":
                    Is212 = true;
                    break;

            }
        }
    }

    for (i = 0; i < chkBettype.length; i++) {
        if (chkBettype[i].checked) CountChecked++;
    }

    $('#chkAllBettype').attr('checked', (CountChecked == chkBettype.length));
    if (Is1378) {
        $('#ddlBetTeam1378').attr('disabled', false);
        $('#hdp').attr('disabled', false);
    }
    if (Is212) {
        $('#ddlBetTeam1378').attr('disabled', false);
    }
    if (Is515) {
        $('#ddlBetTeam515').attr('disabled', false);
    }

    if (Is1378 || Is515 || Is212) {
        $('#ddlLiveIndicator').attr('disabled', false);
    }

    if (Is4) {

        $('#chkAllScores').attr('disabled', false);
        $('#chkAllScores').attr('checked', true);
        if (!IsCSAnyChecked)
            IsEnableCS($('#chkAllScores').is(':checked'), true);

        CountChecked = 0;
        for (i = 0; i < chkBetteamCS.length; i++) {
            if (chkBetteamCS[i].checked) CountChecked++;
        }
        $('#chkAllScores').attr('checked', (CountChecked == chkBetteamCS.length));
    }
    else {
        $('#chkAllScores').attr('checked', false);
        IsEnableCS(false, false);
    }
}

function CheckAllBettype(obj) {
    var chkBettype = document.getElementsByName('chkBettype');
    for (i = 0; i < chkBettype.length; i++) {
        chkBettype[i].checked = obj.checked;
    }
    on_changeBetype(1);
}




function IsEnableCS(IsCheck, IsEnable) {

    $('#spErrCS').html('');
    var chks = document.getElementsByName('chkScore');
    var strDisabled = ((IsEnable) ? '' : 'disabled');
    for (i = 0; i < chks.length; i++) {
        chks[i].checked = IsCheck;
        chks[i].disabled = strDisabled;
    }
}

function IsCheckAllScore() {
    $('#spErrCS').html('');
    IsEnableCS($('#chkAllScores').is(':checked'), true);
}

function CheckOnScore() {
    $('#spErrCS').html('');
    var chks = document.getElementsByName('chkScore');
    var cnt = 0;
    for (i = 0; i < chks.length; i++) {
        if (chks[i].checked)
            cnt++;
    }

    $('#chkAllScores').attr('checked', (cnt == chks.length));
}


function InitFixedHeader(widthmainpage) {


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

    var RealHeight = $(window).height() - 100;
    //alert('topHeader: ' + topHeader + '\ntopFooter: ' + topFooter);
    // alert(RealHeight);

    if ((topFooter - topHeader) > $(window).height()) {
        if (jQuery('#griddelete').height() > RealHeight) {
            jQuery('#griddelete').Scrollable(RealHeight, widthmainpage);
        }
    }

}
