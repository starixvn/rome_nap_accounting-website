


var virtualSportsReportManager = {
    reportDate: null,
    sportTypeId: null,
    sportTypeName: null,
    bettype: null,
    bettypename: null,
    path: "Virtual Sports",
    backToMainMenuText: "Back to Main Menu",
    isFirstLoad: true,
    title: "",
    noneLiveReportTitle: "",
    liveReportTitle: "",
    reportDetailTitle: "",
    reportDetailPath: "",
    sportTypeIdLive: null,
    sportTypeNameLive: null,
    sportTypeIdNoneLive: null,
    sportTypeNameNoneLive: null,
    initGrid: function (sportId, wldate) {
        virtualSportsReportManager.sportTypeId = sportId;
        virtualSportsReportManager.reportDate = wldate;
        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");
        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            virtualSportsReportManager.viewReport(reportDate);
        });
        $("#btnExport").click(function () {
            virtualSportsReportManager.exportToExcel("1");
        });
        $('#spMoreElement').empty();
        var strtoday = '<input type="button" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);
        $("#btnToday").click(function () {
            virtualSportsReportManager.viewReportByDate(1);
        });
        $("#btnYesterday").click(function () {
            virtualSportsReportManager.viewReportByDate(2);
        });

        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromVSR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + virtualSportsReportManager.path + "</b>";

        $('#btnBackMainVirtualSportNonLive').click(function () {
            WL2hShowHideDivManager.showMainReportFromVSR(sportId, wldate);
        });

        $("#dailyWLReportDiv").removeClass("hidden");

        try { parent.jQuery.unblockUI(); } catch (e) { }
        virtualSportsReportManager.viewReport(wldate);
    },
    viewReport: function (wldate) {
        virtualSportsReportManager.getcontent(wldate);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                virtualSportsReportManager.getcontent($('#btnToday').val());
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                virtualSportsReportManager.getcontent($('#btnYesterday').val());
                break;
            default:
                break;
        }
    },
    getcontent: function (wldate) {
        if (!isAllowReadVirtualSportsReport) {
            return;
        }

        virtualSportsReportManager.reportDate = wldate;
        var data = {
            param:
            {
                ReportDate: wldate
            }
        };
        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "VirtualSport2hReport/SelVirtualSportsReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }

                virtualSportsReportManager.renderDailyWLReport(rs, wldate);
                virtualSportsReportManager.showMainReport(true);
                $("#leftPaneVSR").removeClass("hidden");
                $("#virtualSportsReport").removeClass("hidden");
            },
            error: function () {
                $("#nonliveDailyWLVirtualSports").find("tr:not(.header)").remove();
                $("#nonliveDailyWLVirtualSports").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
                virtualSportsReportManager.showNonLiveReport(true);
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        if (dailyWLResult) {
            virtualSportsReportManager.showOtherSportsBettypeReport(false);
            isBARMActive = dailyWLResult.NoneLiveReport.IsBARMActive.toString();
            virtualSportsReportManager.renderNoneLiveReportDWL2h(dailyWLResult.NoneLiveReport, wldate);
        }
    },
    renderNoneLiveReportDWL2h: function (noneLiveReport, wldate) {
        if (noneLiveReport && noneLiveReport.ReportDetails != null && noneLiveReport.ReportDetails.length > 0) {
            var arrBody = [];
            var items = noneLiveReport.ReportDetails;
            var totalSportReport = items.length;

            for (var i = 0; i < totalSportReport; i++) {
                arrBody.push(virtualSportsReportManager.renderDailyWLRow(items[i], (i % 2 == 0 ? "evenrow" : "")));
            }
            arrBody.push('<tr><td colspan="7" style="height: 18px;"></td></tr>');
            arrBody.push(virtualSportsReportManager.renderDailyWLTotal(noneLiveReport.Total.NormalTotal));

            var allIBCTotal = noneLiveReport.Total.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += noneLiveReport.Total.BARMTotal.WinLoss;
            }
            if (allIBCTotal != 0) {
                arrBody.push(virtualSportsReportManager.renderIBCTotalRow_LeftReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = stringformat.removeSpaceIE9(bodyHtml);

            $("#nonliveDailyWLVirtualSports").find("tr:not(.header)").remove();
            $("#nonliveDailyWLVirtualSports").append(bodyHtml);

        }
        else {
            $("#nonliveDailyWLVirtualSports").find("tr:not(.header)").remove();
            $("#nonliveDailyWLVirtualSports").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
        }
        virtualSportsReportManager.showNonLiveReport(true);
    },
    renderDailyWLRow: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                        <td class="l"><span class="blue b" onclick="virtualSportsReportManager.getDailyWLBySport(\'' + row.Id + '\',\'' + row.Name + '\',\'' + row.IsLive + '\');">' + row.Name + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="r"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r"><span class="' + '' + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + (row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0 ? '' : 'red') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    renderDailyWLTotal: function (row) {
        var html = '';
        html = '<tr class="">\
                        <td class="r b"><span>' + row.Name + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="tdtitle r b" bgcolor="#dddddd"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r b"><span class="' + '' + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) < 0) ? '' : 'red') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    renderIBCTotalRow_LeftReport: function (allIBCTotal) {
        var html = '';
        var ibcMsg = '';
        if (isEightRMActive == 'true' && isBARMActive == 'true') {
            ibcMsg = "IBC(" + eightRmStr + " + " + barmStr + ") Total:";
        }
        else if (isEightRMActive == 'true') {
            ibcMsg = "IBC(" + eightRmStr + ") Total:";
        }
        else if (isBARMActive == 'true') {
            ibcMsg = "IBC(" + barmStr + ") Total:";
        }

        html = '<tr class="">\
                        <td class="r b"><span>' + ibcMsg + '</span></td>\
                        <td class="r b"><span class="' + (allIBCTotal < 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(allIBCTotal) + '</span></td>\
                        <td colspan="5"></td>\
                </tr>';
        return html;
    },
    getDailyWLBySport: function (sporttype, sportname, isLive, reportDate) {
        if (!isAllowReadVirtualSportsReport) {
            return;
        }
        virtualSportsReportManager.sportTypeIdBySport = sporttype;
        virtualSportsReportManager.sportTypeId = sporttype;
        if (reportDate == null || reportDate == undefined) {
            virtualSportsReportManager.reportDateBySport = virtualSportsReportManager.reportDate;
        }
        else {
            virtualSportsReportManager.reportDateBySport = reportDate;
        }

        var data = {
            param:
            {
                ReportDate: virtualSportsReportManager.reportDateBySport,
                SportType: sporttype
            }
        };
        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        virtualSportsReportManager.showOtherSportsBettypeReport(false);
        virtualSportsReportManager.sportTypeIdNoneLive = sporttype;
        virtualSportsReportManager.sportTypeNameNoneLive = sportname;
        $.ajax({
            type: "POST",
            url: window.rootUrl + "VirtualSport2hReport/SelVirtualSportsReportBySport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                virtualSportsReportManager.renderDailyWLReportBySport(rs, sporttype, sportname);
            },
            error: function () {
                $("#nonliveDailyWLVirtualSportsBySport").find("tr:not(.header)").remove();
                $("#nonliveDailyWLVirtualSportsBySport").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');

            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReportBySport: function (dailyWLResult, sporttype, sportname) {
        if (dailyWLResult) {

            $("#btnReport").unbind("click");
            $("#btnExport").unbind("click");
            $("#btnToday").unbind("click");
            $("#btnYesterday").unbind("click");

            $("#btnReport").click(function () {
                var reportDate = $("#txtfromdate").val();
                virtualSportsReportManager.getDailyWLBySport(sporttype, sportname, null, reportDate);
            });
            $("#btnExport").click(function () {
                virtualSportsReportManager.exportToExcel("5");
            });
            $("#btnToday").click(function () {
                virtualSportsReportManager.getDailyWLBySport(sporttype, sportname, null, new Date($('#btnToday').val()));
            });
            $("#btnYesterday").click(function () {
                virtualSportsReportManager.getDailyWLBySport(sporttype, sportname, null, new Date($('#btnYesterday').val()));
            });

            var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromVSR('" + virtualSportsReportManager.sportTypeId +
                         "','" + virtualSportsReportManager.reportDate + "')\"> Main </a> > " +
                         "<a onclick=\"virtualSportsReportManager.showNonLiveReport(true);\"> " + virtualSportsReportManager.path + " </a> > " + sportname + "</b>";
            $('#btnBackMainReportVirtualSportNonLive').unbind("click");
            $('#btnBackMainReportVirtualSportNonLive').click(function () {
                virtualSportsReportManager.showNonLiveReport(true);
            });

            virtualSportsReportManager.sportTypeIdNoneLive = sporttype;
            virtualSportsReportManager.sportTypeIdLive = sporttype;

            virtualSportsReportManager.sportTypeNameNoneLive = sportname;
            virtualSportsReportManager.sportTypeNameLive = sportname;

            isBARMActive = dailyWLResult.NoneLiveReport.IsBARMActive.toString();
            virtualSportsReportManager.renderNoneLiveReportBySportDWL2h(dailyWLResult.NoneLiveReport, sporttype, sportname);

            virtualSportsReportManager.showNonLiveReport(false);
        }
    },
    renderNoneLiveReportBySportDWL2h: function (noneLiveReport, sporttype, sportname) {
        virtualSportsReportManager.noneLiveReportTitle = "VIRTUAL SPORTS|" + sportname.toUpperCase() + ' PRODUCT WIN\/LOSS REPORT';
        virtualSportsReportManager.title = virtualSportsReportManager.noneLiveReportTitle;

        $("#dailyWLVirtualSports_NonLiveTitleBySport").html(virtualSportsReportManager.noneLiveReportTitle);
        if (noneLiveReport != null && noneLiveReport.ReportDetails &&
            noneLiveReport.ReportDetails.length > 0) {

            var arrBody = [];
            var items = noneLiveReport.ReportDetails;
            var totalSportReport = items.length;
            for (var i = 0; i < totalSportReport; i++) {
                arrBody.push(virtualSportsReportManager.renderDailyWLRowBySport(items[i], (i % 2 == 0 ? "evenrow" : "")));
            }
            arrBody.push('<tr><td colspan="7" style="height: 18px;"></td></tr>');
            arrBody.push(virtualSportsReportManager.renderDailyWLTotalBySport(noneLiveReport.Total.NormalTotal));

            var allIBCTotal = noneLiveReport.Total.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += noneLiveReport.Total.BARMTotal.WinLoss;
            }
            if (allIBCTotal) {
                arrBody.push(virtualSportsReportManager.renderIBCTotalRowBySport_LeftReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = stringformat.removeSpaceIE9(bodyHtml);

            $("#nonliveDailyWLVirtualSportsBySport").find("tr:not(.header)").remove();
            $("#nonliveDailyWLVirtualSportsBySport").append(bodyHtml);

        }
        else {
            $("#nonliveDailyWLVirtualSportsBySport").find("tr:not(.header)").remove();
            $("#nonliveDailyWLVirtualSportsBySport").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
        }
    },
    renderDailyWLRowBySport: function (row, cssClass) {
        var html = '';
        if (virtualSportsReportManager.sportTypeId == 181) {
            html = '<tr class="' + cssClass + '">\
                        <td class="l"><span class="blue b" onclick="virtualSportsReportManager.getDailyWLBySportBettype_VirtualHR(\'' + row.Id + '\',\'' + row.Name + '\',\'181\');">' + row.AliasName + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="r"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r"><span class="' + '' + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + (row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0 ? '' : 'red') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        }
        else {
            html = '<tr class="' + cssClass + '">\
                        <td class="l"><span class="blue b" onclick="virtualSportsReportManager.getDailyWLBySportBettype(\'' + row.Id + '\',\'' + row.Name + '\',\'' + row.IsLive + '\',\'' + row.Hedge + '\');">' + row.AliasName + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="r"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r"><span class="' + '' + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + (row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0 ? '' : 'red') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        }

        return html;
    },
    renderDailyWLTotalBySport: function (row) {
        var html = '';
        html = '<tr class="">\
                        <td class="r b"><span>' + "All Total:" + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="tdtitle r b" bgcolor="#dddddd"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r b"><span class="' + '' + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0) ? '' : 'red') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    renderIBCTotalRowBySport_LeftReport: function (allIBCTotal) {
        var html = '';
        var ibcMsg = '';
        if (isEightRMActive == 'true' && isBARMActive == 'true') {
            ibcMsg = "IBC(" + eightRmStr + " + " + barmStr + ") Total:";
        }
        else if (isEightRMActive == 'true') {
            ibcMsg = "IBC(" + eightRmStr + ") Total:";
        }
        else if (isBARMActive == 'true') {
            ibcMsg = "IBC(" + barmStr + ") Total:";
        }

        html = '<tr class="">\
                        <td class="r b"><span>' + ibcMsg + '</span></td>\
                        <td class="r b"><span class="' + (allIBCTotal.toFixed(2) < 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(allIBCTotal) + '</span></td>\
                        <td colspan="5"></td>\
                </tr>';
        return html;
    },
    getDailyWLBySportBettype: function (bettype, bettypename, isLive, hedge) {
        if (!isAllowReadVirtualSportsReport) {
            return;
        }
        var sporttype = "";
        var sporttypeName = "";
        if (isLive == 'true') {
            sporttype = virtualSportsReportManager.sportTypeIdLive;
            sporttypeName = virtualSportsReportManager.sportTypeNameLive;
        }
        else {
            sporttype = virtualSportsReportManager.sportTypeIdNoneLive;
            sporttypeName = virtualSportsReportManager.sportTypeNameNoneLive;
        }

        var isHedge = (hedge && hedge.indexOf("(hedge)") != -1);
        var data = {
            param:
            {
                ReportDate: virtualSportsReportManager.reportDateBySport,
                SportType: sporttype,
                BetType: bettype,
                IsLive: (isLive == 'true' ? 1 : 0),
                IsHedge: (isHedge ? true : false)
            }
        };

        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        virtualSportsReportManager.bettype = bettype;
        virtualSportsReportManager.bettypeName = bettypename;
        virtualSportsReportManager.isLive = (isLive == 'true' ? 1 : 0);
        virtualSportsReportManager.isHedge = isHedge;

        $.ajax({
            type: "POST",
            url: window.rootUrl + "VirtualSport2hReport/SelDailyWLDirectDetails",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                virtualSportsReportManager.renderDailyWLReportSportBettype(rs, sporttypeName, bettypename, isLive, isHedge);

            },
            error: function () {
                $("#tblContent_RDtVsp").find("tr:not(.header)").remove();
                $("#tblContent_RDtVsp").append('<tr><td colspan="10" class="b c"><span style="">There is no information available.</span></td></tr>');
                virtualSportsReportManager.showOtherSportsBettypeReport(true);
                $("#gridbettypeVSR").removeClass('hidden');
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReportSportBettype: function (dailyWLResult, sporttypeName, bettypename, isLive, isHedge) {
        $("#tblContent_RDtVsp_VirtualHR").addClass("hidden");
        $("#tblContent_RDtVsp").removeClass('hidden');
        if (dailyWLResult) {
            isBARMActive = dailyWLResult.IsBARMActive.toString();
            var detailReportTitle = "Main>Virtual Sports|" + sporttypeName + ">" + bettypename;
            $("#gridtitlebettype_VSR").html(detailReportTitle);
            if (dailyWLResult.ReportDetails != null && dailyWLResult.ReportDetails.length > 0) {
                var items = dailyWLResult.ReportDetails;
                var arrBody = [];
                var arrFooter = [];
                var numberItems = items.length;

                for (var i = 0; i < numberItems; i++) {
                    arrBody.push(virtualSportsReportManager.renderDailyWLRowSportDetails(items[i], i));
                }

                arrFooter.push(virtualSportsReportManager.renderTotalRow_RightReport(dailyWLResult.Total.NormalTotal, isLive, bettypename, isHedge));

                var allIBCTotal = dailyWLResult.Total.EightRMTotal.WinLoss;
                if (isBARMActive == 'true') {
                    allIBCTotal += dailyWLResult.Total.BARMTotal.WinLoss;
                }

                if (allIBCTotal != 0) {
                    arrFooter.push(virtualSportsReportManager.renderIBCTotalRow_RightReport(allIBCTotal));
                }

                var bodyHtml = arrBody.join('');
                bodyHtml = stringformat.removeSpaceIE9(bodyHtml);
                $("#tblContent_RDtVsp").find("tr:not(.header)").remove();
                $("#tblContent_RDtVsp").append(bodyHtml);

                var footerHtml = arrFooter.join('');
                footerHtml = stringformat.removeSpaceIE9(footerHtml);
                $("#tblContent_RDtVsp").append(footerHtml);

            }
            else {
                $("#tblContent_RDtVsp").find("tr:not(.header)").remove();
                $("#tblContent_RDtVsp").append('<tr><td colspan="10" class="b c"><span style="">There is no information available.</span></td></tr>');
            }

            virtualSportsReportManager.showOtherSportsBettypeReport(true);
            $("#gridbettypeVSR").removeClass('hidden');
        }
    },
    renderDailyWLRowSportDetails: function (dwlRow, index) {
        var html = "";
        switch (dwlRow.RowType.Id) {
            case 0:
                html = virtualSportsReportManager.renderMainRow_RightReport(dwlRow, index);
                break;
            case 3:
                html = virtualSportsReportManager.renderBettypeTotalRow_RightReport(dwlRow);
                break;
            case 9:
                html = virtualSportsReportManager.emptyRowHtml_RightReport(dwlRow);
                break;
            case 10:
                html = virtualSportsReportManager.renderLeagueHeadLine(dwlRow);
                break;
        }
        return html;
    },
    renderLeagueHeadLine: function (dwlRow) {
        var html = '';
        html = '<tr>\
                    <td>\
                        <span style=""></span></td>\
                    <td colspan = "9" class="l" style=""><span class="b longtext-wrapper">' + dwlRow.Name + '</span></td>\
                </tr>';
        return html;
    },
    renderMainRow_RightReport: function (row, index) {
        var html = '';
        var eventDetail = "";
        if (virtualSportsReportManager.bettype == 10 || virtualSportsReportManager.sportTypeId == 201) {
            eventDetail = '<td colspan = "3" style="max-width: 300px;"><span class="c longtext-wrapper" style="">' + row.MatchInfo.HomeTeam.Name + (row.MatchInfo.IsNeutral ? " (N)" : "") + '</span></td>';
        }
        else {
            eventDetail = '<td class="r" style="max-width: 300px;"><span class="longtext-wrapper" style="">' + row.MatchInfo.HomeTeam.Name + (row.MatchInfo.IsNeutral ? " (N)" : "") + '</span></td>\
                        <td class="c" style="width: 25px;">-vs-</td>\
                        <td class="l" style="max-width: 300px;"><span class="longtext-wrapper" style="">' + row.MatchInfo.AwayTeam.Name + '</span></td>';
        }

        html = '<tr class="' + (row.RowIndex % 2 == 0 ? "evenrow" : "") + '">\
                    <td style="word-wrap: break-word;"><span class="c" style="">' + row.MatchInfo.Name + '</span></td>'
                   + eventDetail + '<td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                    <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                    <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                    <td class="r"><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                    <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                    <td class="r"><span class=" ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
                </tr>';
        return html;
    },
    renderBettypeTotalRow_RightReport: function (row) {
        var html = '';
        html = '<tr>\
                <td class="r tdtitle" bgcolor="#888888" colspan = "4"><span class="b" style=""></span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r" style=""><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
            </tr>';
        return html;
    },
    renderTotalRow_RightReport: function (row, isLive, bettypeName, isHedge) {
        var html = '';
        var totalTitle = (isLive == "true" ? "<font color='red'>Live!</font>" : "") + " " +
                        bettypeName + (isHedge ? " (Hegde)" : "") + " Total:";
        html = '<tr>\
                <td class="r" colspan = "4"><div class="b longtext-wrapper" style="">' + totalTitle + '</span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r" style=""><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
            </tr>';
        return html;
    },
    renderIBCTotalRow_RightReport: function (ibcValue) {
        var ibcMsg = '';
        if (isEightRMActive == 'true' && isBARMActive == 'true') {
            ibcMsg = "IBC(" + eightRmStr + " + " + barmStr + ") Total:";
        }
        else if (isEightRMActive == 'true') {
            ibcMsg = "IBC(" + eightRmStr + ") Total:";
        }
        else if (isBARMActive == 'true') {
            ibcMsg = "IBC(" + barmStr + ") Total:";
        }

        var html = '<tr>\
                        <td class="r" colspan = "4"><span class="b" style="">' +
                                                  ibcMsg + '</span></td>\
                        <td class="r"><span class="b ' + (ibcValue.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(ibcValue) + '</span></td>\
                        <td colspan ="5" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    showNonLiveReport: function (isshow) {
        if (isshow) {
            $("#nonliveDailyWLVirtualSportsBySport").addClass("hidden");
            $("#nonliveDailyWLVirtualSports").removeClass("hidden");
            virtualSportsReportManager.changeAction2Sporttype(isshow);
        }
        else {
            $("#nonliveDailyWLVirtualSports").addClass("hidden");
            $("#nonliveDailyWLVirtualSportsBySport").removeClass("hidden");
        }
        virtualSportsReportManager.showOtherSportsBettypeReport(false);
    },
    changeAction2Sporttype: function (isChange) {
        if (isChange) {
            $("#txtfromdate").datepicker("setDate", virtualSportsReportManager.reportDate);

            var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromVSR('" + virtualSportsReportManager.sportTypeId +
                         "','" + virtualSportsReportManager.reportDate + "')\"> Main </a> > " + virtualSportsReportManager.path + "</b>";
            $("#btnReport").unbind("click");
            $("#btnExport").unbind("click");
            $("#btnToday").unbind("click");
            $("#btnYesterday").unbind("click");

            $("#btnReport").click(function () {
                var reportDate = $("#txtfromdate").val();
                virtualSportsReportManager.viewReport(reportDate);
            });
            $("#btnExport").click(function () {
                virtualSportsReportManager.exportToExcel("1");
            });
            $("#btnToday").click(function () {
                virtualSportsReportManager.viewReportByDate(1);
            });
            $("#btnYesterday").click(function () {
                virtualSportsReportManager.viewReportByDate(2);
            });
        }
    },
    showMainReport: function (isshow) {
        if (isshow) {
            virtualSportsReportManager.showOtherSportsBettypeReport(false);
        }
    },
    showOtherSportsBettypeReport: function (isshow) {
        var pagewidth = $(window).width();
        if (isshow) {
            $("#gridSplitter_VSR").removeClass('hidden');
            $("#gridbettypeVSR").removeClass("hidden");
            $("#virtualSportsReport").css("width", "1500px");
            var totalWidth = $("#virtualSportsReportTable").width() + 10;
            $("#virtualSportsReport").css("width", totalWidth + "px");
            var mainReportWidth = $(".mainreport").width();
            if (mainReportWidth < totalWidth) {
                $(".mainreport").css("width", totalWidth + "px");
            }
            else if (totalWidth <= pagewidth) {
                $(".mainreport").css("width", pagewidth + "px");
            }
        }
        else {
            $("#virtualSportsReport").css("width", "100%");
            $("#gridSplitter_VSR").addClass('hidden');
            $("#gridbettypeVSR").addClass("hidden");
            $(".mainreport").css("width", pagewidth + "px");
        }
    },
    hideAllReport: function () {
        $("#virtualSportsReport").addClass("hidden");
        virtualSportsReportManager.showOtherSportsBettypeReport(false);
        $("#nonliveDailyWLVirtualSportsBySport").addClass("hidden");
        $("#nonliveDailyWLVirtualSports").addClass("hidden");
    },
    backToMain: function () {
        WL2hShowHideDivManager.showMainReportFromVSR(virtualSportsReportManager.sportTypeId, virtualSportsReportManager.reportDate);
    },
    exportToExcel: function (reportType) {
        if (!isAllowReadVirtualSportsReport) {
            return;
        }
        var url = "DailyWinLoss2h?";
        switch (reportType) {
            case "1": // VirtualSportsMain
                url += "&ExportType=VirtualSportsMain";
                url += "&ReportDate=" + virtualSportsReportManager.reportDate;
                break;
            case "2": // VirtualSportsMain_NoneLive
                url += "&ExportType=VirtualSportsMain_NoneLive";
                url += "&ReportDate=" + virtualSportsReportManager.reportDate;
                break;
            case "3": // VirtualSportBySport_NoneLive
                url += "&ExportType=VirtualSportsBySport_NoneLive";
                url += "&ReportDate=" + virtualSportsReportManager.reportDateBySport;
                url += "&SportType=" + virtualSportsReportManager.sportTypeIdBySport;
                break;
            case "4": // VirtualSportsBySportBettypeDetails
                if (virtualSportsReportManager.sportTypeId == 181) {
                    url = "DailyWinLoss2h?" + $.param({
                        ExportType: "VirtualSportsBySportBettypeDetails_VirtualHR",
                        sporttype: virtualSportsReportManager.sportTypeId,
                        reportdate: virtualSportsReportManager.reportDateBySport,
                        bettype: virtualSportsReportManager.bettype
                    });

                    $('#frm').attr('src', url);
                    return;
                }
                url += "&ExportType=VirtualSportsBySportBettypeDetails";
                url += "&ReportDate=" + virtualSportsReportManager.reportDateBySport;
                if (virtualSportsReportManager.isLive) {
                    url += "&SportType=" + virtualSportsReportManager.sportTypeIdBySport;
                }
                else {
                    url += "&SportType=" + virtualSportsReportManager.sportTypeIdBySport;
                }
                url += "&BetType=" + virtualSportsReportManager.bettype;
                url += '&BetTypeName=' + virtualSportsReportManager.bettypeName;
                url += '&IsLive=' + virtualSportsReportManager.isLive;
                url += '&IsHedge=' + virtualSportsReportManager.isHedge;
                break;
            case "5": // VirtualSportBySport
                url += "&ExportType=VirtualSportsBySport";
                url += "&ReportDate=" + virtualSportsReportManager.reportDateBySport;
                url += "&SportType=" + virtualSportsReportManager.sportTypeIdBySport;
                break;
        }
        $('#frm').attr('src', url);
    },

    getDailyWLBySportBettype_VirtualHR: function (bettype, bettypename, sporttype) {
        $("#tblContent_RDtVsp_VirtualHR").removeClass('hidden');
        $("#tblContent_RDtVsp").addClass("hidden");
        if (!isAllowReadHorseRacingReport) {
            return;
        }
        var data = {
            param:
            {
                ReportDate: virtualSportsReportManager.reportDateBySport,
                SportType: sporttype,
                BetType: bettype
            }
        };
        virtualSportsReportManager.sportTypeId = sporttype;
        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        virtualSportsReportManager.bettype = bettype;
        virtualSportsReportManager.bettypeName = bettypename;
        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2h/DailyWLDirectRacingBySportDetails",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }

                $("#gridtitlebettype_VSR_VirtualHR").html(virtualSportsReportManager.title + ">" + bettypename.toUpperCase());
                virtualSportsReportManager.renderDailyWLReportSportBettype_VirtualHR(rs);
            },
            error: function () {
                $("#tblContent_RDtVsp_VirtualHR").find("tr:not(.header)").remove();
                $("#tblContent_RDtVsp_VirtualHR").append('<tr><td colspan="7" class="b c"><span style="">There is no information available.</span></td></tr>');
                virtualSportsReportManager.showOtherSportsBettypeReport(true);
                $("#gridbettypeVSR_VirtualHR").removeClass('hidden');
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReportSportBettype_VirtualHR: function (dailyWLResult) {
        $("#tblContent_RDtVsp").addClass("hidden");
        if (dailyWLResult && dailyWLResult.ReportDetails && dailyWLResult.ReportDetails.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var items = dailyWLResult.ReportDetails;
            var itemTotal = items.length;
            for (var i = 0; i < itemTotal; i++) {
                arrBody.push(virtualSportsReportManager.renderDailyWLRowSportDetails_VirtualHR(items[i], i));
            }
            //          arrFooter.push(virtualSportsReportManager.renderDailyWLBettypeGrandTotal(dailyWLResult.ReportTotal.NormalTotal));
            var bodyHtml = arrBody.join('');
            bodyHtml = stringformat.removeSpaceIE9(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = stringformat.removeSpaceIE9(footerHtml);

            $("#tblContent_RDtVsp_VirtualHR").find("tr:not(.header)").remove();
            $("#tblContent_RDtVsp_VirtualHR").append(bodyHtml);
            $("#tblContent_RDtVsp_VirtualHR").append(footerHtml);
        }
        else {
            $("#gridbettypeVSR_VirtualHR").find("tr:not(.header)").remove();
            $("#tblContent_RDtVsp_VirtualHR").append('<tr><td colspan="7" class="b c"><div style="">There is no information available.</div></td></tr>');
        }
        virtualSportsReportManager.showOtherSportsBettypeReport(true);
        $("#gridbettypeVSR").removeClass("hidden");
    },
    renderDailyWLRowSportDetails_VirtualHR: function (dwlRow, index) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = virtualSportsReportManager.normalRowHtmlSportDetails_VirtualHR(dwlRow, index);
                break;
            case 3:
                html = virtualSportsReportManager.sportTotalRowHtmlSportDetails_VirtualHR(dwlRow);
                break;
            case 9:
                html = virtualSportsReportManager.emptyRowHtml_VirtualHR(dwlRow);
                break;
            case 10:
                html = virtualSportsReportManager.leagueHeadLineHtml_VirtualHR(dwlRow);
                break;
        }
        return html;
    },
    leagueHeadLineHtml_VirtualHR: function (row) {
        var html = '';
        html = '<tr><td class="l" colspan="7"><span class="b longtext-wrapper" style="">' +
                        row.Name + '</span></td></tr>';

        return html;
    },
    normalRowHtmlSportDetails_VirtualHR: function (row, index) {
        var html = '';
        html = '<tr class="' + (row.RowIndex % 2 == 0 ? 'evenrow' : '') + '">\
                        <td class="b c"><span>' + moment(row.Name).format('HH:mm') + '<span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + (row.Position.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + (row.Commission.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="r"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r"><span class="' + (row.Turnover.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0) ? '' : 'red') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    sportTotalRowHtmlSportDetails_VirtualHR: function (row) {
        var html = '';
        html = '<tr class="">\
                        <td class="r b"><span>' + row.Name + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + (row.Position.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + (row.Commission.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="linetotal r b"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r b"><span class="' + (row.Turnover.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0) ? '' : 'red') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    }
}