var statusHandler = (function ($, common, rootUrl) {
    "use strict";
    var disabledCheckbox,
        disabledDownlineCheckbox,
        updateButton,
        originalData;

    function disabledCheckbox_change() {
        var checked = this.checked;

        if (!originalData.IsDisabledDownline) {
            disabledDownlineCheckbox.prop("checked", checked);
        } else {
            if (checked) {
                common.enableElement(disabledDownlineCheckbox);
            } else {
                disabledDownlineCheckbox.prop("checked", checked);
                common.disableElement(disabledDownlineCheckbox);
            }
        }

        if (checked === originalData.IsDisabled ||
            disabledDownlineCheckbox.is(":checked") === originalData.IsDisabledDownline) {
            common.enableElement(updateButton);
        } else {
            common.disableElement(updateButton);
        }
    }

    function disabledDownlineCheckbox_change() {
        if (disabledCheckbox.is(":checked") === originalData.IsDisabled ||
            this.checked === originalData.IsDisabledDownline) {
            common.enableElement(updateButton);
        } else {
            common.disableElement(updateButton);
        }
    }

    function updateButton_click(formData, reloadFunction) {
        common.blockUI();
        common.disableElement(updateButton);

        $.ajax({
            type: "POST",
            url: rootUrl + "LicenseeDcsSetting/UpdateStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(formData),
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

    function initialize(defaultParam, reloadFunction) {
        var sportType = $("#hiddenSportType").val(),
            betType = $("#hiddenBetType").val(),
            bitmapOrder = $("#hiddenBitmapOrder").val();

        disabledCheckbox = $("#disabledCheckbox")
            .off("change")
            .on("change", disabledCheckbox_change);

        disabledDownlineCheckbox = $("#disabledDownlineCheckbox")
            .off("change")
            .on("change", disabledDownlineCheckbox_change);

        updateButton = $("#btnUpdateStatus")
            .off("click")
            .on("click", function () {
                var status = {
                    SportType: sportType,
                    BetType: betType,
                    BitmapOrder: bitmapOrder,
                    IsDisabled: !disabledCheckbox.is(":checked"),
                    IsDisabledDownline: !disabledDownlineCheckbox.is(":checked")
                };

                var formData = $.extend({ Status: status }, defaultParam);
                updateButton_click(formData, reloadFunction);
            });

        originalData = {
            IsDisabled: !disabledCheckbox.is(":checked"),
            IsDisabledDownline: !disabledDownlineCheckbox.is(":checked")
        };
    }

    return { initialize: initialize };
})(jQuery, common, rootUrl);