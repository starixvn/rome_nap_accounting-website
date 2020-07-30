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

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            url: rootUrl + "LicenseeDcsSetting/UpdatePT",
            type: 'POST',
            dataType: 'json',
            data: formData,
            success: function (data) {
                common.showMessage(data.ErrorCode, data.ErrorMessage);
                if (data.ErrorCode === 0) {
                    originalPT = formData.SuperPT;
                    setTimeout(common.hideMessage, 3000);
                } else {
                    common.enableElement(updateButton);
                }
            },
            error: function (jqxhr) {
                common.showInternalServerError(jqxhr);
                common.enableElement(updateButton);
            },
            complete: function () {
                common.unblockUI();
            }
        });
    }

    function initialize(defaultParam, getSelectedAgents) {
        var ptDropDown = $(".dropdownpt")
            .off("change")
            .on("change", ptDropDown_change)
            .select2();

        updateButton = $("#btnUpdatePT")
            .off("click")
            .on("click", function () {
                var isAgentOnly = $("#agentOnlyCheckbox").is(":checked");
                var selectedAgentIds = getSelectedAgents().AgentIds;
                var pts = [];
                var rows = $(".ptrow_js");
                rows.each(function (index, option) {
                    var sportType = $(option).attr("data-sporttype");
                    var bettype = $(option).attr("data-bettype");
                    var isLive = $(option).attr("data-islive");
                    var pt_old = $(option).attr("data-pt");
                    var pt_new = $("#ptDropDown" + bettype).val();
                    if (pt_old !== pt_new) {
                        pts.push({
                            SportType: sportType,
                            BetType: bettype,
                            AgentPT: pt_new,
                            IsLive: isLive
                        });
                    }
                });
                var formData = JSON.stringify($.extend({ 'SuperPT': pts, 'AgentIds': selectedAgentIds, 'IsAgentOnly': isAgentOnly }, defaultParam));
                updateButton_click(formData);
            });

        originalPT = ptDropDown.val();
    }


    return { initialize: initialize };
})(jQuery, common, rootUrl);