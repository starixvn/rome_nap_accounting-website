var newLic = {};

newLic.init = function () {
    $("#tblAdd").click(function () {
        newLic.submit();
    });

    $("#selCurrency").unbind("change");
    $("#selCurrency").change(function () {
        newLic.GenerateSampleName();
    });

    $("#btnResetUsenName").unbind("click");
    $("#btnResetUsenName").click(function () {
        newLic.ResetUsenName();
    });

    if (window.kenoSettingsManager) {
        window.kenoSettingsManager.initPage();
    }
};

newLic.verifyData = function () {
    var superName = $("#tbxSuperName").val();
    if (common.isEmpty(superName)) {
        common.showErr("Please input Super's Username!");
        return false;
    }

    if (!common.validateUserName(superName)) {
        common.showErr("Super's Username is invalid!");
        return false;
    }

    var masterName = $("#tbxMasterName").val();
    if (common.isEmpty(masterName)) {
        common.showErr("Please input Master's Username!");
        return false;
    }

    if (!common.validateUserName(masterName)) {
        common.showErr("Master's Username is invalid!");
        return false;
    }

    var agentName = $("#tbxAgentName").val();
    if (common.isEmpty(agentName)) {
        common.showErr("Please input Agent's Username!");
        return false;
    }

    if (!common.validateUserName(agentName)) {
        common.showErr("Agent's Username is invalid!");
        return false;
    }

    var dummyMasterName = $("#tbxDummyMasterName").val();
    if (common.isEmpty(dummyMasterName)) {
        common.showErr("Please input Dummy Master's Username!");
        return false;
    }

    if (!common.validateUserName(dummyMasterName)) {
        common.showErr("Dummy Master's Username is invalid!");
        return false;
    }

    var dummyAgentName = $("#tbxDummyAgentName").val();
    if (common.isEmpty(dummyAgentName)) {
        common.showErr("Please input Dummy Agent's Username!");
        return false;
    }

    if (!common.validateUserName(dummyAgentName)) {
        common.showErr("Dummy Agent's Username is invalid!");
        return false;
    }

    var dummyMemberName = $("#tbxDummyMemberName").val();
    if (common.isEmpty(dummyMemberName)) {
        common.showErr("Please input Dummy Member's Username!");
        return false;
    }

    var dummyMemberNameList = dummyMemberName.split(',');
    if (!common.validateUserName(dummyMemberName) ||
         $.inArray("", dummyMemberNameList) != -1) {
        common.showErr("Dummy Member's Usernames is invalid!");
        return false;
    }

    return true;
};

newLic.assignSportbookSettingAndPT = function (params) {
    for (var i = 0; i < window.sports.length; ++i) {
        var bettypes = window.sports[i].Bettype;
        var id = window.sports[i].Id;
        params.push({
            Name: "minbetv" + id
            , Value: $("#" + "minbetv" + id).getNumberValue()
        });

        params.push({
            Name: "maxbetv" + id
            , Value: $("#" + "maxbetv" + id).getNumberValue()
        });

        params.push({
            Name: "maxpermatchv" + id
            , Value: $("#" + "maxpermatchv" + id).getNumberValue()
        });

        if (id.toString().toLowerCase() == "99mp") {
            continue;
        }
        //sportbook position taking
        if (bettypes.length > 0) {
            for (var l = 0; l < bettypes.length; l++) {
                params.push({
                    Name: "v" + id + "spositiontaking_" + bettypes[l]
                    , Value: $("#selSBPT").val()
                });
                params.push({
                    Name: "v" + id + "mpositiontaking_" + bettypes[l]
                    , Value: $("#selSBPT").val()
                });
                params.push({
                    Name: "v" + id + "apositiontaking_" + bettypes[l]
                    , Value: $("#selSBPT").val()
                });
                if (id == "1") {
                    params.push({
                        Name: "v" + id + "slivepositiontaking_" + bettypes[l]
                        , Value: $("#selSBPT").val()
                    });
                    params.push({
                        Name: "v" + id + "mlivepositiontaking_" + bettypes[l]
                        , Value: $("#selSBPT").val()
                    });
                    params.push({
                        Name: "v" + id + "alivepositiontaking_" + bettypes[l]
                        , Value: $("#selSBPT").val()
                    });
                }
            }
        }
    }
};

newLic.assignNumberGameSetting = function (params) {
    params.push({
        Name: "minbetv161"
        , Value: $("#minbetv161").getNumberValue()
    });
    params.push({
        Name: "maxbetv161"
        , Value: $("#maxbetv161").getNumberValue()
    });
    params.push({
        Name: "maxpermatchv161"
        , Value: $("#maxpermatchv161").getNumberValue()
    });
    params.push({
        Name: "maxperballv161"
        , Value: $("#maxperballv161").getNumberValue()
    });
};

newLic.assignNumberGamePT = function (params) {
    var id = 161;
    var arr = ["s", "m", "a"];
    for (var i = 0; i < arr.length; i++) {
        var type = arr[i];

        var bettypes = window.arrNumberGameBettypes;

        for (var j = 0; j < bettypes.length; j++) {
            params.push({
                Name: "v" + id + type + "positiontaking_" + bettypes[j]
                , Value: $("#selNGPT").val()
            });
        }

        for (var k = 0; k < bettypes.length; k++) {
            params.push({
                Name: "v" + id + type + "livepositiontaking_" + bettypes[k]
                , Value: $("#selNGPT").val()
            });
        }
    }
};

newLic.assignRacingBetSettingComPT = function (params) {
    for (var j = 0; j < window.arrRacing.length; ++j) {
        var racing = window.arrRacing[j];
        for (var k = 0; k < racing.totes.length; ++k) {
            var tote = { Id: racing.totes[k].Id, Bettype: racing.totes[k].Bettype };

            //bet setting
            params.push({
                Name: "v" + racing.Id + "minbet_" + tote.Id
                , Value: $("#v" + racing.Id + "minbet_" + tote.Id).getNumberValue()
            });
            params.push({
                Name: "v" + racing.Id + "maxbet_" + tote.Id
                , Value: $("#v" + racing.Id + "maxbet_" + tote.Id).getNumberValue()
            });
            params.push({
                Name: "v" + racing.Id + "maxpermatch_" + tote.Id
                , Value: $("#v" + racing.Id + "maxpermatch_" + tote.Id).getNumberValue()
            });

            for (var i = 0; i < tote.Bettype.length; i++) {
                //commision
                params.push({
                    Name: "v" + racing.Id + "discount_" + tote.Bettype[i]
                    , Value: $("#v" + racing.Id + "discount_" + tote.Id).val()
                });

                //win place pt
                params.push({
                    Name: "v" + racing.Id + "spositiontaking_" + tote.Bettype[i]
                    , Value: $("#v" + racing.Id + "spositiontaking_" + tote.Id).val()
                });

                params.push({
                    Name: "v" + racing.Id + "mpositiontaking_" + tote.Bettype[i]
                    , Value: $("#v" + racing.Id + "spositiontaking_" + tote.Id).val()
                });

                params.push({
                    Name: "v" + racing.Id + "apositiontaking_" + tote.Bettype[i]
                    , Value: $("#v" + racing.Id + "spositiontaking_" + tote.Id).val()
                });

                //house auto pt
                params.push({
                    Name: "v" + racing.Id + "ibcpositiontaking_" + tote.Bettype[i]
                    , Value: $("#v" + racing.Id + "ibcpositiontaking_" + tote.Id).val()
                });
            }

            //enable
            params.push({
                Name: "disable" + racing.Id + "_" + tote.Id
                , Value: !$("#" + "disable" + racing.Id + "_" + tote.Id).is(":checked")
            });
        }
    }
};

newLic.getPostParam = function () {
    var params = [];
    params.push({
        Name: "creator"
        , Value: $("#hiddenAccID").val()
    });
    params.push({
        Name: "site"
        , Value: $("#selSites").find("option[value='" + $("#selSites").val() + "']").html().replace(/ /g, '')
    });
    params.push({
        Name: "currency"
        , Value: $("#selCurrency").val()
    });

    //sportbook bet setting, postion taking
    this.assignSportbookSettingAndPT(params);

    //numbergame bet setting
    this.assignNumberGameSetting(params);

    //numbergame position taking
    this.assignNumberGamePT(params);

    //racing bet setting, comm & PT setting
    this.assignRacingBetSettingComPT(params);

    //enable number game
    params.push({
        Name: "DisableNumber"
        , Value: !$("#cbxEnableNumberGame").is(":checked")
    });

    //virtual sports
    if (typeof vsSuperSetting != 'undefined') {
        vsSuperSetting.assignVirtualSportsSettings(params);
    }

    if (window.kenoSettingsManager) {
        window.kenoSettingsManager.getSettings(params);
    }

    return {
        postParam: {
            SuperName: $("#tbxSuperName").val(),
            MasterName: $("#tbxMasterName").val(),
            AgentName: $("#tbxAgentName").val(),
            DummyMasterName: $("#tbxDummyMasterName").val(),
            DummyAgentName: $("#tbxDummyAgentName").val(),
            DummyMemberName: $("#tbxDummyMemberName").val(),
            PostParams: params
        }
    };
};

newLic.submit = function () {
    common.hideDivMsg();

    if (!this.verifyData()) return;

    if (!common.verifyDataContrain()) {
        if (common.errMsg != "") {
            common.showErr(common.errMsg);
        }
        else common.showErr("Data is invalid!");
        return;
    }

    common.blockUI();

    $.ajax({
        url: 'NewLicenseeSubmitHandler',
        type: 'POST',
        dataType: 'json',
        data: $.toJSON(this.getPostParam()),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.SuccessMessageList && data.SuccessMessageList.length > 0) {
                var infoPrefix = "";
                for (var i = 0; i < 17; i++) {
                    infoPrefix += "&nbsp;";
                }

                var msg = "Add new: " + "<br />" + infoPrefix +
                          data.SuccessMessageList.join("<br />" + infoPrefix) +
                          "<br />" + "successfully!";
                showAlertMessage("divSuccess", msg, 0);
                $("#divSuccess").show().find("div:first");
                setTimeout(function () {
                    $("#divSuccess").hide();
                }, 4000);
            } else {
                $("#divSuccess").hide();
            }

            if (parseInt(data.ErrorCode) != 0) {
                common.showErr(data.ErrorMessage);
            }
            else {
                common.resetValues();
            }

            common.unBlock();
        }
    });
};

newLic.GenerateSampleName = function () {
    var siteName = $("#selSites").find("option:selected").html();
    var currencyName = $("#selCurrency").find("option:selected").attr("data-currencyname");

    var superName = $("#tbxSuperName").val();
    var masterName = $("#tbxMasterName").val();
    var agentName = $("#tbxAgentName").val();
    var dummyMasterName = $("#tbxDummyMasterName").val();
    var dummyAgentName = $("#tbxDummyAgentName").val();
    var dummyMemberName = $("#tbxDummyMemberName").val();

    var suffix = "_8";
    if (!common.isEmpty(superName) ||
        !common.isEmpty(masterName) ||
        !common.isEmpty(agentName) ||
        !common.isEmpty(dummyMasterName) ||
        !common.isEmpty(dummyAgentName) ||
        !common.isEmpty(dummyMemberName)) {
        return;
    }

    $("#tbxSuperName").val(siteName + "Super" + currencyName + suffix);
    $("#tbxMasterName").val(siteName + currencyName + suffix);
    $("#tbxAgentName").val(siteName + currencyName + "01" + suffix);

    $("#tbxDummyMasterName").val(siteName + "ACC" + currencyName + suffix);
    $("#tbxDummyAgentName").val(siteName + "ACC" + currencyName + "01" + suffix);

    var dummyMemberName = siteName + "Transfer" + currencyName + suffix + "," +
                          siteName + "Charges" + currencyName + suffix + "," +
                          siteName + "Bonus" + currencyName + suffix + "," +
                          siteName + "Withdrawal" + currencyName + suffix;
    $("#tbxDummyMemberName").val(dummyMemberName);
};

newLic.ResetUsenName = function () {
    $("#tbxSuperName").val('');
    $("#tbxMasterName").val('');
    $("#tbxAgentName").val('');

    $("#tbxDummyMasterName").val('');
    $("#tbxDummyAgentName").val('');
    $("#tbxDummyMemberName").val('');
};

$(document).ready(function () {
    newLic.init();
});
