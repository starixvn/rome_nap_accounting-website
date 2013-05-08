$(function () {
    var TplMasterOnMemberUpdate = '<table class="tblcollapse w800">\
                                <tr class="title">\
                                    <td class="c" colspan="3">Commission</td>\
                                </tr>\
                                <tr>\
                                    <td></td>\
                                    <td>Comm for Agent</td>\
                                    <td>Comm for Member</td>\
                                </tr>\
                                  <tr>\
                                    <td></td>\
                                    <td>${AgentComm}</td>\
                                    <td>${Comm}</td>\
                                </tr>\
                                <tr class="title">\
                                    <td class="c" colspan="3">Bet Setting</td>\
                                </tr>\
                                <tr>\
                                    <td>Min Bet: ${MinBet}</td>\
                                    <td>Max Bet: ${MaxBet}</td>\
                                    <td>Max Per Match: ${MaxPerMatch}</td>\
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
                                    <td class="l">Agent</td>\
                                    <td>${this.data.Bettypes[0].PositionTaking.AgentPt}</td>\
                                    <td>${this.data.Bettypes[1].PositionTaking.AgentPt}</td>\
                                </tr>\
                                <tr>\
                                    <td class="l">Master</td>\
                                    <td>\
                                    {{if this.data.Bettypes[0].PositionTaking.IsUplineAutoPt}}\
                                        ${this.data.Bettypes[0].PositionTaking.Pt}\
                                        <input type="hidden" id="${getId(0)}_mpositiontaking" value="${this.data.Bettypes[0].PositionTaking.Pt}"/>\
                                    {{else}}\
                                        <select max="${this.data.Bettypes[0].PositionTaking.MaxPt}" min="${this.data.Bettypes[0].PositionTaking.MinPt}" class="pt" id="${getId(0)}_mpositiontaking" {{if this.data.Bettypes[0].PositionTaking.IsAutoPt}}disabled="disabled"{{/if}} old="${this.data.Bettypes[0].PositionTaking.Pt}">\
                                            <option value="${this.data.Bettypes[0].PositionTaking.Pt}">${this.data.Bettypes[0].PositionTaking.Pt}</option>\
                                        </select>\
                                    {{/if}}\
                                    </td>\
                                    <td>\
                                    {{if this.data.Bettypes[1].PositionTaking.IsUplineAutoPt}}\
                                        ${this.data.Bettypes[1].PositionTaking.Pt}\
                                        <input type="hidden" id="${getId(1)}_mpositiontaking" value="${this.data.Bettypes[0].PositionTaking.Pt}"/>\
                                    {{else}}\
                                        <select max="${this.data.Bettypes[1].PositionTaking.MaxPt}" min="${this.data.Bettypes[1].PositionTaking.MinPt}" class="pt" id="${getId(1)}_mpositiontaking" {{if this.data.Bettypes[1].PositionTaking.IsAutoPt}}disabled="disabled"{{/if}} old="${this.data.Bettypes[1].PositionTaking.Pt}">\
                                            <option value="${this.data.Bettypes[1].PositionTaking.Pt}">${this.data.Bettypes[1].PositionTaking.Pt}</option>\
                                        </select>\
                                    {{/if}}\
                                    </td>\
                                </tr>\
                                <tr {{if !EnableAutoMPT}}class="hidden"{{/if}}>\
                                        <td class="l" rowspan="2">Master Auto</td>\
                                        <td>\
                                            <input type="checkbox" onclick="ChkAutoPT(this)" {{if this.data.Bettypes[0].PositionTaking.IsAutoPt}}checked="true"{{/if}} value="${this.data.Bettypes[0].PositionTaking.IsAutoPt}" id="${getId(0)}_mpositiontaking_isauto" old="${this.data.Bettypes[0].PositionTaking.IsAutoPt}" />\
                                        </td>\
                                        <td>\
                                            <input type="checkbox" onclick="ChkAutoPT(this)" {{if this.data.Bettypes[1].PositionTaking.IsAutoPt}}checked="true"{{/if}} value="${this.data.Bettypes[1].PositionTaking.IsAutoPt}" id="${getId(1)}_mpositiontaking_isauto" old="${this.data.Bettypes[1].PositionTaking.IsAutoPt}" />\
                                        </td>\
                                    </tr>\
                                     <tr {{if !EnableAutoMPT}}class="hidden"{{/if}}>\
                                        <td>\
                                            <select max="${this.data.Bettypes[0].PositionTaking.MaxAutoPt}" min="${this.data.Bettypes[0].PositionTaking.MinPt}" class="pt" id="${getId(0)}_mpositiontaking_auto" {{if this.data.Bettypes[0].PositionTaking.IsAutoPt==false}}disabled="disabled"{{/if}} old="${this.data.Bettypes[0].PositionTaking.AutoPt}">\
                                                <option value="${this.data.Bettypes[0].PositionTaking.AutoPt}">${this.data.Bettypes[0].PositionTaking.AutoPt}</option>\
                                            </select>\
                                        </td>\
                                        <td>\
                                            <select max="${this.data.Bettypes[1].PositionTaking.MaxAutoPt}" min="${this.data.Bettypes[1].PositionTaking.MinPt}" class="pt" id="${getId(1)}_mpositiontaking_auto" {{if this.data.Bettypes[1].PositionTaking.IsAutoPt==false}}disabled="disabled"{{/if}} old="${this.data.Bettypes[1].PositionTaking.AutoPt}">\
                                                <option value="${this.data.Bettypes[1].PositionTaking.AutoPt}">${this.data.Bettypes[1].PositionTaking.AutoPt}</option>\
                                            </select>\
                                        </td>\
                                    </tr>\
                                <tr>\
                                    <td></td>\
                                    <td colspan="2" class="l"><input ${validShowCopyFromWin(this.data.Bettypes)} old="${validShowCopyFromWin(this.data.Bettypes)}" isuplineautopt="${this.data.Bettypes[0].PositionTaking.IsUplineAutoPt}" type="checkbox" onclick="ChkCopyFromWin(this)" id="${getId(0)}_chkCopyFromWin" />&nbsp;Copy from Win</td>\
                                </tr>\
                            </table>';

    $.extend(RacingSetting, { TplRacingTypeSetting: TplMasterOnMemberUpdate });
    RacingData.getCustomerSetting(parseInt(gup("custId")), RacingSetting.init);
});

function ChkCopyFromWin(obj) {
    var IdWin = $(obj).attr("id").split("_")[1];
    var IdPlace = $(obj).attr("id").split("_")[0] + "_" + RacingSetting.getIdPlace(IdWin);
    var isCopied = $(obj).attr("checked");
    if (isCopied) {
        $("#" + IdPlace + "_" + mposition).attr("disabled", "disabled");
        $("#" + IdPlace + "_" + mpositiontaking_isauto).attr("disabled", "disabled");
        $("#" + IdPlace + "_" + mpositiontaking_auto).attr("disabled", "disabled");
    }
    else {
        $("#" + IdPlace + "_" + mpositiontaking_isauto).removeAttr("disabled");
        if ($("#" + IdPlace + "_" + mpositiontaking_isauto).attr("checked")) {
            $("#" + IdPlace + "_" + mpositiontaking_auto).removeAttr("disabled");
        } else {
            $("#" + IdPlace + "_" + mposition).removeAttr("disabled");
        }
    }
}


function ChkAutoPT(obj) {
    var Id = $(obj).attr("id").split("_")[0] + "_" + $(obj).attr("id").split("_")[1];
    var isAuto = $(obj).attr("checked");
    if (isAuto) {
        $("#" + Id + "_" + mposition).attr("disabled", "disabled");
        $("#" + Id + "_" + mpositiontaking_auto).removeAttr("disabled");
        if (typeof $("#" + Id + "_chkCopyFromWin").attr("disabled") != 'undefined') {
            if (!$("#chkCopyAllSetting").attr("checked")) {
                $("#" + Id + "_chkCopyFromWin").removeAttr("disabled");
            }
            if (Id.indexOf("151_41") != -1) {
                $("#chkCopyAllSetting").removeAttr("disabled");
            }
        }
    }
    else {
        $("#" + Id + "_" + mpositiontaking_auto).attr("disabled", "disabled");
        $("#" + Id + "_" + mposition).removeAttr("disabled");
        if (($("#" + Id + "_chkCopyFromWin").attr("old") == 'disabled' || $("#" + Id + "_chkCopyFromWin").attr("isuplineautopt").toString()=='true')) {
            if (Id.indexOf("151_41") != -1) {
                $("#chkCopyAllSetting").attr("disabled", "disabled").attr("checked", false);
                ChkCopyAllSetting($("#chkCopyAllSetting"));
            }
            $("#" + Id + "_chkCopyFromWin").attr("disabled", "disabled").attr("checked", false);
            ChkCopyFromWin($("#" + Id + "_chkCopyFromWin"));
        }
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
                    $("#" + placeId + "_" + mposition).attr("disabled", "disabled");
                    $("#" + placeId + "_" + mpositiontaking_isauto).attr("disabled", "disabled");
                    $("#" + placeId + "_" + mpositiontaking_auto).attr("disabled", "disabled");
                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");
                }
                else {
                    $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");
                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
                        $("#" + placeId + "_" + mpositiontaking_isauto).removeAttr("disabled");
                        if ($("#" + placeId + "_" + mpositiontaking_isauto).attr("checked")) {
                            $("#" + placeId + "_" + mpositiontaking_auto).removeAttr("disabled");
                        } else {
                            $("#" + placeId + "_" + mposition).removeAttr("disabled");
                        }
                    }
                }
            }
            else {
                if (isCopied) {
                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");
                  
                    $("#" + winId + "_" + mposition).attr("disabled", "disabled");
                    $("#" + winId + "_" + mpositiontaking_isauto).attr("disabled", "disabled");
                    $("#" + winId + "_" + mpositiontaking_auto).attr("disabled", "disabled");

                    $("#" + placeId + "_" + mposition).attr("disabled", "disabled");
                    $("#" + placeId + "_" + mpositiontaking_isauto).attr("disabled", "disabled");
                    $("#" + placeId + "_" + mpositiontaking_auto).attr("disabled", "disabled");
                }
                else {
                    if ($("#" + winId + "_" + chkCopyFromWin).attr('old') != 'disabled' || $("#" + winId + "_" + mpositiontaking_isauto).attr("checked")) {
                        $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");
                    }
                    $("#" + winId + "_" + mpositiontaking_isauto).removeAttr("disabled");
                    
                    if ($("#" + winId + "_" + mpositiontaking_isauto).attr("checked")) {
                        $("#" + winId + "_" + mpositiontaking_auto).removeAttr("disabled");
                    } else {
                        $("#" + winId + "_" + mposition).removeAttr("disabled");
                    }

                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
                        $("#" + placeId + "_" + mpositiontaking_isauto).removeAttr("disabled");
                        if ($("#" + placeId + "_" + mpositiontaking_isauto).attr("checked")) {
                            $("#" + placeId + "_" + mpositiontaking_auto).removeAttr("disabled");
                        } else {
                            $("#" + placeId + "_" + mposition).removeAttr("disabled");
                        }
                    }
                }
            }
        }
    }
}

function BtnOnClick() {
    $(".errmsg").empty();
    RacingSetting.updatePT();
}
function validShowCopyFromWin(bettypes) {
    if (typeof bettypes != 'undefined') {
        try {
            var _betWin = bettypes[0].PositionTaking;
            var _betPlace = bettypes[1].PositionTaking;
            if (_betWin.IsUplineAutoPt && !_betWin.IsAutoPt)
                return "disabled";
        } catch (e) { }
    }
    return "";
}