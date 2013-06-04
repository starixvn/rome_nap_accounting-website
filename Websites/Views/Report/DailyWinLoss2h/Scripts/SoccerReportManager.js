



var SoccerReportManager = {
    reportDate: null,
    sportTypeId: null,
    bettype: null,
    path: "Soccer",
    backToMainMenuText: "Back to Main Menu",
    title: "",
    noneLiveReportTitle: "",
    liveReportTitle: "",
    reportDetailTitle: "",
    reportDetailPath: "",
    reportDateForDetail: null,
    bettypeName: null,
    isLive: null,
    IsHedge: null,
    rowIndex: 0,
    initGrid: function (sportId, wldate) {
        SoccerReportManager.sportTypeId = sportId;
        SoccerReportManager.reportDate = wldate;

        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");
        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            SoccerReportManager.viewReport(reportDate);
        });
        $("#btnExport").click(function () {
            SoccerReportManager.exportToExcel();
        });

        $('#spMoreElement').empty();
        var strtoday = '<input type="button" onclick="SoccerReportManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="SoccerReportManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);

        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromSCR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + SoccerReportManager.path + "</b>";
        $('#btnBackMainSoccerNonLive').click(function () {
            WL2hShowHideDivManager.showMainReportFromSCR(sportId, wldate);
        });
        $('#btnBackMainSoccerLive').click(function () {
            WL2hShowHideDivManager.showMainReportFromSCR(sportId, wldate);
        });

        $("#btnExport_SccDetail").unbind("click");
        $("#btnExport_SccDetail").click(function () {
            SoccerReportManager.exportReportDetail();
        });

        SoccerReportManager.initReportTable();
        scoreInfoManager.InitPage();
        $("#dailyWLReportDiv").removeClass("hidden");
        try { parent.jQuery.unblockUI(); } catch (e) { }
        SoccerReportManager.viewReport(wldate);
    },
    initReportTable: function () {
        SoccerReportManager.reportView = $("#soccerReport");

        SoccerReportManager.nonLiveSCR = $("#nonLiveSoccerReport");
        SoccerReportManager.nonLiveSCR_TB = $("#tblContent_NlScr");

        SoccerReportManager.liveSCR = $("#liveSoccerReport");
        SoccerReportManager.liveSCR_TB = $("#tblContent_LScr");

        SoccerReportManager.rightDetailReport = $("#rightDetails_SCR");
        SoccerReportManager.reportDetailSCR = $("#soccerReportDetails");
        SoccerReportManager.reportDetailSCR_TB = $("#tblContent_RDtScr tbody");
    },
    viewReport: function (wldate) {
        SoccerReportManager.getLivetypeSportDetail(wldate);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                SoccerReportManager.getLivetypeSportDetail($('#btnToday').val());
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                SoccerReportManager.getLivetypeSportDetail($('#btnYesterday').val());
                break;
            default:
                break;
        }
    },
    getLivetypeSportDetail: function (wldate) {
        if (!isAllowReadSoccerReport) {
            return;
        }

        scoreInfoManager.ClosePopup();
        SoccerReportManager.reportDate = wldate;
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
            url: window.rootUrl + "DailyWinLoss2h/SelSoccerReportMain",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                SoccerReportManager.nonLiveSCR.addClass('hidden');
                SoccerReportManager.reportView.removeClass('hidden');

                SoccerReportManager.renderDailyWLReport(rs, wldate);
            },
            error: function () {
                SoccerReportManager.noneLiveReportTitle = 'SOCCER "NON - LIVE" PRODUCT WIN/LOSS REPORT';
                $("#nL_SCRHeader").html(SoccerReportManager.noneLiveReportTitle);
                SoccerReportManager.nonLiveSCR.removeClass('hidden');
                SoccerReportManager.renderNoData_NoneLiveReport();
                SoccerReportManager.liveReportTitle = 'SOCCER "LIVE" PRODUCT WIN/LOSS REPORT';
                $("#l_SCRHeader").html(SoccerReportManager.liveReportTitle);
                SoccerReportManager.renderNoData_LiveReport();
            },
            complete: function () {
                $("#soccerReport").css("width", "100%");
                var pagewidth = $(window).width();
                $(".mainreport").css("width", pagewidth + "px");
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        if (dailyWLResult) {
            $("#gridSplitter_SCR").addClass('hidden');
            SoccerReportManager.rightDetailReport.addClass("hidden");
            isBARMActive = dailyWLResult.NoneLiveReport.IsBARMActive.toString();
            SoccerReportManager.renderNoneLiveReportDWL2h(dailyWLResult.NoneLiveReport, wldate);
            SoccerReportManager.renderLiveReportDWL2h(dailyWLResult.LiveReport, wldate);
        }
    },
    renderNoneLiveReportDWL2h: function (noneLiveReport, wldate) {
        SoccerReportManager.noneLiveReportTitle = 'SOCCER "NON - LIVE" PRODUCT WIN/LOSS REPORT';
        $("#nL_SCRHeader").html(SoccerReportManager.noneLiveReportTitle);
        SoccerReportManager.nonLiveSCR.removeClass('hidden');
        var items = noneLiveReport.ReportDetails;
        var arrBody = [];
        var arrFooter = [];
        if (items != null && items.length > 0) {
            var numberItems = items.length;
            var rowOrder = 0;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(SoccerReportManager.renderMainRow_LeftReport(items[i], (rowOrder % 2 == 0 ? "evenrow" : "")));
                rowOrder++;
            }
            arrBody.push(SoccerReportManager.renderEmptyRow_LeftReport());
            arrFooter.push(SoccerReportManager.renderTotalRow_LeftReport(noneLiveReport.ReportTotal.NormalTotal));

            var allIBCTotal = noneLiveReport.ReportTotal.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += noneLiveReport.ReportTotal.BARMTotal.WinLoss;
            }

            if (allIBCTotal != 0) {
                arrFooter.push(SoccerReportManager.render8RMTotalRow_LeftReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = stringformat.removeSpaceIE9(bodyHtml);
            SoccerReportManager.nonLiveSCR_TB.find("tr:not(.header)").remove();
            SoccerReportManager.nonLiveSCR_TB.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = stringformat.removeSpaceIE9(footerHtml);
            SoccerReportManager.nonLiveSCR_TB.append(footerHtml);
            SoccerReportManager.nonLiveSCR.removeClass('hidden');
        }
        else {
            SoccerReportManager.renderNoData_NoneLiveReport();
        }
    },
    renderLiveReportDWL2h: function (liveReport, wldate) {
        SoccerReportManager.liveReportTitle = 'SOCCER "LIVE" PRODUCT WIN/LOSS REPORT';
        $("#l_SCRHeader").html(SoccerReportManager.liveReportTitle);
        var items = liveReport.ReportDetails;
        SoccerReportManager.liveSCR.removeClass('hidden');
        if (items != null && items.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var numberItems = items.length;
            var rowOrder = 0;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(SoccerReportManager.renderMainRow_LeftReport(items[i], (rowOrder % 2 == 0 ? "evenrow" : "")));
                rowOrder++;
            }
            arrBody.push(SoccerReportManager.renderEmptyRow_LeftReport());
            arrFooter.push(SoccerReportManager.renderTotalRow_LeftReport(liveReport.ReportTotal.NormalTotal));

            var allIBCTotal = liveReport.ReportTotal.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += liveReport.ReportTotal.BARMTotal.WinLoss;
            }
            if (allIBCTotal != 0) {
                arrFooter.push(SoccerReportManager.render8RMTotalRow_LeftReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = stringformat.removeSpaceIE9(bodyHtml);
            SoccerReportManager.liveSCR_TB.find("tr:not(.header)").remove();
            SoccerReportManager.liveSCR_TB.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = stringformat.removeSpaceIE9(footerHtml);
            SoccerReportManager.liveSCR_TB.append(footerHtml);
            SoccerReportManager.liveSCR.removeClass('hidden');
        }
        else {
            SoccerReportManager.renderNoData_LiveReport();
        }
    },
    renderNoData_NoneLiveReport: function () {
        SoccerReportManager.nonLiveSCR_TB.find("tr:not(.header)").remove();
        SoccerReportManager.nonLiveSCR_TB.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="11" class="b c" style="" >There is no information available.</td></tr>');
        SoccerReportManager.nonLiveSCR.removeClass('hidden');
    },
    renderNoData_LiveReport: function () {
        SoccerReportManager.liveSCR_TB.find("tr:not(.header)").remove();
        SoccerReportManager.liveSCR_TB.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="11" class="b c" style="" >There is no information available.</td></tr>');
        SoccerReportManager.liveSCR.removeClass('hidden');
    },
    renderMainRow_LeftReport: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td class="l"><span class="b">' + SoccerReportManager.getReportLink(row) + '</span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span>' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span>' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r"><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r"><span class="' + (row.LineTotalVirtual.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.LineTotalVirtual) + '</span></td>\
                <td class="r"><span>' + StringFormatter.getDisplayColorOfValue(row.Variance) + '</span></td>\
                <td class="r"><span>' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span>' + StringFormatter.formatNumber(row.TurnoverVirtual) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
                <td class="r"><span class="' + (row.TurnoverVirtual.toFixed(2) != 0 && row.MarginVirtual.toFixed(2) > 0 ? 'red' : '') + '">' + (row.TurnoverVirtual.toFixed(2) != 0 ? StringFormatter.formatNumber(row.MarginVirtual) + '%' : 'N/A') + '</span></td>\
            </tr>';
        return html;
    },
    renderTotalRow_LeftReport: function (row) {
        var html = '';
        html = '<tr>\
                <td class="r"><span class="b">' + row.Name + '</span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r" style="background-color:#B5C4D0"><span class="b ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r" style="background-color:#B5C4D0"><span class="b ' + (row.LineTotalVirtual.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.LineTotalVirtual) + '</span></td>\
                <td class="r" style="background-color:#B5C4D0"><span class="b">' + StringFormatter.getDisplayColorOfValue(row.Variance) + '</span></td>\
                <td class="r"><span class="b">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span class="b">' + StringFormatter.formatNumber(row.TurnoverVirtual) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
                <td class="r"><span class="' + (row.TurnoverVirtual.toFixed(2) != 0 && row.MarginVirtual.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.TurnoverVirtual.toFixed(2) != 0 ? StringFormatter.formatNumber(row.MarginVirtual) + '%' : 'N/A') + '</span></td>\
            </tr>';
        return html;
    },
    render8RMTotalRow_LeftReport: function (ibcValue) {
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
                        <td class="r"><span class="b">' +
                                       ibcMsg + '</span></td>\
                        <td class="r"><span class="b ' + (ibcValue.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(ibcValue) + '</span></td>\
                        <td colspan ="9" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    renderEmptyRow_LeftReport: function () {
        var html = '<tr>\
                        <td colspan ="11" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    getReportLink: function (normalWLItem) {
        var evt = "SoccerReportManager.openReportDetails('" + normalWLItem.Id + "','" +
                                                        normalWLItem.Name + "','" +
                                                        normalWLItem.Hedge + "','" +
                                                        SoccerReportManager.sportTypeId + "','" +
                                                        SoccerReportManager.reportDate + "','" +
                                                        normalWLItem.IsLive + "'); return;";
        var reportLink = '<b>\
                             <a onclick="' + evt + '">' + normalWLItem.AliasName + '</a>\
                          </b>';

        switch (normalWLItem.Id) {
            case 71:
                reportLink = "Casino";
                break;
            case 72:
                reportLink = "P2P";
                break;
            case 73:
                reportLink = "IBCbet Bingo";
                break;
        }
        return reportLink;
    },
    openReportDetails: function (bettypeId, bettypeName, hedge, sporttypeId, reportDate, isLive) {
        if (!isAllowReadSoccerReport) {
            return;
        }

        scoreInfoManager.ClosePopup();
        var isHedge = (hedge && hedge.indexOf("(hedge)") != -1);
        var data = {
            param:
            {
                ReportDate: reportDate,
                SportType: SoccerReportManager.sportTypeId,
                LiveIndicator: isLive,
                BetType: bettypeId,
                IsHedge: isHedge,
                IsLive: (isLive == 'true' ? 1 : 0)
            }
        };
        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        SoccerReportManager.reportDateForDetail = reportDate;
        SoccerReportManager.bettype = bettypeId;
        SoccerReportManager.bettypeName = bettypeName;
        SoccerReportManager.isLive = (isLive == 'true' ? 1 : 0)
        SoccerReportManager.isHedge = isHedge;

        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2h/SelDailyWLDirectDetails",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                SoccerReportManager.reportDetailSCR.addClass('hidden');
                $("#gridSplitter_SCR").removeClass('hidden');
                SoccerReportManager.rightDetailReport.removeClass("hidden");
                SoccerReportManager.renderReportDetailsDWL2h(rs, reportDate, bettypeId, bettypeName, isHedge, isLive);
            },
            error: function () {
                isBARMActive = reportDetail.IsBARMActive.toString();
                SoccerReportManager.reportDetailTitle = 'SOCCER EVENT WIN/LOSS REPORT';
                $("#RDt_SCRHeader").html(SoccerReportManager.reportDetailTitle);
                SoccerReportManager.reportDetailPath = "Main>Soccer>";
                $("#RDt_SCRHeader_Path").html(SoccerReportManager.reportDetailPath);
                SoccerReportManager.liveSCR.removeClass('hidden');
                SoccerReportManager.renderNoData_ReportDetails();
            },
            complete: function () {
                $("#soccerReport").css("width", "3000px");
                var totalWidth = $("#soccerReportTable").width() + 10;
                $("#soccerReport").css("width", totalWidth + "px");
                var mainReportWidth = $(".mainreport").width();
                var pagewidth = $(window).width();
                if (mainReportWidth < totalWidth) {
                    $(".mainreport").css("width", totalWidth + "px");
                }
                else if (totalWidth <= pagewidth) {
                    $(".mainreport").css("width", pagewidth + "px");
                }

                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderReportDetailsDWL2h: function (reportDetail, wldate, bettypeId, bettypeName, isHedge, isLive) {
        isBARMActive = reportDetail.IsBARMActive.toString();
        SoccerReportManager.reportDetailTitle = 'SOCCER EVENT WIN/LOSS REPORT';
        $("#RDt_SCRHeader").html(SoccerReportManager.reportDetailTitle);
        SoccerReportManager.reportDetailPath = "Main>Soccer>" + bettypeName;
        //SoccerReportManager.reportDetailPath += (isHedge ? "(headge)" : "");
        $("#RDt_SCRHeader_Path").html(SoccerReportManager.reportDetailPath);
        SoccerReportManager.liveSCR.removeClass('hidden');
        if (reportDetail != null && reportDetail.ReportDetails.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var items = reportDetail.ReportDetails;
            var numberItems = items.length;

            SoccerReportManager.rowIndex = 0;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(SoccerReportManager.renderDailyWLRowSportDetails(items[i], i));
            }

            arrFooter.push(SoccerReportManager.renderTotalRow_RightReport(reportDetail.ReportTotal.NormalTotal, isLive, bettypeName, isHedge));
            var allIBCTotal = reportDetail.ReportTotal.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += reportDetail.ReportTotal.BARMTotal.WinLoss;
            }

            if (allIBCTotal != 0) {
                arrFooter.push(SoccerReportManager.render8RMTotalRow_RightReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = stringformat.removeSpaceIE9(bodyHtml);
            SoccerReportManager.reportDetailSCR_TB.find("tr:not(.header)").remove();
            SoccerReportManager.reportDetailSCR_TB.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = stringformat.removeSpaceIE9(footerHtml);
            SoccerReportManager.reportDetailSCR_TB.append(footerHtml);
            SoccerReportManager.reportDetailSCR.removeClass('hidden');

        }
        else {
            SoccerReportManager.renderNoData_ReportDetails();
        }
    },
    renderDailyWLRowSportDetails: function (dwlRow, index) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = SoccerReportManager.renderMainRow_RightReport(dwlRow, index);
                SoccerReportManager.rowIndex++;
                break;
            case 3:
                html = SoccerReportManager.renderBettypeTotalRow_RightReport(dwlRow);
                break;
            case 9:
                html = SoccerReportManager.emptyRowHtml_RightReport(dwlRow);
                break;
            case 10:
                html = SoccerReportManager.renderLeagueHeadLine(dwlRow);
                SoccerReportManager.rowIndex = 0;
                break;
        }
        return html;
    },
    renderLeagueHeadLine: function (dwlRow) {
        var html = '';
        html = '<tr>\
                    <td style="">\
                        <span></span></td>\
                    <td colspan = "13" class="l" style=""><span class="b longtext-wrapper">' +
                        (dwlRow.Name && dwlRow.Name != '' ? dwlRow.Name : '&nbsp') + '</span></td>\
                </tr>';
        return html;
    },
    renderMainRow_RightReport: function (row, index) {
        var html = '';
        var eventDetail = "";
        if (SoccerReportManager.bettype == 10 || SoccerReportManager.sportTypeId == 201) {
            eventDetail = '<td colspan = "3" style="max-width: 300px;"><span class="c longtext-wrapper" style="">' + row.MatchInfo.HomeTeam.Name + (row.MatchInfo.IsNeutral ? " (N)" : "") + '</span></td>';
        }
        else {
            eventDetail = '<td class="r" style="max-width: 300px;"><span class="longtext-wrapper" style="">' + row.MatchInfo.HomeTeam.Name + (row.MatchInfo.IsNeutral ? " (N)" : "") + '</span></td>\
                        <td class="c" style="width: 25px;">-vs-</td>\
                        <td class="l" style="max-width: 300px;"><div class="longtext-wrapper" style="">' + row.MatchInfo.AwayTeam.Name + '</span></td>';
        }

        var openScoreInfoEvent = "scoreInfoManager.GetReport('" + SoccerReportManager.reportDate + "','" +
                                                                  SoccerReportManager.sportTypeId + "','" +
                                                                  "Soccer" + "','" +
                                                                  SoccerReportManager.bettype + "','" +
                                                                  SoccerReportManager.bettypeName + "'," +
                                                                  SoccerReportManager.isLive + ",'" +
                                                                  SoccerReportManager.isHedge + "','" +
                                                                  row.GroupId + "','" +
                                                                  row.GroupName + "'," +
                                                                  row.MatchInfo.Id + ",'" +
                                                                  row.MatchInfo.Name + "','" +
                                                                  row.MatchInfo.HomeTeam.Id + "','" +
                                                                  row.MatchInfo.HomeTeam.Name + "','" +
                                                                  row.MatchInfo.AwayTeam.Id + "','" +
                                                                  row.MatchInfo.AwayTeam.Name + "'); return;";
        var scoreInfoLink = '<a class="b" title="Show Score Information" onclick="' + openScoreInfoEvent + '">' + row.MatchInfo.Name + '</a>';

        html = '<tr class="' + (row.RowIndex % 2 == 0 ? "evenrow" : "") + '">\
                    <td style="word-wrap: break-word;"><div class="c">' + scoreInfoLink + '</span></td>'
                   + eventDetail + '<td class="r"><div class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                    <td class="r"><span>' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                    <td class="r"><span>' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                    <td class="r"><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                    <td class="r"><span class="' + (row.LineTotalVirtual.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.LineTotalVirtual) + '</span></td>\
                    <td class="r"><span>' + StringFormatter.getDisplayColorOfValue(row.Variance) + '</span></td>\
                    <td class="r"><span>' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                    <td class="r"><span>' + StringFormatter.formatNumber(row.TurnoverVirtual) + '</span></td>\
                    <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
                    <td class="r"><span class="' + (row.TurnoverVirtual.toFixed(2) != 0 && row.MarginVirtual.toFixed(2) > 0 ? 'red' : '') + '">' + (row.TurnoverVirtual.toFixed(2) != 0 ? StringFormatter.formatNumber(row.MarginVirtual) + '%' : 'N/A') + '</span></td>\
                </tr>';
        return html;
    },
    renderBettypeTotalRow_RightReport: function (row) {
        var html = '';
        html = '<tr>\
                <td class="r tdtitle" bgcolor="#888888" colspan = "4" ><span class="b"></span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r"><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r"><span class="' + (row.LineTotalVirtual.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.LineTotalVirtual) + '</span></td>\
                <td class="r"><span>' + StringFormatter.getDisplayColorOfValue(row.Variance) + '</span></td>\
                <td class="r"><span>' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span>' + StringFormatter.formatNumber(row.TurnoverVirtual) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
                <td class="r"><span class="' + (row.TurnoverVirtual.toFixed(2) != 0 && row.MarginVirtual.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.TurnoverVirtual.toFixed(2) != 0 ? StringFormatter.formatNumber(row.MarginVirtual) + '%' : 'N/A') + '</span></td>\
            </tr>';
        return html;
    },
    renderTotalRow_RightReport: function (row, isLive, bettypeName, isHedge) {
        var html = '';
        var totalTitle = (isLive == "true" ? "<font color='red'>Live!</font>" : "") + " " +
                        bettypeName + (isHedge ? " (Hegde)" : "") + " Total:";
        html = '<tr>\
                <td class="r" colspan = "4"><span class="b longtext-wrapper">' + totalTitle + '</span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span style="font-weight: normal; width: 90px;">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r"><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r"><span class="' + (row.LineTotalVirtual.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.LineTotalVirtual) + '</span></td>\
                <td class="r"><span style="font-weight: normal;">' + StringFormatter.getDisplayColorOfValue(row.Variance) + '</span></td>\
                <td class="r"><span style="font-weight: normal;">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span style="font-weight: normal;">' + StringFormatter.formatNumber(row.TurnoverVirtual) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
                <td class="r"><span class="' + (row.TurnoverVirtual.toFixed(2) != 0 && row.MarginVirtual.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.TurnoverVirtual.toFixed(2) != 0 ? StringFormatter.formatNumber(row.MarginVirtual) + '%' : 'N/A') + '</span></td>\
            </tr>';
        return html;
    },
    render8RMTotalRow_RightReport: function (ibcValue) {
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
                        <td class="r"><span class="b col100 ' + (ibcValue.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(ibcValue) + '</span></td>\
                        <td class="r" colspan ="9" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    renderNoData_ReportDetails: function () {
        SoccerReportManager.reportDetailSCR_TB.find("tr:not(.header)").remove();
        SoccerReportManager.reportDetailSCR_TB.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="14" class="b c" style="" >There is no information available.</td></tr>');
        SoccerReportManager.reportDetailSCR.removeClass('hidden');
    },
    exportToExcel: function () {
        if (!isAllowReadSoccerReport) {
            return;
        }
        var url = 'DailyWinLoss2h?ExportType=SoccerMain' + '&ReportDate=' +
                    SoccerReportManager.reportDate;
        $('#frm').attr('src', url);

    },
    exportReportDetail: function () {
        if (!isAllowReadSoccerReport) {
            return;
        }

        var url = 'DailyWinLoss2h?ExportType=SoccerDetails' +
                    '&ReportDate=' + SoccerReportManager.reportDateForDetail +
                    '&SportType=' + SoccerReportManager.sportTypeId +
                    '&BetType=' + SoccerReportManager.bettype +
                    '&BetTypeName=' + SoccerReportManager.bettypeName +
                    '&IsLive=' + SoccerReportManager.isLive +
                    '&IsHedge=' + SoccerReportManager.isHedge;
        $('#frm').attr('src', url);
    }
};



