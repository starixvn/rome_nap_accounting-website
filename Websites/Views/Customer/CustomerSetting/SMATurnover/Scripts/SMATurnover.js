var SMATurnover = {
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
            }
        });
    },

    GetSMATurnoverList: function () {
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
                    $("#tblSMATurnover tbody").empty().append(SMATurnover.renderRow(rs));
                    SMATurnover.initGrid();
                }
                else {
                    $("#tblSMATurnover").html('<tr><td colspan="5"><div class="c" style="width:730px;">There is no information available.</div></td></tr>');
                }
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
            freezeWidth: 0,
            numberFreezeCol: 0
        };
        $("#SMATurnover").removeClass("hidden").gridScroller(gridOpts);
    },

    renderRow: function (result) {
        var html = '';
        for (var index = 0; index < result.length; index++) {
            var row = result[index];
            var matchLength = row.CustomerList.length;
            var matchList = row.CustomerList;
            for (var i = 0; i < matchLength; i++) {
                var match = matchList[i]
                html += '<tr>';
                //html += '<td colspan="5" style="background-color:#D9E9FD; font-weight:bold; height:17px"><span style="width:200px">' + row.LeagueName + '</span><span style="width:250px">MatchID: ' + match.MatchId + ' (' + match.EventDate + ')</span><span style="width:170px">' + match.HomeTeam + ' -vs-' + match.AwayTeam + '</span><span style="">Add SMA</span></td>';
                html += '<td  style="border:none; background-color:#D9E9FD; font-weight:bold;"><div class="col200 table-cell" style="height:17px">' + row.LeagueName + '<div></td>';
                html += '<td   style="border:none; background-color:#D9E9FD; font-weight:bold;" colspan="2"><div class="table-cell" style="height:17px">MatchID: ' + match.MatchId + '(' + match.EventDate + ')<div></td>';
                html += '<td  style="border:none; background-color:#D9E9FD; font-weight:bold;"><div class="col160 table-cell">' + match.HomeTeam + ' -vs-' + match.AwayTeam + '<div></td>';
                html += '<td  style="border:none; background-color:#D9E9FD; font-weight:bold;"><div class="col60 table-cell" style="height:17px">Add SMA<div></td>';
                html += '</tr>';
                var customerLength = match.CustomerList.length;
                var customerList = match.CustomerList;
                for (var j = 0; j < customerLength; j++) {
                    var customer = customerList[j];
                    html += '<tr>';
                    html += '<td><div class="col200 c table-cell" style="height:17px">' + customer.Username + '<div></td>';
                    html += '<td><div class="col100 c table-cell" style="height:17px">' + customer.Currency + '<div></td>';
                    html += '<td><div class="col100 c table-cell" style="height:17px">' + customer.SMATurnover + '<div></td>';
                    if (parseInt(customer.IsSettled) == 0) {
                        html += '<td><div class="col100 c table-cell" style="height:17px"><input type="text" value="' + customer.MaxTurnover + '" style="text-align:right; width:155px"><div></td>';
                    }
                    else {
                        html += '<td><div class="col100 c table-cell" style="height:17px">' + customer.MaxTurnover + '</div></td>';
                    }
                    html += '<td><div class="col60 c table-cell" style="height:17px"><input type="button" title="Update" onclick="" class="btnEdit" /></div></td>'
                    html += '</tr>';
                }
            }
        }

        return html;
    },

    AddSMATurnover: function () {
        var data = { matchId: $('#txtMatchId').val(), userName: $('#txtUsername').val(), maxTurnover: $('#txtMaxTurnover').val() };
        $.ajax({
            type: "POST",
            url: "AddSMATurnover",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (parseInt(rs.ErrorCode) == 0) {
                    showAlertMessage("msgError", "Add successfully!", 0);
                }
                else {
                    showAlertMessage("msgError", rs.ErrorMessage, 1);
                }
                $('#msgError').show();
                setTimeout(function () { $('#msgError').hide(); }, 4000);
            }
        });
    }


}