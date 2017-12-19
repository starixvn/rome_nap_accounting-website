var betSettingHandler = (function ($, common, rootUrl) {
    "use strict";
    var globalValid,
        globalBetSettings,
        updateSection,
        applyMembersCheckbox,
        betSettingTableRows,
        allSportsCheckbox,
        sportTypeCheckboxes,
        copySettingIcon,
        nextButton,
        numericInputs,
        reviewSection,
        applyMembersSpan,
        reviewSettingTable,
        updateButton,
        backButton;

    function enableNumericInputs(sportType) {
        var inputs = !sportType ?
            numericInputs :
            $.grep(numericInputs, function (item, i) { return $(item).data("sporttype") === sportType; });

        $.each(inputs, function () {
            common.enableElement($(this));
        });
    }

    function disableNumericInputs(sportType) {
        var inputs = !sportType ?
            numericInputs :
            $.grep(numericInputs, function (item, i) { return $(item).data("sporttype") === sportType; });

        $.each(inputs, function () {
            var $this = $(this);
            $this.removeClass("invalid-value");
            common.disableElement($this);

            var oldValue = parseInt($this.data("oldvalue"));
            $this.val(oldValue.toLocaleString("en-US"));
        });
    }

    function allSportsCheckbox_change() {
        var checked = this.checked;

        if (checked) {
            enableNumericInputs();
        } else {
            disableNumericInputs();
        }

        sportTypeCheckboxes.each(function () {
            this.checked = checked;
        });
    }

    function sportTypeCheckbox_change() {
        var sportType = $(this).data("sporttype"),
            checked = this.checked;

        if (checked) {
            enableNumericInputs(sportType);
        } else {
            disableNumericInputs(sportType);
        }

        for (var i = 0, max = sportTypeCheckboxes.length; i < max; i++) {
            if (sportTypeCheckboxes[i].checked !== checked) {
                allSportsCheckbox.prop("indeterminate", true);
                return;
            }
        }

        allSportsCheckbox
            .prop("indeterminate", false)
            .prop("checked", checked);
    }

    function copySettingIcon_click() {
        var checked;

        if (copySettingIcon.hasClass("icon-copy-setting-checked")) {
            checked = false;
            copySettingIcon.removeClass("icon-copy-setting-checked");
            common.enableElement(allSportsCheckbox);
            disableNumericInputs();
        } else {
            checked = true;
            copySettingIcon.addClass("icon-copy-setting-checked");
            var sportType = $(this).data("sporttype");
            copyBetSettingFrom1stRowAndDisableOthers(sportType);
            common.disableElement(allSportsCheckbox);
        }

        allSportsCheckbox.prop("indeterminate", false).prop("checked", checked);

        sportTypeCheckboxes.each(function () {
            this.checked = checked;

            if (checked) {
                common.disableElement($(this));
            } else {
                common.enableElement($(this));
            }
        });
    }

    function copyBetSettingFrom1stRowAndDisableOthers(sportType) {
        var minBetInput = $("#minbet_" + sportType),
            maxBetInput = $("#maxbet_" + sportType),
            maxPerMatchInput = $("#maxpermatch_" + sportType);

        common.enableElement(minBetInput);
        common.enableElement(maxBetInput);
        common.enableElement(maxPerMatchInput);

        var minBet = minBetInput.val(),
            maxBet = maxBetInput.val(),
            maxPerMatch = maxPerMatchInput.val();

        var tableRow = null, sportTypeRow = null;

        for (var i = 1, max = betSettingTableRows.length; i < max; i++) {
            tableRow = $(betSettingTableRows[i]);
            sportTypeRow = tableRow.data("sporttype");
            common.disableElement(tableRow.find("#minbet_" + sportTypeRow)).val(minBet);
            common.disableElement(tableRow.find("#maxbet_" + sportTypeRow)).val(maxBet);
            common.disableElement(tableRow.find("#maxpermatch_" + sportTypeRow)).val(maxPerMatch);
        }
    }

    function copyBetSettingFrom1stRow_KeyUp(input) {
        var checked = copySettingIcon.hasClass("icon-copy-setting-checked");

        if (checked) {
            var number = input.value;
            var id = input.id;
            var idSubName = id.substring(0, id.indexOf("_"));

            numericInputs.each(function () {
                if ($(this).is(":disabled") && this.id.indexOf(idSubName) >= 0) {
                    this.value = number;
                }
            });
        }
    }

    function validateBetSetting(tableRow) {
        var isValid = true,
            sportType = tableRow.data("sporttype"),
            minBetInput = tableRow.find("#minbet_" + sportType),
            minBet = parseInt(minBetInput.val().replace(/,/g, "")),
            maxBetInput = tableRow.find("#maxbet_" + sportType),
            maxBet = parseInt(maxBetInput.val().replace(/,/g, "")),
            maxPerMatchInput = tableRow.find("#maxpermatch_" + sportType),
            maxPerMatch = parseInt(maxPerMatchInput.val().replace(/,/g, ""));

        minBetInput.removeClass("invalid-value");
        maxBetInput.removeClass("invalid-value");
        maxPerMatchInput.removeClass("invalid-value");

        if (!$.isNumeric(minBet)) {
            isValid = false;
            minBetInput.addClass("invalid-value");
        }

        if (!$.isNumeric(maxBet)) {
            isValid = false;
            maxBetInput.addClass("invalid-value");
        }

        if (!$.isNumeric(maxPerMatch)) {
            isValid = false;
            maxPerMatchInput.addClass("invalid-value");
        }

        if (minBet > maxBet) {
            isValid = false;
            minBetInput.addClass("invalid-value");
            maxBetInput.addClass("invalid-value");
        }

        if (maxBet > maxPerMatch) {
            isValid = false;
            maxBetInput.addClass("invalid-value");
            maxPerMatchInput.addClass("invalid-value");
        }

        if (!isValid) {
            globalValid = false;
        } else {
            var oldMinBet = parseInt(minBetInput.data("oldvalue"));
            var oldMaxBet = parseInt(maxBetInput.data("oldvalue"));
            var oldMaxPerMatch = parseInt(maxPerMatchInput.data("oldvalue"));

            return {
                SportType: sportType,
                SportTypeName: tableRow.data("sporttypename"),
                BetType: tableRow.data("bettype"),
                MinBet: minBet,
                MaxBet: maxBet,
                MaxPerMatch: maxPerMatch,
                Updated: minBet !== oldMinBet || maxBet !== oldMaxBet || maxPerMatch !== oldMaxPerMatch
            };
        }
    }

    function getBetSettings() {
        var setting = null, betSettings = [];

        for (var i = 0, max = betSettingTableRows.length; i < max; i++) {
            setting = validateBetSetting($(betSettingTableRows[i]));

            if (setting) {
                betSettings.push(setting);
            }
        }

        return betSettings;
    }

    function showReviewForm(betSettings) {
        var setting, html = [];

        for (var i = 0, max = betSettings.length; i < max; i++) {
            setting = betSettings[i];
            html.push("<tr>");
            html.push('<td class="l">');
            html.push(setting.SportTypeName);
            html.push("</td>");
            html.push('<td class="r">');
            html.push(setting.MinBet.toLocaleString("en-US"));
            html.push("</td>");
            html.push('<td class="r">');
            html.push(setting.MaxBet.toLocaleString("en-US"));
            html.push("</td>");
            html.push('<td class="r">');
            html.push(setting.MaxPerMatch.toLocaleString("en-US"));
            html.push("</td>");
            html.push("<td>");
            html.push(setting.Updated ? "X" : "");
            html.push("</td>");
            html.push('<td class="l"><div id="message_');
            html.push(setting.SportType);
            html.push('"></div></td>');
            html.push("</tr>");
        }

        reviewSettingTable.html(html.join(""));
        applyMembersSpan.text(applyMembersCheckbox.is(":checked") ? "Yes" : "No");
        updateSection.addClass("hide");
        reviewSection.removeClass("hide");
    }

    function nextButton_click() {
        common.blockUI();
        common.hideMessage();

        globalValid = true;
        globalBetSettings = getBetSettings();

        if (globalValid) {
            var updatedArray = $.grep(globalBetSettings, function (item, i) {
                return item.Updated;
            });

            if (updatedArray.length === 0) {
                common.showNoChangeMessage();
            } else {
                showReviewForm(globalBetSettings);
            }
        }

        common.unblockUI();
    }

    function updateButton_click(defaultParam) {
        if (!globalBetSettings || globalBetSettings.length === 0) {
            return;
        }

        var updatedArray = $.grep(globalBetSettings, function (item, i) {
            return item.Updated;
        });

        if (updatedArray.length === 0) {
            return;
        }

        common.disableElement(updateButton);
        var updateUrl = rootUrl + "LicenseeDcsSetting/UpdateBetSetting";
        var applyExistedMem = applyMembersCheckbox.is(":checked");

        $.each(updatedArray, function (i, item) {
            var messageDiv = $("#message_" + item.SportType);
            messageDiv.html('<span class="btnRefresh"></span> Updating ...');

            $.ajax({
                type: "POST",
                url: updateUrl,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify($.extend({ ApplyExistedMem: applyExistedMem, BetSettings: [item] }, defaultParam)),
                success: function (data) {
                    messageDiv.removeClass().html(data.ErrorMessage);
                    messageDiv.addClass(data.ErrorCode === 0 ? "msgcorrect" : "msgerror");
                },
                error: function (jqxhr) {
                    messageDiv.removeClass().html(jqxhr.statusText).addClass("msgerror");
                }
            });
        });
    }

    function btnBack_click(reloadFunction) {
        if (updateButton.is(":disabled")) {
            reloadFunction();
        } else {
            updateSection.removeClass("hide");
            reviewSection.addClass("hide");
        }
    }

    function initialize(defaultParam, reloadFunction) {
        globalValid = true;
        globalBetSettings = null;
        updateSection = $("#update_section");
        applyMembersCheckbox = updateSection.find("#apply_members_checkbox");
        betSettingTableRows = updateSection.find("#betsetting_table tbody tr");

        allSportsCheckbox = updateSection.find("#allsports_checkbox")
            .off("change")
            .on("change", allSportsCheckbox_change);

        sportTypeCheckboxes = updateSection.find(".sporttype-checkbox")
            .off("change")
            .on("change", sportTypeCheckbox_change);

        copySettingIcon = updateSection.find("#copy_setting_icon")
            .off("click")
            .on("click", copySettingIcon_click);

        nextButton = updateSection.find("#btnNext")
            .off("click")
            .on("click", nextButton_click);

        numericInputs = updateSection.find(".numeric");
        numericInputs.forceNumber(copyBetSettingFrom1stRow_KeyUp);

        reviewSection = $("#review_section");
        applyMembersSpan = reviewSection.find("#apply_members_span");
        reviewSettingTable = reviewSection.find("#reviewsetting_table tbody");

        updateButton = reviewSection.find("#btnBetSettingUpdate")
            .off("click")
            .on("click", function () { updateButton_click(defaultParam); });

        backButton = reviewSection.find("#btnBack")
            .off("click")
            .on("click", function () { btnBack_click(reloadFunction); });
    }

    return { initialize: initialize };
})(jQuery, common, rootUrl);