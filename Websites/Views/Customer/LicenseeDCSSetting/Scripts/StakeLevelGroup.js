var stakeLevelGroupHandler = (function ($, common, rootUrl) {
    "use strict";
    var allStakeLevelCheckbox,
        stakeLevelCheckboxes,
        originalValue;

    function allStakeLevelCheckbox_change() {
        var checked = this.checked;

        stakeLevelCheckboxes.each(function () {
            this.checked = checked;
        });
    }

    function stakeLevelCheckboxes_change() {
        var checked = this.checked;

        for (var i = 0, totalItems = stakeLevelCheckboxes.length; i < totalItems; i++) {
            if (stakeLevelCheckboxes[i].checked !== checked) {
                allStakeLevelCheckbox.prop("indeterminate", true);
                return;
            }
        }

        allStakeLevelCheckbox
            .prop("indeterminate", false)
            .prop("checked", checked);
    }

    function updateForm_submit(defaultParam, reloadFunction, getSelectedAgents) {
        common.blockUI();
        common.hideMessage();

        var stakeLevels = [];
        stakeLevelCheckboxes.each(function () {
            var $this = $(this);

            stakeLevels.push({
                SportType: $this.data("sporttype"),
                BetType: $this.data("bettyperef"),
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
        var stakeLevelGroupForm = $("#formStakeLevelGroup"),
            hideBetTypes = stakeLevelGroupForm.find(".hide"),
            viewMoreAnchor = stakeLevelGroupForm.find("#viewMoreAnchor");

        var isCheckAll = true,
            isIndeterminate = false;
        stakeLevelCheckboxes = $(".stakelevel-checkbox");
        stakeLevelCheckboxes.each(function () {
            $(this)
                .off("change")
                .on("change", stakeLevelCheckboxes_change);

            if (this.checked) {
                isIndeterminate = true;
            } else {
                isCheckAll = false;
            }
        });

        allStakeLevelCheckbox = $("#allStakeLevel_checkbox")
            .off("change")
            .on("change", allStakeLevelCheckbox_change);

        if (isCheckAll) {
            allStakeLevelCheckbox
                .prop("indeterminate", false)
                .prop("checked", true);
        } else if (isIndeterminate) {
            allStakeLevelCheckbox.prop("indeterminate", true);
        }

        viewMoreAnchor
            .off("click")
            .on("click", function () {
                if (this.text === "View More ...") {
                    hideBetTypes.removeClass("hide");
                    this.text = "View Less ...";
                } else {
                    hideBetTypes.addClass("hide");
                    this.text = "View More ...";
                }
            });

        stakeLevelGroupForm
            .off("submit")
            .on("submit", function (e) {
                e.preventDefault();

                if (stakeLevelGroupForm.serialize() === originalValue) {
                    common.showNoChangeMessage();
                } else {
                    updateForm_submit(defaultParam, reloadFunction, getSelectedAgents);
                }
            });

        originalValue = stakeLevelGroupForm.serialize();
    }

    return { initialize: initialize };
})(jQuery, common, rootUrl);