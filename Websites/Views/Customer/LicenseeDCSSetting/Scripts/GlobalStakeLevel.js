var globalStakeLevelHandler = (function ($, common, rootUrl) {
    "use strict";
    var globalValid,
        globalStakeLevelTable,
        updateButton;

    function validateGlobalStakeLevel(tableRow) {
        var isValid = true,
            minStakeInput = tableRow.find(".minstake").first(),
            minStake = parseInt(minStakeInput.val().replace(/,/g, "")),
            maxStakeInput = tableRow.find(".maxstake").first(),
            maxStake = parseInt(maxStakeInput.val().replace(/,/g, "")),
            minLabelInput = tableRow.find(".minlabel").first(),
            minLabelValue = $.trim(minLabelInput.val()),
            minLabel = parseInt(minLabelValue.replace(/,/g, "")),
            maxLabelInput = tableRow.find(".maxlabel").first(),
            maxLabelValue = $.trim(maxLabelInput.val()),
            maxLabel = parseInt(maxLabelValue.replace(/,/g, ""));

        minStakeInput.removeClass("invalid-value");
        maxStakeInput.removeClass("invalid-value");
        minLabelInput.removeClass("invalid-value");
        maxLabelInput.removeClass("invalid-value");

        if (!$.isNumeric(minStake)) {
            isValid = false;
            minStakeInput.addClass("invalid-value");
        }

        if (!$.isNumeric(maxStake)) {
            isValid = false;
            maxStakeInput.addClass("invalid-value");
        }

        if (!$.isNumeric(minLabel)) {
            isValid = false;
            minLabelInput.addClass("invalid-value");
        }

        if (!$.isNumeric(maxLabel)) {
            isValid = false;
            maxLabelInput.addClass("invalid-value");
        }

        if (minStake > maxStake) {
            isValid = false;
            minStakeInput.addClass("invalid-value");
            maxStakeInput.addClass("invalid-value");
        }

        if (minLabel > maxLabel) {
            isValid = false;
            minLabelInput.addClass("invalid-value");
            maxLabelInput.addClass("invalid-value");
        }

        if (!isValid) {
            globalValid = false;
        } else {
            var oldMinStake = parseInt(minStakeInput.data("oldvalue")),
                oldMaxStake = parseInt(maxStakeInput.data("oldvalue")),
                oldMinLabel = minLabelInput.data("oldvalue"),
                oldMaxLabel = maxLabelInput.data("oldvalue");
            // Note: MinLabel and MaxLabel cannot use operator !==
            return {
                GameTypeId: tableRow.data("gametypeid"),
                StakeLevelId: tableRow.data("stakelevelid"),
                MinStake: minStake,
                MaxStake: maxStake,
                MinLabel: minLabelValue,
                MaxLabel: maxLabelValue
            };    
        }
    }

    function getGlobalStakeLevels() {
        var globalStakeLevels = [];

        globalStakeLevelTable.find("tr[data-gametypeid]").each(function () {
            var setting = validateGlobalStakeLevel($(this));

            if (setting) {
                globalStakeLevels.push(setting);
            }
        });

        return globalStakeLevels;
    }

    function addButton_click() {
        var $this = $(this),
            gameTypeId = $this.data("gametypeid"),
            lastRow = globalStakeLevelTable.find('tr[data-gametypeid="' + gameTypeId + '"]').last();

        if (lastRow.length) {
            var html = ['<tr data-gametypeid="', gameTypeId, '" data-stakelevelid="0">',
                '<td><img class="hand remove" src="../App_Themes/Images/icn_del.png" title="Remove Global Bet Limit"></td>',
                '<td><input type="text" class="numeric minstake" data-oldvalue="0" value="0"></td>',
                '<td><input type="text" class="numeric maxstake" data-oldvalue="0" value="0"></td>',
                '<td><input type="text" class="numeric minlabel" data-oldvalue="0" value="0"></td>',
                '<td><input type="text" class="numeric maxlabel" data-oldvalue="0" value="0"></td></tr>'].join("");

            lastRow.after(html);
            var newRow = lastRow.next();

            if (newRow.length) {
                var removeIcon = newRow.find(".remove").first();
                removeIcon.one("click", function () { removeIcon.closest("tr").remove(); });
                newRow.find(".numeric").forceNumber();
            }
        }
    }

    function updateButton_click(defaultParam, reloadFunction) {
        common.blockUI();
        common.hideMessage();
        common.disableElement(updateButton);
        globalValid = true;
        var globalStakeLevels = getGlobalStakeLevels();

        if (!globalValid) {
            common.unblockUI();
            common.enableElement(updateButton);
        } 
        else {
            $.ajax({
                type: "POST",
                url: rootUrl + "LicenseeDcsSetting/UpdateGlobalStakeLevel",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify($.extend({ GlobalStakeLevels: globalStakeLevels }, defaultParam)),
                success: function (data) {
                    common.showMessage(data.ErrorCode, data.ErrorMessage);
                    if (data.ErrorCode === 0) {
                        setTimeout(reloadFunction, 3000);
                    } else {
                        common.enableElement(updateButton);
                    }
                },
                error: common.showInternalServerError,
                complete: common.unblockUI
            });
        }
    }

    function initialize(defaultParam, reloadFunction) {
        globalValid = true;
        globalStakeLevelTable = $("#globalStakeLevel_table tbody");

        $(".btnAdd").each(function () {
            $(this).off("click")
                .on("click", addButton_click);
        });

        updateButton = $("#btnUpdateGlobalStakeLevel")
            .off("click")
            .on("click", function () { updateButton_click(defaultParam, reloadFunction); });

        $(".numeric").forceNumber();
    }

    return { initialize: initialize };
})(jQuery, common, rootUrl);