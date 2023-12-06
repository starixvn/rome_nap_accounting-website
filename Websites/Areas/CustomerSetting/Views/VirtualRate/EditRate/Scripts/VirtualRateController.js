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

        var checkboxEnable = $("#chk-apply-downline-accumulate-balance");
        checkboxEnable.click(function () {
            var isEnable = checkboxEnable.attr("checked") ? 1 : 0;
            self.enableEditSetting(isEnable);
        });

        $(".btn-child").each(function () {
            $(this).click(function () {
                event.preventDefault();
                var ckbInput = $(this).parent().find("input").first();
                ckbInput.attr('checked', true);
                $(this).html(ckbInput.data("downlines"));
                let oppositeCkb = $(`#${ckbInput.data("opposite-id")}`);
                let ckbLabelOpposite = oppositeCkb.parent().find("label").first();
                oppositeCkb.attr('checked', false);
                ckbLabelOpposite.html("0");
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
                        $(this).trigger("click");
                    });
                }
            });
        });
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
                        criteria.EnableVirtualRate = $("#chk-enable").attr("checked") ? true : false;
                        criteria.LockVirtualRateStatus = $("#chk-lockVR").attr("checked") ? true : false;
                        criteria.LockVirtualRateBasketballStatus = $("#chk-lockVRBasketball").attr("checked") ? true : false;
                        criteria.LockVirtualRateOtherStatus = $("#chk-lockVROther").attr("checked") ? true : false;
                        criteria.IsApplyAccumulateBalance = $("#chk-enable-accumulate-balance").is(":checked");
                        criteria.IsApplyDownlineAccumulateBalance = $("#chk-apply-downline-accumulate-balance") !== undefined && $("#chk-apply-downline-accumulate-balance").is(":checked");

                        var chkSCLockMaster, chkSCLockAgent, chkSCLockMember = 0;
                        var chkBBLockMaster, chkBBLockAgent, chkBBLockMember = 0;
                        var chkOSLockMaster, chkOSLockAgent, chkOSLockMember = 0;
                        var chkEnableMaster, chkEnableAgent, chkEnableMember = 0;

                        chkSCLockMaster = $("#chk-lockSoccerVRMaster") !== undefined && $("#chk-lockSoccerVRMaster").attr("checked") ? 1 : 0;
                        chkSCLockAgent = $("#chk-lockSoccerVRAgent") !== undefined && $("#chk-lockSoccerVRAgent").attr("checked") ? 1 : 0;
                        chkSCLockMember = $("#chk-lockSoccerVRMember") !== undefined && $("#chk-lockSoccerVRMember").attr("checked") ? 1 : 0;

                        chkBBLockMaster = $("#chk-lockBasketballVRMaster") !== undefined && $("#chk-lockBasketballVRMaster").attr("checked") ? 1 : 0;
                        chkBBLockAgent = $("#chk-lockBasketballVRAgent") !== undefined && $("#chk-lockBasketballVRAgent").attr("checked") ? 1 : 0;
                        chkBBLockMember = $("#chk-locBasketballkVRMember") !== undefined && $("#chk-lockBasketballVRMember").attr("checked") ? 1 : 0;


                        chkOSLockMaster = $("#chk-lockOtherSportsVRMaster") !== undefined && $("#chk-lockOtherSportsVRMaster").attr("checked") ? 1 : 0;
                        chkOSLockAgent = $("#chk-lockOtherSportsVRAgent") !== undefined && $("#chk-lockOtherSportsVRAgent").attr("checked") ? 1 : 0;
                        chkOSLockMember = $("#chk-lockOtherSportsVRMember") !== undefined && $("#chk-lockOtherSportsVRMember").attr("checked") ? 1 : 0;

                        chkEnableMaster = $("#chk-enableVRMaster") !== undefined && $("#chk-enableVRMaster").attr("checked") ? 1 : 0;
                        chkEnableAgent = $("#chk-enableVRAgent") !== undefined && $("#chk-enableVRAgent").attr("checked") ? 1 : 0;
                        chkEnableMember = $("#chk-enableVRMember") !== undefined && $("#chk-enableVRMember").attr("checked") ? 1 : 0;

                        var additionalAction = '<ROOT><VR UnlockSC3="{0}"  UnlockSC2="{1}" UnlockSC1="{2}" UnlockBB3="{3}"  UnlockBB2="{4}" UnlockBB1="{5}" UnlockOS3="{6}"  UnlockOS2="{7}" UnlockOS1="{8}" Enable3="{9}"  Enable2="{10}" Enable1="{11}" /></ROOT>';
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
};

var virtualRateController = new VirtualRateController();