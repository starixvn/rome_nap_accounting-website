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
    }


}