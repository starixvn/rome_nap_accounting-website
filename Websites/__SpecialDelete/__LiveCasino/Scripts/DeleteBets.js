$(document).ready(function () {
    delbet.init();
});


var delbet = {
    matchGridWidth: 649,
    matchGridHeight: 100,
    ticketsGridWidth: 1150,
    ticketsGridHeight: 800,

    param: {
        matchId: 0,
        fromRefno: 0,
        toRefno: 0,
        normal_option: 0,
        normal_transid: 0,
        deposit_option: 0,
        deposit_transid: 0,
        betdaq_option: 0,
        betdaq_transid: 0,
        eu_option: 0,
        eu_transid: 0,
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
            type: 3,
            matchid: $("#tbxMatchID").val(),
            fromRefno: $("#tbxFromRefNo").val(),
            toRefno: $("#tbxToRefNo").val(),
            type: '3'//Live Casino
        };
        $.ajax({
            type: "POST",
            url: "../Services/specialDeleteService.svc/CheckRefno",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs.length <= 0) {
                    delbet.getData();
                    $("#diveNoInfo").hide();
                    return;
                }
                else {
                    $("#divMainContainer").hide();
                    delbet.clearMsg();
                    $("#diveNoInfo").html(rs[0].Value);
                    $("#diveNoInfo").width(300).show();
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
        if ($("#userpwd").val() == "") {
            $("#ErrorMessage").width(200).html("<div class='alertboxerr'><div>Please enter password</div></div>").show();
            return;
        }
        if (!this.checkMsg()) return;

        delbet.setParam();

        delbet.blockUI();
        $.ajax({
            type: "POST",
            url: "Services/SpecialDeleteService.svc/DeleteBets",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(delbet.param),
            success: function (rs) {
                delbet.unBlock();
                if (rs != "") {
                    $("#ErrorMessage2").width(500).html("<div class='alertboxerr'><div>" + rs + "</div></div>").show();
                    return;
                }
                $("#ErrorMessage2").width(500).html("<div class='alertboxscc'><div>Bet transactions deleted!</div></div>").show();
                $("#divMatchGrid").hide();
                $("#divTicketsGrid").hide();
                $("#divLogin").hide();
                $("#divSendMsg").hide();
                $("#spanNoInfo").hide();
                
                $("#tbxMatchID").val("");
                $("#tbxFromRefNo").val("");
                $("#tbxToRefNo").val("");
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

        delbet.blockUI();
        delbet.FromRefNo = $("#tbxFromRefNo").val();
        delbet.ToRefNo = $("#tbxToRefNo").val();
        $.ajax({
            type: "POST",
            url: "Services/SpecialDeleteService.svc/GetTickets",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"MatchID":' + $("#tbxMatchID").val() + ', "FromRefNo": ' + $("#tbxFromRefNo").val() + ', "ToRefNo": ' + $("#tbxToRefNo").val() + '}',
            success: function (rs) {
                delbet.unBlock();
                if (rs == null) {
                    $("#diveNoInfo").show();
                    return;
                }

                $("#divMainContainer").show();
                if (rs.Licensees == null || rs.Licensees.length <= 0) {
                    $("#divContentMsg").html("There is no information!");
                    $("#diveNoInfo").width(200).show();
                    $("#divMatchGrid").hide();
                    $("#divTicketsGrid").hide();
                    $("#divSendMsg").hide();
                    $("#divLogin").hide();
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
        $("#scriptMatchTemplate").tmpl([rs]).appendTo($("#tobdyMatchGrid").empty());
        $("#divMatchGrid").removeClass("hidden").show().reRenderScroller(delbet.matchGridWidth, delbet.matchGridHeight, true);
    },

    renderTickets: function (rs) {

        $("#tobdyTicketsGrid").empty();

        for (var i = 0; i < rs.Licensees.length; i++) {
            var licensee = rs.Licensees[i];

            $("#tobdyTicketsGrid").append(delbet.assignLicenseeData(licensee));
        }

        $("#divTicketsGrid").removeClass("hidden").show().reRenderScroller(delbet.ticketsGridWidth, delbet.ticketsGridHeight, true);
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

            rows.push('<tr><td><div class="col20 c">' + row.Index + '</div></td>');
            rows.push('<td><div class="col100 c">' + row.UserName + '</div></td>');
            rows.push('<td><div class="col140 c">' + row.TransTime + '</div></td>');
            rows.push('<td><div class="col100 c">' + row.BetID + '</div></td>');
            rows.push('<td><div class="col100 c">' + row.BetTypeName + '</div></td>');
            rows.push('<td><div class="col60 r">' + row.Stake + '<br/>(' + row.CurrencyName + ')</div></td>');
            rows.push('<td><div class="col80 r">' + row.SuperPT + '</div></td>');
            rows.push('<td><div class="col60 c">' + row.Status + '</div></td>');
            rows.push('<td><div class="col80 r">' + row.SuperPT + '</div></td>');
            rows.push('<td><div class="col80 r">' + row.MasterPT + '</div></td>');
            rows.push('<td><div class="col80 r">' + row.AgentPT + '</div></td>');
            rows.push('<td><div class="col160 r">' + row.ChoiceStr + '</div></td>');

            if (licensee.IsAllowOptionDelete) {
                var checked = licensee.DeleteOtionDefault ? 'checked="checked"' : '';
                rows.push('<td><div class="col50 c"><input class="' + licensee.Name + '" ' + checked + ' type="checkbox" value="' + row.BetID + '" id="' + row.BetID + '" name="chk' + licensee.Name + '"/></div></td>');
            }
            else {
                rows.push('<td><div class="col50 c"></td>');
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

    getSeletedTransId: function (name) {
        var transIds = [];

        $('.' + name + ':checked').each(function () {
            transIds.push($(this).val());
        });

        return transIds.join(',');
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

        delbet.param.matchId = $('#tbxMatchID').val();

        delbet.param.fromRefno = $('#tbxFromRefNo').val();
        delbet.param.toRefno = $('#tbxToRefNo').val();

        delbet.param.normal_option = $('#ddlNormal').val();
        delbet.param.normal_transid = delbet.getSeletedTransId('Normal');

        delbet.param.deposit_option = $('#ddlDeposit').val();
        delbet.param.deposit_transid = delbet.getSeletedTransId('Deposit');

        delbet.param.betdaq_option = $('#ddlBetDaq').val();
        delbet.param.betdaq_transid = delbet.getSeletedTransId('BetDaq');

        delbet.param.eu_option = $('#ddl12EU').val();
        delbet.param.eu_transid = delbet.getSeletedTransId('12EU');

        delbet.param.mhc_option = $('#ddlMHC').val();
        delbet.param.mhc_transid = delbet.getSeletedTransId('MHC');

        delbet.param.message = msg;
    }


};



