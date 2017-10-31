(function ($) {
    "use strict";
    function blockUI() {
        try { parent.BlockUI(); } catch (e) { console.error(e); }
    }
    function unblockUI() {
        try { parent.jQuery.unblockUI(); } catch (e) { console.error(e); }
    }
    function showMessage(type, message) {
        var html = [
            '<div class="',
            type === 0 ? "alertboxscc" : "alertboxerr",
            '"><div>',
            message,
            "</div></div>"
        ].join("");

        var messageDiv = $("#sectionMessage");
        return messageDiv.html(html).removeClass("hide");
    }
    function hideMessage() {
        var messageDiv = $("#sectionMessage");
        return messageDiv.html("").addClass("hide");
    }
    function enableElement(element) {
        return element.removeAttr("disabled");
    }
    function disableElement(element) {
        return element.attr("disabled", "disabled");
    }
    /* forceNumber()
 * ==========
 * Allow input Number only.
 *
 * @usage: $(selector).forceNumber();
 *
 */
    $.fn.forceNumber = function (keyupCallback) {
        var number_0 = 48,
            number_9 = 57,
            numpad_0 = 96,
            numpad_9 = 105,
            end = 35,
            home = 36,
            leftArrow = 37,
            rightArrow = 39,
            backspace = 8,
            tab = 9,
            enter = 13,
            escape = 27,
            del = 46,
            a = 65,
            c = 67,
            v = 86,
            x = 88;

        function isNumeric(text) {
            var regExp = /^-?(\d+|\d+\.\d*|\d*\.\d+)$/;
            return text.match(regExp);
        }

        function keydownHandler(e) {
            var code = e.which || e.keyCode;
            if (number_0 <= code && code <= number_9 ||
                numpad_0 <= code && code <= numpad_9 ||
                code === end ||
                code === home ||
                code === leftArrow ||
                code === rightArrow ||
                code === backspace ||
                code === tab ||
                code === enter ||
                code === escape ||
                code === del) {
                return true;
            }
            var ctrlKey = e.ctrlKey;
            var metaKey = e.metaKey;
            return (code === a || code === c || code === v || code === x) && (ctrlKey || metaKey);
        }

        function keyupHandler(e) {
            var text = this.value;
            if (text.length === 0) {
                return;
            }
            text = text.replace(/\,/g, "");
            if (isNumeric(text)) {
                var number = parseInt(text).toLocaleString("en-US");
                this.value = number;
                if (keyupCallback && typeof keyupCallback === "function") {
                    keyupCallback(this);
                }
            }
        }

        this.keydown(keydownHandler)
            .keyup(keyupHandler);
    };
    /* PT()
     * ==========
     * Get/Set Position Talking.
     *
     * @usage: var ptHandler = new PT();
     *
     */
    function PT(defaultParam) {
        function ptDropDown_change() {
            if (this.value === originalPT) {
                disableElement(updateButton);
            } else {
                enableElement(updateButton);
            }
        }

        function updateButton_click() {
            blockUI();
            disableElement(updateButton);
            var formData = {
                SuperPT: ptDropDown.val()
            };
            var jqxhr = $.post(
                rootUrl + "LicenseeDcsSetting/UpdatePT",
                $.extend(formData, defaultParam),
                function (data) {
                    showMessage(data.ErrorCode, data.ErrorMessage);
                    if (data.ErrorCode === 0) {
                        setTimeout(function () {
                            hideMessage();
                            originalPT = formData.SuperPT;
                        }, 3000);
                    } else {
                        enableElement(updateButton);
                    }
                }).fail(function () {
                    showMessage(500, jqxhr.statusText);
                    enableElement(updateButton);
                }).always(unblockUI);
        }

        var ptDropDown = $("#ptDropDown")
            .off("change")
            .on("change", ptDropDown_change)
            .select2();
        var updateButton = $("#btnUpdatePT")
            .off("click")
            .on("click", updateButton_click);
        var originalPT = ptDropDown.val();
    }
    /* BetSetting()
     * ==========
     * Get/Set Bet Setting.
     *
     * @usage: var betSettingHandler = new BetSetting();
     *
     */
    function BetSetting(defaultParam, reloadFunction) {
        function enableNumericInputs(sportType) {
            var inputs = !sportType ?
                numericInputs :
                $.grep(numericInputs, function (item, i) { return $(item).data("sporttype") === sportType; });

            $.each(inputs, function (i, item) {
                enableElement($(item));
            });
        }

        function disableNumericInputs(sportType) {
            var inputs = !sportType ?
                numericInputs :
                $.grep(numericInputs, function (item, i) { return $(item).data("sporttype") === sportType; });

            $.each(inputs, function (i, item) {
                var $item = $(item);
                $item.removeClass("invalid-value");
                disableElement($item);
                var number = parseInt($item.data("oldvalue"));
                $item.val(number.toLocaleString("en-US"));
            });
        }

        function allSportsCheckbox_change() {
            var checked = this.checked;
            if (checked) {
                enableNumericInputs();
            } else {
                disableNumericInputs();
            }
            $.each(sportTypeCheckboxes, function (i, cb) {
                cb.checked = checked;
            });
        }

        function sportTypeCheckbox_change() {
            var sportType = $(this).data("sporttype");
            var checked = this.checked;
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
                enableElement(allSportsCheckbox);
                disableNumericInputs();
            } else {
                checked = true;
                copySettingIcon.addClass("icon-copy-setting-checked");
                var sportType = $(this).data("sporttype");
                copyBetSettingFrom1stRowAndDisableOthers(sportType);
                disableElement(allSportsCheckbox);
            }
            allSportsCheckbox.prop("indeterminate", false).prop("checked", checked);
            $.each(sportTypeCheckboxes, function (i, cb) {
                cb.checked = checked;
                if (checked) {
                    disableElement($(cb));
                } else {
                    enableElement($(cb));
                }
            });
        }

        function copyBetSettingFrom1stRowAndDisableOthers(sportType) {
            var minBetInput = $("#minbet_" + sportType);
            var maxBetInput = $("#maxbet_" + sportType);
            var maxPerMatchInput = $("#maxpermatch_" + sportType);
            enableElement(minBetInput);
            enableElement(maxBetInput);
            enableElement(maxPerMatchInput);
            var minBet = minBetInput.val();
            var maxBet = maxBetInput.val();
            var maxPerMatch = maxPerMatchInput.val();
            var tableRow = null, sportTypeRow = null;
            for (var i = 1, max = betSettingTableRows.length; i < max; i++) {
                tableRow = $(betSettingTableRows[i]);
                sportTypeRow = tableRow.data("sporttype");
                disableElement(tableRow.find("#minbet_" + sportTypeRow)).val(minBet);
                disableElement(tableRow.find("#maxbet_" + sportTypeRow)).val(maxBet);
                disableElement(tableRow.find("#maxpermatch_" + sportTypeRow)).val(maxPerMatch);
            }
        }

        function copyBetSettingFrom1stRow_KeyUp(input) {
            var checked = copySettingIcon.hasClass("icon-copy-setting-checked");
            if (checked) {
                var number = input.value;
                var id = input.id;
                var idSubName = id.substring(0, id.indexOf("_"));
                $.each(numericInputs, function (i, item) {
                    if ($(item).is(":disabled") && item.id.indexOf(idSubName) >= 0) {
                        item.value = number;
                    }
                });
            }
        }

        function validateBetSetting(tableRow) {
            var isValid = true;
            var sportType = tableRow.data("sporttype");
            var minBetInput = tableRow.find("#minbet_" + sportType);
            var maxBetInput = tableRow.find("#maxbet_" + sportType);
            var maxPerMatchInput = tableRow.find("#maxpermatch_" + sportType);
            minBetInput.removeClass("invalid-value");
            maxBetInput.removeClass("invalid-value");
            maxPerMatchInput.removeClass("invalid-value");
            var minBet = parseInt(minBetInput.val().replace(/,/g, ""));
            var oldMinBet = parseInt(minBetInput.data("oldvalue"));
            var maxBet = parseInt(maxBetInput.val().replace(/,/g, ""));
            var oldMaxBet = parseInt(maxBetInput.data("oldvalue"));
            var maxPerMatch = parseInt(maxPerMatchInput.val().replace(/,/g, ""));
            var oldMaxPerMatch = parseInt(maxPerMatchInput.data("oldvalue"));
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
            blockUI();
            hideMessage();
            globalValid = true;
            globalBetSettings = getBetSettings();
            if (globalValid) {
                var updatedArray = $.grep(globalBetSettings, function (item, i) {
                    return item.Updated;
                });
                if (updatedArray.length === 0) {
                    showMessage(1, "There is no change to update.");
                } else {
                    showReviewForm(globalBetSettings);
                }
            }
            unblockUI();
        }

        function updateButton_click() {
            if (!globalBetSettings || globalBetSettings.length === 0) {
                return;
            }
            var updatedArray = $.grep(globalBetSettings, function (item, i) {
                return item.Updated;
            });
            if (updatedArray.length === 0) {
                return;
            }
            disableElement(updateButton);
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

        function btnBack_click() {
            if (updateButton.is(":disabled")) {
                reloadFunction();
            } else {
                updateSection.removeClass("hide");
                reviewSection.addClass("hide");
            }
        }

        var globalValid = true;
        var globalBetSettings = null;
        var updateSection = $("#update_section");
        var applyMembersCheckbox = $("#apply_members_checkbox");
        var betSettingTableRows = $("#betsetting_table tbody tr");
        var allSportsCheckbox = $("#allsports_checkbox")
            .off("change")
            .on("change", allSportsCheckbox_change);
        var sportTypeCheckboxes = $(".sporttype-checkbox")
            .off("change")
            .on("change", sportTypeCheckbox_change);
        var copySettingIcon = $("#copy_setting_icon")
            .off("click")
            .on("click", copySettingIcon_click);
        var nextButton = $("#btnNext")
            .off("click")
            .on("click", nextButton_click);
        var numericInputs = $(".numeric");
        numericInputs.forceNumber(copyBetSettingFrom1stRow_KeyUp);
        var reviewSection = $("#review_section");
        var applyMembersSpan = $("#apply_members_span");
        var reviewSettingTable = $("#reviewsetting_table tbody");
        var updateButton = $("#btnBetSettingUpdate")
            .off("click")
            .on("click", updateButton_click);
        var backButton = $("#btnBack")
            .off("click")
            .on("click", btnBack_click);
    }
    /* Status()
     * ==========
     * Enable/Disable Status function.
     *
     * @usage: var statusHandler = new Status();
     *
     */
    function Status(defaultParam, reloadFunction) {
        function disabledCheckbox_change() {
            var checked = this.checked;
            if (!originalData.IsDisabledDownline) {
                disabledDownlineCheckbox.prop("checked", checked);
            } else {
                if (checked) {
                    enableElement(disabledDownlineCheckbox);
                } else {
                    disabledDownlineCheckbox.prop("checked", checked);
                    disableElement(disabledDownlineCheckbox);
                }
            }
            if (checked === originalData.IsDisabled ||
                disabledDownlineCheckbox.is(":checked") === originalData.IsDisabledDownline) {
                enableElement(updateButton);
            } else {
                disableElement(updateButton);
            }
        }

        function disabledDownlineCheckbox_change() {
            if (disabledCheckbox.is(":checked") === originalData.IsDisabled ||
                this.checked === originalData.IsDisabledDownline) {
                enableElement(updateButton);
            } else {
                disableElement(updateButton);
            }
        }

        function updateButton_click() {
            blockUI();
            disableElement(updateButton);
            var formData = {
                SportType: $("#hiddenSportType").val(),
                BetType: $("#hiddenBetType").val(),
                BitmapOrder: $("#hiddenBitmapOrder").val(),
                IsDisabled: !disabledCheckbox.is(":checked"),
                IsDisabledDownline: !disabledDownlineCheckbox.is(":checked")
            };
            var jqxhr = $.post(
                rootUrl + "LicenseeDcsSetting/UpdateStatus",
                $.extend(formData, defaultParam),
                function (data) {
                    showMessage(data.ErrorCode, data.ErrorMessage);
                    if (data.ErrorCode === 0) {
                        setTimeout(reloadFunction, 3000);
                    } else {
                        enableElement(updateButton);
                    }
                }).fail(function () {
                    showMessage(500, jqxhr.statusText);
                    enableElement(updateButton);
                }).always(unblockUI);
        }

        var disabledCheckbox = $("#disabledCheckbox")
            .off("change")
            .on("change", disabledCheckbox_change);
        var disabledDownlineCheckbox = $("#disabledDownlineCheckbox")
            .off("change")
            .on("change", disabledDownlineCheckbox_change);
        var updateButton = $("#btnUpdateStatus")
            .off("click")
            .on("click", updateButton_click);
        var originalData = {
            IsDisabled: !disabledCheckbox.is(":checked"),
            IsDisabledDownline: !disabledDownlineCheckbox.is(":checked")
        };
    }

    var boxContentDiv = $("#boxContent");
    var siteDropDown = $("#siteDropDown");
    var superDropDown = $("#superDropDown");
    var productDropDown = $("#productDropDown");
    var functionDropDown = $("#functionDropDown");

    function resetFrom() {
        hideMessage();
        boxContentDiv.addClass("hide").empty();
    }

    function renderDropDown(element, data) {
        var datum;
        var html = [];
        for (var i = 0, max = data.length; i < max; i++) {
            datum = data[i];
            html.push('<option value="');
            html.push(datum.Id);
            html.push('">');
            html.push(datum.Name);
            html.push("</option>");
        }
        return element.html(html.join(""));
    }

    function siteDropDown_change() {
        blockUI();
        resetFrom();
        var siteId = this.value;
        var jqxhr = $.post(
            rootUrl + "LicenseeDcsSetting/Supers",
            { site: siteId },
            function (data) {
                renderDropDown(superDropDown, data);
            }).always(unblockUI);
    }

    function productDropDown_change() {
        resetFrom();
        var productId = parseInt(this.value);
        var setting = $.grep(productJSON, function (item, i) {
            return item.Id === productId;
        })[0];
        renderDropDown(functionDropDown, setting.SettingTypes);
    }

    function selectButton_click() {
        blockUI();
        resetFrom();
        var filterParam = {
            Site: siteDropDown.val(),
            ProductId: productDropDown.val(),
            ProductName: productDropDown.find("option:selected").text(),
            SuperId: superDropDown.val(),
            SuperName: superDropDown.find("option:selected").text()
        };
        var url;
        var functionId = functionDropDown.val();
        switch (functionId) {
            case "1":
                url = "LicenseeDcsSetting/PT";
                break;
            case "2":
                url = "LicenseeDcsSetting/BetSetting";
                break;
            case "4":
                url = "LicenseeDcsSetting/Status";
                break;
            default:
                return;
        }
        var jqxhr = $.post(
            rootUrl + url,
            filterParam,
            function (data) {
                boxContentDiv.removeClass("hide").html(data);
                var defaultParam = {
                    ProductId: filterParam.ProductId,
                    SuperId: filterParam.SuperId
                };
                switch (functionId) {
                    case "1":
                        var ptHandler = new PT(defaultParam);
                        break;
                    case "2":
                        var betSettingHandler = new BetSetting(defaultParam, selectButton_click);
                        break;
                    case "4":
                        var statusHandler = new Status(defaultParam, selectButton_click);
                        break;
                    default:
                        break;
                }
            }).always(unblockUI);
    }

    function initialize() {
        siteDropDown.change(siteDropDown_change).select2();
        superDropDown.change(resetFrom).select2();
        productDropDown.change(productDropDown_change).select2({ minimumResultsForSearch: Infinity });
        functionDropDown.change(resetFrom).select2({ minimumResultsForSearch: Infinity });
        $("#selectButton").click(selectButton_click);
    }

    initialize();
})(jQuery);
