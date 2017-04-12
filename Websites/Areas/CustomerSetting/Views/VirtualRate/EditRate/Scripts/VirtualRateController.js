var virtualRateController;
$(document).ready(function () {
    virtualRateController.initPage();
});

var VirtualRateController = function () {
    var self = this;
    self.Constants = {
        SportType: {
            Soccer: 1,
            OtherSports: 99,
            Tennis: 5,
            Basketball: 2,
            Volleyball: 6,
            MotoSports: 11,
            Football: 3,
            Golf: 10
        },
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

        $("#btnSubmit, #btnSubmit1").click(function () {
            self.updateSettings();
        });
        $(".infoIcon").click(function () {
            window.open(window.rootUrl + "VirtualRate/GetSportBetTypeReferences", '_blank');
        });
    };
    self.renderVirtualRateSetting = function () {
        var tableBody = $("#virtual-rate-setting tbody");
        var settingHtml = '';
        var sportLength = customerVirtualRateModel.SportVirtualRateList.length;
        var iconCopySport = '<input id="button-copy-sport-settings" class="icon-copy-setting" type="button" title="' +
                            viewResources.VirtualRate_CopyToAllSportTypeBettypeFromHDP + '"/>';

        var startBetTypeIndex = 0;
        for (var i = 0; i < sportLength; i++) {
            var sport = customerVirtualRateModel.SportVirtualRateList[i];
            if (!sport.BetTypeList || !sport.BetTypeList.length) {
                continue;
            }

            if (i == 0) {
                self.cachedData.copyFromSportId = sport.Id;
            }

            var betTypeLength = sport.BetTypeList.length;
            if (betTypeLength) {
                settingHtml +=
                '<tr class="sport-title">\
                    <td colspan="' + tableColumnCount + '">' +
                        sport.Name +
                        (sport.Notes && sport.Notes != '' ? ' (' + sport.Notes + ')' : '') +
                        (i == 0 ? iconCopySport : '') +
                    '</td>\
                 </tr>';
            }

            for (var j = startBetTypeIndex; j < betTypeLength; j += maxBetTypeColumn) {
                settingHtml += self.getBetTypeTitle(sport.BetTypeList, sport, j);
                if (sport.SubSportList && sport.SubSportList.length) {
                    var subSportListLength = sport.SubSportList.length;
                    for (var k = 0; k < subSportListLength; k++) {
                        var subSport = sport.SubSportList[k];
                        settingHtml += self.getSubSportHtml(subSport.LiveVirtualRate, subSport, sport, j, self.Constants.LiveType.Live);
                        settingHtml += self.getSubSportHtml(subSport.NonLiveVirtualRate, subSport, sport, j, self.Constants.LiveType.NonLive);
                    }
                }
                else {
                    settingHtml += self.getLiveHtml(sport.LiveVirtualRate, self.Constants.LiveType.Live, sport, j);
                    settingHtml += self.getLiveHtml(sport.NonLiveVirtualRate, self.Constants.LiveType.NonLive, sport, j);
                }
            }

            if (i !== sportLength - 1) {
                settingHtml += '<tr class="padding-row"><td colspan="' + tableColumnCount + '">&nbsp;</td></tr>';
            }
        }

        settingHtml += '<tr class="padding-row end-row"><td colspan="' + tableColumnCount + '">&nbsp;</td></tr>';
        tableBody.empty().append(settingHtml);
    };

    self.getBetTypeTitle = function (betTypeList, sport, startBetTypeIndex) {
        var endBetTypeIndex = startBetTypeIndex + maxBetTypeColumn;
        var iconCopyBetType = '<input id="button-copy-bettype-settings-' + sport.Id +
                '" class="icon-copy-setting bettype-setting" type="button" title="' +
                viewResources.VirtualRate_CopyFromHDP + '"' +
                ' sport-id="' + sport.Id + '"/>';

        var titleHtml = '<tr class="table-header">';
        titleHtml += '<td class="first-column" colspan="2">&nbsp;</td>';
        var betTypeLength = betTypeList.length;
        var i;
        for (i = startBetTypeIndex; i < endBetTypeIndex && i < betTypeLength; i++) {
            var betType = betTypeList[i];
            if (i == 0) {
                self.cachedData.copyFromBetTypeId = betType.Id;
            }

            var title = betType.Name + ' (' + viewResources.Symbol_Percent + ')' + (betType.Id == 1 ? iconCopyBetType : '');
            var multiBetTypeLength = betType.MultiBetTypeNames ? betType.MultiBetTypeNames.length : 0;
            for (var j = 0; j < multiBetTypeLength; j++) {
                title += '<br />' + betType.MultiBetTypeNames[j];
            }

            titleHtml += '<td class="bettypevalue-column">' + title + '</td>';
        }

        titleHtml += self.renderPaddingEmptyColumn(startBetTypeIndex, endBetTypeIndex, i);
        titleHtml += '</tr>';
        return titleHtml;
    };
    self.getLiveHtml = function (liveVirtualRate, liveType, sport, startBetTypeIndex) {
        var endBetTypeIndex = startBetTypeIndex + maxBetTypeColumn;
        var betTypeLength = sport.BetTypeList.length;
        var liveHtml = '<tr>';
        liveHtml += '<td class="first-column l" colspan="2">' + self.getLiveTypeName(liveType) + '</td>';
        var i = 0;
        for (i = startBetTypeIndex; i < endBetTypeIndex && i < betTypeLength;) {
            var liveRate = liveVirtualRate[i];
            var value = (liveRate.PercentRate + 1) * customerVirtualRateModel.ActualExchangeRate;
            liveHtml +=
                '<td class="bettypevalue-column c">' +
                    (liveRate.HasData ?
                        self.renderDropdownRate(customerVirtualRateModel.MinVirtualRate,
                            customerVirtualRateModel.MaxVirtualRate,
                            liveRate.PercentRate,
                            customerVirtualRateModel.ActualExchangeRate,
                            sport,
                            liveType,
                            i == 0 ? 'source' : 'target',
                            sport.BetTypeList[i].Id,
                            sport.Id) :
                        '') +
                '</td>';

            i++;
        }

        liveHtml += self.renderPaddingEmptyColumn(startBetTypeIndex, endBetTypeIndex, i);
        liveHtml += '</tr>';
        return liveHtml;
    };
    self.getSubSportHtml = function (liveVirtualRate, subSport, sport, startBetTypeIndex, liveType) {
        var endBetTypeIndex = startBetTypeIndex + maxBetTypeColumn;
        var betTypeLength = sport.BetTypeList.length;
        var liveHtml = '<tr>';
        if (liveType === self.Constants.LiveType.Live) {
            liveHtml += '<td class="subsport-first-column r" rowspan="2">' + subSport.Name + '</td>';
        }

        liveHtml += '<td class="subsport-livetype-column l">' + self.getLiveTypeName(liveType) + '</td>';
        var i = 0;
        for (i = startBetTypeIndex; i < endBetTypeIndex && i < betTypeLength;) {
            var liveRate = liveVirtualRate[i];
            var value = (liveRate.PercentRate + 1) * customerVirtualRateModel.ActualExchangeRate;
            liveHtml +=
                '<td class="bettypevalue-column c">' +
                    (liveRate.HasData ?
                        self.renderDropdownRate(customerVirtualRateModel.MinVirtualRate,
                            customerVirtualRateModel.MaxVirtualRate,
                            liveRate.PercentRate,
                            customerVirtualRateModel.ActualExchangeRate,
                            subSport,
                            liveType,
                            i == 0 ? 'source' : 'target',
                            sport.BetTypeList[i].Id,
                            sport.Id) :
                        '') +
                '</td>';

            i++;
        }

        liveHtml += self.renderPaddingEmptyColumn(startBetTypeIndex, endBetTypeIndex, i);
        liveHtml += '</tr>';
        return liveHtml;
    };

    self.renderPaddingEmptyColumn = function (startBetTypeIndex, endBetTypeIndex, i) {
        var html = '';
        if (startBetTypeIndex < maxBetTypeColumn) {
            if (i < endBetTypeIndex) {
                html += '<td class="bettypevalue-column c padding-column" colspan="' + (endBetTypeIndex - i) + '"></td>';
            }
        } else if (i < endBetTypeIndex) {
            html += '<td class="bettypevalue-column c padding-column" colspan="' + (endBetTypeIndex - maxBetTypeColumn) + '"></td>';
        }

        return html;
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
            var iconCopySettingAvailable = iconCopySetting.find('[disable-level="1"]');
            iconCopySettingAvailable.attr("disabled", null);
            iconCopySettingAvailable.removeAttr("disable-level");

            betTypeColumn.attr("disabled", null);

            var bettypeSelect = betTypeColumn.find('select[disable-level="1"]');
            bettypeSelect.attr("disabled", null);
            bettypeSelect.removeAttr("disable-level");
            self.bindingEvent();
        }
        else {
            var iconCopySettingAvailable = iconCopySetting.find(":not([disabled])");
            iconCopySettingAvailable.attr('disable-level', '1');
            iconCopySettingAvailable.attr("disabled", "disabled");

            betTypeColumn.attr("disabled", "disabled");

            var bettypeSelect = betTypeColumn.find("select:not([disabled])");
            bettypeSelect.attr("disable-level", "1");
            bettypeSelect.attr("disabled", "disabled");
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
        targetDropdown.each(function (index) {
            self.disableDropdownRate($(this), isCopy);
        });

        liveType = self.Constants.LiveType.NonLive;
        targetDropdown = $('select[sport-id!="' + sportId + '"]' +
            '[liveType="' + liveType + '"]');
        targetDropdown.each(function (index) {
            self.disableDropdownRate($(this), isCopy);
        });

        var buttonCopyBetType = $("#button-copy-bettype-settings-" + sportId);
        var isCopyBetType = buttonCopyBetType.hasClass("icon-copy-setting-checked");
        var sourceDropdownNonLive = $('select[sport-id="' + sportId + '"]' +
                '[liveType="' + self.Constants.LiveType.NonLive + '"]' +
                '[copy-direction="source"]'
                );
        sourceDropdownNonLive.attr("disabled", isCopy ? "disabled" : null);
        if (isCopy) {
            liveType = self.Constants.LiveType.Live;
            targetDropdown = $('select[sport-id="' + sportId + '"]' +
                '[liveType="' + liveType + '"]' +
                '[copy-direction="target"]'
                );
            targetDropdown.attr("disabled", "disabled");

            liveType = self.Constants.LiveType.NonLive;
            targetDropdown = $('select[sport-id="' + sportId + '"]' +
                '[liveType="' + liveType + '"]' +
                '[copy-direction="target"]'
                );

            targetDropdown.attr("disabled", "disabled");
        }
        else {
            liveType = self.Constants.LiveType.Live;
            targetDropdown = $('select[sport-id="' + sportId + '"]' +
                '[liveType="' + liveType + '"]' +
                '[copy-direction="target"]');
            targetDropdown.attr("disabled", isCopyBetType ? "disabled" : null);

            liveType = self.Constants.LiveType.NonLive;
            targetDropdown = $('select[sport-id="' + sportId + '"]' +
                '[liveType="' + liveType + '"]' +
                '[copy-direction="target"]'
                );

            targetDropdown.attr("disabled", isCopyBetType ? "disabled" : null);
        }

        buttonCopyBetType.attr("disabled", isCopy ? "disabled" : null);
        $('.icon-copy-setting.bettype-setting[sport-id!="' + sportId + '"]').attr("disabled", isCopy ? "disabled" : null);
    };

    self.disableDropdownRate = function (dropdown, isCopy) {
        if (isCopy) {
            var isDisabled = dropdown.attr("disabled");
            dropdown.attr("disabled", "disabled");
            if (!isDisabled) {
                dropdown.attr('disabled-by', 'copy-from-sports');
            }
        } else {
            var disabledByCopySport = dropdown.attr("disabled-by") == 'copy-from-sports';
            if (disabledByCopySport) {
                dropdown.attr("disabled", null);
                dropdown.attr('disabled-by', null);
            }
        }
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
        var targetDropdown = $('select[groupd-id="' + sportId + '"]' +
            '[liveType="' + liveType + '"]' +
            '[copy-direction="target"]');
        targetDropdown.attr("disabled", isCopy ? "disabled" : null);

        liveType = self.Constants.LiveType.NonLive;
        targetDropdown = $('select[groupd-id="' + sportId + '"]' +
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
        betTypeId,
        groupdId) {
        var html = '<select' +
                        ' sport-id="' + sport.Id + '"' +
                        ' liveType="' + liveType + '"' +
                        ' copy-direction="' + copyDirection + '"' +
                        ' bettype-id="' + betTypeId + '"' +
                        (groupdId ? ' groupd-id="' + groupdId + '"' : '') +
                    '>';
        var rate = parseInt(((selectedRate - 1) * 100).toFixed(0));
        for (var i = 0 ; i < oldCustomerVirtualRateModel.VirtualRateValueRange.length; i++) {
            var value = (oldCustomerVirtualRateModel.VirtualRateValueRange[i] + 100) * actualExchangeRate / 100;
            var rateText = oldCustomerVirtualRateModel.VirtualRateValueRange[i] + ' (' + numeral(value).format(self.Constants.Format.Number.Rate) + ')';
            html += '<option' + (rate == oldCustomerVirtualRateModel.VirtualRateValueRange[i] ? ' selected="selected"' : '') +
                ' value="' + oldCustomerVirtualRateModel.VirtualRateValueRange[i] + '"' + '>' +
                rateText + '</option>';

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

        __confirm("Update Virtual Rate", viewResources.VirtualRate_UpdateConfirm,
                325, 150, null,
                function () {
                    __close(false, null, null, null);
                    self.blockUI();
                    var criteria = $.extend({}, customerVirtualRateModel);
                    criteria.EnableVirtualRate = $("#chk-enable").attr("checked") ? true : false;
                    var sportLength = customerVirtualRateModel.SportVirtualRateList.length;

                    var isCopyFromSport = $("#button-copy-sport-settings").hasClass("icon-copy-setting-checked");
                    criteria.LastSettings = self.cachedData.oldSettings;
                    for (var i = 0; i < sportLength; i++) {
                        var sport = customerVirtualRateModel.SportVirtualRateList[i];
                        var isCopyFromBetType = $("#button-copy-bettype-settings-" + sport.Id).hasClass("icon-copy-setting-checked");
                        if (sport.SubSportList && sport.SubSportList.length) {
                            var subSportListLength = sport.SubSportList.length;
                            for (var k = 0; k < subSportListLength; k++) {
                                var subSport = sport.SubSportList[k];
                                self.updateSubSportBetTypeRate(sport, subSport, isCopyFromSport, isCopyFromBetType, criteria, i, k);
                            }
                        } else {
                            self.updateBetTypeRate(sport, isCopyFromSport, isCopyFromBetType, criteria, i);
                        }
                    }

                    self.CallAjax(self,
                    window.rootUrl + "VirtualRate/UpdateSettings",
                    criteria,
                    self.renderUpdateResult,
                    self.renderUpdateResult,
                    self.unblockUI);
                },
                function () {
                    __close(false, null, null, null);
                },
                null, false);
    };

    self.updateBetTypeRate = function (sport, isCopyFromSport, isCopyFromBetType, criteria, i) {
        var betTypeLength = sport.BetTypeList.length;
        var sportId = sport.Id;
        for (var j = 0; j < betTypeLength; j++) {
            var betType = sport.BetTypeList[j];
            var betTypeId = isCopyFromSport || isCopyFromBetType ? self.cachedData.copyFromBetTypeId : betType.Id;
            var liveRate = $('select[sport-id="' +
                        (isCopyFromSport ? self.cachedData.copyFromSportId : sportId) + '"]' +
                    '[liveType="' + self.Constants.LiveType.Live + '"]' +
                    '[bettype-id="' + betTypeId + '"]').val();
            if (sport.LiveVirtualRate && sport.LiveVirtualRate[j].HasData) {
                criteria.LastSettings.SportVirtualRateList[i].LiveVirtualRate[j].PercentRate = self.toFixed(criteria.LastSettings.SportVirtualRateList[i].LiveVirtualRate[j].PercentRate, 2);
                customerVirtualRateModel.SportVirtualRateList[i].LiveVirtualRate[j].PercentRate = (parseInt(liveRate) / 100 + 1).toFixed(2);
            }

            var nonliveRate = isCopyFromSport ? liveRate : $('select[sport-id="' +
                        (isCopyFromSport ? self.cachedData.copyFromSportId : sport.Id) + '"]' +
                    '[liveType="' + self.Constants.LiveType.NonLive + '"]' +
                    '[bettype-id="' + betTypeId + '"]').val();
            if (sport.NonLiveVirtualRate && sport.NonLiveVirtualRate[j].HasData) {
                criteria.LastSettings.SportVirtualRateList[i].NonLiveVirtualRate[j].PercentRate = self.toFixed(criteria.LastSettings.SportVirtualRateList[i].NonLiveVirtualRate[j].PercentRate, 2);
                customerVirtualRateModel.SportVirtualRateList[i].NonLiveVirtualRate[j].PercentRate = (parseInt(nonliveRate) / 100 + 1).toFixed(2);
            }
        }
    };
    self.updateSubSportBetTypeRate = function (sport, subSport, isCopyFromSport, isCopyFromBetType, criteria, i, k) {
        var betTypeLength = sport.BetTypeList.length;
        var sportId = subSport.Id;
        for (var j = 0; j < betTypeLength; j++) {
            var betType = sport.BetTypeList[j];
            var betTypeId = isCopyFromSport || isCopyFromBetType ? self.cachedData.copyFromBetTypeId : betType.Id;
            var liveRate = $('select[sport-id="' +
                        (isCopyFromSport ? self.cachedData.copyFromSportId : sportId) + '"]' +
                    '[liveType="' + self.Constants.LiveType.Live + '"]' +
                    '[bettype-id="' + betTypeId + '"]').val();
            if (subSport.LiveVirtualRate && subSport.LiveVirtualRate[j].HasData) {
                criteria.LastSettings.SportVirtualRateList[i]
                    .SubSportList[k]
                    .LiveVirtualRate[j]
                    .PercentRate = self.toFixed(criteria.LastSettings.SportVirtualRateList[i].SubSportList[k].LiveVirtualRate[j].PercentRate, 2);
                customerVirtualRateModel.SportVirtualRateList[i]
                    .SubSportList[k]
                    .LiveVirtualRate[j]
                    .PercentRate = (parseInt(liveRate) / 100 + 1).toFixed(2);
            }

            // If copy for all Sport, we just get live rate and copy to all. Note: Change bussiness.
            var nonliveRate = isCopyFromSport ? liveRate : $('select[sport-id="' +
                        (isCopyFromSport ? self.cachedData.copyFromSportId : sportId) + '"]' +
                    '[liveType="' + self.Constants.LiveType.NonLive + '"]' +
                    '[bettype-id="' + betTypeId + '"]').val();
            if (subSport.NonLiveVirtualRate && subSport.NonLiveVirtualRate[j].HasData) {
                criteria.LastSettings.SportVirtualRateList[i]
                    .SubSportList[k]
                    .NonLiveVirtualRate[j]
                    .PercentRate = self.toFixed(criteria.LastSettings.SportVirtualRateList[i].SubSportList[k].NonLiveVirtualRate[j].PercentRate, 2);
                customerVirtualRateModel.SportVirtualRateList[i]
                    .SubSportList[k]
                    .NonLiveVirtualRate[j]
                    .PercentRate = (parseInt(nonliveRate) / 100 + 1).toFixed(2);
            }
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
    self.toFixed = function (value, decimalNumber) {
        if (typeof (value) === "string") {
            return value;
        }

        return value.toFixed(decimalNumber);
    };
};

var virtualRateController = new VirtualRateController();