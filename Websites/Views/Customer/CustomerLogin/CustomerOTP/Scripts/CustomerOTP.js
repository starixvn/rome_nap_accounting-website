customerOTP = {
    initialize: function () {
        $("#btnSubmit").click(function () {
            customerOTP.confirmRemoveOTP();
        });
    },
    confirmRemoveOTP: function () {
        $('#dialogConfirm').empty().append($("#removingOTPConfirm").val());
        $("#dialogConfirm").dialog({
           title: "Remove OTP",
            autoOpen: false,
            resizable: false,
            height: 110,
            width: 400,
            modal: true,
            position: [30, 100],
            create: function (event) { $(event.target).parent().css('position', 'fixed'); },
            buttons: {
                "OK": function () {
                    customerOTP.removeOTP();
                    $(this).dialog("close");
                },
                "Cancel": function () {
                    $(this).dialog("close");
                }
            },
        });
        $('a.ui-dialog-titlebar-close').show();
        $("#dialogConfirm").dialog("open");
    },
    removeOTP: function () {
        try { parent.BlockUI(); } catch (e) { }
        var $errorMessage = $("#errorMessage");
        $errorMessage.empty();
        $errorMessage.addClass("hidden");
        $.ajax({
            type: "POST",
            url: $("#rootUrl").val() + "CustomerOTP/RemoveOTP",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ custId: $("#customerId").val() }),
            success: function (rs) {
                if (rs && rs.ErrorCode > 0) {
                    showAlertMessage("errorMessage", rs.ErrorMessage, 1);
                } else {
                    showAlertMessage("errorMessage", $("#removeOTPsuccessfully").val(), 0);
                }

                $errorMessage.removeClass("hidden");
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    var denyObject = $.parseJSON(xhr.responseText);
                    showAlertMessage("errorMessage", denyObject.ErrorMessage, 1);
                    $errorMessage.removeClass("hidden");
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }
};

$(function () {
    customerOTP.initialize();
});
