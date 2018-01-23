var vsSuperSetting = {
    isUpdateCompleted: false,
    assignVirtualSportsSettings: function (params) {
        var totalSports = window.virtualSports.length;
        params.push({
            Name: "disablevirtualsports",
            Value: !$("#cbxEnableVirtualSports").is(":checked")
        });

        if (totalSports > 0) {
            for (var i = 0; i < totalSports; ++i) {
                var sport = window.virtualSports[i];
                var bettypes = sport.Bettype;
                var id = sport.Id;

                params.push({
                    Name: "v" + id + "minbet"
                    , Value: $("#" + "minbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "v" + id + "maxbet"
                    , Value: $("#" + "maxbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "v" + id + "maxpermatch"
                    , Value: $("#" + "maxpermatchv" + id).getNumberValue()
                });

                //VS position taking
                params.push({
                    Name: "v" + id + "spositiontaking"
                    , Value: $("#selVirtualSportsPT").val()
                });
                params.push({
                    Name: "v" + id + "mpositiontaking"
                    , Value: $("#selVirtualSportsPT").val()
                });
                params.push({
                    Name: "v" + id + "apositiontaking"
                    , Value: $("#selVirtualSportsPT").val()
                });
            }
        }
    },
    assignVirtualSportsSettingsAndUpdate: function (superId, masterId, agentId) {
        var totalSports = window.virtualSports.length;
        if (totalSports > 0) {
            for (var i = 0; i < totalSports; ++i) {
                var sport = window.virtualSports[i];
                var bettypes = sport.Bettype;
                var id = sport.Id;
                var params = [];

                params.push({
                    Name: "creator"
                , Value: $("#hiddenAccID").val()
                });
                params.push({
                    Name: "custid"
                , Value: superId
                });
                params.push({
                    Name: "masterid"
                , Value: masterId
                });
                params.push({
                    Name: "agentid"
                , Value: agentId
                });
                params.push({
                    Name: "sporttype"
                , Value: id
                });
                params.push({
                    Name: "disablevirtualsports"
            , Value: !$("#cbxEnableVirtualSports").is(":checked")
                });

                params.push({
                    Name: "minbet"
            , Value: $("#" + "minbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "maxbet"
            , Value: $("#" + "maxbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "maxpermatch"
            , Value: $("#" + "maxpermatchv" + id).getNumberValue()
                });

                //VS position taking
                params.push({
                    Name: "spositiontaking"
            , Value: $("#selVirtualSportsPT").val()
                });
                params.push({
                    Name: "mpositiontaking"
            , Value: $("#selVirtualSportsPT").val()
                });
                params.push({
                    Name: "apositiontaking"
            , Value: $("#selVirtualSportsPT").val()
                });

                if (i == (totalSports - 1))
                    vsSuperSetting.isUpdateCompleted = true;
                vsSuperSetting.updateSetting(params);
            }


        }
    },
    updateSetting: function (params) {
        $.ajax({
            url: '../VirutalSportsDepositCustomer/AddNewSuperSettings',
            type: 'POST',
            dataType: 'json',
            data: $.toJSON(params),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    common.showErr(data.ErrorMessage);
                    common.unBlock();
                }
                else {
                    if (vsSuperSetting.isUpdateCompleted) {
                        var msg = "Add new super: " + $("#tbxSuperName").val();
                        if ($("#tbxMasterName").val() != "") {
                            msg += ", master: " + $("#tbxMasterName").val();
                        }

                        if ($("#tbxAgentName").val() != "") {
                            msg += ", agent: " + $("#tbxAgentName").val();
                        }

                        msg += " successfully!";

                        common.unBlock();
                        common.showSuccess(msg);
                        common.resetValues();
                    }
                }
            },
            error: function () {
                common.unBlock();
            }
        });
    },
    updateMultipleSports: function (superId, masterId, agentId) {
        this.isUpdateCompleted = false;
        this.assignVirtualSportsSettingsAndUpdate(superId, masterId, agentId);
    },

    domElements: {
        groupTab: null,
        sportsContainer: null,
        buttonCopySetting: null,

        sourceMinBetSettings: null,
        targetMinBetSettings: null,
        sourceMaxBetSettings: null,
        targetMaxBetSettings: null,
        sourceMaxPerMatchSettings: null,
        targetMaxPerMatchSettings: null
    },
    init: function () {
        vsSuperSetting.cacheElements();
        vsSuperSetting.bindEvent();
    },
    cacheElements: function () {
        vsSuperSetting.domElements.groupTab = $("#tblTabVirtualSports");
        vsSuperSetting.domElements.sportsContainer = $("#divVirtualSportsContainer");
        vsSuperSetting.domElements.buttonCopySetting = $("#divVirtualSportsContainer .button-copysetting");

        vsSuperSetting.domElements.sourceMinBetSettings = vsSuperSetting.domElements.sportsContainer.find("input[type='text'][minbet][source]");
        vsSuperSetting.domElements.targetMinBetSettings = vsSuperSetting.domElements.sportsContainer.find("input[type='text'][minbet][target]");
        vsSuperSetting.domElements.sourceMaxBetSettings = vsSuperSetting.domElements.sportsContainer.find("input[type='text'][maxbet][source]");
        vsSuperSetting.domElements.targetMaxBetSettings = vsSuperSetting.domElements.sportsContainer.find("input[type='text'][maxbet][target]");
        vsSuperSetting.domElements.sourceMaxPerMatchSettings = vsSuperSetting.domElements.sportsContainer.find("input[type='text'][maxpermatch][source]");
        vsSuperSetting.domElements.targetMaxPerMatchSettings = vsSuperSetting.domElements.sportsContainer.find("input[type='text'][maxpermatch][target]");
    },
    bindEvent: function () {
        vsSuperSetting.domElements.buttonCopySetting.click(function () {
            vsSuperSetting.enableCopySetting();
        });
    },   
    enableCopySetting: function () {
        var isCopySetting = vsSuperSetting.domElements.buttonCopySetting.is(":checked");
        if (isCopySetting) {
            vsSuperSetting.copyMinBetSetting(vsSuperSetting.domElements.sourceMinBetSettings.val());
            vsSuperSetting.copyMaxBetSetting(vsSuperSetting.domElements.sourceMaxBetSettings.val());
            vsSuperSetting.copyMaxPerMatchSetting(vsSuperSetting.domElements.sourceMaxPerMatchSettings.val());

            vsSuperSetting.domElements.targetMinBetSettings.attr("disabled", "disabled");
            vsSuperSetting.domElements.targetMaxBetSettings.attr("disabled", "disabled");
            vsSuperSetting.domElements.targetMaxPerMatchSettings.attr("disabled", "disabled");

            vsSuperSetting.domElements.sourceMinBetSettings.keyup(function () {
                vsSuperSetting.copyMinBetSetting($(this).val());
            });

            vsSuperSetting.domElements.sourceMaxBetSettings.keyup(function () {
                vsSuperSetting.copyMaxBetSetting($(this).val());
            });

            vsSuperSetting.domElements.sourceMaxPerMatchSettings.keyup(function () {
                vsSuperSetting.copyMaxPerMatchSetting($(this).val());
            });
        } else {
            vsSuperSetting.domElements.sourceMinBetSettings.unbind("keyup");
            vsSuperSetting.domElements.sourceMaxBetSettings.unbind("keyup");
            vsSuperSetting.domElements.sourceMaxPerMatchSettings.unbind("keyup");

            vsSuperSetting.domElements.targetMinBetSettings.attr("disabled", null);
            vsSuperSetting.domElements.targetMaxBetSettings.attr("disabled", null);
            vsSuperSetting.domElements.targetMaxPerMatchSettings.attr("disabled", null);
        }
    },
    copyMinBetSetting: function (minBet) {
        vsSuperSetting.domElements.targetMinBetSettings.val(minBet);
    },
    copyMaxBetSetting: function (maxBet) {
        vsSuperSetting.domElements.targetMaxBetSettings.val(maxBet);
    },
    copyMaxPerMatchSetting: function (maxPerMatch) {
        vsSuperSetting.domElements.targetMaxPerMatchSettings.val(maxPerMatch);
    }
};

$(document).ready(function () {
    vsCommon.init();
    vsSuperSetting.init();
});