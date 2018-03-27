$(document).ready(function () {
    editLicenseeManager.initPage();
});

var EditLicenseeManager = function () {
    var self = this;
    self.Tables = {
        TabHeaderSportsbookNumberGame: null,
        DivSportsbookNumberGameContainer: null,

        TabVirtualSportsSettings: null,
        DivVirtualSportsContainer: null,

        CheckboxCopyAllSportsbook: null,    

        arrInputsRemainSportMinBet_SB: null,
        arrInputsRemainSportMaxBet_SB: null,
        arrInputsRemainSportMaxperMatch_SB: null,      

        DropdownSportsbookPositionTaking_SbNG: null,
        DropdownNumbergamePositiontaking_SbNG: null,
        DropdownSportsbookPositionTaking_VS: null,

        buttonSearch: null,
        buttonUpdate: null,
        textboxUsername: null,
        resultMessage: null,

        divLicenseeInfo: null,
        tableSiteInfo: null,
        labelLicenseeSite: null,
        labelLicenseeCurrency: null,
        checkboxEnableNumbergame: null,
        checkboxEnableVirtualSports: null
    };

    self.Constants = {
        MaxMonerValue: 912337203685477.5807,

        Sports: {
            NumberGame: 161,
            Soccer: 1
        },

        SMALevelNameConstants: {
            Super: 's',
            Master: 'm',
            Agent: 'a'
        }
    };

    self.CacheData = {
        licenseeInfo: null
    };

    self.Params = {
        getLicenseeInfoParams: {
            LicenseeName: ''
        },

        updateLicenseeInfoParams: {
            LicenseeName: ''
        }
    };

    self.PositionTakingRenderManager = new PositionTakingRenderManager();

    self.CallAjax = function (reportHandler,
        url,
        requestParams,
        successFunction,
        errorFunction,
        completeFunction) {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(requestParams),
            beforeSend: function () {
            },
            success: function (rs) {
                if (successFunction && typeof (successFunction) == 'function') {
                    successFunction.call(reportHandler, rs);
                }
            },
            error: function (jqXHR, event, ajaxSettings) {
                if (errorFunction && typeof (errorFunction) == 'function') {
                    errorFunction.call(reportHandler);
                }
            },
            complete: function () {
                if (completeFunction && typeof (completeFunction) == 'function') {
                    completeFunction();
                }
            }
        });
    };

    self.blockUI = function () {
        if (parent.parent.BlockUI) {
            try { parent.parent.BlockUI(); } catch (e) { }
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
        }
    };

    self.unblockUI = function () {
        if (parent.parent.$ && parent.parent.$.unblockUI) {
            try { parent.parent.$.unblockUI(); } catch (e) { }
        }
        else {
            try { parent.$.unblockUI(); } catch (e) { }
        }
    };

    self.initPage = function () {
        self.cachedDOMElements();
        self.bindingEvent();

        self.PositionTakingRenderManager.renderSportbookPTOptions(self.Tables.DropdownSportsbookPositionTaking_SbNG);
        self.PositionTakingRenderManager.renderNumberGamePTOptions(self.Tables.DropdownNumbergamePositiontaking_SbNG);       
        self.PositionTakingRenderManager.renderSportbookPTOptions(self.Tables.DropdownSportsbookPositionTaking_VS);
    };

    self.cachedDOMElements = function () {
        self.Tables.TabHeaderSportsbookNumberGame = $("#tabheader-sportsbook-numbergame");
        self.Tables.DivSportsbookNumberGameContainer = $("#div-sportsbook-numbergame-container");

        self.Tables.TabVirtualSportsSettings = $("#tab-virtualsports-settings");
        self.Tables.DivVirtualSportsContainer = $("#div-virtualsports-container");

        self.Tables.CheckboxCopyAllSportsbook = $("#checkbox-copyall-sportsbook");
        self.Tables.CheckboxCopyAllRacing = $("#checkbox-copyall-racing");

        self.Tables.arrInputsRemainSportMinBet_SB = self.Tables.DivSportsbookNumberGameContainer.find("input[type='text'][remainsport][minbet]");
        self.Tables.arrInputsRemainSportMaxBet_SB = self.Tables.DivSportsbookNumberGameContainer.find("input[type='text'][remainsport][maxbet]");
        self.Tables.arrInputsRemainSportMaxperMatch_SB = self.Tables.DivSportsbookNumberGameContainer.find("input[type='text'][remainsport][maxpermatch]");

       

        self.Tables.DropdownSportsbookPositionTaking_SbNG = self.Tables.DivSportsbookNumberGameContainer.find("#dropdown-sportsbook-positiontaking");
        self.Tables.DropdownNumbergamePositiontaking_SbNG = self.Tables.DivSportsbookNumberGameContainer.find("#dropdown-numbergame-positiontaking");
        self.Tables.DropdownSportsbookPositionTaking_VS = self.Tables.DivVirtualSportsContainer.find("#dropwdown-virtualsports-positiontaking");

        self.Tables.buttonSearch = $("#button-search");
        self.Tables.buttonUpdate = $("#button-update");
        self.Tables.textboxUsername = $("#textbox-username");
        self.Tables.resultMessage = $("#result-message");
        self.Tables.divLicenseeInfo = $(".div-licensee-info");

        self.Tables.tableSiteInfo = self.Tables.divLicenseeInfo.find(".table-site-info");
        self.Tables.labelLicenseeSite = self.Tables.tableSiteInfo.find(".label-licensee-site");
        self.Tables.labelLicenseeCurrency = self.Tables.tableSiteInfo.find(".label-licensee-currency");
        self.Tables.checkboxEnableNumbergame = self.Tables.tableSiteInfo.find("#checkbox-enable-numbergame");
        self.Tables.checkboxEnableVirtualSports = self.Tables.DivVirtualSportsContainer.find("#checkbox-enable-virtualsports");
    };

    self.bindingEvent = function () {
        self.bindingEvent_SportsbookNumberGame();     

        self.Tables.buttonSearch.unbind("click");
        self.Tables.buttonSearch.click(function () {
            self.GetLicenseeInfo();
        });

        self.Tables.buttonUpdate.unbind("click");
        self.Tables.buttonUpdate.click(function () {
            self.UpdateLicenseeInfo();
        });
    };

    self.bindingEvent_SportsbookNumberGame = function () {
        self.Tables.TabHeaderSportsbookNumberGame.unbind("click");
        self.Tables.TabHeaderSportsbookNumberGame.click(function () {
            self.ShowHide_DivSportsbookNumberGameContainer();
        });

        self.Tables.DivSportsbookNumberGameContainer.find("input[type='text']")
                                                    .ForceDecimalOnly()
                                                    .ForceCurrencyOnly();
        self.Tables.DivSportsbookNumberGameContainer.find("input[type='text'][soccer][minbet]")
            .keyup(function () {
                if (self.Tables.CheckboxCopyAllSportsbook.is(":checked")) {
                    self.Tables.arrInputsRemainSportMinBet_SB.val($(this).val());
                }
            });

        self.Tables.DivSportsbookNumberGameContainer.find("input[type='text'][soccer][maxbet]")
            .keyup(function () {
                if (self.Tables.CheckboxCopyAllSportsbook.is(":checked")) {
                    self.Tables.arrInputsRemainSportMaxBet_SB.val($(this).val());
                }
            });

        self.Tables.DivSportsbookNumberGameContainer.find("input[type='text'][soccer][maxpermatch]")
            .keyup(function () {
                if (self.Tables.CheckboxCopyAllSportsbook.is(":checked")) {
                    self.Tables.arrInputsRemainSportMaxperMatch_SB.val($(this).val());
                }
            });

        self.Tables.CheckboxCopyAllSportsbook.change(function () {
            self.CheckboxCopyAllSportsbookChange();
        });
    };
   
    self.bindingEvent_VirtualSport = function () {
        self.Tables.TabVirtualSportsSettings.unbind("click");
        self.Tables.TabVirtualSportsSettings.click(function () {
            self.Tables.DivVirtualSportsContainer.slideToggle("fast", function () {
                self.ShowHide_DivVirtualSportsContainer();
            });
        });

        self.Tables.TabVirtualSportsSettings.find("input[type='text']")
                                            .ForceDecimalOnly()
                                            .ForceCurrencyOnly();
    };

    self.ShowHide_DivSportsbookNumberGameContainer = function () {
        self.Tables.DivSportsbookNumberGameContainer.slideToggle("fast", function () {
            if (!self.Tables.DivSportsbookNumberGameContainer.is(":visible")) {
                self.Tables.TabHeaderSportsbookNumberGame.find("td[arrow]")
                                                         .removeClass("md2")
                                                         .addClass("md3");
            }
            else {
                self.Tables.TabHeaderSportsbookNumberGame.find("td[arrow]")
                                                         .removeClass("md3")
                                                         .addClass("md2");
            }
        });
    };
   
    self.ShowHide_DivVirtualSportsContainer = function () {
        if (!self.Tables.DivVirtualSportsContainer.is(":visible")) {
            self.Tables.TabVirtualSportsSettings.find("td[arrow]")
                                                        .removeClass("md2")
                                                        .addClass("md3");
        }
        else {
            self.Tables.TabVirtualSportsSettings.find("td[arrow]")
                                                        .removeClass("md3")
                                                        .addClass("md2");
        }
    };   
  
    self.CheckboxCopyAllSportsbookChange = function () {
        if (self.Tables.CheckboxCopyAllSportsbook.is(":checked")) {
            self.Tables.DivSportsbookNumberGameContainer.find("input[remainsport]")
                                                        .attr("disabled", "disabled");
            var soccerMinBet = self.Tables.DivSportsbookNumberGameContainer.find("input[soccer][minbet]").val();
            var soccerMaxBet = self.Tables.DivSportsbookNumberGameContainer.find("input[soccer][maxbet]").val();
            var soccerMaxPerMatch = self.Tables.DivSportsbookNumberGameContainer.find("input[soccer][maxpermatch]").val();
            self.Tables.DivSportsbookNumberGameContainer.find("input[remainsport][minbet]").val(soccerMinBet);
            self.Tables.DivSportsbookNumberGameContainer.find("input[remainsport][maxbet]").val(soccerMaxBet);
            self.Tables.DivSportsbookNumberGameContainer.find("input[remainsport][maxpermatch]").val(soccerMaxPerMatch);
        }
        else {
            self.Tables.DivSportsbookNumberGameContainer.find("input[remainsport]").removeAttr("disabled");
        }
    };

    self.GetLicenseeInfo = function () {
        self.blockUI();
        self.Params.getLicenseeInfoParams.LicenseeName = self.Tables.textboxUsername.val();
        if (self.Params.getLicenseeInfoParams.LicenseeName == '') {
            showAlertMessage("result-message", "Please input licensee name!", 1);
            self.Tables.resultMessage.removeClass("display-none");
            self.unblockUI();
            return;
        }

        self.Tables.resultMessage.empty().addClass("display-none");
        self.CallAjax(self,
            "GetLicenseeInfo",
            self.Params.getLicenseeInfoParams,
            self.RenderLicenseeInfo,
            self.RenderLicenseeInfo,
            self.unblockUI);
    };
    self.UpdateLicenseeInfo = function () {
        self.blockUI();
        self.GetUpdateParams();
        if (!self.ValidateUpdateParams()) {
            self.unblockUI();
            return;
        }

        self.CallAjax(self,
            "UpdateLicenseeInfo",
            self.Params.updateLicenseeInfoParams,
            self.RenderUpdateLicenseeInfoResult,
            self.RenderUpdateLicenseeInfoResult,
            self.unblockUI);
    };

    self.GetUpdateParams = function () {
        self.Params.updateLicenseeInfoParams.LicenseeName = self.Params.getLicenseeInfoParams.LicenseeName;
        self.Params.updateLicenseeInfoParams.Site = self.CacheData.licenseeInfo.Site;
        self.Params.updateLicenseeInfoParams.Currency = self.CacheData.licenseeInfo.Currency;
        self.Params.updateLicenseeInfoParams.EnableNumberGame = self.Tables.checkboxEnableNumbergame.prop("checked");

        self.Params.updateLicenseeInfoParams.SettingInfo = [];
        var sportsbookSettings = self.GetUpdateParams_Sportsbook();
        $.merge(self.Params.updateLicenseeInfoParams.SettingInfo, sportsbookSettings);
        var numberGameSettings = self.GetUpdateParams_NumberGame();
        $.merge(self.Params.updateLicenseeInfoParams.SettingInfo, numberGameSettings);     

        self.Params.updateLicenseeInfoParams.EnableVirtualSports = self.Tables.checkboxEnableVirtualSports.prop("checked");
        self.Params.updateLicenseeInfoParams.VirtualSportsSettingInfo = self.GetUpdateParams_VirtualSports();
    };
    self.GetUpdateParams_Sportsbook = function () {
        var settingInfoList = [];
        var sportId;
        var settingName;
        var sportLength = sportsbookSports.length;
        for (var i = 0; i < sportLength; i++) {
            var sport = sportsbookSports[i];
            sportId = sport.Id.toLowerCase();

            if (sportId == self.Constants.Sports.NumberGame.toString()) {
                continue;
            }

            settingName = "minbetv" + sportId;
            settingInfoList.push({
                Name: settingName,
                Value: $("#" + settingName).getNumberValue()
            });

            settingName = "maxbetv" + sportId;
            settingInfoList.push({
                Name: settingName,
                Value: $("#" + settingName).getNumberValue()
            });

            settingName = "maxpermatchv" + sportId;
            settingInfoList.push({
                Name: settingName,
                Value: $("#" + settingName).getNumberValue()
            });

            if (sportId.toString().toLowerCase() == "99mp") {
                continue;
            }

            var betTypesLength = sport.BetTypes.length;
            for (var j = 0; j < betTypesLength; j++) {
                var betType = sport.BetTypes[j];
                var positionTakingValue = self.Tables.DropdownSportsbookPositionTaking_SbNG.val();
                settingName = "v" + sportId + "spositiontaking_" + betType;
                settingInfoList.push({
                    Name: settingName,
                    Value: positionTakingValue
                });

                settingName = "v" + sportId + "mpositiontaking_" + betType;
                settingInfoList.push({
                    Name: settingName,
                    Value: positionTakingValue
                });

                settingName = "v" + sportId + "apositiontaking_" + betType;
                settingInfoList.push({
                    Name: settingName,
                    Value: positionTakingValue
                });

                if (sportId == self.Constants.Sports.Soccer) {
                    settingName = "v" + sportId + "slivepositiontaking_" + betType;
                    settingInfoList.push({
                        Name: settingName,
                        Value: positionTakingValue
                    });

                    settingName = "v" + sportId + "mlivepositiontaking_" + betType;
                    settingInfoList.push({
                        Name: settingName,
                        Value: positionTakingValue
                    });

                    settingName = "v" + sportId + "alivepositiontaking_" + betType;
                    settingInfoList.push({
                        Name: settingName,
                        Value: positionTakingValue
                    });
                }
            }
        }

        return settingInfoList;
    };
    self.GetUpdateParams_NumberGame = function () {
        var settingInfoList = [];
        var sportId = self.Constants.Sports.NumberGame;
        var settingName;
        settingName = "minbetv" + sportId;
        settingInfoList.push({
            Name: settingName,
            Value: $("#" + settingName).getNumberValue()
        });

        settingName = "maxbetv" + sportId;
        settingInfoList.push({
            Name: settingName,
            Value: $("#" + settingName).getNumberValue()
        });

        settingName = "maxperballv" + sportId;
        settingInfoList.push({
            Name: settingName,
            Value: $("#" + settingName).getNumberValue()
        });

        var levelType = [
            self.Constants.SMALevelNameConstants.Super,
            self.Constants.SMALevelNameConstants.Master,
            self.Constants.SMALevelNameConstants.Agent];

        var levelTypeLength = levelType.length;
        for (var i = 0; i < levelTypeLength; i++) {
            var type = levelType[i];
            var betTypes = numberGameBetTypes;
            var betTypesLength = betTypes.length;
            for (var j = 0; j < betTypesLength; j++) {
                var betType = numberGameBetTypes[j];
                var positionTakingValue = self.Tables.DropdownNumbergamePositiontaking_SbNG.val();
                settingName = "v" + sportId + type + "positiontaking_" + betType;
                settingInfoList.push({
                    Name: settingName,
                    Value: positionTakingValue
                });

                settingName = "v" + sportId + type + "livepositiontaking_" + betType;
                settingInfoList.push({
                    Name: settingName,
                    Value: positionTakingValue
                });
            }
        }

        return settingInfoList;
    };
    
    self.GetUpdateParams_VirtualSports = function () {
        var settingInfoList = [];
        var sportId;
        var settingName;
        var virtualSportsLength = virtualSports.length;
        for (var i = 0; i < virtualSportsLength; i++) {
            var sport = virtualSports[i];
            sportId = sport.Id;
            if (self.Tables.DivVirtualSportsContainer.find(".virtualsports_sport" + sportId).hasClass("display-none")) {
                continue;
            }

            var sportSettings = [];
            settingName = "minbetv" + sportId;
            sportSettings.push({
                Name: settingName,
                Value: $("#" + settingName).getNumberValue()
            });

            settingName = "maxbetv" + sportId;
            sportSettings.push({
                Name: settingName,
                Value: $("#" + settingName).getNumberValue()
            });

            settingName = "maxpermatchv" + sportId;
            sportSettings.push({
                Name: settingName,
                Value: $("#" + settingName).getNumberValue()
            });

            var positionTakingValue = self.Tables.DropdownSportsbookPositionTaking_VS.val();
            settingName = "v" + sportId + "spositiontaking";
            sportSettings.push({
                Name: settingName,
                Value: positionTakingValue
            });

            settingName = "v" + sportId + "mpositiontaking";
            sportSettings.push({
                Name: settingName,
                Value: positionTakingValue
            });

            settingName = "v" + sportId + "apositiontaking";
            sportSettings.push({
                Name: settingName,
                Value: positionTakingValue
            });

            settingInfoList.push({
                Id: sportId,
                SettingInfo: sportSettings
            });
        }

        return settingInfoList;
    };

    self.ValidateUpdateParams = function () {
        if (self.Params.updateLicenseeInfoParams.LicenseeName == '') {
            showAlertMessage("result-message", "Please input licensee name!", 1);
            self.Tables.resultMessage.removeClass("display-none");
            return false;
        }

        self.Tables.resultMessage.empty().addClass("display-none");
        return true;
    };
    self.RenderLicenseeInfo = function (licenseeInfo) {
        self.CacheData.licenseeInfo = licenseeInfo;
        if (licenseeInfo == null) {
            self.Tables.divLicenseeInfo.addClass("display-none");
            showAlertMessage("result-message", viewResources.ThereIsNoData, 1);
            self.Tables.resultMessage.removeClass("display-none");
            return;
        }

        if (licenseeInfo != null && licenseeInfo.ErrorCode > 0) {
            self.Tables.divLicenseeInfo.addClass("display-none");
            showAlertMessage("result-message", licenseeInfo.ErrorMessage, 1);
            self.Tables.resultMessage.removeClass("display-none");
            return;
        }

        self.Tables.resultMessage.empty().addClass("display-none");

        self.Tables.labelLicenseeSite.html(licenseeInfo.Site);
        self.Tables.labelLicenseeCurrency.html(licenseeInfo.Currency);
        self.Tables.checkboxEnableNumbergame.attr("checked", licenseeInfo.EnableNumberGame ?
            "checked" :
            null);

        var settingInfoDictionary = self.GetSettingInfoDictionary(licenseeInfo);
        self.RenderLicenseeInfo_Sportsbook(licenseeInfo, settingInfoDictionary);
        self.RenderLicenseeInfo_NumberGame(licenseeInfo, settingInfoDictionary);      
        self.RenderLicenseeInfo_VirtualSport(licenseeInfo, settingInfoDictionary);
        self.Tables.divLicenseeInfo.removeClass("display-none");
    };
    self.GetSettingInfoDictionary = function (licenseeInfo) {
        var settingInfoDictionary = {};

        var settingLength = licenseeInfo.SettingInfo.length;
        for (var i = 0; i < settingLength; i++) {
            var settingInfo = licenseeInfo.SettingInfo[i];
            settingInfoDictionary[settingInfo.Name] = settingInfo.Value;
        }

        var virtualSportsLength = licenseeInfo.VirtualSportsSettingInfo.length;
        for (var i = 0; i < virtualSportsLength; i++) {
            var sport = licenseeInfo.VirtualSportsSettingInfo[i];
            var settingLength = sport.SettingInfo.length;
            for (var j = 0; j < settingLength; j++) {
                var settingInfo = sport.SettingInfo[j];
                settingInfoDictionary[settingInfo.Name] = settingInfo.Value;
            }
        }

        return settingInfoDictionary;
    };

    self.RenderLicenseeInfo_Sportsbook = function (licenseeInfo, settingInfoDictionary) {
        var sportId;
        var settingName;
        var sportLength = sportsbookSports.length;
        for (var i = 0; i < sportLength; i++) {
            var sport = sportsbookSports[i];
            sportId = sport.Id.toLowerCase();
            if (sportId == self.Constants.Sports.NumberGame.toString()) {
                continue;
            }

            settingName = "minbetv" + sportId;
            $("#" + settingName).val(settingInfoDictionary[settingName]);
            settingName = "maxbetv" + sportId;
            $("#" + settingName).val(settingInfoDictionary[settingName]);
            settingName = "maxpermatchv" + sportId;
            $("#" + settingName).val(settingInfoDictionary[settingName]);
        }
    };
    self.RenderLicenseeInfo_NumberGame = function (licenseeInfo, settingInfoDictionary) {
        var sportId = self.Constants.Sports.NumberGame.toString();
        var settingName = "minbetv" + sportId;
        $("#" + settingName).val(settingInfoDictionary[settingName]);
        var settingName = "maxbetv" + sportId;
        $("#" + settingName).val(settingInfoDictionary[settingName]);
        var settingName = "maxpermatchv" + sportId;
        $("#" + settingName).val(settingInfoDictionary[settingName]);
        var settingName = "maxperballv" + sportId;
        $("#" + settingName).val(settingInfoDictionary[settingName]);
    };
   
    self.RenderLicenseeInfo_VirtualSport = function (licenseeInfo, settingInfoDictionary) {
        var sportId;
        var settingName;
        settingName = "disablevirtualsports";
        self.Tables.checkboxEnableVirtualSports.attr("checked", licenseeInfo.EnableNumberGame ?
            "checked" :
            null);
        var sportId;
        var settingName;
        var virtualSportsLength = virtualSports.length;
        self.Tables.DivVirtualSportsContainer.find(".virtualsports_sportrow").addClass("display-none");
        for (var i = 0; i < virtualSportsLength; i++) {
            var sport = virtualSports[i];
            sportId = sport.Id;
            settingName = "maxbetv" + sportId;
            if (settingInfoDictionary[settingName] == undefined) {
                continue;
            }

            $("#" + settingName).val(settingInfoDictionary[settingName]);
            settingName = "minbetv" + sportId;
            $("#" + settingName).val(settingInfoDictionary[settingName]);
            settingName = "maxpermatchv" + sportId;
            $("#" + settingName).val(settingInfoDictionary[settingName]);
            self.Tables.DivVirtualSportsContainer.find(".virtualsports_sport" + sportId).removeClass("display-none");
        }
    };
    self.RenderUpdateLicenseeInfoResult = function (updateLicenseeInfoResult) {
        if (updateLicenseeInfoResult == null) {
            showAlertMessage("result-message", "Something is wrong!", 1);
            self.Tables.resultMessage.removeClass("display-none");
            return;
        }

        if (updateLicenseeInfoResult.ErrorCode > 0) {
            showAlertMessage("result-message", updateLicenseeInfoResult.ErrorMessage, 1);
            self.Tables.resultMessage.removeClass("display-none");
            return;
        }

        showAlertMessage("result-message", updateLicenseeInfoResult.ErrorMessage, 0);
        self.Tables.resultMessage.removeClass("display-none");
        setTimeout(function () {
            self.Tables.resultMessage.addClass("display-none");
        }, 5000);
    };
};

var editLicenseeManager = new EditLicenseeManager();