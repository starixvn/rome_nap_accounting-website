var ptHandler = (function ($, common, rootUrl) {
    "use strict";
    var updateButton,
        originalPT;

    function ptDropDown_change() {
        if (this.value === originalPT) {
            common.disableElement(updateButton);
        } else {
            common.enableElement(updateButton);
        }
    }

    function updateButton_click(formData) {
        common.blockUI();
        common.disableElement(updateButton);

        $.post(
            rootUrl + "LicenseeDcsSetting/UpdatePT",
            formData,
            function (data) {
                common.showMessage(data.ErrorCode, data.ErrorMessage);
                if (data.ErrorCode === 0) {
                    originalPT = formData.SuperPT;
                    setTimeout(common.hideMessage, 3000);
                } else {
                    common.enableElement(updateButton);
                }
            }).fail(function (jqxhr) {
                common.showInternalServerError(jqxhr);
                common.enableElement(updateButton);
            }).always(common.unblockUI);
    }

    function initialize(defaultParam, getSelectedAgents) {
        var ptDropDown = $("#ptDropDown")
            .off("change")
            .on("change", ptDropDown_change)
            .select2();

        updateButton = $("#btnUpdatePT")
            .off("click")
            .on("click", function () {
                var isAgentOnly = $("#agentOnlyCheckbox").is(":checked");
                var selectedAgentIds = getSelectedAgents().AgentIds;
                var formData = $.extend({ SuperPT: ptDropDown.val(), AgentIds: selectedAgentIds, IsAgentOnly: isAgentOnly }, defaultParam);
                updateButton_click(formData);
            });

        originalPT = ptDropDown.val();
    }

    return { initialize: initialize };
})(jQuery, common, rootUrl);