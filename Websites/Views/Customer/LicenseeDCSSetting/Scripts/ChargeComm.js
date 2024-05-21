var chargeCommissionHandler = (function ($, common, rootUrl) {
    "use strict";
    var updateButton,
        originalCommission;

    function commDropDown_change() {
        if (this.value === originalCommission) {
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
            url: rootUrl + "LicenseeDcsSetting/UpdateChargeCommission",
            type: 'POST',
            dataType: 'json',
            data: formData,
            success: function (data) {
                common.showMessage(data.ErrorCode, data.ErrorMessage);
                if (data.ErrorCode === 0) {
                    originalCommission = formData.Commission;
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
        var commDropDown = $(".dropdowncomm")
            .off("change")
            .on("change", commDropDown_change)
            .select2();

        updateButton = $("#btnUpdateComm")
            .off("click")
            .on("click", function () {
                var comm = [];
                var rows = $(".row_js");
                rows.each(function (index, option) {
                    var sportType = $(option).attr("data-sporttype");
                    var bettype = $(option).attr("data-bettype");
                    var comm_new = $("#commDropDown_" + bettype + "_" + sportType).val();
                    comm.push({
                        SportType: sportType,
                        BetType: bettype,
                        Discount: comm_new
                    });
                });
                
                var formData = JSON.stringify($.extend({ 'Commission': comm}, defaultParam));
                updateButton_click(formData);
            });

        originalCommission = commDropDown.val();
    }


    return { initialize: initialize };
})(jQuery, common, rootUrl);