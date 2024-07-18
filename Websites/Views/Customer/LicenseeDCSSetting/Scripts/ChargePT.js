var chargePTHandler = (function ($, common, rootUrl) {
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
            url: rootUrl + "LicenseeDcsSetting/UpdateChargePT",
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
                common.enableElement(updateButton);
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
                var pts = [];
                var rows = $(".ptrow_js");
                rows.each(function (index, option) {
                    var sportType = $(option).attr("data-sporttype");
                    var pt_new = $("#ptDropDown_" + sportType).val();
                    pts.push({
                        SportType: sportType,
                        PT: pt_new
                    });
                });
                
                var formData = JSON.stringify($.extend({ 'SuperPT': pts}, defaultParam));
                updateButton_click(formData);
            });

        originalPT = ptDropDown.val();
    }


    return { initialize: initialize };
})(jQuery, common, rootUrl);