$(function () {
    var TplSuperMinUpdate = '<table class="tblcollapse w800">\
                            <tr class="title">\
                                <td colspan="3">Position Taking</td>\
                            </tr>\
                            <tr>\
                                <td></td>\
                                <td><b>Win</b></td>\
                                <td><b>Place</b></td>\
                            </tr>\
                            <tr>\
                                <td class="l">Max Position</td>\
                                <td>\
                                    <select onclick="ChangeMaxPT(this);" max="${this.data.Bettypes[0].PositionTaking.MaxUplinePt}" min="${this.data.Bettypes[0].PositionTaking.MinUplinePt}" class="pt" id="${getId(0)}_maxpt" old="${this.data.Bettypes[0].PositionTaking.UplinePt}">\
                                        <option value="${this.data.Bettypes[0].PositionTaking.UplinePt}">${this.data.Bettypes[0].PositionTaking.UplinePt}</option>\
                                    </select>\
                                </td>\
                                <td>\
                                    <select onclick="ChangeMaxPT(this);" max="${this.data.Bettypes[1].PositionTaking.MaxUplinePt}" min="${this.data.Bettypes[1].PositionTaking.MinUplinePt}" class="pt" id="${getId(1)}_maxpt" old="${this.data.Bettypes[0].PositionTaking.UplinePt}">\
                                        <option value="${this.data.Bettypes[1].PositionTaking.UplinePt}">${this.data.Bettypes[1].PositionTaking.UplinePt}</option>\
                                    </select>\
                                </td>\
                            </tr>\
                            <tr>\
                                <td class="l">Min Position</td>\
                                <td>\
                                    <select onclick="ChangeMinPT(this);" max="${this.data.Bettypes[0].PositionTaking.MaxPt}" min="${this.data.Bettypes[0].PositionTaking.MinPt}" class="pt" id="${getId(0)}_minpt" old="${this.data.Bettypes[0].PositionTaking.Pt}">\
                                        <option value="${this.data.Bettypes[0].PositionTaking.Pt}">${this.data.Bettypes[0].PositionTaking.Pt}</option>\
                                    </select>\
                                </td>\
                                <td>\
                                    <select onclick="ChangeMinPT(this);" max="${this.data.Bettypes[1].PositionTaking.MaxPt}" min="${this.data.Bettypes[1].PositionTaking.MinPt}" class="pt" id="${getId(1)}_minpt" old="${this.data.Bettypes[1].PositionTaking.Pt}">\
                                        <option value="${this.data.Bettypes[1].PositionTaking.Pt}">${this.data.Bettypes[1].PositionTaking.Pt}</option>\
                                    </select>\
                                </td>\
                            </tr>\
                            <tr>\
                                <td></td>\
                                <td colspan="2" class="l"><input type="checkbox" onclick="ChkCopyFromWin(this)" id="${getId(0)}_chkCopyFromWin" />&nbsp;Copy from ${this.data.Bettypes[0].Name}</td>\
                            </tr>\
                        </table>';

    $.extend(RacingSetting, { TplRacingTypeSetting: TplSuperMinUpdate });
    RacingData.getCustomerSetting(parseInt(gup("custId")), RacingSetting.init);
});

function ChkCopyFromWin(obj) {
    var IdWin = $(obj).attr("id").split("_")[1];
    var IdPlace = $(obj).attr("id").split("_")[0] + "_" + RacingSetting.getIdPlace(IdWin);

    var isCopied = $(obj).attr("checked");
    if (isCopied) {
        $("#" + IdPlace + "_minpt").attr("disabled", "disabled");
        $("#" + IdPlace + "_maxpt").attr("disabled", "disabled");
    }
    else {
        $("#" + IdPlace + "_minpt").removeAttr("disabled");
        $("#" + IdPlace + "_maxpt").removeAttr("disabled");
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
                    $("#" + placeId + "_minpt").attr("disabled", "disabled");
                    $("#" + placeId + "_maxpt").attr("disabled", "disabled");
                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");
                }
                else {
                    $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");

                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
                        $("#" + placeId + "_minpt").removeAttr("disabled");
                        $("#" + placeId + "_maxpt").removeAttr("disabled");
                    }
                }
            }
            else {
                if (isCopied) {
                    $("#" + winId + "_" + chkCopyFromWin).attr("disabled", "disabled");
                    $("#" + winId + "_minpt").attr("disabled", "disabled");
                    $("#" + placeId + "_minpt").attr("disabled", "disabled");
                    $("#" + winId + "_maxpt").attr("disabled", "disabled");
                    $("#" + placeId + "_maxpt").attr("disabled", "disabled");
                }
                else {
                    $("#" + winId + "_" + chkCopyFromWin).removeAttr("disabled");
                    $("#" + winId + "_minpt").removeAttr("disabled");
                    $("#" + winId + "_maxpt").removeAttr("disabled");

                    if ($("#" + winId + "_" + chkCopyFromWin).attr("checked") == false) {
                        $("#" + placeId + "_minpt").removeAttr("disabled");
                        $("#" + placeId + "_maxpt").removeAttr("disabled");
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

function ChangeMaxPT(obj) {
    var IdWin = $(obj).attr("id").split("_")[0] + "_" + $(obj).attr("id").split("_")[1];
    var max = $(obj).val();
    var master_obj = $("#" + IdWin + "_" + "minpt");

    max = Math.min(master_obj.attr("max"), max)
    var curr = Math.min(master_obj.val(), max);
    //master_obj.attr("max", max);
    CreateOption(master_obj, curr, max, 0, 0.1, 2);
}

function ChangeMinPT(obj) {
    var IdWin = $(obj).attr("id").split("_")[0] + "_" + $(obj).attr("id").split("_")[1];
    var min_of_max = $(obj).val();
    var max_obj = $("#" + IdWin + "_" + "maxpt");
    var max_of_max = max_obj.attr("max");
    var cur_of_max = max_obj.val();
    CreateOption(max_obj, cur_of_max , max_of_max, min_of_max, 0.1, 2);
}
