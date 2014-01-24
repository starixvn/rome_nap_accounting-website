$(document).ready(function () {
    var limitFromDate = {
        minDate: minDay,
        maxDate: toDay
    };
    var options = {
        showOtherMonths: false,
        selectOtherMonths: false,
        checkMinMax: true
    };
    calendarUtils.renderCalendar("#fdate", limitFromDate, "#fdate", "", fDay, options);
    calendarUtils.renderCalendar("#tdate", limitFromDate, "#fdate", "#tdate", tDay, options);

    var gridOpts = {
        tblContentId: 'traderPerformanceReport_Container',
        isShowPager: false
    };
    $("#traderPerformanceReport").initGrid(gridOpts);
    $("#traderPerformanceReport").find(".free-cell-bottom-left").css("border-bottom", "none");
    permissionManagement.viewReport();
});

var permissionManagement = {
    permissionType: "None",
    isSoccer: false,
    getPermissionType: function () {
        if (hasTraderSoccerPermission == 1) {
            if (hasTraderOtherSportsPermission == 1) {
                permissionManagement.permissionType = "All";
            }
            else {
                permissionManagement.permissionType = "Soccer";
            }
        } else {
            if (hasTraderOtherSportsPermission == 1) {
                permissionManagement.permissionType = "OtherSports";
            }
            else {
                permissionManagement.permissionType = "None";
            }
        }
    },
    viewReport: function () {
        permissionManagement.getPermissionType();
        permissionManagement.hideLink();
        traderPerformanceManager.initGrid();

        var isSoccerObj = $('#IsSoccer');
        var soccer = $('#soccer');
        var other = $('#other');

        switch (permissionManagement.permissionType) {
            case "All":
                permissionManagement.isSoccer = 1;
                permissionManagement.switchView(1);
                break;
            case "Soccer":
                permissionManagement.isSoccer = 1;
                soccer.removeClass('lnk');
                soccer.addClass('blueText');
                traderPerformanceManager.viewReport();
                break;
            case "OtherSports":
                permissionManagement.isSoccer = 0;
                other.removeClass('lnk');
                other.addClass('blueText');
                traderPerformanceManager.viewReport();
                break;
            case "None":
                soccer.removeClass('lnk');
                other.removeClass('lnk');
                break;
        }
    },
    hideLink: function () {
        var soccer = $('#soccer');
        var permissionSeperator = $("#permissionSeperator");
        var other = $('#other');

        switch (permissionManagement.permissionType) {
            case "All":
                soccer.removeClass("hidden");
                permissionSeperator.removeClass("hidden");
                other.removeClass("hidden");
                break;
            case "Soccer":
                soccer.removeClass("hidden");
                permissionSeperator.addClass("hidden");
                other.addClass("hidden");
                break;
            case "OtherSports":
                soccer.addClass("hidden");
                permissionSeperator.addClass("hidden");
                other.removeClass("hidden");
                break;
            case "None":
                break;
        }
    },
    switchView: function (viewId) {
        var soccer = $('#soccer');
        var other = $('#other');
        switch (viewId) {
            case 0:
                soccer.addClass('lnk');
                soccer.removeClass('blueText');
                soccer.bind('click', function (e) {
                    permissionManagement.switchView(1);
                });

                other.unbind('click');
                other.removeClass('lnk');
                other.removeClass('blueText');
                permissionManagement.isSoccer = 0;
                traderPerformanceManager.viewReport();
                break;
            case 1:
                other.addClass('lnk');
                other.removeClass('blueText');
                other.bind('click', function (e) {
                    permissionManagement.switchView(0);
                });
                soccer.unbind('click');
                soccer.removeClass('lnk');
                soccer.removeClass('blueText');
                permissionManagement.isSoccer = 1;
                traderPerformanceManager.viewReport();
                break;
        }

    }
};

var traderPerformanceManager = {
    enums: {
        SortDirection: {
            Desc: "desc",
            Asc: "asc",
            None: ""
        },

        SortDataType: {
            Number: "number",
            String: "string"
        }
    },
    param: {
        FromDate: "",
        ToDate: "",
        IsSooccer: 0
    },
    cacheData: {
        sorterList: null,
        currentSorter: null,
        report: null,

        sortDataType: null,
        sortField: null,
        sortFactor: null
    },
    setParam: function () {
        traderPerformanceManager.param.FromDate = $("#fdate").val();
        traderPerformanceManager.param.ToDate = $("#tdate").val();
        traderPerformanceManager.param.IsSooccer = permissionManagement.isSoccer;
    },
    initGrid: function () {
        traderPerformanceManager.initReportElement();
    },
    initReportElement: function () {
        traderPerformanceManager.gridtitleSummary = $("#gridtitleSummary");
        traderPerformanceManager.traderPerformanceReport = $("#traderPerformanceReport");
        traderPerformanceManager.traderPerformanceReportContainer = $("#traderPerformanceReport_Container");
    },
    viewReport: function () {
        try { parent.BlockUI(); } catch (e) { }

        var reportGrid = traderPerformanceManager.traderPerformanceReport;
        traderPerformanceManager.setParam();
        reportGrid.addClass("hidden");
        reportGrid.resetGrid("traderPerformanceReport_Container");
        traderPerformanceManager.gridtitleSummary.html("Trader Performance - From Date: " +
                            traderPerformanceManager.param.FromDate + " To Date: " +
                            traderPerformanceManager.param.ToDate);
        $.ajax({
            type: "POST",
            url: "../../Services/WCF/Report/TraderPerformanceService.svc/GetTraderPerformance",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"fromDate":"' + traderPerformanceManager.param.FromDate + '","toDate":"' +
                        traderPerformanceManager.param.ToDate + '","isSoccer":"' +
                        traderPerformanceManager.param.IsSooccer + '"}',
            success: function (rs) {
                traderPerformanceManager.renderReport(rs);
            },
            error: function () {
                //                traderPerformanceManager.traderPerformanceReport.find(".col_header_content thead").empty().append(traderPerformanceManager.displayTraderPerformanceReportHeader(traderPerformanceManager.param.IsSooccer));
                //                $("#traderPerformanceReport_Container tbody").empty().append(traderPerformanceManager.displayTraderPerformanceReportNoDetail());
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderReport: function (rs, isSorting) {
        var reportGrid = traderPerformanceManager.traderPerformanceReport;
        if (!isSorting) {
            traderPerformanceManager.traderPerformanceReport.find(".col_header_content thead").empty().append(traderPerformanceManager.displayTraderPerformanceReportHeader(traderPerformanceManager.param.IsSooccer));
        }

        $("#traderPerformanceReport_Container tbody").empty().append(traderPerformanceManager.displayTraderPerformanceReport(rs));
        reportGrid.removeClass("hidden");
        var gridOpts = {
            tblContentId: 'traderPerformanceReport_Container',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 200,
            gridWidth: 710,
            freezeWidth: 405,
            freezeHeight: 56,
            numberFreezeCol: 8
        };

        traderPerformanceManager.traderPerformanceReport.gridScroller(gridOpts);
        traderPerformanceManager.make2TableHeaderSameHeight($(".row_footer_content"), $(".col_footer_content"));
        traderPerformanceManager.displaySorter(rs);
    },
    displayTraderPerformanceReport: function (rs) {
        var bodyHtml = "";
        if (rs.rows != null && rs.rows.length > 0) {
            var frozenHtml = "";
            var traderPerformanceDetail = null;
            var totalRow = rs.rows.length;
            for (var i = 0; i < totalRow; i++) {
                traderPerformanceDetail = rs.rows[i];
                bodyHtml += traderPerformanceManager.renderEachRowScroll(traderPerformanceDetail, traderPerformanceManager.param.IsSooccer, i);
                frozenHtml += traderPerformanceManager.renderEachRowFrozen(i, traderPerformanceDetail);
            }

            $("#traderPerformanceReport_RowHeader tbody").empty().append(frozenHtml);
            traderPerformanceManager.displayTraderPerformanceFooter(rs.footers[0], traderPerformanceManager.param.IsSooccer);
        }
        else {
            bodyHtml = traderPerformanceManager.displayTraderPerformanceReportNoDetail();
        }

        return bodyHtml;
    },
    displayTraderPerformanceFooter: function (footer, isSooccer) {
        traderPerformanceManager.traderPerformanceReport.find(".row_footer_content thead").empty().append(traderPerformanceManager.renderTraderPerformanceReportFooterFrozen(footer));
        traderPerformanceManager.traderPerformanceReport.find(".col_footer_content thead").empty().append(traderPerformanceManager.renderTraderPerformanceReportFooterScroll(footer, isSooccer));
    },
    displayTraderPerformanceReportNoDetail: function (isSoccer) {
        var colspan = 76;
        if (isSoccer == 0) {
            colspan = 46;
        }
        var html = "";
        frozenHtml = '<tr><td style="height:17px;" colspan="8">&nbsp</td></tr>';
        $("#traderPerformanceReport_RowHeader tbody").empty().append(frozenHtml);
        html = '<tr><td style="height:17px;" colspan="8">&nbsp</td><td class="l b" style="height:17px;" colspan="' + colspan + '">There is no information available.</td></tr>';

        traderPerformanceManager.traderPerformanceReport.find(".row_footer_content thead").empty();
        traderPerformanceManager.traderPerformanceReport.find(".col_footer_content thead").empty();

        return html;
    },
    renderEachRowFrozen: function (i, n) {
        var row = "";
        var major = parseInt(n.Major) == 0 ? '&nbsp;' : n.Major;
        var minor1 = parseInt(n.Minor1) == 0 ? '&nbsp;' : n.Minor1;
        var minor2 = parseInt(n.Minor2) == 0 ? '&nbsp;' : n.Minor2;
        var minor3 = parseInt(n.Minor3) == 0 ? '&nbsp;' : n.Minor3;
        var others = parseInt(n.Others) == 0 ? '&nbsp;' : n.Others;

        row += '<tr class="' + ((i + 1) % 2 == 0 ? 'evenrow' : 'oddrow') + '">';
        row += '<td><div class="col30 l">' + (i + 1) + '</div></td>';
        row += '<td><div class="col100 l">' + n.UserName + '</div></td>';
        row += '<td><div class="col40 r">' + n.MatchNo + '</div></td>';
        row += '<td><div class="col40 r">' + major + '</div></td>';
        row += '<td><div class="col40 r">' + minor1 + '</div></td>';
        row += '<td><div class="col40 r">' + minor2 + '</div></td>';
        row += '<td><div class="col40 r">' + minor3 + '</div></td>';
        row += '<td><div class="col40 r">' + others + '</div></td>';
        row += '</tr>';
        return row;
    },
    renderEachRowScroll: function (n, isSoccer, i) {
        var major = parseInt(n.Major) == 0 ? '&nbsp;' : n.Major;
        var minor1 = parseInt(n.Minor1) == 0 ? '&nbsp;' : n.Minor1;
        var minor2 = parseInt(n.Minor2) == 0 ? '&nbsp;' : n.Minor2;
        var minor3 = parseInt(n.Minor3) == 0 ? '&nbsp;' : n.Minor3;
        var others = parseInt(n.Others) == 0 ? '&nbsp;' : n.Others;
        var row = "";
        row += '<tr class="' + ((i + 1) % 2 == 0 ? 'evenrow' : 'oddrow') + '">';

        row += '<td><div class="col30 l">' + (i + 1) + '</div></td>';
        row += '<td><div class="col100 l">' + n.UserName + '</div></td>';
        row += '<td><div class="col40 r">' + n.MatchNo + '</div></td>';
        row += '<td><div class="col40 r">' + major + '</div></td>';
        row += '<td><div class="col40 r">' + minor1 + '</div></td>';
        row += '<td><div class="col40 r">' + minor2 + '</div></td>';
        row += '<td><div class="col40 r">' + minor3 + '</div></td>';
        row += '<td><div class="col40 r">' + others + '</div></td>';

        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1st1x2) < 0 ? '<font style="color:#f11">(' + n.WinLost1st1x2 + ')</font>' : (n.WinLost1st1x2 == 0 ? '' : n.WinLost1st1x2)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1x2) < 0 ? '<font style="color:#f11">(' + n.WinLost1x2 + ')</font>' : (n.WinLost1x2 == 0 ? '' : n.WinLost1x2)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1stHdp) < 0 ? '<font style="color:#f11">(' + n.WinLost1stHdp + ')</font>' : (n.WinLost1stHdp == 0 ? '' : n.WinLost1stHdp)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostHdp) < 0 ? '<font style="color:#f11">(' + n.WinLostHdp + ')</font>' : (n.WinLostHdp == 0 ? '' : n.WinLostHdp)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1stOu) < 0 ? '<font style="color:#f11">(' + n.WinLost1stOu + ')</font>' : (n.WinLost1stOu == 0 ? '' : n.WinLost1stOu)) + '</div></td>';
        row += '<td class="a3"><div class="col100 r">&nbsp;' + (parseFloat(n.WinLostOu) < 0 ? '<font style="color:#f11">(' + n.WinLostOu + ')</font>' : (n.WinLostOu == 0 ? '' : n.WinLostOu)) + '</div></td>';
        if (isSoccer == 1) {
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost15minOu) < 0 ? '<font style="color:#f11">(' + n.WinLost15minOu + ')</font>' : (n.WinLost15minOu == 0 ? '' : n.WinLost15minOu)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost10minsOu) < 0 ? '<font style="color:#f11">(' + n.WinLost10minsOu + ')</font>' : (n.WinLost10minsOu == 0 ? '' : n.WinLost10minsOu)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostSingleTeamOu) < 0 ? '<font style="color:#f11">(' + n.WinLostSingleTeamOu + ')</font>' : (n.WinLostSingleTeamOu == 0 ? '' : n.WinLostSingleTeamOu)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.WinLost1stNoOfCorners + ')</font>' : (n.WinLost1stNoOfCorners == 0 ? '' : n.WinLost1stNoOfCorners)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.WinLostNoOfCorners + ')</font>' : (n.WinLostNoOfCorners == 0 ? '' : n.WinLostNoOfCorners)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostCorners15minsOU) < 0 ? '<font style="color:#f11">(' + n.WinLostCorners15minsOU + ')</font>' : (n.WinLostCorners15minsOU == 0 ? '' : n.WinLostCorners15minsOU)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostNextCorner) < 0 ? '<font style="color:#f11">(' + n.WinLostNextCorner + ')</font>' : (n.WinLostNextCorner == 0 ? '' : n.WinLostNextCorner)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1stTotalBookings) < 0 ? '<font style="color:#f11">(' + n.WinLost1stTotalBookings + ')</font>' : (n.WinLost1stTotalBookings == 0 ? '' : n.WinLost1stTotalBookings)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostTotalBookings) < 0 ? '<font style="color:#f11">(' + n.WinLostTotalBookings + ')</font>' : (n.WinLostTotalBookings == 0 ? '' : n.WinLostTotalBookings)) + '</div></td>';
            row += '<td class="a3"><div class="col100 r">&nbsp;' + (parseFloat(n.WinLostTotalBookings15minsOu) < 0 ? '<font style="color:#f11">(' + n.WinLostTotalBookings15minsOu + ')</font>' : (n.WinLostTotalBookings15minsOu == 0 ? '' : n.WinLostTotalBookings15minsOu)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostNextBookings) < 0 ? '<font style="color:#f11">(' + n.WinLostNextBookings + ')</font>' : (n.WinLostNextBookings == 0 ? '' : n.WinLostNextBookings)) + '</div></td>';
        }
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1stOe) < 0 ? '<font style="color:#f11">(' + n.WinLost1stOe + ')</font>' : (n.WinLost1stOe == 0 ? '' : n.WinLost1stOe)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostOe) < 0 ? '<font style="color:#f11">(' + n.WinLostOe + ')</font>' : (n.WinLostOe == 0 ? '' : n.WinLostOe)) + '</div></td>';
        row += '<td class="a3"><div class="col90 r">&nbsp;' + (parseFloat(n.WinLostOverall) < 0 ? '<font style="color:#f11">(' + n.WinLostOverall + ')</font>' : (n.WinLostOverall == 0 ? '' : n.WinLostOverall)) + '</div></td>';

        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1st1x2) < 0 ? '<font style="color:#f11">(' + n.TurnOver1st1x2 + ')</font>' : (n.TurnOver1st1x2 == 0 ? '' : n.TurnOver1st1x2)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1x2) < 0 ? '<font style="color:#f11">(' + n.TurnOver1x2 + ')</font>' : (n.TurnOver1x2 == 0 ? '' : n.TurnOver1x2)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1stHdp) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stHdp + ')</font>' : (n.TurnOver1stHdp == 0 ? '' : n.TurnOver1stHdp)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverHdp) < 0 ? '<font style="color:#f11">(' + n.TurnOverHdp + ')</font>' : (n.TurnOverHdp == 0 ? '' : n.TurnOverHdp)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1stOu) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stOu + ')</font>' : (n.TurnOver1stOu == 0 ? '' : n.TurnOver1stOu)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverOu) < 0 ? '<font style="color:#f11">(' + n.TurnOverOu + ')</font>' : (n.TurnOverOu == 0 ? '' : n.TurnOverOu)) + '</div></td>';
        if (isSoccer == 1) {
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver15minOu) < 0 ? '<font style="color:#f11">(' + n.TurnOver15minOu + ')</font>' : (n.TurnOver15minOu == 0 ? '' : n.TurnOver15minOu)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver10minsOu) < 0 ? '<font style="color:#f11">(' + n.TurnOver10minsOu + ')</font>' : (n.TurnOver10minsOu == 0 ? '' : n.TurnOver10minsOu)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverSingleTeamOu) < 0 ? '<font style="color:#f11">(' + n.TurnOverSingleTeamOu + ')</font>' : (n.TurnOverSingleTeamOu == 0 ? '' : n.TurnOverSingleTeamOu)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stNoOfCorners + ')</font>' : (n.TurnOver1stNoOfCorners == 0 ? '' : n.TurnOver1stNoOfCorners)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.TurnOverNoOfCorners + ')</font>' : (n.TurnOverNoOfCorners == 0 ? '' : n.TurnOverNoOfCorners)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverCorners15minsOU) < 0 ? '<font style="color:#f11">(' + n.TurnOverCorners15minsOU + ')</font>' : (n.TurnOverCorners15minsOU == 0 ? '' : n.TurnOverCorners15minsOU)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverNextCorner) < 0 ? '<font style="color:#f11">(' + n.TurnOverNextCorner + ')</font>' : (n.TurnOverNextCorner == 0 ? '' : n.TurnOverNextCorner)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1stTotalBookings) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stTotalBookings + ')</font>' : (n.TurnOver1stTotalBookings == 0 ? '' : n.TurnOver1stTotalBookings)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverTotalBookings) < 0 ? '<font style="color:#f11">(' + n.TurnOverTotalBookings + ')</font>' : (n.TurnOverTotalBookings == 0 ? '' : n.TurnOverTotalBookings)) + '</div></td>';
            row += '<td class="a2"><div class="col100 r">&nbsp;' + (parseFloat(n.TurnOverTotalBookings15minsOu) < 0 ? '<font style="color:#f11">(' + n.TurnOverTotalBookings15minsOu + ')</font>' : (n.TurnOverTotalBookings15minsOu == 0 ? '' : n.TurnOverTotalBookings15minsOu)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverNextBookings) < 0 ? '<font style="color:#f11">(' + n.TurnOverNextBookings + ')</font>' : (n.TurnOverNextBookings == 0 ? '' : n.TurnOverNextBookings)) + '</div></td>';
        }
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1stOe) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stOe + ')</font>' : (n.TurnOver1stOe == 0 ? '' : n.TurnOver1stOe)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverOe) < 0 ? '<font style="color:#f11">(' + n.TurnOverOe + ')</font>' : (n.TurnOverOe == 0 ? '' : n.TurnOverOe)) + '</div></td>';
        row += '<td class="a2"><div class="col100 r">&nbsp;' + (parseFloat(n.TurnOverOverall) < 0 ? '<font style="color:#f11">(' + n.TurnOverOverall + ')</font>' : (n.TurnOverOverall == 0 ? '' : n.TurnOverOverall)) + '</div></td>';

        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1st1x2) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1st1x2 + ')</font>' : (n.LiveBetCount1st1x2 == 0 ? '' : n.LiveBetCount1st1x2)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1x2) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1x2 + ')</font>' : (n.LiveBetCount1x2 == 0 ? '' : n.LiveBetCount1x2)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1stHdp) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stHdp + ')</font>' : (n.LiveBetCount1stHdp == 0 ? '' : n.LiveBetCount1stHdp)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountHdp) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountHdp + ')</font>' : (n.LiveBetCountHdp == 0 ? '' : n.LiveBetCountHdp)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1stOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stOu + ')</font>' : (n.LiveBetCount1stOu == 0 ? '' : n.LiveBetCount1stOu)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOu + ')</font>' : (n.LiveBetCountOu == 0 ? '' : n.LiveBetCountOu)) + '</div></td>';
        if (isSoccer == 1) {
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount15minOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount15minOu + ')</font>' : (n.LiveBetCount15minOu == 0 ? '' : n.LiveBetCount15minOu)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount10minsOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount10minsOu + ')</font>' : (n.LiveBetCount10minsOu == 0 ? '' : n.LiveBetCount10minsOu)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountSingleTeamOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountSingleTeamOu + ')</font>' : (n.LiveBetCountSingleTeamOu == 0 ? '' : n.LiveBetCountSingleTeamOu)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stNoOfCorners + ')</font>' : (n.LiveBetCount1stNoOfCorners == 0 ? '' : n.LiveBetCount1stNoOfCorners)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNoOfCorners + ')</font>' : (n.LiveBetCountNoOfCorners == 0 ? '' : n.LiveBetCountNoOfCorners)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountCorners15minsOU) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountCorners15minsOU + ')</font>' : (n.LiveBetCountCorners15minsOU == 0 ? '' : n.LiveBetCountCorners15minsOU)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountNextCorner) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNextCorner + ')</font>' : (n.LiveBetCountNextCorner == 0 ? '' : n.LiveBetCountNextCorner)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1stTotalBookings) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stTotalBookings + ')</font>' : (n.LiveBetCount1stTotalBookings == 0 ? '' : n.LiveBetCount1stTotalBookings)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountTotalBookings) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountTotalBookings + ')</font>' : (n.LiveBetCountTotalBookings == 0 ? '' : n.LiveBetCountTotalBookings)) + '</div></td>';
            row += '<td class="a6"><div class="col100 r">&nbsp;' + (parseFloat(n.LiveBetCountTotalBookings15minsOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountTotalBookings15minsOu + ')</font>' : (n.LiveBetCountTotalBookings15minsOu == 0 ? '' : n.LiveBetCountTotalBookings15minsOu)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountNextBookings) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNextBookings + ')</font>' : (n.LiveBetCountNextBookings == 0 ? '' : n.LiveBetCountNextBookings)) + '</div></td>';
        }
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1stOe) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stOe + ')</font>' : (n.LiveBetCount1stOe == 0 ? '' : n.LiveBetCount1stOe)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountOe) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOe + ')</font>' : (n.LiveBetCountOe == 0 ? '' : n.LiveBetCountOe)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountOverall) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOverall + ')</font>' : (n.LiveBetCountOverall == 0 ? '' : n.LiveBetCountOverall)) + '</div></td>';

        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1st1x2) < 0 ? '<font style="color:#f11">(' + n.Margin1st1x2 + '%)</font>' : (n.Margin1st1x2 == 0 ? '' : n.Margin1st1x2 + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1x2) < 0 ? '<font style="color:#f11">(' + n.Margin1x2 + '%)</font>' : (n.Margin1x2 == 0 ? '' : n.Margin1x2 + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1stHdp) < 0 ? '<font style="color:#f11">(' + n.Margin1stHdp + '%)</font>' : (n.Margin1stHdp == 0 ? '' : n.Margin1stHdp + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.MarginHdp) < 0 ? '<font style="color:#f11">(' + n.MarginHdp + '%)</font>' : (n.MarginHdp == 0 ? '' : n.MarginHdp + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1stOu) < 0 ? '<font style="color:#f11">(' + n.Margin1stOu + '%)</font>' : (n.Margin1stOu == 0 ? '' : n.Margin1stOu + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.MarginOu) < 0 ? '<font style="color:#f11">(' + n.MarginOu + '%)</font>' : (n.MarginOu == 0 ? '' : n.MarginOu + '%')) + '</div></td>';
        if (isSoccer == 1) {
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.Margin15minOu) < 0 ? '<font style="color:#f11">(' + n.Margin15minOu + '%)</font>' : (n.Margin15minOu == 0 ? '' : n.Margin15minOu + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.Margin10minsOu) < 0 ? '<font style="color:#f11">(' + n.Margin10minsOu + '%)</font>' : (n.Margin10minsOu == 0 ? '' : n.Margin10minsOu + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.MarginSingleTeamOu) < 0 ? '<font style="color:#f11">(' + n.MarginSingleTeamOu + '%)</font>' : (n.MarginSingleTeamOu == 0 ? '' : n.MarginSingleTeamOu + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.Margin1stNoOfCorners + '%)</font>' : (n.Margin1stNoOfCorners == 0 ? '' : n.Margin1stNoOfCorners + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.MarginNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.MarginNoOfCorners + '%)</font>' : (n.MarginNoOfCorners == 0 ? '' : n.MarginNoOfCorners + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.MarginCorners15minsOU) < 0 ? '<font style="color:#f11">(' + n.MarginCorners15minsOU + '%)</font>' : (n.MarginCorners15minsOU == 0 ? '' : n.MarginCorners15minsOU + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.MarginNextCorner) < 0 ? '<font style="color:#f11">(' + n.MarginNextCorner + '%)</font>' : (n.MarginNextCorner == 0 ? '' : n.MarginNextCorner + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1stTotalBookings) < 0 ? '<font style="color:#f11">(' + n.Margin1stTotalBookings + ')</font>' : (n.Margin1stTotalBookings == 0 ? '' : n.Margin1stTotalBookings + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.MarginTotalBookings) < 0 ? '<font style="color:#f11">(' + n.MarginTotalBookings + '%)</font>' : (n.MarginTotalBookings == 0 ? '' : n.MarginTotalBookings + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col100 r">&nbsp;' + (parseFloat(n.MarginTotalBookings15minsOu) < 0 ? '<font style="color:#f11">(' + n.MarginTotalBookings15minsOu + ')</font>' : (n.MarginTotalBookings15minsOu == 0 ? '' : n.MarginTotalBookings15minsOu + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.MarginNextBookings) < 0 ? '<font style="color:#f11">(' + n.MarginNextBookings + '%)</font>' : (n.MarginNextBookings == 0 ? '' : n.MarginNextBookings + '%')) + '</div></td>';
        }
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1stOe) < 0 ? '<font style="color:#f11">(' + n.Margin1stOe + '%)</font>' : (n.Margin1stOe == 0 ? '' : n.Margin1stOe + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.MarginOe) < 0 ? '<font style="color:#f11">(' + n.MarginOe + '%)</font>' : (n.MarginOe == 0 ? '' : n.MarginOe + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseFloat(n.MarginOverall) < 0 ? '<font style="color:#f11">(' + n.MarginOverall + '%)</font>' : (n.MarginOverall == 0 ? '' : n.MarginOverall + '%')) + '</div></td>';


        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1st1x2) < 0 ? '<font style="color:#f11">(' + n.Comm1st1x2 + ')</font>' : (n.Comm1st1x2 == 0 ? '' : n.Comm1st1x2)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1x2) < 0 ? '<font style="color:#f11">(' + n.Comm1x2 + ')</font>' : (n.Comm1x2 == 0 ? '' : n.Comm1x2)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1stHdp) < 0 ? '<font style="color:#f11">(' + n.Comm1stHdp + ')</font>' : (n.Comm1stHdp == 0 ? '' : n.Comm1stHdp)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.CommHdp) < 0 ? '<font style="color:#f11">(' + n.CommHdp + ')</font>' : (n.CommHdp == 0 ? '' : n.CommHdp)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1stOu) < 0 ? '<font style="color:#f11">(' + n.Comm1stOu + ')</font>' : (n.Comm1stOu == 0 ? '' : n.Comm1stOu)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.CommOu) < 0 ? '<font style="color:#f11">(' + n.CommOu + ')</font>' : (n.CommOu == 0 ? '' : n.CommOu)) + '</div></td>';
        if (isSoccer == 1) {
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.Comm15minOu) < 0 ? '<font style="color:#f11">(' + n.Comm15minOu + ')</font>' : (n.Comm15minOu == 0 ? '' : n.Comm15minOu)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.Comm10minsOu) < 0 ? '<font style="color:#f11">(' + n.Comm10minsOu + ')</font>' : (n.Comm10minsOu == 0 ? '' : n.Comm10minsOu)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.CommSingleTeamOu) < 0 ? '<font style="color:#f11">(' + n.CommSingleTeamOu + ')</font>' : (n.CommSingleTeamOu == 0 ? '' : n.CommSingleTeamOu)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.Comm1stNoOfCorners + ')</font>' : (n.Comm1stNoOfCorners == 0 ? '' : n.Comm1stNoOfCorners)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.CommNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.CommNoOfCorners + ')</font>' : (n.CommNoOfCorners == 0 ? '' : n.CommNoOfCorners)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.CommCorners15minsOU) < 0 ? '<font style="color:#f11">(' + n.CommCorners15minsOU + ')</font>' : (n.CommCorners15minsOU == 0 ? '' : n.CommCorners15minsOU)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.CommNextCorner) < 0 ? '<font style="color:#f11">(' + n.CommNextCorner + ')</font>' : (n.CommNextCorner == 0 ? '' : n.CommNextCorner)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1stTotalBookings) < 0 ? '<font style="color:#f11">(' + n.Comm1stTotalBookings + ')</font>' : (n.Comm1stTotalBookings == 0 ? '' : n.Comm1stTotalBookings)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.CommTotalBookings) < 0 ? '<font style="color:#f11">(' + n.CommTotalBookings + ')</font>' : (n.CommTotalBookings == 0 ? '' : n.CommTotalBookings)) + '</div></td>';
            row += '<td class="a4"><div class="col100 r">&nbsp;' + (parseFloat(n.CommTotalBookings15minsOu) < 0 ? '<font style="color:#f11">(' + n.CommTotalBookings15minsOu + ')</font>' : (n.CommTotalBookings15minsOu == 0 ? '' : n.CommTotalBookings15minsOu)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.CommNextBookings) < 0 ? '<font style="color:#f11">(' + n.CommNextBookings + ')</font>' : (n.CommNextBookings == 0 ? '' : n.CommNextBookings)) + '</div></td>';
        }
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1stOe) < 0 ? '<font style="color:#f11">(' + n.Comm1stOe + ')</font>' : (n.Comm1stOe == 0 ? '' : n.Comm1stOe)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.CommOe) < 0 ? '<font style="color:#f11">(' + n.CommOe + ')</font>' : (n.CommOe == 0 ? '' : n.CommOe)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseFloat(n.CommOverall) < 0 ? '<font style="color:#f11">(' + n.CommOverall + ')</font>' : (n.CommOverall == 0 ? '' : n.CommOverall)) + '</div></td>';

        row += '<td><div class="col100 l">' + n.UserName + '</div></td>';
        row += '</tr>';

        return row;
    },
    displayTraderPerformanceReportHeader: function (isSoccer) {
        var colspanOfEachType = 20;
        if (isSoccer == 0) {
            colspanOfEachType = 9;
        }

        var headerHtml = "";
        //FirstRow
        headerHtml += '<tr>';
        headerHtml += '<th rowspan="2"><div class="col30 c" style="height: 61px;">&nbsp</div></th>';
        headerHtml += '<th rowspan="2"><div class="col100 c" style="height: 61px;">&nbsp</div></th>';
        headerHtml += '<th rowspan="2"><div class="col40" style="height: 61px;">&nbsp</div></th>';
        headerHtml += '<th rowspan="2"><div class="col40" style="height: 61px;">&nbsp</div></th>';
        headerHtml += '<th rowspan="2"><div class="col40" style="height: 61px;">&nbsp</div></th>';
        headerHtml += '<th rowspan="2"><div class="col40" style="height: 61px;">&nbsp</div></th>';
        headerHtml += '<th rowspan="2"><div class="col40" style="height: 61px;">&nbsp</div></th>';
        headerHtml += '<th rowspan="2"><div class="col40" style="height: 61px;">&nbsp</div></th>';
        headerHtml += '<th colspan="' + colspanOfEachType + '"><div class="colHeaderSoccer c table-cell" style="height: 17px;">Winlost</div></th>';
        headerHtml += '<th colspan="' + colspanOfEachType + '"><div class="colHeaderSoccer c table-cell" style="height: 17px;">Turnover</div></th>';
        headerHtml += '<th colspan="' + colspanOfEachType + '"><div class="colHeaderSoccer c table-cell" style="height: 17px;">Live BetCount</div></th>';
        headerHtml += '<th colspan="' + colspanOfEachType + '"><div class="colHeaderSoccer c table-cell" style="height: 17px;">Margin %</div></th>';
        headerHtml += '<th colspan="' + colspanOfEachType + '"><div class="colHeaderSoccer c table-cell" style="height: 17px;">Commission</div></th>';
        headerHtml += '<th rowspan="2" align="center"><div class="col100 c sorter" fieldToSort="Trader">Trader</div></th>';
        headerHtml += '</tr>';

        //SecondRow
        headerHtml += '<tr>';
        //Winlost
        headerHtml += '<th><div class="col80 c">1st 1x2</div></th>';
        headerHtml += '<th><div class="col80 c">1x2</div></th>';
        headerHtml += '<th><div class="col80 c">1st HDP</div></th>';
        headerHtml += '<th><div class="col80 c">HDP</div></th>';
        headerHtml += '<th><div class="col80 c">1st O/U</div></th>';
        headerHtml += '<th><div class="col100 c">O/U</div></th>';
        if (isSoccer == 1) {
            headerHtml += '<th><div class="col80 c">15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">10mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Single Team O/U</div></th>';
            headerHtml += '<th><div class="col80 c">1st No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c">No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c">Corners 15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Next Corner</div></th>';
            headerHtml += '<th><div class="col80 c">1st Total Bookings</div></th>';
            headerHtml += '<th><div class="col80 c">Total Bookings</div></th>';
            headerHtml += '<th><div class="col100 c">Total Bookings 15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Next Booking</div></th>';
        }
        headerHtml += '<th><div class="col80 c">1st O/E</div></th>';
        headerHtml += '<th><div class="col80 c">O/E</div></th>';
        headerHtml += '<th><div class="col90 c sorter" fieldToSort="Win Loss">Overall</div></th>';
        //Turnover
        headerHtml += '<th><div class="col80 c">1st 1x2</div></th>';
        headerHtml += '<th><div class="col80 c">1x2</div></th>';
        headerHtml += '<th><div class="col80 c">1st HDP</div></th>';
        headerHtml += '<th><div class="col80 c">HDP</div></th>';
        headerHtml += '<th><div class="col80 c">1st O/U</div></th>';
        headerHtml += '<th><div class="col80 c">O/U</div></th>';
        if (isSoccer == 1) {
            headerHtml += '<th><div class="col80 c">15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">10mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Single Team O/U</div></th>';
            headerHtml += '<th><div class="col80 c">1st No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c">No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c">Corners 15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Next Corner</div></th>';
            headerHtml += '<th><div class="col80 c">1st Total Bookings</div></th>';
            headerHtml += '<th><div class="col80 c">Total Bookings</div></th>';
            headerHtml += '<th><div class="col100 c">Total Bookings 15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Next Booking</div></th>';
        }
        headerHtml += '<th><div class="col80 c">1st O/E</div></th>';
        headerHtml += '<th><div class="col80 c">O/E</div></th>';
        headerHtml += '<th><div class="col100 c sorter" fieldToSort="Turnover">Overall</div></th>';
        //Live BetCount
        headerHtml += '<th><div class="col80 c">1st 1x2</div></th>';
        headerHtml += '<th><div class="col80 c">1x2</div></th>';
        headerHtml += '<th><div class="col80 c">1st HDP</div></th>';
        headerHtml += '<th><div class="col80 c">HDP</div></th>';
        headerHtml += '<th><div class="col80 c">1st O/U</div></th>';
        headerHtml += '<th><div class="col80 c">O/U</div></th>';
        if (isSoccer == 1) {
            headerHtml += '<th><div class="col80 c">15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">10mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Single Team O/U</div></th>';
            headerHtml += '<th><div class="col80 c">1st No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c">No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c">Corners 15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Next Corner</div></th>';
            headerHtml += '<th><div class="col80 c">1st Total Bookings</div></th>';
            headerHtml += '<th><div class="col80 c">Total Bookings</div></th>';
            headerHtml += '<th><div class="col100 c">Total Bookings 15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Next Booking</div></th>';
        }
        headerHtml += '<th><div class="col80 c">1st O/E</div></th>';
        headerHtml += '<th><div class="col80 c">O/E</div></th>';
        headerHtml += '<th><div class="col80 c sorter" fieldToSort="Live Bet Count">Overall</div></th>';
        //Margin %
        headerHtml += '<th><div class="col80 c">1st 1x2</div></th>';
        headerHtml += '<th><div class="col80 c">1x2</div></th>';
        headerHtml += '<th><div class="col80 c">1st HDP</div></th>';
        headerHtml += '<th><div class="col80 c">HDP</div></th>';
        headerHtml += '<th><div class="col80 c">1st O/U</div></th>';
        headerHtml += '<th><div class="col80 c">O/U</div></th>';
        if (isSoccer == 1) {
            headerHtml += '<th><div class="col80 c">15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">10mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Single Team O/U</div></th>';
            headerHtml += '<th><div class="col80 c">1st No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c">No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c">Corners 15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Next Corner</div></th>';
            headerHtml += '<th><div class="col80 c">1st Total Bookings</div></th>';
            headerHtml += '<th><div class="col80 c">Total Bookings</div></th>';
            headerHtml += '<th><div class="col100 c">Total Bookings 15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Next Booking</div></th>';
        }
        headerHtml += '<th><div class="col80 c">1st O/E</div></th>';
        headerHtml += '<th><div class="col80 c">O/E</div></th>';
        headerHtml += '<th><div class="col80 c sorter" fieldToSort="Margin">Overall</div></th>';
        //Commission
        headerHtml += '<th><div class="col80 c">1st 1x2</div></th>';
        headerHtml += '<th><div class="col80 c">1x2</div></th>';
        headerHtml += '<th><div class="col80 c">1st HDP</div></th>';
        headerHtml += '<th><div class="col80 c">HDP</div></th>';
        headerHtml += '<th><div class="col80 c">1st O/U</div></th>';
        headerHtml += '<th><div class="col80 c">O/U</div></th>';
        if (isSoccer == 1) {
            headerHtml += '<th><div class="col80 c">15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">10mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Single Team O/U</div></th>';
            headerHtml += '<th><div class="col80 c">1st No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c">No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c">Corners 15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Next Corner</div></th>';
            headerHtml += '<th><div class="col80 c">1st Total Bookings</div></th>';
            headerHtml += '<th><div class="col80 c">Total Bookings</div></th>';
            headerHtml += '<th><div class="col100 c">Total Bookings 15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c">Next Booking</div></th>';
        }
        headerHtml += '<th><div class="col80 c">1st O/E</div></th>';
        headerHtml += '<th><div class="col80 c">O/E</div></th>';
        headerHtml += '<th><div class="col80 c sorter" fieldToSort="Comission">Overall</div></th>';

        headerHtml += '</tr>';
        return headerHtml;
    },
    renderTraderPerformanceReportFooterFrozen: function (n, isSoccer) {
        var row = "";
        var major = parseInt(n.Major) == 0 ? '&nbsp;' : n.Major;
        var minor1 = parseInt(n.Minor1) == 0 ? '&nbsp;' : n.Minor1;
        var minor2 = parseInt(n.Minor2) == 0 ? '&nbsp;' : n.Minor2;
        var minor3 = parseInt(n.Minor3) == 0 ? '&nbsp;' : n.Minor3;
        var others = parseInt(n.Others) == 0 ? '&nbsp;' : n.Others;
        row += '<tr>';
        row += '<th><div class="col30 c">&nbsp;</div></th>';
        row += '<th><div class="col100 l">Total</div></th>';
        row += '<th><div class="col40 r">' + n.MatchNo + '</div></th>';
        row += '<th><div class="col40 r">' + major + '</div></th>';
        row += '<th><div class="col40 r">' + minor1 + '</div></th>';
        row += '<th><div class="col40 r">' + minor2 + '</div></th>';
        row += '<th><div class="col40 r">' + minor3 + '</div></th>';
        row += '<th><div class="col40 r">' + others + '</div></th>';
        row += '</tr>';

        return row;
    },
    renderTraderPerformanceReportFooterScroll: function (n, isSoccer) {
        var row = "";
        row += '<tr>';

        row += '<th><div class="col30 c">&nbsp;</div></td>';
        row += '<th><div class="col100 l">Total</div></td>';
        row += '<th><div class="col40 r">&nbsp;</div></td>';
        row += '<th><div class="col40 r">&nbsp;</div></td>';
        row += '<th><div class="col40 r">&nbsp;</div></td>';
        row += '<th><div class="col40 r">&nbsp;</div></td>';
        row += '<th><div class="col40 r">&nbsp;</div></td>';
        row += '<th><div class="col40 r">&nbsp;</div></td>';

        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1st1x2) < 0 ? '<font style="color:#f11">(' + n.WinLost1st1x2 + ')</font>' : (n.WinLost1st1x2 == 0 ? '' : n.WinLost1st1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1x2) < 0 ? '<font style="color:#f11">(' + n.WinLost1x2 + ')</font>' : (n.WinLost1x2 == 0 ? '' : n.WinLost1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1stHdp) < 0 ? '<font style="color:#f11">(' + n.WinLost1stHdp + ')</font>' : (n.WinLost1stHdp == 0 ? '' : n.WinLost1stHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostHdp) < 0 ? '<font style="color:#f11">(' + n.WinLostHdp + ')</font>' : (n.WinLostHdp == 0 ? '' : n.WinLostHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1stOu) < 0 ? '<font style="color:#f11">(' + n.WinLost1stOu + ')</font>' : (n.WinLost1stOu == 0 ? '' : n.WinLost1stOu)) + '</div></th>';
        row += '<th><div class="col100 r">&nbsp;' + (parseFloat(n.WinLostOu) < 0 ? '<font style="color:#f11">(' + n.WinLostOu + ')</font>' : (n.WinLostOu == 0 ? '' : n.WinLostOu)) + '</div></th>';
        if (isSoccer == 1) {
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost15minOu) < 0 ? '<font style="color:#f11">(' + n.WinLost15minOu + ')</font>' : (n.WinLost15minOu == 0 ? '' : n.WinLost15minOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost10minsOu) < 0 ? '<font style="color:#f11">(' + n.WinLost10minsOu + ')</font>' : (n.WinLost10minsOu == 0 ? '' : n.WinLost10minsOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostSingleTeamOu) < 0 ? '<font style="color:#f11">(' + n.WinLostSingleTeamOu + ')</font>' : (n.WinLostSingleTeamOu == 0 ? '' : n.WinLostSingleTeamOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.WinLost1stNoOfCorners + ')</font>' : (n.WinLost1stNoOfCorners == 0 ? '' : n.WinLost1stNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.WinLostNoOfCorners + ')</font>' : (n.WinLostNoOfCorners == 0 ? '' : n.WinLostNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostCorners15minsOU) < 0 ? '<font style="color:#f11">(' + n.WinLostCorners15minsOU + ')</font>' : (n.WinLostCorners15minsOU == 0 ? '' : n.WinLostCorners15minsOU)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostNextCorner) < 0 ? '<font style="color:#f11">(' + n.WinLostNextCorner + ')</font>' : (n.WinLostNextCorner == 0 ? '' : n.WinLostNextCorner)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1stTotalBookings) < 0 ? '<font style="color:#f11">(' + n.WinLost1stTotalBookings + ')</font>' : (n.WinLost1stTotalBookings == 0 ? '' : n.WinLost1stTotalBookings)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostTotalBookings) < 0 ? '<font style="color:#f11">(' + n.WinLostTotalBookings + ')</font>' : (n.WinLostTotalBookings == 0 ? '' : n.WinLostTotalBookings)) + '</div></th>';
            row += '<th><div class="col100 r">&nbsp;' + (parseFloat(n.WinLostTotalBookings15minsOu) < 0 ? '<font style="color:#f11">(' + n.WinLostTotalBookings15minsOu + ')</font>' : (n.WinLostTotalBookings15minsOu == 0 ? '' : n.WinLostTotalBookings15minsOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostNextBookings) < 0 ? '<font style="color:#f11">(' + n.WinLostNextBookings + ')</font>' : (n.WinLostNextBookings == 0 ? '' : n.WinLostNextBookings)) + '</div></th>';
        }
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLost1stOe) < 0 ? '<font style="color:#f11">(' + n.WinLost1stOe + ')</font>' : (n.WinLost1stOe == 0 ? '' : n.WinLost1stOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.WinLostOe) < 0 ? '<font style="color:#f11">(' + n.WinLostOe + ')</font>' : (n.WinLostOe == 0 ? '' : n.WinLostOe)) + '</div></th>';
        row += '<th><div class="col90 r">&nbsp;' + (parseFloat(n.WinLostOverall) < 0 ? '<font style="color:#f11">(' + n.WinLostOverall + ')</font>' : (n.WinLostOverall == 0 ? '' : n.WinLostOverall)) + '</div></th>';

        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1st1x2) < 0 ? '<font style="color:#f11">(' + n.TurnOver1st1x2 + ')</font>' : (n.TurnOver1st1x2 == 0 ? '' : n.TurnOver1st1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1x2) < 0 ? '<font style="color:#f11">(' + n.TurnOver1x2 + ')</font>' : (n.TurnOver1x2 == 0 ? '' : n.TurnOver1x2)) + '</div></td>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1stHdp) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stHdp + ')</font>' : (n.TurnOver1stHdp == 0 ? '' : n.TurnOver1stHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverHdp) < 0 ? '<font style="color:#f11">(' + n.TurnOverHdp + ')</font>' : (n.TurnOverHdp == 0 ? '' : n.TurnOverHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1stOu) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stOu + ')</font>' : (n.TurnOver1stOu == 0 ? '' : n.TurnOver1stOu)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverOu) < 0 ? '<font style="color:#f11">(' + n.TurnOverOu + ')</font>' : (n.TurnOverOu == 0 ? '' : n.TurnOverOu)) + '</div></th>';
        if (isSoccer == 1) {
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver15minOu) < 0 ? '<font style="color:#f11">(' + n.TurnOver15minOu + ')</font>' : (n.TurnOver15minOu == 0 ? '' : n.TurnOver15minOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver10minsOu) < 0 ? '<font style="color:#f11">(' + n.TurnOver10minsOu + ')</font>' : (n.TurnOver10minsOu == 0 ? '' : n.TurnOver10minsOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverSingleTeamOu) < 0 ? '<font style="color:#f11">(' + n.TurnOverSingleTeamOu + ')</font>' : (n.TurnOverSingleTeamOu == 0 ? '' : n.TurnOverSingleTeamOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stNoOfCorners + ')</font>' : (n.TurnOver1stNoOfCorners == 0 ? '' : n.TurnOver1stNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.TurnOverNoOfCorners + ')</font>' : (n.TurnOverNoOfCorners == 0 ? '' : n.TurnOverNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverCorners15minsOU) < 0 ? '<font style="color:#f11">(' + n.TurnOverCorners15minsOU + ')</font>' : (n.TurnOverCorners15minsOU == 0 ? '' : n.TurnOverCorners15minsOU)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverNextCorner) < 0 ? '<font style="color:#f11">(' + n.TurnOverNextCorner + ')</font>' : (n.TurnOverNextCorner == 0 ? '' : n.TurnOverNextCorner)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1stTotalBookings) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stTotalBookings + ')</font>' : (n.TurnOver1stTotalBookings == 0 ? '' : n.TurnOver1stTotalBookings)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverTotalBookings) < 0 ? '<font style="color:#f11">(' + n.TurnOverTotalBookings + ')</font>' : (n.TurnOverTotalBookings == 0 ? '' : n.TurnOverTotalBookings)) + '</div></th>';
            row += '<th><div class="col100 r">&nbsp;' + (parseFloat(n.TurnOverTotalBookings15minsOu) < 0 ? '<font style="color:#f11">(' + n.TurnOverTotalBookings15minsOu + ')</font>' : (n.TurnOverTotalBookings15minsOu == 0 ? '' : n.TurnOverTotalBookings15minsOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverNextBookings) < 0 ? '<font style="color:#f11">(' + n.TurnOverNextBookings + ')</font>' : (n.TurnOverNextBookings == 0 ? '' : n.TurnOverNextBookings)) + '</div></th>';
        }
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOver1stOe) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stOe + ')</font>' : (n.TurnOver1stOe == 0 ? '' : n.TurnOver1stOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.TurnOverOe) < 0 ? '<font style="color:#f11">(' + n.TurnOverOe + ')</font>' : (n.TurnOverOe == 0 ? '' : n.TurnOverOe)) + '</div></th>';
        row += '<th><div class="col100 r">&nbsp;' + (parseFloat(n.TurnOverOverall) < 0 ? '<font style="color:#f11">(' + n.TurnOverOverall + ')</font>' : (n.TurnOverOverall == 0 ? '' : n.TurnOverOverall)) + '</div></th>';

        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1st1x2) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1st1x2 + ')</font>' : (n.LiveBetCount1st1x2 == 0 ? '' : n.LiveBetCount1st1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1x2) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1x2 + ')</font>' : (n.LiveBetCount1x2 == 0 ? '' : n.LiveBetCount1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1stHdp) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stHdp + ')</font>' : (n.LiveBetCount1stHdp == 0 ? '' : n.LiveBetCount1stHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountHdp) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountHdp + ')</font>' : (n.LiveBetCountHdp == 0 ? '' : n.LiveBetCountHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1stOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stOu + ')</font>' : (n.LiveBetCount1stOu == 0 ? '' : n.LiveBetCount1stOu)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOu + ')</font>' : (n.LiveBetCountOu == 0 ? '' : n.LiveBetCountOu)) + '</div></th>';
        if (isSoccer == 1) {
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount15minOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount15minOu + ')</font>' : (n.LiveBetCount15minOu == 0 ? '' : n.LiveBetCount15minOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount10minsOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount10minsOu + ')</font>' : (n.LiveBetCount10minsOu == 0 ? '' : n.LiveBetCount10minsOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountSingleTeamOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountSingleTeamOu + ')</font>' : (n.LiveBetCountSingleTeamOu == 0 ? '' : n.LiveBetCountSingleTeamOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stNoOfCorners + ')</font>' : (n.LiveBetCount1stNoOfCorners == 0 ? '' : n.LiveBetCount1stNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNoOfCorners + ')</font>' : (n.LiveBetCountNoOfCorners == 0 ? '' : n.LiveBetCountNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountCorners15minsOU) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountCorners15minsOU + ')</font>' : (n.LiveBetCountCorners15minsOU == 0 ? '' : n.LiveBetCountCorners15minsOU)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountNextCorner) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNextCorner + ')</font>' : (n.LiveBetCountNextCorner == 0 ? '' : n.LiveBetCountNextCorner)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1stTotalBookings) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stTotalBookings + ')</font>' : (n.LiveBetCount1stTotalBookings == 0 ? '' : n.LiveBetCount1stTotalBookings)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountTotalBookings) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountTotalBookings + ')</font>' : (n.LiveBetCountTotalBookings == 0 ? '' : n.LiveBetCountTotalBookings)) + '</div></th>';
            row += '<th><div class="col100 r">&nbsp;' + (parseFloat(n.LiveBetCountTotalBookings15minsOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountTotalBookings15minsOu + ')</font>' : (n.LiveBetCountTotalBookings15minsOu == 0 ? '' : n.LiveBetCountTotalBookings15minsOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountNextBookings) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNextBookings + ')</font>' : (n.LiveBetCountNextBookings == 0 ? '' : n.LiveBetCountNextBookings)) + '</div></th>';
        }
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCount1stOe) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stOe + ')</font>' : (n.LiveBetCount1stOe == 0 ? '' : n.LiveBetCount1stOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountOe) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOe + ')</font>' : (n.LiveBetCountOe == 0 ? '' : n.LiveBetCountOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.LiveBetCountOverall) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOverall + ')</font>' : (n.LiveBetCountOverall == 0 ? '' : n.LiveBetCountOverall)) + '</div></th>';

        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1st1x2) < 0 ? '<font style="color:#f11">(' + n.Margin1st1x2 + '%)</font>' : (n.Margin1st1x2 == 0 ? '' : n.Margin1st1x2 + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1x2) < 0 ? '<font style="color:#f11">(' + n.Margin1x2 + '%)</font>' : (n.Margin1x2 == 0 ? '' : n.Margin1x2 + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1stHdp) < 0 ? '<font style="color:#f11">(' + n.Margin1stHdp + '%)</font>' : (n.Margin1stHdp == 0 ? '' : n.Margin1stHdp + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.MarginHdp) < 0 ? '<font style="color:#f11">(' + n.MarginHdp + '%)</font>' : (n.MarginHdp == 0 ? '' : n.MarginHdp + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1stOu) < 0 ? '<font style="color:#f11">(' + n.Margin1stOu + '%)</font>' : (n.Margin1stOu == 0 ? '' : n.Margin1stOu + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.MarginOu) < 0 ? '<font style="color:#f11">(' + n.MarginOu + '%)</font>' : (n.MarginOu == 0 ? '' : n.MarginOu + '%')) + '</div></th>';
        if (isSoccer == 1) {
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Margin15minOu) < 0 ? '<font style="color:#f11">(' + n.Margin15minOu + '%)</font>' : (n.Margin15minOu == 0 ? '' : n.Margin15minOu + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Margin10minsOu) < 0 ? '<font style="color:#f11">(' + n.Margin10minsOu + '%)</font>' : (n.Margin10minsOu == 0 ? '' : n.Margin10minsOu + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.MarginSingleTeamOu) < 0 ? '<font style="color:#f11">(' + n.MarginSingleTeamOu + '%)</font>' : (n.MarginSingleTeamOu == 0 ? '' : n.MarginSingleTeamOu + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.Margin1stNoOfCorners + ')</font>' : (n.Margin1stNoOfCorners == 0 ? '' : n.Margin1stNoOfCorners + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.MarginNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.MarginNoOfCorners + '%)</font>' : (n.MarginNoOfCorners == 0 ? '' : n.MarginNoOfCorners + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.MarginCorners15minsOU) < 0 ? '<font style="color:#f11">(' + n.MarginCorners15minsOU + '%)</font>' : (n.MarginCorners15minsOU == 0 ? '' : n.MarginCorners15minsOU + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.MarginNextCorner) < 0 ? '<font style="color:#f11">(' + n.MarginNextCorner + '%)</font>' : (n.MarginNextCorner == 0 ? '' : n.MarginNextCorner + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1stTotalBookings) < 0 ? '<font style="color:#f11">(' + n.Margin1stTotalBookings + '%)</font>' : (n.Margin1stTotalBookings == 0 ? '' : n.Margin1stTotalBookings + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.MarginTotalBookings) < 0 ? '<font style="color:#f11">(' + n.MarginTotalBookings + '%)</font>' : (n.MarginTotalBookings == 0 ? '' : n.MarginTotalBookings + '%')) + '</div></th>';
            row += '<th><div class="col100 r">&nbsp;' + (parseFloat(n.MarginTotalBookings15minsOu) < 0 ? '<font style="color:#f11">(' + n.MarginTotalBookings15minsOu + ')</font>' : (n.MarginTotalBookings15minsOu == 0 ? '' : n.MarginTotalBookings15minsOu + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.MarginNextBookings) < 0 ? '<font style="color:#f11">(' + n.MarginNextBookings + '%)</font>' : (n.MarginNextBookings == 0 ? '' : n.MarginNextBookings + '%')) + '</div></th>';
        }
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Margin1stOe) < 0 ? '<font style="color:#f11">(' + n.Margin1stOe + '%)</font>' : (n.Margin1stOe == 0 ? '' : n.Margin1stOe + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.MarginOe) < 0 ? '<font style="color:#f11">(' + n.MarginOe + '%)</font>' : (n.MarginOe == 0 ? '' : n.MarginOe + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.MarginOverall) < 0 ? '<font style="color:#f11">(' + n.MarginOverall + '%)</font>' : (n.MarginOverall == 0 ? '' : n.MarginOverall + '%')) + '</div></th>';


        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1st1x2) < 0 ? '<font style="color:#f11">(' + n.Comm1st1x2 + ')</font>' : (n.Comm1st1x2 == 0 ? '' : n.Comm1st1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1x2) < 0 ? '<font style="color:#f11">(' + n.Comm1x2 + ')</font>' : (n.Comm1x2 == 0 ? '' : n.Comm1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1stHdp) < 0 ? '<font style="color:#f11">(' + n.Comm1stHdp + ')</font>' : (n.Comm1stHdp == 0 ? '' : n.Comm1stHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.CommHdp) < 0 ? '<font style="color:#f11">(' + n.CommHdp + ')</font>' : (n.CommHdp == 0 ? '' : n.CommHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1stOu) < 0 ? '<font style="color:#f11">(' + n.Comm1stOu + ')</font>' : (n.Comm1stOu == 0 ? '' : n.Comm1stOu)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.CommOu) < 0 ? '<font style="color:#f11">(' + n.CommOu + ')</font>' : (n.CommOu == 0 ? '' : n.CommOu)) + '</div></th>';
        if (isSoccer == 1) {
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Comm15minOu) < 0 ? '<font style="color:#f11">(' + n.Comm15minOu + ')</font>' : (n.Comm15minOu == 0 ? '' : n.Comm15minOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Comm10minsOu) < 0 ? '<font style="color:#f11">(' + n.Comm10minsOu + ')</font>' : (n.Comm10minsOu == 0 ? '' : n.Comm10minsOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.CommSingleTeamOu) < 0 ? '<font style="color:#f11">(' + n.CommSingleTeamOu + ')</font>' : (n.CommSingleTeamOu == 0 ? '' : n.CommSingleTeamOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.Comm1stNoOfCorners + ')</font>' : (n.Comm1stNoOfCorners == 0 ? '' : n.Comm1stNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.CommNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.CommNoOfCorners + ')</font>' : (n.CommNoOfCorners == 0 ? '' : n.CommNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.CommCorners15minsOU) < 0 ? '<font style="color:#f11">(' + n.CommCorners15minsOU + ')</font>' : (n.CommCorners15minsOU == 0 ? '' : n.CommCorners15minsOU)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.CommNextCorner) < 0 ? '<font style="color:#f11">(' + n.CommNextCorner + ')</font>' : (n.CommNextCorner == 0 ? '' : n.CommNextCorner)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1stTotalBookings) < 0 ? '<font style="color:#f11">(' + n.Comm1stTotalBookings + ')</font>' : (n.Comm1stTotalBookings == 0 ? '' : n.Comm1stTotalBookings)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.CommTotalBookings) < 0 ? '<font style="color:#f11">(' + n.CommTotalBookings + ')</font>' : (n.CommTotalBookings == 0 ? '' : n.CommTotalBookings)) + '</div></th>';
            row += '<th><div class="col100 r">&nbsp;' + (parseFloat(n.CommTotalBookings15minsOu) < 0 ? '<font style="color:#f11">(' + n.CommTotalBookings15minsOu + ')</font>' : (n.CommTotalBookings15minsOu == 0 ? '' : n.CommTotalBookings15minsOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.CommNextBookings) < 0 ? '<font style="color:#f11">(' + n.CommNextBookings + ')</font>' : (n.CommNextBookings == 0 ? '' : n.CommNextBookings)) + '</div></th>';
        }
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.Comm1stOe) < 0 ? '<font style="color:#f11">(' + n.Comm1stOe + ')</font>' : (n.Comm1stOe == 0 ? '' : n.Comm1stOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.CommOe) < 0 ? '<font style="color:#f11">(' + n.CommOe + ')</font>' : (n.CommOe == 0 ? '' : n.CommOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseFloat(n.CommOverall) < 0 ? '<font style="color:#f11">(' + n.CommOverall + ')</font>' : (n.CommOverall == 0 ? '' : n.CommOverall)) + '</div></th>';

        row += '<th><div class="col100 l">' + n.UserName + '</div></th>';

        row += '</tr>';

        return row;
    },
    loadTraderDetail: function () {
        var isSoccer = permissionManagement.isSoccer;
        var fromDate = $("#fdate").val();
        var toDate = $("#tdate").val();

        //var url = '../Report/TraderPerformance/TraderPerformanceDetail.aspx?fdate=' + fromDate + '&tdate=' + toDate + '&IsSoccer=' + isSoccer;
        //parent.gotoURL('centerFr', url);
        window.open('TraderPerformanceDetail.aspx?fdate=' + fromDate + '&tdate=' + toDate + '&IsSoccer=' + isSoccer, 'TraderPerformanceDetail');
    },
    export2Excel1: function () {
        var isSoccer = permissionManagement.isSoccer;
        var fromDate = $("#fdate").val();
        var toDate = $("#tdate").val();

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "../../Services/WCF/Report/TraderPerformanceService.svc/TraderPerformanceExport2Excel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"fromDate":"' + fromDate + '","toDate":"' + toDate + '","isSoccer":"' + isSoccer + '"}',
            success: function () {

            }
        });
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },
    export2Excel: function () {
        var isSoccer = permissionManagement.isSoccer;
        var fromDate = $("#fdate").val();
        var toDate = $("#tdate").val();
        var params = 'type=export&fdate=' + fromDate + '&tdate=' + toDate + '&issoccer=' + isSoccer;
        $('#frm').attr('src', 'TraderPerformance.aspx?' + params);

    },
    make2TableHeaderSameHeight: function (tableA, tableB) {
        var rowsA = tableA.find("thead tr");
        var rowsB = tableB.find("thead tr");
        var numberOfRows = (rowsA.length < rowsB.length) ? rowsA.length : rowsB.length;
        for (i = 0; i < numberOfRows; i++) {
            var currentRowA = $(rowsA[i]);
            var currentRowB = $(rowsB[i]);
            var cellsA = currentRowA.find("th");
            var cellsB = currentRowB.find("th");
            var cellsALength = (cellsA ? cellsA.length : 0);
            var cellsBLength = (cellsB ? cellsB.length : 0);
            var numberOfCells = (cellsALength < cellsBLength) ? cellsALength : cellsBLength;
            for (j = 0; j < numberOfCells; j++) {
                var currentHeightA = $(cellsA[j]).height();
                var currentHeightB = $(cellsB[j]).height();
                if (currentHeightA > currentHeightB && currentHeightA > 4) {
                    $(cellsB[j]).css("height", currentHeightA - 4 + "px").css("vertical-align", "middle");
                }
                else if (currentHeightA != currentHeightB) {
                    $(cellsA[j]).css("height", currentHeightB + 4 + "px").css("vertical-align", "middle");
                }
            }
        }
    },
    sorterClick: function (btn) {
        var oldSorter = traderPerformanceManager.cacheData.currentSorter;
        var fieldToSort = btn.attr("fieldToSort");
        if (oldSorter && oldSorter.attr("fieldToSort") != fieldToSort) {
            oldSorter.removeClass(traderPerformanceManager.enums.SortDirection.Asc)
                     .removeClass(traderPerformanceManager.enums.SortDirection.Desc);
        }

        traderPerformanceManager.cacheData.currentSorter = btn;
        var sortDirection = traderPerformanceManager.changeSorterStatus(btn);
        traderPerformanceManager.mappingSortInfo(sortDirection, fieldToSort);

        var report = traderPerformanceManager.cacheData.report;
        if (report && report && report.rows.length > 0) {
            report.rows.sort(function (elementA, elementB) {
                var valueA;
                var valueB;
                var isNumberSort = traderPerformanceManager.cacheData.sortDataType == traderPerformanceManager.enums.SortDataType.Number;
                if (isNumberSort) {
                    valueA = parseFloat(elementA[traderPerformanceManager.cacheData.sortField].replace(/\,/g, ''));
                    valueB = parseFloat(elementB[traderPerformanceManager.cacheData.sortField].replace(/\,/g, ''));
                } else {
                    valueA = elementA[traderPerformanceManager.cacheData.sortField];
                    valueB = elementB[traderPerformanceManager.cacheData.sortField];
                }

                if (valueA == valueB) {
                    return 0;
                }

                var lessThanFactor = (!isNumberSort && valueA < valueB) ||
                                     (isNumberSort && (valueA - valueB) < 0) ?
                                     -1 : 1;
                if (elementB["UserName"] == "(Unassigned)") {
                    return 1;
                }

                if (elementA["UserName"] == "(Unassigned)" ||
                    lessThanFactor * traderPerformanceManager.cacheData.sortFactor < 0) {
                    return -1;
                }

                return 1;
            });

            traderPerformanceManager.renderReport(report, true);
        }
    },
    changeSorterStatus: function (btn) {
        var sortDirection = "";
        var sortDirectionEnums = traderPerformanceManager.enums.SortDirection;
        if (btn.hasClass(sortDirectionEnums.Asc)) {
            sortDirection = sortDirectionEnums.Desc;
            btn.removeClass(sortDirectionEnums.Asc).addClass(sortDirection);
        } else if (btn.hasClass(sortDirectionEnums.Desc)) {
            sortDirection = sortDirectionEnums.Asc;
            btn.removeClass(sortDirectionEnums.Desc).addClass(sortDirection);
        } else {
            sortDirection = sortDirectionEnums.Asc;
            btn.addClass(sortDirection);
        }

        return sortDirection;
    },
    resetSorter: function () {
        traderPerformanceManager.cacheData.sorterList.removeClass("asc").removeClass("desc");
    },
    displaySorter: function (rs) {
        traderPerformanceManager.cacheData.report = rs;
        traderPerformanceManager.cacheData.sorterList = traderPerformanceManager.traderPerformanceReport.find(".sorter");
        if (rs && rs.rows != null && rs.rows.length > 0) {
            traderPerformanceManager.cacheData.sorterList.removeClass("disableSorter");
            traderPerformanceManager.cacheData.sorterList.each(function () {
                $(this).attr("title",
                        "Sort by " + $(this).attr("fieldToSort"))
            });

            traderPerformanceManager.cacheData.sorterList.unbind("click");
            traderPerformanceManager.cacheData.sorterList.click(function () {
                traderPerformanceManager.sorterClick($(this));
            });
        } else {
            traderPerformanceManager.cacheData.sorterList.addClass("disableSorter");
        }
    },
    mappingSortInfo: function (sortDirection, fieldToSort) {
        var sortDirectionEnums = traderPerformanceManager.enums.SortDirection;
        switch (sortDirection) {
            case sortDirectionEnums.Asc:
                traderPerformanceManager.cacheData.sortFactor = 1;
                break;
            case sortDirectionEnums.Desc:
                traderPerformanceManager.cacheData.sortFactor = -1;
                break;
            default:
                traderPerformanceManager.cacheData.sortFactor = 0;
                break;
        }

        switch (fieldToSort) {
            case "Trader":
                traderPerformanceManager.cacheData.sortField = "UserName";
                traderPerformanceManager.cacheData.sortDataType = traderPerformanceManager.enums.SortDataType.String;
                break;
            case "Win Loss":
                traderPerformanceManager.cacheData.sortField = "WinLostOverall";
                traderPerformanceManager.cacheData.sortDataType = traderPerformanceManager.enums.SortDataType.Number;
                break;
            case "Turnover":
                traderPerformanceManager.cacheData.sortField = "TurnOverOverall";
                traderPerformanceManager.cacheData.sortDataType = traderPerformanceManager.enums.SortDataType.Number;
                break;
            case "Live Bet Count":
                traderPerformanceManager.cacheData.sortField = "LiveBetCountOverall";
                traderPerformanceManager.cacheData.sortDataType = traderPerformanceManager.enums.SortDataType.Number;
                break;
            case "Margin":
                traderPerformanceManager.cacheData.sortField = "MarginOverall";
                traderPerformanceManager.cacheData.sortDataType = traderPerformanceManager.enums.SortDataType.Number;
                break;
            case "Comission":
                traderPerformanceManager.cacheData.sortField = "CommOverall";
                traderPerformanceManager.cacheData.sortDataType = traderPerformanceManager.enums.SortDataType.Number;
                break;
            default:
                traderPerformanceManager.cacheData.sortField = null;
                traderPerformanceManager.cacheData.sortDataType = null;
                break;
        }
    }
}
