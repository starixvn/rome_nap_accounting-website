$(function () {
    var TplSuperUpdate = '<table class="tblcollapse w800">\
                                    <tr>\
                                        <td colspan="3">\
                                            Enable:\&nbsp;<input type="checkbox" onclick="ChkEnalbe(this)" {{if Enable}}checked="true"{{/if}} value="${Enable}" id="${getId(0)}_chkEnable" />&nbsp;Super\
                                            &nbsp;&nbsp;&nbsp;<input type="checkbox" id="${getId(0)}_chkEnableDownline" {{if EnableDownline}}checked="true"{{/if}} value="${EnableDownline}" disabled="disabled" />&nbsp;Super\'s downlines\
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
                                           Min Bet <input type="text" maxlength="14" onkeypress="return AllowNumber(event)" id="${getId(0)}_txtMinbet" value="${MinBet}" old="${MinBet}" />\
                                        </td>\
                                        <td>\
                                           Max Bet <input type="text" maxlength="14" onkeypress="return AllowNumber(event)" id="${getId(0)}_txtMaxbet" value="${MaxBet}" old="${MaxBet}" />\
                                        </td>\
                                        <td>\
                                           Max Per Match <input type="text" maxlength="14" onkeypress="return AllowNumber(event)" id="${getId(0)}_txtMaxPerMatch" value="${MaxPerMatch}" old="${MaxPerMatch}" />\
                                        </td>\
                                    </tr>\
                                    <tr class="title">\
                                        <td colspan="3">Position Taking</td>\
                                    </tr>\
                                    <tr>\
                                        <td class="l">House Auto Position</td>\
                                        <td colspan="2">\
                                            <select max="${MaxHousePt}" min="${MinHousePt}" class="pt" id="${getId(0)}_IBCPositiontaking" old="${HousePt}">\
                                                <option value="${HousePt}">${HousePt}</option>\
                                            </select>\
                                        </td>\
                                    </tr>\
                                    <tr>\
                                        <td></td>\
                                        <td><b>Win</b></td>\
                                        <td><b>Place</b></td>\
                                    </tr>\
                                    <tr>\
                                        <td class="l">Super Max Position</td>\
                                        <td>\
                                            <select onchange="ChangeMaxPT(this,mposition);" max="${this.data.Bettypes[0].PositionTaking.MaxPt}" min="${this.data.Bettypes[0].PositionTaking.MinPt}" class="pt" id="${getId(0)}_spositiontaking" old="${this.data.Bettypes[0].PositionTaking.Pt}">\
                                                <option value="${this.data.Bettypes[0].PositionTaking.Pt}">${this.data.Bettypes[0].PositionTaking.Pt}</option>\
                                            </select>\
                                        </td>\
                                        <td>\
                                            <select onchange="ChangeMaxPT(this,mposition);" max="${this.data.Bettypes[1].PositionTaking.MaxPt}" min="${this.data.Bettypes[0].PositionTaking.MinPt}" class="pt" id="${getId(1)}_spositiontaking" old="${this.data.Bettypes[1].PositionTaking.Pt}">\
                                                <option value="${this.data.Bettypes[1].PositionTaking.Pt}">${this.data.Bettypes[1].PositionTaking.Pt}</option>\
                                            </select>\
                                        </td>\
                                    </tr>\
                                     <tr>\
                                        <td class="l">Master Max Position</td>\
                                        <td>\
                                            <select onchange="ChangeMaxPT(this,aposition);" max="${this.data.Bettypes[0].PositionTaking.Pt}" min="0" class="pt" id="${getId(0)}_mpositiontaking" old="${this.data.Bettypes[0].PositionTaking.MasterPt}">\
                                                <option value="${this.data.Bettypes[0].PositionTaking.MasterPt}">${this.data.Bettypes[0].PositionTaking.MasterPt}</option>\
                                            </select>\
                                        </td>\
                                        <td>\
                                            <select onchange="ChangeMaxPT(this,aposition);" max="${this.data.Bettypes[1].PositionTaking.Pt}" min="0" class="pt" id="${getId(1)}_mpositiontaking" old="${this.data.Bettypes[1].PositionTaking.MasterPt}">\
                                                <option value="${this.data.Bettypes[1].PositionTaking.MasterPt}">${this.data.Bettypes[1].PositionTaking.MasterPt}</option>\
                                            </select>\
                                        </td>\
                                    </tr>\
                                    <tr>\
                                        <td class="l">Agent Max Position</td>\
                                        <td>\
                                            <select max="${this.data.Bettypes[0].PositionTaking.MasterPt}" min="0" class="pt" id="${getId(0)}_apositiontaking" old="${this.data.Bettypes[0].PositionTaking.AgentPt}">\
                                                <option value="${this.data.Bettypes[0].PositionTaking.AgentPt}">${this.data.Bettypes[0].PositionTaking.AgentPt}</option>\
                                            </select>\
                                        </td>\
                                        <td>\
                                            <select max="${this.data.Bettypes[1].PositionTaking.MasterPt}" min="0" class="pt" id="${getId(1)}_apositiontaking" old="${this.data.Bettypes[1].PositionTaking.AgentPt}">\
                                                <option value="${this.data.Bettypes[1].PositionTaking.AgentPt}">${this.data.Bettypes[1].PositionTaking.AgentPt}</option>\
                                            </select>\
                                        </td>\
                                    </tr>\
                                    <tr>\
                                        <td></td>\
                                        <td colspan="2" class="l"><input type="checkbox" onclick="ChkCopyFromWin(this)" id="${getId(0)}_chkCopyFromWin" />&nbsp;Copy from Win</td>\
                                    </tr>\
                                </table>';

    $.extend(true, RacingSetting, { TplRacingTypeSetting: TplSuperUpdate });
    RacingData.getCustomerSetting(parseInt(gup("custId")), RacingSetting.init);
});

function ChkCopyFromWin(obj) {
    var IdWin=   $(obj).attr("id").split("_")[1];
    var IdPlace = $(obj).attr("id").split("_")[0] + "_" + RacingSetting.getIdPlace(IdWin);
    var isCopied = $(obj).attr("checked");
    if (isCopied) {
        $("#" + IdPlace + "_" + sposition).attr("disabled", "disabled");
        $("#" + IdPlace + "_" + mposition).attr("disabled", "disabled");
        $("#" + IdPlace + "_" + aposition).attr("disabled", "disabled");
    }
    else {
        $("#" + IdPlace + "_" + sposition).removeAttr("disabled");
        $("#" + IdPlace + "_" + mposition).removeAttr("disabled");
        $("#" + IdPlace + "_" + aposition).removeAttr("disabled");
    }
}

function ChangeMaxPT(obj, type) {
    var IdWin = $(obj).attr("id").split("_")[0] + "_" + $(obj).attr("id").split("_")[1];
    var max=$(obj).val();
    var master_obj = $("#" + IdWin + "_" + type);
    master_obj.attr("max",max);
    CreateOption(master_obj, max, max, 0, 0.1, 2);
    if (type == mposition) {
        ChangeMaxPT(master_obj, aposition);
    }
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
                    $("#" + placeId + "_" + aposition).attr("disabled", "disabled");
                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");
                }
                else {
                    $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");

                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
                        $("#" + placeId + "_" + sposition).removeAttr("disabled");
                        $("#" + placeId + "_" + mposition).removeAttr("disabled");
                        $("#" + placeId + "_" + aposition).removeAttr("disabled");
                    }
                }
            }
            else {
                if (isCopied) {
                    $("#" + winId + "_" + commission).attr("disabled", "disabled");
                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");

                    $("#" + winId + "_" + sposition).attr("disabled", "disabled");
                    $("#" + winId + "_" + mposition).attr("disabled", "disabled");
                    $("#" + winId + "_" + aposition).attr("disabled", "disabled");
                    $("#" + winId + "_" + IBCPositiontaking).attr("disabled", "disabled");

                    $("#" + placeId + "_" + sposition).attr("disabled", "disabled");
                    $("#" + placeId + "_" + mposition).attr("disabled", "disabled");
                    $("#" + placeId + "_" + aposition).attr("disabled", "disabled");
                }
                else {
                    $("#" + winId + "_" + commission).removeAttr("disabled");
                    $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");

                    $("#" + winId + "_" + sposition).removeAttr("disabled");
                    $("#" + winId + "_" + mposition).removeAttr("disabled");
                    $("#" + winId + "_" + aposition).removeAttr("disabled");
                    $("#" + winId + "_" + IBCPositiontaking).removeAttr("disabled");

                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
                        $("#" + placeId + "_" + sposition).removeAttr("disabled");
                        $("#" + placeId + "_" + mposition).removeAttr("disabled");
                        $("#" + placeId + "_" + aposition).removeAttr("disabled");
                    }
                }
            }
        }
    }
}
function BtnOnClick() {
    $(".errmsg").empty();
    if (!RacingSetting.checkValidBetSettingSuper()) {
        return;
    } else {
        RacingSetting.updatePT();
    }
}
