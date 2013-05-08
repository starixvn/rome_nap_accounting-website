$(function () {
    var TplSuperOnMemberUpdate = '<table class="tblcollapse w800">\
                                <tr class="title">\
                                    <td class="c" colspan="3">Commission</td>\
                                </tr>\
                                <tr>\
                                    <td>Comm for Master</td>\
                                    <td>Comm for Agent</td>\
                                    <td>Comm for Member</td>\
                                </tr>\
                                <tr>\
                                    <td>${MasterComm}</td>\
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
                                    <td>${this.data.Bettypes[0].PositionTaking.MasterPt}</td>\
                                    <td>${this.data.Bettypes[1].PositionTaking.MasterPt}</td>\
                                </tr>\
                                    <tr>\
                                    <td class="l">Super</td>\
                                    <td>\
                                    {{if this.data.Bettypes[0].PositionTaking.IsUplineAutoPt}}\
                                    ${this.data.Bettypes[0].PositionTaking.Pt}\
                                    <input type="hidden" id="${getId(0)}_spositiontaking" value="${this.data.Bettypes[0].PositionTaking.Pt}"/>\
                                    {{else}}\
                                        <select max="${this.data.Bettypes[0].PositionTaking.MaxPt}" min="${this.data.Bettypes[0].PositionTaking.MinPt}" {{if this.data.Bettypes[0].PositionTaking.IsAutoPt}}disabled="disabled"{{/if}} class="pt" id="${getId(0)}_spositiontaking" old="${this.data.Bettypes[0].PositionTaking.Pt}">\
                                            <option value="${this.data.Bettypes[0].PositionTaking.Pt}">${this.data.Bettypes[0].PositionTaking.Pt}</option>\
                                        </select>\
                                    {{/if}}\
                                    </td>\
                                    <td>\
                                    {{if this.data.Bettypes[1].PositionTaking.IsUplineAutoPt}}\
                                        ${this.data.Bettypes[1].PositionTaking.Pt}\
                                        <input type="hidden" id="${getId(1)}_spositiontaking" value="${this.data.Bettypes[1].PositionTaking.Pt}"/>\
                                    {{else}}\
                                        <select max="${this.data.Bettypes[1].PositionTaking.MaxPt}" min="${this.data.Bettypes[1].PositionTaking.MinPt}" class="pt" id="${getId(1)}_spositiontaking" {{if this.data.Bettypes[1].PositionTaking.IsAutoPt}}disabled="disabled"{{/if}} value="${this.data.Bettypes[1].PositionTaking.Pt}">\
                                            <option value="${this.data.Bettypes[1].PositionTaking.Pt}">${this.data.Bettypes[1].PositionTaking.Pt}</option>\
                                        </select>\
                                    {{/if}}\
                                    </td>\
                                </tr>\
                                <tr>\
                                    <td class="l" rowspan="2">Super Auto</td>\
                                    <td>\
                                        <input type="checkbox" onclick="ChkAutoPT(this)" {{if this.data.Bettypes[0].PositionTaking.IsAutoPt}}checked="true"{{/if}} value="${this.data.Bettypes[0].PositionTaking.IsAutoPt}" id="${getId(0)}_spositiontaking_isauto" old="${this.data.Bettypes[0].PositionTaking.IsAutoPt}" />\
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
                                    <td colspan="2" class="l"><input ${validShowCopyFromWin(this.data.Bettypes)} old="${validShowCopyFromWin(this.data.Bettypes)}" isuplineautopt="${this.data.Bettypes[0].PositionTaking.IsUplineAutoPt}" type="checkbox" onclick="ChkCopyFromWin(this)" id="${getId(0)}_chkCopyFromWin" />&nbsp;Copy from Win</td>\
                                </tr>\
                            </table>';

    $.extend(RacingSetting, { TplRacingTypeSetting: TplSuperOnMemberUpdate });
    RacingData.getCustomerSetting(parseInt(gup("custId")), RacingSetting.init);
});

function ChkCopyFromWin(obj) {
    var IdWin = $(obj).attr("id").split("_")[1];
    var IdPlace = $(obj).attr("id").split("_")[0] + "_" + RacingSetting.getIdPlace(IdWin);
    var isCopied = $(obj).attr("checked");
    if (isCopied) {
        $("#" + IdPlace + "_" + spositiontaking_isauto).attr("disabled", "disabled");
        $("#" + IdPlace + "_" + spositiontaking_auto).attr("disabled", "disabled");
        $("#" + IdPlace + "_" + sposition).attr("disabled", "disabled");
    }
    else {
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
        if (typeof $("#" + Id + "_chkCopyFromWin").attr("disabled") != 'undefined') {
            if (!$("#chkCopyAllSetting").attr("checked")) {
                $("#" + Id + "_chkCopyFromWin").removeAttr("disabled");
            }
            if(Id.indexOf("151_41")!=-1) {
                $("#chkCopyAllSetting").removeAttr("disabled");
            }
        }
    }
    else {
        $("#" + Id + "_" + spositiontaking_auto).attr("disabled", "disabled");
        $("#" + Id + "_" + sposition).removeAttr("disabled");
        if (($("#" + Id + "_chkCopyFromWin").attr("old") == 'disabled' || $("#" + Id + "_chkCopyFromWin").attr("isuplineautopt").toString() == 'true')) {
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
                    $("#" + placeId + "_" + sposition).attr("disabled", "disabled");
                    $("#" + placeId + "_" + spositiontaking_isauto).attr("disabled", "disabled");
                    $("#" + placeId + "_" + spositiontaking_auto).attr("disabled", "disabled");
                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");
                }
                else {
                    $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");
                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
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
                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");
                    $("#" + winId + "_" + sposition).attr("disabled", "disabled");
                   
                    $("#" + winId + "_" + spositiontaking_isauto).attr("disabled", "disabled");
                    $("#" + winId + "_" + spositiontaking_auto).attr("disabled", "disabled");
                    
                    $("#" + placeId + "_" + sposition).attr("disabled", "disabled");
                  
                    $("#" + placeId + "_" + spositiontaking_isauto).attr("disabled", "disabled");
                    $("#" + placeId + "_" + spositiontaking_auto).attr("disabled", "disabled");
                }
                else {
                    if ($("#" + winId + "_" + chkCopyFromWin).attr('old') != 'disabled' || $("#" + winId + "_" + spositiontaking_isauto).attr("checked")) {
                        $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");
                    }
                   
                    $("#" + winId + "_" + spositiontaking_isauto).removeAttr("disabled");

                    if ($("#" + winId + "_" + spositiontaking_isauto).attr("checked")) {
                        $("#" + winId + "_" + spositiontaking_auto).removeAttr("disabled");
                    } else {
                        $("#" + winId + "_" + sposition).removeAttr("disabled");
                    }

                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
                    
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
