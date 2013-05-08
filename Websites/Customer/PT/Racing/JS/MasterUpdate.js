$(function () {
    RacingSetting.initConfirmUpdateMember();
    var TplMasterUpdate = '<table class="tblcollapse w800">\
                                <tr>\
                                    <td colspan="3">\
                                        Enable:\&nbsp;<input type="checkbox" onclick="ChkEnalbe(this)" {{if !EnableUpline}}disabled="disabled"{{/if}} {{if Enable}}checked="true"{{/if}} value="${Enable}" id="${getId(0)}_chkEnable" />&nbsp;Master\
                                        &nbsp;&nbsp;&nbsp;<input type="checkbox" id="${getId(0)}_chkEnableDownline" {{if EnableDownline}}checked="true"{{/if}} value="${EnableDownline}" disabled="disabled" />&nbsp;Master\'s downlines\
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
                                        Min Bet <input type="text" maxlength="14" onkeypress="return AllowNumber(event)" id="${getId(0)}_txtMinbet" value="${MinBet}" old="${MinBet}" />&nbsp;\
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
                                    <td class="l">Master Preset Position</td>\
                                    <td>\
                                        <select max="{{if this.data.Bettypes[1].PositionTaking.IsAutoPt}}${roundNumber(this.data.Bettypes[1].PositionTaking.MaxPt,2)}{{else}}${CalMaxPt(this.data.Bettypes[0].PositionTaking.MaxUplinePt, this.data.Bettypes[0].PositionTaking.UplinePt,this.data.Bettypes[0].PositionTaking.MaxPt)}{{/if}}" min="${this.data.Bettypes[0].PositionTaking.MinPt}" class="pt" id="${getId(0)}_mpositiontaking" old="${this.data.Bettypes[0].PositionTaking.Pt}" oldmax="${this.data.Bettypes[0].PositionTaking.MaxPt}">\
                                            <option value="${this.data.Bettypes[0].PositionTaking.Pt}">${this.data.Bettypes[0].PositionTaking.Pt}</option>\
                                        </select>\
                                    </td>\
                                    <td>\
                                        <select max="{{if this.data.Bettypes[1].PositionTaking.IsAutoPt}}${roundNumber(this.data.Bettypes[1].PositionTaking.MaxPt,2)}{{else}}${CalMaxPt(this.data.Bettypes[1].PositionTaking.MaxUplinePt, this.data.Bettypes[1].PositionTaking.UplinePt,this.data.Bettypes[1].PositionTaking.MaxPt)}{{/if}}" min="${this.data.Bettypes[1].PositionTaking.MinPt}" class="pt" id="${getId(1)}_mpositiontaking" old="${this.data.Bettypes[1].PositionTaking.Pt}" oldmax="${this.data.Bettypes[1].PositionTaking.MaxPt}">\
                                            <option value="${this.data.Bettypes[1].PositionTaking.Pt}">${this.data.Bettypes[1].PositionTaking.Pt}</option>\
                                        </select>\
                                    </td>\
                                </tr>\
                                    <tr>\
                                    <td class="l">Super Preset Position</td>\
                                    <td>\
                                        <select onchange="ChangePresetPT(this,mposition);" maxLine="${this.data.Bettypes[0].PositionTaking.MaxUplinePt}" max="${roundNumber(parseFloat(this.data.Bettypes[0].PositionTaking.MaxUplinePt)-parseFloat(this.data.Bettypes[0].PositionTaking.MinPt),2)}" min="${this.data.Bettypes[0].PositionTaking.MinUplinePt}" class="pt" id="${getId(0)}_spositiontaking" {{if this.data.Bettypes[0].PositionTaking.IsAutoPt}}disabled="disabled"{{/if}} old="${this.data.Bettypes[0].PositionTaking.UplinePt}">\
                                            <option value="${this.data.Bettypes[0].PositionTaking.UplinePt}">${this.data.Bettypes[0].PositionTaking.UplinePt}</option>\
                                        </select>\
                                    </td>\
                                    <td>\
                                        <select onchange="ChangePresetPT(this,mposition);" maxLine="${this.data.Bettypes[1].PositionTaking.MaxUplinePt}" max="${roundNumber(parseFloat(this.data.Bettypes[1].PositionTaking.MaxUplinePt)-parseFloat(this.data.Bettypes[1].PositionTaking.MinPt),2)}" min="${this.data.Bettypes[1].PositionTaking.MinUplinePt}" class="pt" id="${getId(1)}_spositiontaking" {{if this.data.Bettypes[1].PositionTaking.IsAutoPt}}disabled="disabled"{{/if}} old="${this.data.Bettypes[1].PositionTaking.UplinePt}">\
                                            <option value="${this.data.Bettypes[1].PositionTaking.UplinePt}">${this.data.Bettypes[1].PositionTaking.UplinePt}</option>\
                                        </select>\
                                    </td>\
                                </tr>\
                                <tr>\
                                    <td class="l" rowspan="2">Super Auto Preset Position</td>\
                                    <td>\
                                        <input type="checkbox" onclick="ChkAutoPT(this)" {{if this.data.Bettypes[0].PositionTaking.IsAutoPt}}checked="true"{{/if}} value="${this.data.Bettypes[0].PositionTaking.IsAutoPt}" id="${getId(0)}_spositiontaking_isauto" old="${this.data.Bettypes[0].PositionTaking.IsAutoPt}"/>\
                                    </td>\
                                    <td>\
                                        <input type="checkbox" onclick="ChkAutoPT(this)" {{if this.data.Bettypes[1].PositionTaking.IsAutoPt}}checked="true"{{/if}} value="${this.data.Bettypes[1].PositionTaking.IsAutoPt}" id="${getId(1)}_spositiontaking_isauto" old="${this.data.Bettypes[1].PositionTaking.IsAutoPt}"/>\
                                    </td>\
                                </tr>\
                                    <tr>\
                                    <td>\
                                        <select max="${this.data.Bettypes[0].PositionTaking.MaxAutoPt}" min="${this.data.Bettypes[0].PositionTaking.MinAutoPt}" class="pt" id="${getId(0)}_spositiontaking_auto" {{if this.data.Bettypes[0].PositionTaking.IsAutoPt==false}}disabled="disabled"{{/if}} old="${this.data.Bettypes[0].PositionTaking.AutoPt}">\
                                            <option value="${this.data.Bettypes[0].PositionTaking.AutoPt}">${this.data.Bettypes[0].PositionTaking.AutoPt}</option>\
                                        </select>\
                                    </td>\
                                    <td>\
                                        <select max="${this.data.Bettypes[1].PositionTaking.MaxAutoPt}" min="${this.data.Bettypes[1].PositionTaking.MinAutoPt}" class="pt" id="${getId(1)}_spositiontaking_auto" {{if this.data.Bettypes[1].PositionTaking.IsAutoPt==false}}disabled="disabled"{{/if}} old="${this.data.Bettypes[1].PositionTaking.AutoPt}">\
                                            <option value="${this.data.Bettypes[1].PositionTaking.AutoPt}">${this.data.Bettypes[1].PositionTaking.AutoPt}</option>\
                                        </select>\
                                    </td>\
                                </tr>\
                                <tr>\
                                    <td></td>\
                                    <td colspan="2" class="l"><input type="checkbox" onclick="ChkCopyFromWin(this)" id="${getId(0)}_chkCopyFromWin" />&nbsp;Copy from Win</td>\
                                </tr>\
                                    <tr>\
                                    <td colspan="3" class="l"><input type="checkbox" id="${getId(0)}_chkApplyToMember" />&nbsp;<b>Apply Super Preset ${this.parent.data.Name} PT to all Members</b></td>\
                                </tr>\
                            </table>';

    $.extend(true, RacingSetting, { TplRacingTypeSetting: TplMasterUpdate });
    RacingData.getCustomerSetting(parseInt(gup("custId")), RacingSetting.init);
});

function ChkCopyFromWin(obj) {
    var IdWin = $(obj).attr("id").split("_")[1];
    var IdPlace = $(obj).attr("id").split("_")[0] + "_" + RacingSetting.getIdPlace(IdWin);
    var isCopied = $(obj).attr("checked");
    if (isCopied) {
        $("#" + IdPlace + "_" + sposition).attr("disabled", "disabled");
        $("#" + IdPlace + "_" + mposition).attr("disabled", "disabled");
        $("#" + IdPlace + "_" + spositiontaking_isauto).attr("disabled", "disabled");
        $("#" + IdPlace + "_" + spositiontaking_auto).attr("disabled", "disabled");
    }
    else {
        
        $("#" + IdPlace + "_" + mposition).removeAttr("disabled");
        $("#" + IdPlace + "_" + spositiontaking_isauto).removeAttr("disabled");
        

        if ($("#" + IdPlace + "_" + spositiontaking_isauto).attr("checked")) {
            $("#" + IdPlace + "_" + spositiontaking_auto).removeAttr("disabled");
        } else {
            $("#" + IdPlace + "_" + sposition).removeAttr("disabled");
        }
    }
}
function ChkAutoPT(obj) {
    var Id = $(obj).attr("id").split("_")[0] + "_" + $(obj).attr("id").split("_")[1];
    var isAuto = $(obj).attr("checked");
    if (isAuto) {
        $("#" + Id + "_" + sposition).attr("disabled", "disabled");
        $("#" + Id + "_" + spositiontaking_auto).removeAttr("disabled");
        var max = $("#" + Id + "_" + mposition).attr("oldmax");
        var curr = $("#" + Id + "_" + mposition).val();

        CreateOption($("#" + Id + "_" + mposition), curr, max, $("#" + Id + "_" + mposition).attr("min"), 0.1, 2);
    }
    else {
        $("#" + Id + "_" + spositiontaking_auto).attr("disabled", "disabled");
        $("#" + Id + "_" + sposition).removeAttr("disabled");
        ChangePresetPT(document.getElementById(Id + "_" + sposition), mposition);
    }
}

function ChangePresetPT(obj, type) {
    var Id = $(obj).attr("id").split("_")[0] + "_" + $(obj).attr("id").split("_")[1];
    var max = roundNumber($(obj).attr("maxLine") - $(obj).val(), 2);
    var master_obj = $("#" + Id + "_" + type);
    var min = master_obj.attr("min");
    max = (max == 0) ? Math.max(max, min) : max;
    max = Math.min(max, master_obj.attr("oldmax"));
    //min = Math.max(roundNumber($(obj).attr("minLine") - $(obj).val(), 2), min);
    CreateOption(master_obj, max, max, min, 0.1, 2);
}

function ChkEnalbe(obj)
{
    var checked = $(obj).attr("checked");
    var oldvalue = $(obj).attr("value") == "true" ? true : false;
   
    var downlineId = $(obj).attr("id").replace(chkEnable, chkEnableDownline);
    var oldvaluedownline = $("#" + downlineId).attr("value") == "true" ? true : false;
    if (checked != oldvalue) {
        if (checked) {
            $("#" + downlineId).removeAttr("disabled");
        }
        else {
            $("#" + downlineId).attr({ "disabled": "disabled", "checked": false });
        }
    }
    else {
        $("#" + downlineId).attr({ "disabled": "disabled", "checked": oldvaluedownline });
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
                    $("#" + placeId + "_" + sposition).attr("disabled", "disabled");
                    $("#" + placeId + "_" + mposition).attr("disabled", "disabled");
                    $("#" + placeId + "_" + spositiontaking_isauto).attr("disabled", "disabled");
                    $("#" + placeId + "_" + spositiontaking_auto).attr("disabled", "disabled");

                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");
                }
                else {
                    $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");

                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
                        $("#" + placeId + "_" + mposition).removeAttr("disabled");
                        $("#" + placeId + "_" + spositiontaking_isauto).removeAttr("disabled");
                        if ($("#" + placeId + "_" + spositiontaking_isauto).attr("checked")) {
                            $("#" + placeId + "_" + spositiontaking_auto).removeAttr("disabled");
                        } else {
                            $("#" + placeId + "_" + sposition).removeAttr("disabled");
                        }
                    }
                }
            }
            else {
                if (isCopied) {
                    $("#" + winId + "_" + commission).attr("disabled", "disabled");
                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");

                    $("#" + winId + "_" + sposition).attr("disabled", "disabled");
                    $("#" + winId + "_" + mposition).attr("disabled", "disabled");
                    $("#" + winId + "_" + spositiontaking_isauto).attr("disabled", "disabled");
                    $("#" + winId + "_" + spositiontaking_auto).attr("disabled", "disabled");

                    $("#" + placeId + "_" + sposition).attr("disabled", "disabled");
                    $("#" + placeId + "_" + mposition).attr("disabled", "disabled");
                    $("#" + placeId + "_" + spositiontaking_isauto).attr("disabled", "disabled");
                    $("#" + placeId + "_" + spositiontaking_auto).attr("disabled", "disabled");
                }
                else {
                    $("#" + winId + "_" + commission).removeAttr("disabled");
                    $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");

                    $("#" + winId + "_" + mposition).removeAttr("disabled");
                    $("#" + winId + "_" + spositiontaking_isauto).removeAttr("disabled");

                    if ($("#" + winId + "_" + spositiontaking_isauto).attr("checked")) {
                        $("#" + winId + "_" + spositiontaking_auto).removeAttr("disabled");
                    } else {
                        $("#" + winId + "_" + sposition).removeAttr("disabled");
                    }

                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
                        $("#" + placeId + "_" + mposition).removeAttr("disabled");
                        $("#" + placeId + "_" + spositiontaking_isauto).removeAttr("disabled");

                        if ($("#" + placeId + "_" + spositiontaking_isauto).attr("checked")) {
                            $("#" + placeId + "_" + spositiontaking_auto).removeAttr("disabled");
                        } else {
                            $("#" + placeId + "_" + sposition).removeAttr("disabled");
                        }
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
    }
    else {
        RacingSetting.checkUpdateToAllMember();
   }
}

function CalMaxPt(max_upline_pt, upline_pt, max_pt) {
    if (max_pt == 0)
        return roundNumber(parseFloat(max_upline_pt) - parseFloat(upline_pt), 2);
    return roundNumber(Math.min(parseFloat(max_upline_pt) - parseFloat(upline_pt), max_pt), 2);
}