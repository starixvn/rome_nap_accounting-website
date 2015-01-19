var virtualRateController;
$(document).ready(function () {
    virtualRateController.initPage();
});

var VirtualRateController = function () {
    var self = this;
    self.Constants = {
        LiveType: {
            Live: 1,
            NonLive: 0
        },
        Format: {
            Number: {
                Rate: "#,###.0000"
            }
        }
    };
    self.cachedData = {
        copyFromSportId: 0,
        copyFromBetTypeId: 0,
        oldSettings: null
    };

    self.initPage = function () {
        self.cachedData.oldSettings = oldCustomerVirtualRateModel;
        self.renderVirtualRateSetting();

        var checkboxEnable = $("#chk-enable");
        checkboxEnable.attr("checked", customerVirtualRateModel.EnableVirtualRate ? "checked" : null);
        checkboxEnable.click(function () {
            var isEnable = checkboxEnable.attr("checked") ? 1 : 0;
            self.enableEditSetting(isEnable);
        });

        self.bindingEvent();
        self.enableEditSetting(customerVirtualRateModel.EnableVirtualRate);

        $("#btnSubmit").click(function () {
            self.updateSettings();
        });
    };
    self.renderVirtualRateSetting = function () {
        var tableBody = $("#virtual-rate-setting tbody");
        var settingHtml = '';
        var tableColumnCount = betTypeCount * 2;

        var sportLength = customerVirtualRateModel.SportVirtualRateList.length;
        var iconCopySport = '<input id="button-copy-sport-settings" class="icon-copy-setting" type="button" title="' +
                            viewResources.Copy + '"/>';
        for (var i = 0; i < sportLength; i++) {
            var sport = customerVirtualRateModel.SportVirtualRateList[i];
            if (i == 0) {
                self.cachedData.copyFromSportId = sport.Id;
            }

            settingHtml +=
                '<tr class="sport-title">\
                    <td colspan="' + tableColumnCount + '">' +
                        sport.Name +
                        (i == 0 ? iconCopySport : '') +
                    '</td>\
                 </tr>';

            settingHtml += self.getBetTypeTitle(customerVirtualRateModel.BetTypeList, sport);
            settingHtml += self.getLiveHtml(sport.LiveVirtualRate, self.Constants.LiveType.Live, sport);
            settingHtml += self.getLiveHtml(sport.NonLiveVirtualRate, self.Constants.LiveType.NonLive, sport);
            settingHtml += '<tr><td colspan="' + tableColumnCount + '">&nbsp;</td></tr>';
        }

        tableBody.empty().append(settingHtml);
    };

    self.getBetTypeTitle = function (betTypeList, sport) {
        var titleHtml = '<tr>';
        var iconCopyBetType = '<input id="button-copy-bettype-settings-' + sport.Id +
                '" class="icon-copy-setting bettype-setting" type="button" title="' +
                viewResources.Copy + '"' +
                ' sport-id="' + sport.Id + '"/>';
        for (var i = 0; i < betTypeCount; i++) {
            var betType = betTypeList[i];
            if (i == 0) {
                self.cachedData.copyFromBetTypeId = betType.Id;
            }

            titleHtml += '<td class="' + (i == 0 ? 'first-column' : 'bettype-separator-column') + '">&nbsp;</td>';
            titleHtml += '<td class="bettypevalue-column">' +
                    betType.Name + ' (' + viewResources.Symbol_Percent + ')' + (i == 0 ? iconCopyBetType : '') +
                '</td>';
        }

        titleHtml += '</tr>';
        return titleHtml;
    };
    self.getLiveHtml = function (liveVirtualRate, liveType, sport) {
        var liveHtml = '<tr>';
        for (var i = 0; i < betTypeCount; i++) {
            var liveRate = liveVirtualRate[i];
            var value = (liveRate.PercentRate + 1) * customerVirtualRateModel.ActualExchangeRate;
            liveHtml += '<td class="l">' + (i == 0 ? self.getLiveTypeName(liveType) : '&nbsp;') + '</td>';
            liveHtml +=
                '<td class="bettypevalue-column l">' +
                    self.renderDropdownRate(customerVirtualRateModel.MinVirtualRate,
                        customerVirtualRateModel.MaxVirtualRate,
                        liveRate.PercentRate,
                        customerVirtualRateModel.ActualExchangeRate,
                        sport,
                        liveType,
                        i == 0 ? 'source' : 'target',
                        customerVirtualRateModel.BetTypeList[i].Id) +
                '</td>';
        }

        liveHtml += '</tr>';
        return liveHtml;
    };
    self.getLiveTypeName = function (liveType) {
        switch (liveType) {
            case self.Constants.LiveType.Live:
                return viewResources.Live;
            case self.Constants.LiveType.NonLive:
                return viewResources.Non_Live;
            default:
                return '';
        }
    };
    self.bindingEvent = function () {
        var buttonCopy = $("#button-copy-sport-settings");
        buttonCopy.unbind("click");
        buttonCopy.click(function () {
            self.copySettingFromSport();
        });

        var buttonCopyBetType = $(".icon-copy-setting.bettype-setting");
        buttonCopyBetType.unbind("click");
        buttonCopyBetType.click(function () {
            var sportId = parseInt($(this).attr("sport-id"));
            self.copySettingFromBetType(sportId, $(this));
        });
    };
    self.enableEditSetting = function (isEnable) {
        var iconCopySetting = $(".icon-copy-setting");
        var betTypeColumn = $(".bettypevalue-column");
        if (isEnable) {
            iconCopySetting.attr("disabled", null);
            betTypeColumn.attr("disabled", null);
            betTypeColumn.find("select").attr("disabled", null);
            self.bindingEvent();
        }
        else {
            iconCopySetting.attr("disabled", "disabled");
            betTypeColumn.attr("disabled", "disabled");
            betTypeColumn.find("select").attr("disabled", "disabled");
            iconCopySetting.unbind("click");
            $("#button-copy-sport-settings").unbind("click");
            $(".icon-copy-setting.bettype-setting").unbind("click");
        }
    };
    self.copySettingFromSport = function () {
        var buttonCopy = $("#button-copy-sport-settings");
        var isCopy = !buttonCopy.hasClass("icon-copy-setting-checked");
        if (isCopy) {
            buttonCopy.addClass("icon-copy-setting-checked");
        }
        else {
            buttonCopy.removeClass("icon-copy-setting-checked");
        }

        var sportId = self.cachedData.copyFromSportId;
        var liveType = self.Constants.LiveType.Live;
        var targetDropdown = $('select[sport-id!="' + sportId + '"]' +
            '[liveType="' + liveType + '"]');
        targetDropdown.attr("disabled", isCopy ? "disabled" : null);

        liveType = self.Constants.LiveType.NonLive;
        targetDropdown = $('select[sport-id!="' + sportId + '"]' +
            '[liveType="' + liveType + '"]');
        targetDropdown.attr("disabled", isCopy ? "disabled" : null);

        var buttonCopyBetType = $("#button-copy-bettype-settings-" + sportId);
        if (isCopy) {
            var isCopyBetType = buttonCopyBetType.hasClass("icon-copy-setting-checked");
            liveType = self.Constants.LiveType.Live;
            targetDropdown = $('select[sport-id="' + sportId + '"]' +
                '[liveType="' + liveType + '"]' +
                '[copy-direction="target"]');
            targetDropdown.attr("disabled", "disabled");

            liveType = self.Constants.LiveType.NonLive;
            targetDropdown = $('select[sport-id="' + sportId + '"]' +
                '[liveType="' + liveType + '"]' +
                '[copy-direction="target"]');
            targetDropdown.attr("disabled", "disabled");
        }
        else if (!buttonCopyBetType.hasClass("icon-copy-setting-checked")) {
            liveType = self.Constants.LiveType.Live;
            targetDropdown = $('select[sport-id="' + sportId + '"]' +
                '[liveType="' + liveType + '"]' +
                '[copy-direction="target"]');
            targetDropdown.attr("disabled", null);

            liveType = self.Constants.LiveType.NonLive;
            targetDropdown = $('select[sport-id="' + sportId + '"]' +
                '[liveType="' + liveType + '"]' +
                '[copy-direction="target"]');
            targetDropdown.attr("disabled", null);

        }

        buttonCopyBetType.attr("disabled", isCopy ? "disabled" : null);
        $('.icon-copy-setting.bettype-setting[sport-id!="' + sportId + '"]').attr("disabled", isCopy ? "disabled" : null);
    };

    self.copySettingFromBetType = function (sportId, buttonCopy, isKeepIcon) {
        var isCopy = !buttonCopy.hasClass("icon-copy-setting-checked");
        if (!isKeepIcon) {
            if (isCopy) {
                buttonCopy.addClass("icon-copy-setting-checked");
            }
            else {
                buttonCopy.removeClass("icon-copy-setting-checked");
            }
        }

        var liveType = self.Constants.LiveType.Live;
        var targetDropdown = $('select[sport-id="' + sportId + '"]' +
            '[liveType="' + liveType + '"]' +
            '[copy-direction="target"]');
        targetDropdown.attr("disabled", isCopy ? "disabled" : null);

        liveType = self.Constants.LiveType.NonLive;
        targetDropdown = $('select[sport-id="' + sportId + '"]' +
            '[liveType="' + liveType + '"]' +
            '[copy-direction="target"]');
        targetDropdown.attr("disabled", isCopy ? "disabled" : null);
    };
    self.copySettingFromBetTypeLiveType = function (sportId, liveType) {
        var fromDropdown = $('select[sport-id="' + sportId + '"]' +
            '[liveType="' + liveType + '"]' +
            '[copy-direction="source"]');
        var targetDropdown = $('select[sport-id="' + sportId + '"]' +
            '[liveType="' + liveType + '"]' +
            '[copy-direction="target"]');

        var selectedValue = fromDropdown.val();
        targetDropdown.val(selectedValue);
    };
    self.renderDropdownRate = function (
        minRate,
        maxRate,
        selectedRate,
        actualExchangeRate,
        sport,
        liveType,
        copyDirection,
        betTypeId) {
        var html = '<select' +
                        ' sport-id="' + sport.Id + '"' +
                        ' liveType="' + liveType + '"' +
                        ' copy-direction="' + copyDirection + '"' +
                        ' bettype-id="' + betTypeId + '"' +
                    '>';
        var currentRate = parseInt((maxRate * 100).toFixed(0));
        var limitRate = parseInt((minRate * 100).toFixed(0));
        var rate = parseInt(((selectedRate - 1) * 100).toFixed(0));
        while (currentRate >= limitRate) {
            var value = (currentRate + 100) * actualExchangeRate / 100;
            var rateText = currentRate + ' (' + numeral(value).format(self.Constants.Format.Number.Rate) + ')';
            html += '<option' + (rate == currentRate ? ' selected="selected"' : '') +
                ' value="' + currentRate + '"' + '>' +
                rateText + '</option>';

            if (5 < currentRate && currentRate <= 10) {
                currentRate -= 1;
            }
            else {
                currentRate -= 5;
            }
        }

        html += '</select>';
        return html;
    };
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
    self.updateSettings = function () {
        if (!isManageVirtualRateAccess) {
            self.renderUpdateResult({
                ErrorCode: 1,
                ErrorMessage: viewResources.NoPermission_OnFunction
            });
            return;
        } else {
            $("#result-message").empty();
        }

        if (confirm(viewResources.VirtualRate_UpdateConfirm)) {
            self.blockUI();
            var criteria = $.extend({}, customerVirtualRateModel);
            criteria.EnableVirtualRate = $("#chk-enable").attr("checked") ? true : false;
            var sportLength = customerVirtualRateModel.SportVirtualRateList.length;

            var isCopyFromSport = $("#button-copy-sport-settings").hasClass("icon-copy-setting-checked");
            for (var i = 0; i < sportLength; i++) {
                var sport = customerVirtualRateModel.SportVirtualRateList[i];
                var isCopyFromBetType = $("#button-copy-bettype-settings-" + sport.Id).hasClass("icon-copy-setting-checked");

                for (var j = 0; j < betTypeCount; j++) {
                    var betType = customerVirtualRateModel.BetTypeList[j];

                    var liveRate = $('select[sport-id="' +
                        (isCopyFromSport ? self.cachedData.copyFromSportId : sport.Id) + '"]' +
                    '[liveType="' + self.Constants.LiveType.Live + '"]' +
                    '[bettype-id="' +
                        (isCopyFromSport || isCopyFromBetType ? self.cachedData.copyFromBetTypeId : betType.Id) + '"]').val();
                    customerVirtualRateModel.SportVirtualRateList[i].LiveVirtualRate[j].PercentRate = parseInt(liveRate) / 100 + 1;

                    var nonliveRate = $('select[sport-id="' +
                        (isCopyFromSport ? self.cachedData.copyFromSportId : sport.Id) + '"]' +
                    '[liveType="' + self.Constants.LiveType.NonLive + '"]' +
                    '[bettype-id="' +
                        (isCopyFromSport || isCopyFromBetType ? self.cachedData.copyFromBetTypeId : betType.Id) + '"]').val();
                    customerVirtualRateModel.SportVirtualRateList[i].NonLiveVirtualRate[j].PercentRate = parseInt(nonliveRate) / 100 + 1;
                }
            }

            criteria.LastSettings = self.cachedData.oldSettings;
            self.CallAjax(self,
                window.rootUrl + "VirtualRate/UpdateSettings",
                criteria,
                self.renderUpdateResult,
                self.renderUpdateResult,
                self.unblockUI);
        }
    };
    self.renderUpdateResult = function (updateResult) {
        if (updateResult == null) {
            showAlertMessage("result-message", viewResources.System_Error, 1);
            return;
        }

        if (updateResult.ErrorCode > 0) {
            showAlertMessage("result-message", updateResult.ErrorMessage, 1);
            return;
        }

        showAlertMessage("result-message", updateResult.ErrorMessage, 0);
        setTimeout(function () {
            $("#result-message").empty();
            parent.afterUpdate(true, 1000);
        }, 3000);
    };
    self.blockUI = function () {
        try {
            var blocker = parent.parent.BlockUI ? parent.parent : parent;
            blocker.BlockUI();
        }
        catch (e) {
        }
    };
    self.unblockUI = function () {
        try {
            var blocker = parent.parent.$ && parent.parent.$.unblockUI ?
             parent.parent.$ :
             parent.$;
            blocker.unblockUI();
        }
        catch (e) {
        }
    };
};

var virtualRateController = new VirtualRateController();