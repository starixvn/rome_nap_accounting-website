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
        matchid: 0,
        f_transid: 0,
        t_transid: 0,
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
            f_transid: $("#tbxFromRefNo").val(),
            t_transid: $("#tbxToRefNo").val()
        };

        if (data.matchid.length == 0) {
            displayAlertMessage('msg', 'Invalid Match ID.', 1);
            $('#tbxMatchID').focus();
            return;
        }
        if (data.f_transid.length == 0 || data.t_transid.length == 0) {
            displayAlertMessage('msg', 'Invalid From Refno.', 1);
            $('#tbxFromRefNo').focus();
            return;
        }
        if (data.t_transid.length == 0) {
            displayAlertMessage('msg', 'Invalid To Refno.', 1);
            $('#tbxToRefNo').focus();
            return;
        }

        delbet.blockUI();

        $.ajax({
            type: "POST",
            url: "CheckRefno",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify(data),
            success: function (rs) {
                delbet.unBlock();
                if (rs.length == 0) {
                    delbet.getData();
                    $("#msg").hide();
                    return;
                }
                else {
                    if (rs.ErrorHandler != null) {
                        showEnrichMessage('msg', rs.ErrorHandler);
                    }
                    else {
                        displayAlertMessage('msg', rs, 1);
                    }

                    $("#divMainContainer").hide();
                    $("#msg").show();                    
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
        $("#msg").hide();
        if ($("#userpwd").val() == "") {
            $("#ErrorMessage").width(200).html("<div class='alertboxerr'><div>Please enter password</div></div>").show();
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


        delbet.clearMsg();

        if ($("#tbxFromRefNo").val().length > 18) {
            $("#divContentMsg").html("FromRefNo is invalid!");

            return;
        }

        if ($("#tbxToRefNo").val().length > 18) {
            $("#divContentMsg").html("ToRefNo is invalid!");

            return;
        }

        delbet.blockUI();

        delbet.param.matchid = $('#tbxMatchID').val();
        delbet.param.f_transid = $('#tbxFromRefNo').val();
        delbet.param.t_transid = $('#tbxToRefNo').val();

        $.ajax({
            type: "POST",
            url: "GetTickets",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"matchid":' + $("#tbxMatchID").val() + ', "f_transid": ' + $("#tbxFromRefNo").val() + ', "t_transid": ' + $("#tbxToRefNo").val() + '}',
            success: function (rs) {
                delbet.unBlock();
                if (rs.Licensees.length == 0) {
                    displayAlertMessage('msg', 'There is no information available.', 1);
                    $("#divMatchGrid").hide();
                    $("#divTicketsGrid").hide();
                    $("#divSendMsg").hide();
                    $("#divLogin").hide();
                    $("#divMainContainer").hide();

                    return;
                }

                $("#divMainContainer").show();
                $("#divMatchGrid").show();
                if (rs.Licensees == null || rs.Licensees.length == 0) {
                    $("#divMatchGrid").hide();
                    $("#divTicketsGrid").hide();
                    $("#divSendMsg").hide();
                    $("#divLogin").hide();
                    $("#divMainContainer").hide();
                    return;
                }

                delbet.renderMatch(rs);
                delbet.renderTickets(rs);

                for (var i = 0; i < rs.Licensees.length; i++) {
                    delbet.ddlChange(rs.Licensees[i].Name);
                }

                $("#divLogin").show();
                $("#divSendMsg").show();
            }
        });
    },

    renderMatch: function (rs) {
        $('#spCode').html(rs.MatchCode);
        $('#spMatchEvent').html(rs.MatchEvent);
        $('#spEventDate').html(rs.EventDate);

    },

    renderTickets: function (rs) {

        var mainreport = $("#mainreport");

        mainreport.removeClass("hidden");
        mainreport.resetGrid("grid1");
        $("#grid1 tbody").empty().append(delbet.genBody(rs));
        $("#grid1rowheader tbody").empty().append(delbet.gen2ndColBodyScroll(rs));
        mainreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:20px;" class="c table-cell"></div></th></tr>');
        mainreport.find(".col_footer_content thead").empty().append('');

        var gridWidth = 630;

        var gridOpts = {
            tblContentId: 'grid1',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 150,
            gridWidth: gridWidth,
            freezeWidth: 125,
            numberFreezeCol: 2
        };
        mainreport.gridScroller(gridOpts);
    },

    assignLicenseeData: function (licensee) {
        var rows = [];
        var hide = licensee.IsAllowOptionDelete ? '' : 'hide';
        var ddl = [];

        ddl.push('<select onchange="delbet.ddlChange(\'' + licensee.Name + '\')" class="' + hide + '" id="ddl' + licensee.Name + '">');
        ddl.push('<option value="0">Not Delete</option>');
        ddl.push('<option ' + (licensee.DeleteOtionDefault ? 'selected="selected"' : '') + 'value="1">Delete All</option>');
        ddl.push('<option value="2">Optional</option>');
        ddl.push('</select>');

        var tr = '<tr class="bgcolor height"><td colspan="11" class="c licensee">' + licensee.Name + '</td><td colspan="2" class="r">' + ddl.join('') + '</td></tr>';
        rows.push(tr);

        for (var i = 0; i < licensee.TicketDetails.length; i++) {

            var row = licensee.TicketDetails[i];

            var height = (row.cntChoice == 1 ? 2 : row.cntChoice) * 18;

            rows.push('<tr><td><div class="c">' + row.Index + '</div></td>');
            rows.push('<td><div class="c">' + row.UserName + '</div></td>');
            rows.push('<td><div class="c">' + row.TransTime + '</div></td>');
            rows.push('<td><div class="c">' + row.BetID + '</div></td>');
            rows.push('<td><div class="c">' + row.BetTypeName + '</div></td>');
            rows.push('<td><div class="r">' + row.Stake + '<br/>(' + row.CurrencyName + ')</div></td>');
            rows.push('<td><div class="r">' + row.WinLoss + '<br/>' + row.Comm + '</div></td>');
            rows.push('<td><div class="c" style="text-transform:capitalize">' + row.Status + '</div></td>');
            rows.push('<td><div class="r">' + row.SuperPT + '</div></td>');
            rows.push('<td><div class="r">' + row.MasterPT + '</div></td>');
            rows.push('<td><div class="r">' + row.AgentPT + '</div></td>');
            rows.push('<td class=""><div style="height:' + height + 'px" class="r"><span style="width:200px; height:' + height + 'px" class="table-cell">' + row.ChoiceStr + '</span></div></td>');

            if (licensee.IsAllowOptionDelete) {
                var checked = licensee.DeleteOtionDefault ? 'checked="checked"' : '';
                rows.push('<td><div class="c"><input class="' + licensee.Name + '" ' + checked + ' type="checkbox" value="' + row.BetID + '" id="' + row.BetID + '" name="chk' + licensee.Name + '"/></div></td>');
            }
            else {
                rows.push('<td><div class="c"></td>');
            }

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


        delbet.param.transids_n = delbet.getSeletedId('Normal');
        delbet.param.transids_d = delbet.getSeletedId('Deposit');
        delbet.param.transids_bd = delbet.getSeletedId('BetDaq');
        delbet.param.transids_12eu = delbet.getSeletedId('12EU');
        delbet.param.transids_mhc = delbet.getSeletedId('MHC');
        delbet.param.isSendPrivateMSG = $('#cbxSendMsg').is(':checked');
        delbet.param.message = msg;
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
                var height = (row.cntChoice == 1 ? 2 : row.cntChoice) * 18;
                rows.push('<tr><td><div class="c">' + row.Index + '</div></td>');
                rows.push('<td><div style="height:' + height + 'px" class="c table-cell">' + row.UserName + '</div></td></tr>');
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

