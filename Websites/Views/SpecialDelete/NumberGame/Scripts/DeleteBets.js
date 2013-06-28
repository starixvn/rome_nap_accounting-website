$(document).ready(function () {
    delbet.init();
    delbet.selBettype();

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
    cntBettypes: 0,

    param: {
        matchid: 0,
        f_transid: 0,
        t_transid: 0,
        dead: '',
        live: '',
        transids_n: 0,
        transids_d: 0,
        transids_bd: 0,
        transids_12eu: 0,
        transids_mhc: 0,
        isSendPrivateMSG: false,
        message: ''
    },

    changeStatusShowButton: function () {
        if ($("#tbxMatchID").val() != "" && $("#tbxFromRefNo").val() != "" && $("#tbxToRefNo").val() != "") {
            $("#btnShow").removeAttr("disabled");
        }
        else {
            $("#btnShow").attr("disabled", "disabled");
        }
    },



    checkRefNo: function () {
        var data = {
            matchid: $("#tbxMatchID").val(),
            fromRefno: $("#tbxFromRefNo").val(),
            toRefno: $("#tbxToRefNo").val()
        };

        delbet.blockUI();

        $.ajax({
            type: "POST",
            url: "CheckRefno",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
          ///  async: false,
            data: JSON.stringify(data),
            success: function (rs) {

                delbet.unBlock();

                if (rs.IsSuccess != null && !rs.IsSuccess) {
                    $("#msg").show();
                    showEnrichMessage('msg', rs.ErrorHandler);
                    $("#divMainContainer").hide();
                }
                else {
                    if (rs.length == 0) {
                        delbet.getData();
                        return;
                    }
                    else {
                        $("#divMainContainer").hide();
                        $("#msg").show();
                        displayAlertMessage('msg', rs, 1);
                    }
                }
            }
        });
    },

    init: function () {
        $("#tbxMatchID").keyup(function () {
            delbet.changeStatusShowButton();
        }).ForceDecimalOnly();
        $("#tbxFromRefNo").keyup(function () {
            delbet.changeStatusShowButton();
        }).ForceDecimalOnly();
        $("#tbxToRefNo").keyup(function () {
            delbet.changeStatusShowButton();
        }).ForceDecimalOnly();

        $("#btnShow").click(function () {
            $('#reason').val('');
            delbet.checkRefNo();
        });

        $("#btnDeleteBets").click(function () {
            delbet.submit();
        });
    },

    checkMsg: function () {
        if (!$("#cbxSendMsg").get(0).checked) {
            return true;
        }
        if ($("#men").val() == "") {
            $("#ErrorMessage").width(250).html("<div class='alertboxerr'><div>You must enter English message</div></div>").show();
            return false;
        }
        if ($("#men").val().indexOf("@@") < 0) {
            $("#ErrorMessage").width(500).html("<div class='alertboxerr'><div>You must include @@ in English message for replacing ticket number</div></div>").show();
            return false;
        }
        if ($("#mch").val() == "") {
            $("#ErrorMessage").width(250).html("<div class='alertboxerr'><div>You must enter Chinese message</div></div>").show();
            return false;
        }
        if ($("#mch").val().indexOf("@@") < 0) {
            $("#ErrorMessage").width(500).html("<div class='alertboxerr'><div>You must include @@ in Chinese message for replacing ticket number</div></div>").show();
            return false;
        }
        return true;
    },

    submit: function () {
        $("#ErrorMessage").hide();
        $("#ErrorMessage2").hide();

        if ($.trim($("#reason").val()) == "") {  
            $("#ErrorMessage").width(200).html("<div class='alertboxerr'><div>Please enter a reason.</div></div>").show();
            $("#reason").focus();
            return;
        }
        else if ($("#userpwd").val() == "") {
            $("#ErrorMessage").width(200).html("<div class='alertboxerr'><div>Please enter password.</div></div>").show();
            $("#userpwd").focus();
            return;
        }
        if (!this.checkMsg()) return;

        delbet.setParam();

        if (!delbet.checkParam()) {
            displayAlertMessage('msg', 'Please choose at least one ticket.', 1);
            return;
        }

        var customers = delbet.checkNegativeBalance();
        if (customers.length > 0) {
            var msg = 'This action could lead to a negative amount of Bet Credit of the following customers: ' + customers;
            msg += '<br/>Are you sure you want to proceed ?';
            Popups.Confirm(msg);
        }
        else {
            delbet.deleteBets();
        }
    },

    deleteBets: function () {

        delbet.blockUI();
        $.ajax({
            type: "POST",
            url: "DeleteBets",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(delbet.param),
            success: function (rs) {
                delbet.unBlock();

                showEnrichMessage('msg', rs.ErrorHandler);
                $('#msg').show();
                if (rs.IsSuccess) {
                    $("#divMainContainer").hide();
                    $("#divLogin").hide();
                    $("#divSendMsg").hide();
                    $("#spanNoInfo").hide();

                    $("#tbxMatchID").val("");
                    $("#tbxFromRefNo").val("");
                    $("#tbxToRefNo").val("");

                    $('#btnShow').attr('disabled', true);
                    $('input.bettype').attr('checked', false);
                    $('#chkAllBettype').attr('checked', false);
                }
            }
        });
    },

    clearMsg: function () {
        $('#men').val("");
        $('#mch').val("");
        $('#mcs').val("");
        $('#mit').val("");
        $('#mjp').val("");
        $('#mth').val("");
        $('#mes').val("");
        $('#mko').val("");
        $('#userpwd').val("");
    },

    getData: function () {
        $("#ErrorMessage2").hide();
        $("#diveNoInfo").hide();
        $('#msg').html('');
        delbet.clearMsg();

        if ($("#tbxFromRefNo").val().length > 18) {
            $("#divContentMsg").html("FromRefNo is invalid!");
            $("#diveNoInfo").show();
            return;
        }

        if ($("#tbxToRefNo").val().length > 18) {
            $("#divContentMsg").html("ToRefNo is invalid!");
            $("#diveNoInfo").show();
            return;
        }


        delbet.FromRefNo = $("#tbxFromRefNo").val();
        delbet.ToRefNo = $("#tbxToRefNo").val();

        var param = {
            dead: delbet.getSeletedId('nonelive', '^'),
            live: delbet.getSeletedId('live', '^'),
            matchId: $('#tbxMatchID').val(),
            fromRefNo: $('#tbxFromRefNo').val(),
            toRefNo: $('#tbxToRefNo').val()
        };

        if (param.dead.length == 0 && param.live.length == 0) {
            displayAlertMessage('msg', 'Please choose at least a bet type.', 1);
            return;
        }
        delbet.blockUI();

        delbet.param.matchid = $('#tbxMatchID').val();
        delbet.param.f_transid = $('#tbxFromRefNo').val();
        delbet.param.t_transid = $('#tbxToRefNo').val();
        delbet.param.dead = delbet.getSeletedId('nonelive', '^');
        delbet.param.live = delbet.getSeletedId('live', '^');

        $.ajax({
            type: "POST",
            url: "GetTickets",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
          //  async: false,
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs.IsSuccess != null && !rs.IsSuccess) {
                    $('#msg').show();
                    $("#divMainContainer").hide();
                    showEnrichMessage('msg', rs.ErrorHandler);
                    delbet.unBlock();
                }
                else {
                    if (rs.Licensees == null || rs.Licensees.length == 0) {
                        $('#msg').show();
                        displayAlertMessage('msg', 'There is no information available.', 1);
                        $("#divMatchGrid").hide();
                        $("#divTicketsGrid").hide();
                        $("#divSendMsg").hide();
                        $("#divLogin").hide();
                        $("#divMainContainer").hide();
                        delbet.unBlock();
                        return;
                    }


                    $('#msg').hide();
                    $("#divMainContainer").show();
                    $("#divMatchGrid").show();

                    delbet.renderMatch(rs);
                    delbet.renderTickets(rs);


                    for (var i = 0; i < rs.Licensees.length; i++) {
                        delbet.ddlChange(rs.Licensees[i].Name);
                    }
                    delbet.unBlock();
                    $("#divLogin").show();
                    $("#divSendMsg").show();
                }
            }
        });
    },

    renderMatch: function (rs) {
        $("#divMatchGrid").removeClass('hidden');
        $('#spCode').html(rs.MatchCode);
        $('#spMatchEvent').html('Number Game No. ' + rs.MatchCode);
        $('#spEventDate').html(rs.EventDate);
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
            gridWidth: 1135,
            freezeWidth: 145,
            numberFreezeCol: 2
        };
        mainreport.gridScroller(gridOpts);
    },

    assignLicenseeData: function (licensee) {
        var rows = '';
        var hide = licensee.IsAllowOptionDelete ? '' : 'hide';
        var ddl = [];

        ddl.push('<select onchange="delbet.ddlChange(\'' + licensee.Name + '\')" class="' + hide + '" id="ddl' + licensee.Name + '">');
        ddl.push('<option value="0">Not Delete</option>');
        ddl.push('<option ' + (licensee.DeleteOtionDefault ? 'selected="selected"' : '') + 'value="1">Delete All</option>');
        ddl.push('<option value="2">Optional</option>');
        ddl.push('</select>');

        var tr = '<tr class="bgcolor height"><td colspan="11" class="c licensee">' + licensee.Name + '</td><td colspan="2" class="r">' + ddl.join('') + '</td></tr>';
        rows += tr;

        for (var i = 0; i < licensee.TicketDetails.length; i++) {

            var row = licensee.TicketDetails[i];

            var cnt = row.Betteam.split('<br/>').length;
            var height = (cnt == 1 ? 2 : cnt) * 15;

            rows += '<tr><td><div class="c " style=" width: 40px;"><span style=" width: 20px;" class="table-cell ">' + row.Index + '</span></div></td>\
            <td><div class="c " style=" width: 100px;">' + row.UserName + '</div></td>\
            <td><div class="c " style=" width: 100px;">' + row.BetID + '</div></td>\
            <td><div class="c " style=" width: 160px;">' + row.BetTypeName + '</div></td>\
            <td><div class="c " style="height:' + height + 'px;width: 100px;"><div class="table-cell " style="height:30px;width: 100px;">' + row.Betteam + '</div></div></td>\
            <td><div class="r b  " style=" width: 60px;"><span style="color:blue">' + row.Odds + '</span><br/><span class="b">' + row.OddsType + '</span></div></td>\
            <td><div class="c b " style="width: 80px;">' + row.Stake + '</div></td>\
            <td><div class="r " style=" width: 60px;">' + row.WinLoss + '<br/>' + row.Comm + '</div></td>\
            <td><div class="c " style=" width: 80px;text-transform:capitalize">' + row.Status + '</div></td>\
            <td><div class="c " style=" width: 80px;">' + row.CurrencyName + '</div></td>\
            <td><div class="r " style=" width: 140px;">' + row.TransTime + '</div></td>\
            <td><div class="c " style=" width: 100px;text-overflow:ellipsis;">' + row.Creator + '</div></td>';

            if (licensee.IsAllowOptionDelete) {
                var checked = licensee.DeleteOtionDefault ? 'checked="checked"' : '';
                rows += '<td><div class="c" style="height: 30px; width: 50px;"><span class="table-cell " style="height: 30px; width: 50px;"><input class="' + licensee.Name + '" ' + checked + ' type="checkbox" value="' + row.BetID + '" id="' + row.BetID + '" name="chk' + licensee.Name + '"/></span></div></td>';
            }
            else {
                rows += '<td><div class="c " style="height: 30px; width: 50px;"></td>';
            }

            rows += '</tr>'
        }

        return rows;
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

    ddlChange: function (name) {
        var ddlId = 'ddl' + name, chkCls = name;
        if ($('select#' + ddlId).length > 0) {
            var _val = $('select#' + ddlId).val();
            switch (_val.toString()) {
                case "0": //Not Delete
                    $('input.' + chkCls).attr('disabled', 'disabled')
                        .attr('checked', false);
                    break;
                case "1": //Delete All
                    $('input.' + chkCls).attr('disabled', 'disabled')
                        .attr('checked', true);
                    break;
                case "2": //Optional
                    $('input.' + chkCls).removeAttr('disabled')
                        .attr('checked', false);
                    break;
                default:
                    break;
            }


        }
    },

    getSeletedId: function (classname, separator) {
        var Ids = [];

        $('.' + classname + ':checked').each(function () {
            Ids.push($(this).val());
        });

        return Ids.join(separator);
    },

    setParam: function () {

        var msg = {};
        msg.Content_en = $('#men').val();
        msg.Content_ch = $('#mch').val();
        msg.Content_cs = $('#mcs').val();
        msg.Content_it = $('#mit').val();
        msg.Content_jp = $('#mjp').val();
        msg.Content_th = $('#mth').val();
        msg.Content_es = $('#mes').val();
        msg.Content_ko = $('#mko').val();        
        msg.UserName = $("#hiddenAccUserName").val();
        msg.UserId = $("#hiddenAccID").val();
        msg.userPwd = $('#userpwd').val();
        msg.CreatorId = $('#hiddenAccID').val();
        msg.CreatorName = $('#hiddenAccUserName').val();
        msg.Reason = $('#reason').val();
        
        delbet.param.transids_n = delbet.getSeletedId('Normal');
        delbet.param.transids_d = delbet.getSeletedId('Deposit');
        delbet.param.transids_bd = delbet.getSeletedId('BetDaq');
        delbet.param.transids_12eu = delbet.getSeletedId('12EU');
        delbet.param.transids_mhc = delbet.getSeletedId('MHC');

        delbet.param.isSendPrivateMSG = $('#cbxSendMsg').is(':checked');
        delbet.param.message = msg;
    },

    selBettype: function () {
        delbet.blockUI();
        $.ajax({
            type: "POST",
            url: "SelBettype",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                $('#dvBettype').html(delbet.renderBettype(rs));
                delbet.unBlock();
            }
        });
    },

    renderBettype: function (rs) {

        delbet.cntBettypes = 0;

        var html = [];
        html.push('<table style="width: 600px; text-align: left;" class="tblcollapse">');
        html.push('<tbody>');
        for (var i = 0; i < rs.length; i++) {
            html.push('<tr class="title">');
            html.push('<td style="text-align: left;" colspan="3">' + (rs[i].IsLive ? 'Live bet types' : 'Dead ball bet types') + '</td>');
            html.push('</tr>');

            var bettypes = rs[i].Bettypes;

            delbet.cntBettypes += bettypes.length;

            var cnt = Math.floor(bettypes.length / 3) + (bettypes.length % 3 > 0 ? 1 : 0);

            var index = 0;
            for (var j = 0; j < cnt; j++) {
                html.push('<tr>')
                for (var k = 0; k < 3; k++) {
                    if (index < bettypes.length) {
                        html.push('<td style="width: 180px"><input onclick="delbet.onclickBettype()" type="checkbox" class="hand bettype ' + (rs[i].IsLive ? 'live' : 'nonelive') + '" value="' + bettypes[index].Id + '">' + bettypes[index].Name + '</td>');
                    }
                    else {
                        html.push('<td style="width: 180px"></td>');
                    }

                    index++;
                }
                html.push('</tr>')
            }
        }

        html.push('<tr style="background-color:#c3c3c3"><td align="left" colspan="3"><input type="checkbox" onclick="delbet.onclickAllBettype()" class="hand" name="chkAllBettype" id="chkAllBettype"><b>All Bet types</b></td></tr>');
        html.push('</tbody>');
        html.push('</table>');

        return html.join('');
    },

    onclickAllBettype: function () {
        $('input.bettype').attr('checked', $('#chkAllBettype').is(':checked'));
    },

    onclickBettype: function () {
        var numberOfChecked = $('input.bettype:checkbox:checked').length;
        $('#chkAllBettype').attr('checked', numberOfChecked == delbet.cntBettypes);
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
            rows.push('<tr class="height bgcolor"><td colspan="2"></td></tr>');
            for (var j = 0; j < licensee.TicketDetails.length; j++) {
                var row = licensee.TicketDetails[j];

                var cnt = row.Betteam.split('<br/>').length;
                var height = (cnt == 1 ? 2 : cnt) * 15;

                rows.push('<tr><td><div class="c " style="height: 30px; width: 40px;"><span style="height: 30px; width: 40px;" class="c table-cell">' + row.Index + '</span></div></td>');
                rows.push('<td ><div style="height:' + height + 'px;width: 100px;" class="c table-cell">' + row.UserName + '</div></td></tr>');
            }
        }
        return rows.join('');
    },

    checkNegativeBalance: function () {
        delbet.blockUI();

        var transid = delbet.param.transids_n + ","
        + delbet.param.transids_d + ","
        + delbet.param.transids_bd + ","
        + delbet.param.transids_12eu + ","
        + delbet.param.transids_mhc;

        var param = {
            transid: transid,
            actionType: 0//delete bet
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
    },

    checkParam: function () {
        var cntTransidSeleted = delbet.param.transids_n.length
        + delbet.param.transids_d.length
        + delbet.param.transids_bd.length
        + delbet.param.transids_12eu.length
        + delbet.param.transids_mhc.length;

        return cntTransidSeleted > 0;

    }
};



function ConfirmYes() {
    Popups.Close();
    delbet.deleteBets();    
}

function ConfirmNo() {
    Popups.Close();
}