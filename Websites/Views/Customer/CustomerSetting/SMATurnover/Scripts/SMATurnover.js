var SMATurnover = {
    InitPage: function () {
        var fromdate_opts = { minDate: new Date(balanceUpDay) };
        SMATurnover.RenderCalendarFromTo("#txtfromdate, #txttodate", fromdate_opts);
        SMATurnover.ChangeTab();
        SMATurnover.RenderSport();
        SMATurnover.RenderLeague("1", currDay, currDay);

        $("#SMATurnover").initGrid({
            isShowPager: false
        });

        $("#SMATurnover").delegate(".MaxTurnover", "keyup", function () {
            var obj = $(this);
            var matchId = obj.attr("matchId");
            var custId = obj.attr("custId");
            var oldValue = obj.attr("oldValue");
            var currValue = obj.val();
            currValue = SeparateNumricByThousand(currValue);
            obj.val(currValue);
            if (oldValue.trim() != currValue.trim()) {
                $('#btnUpdate_' + matchId + '_' + custId).removeAttr('disabled');
            }
            else {
                $('#btnUpdate_' + matchId + '_' + custId).attr('disabled', true);
            }

        });
    },

    RenderCalendarFromTo: function (ids, opts) {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);

            }
        };
        var new_opts = $.extend(true, defaults, opts);
        var dates = $(ids).datepicker(new_opts);
        $(ids.split(',')[0]).datepicker("setDate", new Date());
        $(ids.split(',')[1]).datepicker("setDate", new Date());
    },

    ChangeTab: function () {
        $('#ListSMATab').click(function () {
            $(this).attr('class', 'current');
            $('#AddSMATab').removeAttr('class');
            $('#ListSMAContent').show();
            $('#AddSMAContent').hide();
        });

        $('#AddSMATab').click(function () {
            $(this).attr('class', 'current');
            $('#ListSMATab').removeAttr('class');
            $('#ListSMAContent').hide();
            $('#AddSMAContent').show();
            $('#txtMatchId').val('');
            $('#txtLeague').val('');
            $('#txtEventDate').val('');
            $('#txtTeam').val('');
            $('#txtSuperName').val('');
            $('#txtTurnover').val('');
        });
    },

    RenderSport: function () {
        var data = { mode: "0" };
        $.ajax({
            type: "POST",
            url: "GetSportType",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var idSport = $('#ddlSport');
                for (var i = 0; i < rs.length; i++) {
                    idSport.append('<option value="' + rs[i].Id + '">' + rs[i].Name + '</option>')
                }
            }
        });
    },

    RenderLeague: function (sportId, fromDate, toDate) {
        var data = { sport: sportId, fromDate: fromDate, toDate: toDate };
        $.ajax({
            type: "POST",
            url: "GetLeague",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var idLeague = $('#ddlLeague');
                idLeague.append('<option value="0">All Leagues</option>')
                for (var i = 0; i < rs.length; i++) {
                    idLeague.append('<option value="' + rs[i].Id + '">' + rs[i].Name + '</option>')
                }
            }
        });
    },

    ChangeSport: function () {
        $('#ddlLeague').empty();
        var sportId = $('#ddlSport').val();
        var fromDate = $('#txtfromdate').val();
        var toDate = $('#txttodate').val();
        SMATurnover.RenderLeague(sportId, fromDate, toDate);
    },

    GetInfoByMatch: function () {
        try { parent.BlockUI(); } catch (e) { }
        $('#msgError').hide();
        var data = { matchId: $('#txtMatchId').val() };
        $.ajax({
            type: "POST",
            url: "GetInfoByMatchId",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null) {
                    if (rs.ErrCode != 0) {
                        $('#msgError').show();
                        showAlertMessage("msgError", rs.ErrMsg, 1);
                        setTimeout(function () { $('#msgError').hide(); }, 4000);
                        $('#txtLeague').val('');
                        $('#txtEventDate').val('');
                        $('#txtTeam').val('');
                    }
                    else {
                        $('#msgError').hide();
                        $('#txtLeague').val(rs.LeagueName);
                        $('#txtEventDate').val(rs.EventDate);
                        $('#txtTeam').val(rs.HomeTeam + ' -vs- ' + rs.AwayTeam);
                    }
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    GetSMATurnoverList: function () {
        try { parent.BlockUI(); } catch (e) { }
        var data = { sportId: $('#ddlSport').val(), leagueId: $('#ddlLeague').val(), fromDate: $('#txtfromdate').val(), toDate: $('#txttodate').val() };
        $.ajax({
            type: "POST",
            url: "GetSMATurnoverList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var arr = [];
                if (rs != null && rs.length > 0) {
                    $("#tblContentHeader tbody").empty().append(SMATurnover.renderFrozenRow(rs));
                    $("#tblSMATurnover tbody").empty().append(SMATurnover.renderRow(rs));
                }
                else {
                    $("#tblContentHeader tbody").empty().append('<tr><td><div class="">&nbsp;</div></td></tr>');
                    $("#tblSMATurnover tbody").empty().append('<tr><td><div class="">&nbsp;</div></td><td colspan="4"><div class="c">There is no data.</div></td></tr>');
                }

                SMATurnover.initGrid();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    initGrid: function () {
        var gridOpts = {
            tblContentId: 'tblSMATurnover',
            manualCopyHeader: true,
            fullWidth: true,
            gridWidth: 705, //=totalColumnWidth + totalColumn*4 +totalColumn
            gridHeight: 200,
            freezeWidth: 200,
            numberFreezeCol: 1
        };

        $("#SMATurnover").removeClass("hidden").gridScroller(gridOpts);
        SMATurnover.makeTwoTableSameHeight($("#SMATurnover table.row_header_content"), $("#SMATurnover table.col_header_content"), false, true);
    },

    renderRow: function (result) {
        var html = '';
        var countUser = 0;
        for (var index = 0; index < result.length; index++) {
            var row = result[index];
            var matchLength = row.CustomerList.length;
            var matchList = row.CustomerList;
            for (var i = 0; i < matchLength; i++) {
                var match = matchList[i]
                html += '<tr>';
                //html += '<td colspan="5" style="background-color:#D9E9FD; font-weight:bold; height:17px"><span style="width:200px">' + row.LeagueName + '</span><span style="width:250px">MatchID: ' + match.MatchId + ' (' + match.EventDate + ')</span><span style="width:170px">' + match.HomeTeam + ' -vs-' + match.AwayTeam + '</span><span style="">Add SMA</span></td>';
                html += '<td class="rowLeague"><div class="col200">' + row.LeagueName + '</div></td>';
                html += '<td class="rowLeague" colspan="2"><div class="">MatchID: ' + match.MatchId + '(' + match.EventDate + ')</div></td>';
                html += '<td class="rowLeague"><div class="col200">' + match.HomeTeam + ' -vs- ' + match.AwayTeam + '</div></td>';
                html += '<td class="rowLeague"><div class="col60" ><input type="button" class="colEdit" value="Add SMA" onclick="SMATurnover.RedirectToAddSMA(\'' + match.MatchId + '\')"></div></td>';
                html += '</tr>';
                var customerLength = match.CustomerList.length;
                var customerList = match.CustomerList;
                for (var j = 0; j < customerLength; j++) {
                    var customer = customerList[j];
                    html += '<tr>';
                    html += '<td><div class="col200 c">' + customer.Username + '</div></td>';
                    html += '<td><div class="col100 c ">' + customer.Currency + '</div></td>';
                    html += '<td><div class="col160 r ">' + customer.SMATurnover + '</div></td>';
                    if (parseInt(customer.IsSettled) == 0) {
                        html += '<td><div class="col200 table-cell" style="height:17px"><input type="text" class="MaxTurnover" id="txtMaxTurnover_' + match.MatchId + "_" + customer.CustId + '" matchId="' + match.MatchId + '" custId="' + customer.CustId + '" oldValue="' + customer.MaxTurnover + '" value="' + customer.MaxTurnover + '" style="text-align:right; width:197px" onkeypress="return CheckDecNumberValue(event)"></div></td>';
                    }
                    else {
                        html += '<td><div class="col200 r" style="height:17px">' + customer.MaxTurnover + '</div></td>';
                    }
                    html += '<td><div class="col60 c" style="height:17px"><input type="button" disabled="disabled" id="btnUpdate_' + match.MatchId + "_" + customer.CustId + '" title="Update" onclick="SMATurnover.UpdateSMATurnoverProccess(\'' + customer.CustId + '\',\'' + match.MatchId + '\',\'' + customer.Username + '\')" class="btnEdit" /></div></td>';
                    html += '</tr>';
                    countUser++;
                }
            }
        }

        return html;
    },

    renderFrozenRow: function (result) {
        var html = '';
        for (var index = 0; index < result.length; index++) {
            var row = result[index];
            var matchLength = row.CustomerList.length;
            var matchList = row.CustomerList;
            for (var i = 0; i < matchLength; i++) {
                var match = matchList[i]
                html += '<tr>';
                //html += '<td colspan="5" style="background-color:#D9E9FD; font-weight:bold; height:17px"><span style="width:200px">' + row.LeagueName + '</span><span style="width:250px">MatchID: ' + match.MatchId + ' (' + match.EventDate + ')</span><span style="width:170px">' + match.HomeTeam + ' -vs-' + match.AwayTeam + '</span><span style="">Add SMA</span></td>';
                html += '<td class="rowLeague"><div class="col200" >' + row.LeagueName + '</div></td>';

                html += '</tr>';
                var customerLength = match.CustomerList.length;
                var customerList = match.CustomerList;
                for (var j = 0; j < customerLength; j++) {
                    var customer = customerList[j];
                    html += '<tr>';
                    html += '<td><div class="col200 c">' + customer.Username + '</div></td>';
                    html += '</tr>';
                }
            }
        }
        return html;
    },

    RedirectToAddSMA: function (matchId) {
        $('#ListSMAContent').hide();
        $('#AddSMAContent').show();
        $('#ListSMATab').removeAttr('class');
        $('#AddSMATab').attr('class', 'current');
        $('#txtMatchId').val(matchId);
        $('#txtSuperName').val('');
        $('#txtTurnover').val('');
        SMATurnover.GetInfoByMatch();
    },

    AddSMATurnoverProccess: function () {
        var matchId = $('#txtMatchId').val();
        var custName = $('#txtSuperName').val();
        var maxTurnover = $('#txtTurnover').val();
        try { parent.BlockUI(); } catch (e) { }
        var data = { matchId: matchId, custName: custName, maxTurnover: maxTurnover, mode: 0 };
        $.ajax({
            type: "POST",
            url: "CheckMaxTurnoverValue",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (parseInt(rs.ErrorCode) == 0) {
                    SMATurnover.AddSuperTurnoverValue(matchId, custName, maxTurnover);
                }
                else if (parseInt(rs.ErrorCode) == 100) {
                    showAlertMessage("msgErrorAdd", rs.ErrorMessage, 1);
                    $('#msgErrorAdd').show();
                    setTimeout(function () { $('#msgError').hide(); }, 4000);
                }
                else {
                    if (!confirm(rs.ErrorMessage)) {
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                        return;
                    }
                    else {
                        SMATurnover.AddSuperTurnoverValue(matchId, custName, maxTurnover);
                    }
                }

                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    AddSuperTurnoverValue: function (matchId, custName, maxTurnover) {
        var data = { matchId: matchId, custName: custName, maxTurnover: maxTurnover };
        $.ajax({
            type: "POST",
            url: "AddSMATurnover",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (parseInt(rs.ErrorCode) == 0) {
                    showAlertMessage("msgErrorAdd", "Add successfully!", 0);
                }
                else {
                    showAlertMessage("msgErrorAdd", rs.ErrorMessage, 1);
                }
                $('#msgErrorAdd').show();
                setTimeout(function () { $('#msgErrorAdd').hide(); }, 4000);
            }
        });
    },

    UpdateSMATurnoverProccess: function (custId, matchId, custName) {
        var maxTurnover = $("#txtMaxTurnover_" + matchId + "_" + custId).val();
        try { parent.BlockUI(); } catch (e) { }
        var dataCheckSuperTurnoverValue = { matchId: matchId, custName: custName, maxTurnover: maxTurnover, mode: 1 };
        $.ajax({
            type: "POST",
            url: "CheckMaxTurnoverValue",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataCheckSuperTurnoverValue),
            success: function (rs) {
                if (parseInt(rs.ErrorCode) == 0) {
                    SMATurnover.UpdateSuperTurnoverValue(matchId, custId, maxTurnover);
                }
                else if (parseInt(rs.ErrorCode) == 100) {
                    showAlertMessage("msgError", rs.ErrorMessage, 1);
                    $('#msgError').show();
                    setTimeout(function () { $('#msgError').hide(); }, 4000);
                }
                else {
                    if (!confirm(rs.ErrorMessage)) {
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                        return;
                    }
                    else {
                        SMATurnover.UpdateSuperTurnoverValue(matchId, custId, maxTurnover);
                    }

                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },

    UpdateSuperTurnoverValue: function (matchId, custId, maxTurnover) {
        var data = { matchId: matchId, custId: custId, maxTurnover: maxTurnover };
        $.ajax({
            type: "POST",
            url: "UpdateSMATurnover",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (parseInt(rs.ErrorCode) == 0) {
                    showAlertMessage("msgError", "Update successfully!", 0);
                }
                else {
                    showAlertMessage("msgError", rs.ErrorMessage, 1);
                }
                $('#msgError').show();
                SMATurnover.GetSMATurnoverList();
                setTimeout(function () { $('#msgError').hide(); }, 4000);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    makeTwoTableSameHeight: function (tableA, tableB, isFormatHeader, isFormatBody) {
        if (isFormatBody) {
            var rows_TableA = tableA.find("tbody tr");
            var rows_TableB = tableB.find("tbody tr");
            var minRowLength = rows_TableA.length < rows_TableB.length ? rows_TableA.length : rows_TableB.length;
            for (var i = 0; i < minRowLength; i++) {
                var columns_TableA = $(rows_TableA[i]).find("td");
                var columns_TableB = $(rows_TableB[i]).find("td");
                var minColumnLength = columns_TableA.length < columns_TableB.length ? columns_TableA.length : columns_TableB.length;
                if (minColumnLength > 0) {
                    var height_TableA = $(columns_TableA[0]).height();
                    var height_TableB = $(columns_TableB[0]).height();
                    var maxHeight = height_TableA > height_TableB ? height_TableA : height_TableB;

                    if (height_TableA != height_TableB) {
                        $(columns_TableA[0]).css("height", maxHeight);
                        $(columns_TableB[0]).css("height", maxHeight);
                    }
                }
            }
        }
    },

    InputMaxTurnover: function () {
        var maxTurnover = $('#txtTurnover').val();
        $('#txtTurnover').val(SeparateNumricByThousand(maxTurnover));

    }
}

function addCommas(n) {
    var rx = /(\d+)(\d{3})/;
    return String(n).replace(/^\d+/, function (w) {
        while (rx.test(w)) {
            w = w.replace(rx, '$1,$2');
        }
        return w;
    });
}

function SeparateNumricByThousand(number) {
    var value = "";
    stringarr = number.split(',');
    for (var i = 0; i < stringarr.length; i++) {
        value += stringarr[i];
    }
    return addCommas(value);
}


function CheckDecNumberValue(e) {
    var code = "";
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    if (code == 9) {
        return code;
    }
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 45 && code != 37 && code != 39) {
        return false;
    }
    return true;
}

function onKeyEnter(e) {
    if (e.keyCode == 13) {
        SMATurnover.GetInfoByMatch();
    }
}
