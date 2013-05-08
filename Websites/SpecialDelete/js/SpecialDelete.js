
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

    //For Deposit 

    var DelOption = 0;
    var transids = '';
    if ($('#ddlDeposit').length > 0) {
        DelOption = $('#ddlDeposit').val();
        var chkAlls = document.getElementsByName('chkDeposit');
        
        for (i = 0; i < chkAlls.length; i++) {
            if (chkAlls[i].checked) {
                transids += chkAlls[i].id.split('-')[0] + ',';

            }
        }
        transids = transids.substring(0, transids.length - 1);
    }

    //For BD
    var DelOption_BD = 0;
    var transids_BD = '';
    if ($('#ddlBD').length > 0) {
        DelOption_BD = $('#ddlBD').val();
        var chkAlls_BD = document.getElementsByName('chkBD');
        
        for (i = 0; i < chkAlls_BD.length; i++) {
            if (chkAlls_BD[i].checked) {
                transids_BD += chkAlls_BD[i].id.split('-')[0] + ',';
            }
        }
        transids_BD = transids_BD.substring(0, transids_BD.length - 1);
    }

    //For 12EU
    var DelOption_12EU = 0;
    var transids_12EU = '';
    if ($('#ddl12EU').length > 0) {
        DelOption_12EU = $('#ddl12EU').val();
        var chkAlls_12EU = document.getElementsByName('chk12EU');
        
        for (i = 0; i < chkAlls_12EU.length; i++) {
            if (chkAlls_12EU[i].checked) {
                transids_12EU += chkAlls_12EU[i].id.split('-')[0] + ',';
            }
        }
        transids_12EU = transids_12EU.substring(0, transids_12EU.length - 1);
    }

    //For MHC Master
    var DelOption_MHC = 0
    var transids_MHC = '';
    if ($('#ddlMasterMHC').length > 0) {
        DelOption_MHC = $('#ddlMasterMHC').val();
        var chkAlls_MHC = document.getElementsByName('chkMasterMHC');
        
        for (i = 0; i < chkAlls_MHC.length; i++) {
            if (chkAlls_MHC[i].checked) {
                transids_MHC += chkAlls_MHC[i].id.split('-')[0] + ',';
            }
        }
        transids_MHC = transids_MHC.substring(0, transids_MHC.length - 1);
    }

    try{parent.BlockUI();}catch(e){}


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
        'frefno': $('#frefno').val(),
        'trefno': $('#trefno').val(),
        'bettype': bettype,
        'betteam1378': $('#betteam1378').val(),
        'betteam515': $('#betteam515').val(),
        'hdp': ($('#hdp').val() == '' ? '9999' : $('#hdp').val()),
        'uname': $('#username').val(),
        'userId': $('#userId').val(),
        'pass': $('#userpwd').val(),
        'DelOption': DelOption,
        'transids': transids,
        'liveindicator': $('#ddlLiveIndicator').val(),
        'DelOption_BD': DelOption_BD,
        'transids_BD': transids_BD,
        'DelOption_12EU': DelOption_12EU,
        'transids_12EU': transids_12EU,
        'DelOption_MHC': DelOption_MHC,
        'transids_MHC': transids_MHC,
        'betteam4': betteam4,
        'message': msg
    };



    $.ajax({
        type: "POST",
        url: "Services/SpecialDeleteService.svc/DeleteSpecialTransaction",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {

            if (rs.ErrorCode == 0) {
                $('#dvHeader').html(rs);
                $('#mainreport').html('');
                $('.bg_table').hide();

            }

            $('#ErrMsg').html(rs.ErrorMessage);
            parent.jQuery.unblockUI();
            return false;
        }
    });

    return true;
}



function ddl_change(type) {
    var ddl = 'ddl';
    ddl = ddl + type;

    var checkbox = 'chk';
    checkbox = checkbox + type;

    if (!obj(ddl)) return;
    var DelOption = obj(ddl).value;
    var chkAlls = document.getElementsByName(checkbox);
    if (!chkAlls) return;
    switch (DelOption) {
        case '0': //Not Delete
            for (i = 0; i < chkAlls.length; i++) {
                chkAlls[i].disabled = true;
                chkAlls[i].checked = false;
            }
            break;
        case '1': //Delete All

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
        case '2': //Option
            for (i = 0; i < chkAlls.length; i++) {

                var id = chkAlls[i].id;
                var IsDelete = id.split('-')[2];
                if (IsDelete == '0') {
                    chkAlls[i].disabled = true;
                    chkAlls[i].checked = false;
                }
                else {
                    chkAlls[i].disabled = false;
                    chkAlls[i].checked = false;
                }
            }
            break;

    }
}

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
}

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
    if (!isNumberNotNull('frefno')) {
        showAlertMessage("ErrMsg", "Please enter a valid From Ref Number!", 1);
        $('#frefno').focus();
        return false;
    }
    if (!isNumberNotNull('trefno')) {
        showAlertMessage("ErrMsg", "Please enter a valid To Ref Number!", 1);
        $('#trefno').focus();
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
    
    try{parent.BlockUI();}catch(e){}
    var from = $('#frmSpecialDelete').serialize(); 


    p.MatchId = $('#matchid').val();
    p.FromRefNo = $('#frefno').val();
    p.ToRefNo = $('#trefno').val();
    p.Hdp = $('#hdp').val().length == 0 ? '9999' : $('#hdp').val();
    p.Liveindicator = $('#ddlLiveIndicator').val() == 'All' ? '2' : $('#ddlLiveIndicator').val();
    p.Bettypes = strBettype;
    p.Betteam1378 = $('#ddlBetTeam1378').val();
    p.Betteam515 = $('#ddlBetTeam515').val();
    p.Betteam4 = scores;

    ParamWhenSelData = p;
    
    $.ajax({
        type: 'POST',
        url: 'GetData.ashx',
        async: true,
        data: from,
        success: function (rs) {
            if (rs.indexOf('alertboxerr') >= 0) {
                $('#ErrMsg').html(rs);
                $("#mainreport").addClass("hidden");
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            else {
                $('#ErrMsg').empty();
                $('#dvContent').html(rs);
                specialdelete.renderSpecialDeleteGrid(p, true);
            }
            return true;
        }
    });

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

//----------------------- for outright-------------------
function GetData_OR() {
    if (!isNumberNotNull('frefno')) {
        showAlertMessage("ErrMsg", "Please enter a valid From Ref Number!", 1);
        $('#frefno').focus();
        return false;
    }
    if (!isNumberNotNull('trefno')) {
        showAlertMessage("ErrMsg", "Please enter a valid From Ref Number!", 1);
        $('#trefno').focus();
        return false;
    }

    $('#ErrMsg').html('');

    try{parent.BlockUI();}catch(e){}

    $('#dvContent').html('');
    var param = 'mode=2&OutrightID=' + $('#ddlOutright').val();
    param += '&frefno=' + $('#frefno').val();
    param += '&trefno=' + $('#trefno').val();

    $.ajax({
        type: 'POST', //.Request("GetDataOutright.ashx", {
        url: 'GetDataOutright.ashx',
        async: true,
        data: param,
        success: function(rs) {
            $('#dvContent').html(rs);
            on_deposit();
            parent.jQuery.unblockUI();
        }
    });

    return true;
}


function ChangeLeague(league) {
    try{parent.BlockUI();}catch(e){}
    var param = 'mode=1&LeagueID=' + league.value;
    $.ajax({//Request("GetDataOutright.ashx", {
        type: 'POST',
        url: 'GetDataOutright.ashx',
        async: true,
        data: param,
        success: function(rs) {
            $('#sOutright').html(rs);
            parent.jQuery.unblockUI();
        }
    });

}

//-----------------------END for outright-------------------


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
