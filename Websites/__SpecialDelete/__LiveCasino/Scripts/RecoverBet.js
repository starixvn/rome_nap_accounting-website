var recoverBet = {
    matchGridWidth: 649,
    matchGridHeight: 100,
    ticketsGridWidth: 1193,
    ticketsGridHeight: 800
};

recoverBet.init = function () {
    $("#tbxTransId").keyup(function () {
        if ($(this).val() != "") {
            $("#btnSearch").removeAttr("disabled");
        }
        else {
            $("#btnSearch").attr("disabled", "disabled");
        }
    }).ForceDecimalOnly();

    $("#btnSearch").click(function () {
        recoverBet.search();
    });
};

recoverBet.recoverOne = function(transid) {
    $("#diveNoInfo").hide();
    $("#ErrorMessage").hide();

    var data = {};
    data.TransId = transid;
    data.UserId = parseInt($("#hiddenAccID").val());
    data.UserName = $("#hiddenAccUserName").val();
    recoverBet.blockUI();
    $.ajax({
        type: "POST",
        url: "Services/SpecialDeleteService.svc/RecoverBet",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {
            recoverBet.unBlock();
            if (rs != "") {
                showAlertMessage("ErrorMessage",rs,1);
                $("#ErrorMessage").width(500).show();
            }
            else {
                showAlertMessage("ErrorMessage","Transaction " + transid + " was recovered.",0);
                $("#ErrorMessage").width(500).show();
                $("#divMainContainer").hide();
                $("#tbxTransId").val("");
            }
        }
    });
};

recoverBet.search = function () {
    if ($("#tbxTransId").val() == "") return;
    $("#diveNoInfo").hide();
    $("#ErrorMessage").hide();
    recoverBet.blockUI();
    $.ajax({
        type: "POST",
        url: "Services/SpecialDeleteService.svc/GetTicketsForRecoverBet",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"TransID":' + $("#tbxTransId").val() + '}',
        success: function (rs) {
            recoverBet.unBlock();
            if (rs == null) {
                $("#diveNoInfo").show();
                $("#divMainContainer").hide();
                $("#ErrorMessage").hide();
                return;
            }
            $("#divMainContainer").show();
            recoverBet.renderMatch(rs);
            recoverBet.renderTickets(rs);
        }
    });
};

recoverBet.renderMatch = function (rs) {
    $("#scriptMatchTemplate").tmpl([rs]).appendTo($("#tobdyMatchGrid").empty());
    $("#divMatchGrid").removeClass("hidden").show().reRenderScroller(recoverBet.matchGridWidth, recoverBet.matchGridHeight, true);
};

recoverBet.renderTickets = function (rs) {
    $("#scriptTicketsTemplate").tmpl(rs.TicketDetails).appendTo($("#tobdyTicketsGrid").empty());
    $("#divTicketsGrid").removeClass("hidden").show().reRenderScroller(recoverBet.ticketsGridWidth, recoverBet.ticketsGridHeight, true);
};


recoverBet.blockUI = function () {
    if (parent.parent.BlockUI) {
        try { parent.parent.BlockUI(); } catch (e) { }
    }
    else {
        try { parent.BlockUI(); } catch (e) { }
    }
};

recoverBet.unBlock = function () {
    if (parent.parent.$ && parent.parent.$.unblockUI) {
        try { parent.parent.$.unblockUI(); } catch (e) { }
    }
    else {
        try { parent.$.unblockUI(); } catch (e) { }
    }
};

$(document).ready(function () {
    recoverBet.init();
});