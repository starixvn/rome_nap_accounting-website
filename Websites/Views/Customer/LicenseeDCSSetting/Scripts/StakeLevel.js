var stakeLevelHandler = (function ($, common, rootUrl) {
    "use strict";
    var allBetTypesCheckbox,
        betTypeCheckboxes,
        stakeLevelCheckboxes,
        originalValue;

    function allBetTypesCheckbox_change() {
        var checked = this.checked;

        betTypeCheckboxes.each(function () {
            this.checked = checked;
        });

        stakeLevelCheckboxes.each(function () {
            this.checked = checked;
        });
    }

    function betTypeCheckbox_change() {
        var $this = $(this),
            sportType = $this.data("sporttype"),
            betType = $this.data("bettype"),
            checked = this.checked,
            isCheckAll = checked;

        stakeLevelCheckboxes.each(function () {
            var $this = $(this);
            if ($this.data("sporttype") === sportType && $this.data("bettype") === betType) {
                this.checked = checked;
            }
        });

        for (var i = 0, max = betTypeCheckboxes.length; i < max; i++) {
            if (betTypeCheckboxes[i].checked !== checked) {
                isCheckAll = false;
                break;
            }
        }

        allBetTypesCheckbox.prop("checked", isCheckAll);
    }

    function stakeLevelCheckbox_change() {
        var $this = $(this),
            sportType = $this.data("sporttype"),
            betType = $this.data("bettype"),
            checked = this.checked,
            isCheckBetType = checked,
            isCheckAll = checked,
            i,
            totalItems,
            checkbox;

        for (i = 0, totalItems = stakeLevelCheckboxes.length; i < totalItems; i++) {
            checkbox = $(stakeLevelCheckboxes[i]);

            if (checkbox.data("sporttype") === sportType &&
                checkbox.data("bettype") === betType &&
                checkbox.is(":checked") !== checked) {
                isCheckBetType = false;
                break;
            }
        }

        for (i = 0, totalItems = betTypeCheckboxes.length; i < totalItems; i++) {
            checkbox = $(betTypeCheckboxes[i]);

            if (checkbox.data("sporttype") === sportType && checkbox.data("bettype") === betType) {
                checkbox.prop("checked", isCheckBetType);
            }

            if (checkbox.is(":checked") !== checked) {
                isCheckAll = false;
            }
        }

        allBetTypesCheckbox.prop("checked", isCheckAll);
    }

    function updateForm_submit(defaultParam, reloadFunction, getSelectedAgents) {
        common.blockUI();
        common.hideMessage();

        var stakeLevels = [];
        stakeLevelCheckboxes.each(function () {
            var $this = $(this);

            stakeLevels.push({
                SportType: $this.data("sporttype"),
                BetType: $this.data("bettype"),
                StakeLevelId: parseInt($this.attr("name")),
                IsChecked: this.checked
            });
        });
        var isAgentOnly = $("#agentOnlyCheckbox").is(":checked");
        var selectedAgentIds = getSelectedAgents().AgentIds;

        $.ajax({
            type: "POST",
            url: rootUrl + "LicenseeDcsSetting/UpdateStakeLevel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify($.extend({ StakeLevels: stakeLevels, AgentIds: selectedAgentIds, IsAgentOnly: isAgentOnly }, defaultParam)),
            success: function (data) {
                common.showMessage(data.ErrorCode, data.ErrorMessage);
                if (data.ErrorCode === 0) {
                    setTimeout(reloadFunction, 3000);
                }
            },
            error: common.showInternalServerError,
            complete: common.unblockUI
        });
    }

    function initialize(defaultParam, reloadFunction, getSelectedAgents) {
        stakeLevelCheckboxes = $(".stakelevel-checkbox")
            .off("change")
            .on("change", stakeLevelCheckbox_change);

        var isCheckAll = true;
        betTypeCheckboxes = $(".bettype-checkbox");
        betTypeCheckboxes.each(function () {
            $(this)
                .off("change")
                .on("change", betTypeCheckbox_change);

            if (!this.checked) {
                isCheckAll = false;
            }
        });

        allBetTypesCheckbox = $("#allbettypes_checkbox")
            .off("change")
            .on("change", allBetTypesCheckbox_change)
            .prop("checked", isCheckAll);

        var stakeLevelForm = $("#formStakeLevel")
            .off("submit")
            .on("submit", function (e) {
                e.preventDefault();

                if (stakeLevelForm.serialize() === originalValue) {
                    common.showNoChangeMessage();
                } else {
                    updateForm_submit(defaultParam, reloadFunction, getSelectedAgents);
                }
            });

        originalValue = stakeLevelForm.serialize();
    }

    return { initialize: initialize };
})(jQuery, common, rootUrl);