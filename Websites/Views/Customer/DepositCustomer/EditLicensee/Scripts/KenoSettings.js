var KenoSettingsManager = function () {
    var self = this;

    self.domElements = {
        groupTab: null,
        sportsContainer: null,
        buttonCopySetting: null,

        sourceMinBetSettings: null,
        targetMinBetSettings: null,
        sourceMaxBetSettings: null,
        targetMaxBetSettings: null,
        sourceMaxPerMatchSettings: null,
        targetMaxPerMatchSettings: null,

        tableBetSetting: null
    };
    self.cachedData = {
        custSettings: null
    };

    self.initPage = function () {
        self.cacheElements();
        self.renderDropdownPT();
        self.renderBetSettings(window.dataJson);
        self.bindEvent();
    };
    self.cacheElements = function () {
        self.domElements.groupTab = $("#table-kenosport");
        self.domElements.sportsContainer = $("#div-kenosport-container");
        self.domElements.tableBetSetting = $("#div-kenosport-container .table-kenosport-betsetting");

        self.domElements.dropdownPT = $("#div-kenosport-container .dropdown-pt");
    };
    self.bindEvent = function () {
        self.domElements.sportsContainer.find("input[type='text']").ForceDecimalOnly().ForceCurrencyOnly();

        self.domElements.groupTab.click(function () {
            self.domElements.sportsContainer.slideToggle("fast", function () {
                self.showHideSportsContainer();
            });
        });

        self.domElements.buttonCopySetting.click(function () {
            self.enableCopySetting();
        });
    };

    self.getSettingInfoDictionary = function (custSettings) {
        var settingInfoDictionary = {};
        var kenoBetTypesLength = custSettings.KenoBetTypesSettingInfo.length;
        for (var i = 0; i < kenoBetTypesLength; i++) {
            var betType = custSettings.KenoBetTypesSettingInfo[i];
            var settingLength = betType.SettingInfo.length;
            for (var j = 0; j < settingLength; j++) {
                var settingInfo = betType.SettingInfo[j];
                settingInfoDictionary[settingInfo.Name] = settingInfo.Value;
            }
        }

        return settingInfoDictionary;
    },
    self.renderBetSettings = function (custSettings) {
        self.cachedData.custSettings = custSettings;
        var settingInfoDictionary = self.getSettingInfoDictionary(custSettings);
        self.domElements.tableBetSetting.html('');
        var htmlTilte =
            '<tr class="title">\
                <td colspan="4" class="headercolumn">\
                    <span>Bet Setting</span>\
                </td>\
            </tr>\
            <tr class="title">\
                <td class="headercolumn"></td>\
                <td class="headercolumn">\
                    <span>Min Bet</span>\
                </td>\
                <td class="headercolumn">\
                    <span>Max Bet</span>\
                </td>\
                <td class="headercolumn">\
                    <span>Max Per Match</span>\
                </td>\
            </tr>';
        self.domElements.tableBetSetting.append(htmlTilte);

        var kenoBetTypesLength = custSettings.KenoBetTypesSettingInfo.length;
        var bodyHtml = '';
        for (var i = 0; i < kenoBetTypesLength; i++) {
            var betType = custSettings.KenoBetTypesSettingInfo[i];
            var minBetv = "minbetv" + betType.Id;
            var maxBetv = "maxbetv" + betType.Id;
            var maxPerMatchv = "maxpermatchv" + betType.Id;

            var minBetVal = parseFloat(settingInfoDictionary[minBetv]).formatMoneyFromVal();
            var maxBetVal = parseFloat(settingInfoDictionary[maxBetv]).formatMoneyFromVal();
            var maxPerMacthVal = parseFloat(settingInfoDictionary[maxPerMatchv]).formatMoneyFromVal();

            var isFirstSport = i == 0;
            var buttonCopyAll = isFirstSport ? "<span><input type=\"checkbox\" class=\"button-copysetting\" /></span>" : "";
            var copyDirection = isFirstSport ? "source=\"true\"" : "target=\"true\"";

            bodyHtml += '<tr>\
                            <td class="bettypename">\
                                <span>' + betType.Name + '</span>' + buttonCopyAll +
                            '</td>\
                            <td>\
                                <input id="' + minBetv + '"' + copyDirection + ' minbet="true" type="text" class="minbet" value="' + minBetVal + '" />\
                            </td>\
                            <td>\
                                <input id="' + maxBetv + '"' + copyDirection + ' maxbet="true" type="text" class="maxbet" value="' + maxBetVal + '" />\
                            </td>\
                            <td>\
                                <input id="' + maxPerMatchv + '"' + copyDirection + ' maxpermatch="true" type="text" class="maxpermatch" value="' + maxPerMacthVal + '" />\
                            </td>\
                        </tr>';
        }

        self.domElements.tableBetSetting.append(bodyHtml);

        var positionTakingValue = settingInfoDictionary["v180spositiontaking"] ?
            parseFloat(settingInfoDictionary["v180spositiontaking"]).formatMoneyFromVal() :
            0;
        self.domElements.dropdownPT.removeAttr("selected").find("option[value='" + positionTakingValue + "']").attr("selected", "selected");

        self.domElements.buttonCopySetting = $("#div-kenosport-container .button-copysetting");
        self.domElements.sourceMinBetSettings = self.domElements.sportsContainer.find("input[type='text'][minbet][source]");
        self.domElements.targetMinBetSettings = self.domElements.sportsContainer.find("input[type='text'][minbet][target]");
        self.domElements.sourceMaxBetSettings = self.domElements.sportsContainer.find("input[type='text'][maxbet][source]");
        self.domElements.targetMaxBetSettings = self.domElements.sportsContainer.find("input[type='text'][maxbet][target]");
        self.domElements.sourceMaxPerMatchSettings = self.domElements.sportsContainer.find("input[type='text'][maxpermatch][source]");
        self.domElements.targetMaxPerMatchSettings = self.domElements.sportsContainer.find("input[type='text'][maxpermatch][target]");
    };

    self.showHideSportsContainer = function () {
        var isCurrentlyHide = self.domElements.sportsContainer.hasClass(":visible");
        if (isCurrentlyHide) {
            self.domElements.groupTab.find("td[arrow]").removeClass("md3").addClass("md2");
        }
        else {
            self.domElements.groupTab.find("td[arrow]").removeClass("md2").addClass("md3");
        }
    };
    self.enableCopySetting = function () {
        var isCopySetting = self.domElements.buttonCopySetting.is(":checked");
        if (isCopySetting) {
            self.copyMinBetSetting(self.domElements.sourceMinBetSettings.val());
            self.copyMaxBetSetting(self.domElements.sourceMaxBetSettings.val());
            self.copyMaxPerMatchSetting(self.domElements.sourceMaxPerMatchSettings.val());

            self.domElements.targetMinBetSettings.attr("disabled", "disabled");
            self.domElements.targetMaxBetSettings.attr("disabled", "disabled");
            self.domElements.targetMaxPerMatchSettings.attr("disabled", "disabled");

            self.domElements.sourceMinBetSettings.keyup(function () {
                self.copyMinBetSetting($(this).val());
            });

            self.domElements.sourceMaxBetSettings.keyup(function () {
                self.copyMaxBetSetting($(this).val());
            });

            self.domElements.sourceMaxPerMatchSettings.keyup(function () {
                self.copyMaxPerMatchSetting($(this).val());
            });
        } else {
            self.domElements.sourceMinBetSettings.unbind("keyup");
            self.domElements.sourceMaxBetSettings.unbind("keyup");
            self.domElements.sourceMaxPerMatchSettings.unbind("keyup");

            self.domElements.targetMinBetSettings.attr("disabled", null);
            self.domElements.targetMaxBetSettings.attr("disabled", null);
            self.domElements.targetMaxPerMatchSettings.attr("disabled", null);
        }
    };
    self.copyMinBetSetting = function (minBet) {
        self.domElements.targetMinBetSettings.val(minBet);
    };
    self.copyMaxBetSetting = function (maxBet) {
        self.domElements.targetMaxBetSettings.val(maxBet);
    };
    self.copyMaxPerMatchSetting = function (maxPerMatch) {
        self.domElements.targetMaxPerMatchSettings.val(maxPerMatch);
    };
    self.renderDropdownPT = function () {
        self.domElements.dropdownPT.empty().append(self.getPTOptions());
    };
    self.getPTOptions = function () {
        var options = "";

        var pt = 0.9;
        var val;
        while (pt > 0) {
            val = parseFloat(pt.toFixed(2));
            options += "<option value='" + val + "'>" + val + "</option>";
            pt = pt - 0.01;
        }

        options += "<option selected='selected' value='0'>0</option>";
        return options;
    };

    self.verifyDataContrains = function (valid, job, setMsg1, setMsg2, setMsg3, setMsg4, setMsg5, setMsg6, setMsg7, setMsg8, setMsg9, setMsg10, setMsg11) {
        self.domElements.sportsContainer.find("input[type='text']").css("border-color", "").each(function () {
            var currentVal = parseFloat($(this).getNumberValue());
            if (valid) {
                if (common.isEmpty($(this).val()) ||
                    !common.isNumber($(this).getNumberValue()) ||
                    currentVal < 0 ||
                    currentVal > common.moneyMax) {
                    job(this);
                    return false;
                }

                var minBet, maxBet, maxPerMatch;
                if ($(this).attr("minbet") != null &&
                    typeof $(this).attr("minbet") != "undefined") {
                    maxBet = parseFloat($(this).parents("tr:first").find("input[maxbet]").getNumberValue());

                    if (isNaN(maxBet)) {
                        return false;
                    }

                    maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                    if (isNaN(maxPerMatch)) {
                        return false;
                    }

                    if (currentVal > maxBet) {
                        job(this);
                        setMsg1(maxBet);
                        return false;
                    }

                    if (currentVal > maxPerMatch) {
                        job(this);
                        setMsg11(maxPerMatch);
                        return false;
                    }
                }
                else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined") {
                    maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                    if (isNaN(maxPerMatch)) return false;
                    if (currentVal > maxPerMatch) {
                        job(this);
                        setMsg6(maxPerMatch);
                        return false;
                    }
                }

                if (typeof this.uplineVal == "undefined") {
                    return;
                }

                var uplineMinBet, uplineMaxBet, uplineMaxPermatch;
                uplineMinBet = $(this).parents("tr:first").find("input[minbet]").get(0).uplineVal;
                uplineMaxBet = $(this).parents("tr:first").find("input[maxbet]").get(0).uplineVal;
                uplineMaxPermatch = $(this).parents("tr:first").find("input[maxpermatch]").get(0).uplineVal;
                if (uplineMinBet == 0 && uplineMaxBet == 0 && uplineMaxPermatch == 0) {
                    return; //valid case
                }

                minBet = parseFloat($(this).parents("tr:first").find("input[minbet]").getNumberValue());
                if (isNaN(minBet)) {
                    minBet = 0;
                }

                maxBet = parseFloat($(this).parents("tr:first").find("input[maxbet]").getNumberValue());
                if (isNaN(maxBet)) {
                    maxBet = 0;
                }

                maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                if (isNaN(maxPerMatch)) {
                    maxPerMatch = 0;
                }

                if (minBet == 0 && maxBet == 0 && maxPerMatch == 0) {
                    return; //valid case
                }

                if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined") {
                    if (currentVal < this.uplineVal) {
                        job(this);
                        setMsg2(this.uplineVal);
                        return;
                    }
                }
                else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined") {
                    if (currentVal > this.uplineVal) {
                        job(this);
                        setMsg3(this.uplineVal);
                        return;
                    }
                }
                else if ($(this).attr("maxpermatch") != null && typeof $(this).attr("maxpermatch") != "undefined") {
                    if (currentVal > this.uplineVal) {
                        job(this);
                        setMsg4(this.uplineVal);
                        return;
                    }
                }
            }
        });
    };
    self.resetValues = function () {
        self.domElements.sportsContainer.find("input[type='text']").val("0");
        self.domElements.sportsContainer.find("select").find("option[value='0']").attr("selected", "selected");
    };

    self.getBetSettings = function (params, betType) {
        var id = betType.Id;
        params.push({
            Name: "v" + id + "minbet",
            Value: $("#" + "minbetv" + id).getNumberValue()
        });

        params.push({
            Name: "v" + id + "maxbet",
            Value: $("#" + "maxbetv" + id).getNumberValue()
        });

        params.push({
            Name: "v" + id + "maxpermatch",
            Value: $("#" + "maxpermatchv" + id).getNumberValue()
        });
    };
    self.getPT = function (params, betType, positionTaking) {
        var id = betType.Id;
        params.push({
            Name: "v" + id + "spositiontaking",
            Value: positionTaking
        });
        params.push({
            Name: "v" + id + "mpositiontaking",
            Value: positionTaking
        });
        params.push({
            Name: "v" + id + "apositiontaking",
            Value: positionTaking
        });
    };
    self.getSettings = function (params) {
        if (self.cachedData.custSettings == null) {
            return;
        }

        var custSettings = self.cachedData.custSettings;
        var kenoBetTypesLength = custSettings.KenoBetTypesSettingInfo.length;
        var settingInfoList = [];

        for (var i = 0; i < kenoBetTypesLength; i++) {
            var betType = custSettings.KenoBetTypesSettingInfo[i];
            var id = parseInt(betType.Id);
            var kenoBetTypeId = id;
            switch (id) {
                case 180:
                    kenoBetTypeId = 1501;
                    break;
                case 181:
                    kenoBetTypeId = 1503;
                    break;
                case 182:
                    kenoBetTypeId = 1504;
                    break;
                case 183:
                    kenoBetTypeId = 1505;
                    break;
                case 184:
                    kenoBetTypeId = 1507;
                    break;
                case 185:
                    kenoBetTypeId = 1508;
                    break;
                case 186:
                    kenoBetTypeId = 1509;
                    break;
            }

            var betTypeSettings = [];

            settingName = "v" + kenoBetTypeId + "minbet";
            betTypeSettings.push({
                Name: settingName,
                Value: self.domElements.tableBetSetting.find("#" + "minbetv" + id).getNumberValue()
            });

            settingName = "v" + kenoBetTypeId + "maxbet";
            betTypeSettings.push({
                Name: settingName,
                Value: self.domElements.tableBetSetting.find("#" + "maxbetv" + id).getNumberValue()
            });

            settingName = "v" + kenoBetTypeId + "maxpermatch";
            betTypeSettings.push({
                Name: settingName,
                Value: self.domElements.tableBetSetting.find("#" + "maxpermatchv" + id).getNumberValue()
            });

            //Position taking
            var positionTakingValue = self.domElements.dropdownPT.val();

            settingName = "v" + kenoBetTypeId + "spositiontaking";
            betTypeSettings.push({
                Name: settingName,
                Value: positionTakingValue
            });

            settingName = "v" + kenoBetTypeId + "mpositiontaking";
            betTypeSettings.push({
                Name: settingName,
                Value: positionTakingValue
            });

            settingName = "v" + kenoBetTypeId + "apositiontaking";
            betTypeSettings.push({
                Name: settingName,
                Value: positionTakingValue
            });

            settingInfoList.push({
                Id: kenoBetTypeId,
                SettingInfo: betTypeSettings
            });
        }

        return settingInfoList;
    };

    self.initConstraints = function () {
        self.domElements.sportsContainer = $("#div-kenosport-container");
        self.domElements.sportsContainer.find("input[type='text']").each(function () {
            if ($(this).attr("minbet") != null &&
                typeof $(this).attr("minbet") != "undefined" &&
                this.uplineVal > 0) {
                $(this).parent().find("span").html(">=" + parseFloat(this.uplineVal).formatMoneyFromVal());
            }
            else if ($(this).attr("maxbet") != null &&
                typeof $(this).attr("maxbet") != "undefined" &&
                this.uplineVal > 0) {
                $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
            }
            else if ($(this).attr("maxpermatch") != null &&
                typeof $(this).attr("maxpermatch") != "undefined" &&
                this.uplineVal > 0) {
                $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
            }
        });
    };
};

var kenoSettingsManager = new KenoSettingsManager();