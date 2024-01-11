var virtualRateController;
$(document).ready(function () {
    virtualRateController.initPage();
});

String.prototype.format = function () {
    a = this;
    for (k in arguments) {
        a = a.replace("{" + k + "}", arguments[k]);
    }
    return a;
};

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
        oldSettings: null,
        closetimer: null,
        vrSettings: {
            isApplyVRAB: -1,
            isApplyVRABMaster: -1,
            isApplyVRABAgent: -1,
            isApplyVRABMember: -1,
            isApplyVRLSSoccer: -1,
            isApplyVRLSSoccerMaster: -1,
            isApplyVRLSSoccerAgent: -1,
            isApplyVRLSSoccerMember: -1,
            isApplyVRLSBasketball: -1,
            isApplyVRLSBasketballMaster: -1,
            isApplyVRLSBasketballAgent: -1,
            isApplyVRLSBasketballMember: -1,
            isApplyVRLSOther: -1,
            isApplyVRLSOtherMaster: -1,
            isApplyVRLSOtherAgent: -1,
            isApplyVRLSOtherMember: -1

        }
    };

    self.initPage = function () {
        self.cachedData.oldSettings = oldCustomerVirtualRateModel;
        self.renderVirtualRateSetting();

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

        var startBetTypeIndex = 0;
        for (var i = 0; i < sportLength; i++) {
            var sport = customerVirtualRateModel.SportVirtualRateList[i];
            if (!sport.BetTypeList || !sport.BetTypeList.length) {
                continue;
            }

            var betTypeLength = sport.BetTypeList.length;
            if (betTypeLength) {
                settingHtml +=
                '<tr class="sport-title">\
                    <td colspan="' + tableColumnCount + '">' +
                        sport.Name +
                        (sport.Notes && sport.Notes != '' ? ' (' + sport.Notes + ')' : '') +
                        self.renderIconCopySport(sport.Id) +
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

        if (settingHtml !== '') {
            settingHtml += '<tr class="padding-row end-row"><td colspan="' + tableColumnCount + '">&nbsp;</td></tr>';
        }

        tableBody.empty().append(settingHtml);
    };

    self.getBetTypeTitle = function (betTypeList, sport, startBetTypeIndex) {
        var endBetTypeIndex = startBetTypeIndex + maxBetTypeColumn;
        var iconCopyBetType = '<input id="button-copy-bettype-settings-' + sport.Id +
                '" class="icon-copy-setting bettype-setting" type="button" title="' +
                viewResources.VirtualRate_CopyFromHDP + '"' +
                ' sport-id="' + sport.Id + '"/>';

        var titleHtml = '<tr class="table-header">';
        titleHtml += '<td class="first-column" colspan="3">&nbsp;</td>';
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
        liveHtml += '<td class="first-column l p-l5" colspan="3">' + self.getLiveTypeName(liveType) + '</td>';
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
            liveHtml += '<td class="subsport-first-column r" rowspan="3">' + subSport.Name + '</td>';
        }

        liveHtml += '<td class="subsport-livetype-column l p-l5">' + self.getLiveTypeName(liveType) + '</td>';
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
            html += '<td class="bettypevalue-column c padding-column" colspan="' + (endBetTypeIndex - i) + '"></td>';
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
        var checkboxEnable = $("#chk-enableVR");
        checkboxEnable.click(function () {
            var isEnable = checkboxEnable.attr("checked") ? 1 : 0;
            self.enableEditSetting(isEnable);
        });

        $(".btn-child").each(function () {
            $(this).click(function () {
                event.preventDefault();
                self.clickDisableVRItemStatus($(this));
            });
        });

        $(".btn-parent").each(function () {
            $(this).click(function () {
                event.preventDefault();
                let ckbLabel = $(this);
                var ckbInput = ckbLabel.parent().find("input").first();
                let isEnable = ckbInput.is(":checked") ? false : true;
                ckbInput.attr('checked', isEnable);
                if (isEnable) {
                    ckbLabel.removeClass("btn-disable");
                    ckbLabel.addClass("btn-enable");
                    ckbLabel.html(ckbLabel.data("value-enable"));
                } else {
                    ckbLabel.removeClass("btn-enable");
                    ckbLabel.addClass("btn-disable");
                    ckbLabel.html(ckbLabel.data("value-disable"));
                    let ckbType = ckbLabel.data("btn-type");
                    $(`.btn-${ckbType}-disable-child`).each(function () {
                        var ckbInputChild = $(this).parent().find("input").first();
                        ckbInputChild.attr('checked', true);
                        $(this).html(ckbInputChild.data("downlines"));
                        let oppositeCkb = $(`#${ckbInputChild.data("opposite-id")}`);
                        let ckbLabelOpposite = oppositeCkb.parent().find("label").first();
                        oppositeCkb.attr('checked', false);
                        ckbLabelOpposite.html("0");
                    });
                }
            });
        });

    };

    self.clickDisableVRItemStatus = function (currentSelected) {
        var ckbInput = currentSelected.parent().find("input").first();
        ckbInput.attr('checked', true);
        currentSelected.html(ckbInput.data("downlines"));
        let oppositeCkb = $(`#${ckbInput.data("opposite-id")}`);
        let ckbLabelOpposite = oppositeCkb.parent().find("label").first();
        oppositeCkb.attr('checked', false);
        ckbLabelOpposite.html("0");
        if (currentSelected.data("downline-id")) {
            self.clickDisableVRItemStatus($(`#${currentSelected.data("downline-id")}`).parent().find("label").first());
        }
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

            var buttonCopy = $("#button-copy-sport-settings-" + self.Constants.SportType.Soccer);
            var buttonCopyBasketball = $("#button-copy-sport-settings-" + self.Constants.SportType.Basketball);

            buttonCopy
                .unbind("click")
                .click(function () {
                    if (buttonCopyBasketball.hasClass("icon-copy-setting-checked")) {
                        buttonCopyBasketball.trigger("click");
                    }

                    self.copySettingFromSport(buttonCopy);
                });

            buttonCopyBasketball
                .unbind("click")
                .click(function () {
                    if (buttonCopy.hasClass("icon-copy-setting-checked")) {
                        buttonCopy.trigger("click");
                    }

                    self.copySettingFromSport(buttonCopyBasketball);
                });

            var buttonCopyBetType = $(".icon-copy-setting.bettype-setting");
            buttonCopyBetType.unbind("click");
            buttonCopyBetType.click(function () {
                var sportId = parseInt($(this).attr("sport-id"));
                self.copySettingFromBetType(sportId, $(this));
            });
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
            $(".sport-title .icon-copy-setting").unbind("click");
            $(".icon-copy-setting.bettype-setting").unbind("click");
        }
    };
    self.copySettingFromSport = function (buttonCopy) {
        var sportId = parseInt(buttonCopy.attr("data-sport-id"));
        var isCopy = !buttonCopy.hasClass("icon-copy-setting-checked");
        if (isCopy) {
            buttonCopy.addClass("icon-copy-setting-checked");
            self.cachedData.copyFromSportId = sportId;
        }
        else {
            buttonCopy.removeClass("icon-copy-setting-checked");
            self.cachedData.copyFromSportId = 0;
        }

        var liveType = self.Constants.LiveType.Live;
        var targetDropdown = $('select[liveType="' + liveType + '"]')
            .not(self.generateSelectSelector(sportId));
        targetDropdown.each(function (index) {
            self.disableDropdownRate($(this), isCopy);
        });

        liveType = self.Constants.LiveType.NonLive;
        targetDropdown = $('select[liveType="' + liveType + '"]')
            .not(self.generateSelectSelector(sportId));
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
        $('.icon-copy-setting.bettype-setting').not(self.generateSelectSelector(sportId)).attr("disabled", isCopy ? "disabled" : null);
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

        var url = window.rootUrl + "VirtualRate/GetConfirmUpdateMessage";
        var params = {
            CustId: custId
        };
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (rs) {
                __confirm("Update Virtual Rate", rs,
                    325, 210, null,
                    function () {
                        __close(false, null, null, null);
                        self.blockUI();
                        var criteria = $.extend({}, customerVirtualRateModel);
                        criteria.EnableVirtualRate = $("#chk-enableVR").is(":checked") ? true : false;
                        criteria.LockVirtualRateStatus = $("#ckb_unlockSoccerVR").is(":checked") ? false : true;
                        criteria.LockVirtualRateBasketballStatus = $("#ckb_unlockBasketballVR").is(":checked") ? false : true;
                        criteria.LockVirtualRateOtherStatus = $("#ckb_unlockOtherSportsVR").is(":checked") ? false : true;
                        criteria.IsApplyAccumulateBalance = $("#chk_enableAccumulateBalance").is(":checked");

                        var chkSCLockMaster, chkSCLockAgent, chkSCLockMember;
                        var chkBBLockMaster, chkBBLockAgent, chkBBLockMember;
                        var chkOSLockMaster, chkOSLockAgent, chkOSLockMember;
                        var chkEnableMaster, chkEnableAgent, chkEnableMember;
                        chkSCLockMaster = chkSCLockAgent = chkSCLockMember = chkBBLockMaster = chkBBLockAgent = chkBBLockMember = chkOSLockMaster = chkOSLockAgent = chkOSLockMember = chkEnableMaster = chkEnableAgent = chkEnableMember = 2;

                        if ($("#ckb_lockSoccerVRMaster") !== undefined && $("#ckb_lockSoccerVRMaster").is(":checked")) chkSCLockMaster = 0;
                        if ($("#ckb_lockSoccerVRAgent") !== undefined && $("#ckb_lockSoccerVRAgent").is(":checked")) chkSCLockAgent = 0;
                        if ($("#ckb_lockSoccerVRMember") !== undefined && $("#ckb_lockSoccerVRMember").is(":checked")) chkSCLockMember = 0;
                        if ($("#ckb_unlockSoccerVRMaster") !== undefined && $("#ckb_unlockSoccerVRMaster").is(":checked")) chkSCLockMaster = 1;
                        if ($("#ckb_unlockSoccerVRAgent") !== undefined && $("#ckb_unlockSoccerVRAgent").is(":checked")) chkSCLockAgent = 1;
                        if ($("#ckb_unlockSoccerVRMember") !== undefined && $("#ckb_unlockSoccerVRMember").is(":checked")) chkSCLockMember = 1;

                        if ($("#ckb_lockBasketballVRMaster") !== undefined && $("#ckb_lockBasketballVRMaster").is(":checked")) chkBBLockMaster = 0;
                        if ($("#ckb_lockBasketballVRAgent") !== undefined && $("#ckb_lockBasketballVRAgent").is(":checked")) chkBBLockAgent = 0;
                        if ($("#ckb_lockBasketballVRMember") !== undefined && $("#ckb_lockBasketballVRMember").is(":checked")) chkBBLockMember = 0;
                        if ($("#ckb_unlockBasketballVRMaster") !== undefined && $("#ckb_unlockBasketballVRMaster").is(":checked")) chkBBLockMaster = 1;
                        if ($("#ckb_unlockBasketballVRAgent") !== undefined && $("#ckb_unlockBasketballVRAgent").is(":checked")) chkBBLockAgent = 1;
                        if ($("#ckb_unlockBasketballVRMember") !== undefined && $("#ckb_unlockBasketballVRMember").is(":checked")) chkBBLockMember = 1;

                        if ($("#ckb_lockOtherSportsVRMaster") !== undefined && $("#ckb_lockOtherSportsVRMaster").is(":checked")) chkOSLockMaster = 0;
                        if ($("#ckb_lockOtherSportsVRAgent") !== undefined && $("#ckb_lockOtherSportsVRAgent").is(":checked")) chkOSLockAgent = 0;
                        if ($("#ckb_lockOtherSportsVRMember") !== undefined && $("#ckb_lockOtherSportsVRMember").is(":checked")) chkOSLockMember = 0;
                        if ($("#ckb_unlockOtherSportsVRMaster") !== undefined && $("#ckb_unlockOtherSportsVRMaster").is(":checked")) chkOSLockMaster = 1;
                        if ($("#ckb_unlockOtherSportsVRAgent") !== undefined && $("#ckb_unlockOtherSportsVRAgent").is(":checked")) chkOSLockAgent = 1;
                        if ($("#ckb_unlockOtherSportsVRMember") !== undefined && $("#ckb_unlockOtherSportsVRMember").is(":checked")) chkOSLockMember = 1;

                        if ($("#chk_enable_master_AB") !== undefined && $("#chk_enable_master_AB").is(":checked")) chkEnableMaster = 1;
                        if ($("#chk_enable_agent_AB") !== undefined && $("#chk_enable_agent_AB").is(":checked")) chkEnableAgent = 1;
                        if ($("#chk_enable_member_AB") !== undefined && $("#chk_enable_member_AB").is(":checked")) chkEnableMember = 1;
                        if ($("#chk_disable_master_AB") !== undefined && $("#chk_disable_master_AB").is(":checked")) chkEnableMaster = 0;
                        if ($("#chk_disable_agent_AB") !== undefined && $("#chk_disable_agent_AB").is(":checked")) chkEnableAgent = 0;
                        if ($("#chk_disable_member_AB") !== undefined && $("#chk_disable_member_AB").is(":checked")) chkEnableMember = 0;

                        var additionalAction = '<ROOT><VR UnlockSC3="{0}"  UnlockSC2="{1}" UnlockSC1="{2}" UnlockBB3="{3}"  UnlockBB2="{4}" UnlockBB1="{5}" UnlockOS3="{6}"  UnlockOS2="{7}" UnlockOS1="{8}" EnableAB3="{9}"  EnableAB2="{10}" EnableAB1="{11}" /></ROOT>';
                        criteria.AdditionalActions = additionalAction.format(chkSCLockMaster, chkSCLockAgent, chkSCLockMember, chkBBLockMaster, chkBBLockAgent, chkBBLockMember, chkOSLockMaster, chkOSLockAgent, chkOSLockMember, chkEnableMaster, chkEnableAgent, chkEnableMember);

                        var sportLength = customerVirtualRateModel.SportVirtualRateList.length;

                        var isCopyFromSport = $("#button-copy-sport-settings-" + self.Constants.SportType.Soccer).hasClass("icon-copy-setting-checked");
                        var isCopyFromBasketball = $("#button-copy-sport-settings-" + self.Constants.SportType.Basketball).hasClass("icon-copy-setting-checked");

                        criteria.LastSettings = self.cachedData.oldSettings;
                        for (var i = 0; i < sportLength; i++) {
                            var sport = customerVirtualRateModel.SportVirtualRateList[i];
                            var isCopyFromBetType = $("#button-copy-bettype-settings-" + sport.Id).hasClass("icon-copy-setting-checked");

                            if (!isCopyFromSport) { // Check "Copy Settings of Basketball" button is checked.
                                isCopyFromSport = isCopyFromBasketball && sport.Id !== self.Constants.SportType.Soccer;
                            }

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
            },
            error: function (jqXHR, event, ajaxSettings) {
            }
        });
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
            //parent.afterUpdate(true, 1000);
            location.href = location.href;
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
    self.renderIconCopySport = function (sportId) {
        var title = "";

        if (sportId === self.Constants.SportType.Soccer) {
            title = viewResources.VirtualRate_CopyToAllSportTypeBettypeFromHDP;
        }
        else if (sportId === self.Constants.SportType.Basketball) {
            title = viewResources.VirtualRate_CopyBasketballToAllSportTypeBettypeFromHDP;
        }
        else {
            return "";
        }

        return [
            '<input class="icon-copy-setting" type="button" title="',
            title,
            '" data-sport-id="',
            sportId,
            '" id="button-copy-sport-settings-',
            sportId,
            '"/>'
        ].join("");
    };
    self.generateSelectSelector = function (sportId) {
        if (sportId === self.Constants.SportType.Soccer) {
            return '[sport-id="1"]';
        }
        else if (sportId === self.Constants.SportType.Basketball) {
            return '[sport-id="1"],[sport-id="2"]';
        }

        return "";
    };

    self.openNote = function (id) {
        $(`#noteInfo`).removeClass("note-hide");
    };

    self.closeNote = function (id) {
        self.cachedData.closetimer = window.setTimeout(function () { $(`#noteInfo`).addClass("note-hide"); }, 300);
        
    };

    self.cancelCloseNote = function (id) {
        if (self.cachedData.closetimer) {
            window.clearTimeout(self.cachedData.closetimer);
            self.cachedData.closetimer = null;
        }
    };

    self.showUpdateVRFlow = function (id) {
        var flow1 = "If you enable Virtual rate and change the Virtual Rate value of this account at the same time:";
        var sub1 = "This account will enable Virtual Rate, then change Virtual Rate value.";
        var flow2 = "If you lock Virtual Rate status and change Virtual Rate value of this account at the same time:";
        var sub2 = "This account and all downlines will change the Virtual Rate value, then lock the VR status.";
        var flow3 = "If you unlock Virtual Rate for this account or downlines & change Virtual Rate value at the same time:";
        var sub3 = "This account or all downlines will unlock Virtual Rate, then change Virtual Rate value.";
        var message = `<ul class="roman"><li>${flow1}<ul class="square"><li>${sub1}</li></ul></li><li>${flow2}<ul class="square"><li>${sub2}</li></ul></li><li>${flow3}<ul class="square"><li>${sub3}</li></ul></li></ul>`;
        __showMessageWihtoutBtn("Update VR flow", message, 700, 180);
    };
};

var virtualRateController = new VirtualRateController();