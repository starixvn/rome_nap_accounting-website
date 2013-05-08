var MatchSelectionManager = {
    lastEventDayPicker: null,
    getLeagueList: function () {
        if ($("#InformationMssg").html() != "") {
            $("#InformationMssg").hide();
            $("#InformationMssg").empty();
        }
        MatchSelectionManager.lastEventDayPicker = $("#txtfromdate").val();
        var data = {
            param: { EventDate: $("#txtfromdate").val(), MoveBetsDay: moveBetDay, BalanceUpDay: balanceUpDay }
        };

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "MatchAnalysis/GetLeagueList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs.length == 0) {
                    $("#ddlLeague").multiSelect(
                    {
                        minWidth: 300,
                        maxHeight: 450,
                        shadow: true,
                        showHeader: true,
                        isCheckAll: true,
                        showFooter: false,
                        listData: []
                    });
                    $("#matchLeagueList").addClass('hidden');
                    $('#tblMatchDetail tbody').empty();
                    showAlertMessage("InformationMssg", "No league found.", 1);
                    $("#InformationMssg").show();
                }
                else {
                    MatchSelectionManager.renderLeagues(rs);
                }
            },
            error: function () {
                $("#ddlLeague").multiSelect(
                    {
                        minWidth: 300,
                        maxHeight: 450,
                        shadow: true,
                        showHeader: true,
                        isCheckAll: true,
                        showFooter: false,
                        listData: []
                    });
                $("#matchLeagueList").addClass('hidden');
                $('#tblMatchDetail tbody').empty();
                showAlertMessage("InformationMssg", "No league found.", 1);
                $("#InformationMssg").show();
            },
            complete: function () {
                $("#matchSelectionDiv").show();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    viewMatchList: function () {
        if (!isAllowRead) {
            return;
        }
        var currentDate = new Date(today);
        currentDate.setMonth(currentDate.getMonth() - 6);
        var minDate;
        if (currentDate.getYear() + 1900 <= currentYear) {
            minDate = new Date(currentDate.getYear() + 1900, currentDate.getMonth(), currentDate.getDate(), 0, 0, 0, 0);
        }
        else {
            minDate = new Date(currentDate.getYear(), currentDate.getMonth(), currentDate.getDate(), 0, 0, 0, 0);
        }
        var selectedDate = new Date($("#txtfromdate").val());
        if (selectedDate < minDate) {
            return;
        }

        try { parent.BlockUI(); } catch (e) { }
        $("#InformationMssg").hide();
        $("#InformationMssg").empty();
        var leagueIds = MatchSelectionManager.getSelectedLeagues();
        var data = {
            param: {
                EventDate: $("#txtfromdate").val(),
                LeagueIds: leagueIds,
                MoveBetsDay: moveBetDay,
                BalanceUpDay: balanceUpDay
            }
        };
        var mainreport = $('#matchLeagueList');
        $.ajax({
            type: "POST",
            url: window.rootUrl + "MatchAnalysis/GetMatchList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                MatchAnalysisManager.matchLeagueList = rs;
                if (rs && rs.length > 0) {
                    $('#tblMatchDetailHeader tbody').empty();

                    var frozenHtml = '';
                    var len = rs.length;
                    for (var i = 0; i < len; i++) {
                        var league = rs[i];
                        var matchLength = league.Matches.length;
                        frozenHtml += '<tr class="site"><td colspan="2" style="background-color:#FFEEDD;"><div class="b" style="width:800px;  height: 17px"><span class="tree-expanded" onclick="MatchSelectionManager.toggleDetail(this,\'' + league.Id + '\')"></span>' + league.Name + '</div></td></tr>';

                        for (var j = 0; j < matchLength; j++) {
                            var leagueDetail = league.Matches[j];
                            frozenHtml += '<tr class="lic_' + league.Id + '">\
                                              <td><div style="width: 150px; text-align: center; color: blue; height: 17px"><a style="cursor: pointer;" onclick="MatchSelectionManager.matchClick(' + "'" + data.param.EventDate + "'," + "'" + i + "', " + "'" + j + "'" + ')">&nbsp;' + leagueDetail.Name + '</a></div></td>\
                                              <td><div style="width: 650px; text-align: left; height: 17px">&nbsp;' + MatchAnalysisManager.getMatchDetail(leagueDetail.HomeTeam.Name, '<font color="#428AA0"> vs </font>', leagueDetail.AwayTeam.Name, leagueDetail.SportType.Id, leagueDetail.ShowTime, leagueDetail.Name) + '</div></td>\
                                          </tr>';
                        }
                    }
                    if ($.browser.msie && parseInt($.browser.version, 10) === 9) {
                        frozenHtml = MatchAnalysisManager.removeSpaceIE9(frozenHtml);
                    }
                    $('#tblMatchDetailHeader tbody').html(frozenHtml);
                    var gridOpts = {
                        tblContentId: 'tblMatchDetailHeader',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: 100,
                        freezeWidth: 805,
                        numberFreezeCol: 2
                    };
                    mainreport.removeClass("hidden").gridScroller(gridOpts);
                    var t = setTimeout(function () {
                        $("#InformationMssg").hide();
                        $("#InformationMssg").empty();
                    }, 4000);
                }
                else {
                    mainreport.addClass("hidden");
                    $('#tblMatchDetailHeader tbody').empty();
                    showAlertMessage("InformationMssg", "No match found.", 1);
                    $("#InformationMssg").show();
                }
            },
            error: function () {
                showAlertMessage("InformationMssg", "No match found.", 1);
                $("#InformationMssg").show();
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderLeagues: function (data) {
        var tempData = [];
        for (var i = 0; i < data.length; i++) {
            var tempLeagues = [];
            for (var j = 0; j < data[i].Leagues.length; j++) {
                tempLeagues.push({
                    Id: "#ddlLeagueDetail" + data[i].Leagues[j].Id.toString(),
                    Value: data[i].Leagues[j].Id,
                    Text: data[i].Leagues[j].Name,
                    Checked: true,
                    Enable: true,
                    CssClass: ""
                });
            }
            tempData.push({
                Id: "#ddlLeague" + data[i].Id.toString(),
                Value: data[i].Id,
                Text: data[i].Name,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: tempLeagues
            });
        }
        $("#ddlLeague").multiSelect(
        {
            minWidth: 500,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            listData: tempData
        });
    },
    toggleDetail: function (obj, leagueId) {
        if ($(obj).hasClass("tree-expand")) {
            $(obj).removeClass("tree-expand");

            $("#tblMatchDetail tbody tr.lic_" + leagueId).removeClass("hidden");
            $("#tblMatchDetailHeader tbody tr.lic_" + leagueId).removeClass("hidden");
        }
        else {
            $(obj).addClass("tree-expand");

            $("#tblMatchDetail tbody tr.lic_" + leagueId).addClass("hidden");
            $("#tblMatchDetailHeader tbody tr.lic_" + leagueId).addClass("hidden");
        }

        var gridOpts = {
            tblContentId: 'tblMatchDetailHeader',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 100,
            freezeWidth: 806,
            numberFreezeCol: 2
        };
        $('#matchLeagueList').removeClass("hidden").gridScroller(gridOpts);
    },
    getSelectedLeagues: function () {
        var selectedList = $('#ddlLeague').multiSelect('getCheckedItems');
        var result = "";
        for (var i = 0; i < selectedList.length; i++) {
            result = result + selectedList[i] + ",";
        }
        result = result.replace(/,*$/, "");
        return result;
    },
    matchClick: function (eventDate, leagueIndex, matchIndex) {
        if (!isAllowRead) {
            return;
        }
        MatchAnalysisManager.showMatchReportDetails(eventDate, leagueIndex, matchIndex);
    },
    isCalendarOpen: false,
    isfirstCalendarSetUp: true,
    setupCalendar: function (id, strdatetime) {
        if (MatchSelectionManager.isfirstCalendarSetUp) {
            var currentDate = new Date(today);
            currentDate.setMonth(currentDate.getMonth() - 6);
            var minDate;
            if (currentDate.getYear() + 1900 <= currentYear) {
                minDate = new Date(currentDate.getYear() + 1900, currentDate.getMonth(), currentDate.getDate(), 0, 0, 0, 0);
            }
            else {
                minDate = new Date(currentDate.getYear(), currentDate.getMonth(), currentDate.getDate(), 0, 0, 0, 0);
            }

            var limitDate = {
                minDate: minDate,
                maxDate: new Date(today)
            };
            MatchSelectionManager.renderCalendar('#txtfromdate', limitDate);
            $('#txtfromdate').click(function () {
                if (MatchSelectionManager.isCalendarOpen) {
                    $('#txtfromdate').datepicker("hide");
                    MatchSelectionManager.isCalendarOpen = false;
                }
                else {
                    $('#txtfromdate').datepicker("show");
                    MatchSelectionManager.isCalendarOpen = true;
                }
            });
            MatchSelectionManager.isfirstCalendarSetUp = false;
        }
    },
    renderCalendar: function (ids, opts) {
        var defaults = {
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
                if (MatchSelectionManager.lastEventDayPicker != $("#txtfromdate").val()) {
                    MatchSelectionManager.getLeagueList();
                    MatchSelectionManager.lastEventDayPicker = $("#txtfromdate").val();
                }
            },
            onClose: function () {

                MatchSelectionManager.isCalendarOpen = false;
            }
        };
        var new_opts = $.extend(true, defaults, opts);
        var dates = $(ids).datepicker(new_opts);
        $(ids).datepicker("setDate", new Date(opts.maxDate));
    },
    updateLeagueList: function () {
        if (MatchSelectionManager.lastEventDayPicker != $("#txtfromdate").val()) {
            MatchSelectionManager.getLeagueList();
            MatchSelectionManager.lastEventDayPicker = $("#txtfromdate").val();
        }
    }
};

$(document).ready(function () {
    var gridOpts = {
        tblContentId: 'tblMatchDetailHeader',
        isShowPager: false
    };
    $("#matchLeagueList").initGrid(gridOpts);

    MatchAnalysisManager.showMatchSelection();
}); 