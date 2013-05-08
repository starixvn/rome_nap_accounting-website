$(document).ready(function () {
    delbet.init();

    var gridOpts = {
        tblContentId: 'grid1'
    };
    $("#mainreport").initGrid(gridOpts);
});

var help = {
    checkInt: function (value) {
        if (value == null || value.length == 0)
            return 0;
        return parseInt(value);
    }
};

var delbet = {
    matchGridWidth: 649,
    matchGridHeight: 100,
    ticketsGridWidth: 1150,
    ticketsGridHeight: 800,

    param: {
        transid: 0
    },

    changeStatusShowButton: function () {
        if ($("#txtTransId").val() != "") {
            $("#btnShow").removeAttr("disabled");
        }
        else {
            $("#btnShow").attr("disabled", "disabled");
        }
    },



    init: function () {
        $("#txtTransId").keyup(function () {
            delbet.changeStatusShowButton();
        }).ForceDecimalOnly();

        $("#btnDeleteBets").click(function () {
            delbet.submit();
        });
        $("#btnShow").click(function () {
            delbet.getData();
        });
    },

    submit: function () {

        $('#msg').html('');

        var customers = delbet.checkNegativeBalance();
        if (customers.length > 0) {
            var msg = 'This action could lead to a negative amount of Bet Credit of the following customers: ' + customers;
            msg += '<br/>Are you sure you want to proceed ?';
            Popups.Confirm(msg);
        }
        else {
            delbet.recoverBet();
        }
    },

    recoverBet: function () {


        var param = {
            transid: delbet.param.transid,
            userid: $('#hiddenAccID').val(),
            username: $('#hiddenAccUserName').val()
        };

        delbet.blockUI();
        $.ajax({
            type: "POST",
            url: "RecoverBet",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                delbet.unBlock();

                showEnrichMessage('msg', rs.ErrorHandler);
                $('#msg').show();
                if (rs.IsSuccess) {
                    Popups.Alert('Caution: This ticket will return to "Running" status, please resettle the game to calculate it again.');
                    $("#divMainContainer").hide();
                    $("#txtTransId").val("");
                    delbet.changeStatusShowButton();
                }                
            }
        });
    },

    getData: function () {
        $('#msg').html('');

        var transIdLength = $("#txtTransId").val().length;
        if (transIdLength > 18 || transIdLength == 0 || isNaN($("#txtTransId").val())) {
            displayAlertMessage('msg', 'Invalid TransID.', 1);
            return;
        }

        delbet.blockUI();
        delbet.param.transid = $("#txtTransId").val();

        $.ajax({
            type: "POST",
            url: "GetTicket_RecoverBet",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"transid":' + $("#txtTransId").val() + '}',
            success: function (rs) {
                delbet.unBlock();

                if (rs != null && rs.ErrorHandler != null) {
                    showEnrichMessage('msg', rs.ErrorHandler);
                    $("#divMatchGrid").addClass("hidden");
                    $("#mainreport").addClass("hidden");
                }
                else {

                    if (rs == null || rs.Licensees.length == 0) {
                        displayAlertMessage('msg', 'There is no information available.', 1);
                        $("#divMatchGrid").addClass("hidden");
                        $("#mainreport").addClass("hidden");
                        return;
                    }

                    $("#divMainContainer").show();
                    if (rs.Licensees == null || rs.Licensees.length == 0) {
                        $("#divMatchGrid").addClass("hidden");
                        $("#mainreport").addClass("hidden");
                        return;
                    }

                    delbet.renderMatch(rs);
                    delbet.renderTickets(rs);
                }
            }
        });
    },

    renderMatch: function (rs) {
        $('#spCode').html(rs.MatchCode);
        $('#spMatchEvent').html("<b>Number Game No</b> " + rs.MatchCode);
        $('#spEventDate').html(rs.EventDate);
        $("#divMatchGrid").removeClass('hidden');
    },

    renderTickets: function (rs) {

        var mainreport = $("#mainreport");

        mainreport.removeClass("hidden");
        mainreport.resetGrid("grid1");
        $("#grid1 tbody").empty().append(delbet.genBody(rs));
        $("#grid1rowheader tbody").empty().append(delbet.gen2ndColBodyScroll(rs));

        var gridOpts = {
            tblContentId: 'grid1',
            manualCopyHeader: true, //=totalColumnWidth + totalColumn*4 +totalColumn
            fullWidth: true,
            fullHeight: true,
            gridHeight: 150,
            gridWidth: 1115,
            freezeWidth: 125,
            numberFreezeCol: 2
        };
        mainreport.gridScroller(gridOpts);
    },

    assignLicenseeData: function (licensee) {
        var rows = [];

        for (var i = 0; i < licensee.TicketDetails.length; i++) {

            var row = licensee.TicketDetails[i];

            var cnt = row.Betteam.split('<br/>').length;
            var height = (cnt == 1 ? 2 : cnt) * 15;

            rows.push('<tr><td><div class="c " style=" width: 20px;"><span style=" width: 20px;" class="table-cell " >' + row.Index + '</span></div></td>');
            rows.push('<td><div class="c " style=" width: 100px;">' + row.UserName + '</div></td>');
            rows.push('<td><div class="c " style=" width: 100px;">' + row.BetID + '</div></td>');
            rows.push('<td><div class="c " style=" width: 160px;">' + row.BetTypeName + '</div></td>');
            rows.push('<td><div class="c " style="height:' + height + 'px;width: 100px;"><div class="table-cell " style="height:30px;width: 100px;">' + row.Betteam + '</div></div></td>');
            rows.push('<td><div class="r b  " style=" width: 60px;"><span style="color:blue">' + row.Odds + '</span><br/><span class="b">' + row.OddsType + '</span></div></td>');
            rows.push('<td><div class="c b " style="width: 80px;">' + row.Stake + '</div></td>');
            rows.push('<td><div class="r " style=" width: 60px;">' + row.WinLoss + '<br/>' + row.Comm + '</div></td>');
            rows.push('<td><div class="c " style=" width: 80px;text-transform:capitalize">' + row.Status + '</div></td>');
            rows.push('<td><div class="c " style=" width: 80px;">' + row.CurrencyName + '</div></td>');
            //            rows.push('<td><div class="r " style=" width: 20px;">' + row.Hdp + '</div></td>');
            rows.push('<td><div class="r " style=" width: 140px;">' + row.TransTime + '</div></td>');
            rows.push('<td><div class="c " style=" width: 100px;text-overflow:ellipsis;">' + row.Creator + '</div></td>');

            rows.push('<td><div class="c" style="width: 50px;"><span style="margin-left: 18px;" class="recover hand" onclick="delbet.submit()"></span></div></td>');

            rows.push('</tr>');
        }

        return rows.join('');
    },

    blockUI: function () {
        if (parent.parent.BlockUI) {
            try { parent.parent.BlockUI(); } catch (e) { }
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
        }
    },

    unBlock: function () {
        if (parent.parent.$ && parent.parent.$.unblockUI) {
            try { parent.parent.$.unblockUI(); } catch (e) { }
        }
        else {
            try { parent.$.unblockUI(); } catch (e) { }
        }
    },

    genBody: function (rs) {
        var html = [];
        for (var i = 0; i < rs.Licensees.length; i++) {
            var licensee = rs.Licensees[i];

            html.push(delbet.assignLicenseeData(licensee));
        }
        return html.join('');
    },

    gen2ndColBodyScroll: function (rs) {
        var rows = [];
        for (var i = 0; i < rs.Licensees.length; i++) {
            var licensee = rs.Licensees[i];

            for (var j = 0; j < licensee.TicketDetails.length; j++) {
                var row = licensee.TicketDetails[j];

                var cnt = row.Betteam.split('<br/>').length;
                var height = (cnt == 1 ? 2 : cnt) * 15;

                rows.push('<tr><td><div class="c " style="height: 30px; width: 20px;"><span class="c table-cell"  style="height: 30px; width: 20px;">' + row.Index + '</span></div></td>');
                rows.push('<td ><div style="height:' + height + 'px;width: 100px;" class="c table-cell">' + row.UserName + '</div></td></tr>');
            }
        }
        return rows.join('');
    },

    checkNegativeBalance: function () {
        delbet.blockUI();

        var param = {
            transid: $('#txtTransId').val(),
            actionType: 1//recover bet
        };

        var customers = [];


        $.ajax({
            type: "POST",
            url: "SelNegativeBalanceCustomer",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify(param),
            success: function (rs) {
                delbet.unBlock();
                for (var i = 0; i < rs.length; i++) {
                    customers.push(rs[i].UserName);
                }
            }
        });

        return customers.join(', ');
    }
};


function ConfirmYes() {
    Popups.Close();
    delbet.recoverBet();    
}

function ConfirmNo() {
    Popups.Close();
}
