var vsDownlineSettings = {
    roleId: 0,
    roleName: '',
    isUpdateCompleted: false,
    domElements: {
        sportsContainer: null
    },
    getSettingInfoDictionary: function (custSettings) {
        var settingInfoDictionary = {};
        var virtualSportsLength = custSettings.VirtualSportsSettingInfo.length;
        for (var i = 0; i < virtualSportsLength; i++) {
            var sport = custSettings.VirtualSportsSettingInfo[i];
            var settingLength = sport.SettingInfo.length;
            for (var j = 0; j < settingLength; j++) {
                var settingInfo = sport.SettingInfo[j];
                settingInfoDictionary[settingInfo.Name] = settingInfo.Value;
            }
        }

        return settingInfoDictionary;
    },
    getDefaultSettings: function (uplineData) {
        var settingInfoDictionary = vsDownlineSettings.getSettingInfoDictionary(uplineData);
        this.roleId = parseInt($("#hiddenRoleID").val());
        var htmlTilte = '<tr class="title">\
                                    <td colspan="4" align="center">\
                                        <span style="font-weight: bold">Bet Setting</span>\
                                    </td>\
                                </tr>\
                                <tr class="title">\
                                    <td style="width: 100px">&nbsp;\
                                    </td>\
                                    <td style="width: 185px" align="center">\
                                        <span style="font-weight: bold">Min Bet</span>\
                                    </td>\
                                    <td style="width: 185px" align="center">\
                                        <span style="font-weight: bold">Max Bet</span>\
                                    </td>\
                                    <td style="width: 185px" align="center">\
                                        <span style="font-weight: bold">Max Per Match</span>\
                                    </td>\
                                </tr>'
                                ;
        $('#tblSettingLimit tbody').append(htmlTilte);

        //init common setting
        vsCommon.init();
        //re-assign constrain
        var totalSports = window.virtualSports.length;
        for (var i = 0; i < totalSports; i++) {
            var sport = window.virtualSports[i];
            var minBetv = "minbetv" + sport.Id;
            var maxBetv = "maxbetv" + sport.Id;
            var maxPerMatchv = "maxpermatchv" + sport.Id;

            var minBet = parseFloat(settingInfoDictionary[minBetv]);
            var maxBet = parseFloat(settingInfoDictionary[maxBetv]);
            var maxPerMatch = parseFloat(settingInfoDictionary[maxPerMatchv]);
            //current line
            if (minBet == 0) {
                if (this.roleId == 2) {
                    minBet = parseFloat(settingInfoDictionary[minBetv + '_min']);
                }
                else {
                    minBet = parseFloat(settingInfoDictionary[minBetv + '_max']);
                }
            }
            if (maxBet == 0) {
                if (this.roleId == 2) {
                    minBet = parseFloat(settingInfoDictionary[minBetv + '_max']);
                }
                else {
                    maxBet = parseFloat(settingInfoDictionary[maxBetv + '_min']);
                }

            }
            if (maxPerMatch == 0) {
                maxPerMatch = parseFloat(settingInfoDictionary[maxPerMatchv + '_max']);
            }

            var isFirstSport = i == 0;
            var buttonCopyAll = isFirstSport ? "<span><input type=\"checkbox\" class=\"button-copysetting\" title=\"Copy all setting from this\" /></span>" : "";
            var copyDirection = isFirstSport ? "source=\"true\"" : "target=\"true\"";
            var html = '<tr>\
                                <td align="left" style="padding-left: 4px">\
                                    <span>' + sport.Name + '</span>' + buttonCopyAll +
                                '</td>\
                                <td>\
                                    <input id="' + minBetv + '"' + copyDirection + ' minbet="true" type="text"  style="width: 120px; text-align: right" value="' + minBet.formatMoneyFromVal() + '" />\
                                    <span></span>\
                                </td>\
                                <td>\
                                    <input id="' + maxBetv + '"' + copyDirection + ' maxbet="true" type="text"  style="width: 120px; text-align: right" value="' + maxBet.formatMoneyFromVal() + '" />\
                                    <span></span>\
                                </td>\
                                <td>\
                                    <input id="' + maxPerMatchv + '"' + copyDirection + ' maxpermatch="true" type="text"  style="width: 120px; text-align: right" value="' + maxPerMatch.formatMoneyFromVal() + '" />\
                                    <span></span>\
                                </td>\
                            </tr>'
                            ;
            $('#tblSettingLimit tbody').append(html);
            //upline
            if (vsDownlineSettings.roleId == 3) {
                $("#" + minBetv).get(0).uplineVal = parseFloat(settingInfoDictionary[minBetv]);
                $("#" + maxBetv).get(0).uplineVal = parseFloat(settingInfoDictionary[maxBetv]);
                $("#" + maxPerMatchv).get(0).uplineVal = parseFloat(settingInfoDictionary[maxPerMatchv]);

            }
            else {
                $("#" + minBetv).get(0).uplineVal = parseFloat(settingInfoDictionary[minBetv + '_max']);
                $("#" + maxBetv).get(0).uplineVal = parseFloat(settingInfoDictionary[maxBetv + '_min']);

                $("#" + maxPerMatchv).get(0).uplineVal = parseFloat(settingInfoDictionary[maxPerMatchv + '_max']);
            }

            if (minBet < ($("#" + minBetv).get(0).uplineVal)) {
                $("#" + minBetv).val($("#" + minBetv).get(0).uplineVal.formatMoneyFromVal());
            }
            if (maxBet > ($("#" + maxBetv).get(0).uplineVal)) {
                $("#" + maxBetv).val($("#" + maxBetv).get(0).uplineVal.formatMoneyFromVal());
            }
            if (maxPerMatch > ($("#" + maxPerMatchv).get(0).uplineVal)) {
                $("#" + maxPerMatchv).val($("#" + maxPerMatchv).get(0).uplineVal.formatMoneyFromVal());
            }
        }

        //PT
        var uplineValMax, selectedValue;

        var v180Settings = null;
        for (var i = 0; i < uplineData.VirtualSportsSettingInfo.length; i++) {
            var settingInfo = uplineData.VirtualSportsSettingInfo[i];
            if (settingInfo.Id == 180) {
                for (var j = 0; j < settingInfo.SettingInfo.length; j++) {
                    uplineData[settingInfo.SettingInfo[j].Name] = settingInfo.SettingInfo[j].Value;
                }
            }
        }

        if (vsDownlineSettings.roleId == 3) {
            uplineValMax = parseFloat(uplineData["v180spositiontaking"]);
        }
        else if (vsDownlineSettings.roleId == 2) {
            uplineValMax = parseFloat(uplineData["v180mpositiontaking"]);
        }
        else {
            uplineValMax = parseFloat(uplineData["v180apositiontaking"]);
        }

        if (isNaN(uplineValMax)) uplineValMax = 0;
        selectedValue = uplineValMax.formatMoneyFromVal();
        $("#selVirtualSportsPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
        });
        $("#selVirtualSportsPT").removeAttr("selected").find("option[value='" + selectedValue + "']").attr("selected", "selected");

        vsCommon.initConstraint();
        vsDownlineSettings.bindEventCopyAllSetting();
    },
    assignVirtualSportsSettings: function (params) {
        var totalSports = window.virtualSports.length;
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
                    , Value: 0
                });
                params.push({
                    Name: "v" + id + "mpositiontaking"
                    , Value: (vsDownlineSettings.roleId >= 3 ? $("#selVirtualSportsPT").val() : 0)
                });
                params.push({
                    Name: "v" + id + "apositiontaking"
                    , Value: $("#selVirtualSportsPT").val()
                });
            }
        }
    },
    assignVirtualSportsSettingsAndAddNew: function (customerId) {
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
                    , Value: customerId
                });
                params.push({
                    Name: "sporttype"
                    , Value: id
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
                    , Value: 0
                });
                params.push({
                    Name: "mpositiontaking"
                    , Value: (vsDownlineSettings.roleId >= 3 ? $("#selVirtualSportsPT").val() : 0)
                });
                params.push({
                    Name: "apositiontaking"
                    , Value: $("#selVirtualSportsPT").val()
                });

                vsDownlineSettings.addSetting(params);

                if (i == totalSports - 1) {
                    vsDownlineSettings.isUpdateCompleted = true;
                }
            }


        }
    },
    addSetting: function (params) {
        var paramName = 'srecommend';
        var urlRequest = '../VirutalSportsDepositCustomer/AddNewMasterSettings';
        if (this.roleId == 3) {
            paramName = 'srecommend';
            urlRequest = '../VirutalSportsDepositCustomer/AddNewMasterSettings';
            this.roleName = 'master';
        }
        else if (this.roleId == 2) {
            paramName = 'mrecommend';
            urlRequest = '../VirutalSportsDepositCustomer/AddNewAgentSettings';
            this.roleName = 'agent';
        }
        else if (this.roleId == 1) {
            paramName = 'recommend';
            urlRequest = '../VirutalSportsDepositCustomer/AddNewMemberSettings';
            this.roleName = 'member';
        }

        params.push({
            Name: paramName
            , Value: $("#hiddenCustID").val()
        });

        $.ajax({
            url: urlRequest,
            type: 'POST',
            dataType: 'json',
            data: $.toJSON(params),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    common.showErr(data.ErrorMessage);
                    vsDownlineSettings.isSuccess = false;
                    common.unBlock();
                }
                else {
                    if (vsDownlineSettings.isUpdateCompleted) {
                        common.unBlock();
                        common.showSuccess("Add new " + vsDownlineSettings.roleName + ' ' + $("#tbxUserNameAddNew").val() + " successfully!");
                        $("#tbxUserNameAddNew").val("");
                    }
                }
            },
            error: function () {
                common.unBlock();
            }
        });

    },
    updateMultipleSports: function (customerId) {
        this.isUpdateCompleted = false;
        this.assignVirtualSportsSettingsAndAddNew(customerId);
    },
    bindEventCopyAllSetting: function () {
        var sportsContainer = $("#divVirtualSportsContainer");
        sportsContainer.find("input[type='text']").ForceDecimalOnly().ForceCurrencyOnly();
        vsDownlineSettings.domElements.sportsContainer = sportsContainer;
        vsDownlineSettings.domElements.buttonCopySetting = sportsContainer.find(".button-copysetting");
        vsDownlineSettings.domElements.sourceMinBetSettings = sportsContainer.find("input[type='text'][minbet][source]");
        vsDownlineSettings.domElements.targetMinBetSettings = sportsContainer.find("input[type='text'][minbet][target]");
        vsDownlineSettings.domElements.sourceMaxBetSettings = sportsContainer.find("input[type='text'][maxbet][source]");
        vsDownlineSettings.domElements.targetMaxBetSettings = sportsContainer.find("input[type='text'][maxbet][target]");
        vsDownlineSettings.domElements.sourceMaxPerMatchSettings = sportsContainer.find("input[type='text'][maxpermatch][source]");
        vsDownlineSettings.domElements.targetMaxPerMatchSettings = sportsContainer.find("input[type='text'][maxpermatch][target]");
        vsDownlineSettings.domElements.buttonCopySetting.click(function () {
            vsDownlineSettings.enableCopySetting();
        });
    },
    enableCopySetting: function () {
        var isCopySetting = vsDownlineSettings.domElements.buttonCopySetting.is(":checked");
        if (isCopySetting) {
            vsDownlineSettings.copyMinBetSetting(vsDownlineSettings.domElements.sourceMinBetSettings.val());
            vsDownlineSettings.copyMaxBetSetting(vsDownlineSettings.domElements.sourceMaxBetSettings.val());
            vsDownlineSettings.copyMaxPerMatchSetting(vsDownlineSettings.domElements.sourceMaxPerMatchSettings.val());

            vsDownlineSettings.domElements.targetMinBetSettings.attr("disabled", "disabled");
            vsDownlineSettings.domElements.targetMaxBetSettings.attr("disabled", "disabled");
            vsDownlineSettings.domElements.targetMaxPerMatchSettings.attr("disabled", "disabled");

            vsDownlineSettings.domElements.sourceMinBetSettings.keyup(function () {
                vsDownlineSettings.copyMinBetSetting($(this).val());
            });

            vsDownlineSettings.domElements.sourceMaxBetSettings.keyup(function () {
                vsDownlineSettings.copyMaxBetSetting($(this).val());
            });

            vsDownlineSettings.domElements.sourceMaxPerMatchSettings.keyup(function () {
                vsDownlineSettings.copyMaxPerMatchSetting($(this).val());
            });
        } else {
            vsDownlineSettings.domElements.sourceMinBetSettings.unbind("keyup");
            vsDownlineSettings.domElements.sourceMaxBetSettings.unbind("keyup");
            vsDownlineSettings.domElements.sourceMaxPerMatchSettings.unbind("keyup");

            vsDownlineSettings.domElements.targetMinBetSettings.attr("disabled", null);
            vsDownlineSettings.domElements.targetMaxBetSettings.attr("disabled", null);
            vsDownlineSettings.domElements.targetMaxPerMatchSettings.attr("disabled", null);
        }
    },
    copyMinBetSetting: function (minBet) {
        vsDownlineSettings.domElements.targetMinBetSettings.val(minBet);
    },
    copyMaxBetSetting: function (maxBet) {
        vsDownlineSettings.domElements.targetMaxBetSettings.val(maxBet);
    },
    copyMaxPerMatchSetting: function (maxPerMatch) {
        vsDownlineSettings.domElements.targetMaxPerMatchSettings.val(maxPerMatch);
    }
};

$(document).ready(function () {
    vsDownlineSettings.getDefaultSettings(window.dataJson);
});