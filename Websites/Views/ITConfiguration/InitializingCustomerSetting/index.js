function SwitchTab(type) {
    $('#tabs').find('li').removeClass('current');
    $('#tab_' + type).addClass('current');
    GetPartialView(type);
}

function GetPartialView(requestUrl) {
    parent.BlockUI();
    $("#setting-content").html("<div id=\"errMsg\"></div>");
    $.ajax({
        type: "GET",
        url: requestUrl,
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                $("#setting-content").html(rs);
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('errMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}