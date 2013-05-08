$(function () {
    var TplMemberUpdate = '<table class="tblcollapse w800">\
                            <tr>\
                                <td colspan="3">\
                                    Enable:\&nbsp;<input type="checkbox" value="${Enable}" id="${getId(0)}_chkEnable" {{if !EnableUpline}}disabled="disabled"{{/if}} {{if Enable}}checked="true"{{/if}} />&nbsp;Member\
                                </td>\
                            </tr>\
                            <tr class="title">\
                                <td class="c" colspan="3">Commission</td>\
                            </tr>\
                            <tr>\
                                <td colspan="3" >\
                                     <select class="select-comm" max="${MaxComm}" id="${getId(0)}_commission" old="${Comm}">\
                                        <option value="${Comm}">${Comm}</option>\
                                    </select>\
                                </td>\
                            </tr>\
                            <tr class="title">\
                                <td class="c" colspan="3">Bet Setting</td>\
                            </tr>\
                            <tr>\
                                <td>\
                                    Min Bet <input type="text" maxlength="14" onkeypress="return AllowNumber(event)" id="${getId(0)}_txtMinbet" value="${MinBet}" old="${MinBet}"/>&nbsp;\
                                    {{if MinBetMin>0 }}>=<span id="${getId(0)}_txtMinbet_Min">${MinBetMin}</span>{{/if}}\
                                </td>\
                                <td>\
                                    Max Bet <input type="text" maxlength="14" onkeypress="return AllowNumber(event)" id="${getId(0)}_txtMaxbet" value="${MaxBet}" old="${MaxBet}"/>&nbsp;\
                                    {{if MaxBetMax>0 }}<=<span id="${getId(0)}_txtMaxbet_Max">${MaxBetMax}</span>{{/if}}\
                                </td>\
                                <td>\
                                    Max Per Match <input type="text" maxlength="14" onkeypress="return AllowNumber(event)" id="${getId(0)}_txtMaxPerMatch" value="${MaxPerMatch}" old="${MaxPerMatch}"/>&nbsp;\
                                    {{if MaxPerMatchMax>0 }}<=<span id="${getId(0)}_txtMaxPerMatch_Max">${MaxPerMatchMax}</span>{{/if}}\
                                </td>\
                            </tr>\
                            <tr class="title">\
                                <td colspan="3">Position Taking</td>\
                            </tr>\
                            <tr>\
                                <td></td>\
                                <td><b>Win</b></td>\
                                <td><b>Place</b></td>\
                            </tr>\
                            <tr>\
                                <td class="l">Agent Position</td>\
                                <td>\
                                    <select max="${this.data.Bettypes[0].PositionTaking.MaxPt}" min="${this.data.Bettypes[0].PositionTaking.MinPt}" class="pt" id="${getId(0)}_apositiontaking" old="${this.data.Bettypes[0].PositionTaking.Pt}">\
                                        <option value="${this.data.Bettypes[0].PositionTaking.Pt}">${this.data.Bettypes[0].PositionTaking.Pt}</option>\
                                    </select>\
                                </td>\
                                <td>\
                                    <select max="${this.data.Bettypes[1].PositionTaking.MaxPt}" min="${this.data.Bettypes[1].PositionTaking.MinPt}" class="pt" id="${getId(1)}_apositiontaking" old="${this.data.Bettypes[1].PositionTaking.Pt}">\
                                        <option value="${this.data.Bettypes[1].PositionTaking.Pt}">${this.data.Bettypes[1].PositionTaking.Pt}</option>\
                                    </select>\
                                </td>\
                            </tr>\
                            <tr>\
                                <td></td>\
                                <td colspan="2" class="l"><input type="checkbox" onclick="ChkCopyFromWin(this)" id="${getId(0)}_chkCopyFromWin" />&nbsp;Copy from Win</td>\
                            </tr>\
                        </table>';

    $.extend(RacingSetting, { TplRacingTypeSetting: TplMemberUpdate });
    RacingData.getCustomerSetting(parseInt(gup("custId")), RacingSetting.init);
});

function ChkCopyFromWin(obj) {
    var IdWin = $(obj).attr("id").split("_")[1];
    var IdPlace = $(obj).attr("id").split("_")[0] + "_" + RacingSetting.getIdPlace(IdWin);

    var isCopied = $(obj).attr("checked");
    if (isCopied) {
        $("#" + IdPlace + "_" + aposition).attr("disabled", "disabled");
    }
    else {
        
        $("#" + IdPlace + "_" + aposition).removeAttr("disabled");
    }
}


function ChkCopyAllSetting(obj) {
    var isCopied = $(obj).attr("checked");
    var sportId;
    var winId;
    var placeId;
    for (var i = 0; i < RacingSettingObj.RacingSportType.length; i++) {
        sportId = RacingSettingObj.RacingSportType[i].Id;
        for (var j = 0; j < RacingSettingObj.RacingSportType[i].Totes.length; j++) {
            var obj = RacingSettingObj.RacingSportType[i].Totes[j];
            winId = sportId + "_" + obj.Bettypes[0].Id;
            placeId = sportId + "_" + obj.Bettypes[1].Id;
            if (obj.Bettypes.length > 0 && (winId == RacingSettingObj.DefaultRacingSportTypeId)) {
                if (isCopied)
                {
                    $("#" + placeId + "_" + aposition).attr("disabled", "disabled");
                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");
                }
                else {
                    $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");

                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
                        $("#" + placeId + "_" + aposition).removeAttr("disabled");
                    }
                }
            }
            else {
                if (isCopied) {
                    $("#" + winId + "_" + commission).attr("disabled", "disabled");
                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");
                    $("#" + winId + "_" + aposition).attr("disabled", "disabled");
                    
                    $("#" + placeId + "_" + aposition).attr("disabled", "disabled");
                }
                else {
                    $("#" + winId + "_" + commission).removeAttr("disabled");
                    $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");
                    $("#" + winId + "_" + aposition).removeAttr("disabled");

                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
                        $("#" + placeId + "_" + aposition).removeAttr("disabled");
                    }
                }
            }
        }
    }
}
function BtnOnClick() {
    $(".errmsg").empty();
    if (!RacingSetting.checkValidBetSetting()) {
        return;
    } else {
        RacingSetting.updatePT();
    }
}
