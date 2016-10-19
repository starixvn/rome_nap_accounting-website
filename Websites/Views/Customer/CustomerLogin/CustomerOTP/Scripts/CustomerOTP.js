customerOTP = {
    initialize: function () {
        $("#btnSubmit").click(function () {
            customerOTP.RemoveOTP();
        });
    },
    RemoveOTP: function () {
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
            error: function () {
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
