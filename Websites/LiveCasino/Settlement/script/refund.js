var refund = {};

refund.init = function () {

};

refund.submit = function () {
    $("#msg").hide();
    if (!confirm("Are you sure you want to Refund this game?")) return;
    var MatchID = $("#hiddenMatchID").val();
    var AdminID = $("#hiddenAdminID").val();
    var AdminName = $("#hiddenAdminName").val();
    refund.blockUI();
    $.ajax({
        type: "POST",
        url: "service/SettleLiveCasinoService.svc/Refund",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"MatchID":' + MatchID + ', "AdminID": ' + AdminID + ', "AdminName": "' + AdminName + '", "ShowDate":"' + $("#hiddenShowDate").val() + '"}',
        success: function (rs) {
            refund.unBlock();
            if (rs != "") {
                $("#msg").css("border-color", "red").css("color", "red").html(rs).show();
            }
            else {
                $("#msg").css("border-color", "green").css("color", "green").html("Refund is successfully!").show();
                var timerId = setTimeout(function () {
                    clearTimeout(timerId);
                    if (parent.parent.settlelc && parent.parent.settlelc.getData) {
                        parent.parent.settlelc.getData();
                    }
                }, 2000);
            }
        }
    });
};

refund.closePopup = function () {
    if (typeof parent.ClosePopup != "undefined") {
        parent.ClosePopup();
    }
};

refund.blockUI = function () {
    if (parent.parent.BlockUI) {
        try { parent.parent.BlockUI(); } catch (e) { }
    }
    else {
        try { parent.BlockUI(); } catch (e) { }
    }
};

refund.unBlock = function () {
    if (parent.parent.$ && parent.parent.$.unblockUI) {
        try { parent.parent.$.unblockUI(); } catch (e) { }
    }
    else {
        try { parent.$.unblockUI(); } catch (e) { }
    }
};

$(document).ready(function () {
    refund.init();
});