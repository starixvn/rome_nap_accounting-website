function CheckDecNumber(e) {
    var code = "";
    if (e.which) {
        code = e.which;
    }
    else if (e.keyCode) {
        code = e.keyCode;
    }

    if (code == 37 || code == 39 || code == 36 || code == 35 || code == 118 || code == 46) { //e or E or "-"
        return true;
    }
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 9) {
        return false;
    }
    return true;
}

function ParamObj() {
    this.MatchId = '';
    this.FromRefNo = '';
    this.ToRefNo = '';
    this.Hdp = '';
    this.LiveIndicator = '';
    this.Bettypes = '';
    this.Betteam1378 = '';
    this.Betteam515 = '';
    this.Betteam4 = '';
    this.Betteam430 = '';
    this.KichOffDate = '';
    this.Betteam6 = '';
    this.Betteam14 = '';
    this.Betteam16 = '';
    this.Type = '';
    this.HomeScore = '';
    this.AwayScore = '';
    this.FromTransDate = '';
    this.ToTransDate = '';
    this.SelectBettype = 9; //0: all, 9: Mix Parlay, 29: System Parlay
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
var msg = new Message();
var par = new ParamObj();
function SetupCalendar(strdatetime, txtdate) {
    $(txtdate).datetimepicker({
        showSecond: true,
        timeFormat: 'hh:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        hourGrid: 3,
        minuteGrid: 5,
        secondGrid: 5

    });

    if (strdatetime != null && strdatetime != "") {
        $(txtdate).val(strdatetime).datetimepicker('setDate', (new Date(strdatetime)));
    }
}

function ddl_change(type) {
    var ddl = 'ddl';
    ddl = ddl + type;

    var checkbox = 'chk';
    checkbox = checkbox + type + '-1';

    var DelOption = $('#' + ddl).val();
    var chkAlls = $('input.' + checkbox);
    if (!chkAlls || chkAlls.length == 0) return;
    switch (DelOption) {
        case '0': //Not Delete
            $(chkAlls).each(function (i, elm) {
                $(elm).attr('disabled', 'disabled');
                $(elm).attr('checked', false);
            });
            break;
        case '1': //Delete All
            $(chkAlls).each(function (i, elm) {
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
            $(chkAlls).each(function (i, elm) {
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
var timeoutID = 0;
var isShowOtherLangue = false;
function hideLanguage(obj) {
    $("#otherLanguage_content2").slideToggle();
    $("#otherLanguage_title2").slideToggle();
    $("#otherLanguage_content").slideToggle();
    $("#otherLanguage_title").slideToggle();
    if (isShowOtherLangue) {
        //current opened
        $(obj).html("<span style=\"color: Black;\">Other Language</span> [+]");
    }
    else {
        //current closed
        $(obj).html("<span style=\"color: Black;\">Other Language</span> [-]");
    }
    isShowOtherLangue = !isShowOtherLangue;
}

function gotoBottom() {
    window.location = '#men';
}
