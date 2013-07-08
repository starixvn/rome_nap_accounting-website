var SMATurnover = {
    constants: {
        CountdownValue: {
            NotRefresh: 0
        },

        MaxPercentageUsed: 100
    },
    cachedData: {
        ddlSport: null,
        ddlLeague: null,
        txtfromdate: null,
        txttodate: null,
        reportParams: null,
        tblSMATurnoverBody: null,
        refreshIntervalCounter: 0,
        refreshIntervalEvent: null,
        sendMailInteralCounter: 10000,
        sendMailIntervalEvent: null,
        lastReport: null,
        reportRows: null,
        updateCountdownEvent: null,
        countdownTime: 0
    },
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
            var currValueOriginal = obj.val();
            if (currValueOriginal != "") {
                currValue = Helper.SeparateNumricByThousand(currValueOriginal);
                var formatCurrentValue = formatNumber2(currValue);
                if (formatCurrentValue != "NaN") {
                    obj.val(formatCurrentValue);
                }
            }
            else {
                obj.val('');
            }
            if (formatCurrentValue != oldValue && formatCurrentValue != "NaN" && currValueOriginal != "") {
                $('#btnUpdate_' + matchId + '_' + custId).removeAttr('disabled');
            }
            else {
                $('#btnUpdate_' + matchId + '_' + custId).attr('disabled', true);
            }

        });

        SMATurnover.CacheDOMElement();
        var refreshIcon = $(".refreshBox .refreshIcon");
        refreshIcon.unbind("click");
        refreshIcon.click(function () {
            if (SMATurnover.cachedData.refreshIntervalCounter > 1) {
                var refreshIcon = $(".refreshBox .refreshIcon");
                SMATurnover.cachedData.countdownTime = SMATurnover.cachedData.refreshIntervalCounter / 1000;
                window.clearInterval(SMATurnover.cachedData.refreshIntervalEvent);
                SMATurnover.cachedData.refreshIntervalEvent = setInterval(function () {
                    SMATurnover.cachedData.countdownTime = SMATurnover.cachedData.refreshIntervalCounter / 1000;
                    SMATurnover.refreshSMATurnoverList(false);
                }, SMATurnover.cachedData.refreshIntervalCounter);

                refreshIcon.html(SMATurnover.cachedData.countdownTime);
                SMATurnover.refreshSMATurnoverList(false);
            }
        });

        var refreshDropdownIcon = $(".refreshBox .refreshDropdownIcon");
        refreshDropdownIcon.unbind("click");
        refreshDropdownIcon.click(function () {
            SMATurnover.ShowCountDownOptions($(this));
        });

        var countDownOptions = $(".countDownOptionContainer .countDownOption");
        countDownOptions.unbind("click");
        countDownOptions.click(function () {
            SMATurnover.SaveCountDownRefreshOption(this);
        });
    },
    ShowCountDownOptions: function (btnDown) {
        var pnlDown = $('.countDownOptionContainer');
        var arrBtnPos = SMATurnover.FindPosition(btnDown.parent());
        if (pnlDown.css("display") == 'none') {
            pnlDown.css("left", arrBtnPos[0] + 'px');
            pnlDown.css("top", (arrBtnPos[1] + 20) + 'px');
            pnlDown.css("display", 'inline-block');
        }
        else {
            pnlDown.css("display", 'none');
        }
    },
    CacheDOMElement: function () {
        SMATurnover.cachedData.tblSMATurnoverBody = $("#tblSMATurnover tbody");
        SMATurnover.cachedData.ddlSport = $('#ddlSport');
        SMATurnover.cachedData.ddlLeague = $('#ddlLeague');
        SMATurnover.cachedData.txtfromdate = $('#txtfromdate');
        SMATurnover.cachedData.txttodate = $('#txttodate');
    },
    FindPosition: function (oElement) {
        if (typeof (oElement.offsetParent()) != 'undefined') {
            for (var posX = 0, posY = 0; oElement && oElement.length; oElement = oElement.offsetParent()) {
                posX += oElement.offset().left;
                posY += oElement.offset().top;
                if (oElement[0].nodeName.toLowerCase() == 'body') {
                    break;
                }
            }

            return [posX, posY];
        } else {
            return [oElement.x, oElement.y];
        }
    },
    SaveCountDownRefreshOption: function (btnCountDownOption) {
        btnCountDownOption = $(btnCountDownOption);
        $(".countDownOption.countDownOptionCheck").removeClass("countDownOptionCheck");
        btnCountDownOption.addClass("countDownOptionCheck");
        setTimeout(function () {
            $('.countDownOptionContainer').css("display", 'none');
            var refreshIcon = $(".refreshBox .refreshIcon");
            var coundownAfter = btnCountDownOption.attr("val");
            if (coundownAfter == "Not Refresh") {
                refreshIcon.css("text-align", 'left');
                refreshIcon.css("padding-left", '8px');
                refreshIcon.css("width", '85px');
                refreshIcon.html(coundownAfter);

                SMATurnover.cachedData.refreshIntervalCounter = 0;
                if (SMATurnover.cachedData.refreshIntervalEvent > 0) {
                    window.clearInterval(SMATurnover.cachedData.refreshIntervalEvent);
                    window.clearInterval(SMATurnover.cachedData.updateCountdownEvent);
                }
            }
            else {
                refreshIcon.css("text-align", 'center');
                refreshIcon.css("padding-left", '0');
                refreshIcon.css("width", '93px');
                refreshIcon.html(coundownAfter);

                if (SMATurnover.cachedData.refreshIntervalEvent > 0) {
                    window.clearInterval(SMATurnover.cachedData.refreshIntervalEvent);
                    window.clearInterval(SMATurnover.cachedData.updateCountdownEvent);
                }

                SMATurnover.cachedData.refreshIntervalCounter = parseInt(coundownAfter * 1000);
                SMATurnover.cachedData.countdownTime = parseInt(coundownAfter);
                SMATurnover.cachedData.updateCountdownEvent = setInterval(function () {
                    if (SMATurnover.cachedData.countdownTime > 1) {
                        SMATurnover.cachedData.countdownTime--;
                    } else {
                        SMATurnover.cachedData.countdownTime = SMATurnover.cachedData.refreshIntervalCounter / 1000;
                    }

                    refreshIcon.html(SMATurnover.cachedData.countdownTime);
                }, 1000);
                SMATurnover.cachedData.refreshIntervalEvent = setInterval(function () {
                    SMATurnover.cachedData.countdownTime = SMATurnover.cachedData.refreshIntervalCounter / 1000;
                    SMATurnover.refreshSMATurnoverList(false);
                }, SMATurnover.cachedData.refreshIntervalCounter);
            }

        }, 200);
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
                SMATurnover.ChangeSport();
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
            $('#msgErrorAdd').hide();
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
                    idLeague.append('<option value="' + rs[i].Id + '" title="' + rs[i].Name + '">' + rs[i].Name + '</option>')
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
        $('#msgErrorAdd').hide();
        var matchId = $('#txtMatchId').val();
        if (matchId == "") {
            $('#msgErrorAdd').show();
            showAlertMessage("msgErrorAdd", "Please input matchId !", 1);
            setTimeout(function () { $('#msgErrorAdd').hide(); }, 4000);
        }
        else {
            var data = { matchId: matchId };
            $.ajax({
                type: "POST",
                url: "GetInfoByMatchId",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    if (rs != null) {
                        if (rs.ErrorCode != 0) {
                            $('#msgErrorAdd').show();
                            showAlertMessage("msgErrorAdd", rs.ErrorMessage, 1);
                            setTimeout(function () { $('#msgErrorAdd').hide(); }, 4000);
                            $('#txtLeague').val('');
                            $('#txtEventDate').val('');
                            $('#txtTeam').val('');
                        }
                        else {
                            $('#msgErrorAdd').hide();
                            $('#txtLeague').val(rs.LeagueName);
                            $('#txtEventDate').val(rs.EventDate);
                            $('#txtTeam').val(rs.HomeTeam + ' -vs- ' + rs.AwayTeam);
                        }
                    }

                }
            });
        }
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },

    getReportParams: function (getNewParams) {
        if (getNewParams || SMATurnover.cachedData.reportParams == null) {
            SMATurnover.cachedData.reportParams = {
                sportId: SMATurnover.cachedData.ddlSport.val(),
                leagueId: SMATurnover.cachedData.ddlLeague.val(),
                fromDate: SMATurnover.cachedData.txtfromdate.val(),
                toDate: SMATurnover.cachedData.txttodate.val()
            };
        }
    },
    GetOldInfo: function (matchId, custId) {
        var report = SMATurnover.cachedData.lastReport;
        var reportLength = report.length;
        for (var i = 0; i < reportLength; i++) {
            var row = report[i];
            var matchLength = row.CustomerList.length;
            var matchList = row.CustomerList;
            for (var j = 0; j < matchLength; j++) {
                var match = matchList[j];
                var customerLength = match.CustomerList.length;
                var customerList = match.CustomerList;
                for (var k = 0; k < customerLength; k++) {
                    var customer = customerList[k];
                    if (matchId == match.MatchId &&
                        custId == customer.CustId) {
                        return {
                            oldMatch: match,
                            oldCustomer: customer
                        };
                    }
                }
            }
        }

        return null;
    },
    refreshSMATurnoverList: function (getNewParams) {
        try { parent.BlockUI(); } catch (e) { }
        SMATurnover.getReportParams(getNewParams);

        $.ajax({
            type: "POST",
            url: "GetSMATurnoverList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(SMATurnover.cachedData.reportParams),
            success: function (rs) {
                if (SMATurnover.cachedData.lastReport == null ||
                    SMATurnover.cachedData.lastReport.length != rs.length) {
                    SMATurnover.cachedData.lastReport = rs;
                    SMATurnover.RenderReport(rs);
                } else {
                    var reportLength = rs.length;
                    for (var i = 0; i < reportLength; i++) {
                        var row = rs[i];
                        var matchLength = row.CustomerList.length;
                        var matchList = row.CustomerList;
                        for (var j = 0; j < matchLength; j++) {
                            var match = matchList[j];
                            var customerLength = match.CustomerList.length;
                            var customerList = match.CustomerList;
                            for (var k = 0; k < customerLength; k++) {
                                var customer = customerList[k];

                                var oldMatch = SMATurnover.cachedData.lastReport[i].CustomerList[j];
                                var oldCustomer = SMATurnover.cachedData.lastReport[i].CustomerList[j].CustomerList[k];
                                if (match.MatchId != oldMatch.MatchId ||
                                    customer.CustId != oldCustomer.CustId) {
                                    var oldInfo = SMATurnover.GetOldInfo(match.MatchId, customer.CustId);
                                    if (oldInfo) {
                                        oldMatch = oldInfo.oldMatch;
                                        oldCustomer = oldInfo.oldCustomer;
                                    } else {
                                        continue;
                                    }
                                }

                                if (customer.SMATurnover != oldCustomer.SMATurnover ||
                                    customer.PercentageUsed != oldCustomer.PercentageUsed) {
                                    var oldRow = SMATurnover.cachedData.tblSMATurnoverBody.find('.match_' + customer.CustId + '_' + match.MatchId);
                                    if (customer.SMATurnover != oldCustomer.SMATurnover) {
                                        var colSMATurnover = oldRow.find(".colSMATurnover");
                                        colSMATurnover.html(customer.SMATurnover);
                                    }

                                    if (customer.PercentageUsed != oldCustomer.PercentageUsed) {
                                        var colPercentUsed = oldRow.find(".colPercentUsed");
                                        if (customer.PercentageUsed == SMATurnover.constants.MaxPercentageUsed) {
                                            colPercentUsed.html('<div class="col160 c boxPercentUsed_100Percent ">' + customer.PercentageUsed + '%</div>');
                                        } else {
                                            colPercentUsed.html('<div class="col160 c boxPercentUsed_Low" style="background:url(\'../Views/Customer/CustomerSetting/SMATurnover/Style/Images/MaxPercentStatus.png\') no-repeat scroll -' + (100 - customer.PercentageUsed) + 'px 100% transparent">' + customer.PercentageUsed + '%</div>');
                                        }
                                    }
                                }
                            }
                        }
                    }

                    SMATurnover.cachedData.reportRows
                }

                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    RenderReport: function (report) {
        var arr = [];
        if (report != null && report.length > 0) {
            SMATurnover.cachedData.tblSMATurnoverBody.empty().append(SMATurnover.renderRow(report));
        }
        else {
            SMATurnover.cachedData.tblSMATurnoverBody.empty().append('<tr><td colspan="6"><div class="c">There is no data.</div></td></tr>');
        }

        SMATurnover.initGrid();
    },
    GetSMATurnoverList: function () {
        try { parent.BlockUI(); } catch (e) { }
        SMATurnover.cachedData.reportParams = {
            sportId: $('#ddlSport').val(),
            leagueId: $('#ddlLeague').val(),
            fromDate: $('#txtfromdate').val(),
            toDate: $('#txttodate').val()
        };

        $.ajax({
            type: "POST",
            url: "GetSMATurnoverList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(SMATurnover.cachedData.reportParams),
            success: function (rs) {
                SMATurnover.cachedData.lastReport = rs;
                var arr = [];
                SMATurnover.cachedData.tblSMATurnoverBody = $("#tblSMATurnover tbody");
                if (rs != null && rs.length > 0) {
                    //$("#tblContentHeader tbody").empty().append(SMATurnover.renderFrozenRow(rs));
                    SMATurnover.cachedData.tblSMATurnoverBody.empty().append(SMATurnover.renderRow(rs));
                    SMATurnover.cachedData.reportRows = SMATurnover.cachedData.tblSMATurnoverBody.find(">tr");
                }
                else {
                    //$("#tblContentHeader tbody").empty().append('<tr><td><div class="">&nbsp;</div></td></tr>');
                    //$("#tblSMATurnover tbody").empty().append('<tr><td><div class="">&nbsp;</div></td><td colspan="4"><div class="c">There is no data.</div></td></tr>');
                    SMATurnover.cachedData.tblSMATurnoverBody.empty().append('<tr><td colspan="6"><div class="c">There is no data.</div></td></tr>');
                    $(".lbl100ReachedCounter").html(0);
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
            gridWidth: 830, //=totalColumnWidth + totalColumn*4 +totalColumn
            gridHeight: 200,
            freezeWidth: 0,
            numberFreezeCol: 0
        };

        $("#SMATurnover").removeClass("hidden").gridScroller(gridOpts);
        // SMATurnover.makeTwoTableSameHeight($("#SMATurnover table.row_header_content"), $("#SMATurnover table.col_header_content"), false, true);
    },

    renderRow: function (result) {
        var html = '';
        var _100PercentageUsedCounter = 0;
        //        var isChanged = 0;

        for (var index = 0; index < result.length; index++) {
            var row = result[index];
            var matchLength = row.CustomerList.length;
            var matchList = row.CustomerList;
            for (var i = 0; i < matchLength; i++) {
                var match = matchList[i];

                //                var matchInfo = '<table class="smaTurnover-matchInfo">\
                //                                    <tr>\
                //                                        <td class="col200">' + 'MatchID: ' + match.MatchId + '(' + match.EventDate + ')' + '</td>\
                //                                        <td  class="col200">' + match.HomeTeam + ' -vs- ' + match.AwayTeam + '</td>\
                //                                    </tr>\
                //                                 </table>';

                html += '<tr>';
                html += '<td class="rowLeague" colspan="5">\
                            <div class="lblLeagueHeadline">' + row.LeagueName + '</div>' +
                            '<div class="lblTeamInfo">' + match.HomeTeam + ' -vs- ' + match.AwayTeam + '</div>' +
                            '<div class="rightArrow_LeagueInfo"></div>' +
                            '<div class="eventDate_LeagueInfo">Event Date: ' + match.EventDate + '</div>' +
                            '<div class="matchIdBox_LeagueInfo">Match Id: ' + match.MatchId + '</div>' +
                         '</td>';

                //                html += '<td class="rowLeague"><div class="col200">' + row.LeagueName + '</div></td>';
                //                html += '<td class="rowLeague" colspan="4">' + matchInfo + '</div></td>';
                html += '<td class="rowLeague"><div class="col60" ><div class="btnAddSMA" onclick="SMATurnover.RedirectToAddSMA(\'' + match.MatchId + '\')">Add SMA</div></td>';
                html += '</tr>';
                var customerLength = match.CustomerList.length;
                var customerList = match.CustomerList;
                for (var j = 0; j < customerLength; j++) {
                    var customer = customerList[j];
                    //                    var percentageUsed = customer.MaxTurnover != 0 ?
                    //                                        parseInt(customer.SMATurnover / customer.MaxTurnover) * 100 :
                    //                                        100;
                    var percentageUsed = customer.PercentageUsed;
                    //                    percentageUsed = 70;
                    if (percentageUsed >= SMATurnover.constants.MaxPercentageUsed) {
                        _100PercentageUsedCounter++;
                        percentageUsed = SMATurnover.constants.MaxPercentageUsed;
                    }

                    //                    SMATurnover.cachedData.lastReport[index].CustomerList[i].CustomerList[j].SMATurnover = 70;
                    //                    SMATurnover.cachedData.lastReport[index].CustomerList[i].CustomerList[j].PercentageUsed = 70;
                    //                    if (isChanged == 0) {
                    //                        var temp = SMATurnover.cachedData.lastReport[0].CustomerList[i].MatchId;
                    //                        SMATurnover.cachedData.lastReport[0].CustomerList[0].MatchId = SMATurnover.cachedData.lastReport[1].CustomerList[0].MatchId;
                    //                        SMATurnover.cachedData.lastReport[1].CustomerList[0].MatchId = temp;

                    //                        temp = SMATurnover.cachedData.lastReport[0].CustomerList[0].CustomerList[0].CustId;
                    //                        SMATurnover.cachedData.lastReport[0].CustomerList[0].CustomerList[0].CustId = SMATurnover.cachedData.lastReport[1].CustomerList[0].CustomerList[0].CustId;
                    //                        SMATurnover.cachedData.lastReport[1].CustomerList[0].CustomerList[0].CustId = temp;
                    //                        isChanged = 1;
                    //                    }

                    html += '<tr class="match_' + customer.CustId + '_' + match.MatchId + '">';
                    html += '<td><div class="col200 c">' + customer.Username + '</div></td>';
                    html += '<td><div class="col100 c ">' + customer.Currency + '</div></td>';
                    html += '<td><div class="col160 r colSMATurnover">' + customer.SMATurnover + '</div></td>';
                    if (match.EventStatus != "completed") {
                        html += '<td><div class="col160 table-cell" style="height:17px"><input type="text" class="MaxTurnover" id="txtMaxTurnover_' + match.MatchId + "_" + customer.CustId + '" matchId="' + match.MatchId + '" custId="' + customer.CustId + '" oldValue="' + customer.MaxTurnover + '" value="' + customer.MaxTurnover + '" style="text-align:right; width:157px" maxlength="15" onkeypress="return CheckIntNumber(event)"></div></td>';
                    }
                    else {
                        html += '<td><div class="col160 r" style="height:17px">' + customer.MaxTurnover + '</div></td>';
                    }

                    if (percentageUsed == SMATurnover.constants.MaxPercentageUsed) {
                        html += '<td class="c colPercentUsed">\
                                    <div class="col160 c boxPercentUsed_100Percent ">' + percentageUsed + '%</div>\
                                 </td>';
                    } else {
                        html += '<td class="c colPercentUsed">\
                                    <div class="col160 c boxPercentUsed_Low" style="background:url(\'../Views/Customer/CustomerSetting/SMATurnover/Style/Images/MaxPercentStatus.png\') no-repeat scroll -' + (100 - percentageUsed) + 'px 100% transparent">' + percentageUsed + '%</div>\
                                 </td>';
                    }

                    html += '<td><div class="col60 table-cell c" style="height:17px"><input type="button" disabled="disabled" id="btnUpdate_' + match.MatchId + "_" + customer.CustId + '" title="Update" onclick="SMATurnover.UpdateSMATurnoverProccess(\'' + customer.CustId + '\',\'' + match.MatchId + '\',\'' + customer.Username + '\')" class="btnSave" /></div></td>';
                    html += '</tr>';
                }
            }
        }

        $(".lbl100ReachedCounter").html(_100PercentageUsedCounter);
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
        var maxturnover = Helper.SeparateNumricByThousand($('#txtTurnover').val());
        if (parseInt(maxturnover) > 999999999999) {
            showAlertMessage("msgErrorAdd", "Max Turnover should be less than or equal 999,999,999,999", 1);
            $('#msgErrorAdd').show();
            setTimeout(function () { $('#msgErrorAdd').hide(); }, 4000);
        }
        else {
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
                        setTimeout(function () { $('#msgErrorAdd').hide(); }, 4000);
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

                }
            });
        }
        try { parent.jQuery.unblockUI(); } catch (e) { }
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
                    $('#txtSuperName').val('');
                    $('#txtTurnover').val('');
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
        var maxturnover = Helper.SeparateNumricByThousand(maxTurnover);
        if (parseInt(maxturnover) > 999999999999) {
            showAlertMessage("msgError", "Max Turnover should be less than or equal 999,999,999,999", 1);
            $('#msgError').show();
            setTimeout(function () { $('#msgError').hide(); }, 4000);
        }
        else {
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
                }
            });
        }
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },

    UpdateSuperTurnoverValue: function (matchId, custId, maxTurnover) {
        var data = { matchId: matchId, custId: custId, maxTurnover: maxTurnover };
        $.ajax({
            type: "POST",
            url: "UpdateSMATurnover",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
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
            var rows_TableA = tableA.find("> tbody > tr");
            var rows_TableB = tableB.find("> tbody > tr");
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
                        $(columns_TableA[0]).css("height", maxHeight + 'px');
                        $(columns_TableB[0]).css("height", maxHeight + 'px');

                        if ($(columns_TableA[0]).hasClass("rowLeague") &&
                            $.browser.mozilla) {
                            $(columns_TableA[0]).css("height", maxHeight + 5 + 'px');
                        }
                    }
                }
            }
        }
    },

    InputMaxTurnover: function () {
        // var maxTurnover = $('#txtTurnover').val();
        var maxturnover = Helper.SeparateNumricByThousand($('#txtTurnover').val());
        if (maxturnover != "") {
            $('#txtTurnover').val(formatNumber2(maxturnover));
        }
    }
}

