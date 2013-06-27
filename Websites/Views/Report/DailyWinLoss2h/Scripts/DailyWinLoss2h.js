

var DailyWinLoss2hManager = {
    isFirstLoad: true,
    reportDate: null,
    applyVirtualRateStr: '',
    initGrid: function () {
        DailyWinLoss2hManager.topLocation();
        $("#btnReport").click(function () {
            DailyWinLoss2hManager.viewReport();
        });
        $("#btnExport").click(function () {
            DailyWinLoss2hManager.exportToExcel();
        });

        DailyWinLoss2hManager.setupCalendar('txtfromdate', today);
        var strtoday = '<input type="button" onclick="DailyWinLoss2hManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="DailyWinLoss2hManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);

        DailyWinLoss2hManager.me = $("#dailyWinLossReport");
        DailyWinLoss2hManager.tb = $("#dailyWinLossReportContent");
        DailyWinLoss2hManager.getcontent(today);
    },
    reloadGrid: function (sportId, wldate) {
        var sportTypeId = sportId;
        var reportDate = wldate;
        $("#txtfromdate").datepicker("setDate", DailyWinLoss2hManager.reportDate);
        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");
        $("#btnReport").click(function () {
            DailyWinLoss2hManager.viewReport();
        });
        $("#btnExport").click(function () {
            DailyWinLoss2hManager.exportToExcel();
        });

        $('#spMoreElement').empty();
        var strtoday = '<input type="button" onclick="DailyWinLoss2hManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="DailyWinLoss2hManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);

        $("#dailyWinLossReport").removeClass("hidden");
        $("#dailyWLReportDiv").removeClass("hidden");
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },
    topLocation: function () {
        //window.location.href = window.location.href;
    },
    viewReport: function () {
        DailyWinLoss2hManager.getcontent($("#txtfromdate").val());
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                DailyWinLoss2hManager.getcontent($('#btnToday').val());
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                DailyWinLoss2hManager.getcontent($('#btnYesterday').val());
                break;
            default:
                break;
        }
    },
    exportToExcel: function () {
        if (!isAllowRead) {
            return;
        }
        var url = 'DailyWinLoss2h?ExportType=MainReport' + '&ReportDate=' +
                    DailyWinLoss2hManager.reportDate;
        $('#frm').attr('src', url);

    },
    isFirstSetUpCalendar: true,
    isCalendarOpen: false,
    setupCalendar: function (id, strdatetime) {
        if (DailyWinLoss2hManager.isFirstSetUpCalendar) {
            var limitDate = {
                minDate: '1/01/2011',
                maxDate: today
            };
            $('#txtfromdate').click(function () {
                if (DailyWinLoss2hManager.isCalendarOpen) {
                    $('#txtfromdate').datepicker("hide");
                    DailyWinLoss2hManager.isCalendarOpen = false;
                } else {
                    $('#txtfromdate').datepicker("show");
                    DailyWinLoss2hManager.isCalendarOpen = true;
                }
            });
            DailyWinLoss2hManager.renderCalendar('#txtfromdate', limitDate);
            DailyWinLoss2hManager.isFirstSetUpCalendar = false;
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
            },
            onClose: function () {
                DailyWinLoss2hManager.isCalendarOpen = false;
            }
        };
        var new_opts = $.extend(true, defaults, opts);
        var dates = $(ids).datepicker(new_opts);
        $(ids).datepicker("setDate", new Date(opts.maxDate));
    },
    getcontent: function (wldate) {
        if (!isAllowRead) {
            return;
        }

        DailyWinLoss2hManager.reportDate = wldate;
        var data = {
            param: { ReportDate: wldate }
        };

        try { parent.BlockUI(); } catch (e) { }
        document.body.style.cursor = 'wait';
        DailyWinLoss2hManager.disableReportButton();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2h/DailyWLDirect_Sporttype",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }

                DailyWinLoss2hManager.me.addClass('hidden');
                DailyWinLoss2hManager.renderDailyWLReport(rs, wldate);
            },
            error: function () {
                DailyWinLoss2hManager.renderNoData();
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                document.body.style.cursor = 'default';
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        if (dailyWLResult && dailyWLResult.ReportDetails && dailyWLResult.ReportDetails.length > 0) {
            isBARMActive = dailyWLResult.IsBARMActive.toString();
            var arrBody = [];
            var arrFooter = [];
            var odd_even = "";
            var items = dailyWLResult.ReportDetails;
            var isChange_ApplyVirtualRateType = false;
            for (var i = 0; i < items.length; i++) {
                var info = items[i];
                if (info.RowTypeDWL2h.Id == 0 &&
                    !info.IsApplyVirtualRate &&
                    !isChange_ApplyVirtualRateType) {
                    arrBody.push(DailyWinLoss2hManager.renderAllTotalRF(dailyWLResult.ReportTotal.NormalTotal));
                    if (isEightRMActive == 'true' &&
                        dailyWLResult.ReportTotal.SoccerEightRMTotal &&
                        dailyWLResult.ReportTotal.SoccerEightRMTotal.LineTotalVirtual != 0) {
                        arrBody.push(DailyWinLoss2hManager.eightrmFooter(dailyWLResult.ReportTotal.SoccerEightRMTotal));
                    }

                    if (isBARMActive == 'true' &&
                        dailyWLResult.ReportTotal.SoccerBARMTotal &&
                        dailyWLResult.ReportTotal.SoccerBARMTotal.LineTotalVirtual != 0) {
                        arrBody.push(DailyWinLoss2hManager.barmFooter(dailyWLResult.ReportTotal.SoccerBARMTotal));
                    }

                    arrBody.push(DailyWinLoss2hManager.emptyRowHtml());
                }

                var nextInfoIndex = i < items.length - 1 ? i + 1 : i;
                var nextInfo = items[nextInfoIndex];
                if (info.RowTypeDWL2h.Id != 9 ||
                    isChange_ApplyVirtualRateType ||
                    nextInfo.IsApplyVirtualRate) {
                    arrBody.push(DailyWinLoss2hManager.renderDailyWLRow(info, wldate));
                }

                if (info.RowTypeDWL2h.Id == 0 &&
                    !info.IsApplyVirtualRate &&
                    !isChange_ApplyVirtualRateType) {
                    isChange_ApplyVirtualRateType = true;
                }
            }

            arrFooter.push(DailyWinLoss2hManager.renderReportFooter(dailyWLResult));

            var bodyHtml = arrBody.join('');
            bodyHtml = stringformat.removeSpaceIE9(bodyHtml);
            DailyWinLoss2hManager.tb.find("tr:not(.header)").remove();
            DailyWinLoss2hManager.tb.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = stringformat.removeSpaceIE9(footerHtml);
            DailyWinLoss2hManager.tb.append(footerHtml);
            DailyWinLoss2hManager.createScroll();
        }
        else {
            DailyWinLoss2hManager.renderNoData();
        }
    },
    createScroll: function () {
        DailyWinLoss2hManager.me.removeClass('hidden');
    },
    renderDailyWLRow: function (dwlRow, wldate) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = DailyWinLoss2hManager.normalRowHtml(dwlRow, wldate);
                break;
            case 1:
                if (dwlRow.Id != 1) {
                    html = DailyWinLoss2hManager.eightrmRowHtml(dwlRow, wldate);
                }
                break;
            case 2:
                if (dwlRow.Id != 1) {
                    html = DailyWinLoss2hManager.barmRowHtml(dwlRow, wldate);
                }
                break;
            case 3:
                //                if (dwlRow.Id != 1) {
                //                    html = DailyWinLoss2hManager.sportTotalRowHtml(dwlRow, wldate);
                //                }
                break;
            case 9:
                html = DailyWinLoss2hManager.emptyRowHtml(dwlRow, wldate);
                break;
        }
        return html;
    },
    normalRowHtml: function (dwlRow, wldate) {
        var html = "";
        if (dwlRow) {
            html = '<tr class="' + (dwlRow.IsApplyVirtualRate == true ? "evenrow" : "") + '" >\
                        <td>\
                            <div class="col160 b l">' + DailyWinLoss2hManager.getSportLink(dwlRow, wldate) + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r">' + StringFormatter.getDisplayColorOfValue(dwlRow.WinLoss) + '</div>\
                        </td>\
                        <td>\
                            <div class="col90 r">' + StringFormatter.formatNumber(dwlRow.Position) + '</div>\
                        </td>\
                         <td>\
                            <div class="col80 r">' + StringFormatter.formatNumber(dwlRow.Commission) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotal) + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r">' + (dwlRow.IsApplyVirtualRate ? StringFormatter.getDisplayColorOfValue(dwlRow.LineTotalVirtual) : "N/A") + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r">' + (dwlRow.IsApplyVirtualRate ? StringFormatter.getDisplayColorOfValue(dwlRow.Variance) : "N/A") + '</div>\
                         </td>\
                         <td>\
                            <div class="col100 r">' + StringFormatter.formatNumber(dwlRow.Turnover) + '</div>\
                         </td>\
                         <td>\
                            <div class="col100 r">' + (dwlRow.IsApplyVirtualRate ? StringFormatter.formatNumber(dwlRow.TurnoverVirtual) : "N/A") + '</div>\
                         </td>\
                         <td>\
                            <div class="col80 r ' + (dwlRow.Turnover.toFixed(2) != 0 && dwlRow.Margin.toFixed(2) > 0 ? 'red' : '') + '">' +
                                (dwlRow.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(dwlRow.Margin) + '%' : 'N/A') + '</div>\
                         </td>\
                         <td>\
                            <div class="col80 r ' + (dwlRow.TurnoverVirtual.toFixed(2) != 0 && dwlRow.MarginVirtual.toFixed(2) > 0 ? 'red' : '') + '">' +
                                (dwlRow.TurnoverVirtual.toFixed(2) != 0 ? StringFormatter.formatNumber(dwlRow.MarginVirtual) + '%' : 'N/A') + '</div>\
                         </td>\
                    </tr>';
        }
        return html;
    },
    eightrmRowHtml: function (dwlRow, wldate) {
        var html = "";
        if (dwlRow) {
            html = '<tr style="height:18px;" align="right">\
                                <td align="left" class="longtext-wrapper">' + dwlRow.AliasName + '</td>\
                                <td colspan="3"></td>\
                                <td nowrap class="longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotal) + '</td>\
                                 <td nowrap class="longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotalVirtual) + '</td>\
                               <td colspan="5"></td>\
                   </tr>';
        }
        return html;
    },
    barmRowHtml: function (dwlRow, wldate) {
        var html = "";
        if (dwlRow) {
            html = '<tr style="height:18px;" align="right">\
                                <td align="left" class="longtext-wrapper">' + dwlRow.AliasName + '</td>\
                                <td colspan="3"></td>\
                                <td nowrap class="longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotal) + '</td>\
                                 <td nowrap class="longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotalVirtual) + '</td>\
                               <td colspan="5"></td>\
                   </tr>';
        }
        return html;
    },
    sportTotalRowHtml: function (dwlRow, wldate) {
        var html = "";
        if (dwlRow) {
            html = '<tr style="height:18px;" align="right">\
                                <td>' + dwlRow.Name + '</td>\
                                <td colspan="3"></td>\
                                <td nowrap class="longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotal) + '</td>\
                                <td nowrap class="longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotalVirtual) + '</td>\
                                <td colspan="5"></td>\
                   </tr>';
        }
        return html;
    },
    emptyRowHtml: function (dwlRow, wldate) {
        var html = '<tr>\
                        <td colspan ="11" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    getSportLink: function (dwlRow, wldate) {
        var sportLink = dwlRow.Name;
        switch (dwlRow.Id) {
            case 1: //Soccer
                if (isAllowReadSoccerReport) {
                    sportLink = "<a onclick=\"WL2hShowHideDivManager.showSoccerReport('" +
                            dwlRow.Id + "','" + wldate + "'); return;\">" +
                            dwlRow.Name + "</a>";
                }

                break;
            case 151: //HorseRacing
                if (isAllowReadHorseRacingReport) {
                    sportLink = "<a onclick=\"WL2hShowHideDivManager.showHorseRacingReport('" +
                            dwlRow.Id + "','" + wldate + "'); return;\">" +
                            dwlRow.Name + "</a>";
                }

                break;
            case 201: //FI
                sportLink = dwlRow.Name;
                //                sportLink = "<a onclick=\"WL2hShowHideDivManager.showFinanceReport('" +
                //                                dwlRow.Id + "','" + wldate + "'); return;\">" +
                //                                dwlRow.Name + "</a>";
                break;
            case 251: //Casino
                if (isAllowReadCasinoReport) {
                    sportLink = "<a onclick=\"WL2hShowHideDivManager.showCasinoReport('" +
                            dwlRow.Id + "','" + wldate + "'); return;\">" +
                            dwlRow.Name + "</a>";
                }

                break;
            case 252: //Bingo
                if (isAllowReadBingoReport) {
                    sportLink = "<a onclick=\"WL2hShowHideDivManager.showBingoReport('" +
                            dwlRow.Id + "','" + wldate + "'); return;\">" +
                            dwlRow.Name + "</a>";
                }
                break;
            case 99: //MixParlay
                if (isAllowReadMixParlayReport) {
                    var reportDate = new Date(wldate);
                    var settingDate = new Date(dailyWLMP_WebSettings);
                    if (reportDate < settingDate) {
                        sportLink = "<a onclick=\"WL2hShowHideDivManager.showMixParlayReport('" +
                            dwlRow.Id + "','" + wldate + "'); return;\">" +
                            dwlRow.Name + "</a>";
                    }
                    else {
                        sportLink = "<a onclick=\"WL2hShowHideDivManager.showAdvanceMixParlayReport('" +
                            dwlRow.Id + "','" + wldate + "'); return;\">" +
                            dwlRow.Name + "</a>";
                    }
                }
                break;
            case 98: //CombinationMixParlay
                if (isAllowReadCombinationMixParlayReport && new Date(balanceUpDay) <= new Date(wldate)) {
                    sportLink = "<a onclick=\"WL2hShowHideDivManager.showCombinationMixParlay('" +
                            dwlRow.Id + "','" + wldate + "'); return;\">" +
                            dwlRow.Name + "</a>";
                }
                else {
                    sportLink = dwlRow.Name;
                }
                break;
            case 2: //OtherSports
                if (isAllowReadOtherSportsReport) {
                    sportLink = "<a onclick=\"WL2hShowHideDivManager.showOtherSportsReport('" +
                            dwlRow.Id + "','" + wldate + "'); return;\">" +
                            dwlRow.Name + "</a>";
                }
                break;
            case 161: //NumberGame
                if (isAllowReadNumberGameReport) {
                    sportLink = "<a onclick=\"WL2hShowHideDivManager.showNumberGameReport('" +
                            dwlRow.Id + "','" + wldate + "'); return;\">" +
                            dwlRow.Name + "</a>";
                }
                break;
            case 162: //LiveCasino
                if (isAllowReadLiveCasinoReport) {
                    sportLink = "<a onclick=\"WL2hShowHideDivManager.showLiveCasinoReport('" +
                            dwlRow.Id + "','" + wldate + "'); return;\">" +
                            dwlRow.Name + "</a>";
                }
                break;
            case 180: //Virtual Soccer
            case 181: //Virtual Horse Racing
            case 186: //Virtual Tennis
                if (isAllowReadVirtualSportsReport) {
                    sportLink = "<a onclick=\"WL2hShowHideDivManager.showVirtualSportsReport('" +
                            dwlRow.Id + "','" + wldate + "'); return;\">" +
                            dwlRow.Name + "</a>";
                }
                break;
            default:
                sportLink = dwlRow.Name;
                break;
        }
        return sportLink;
    },
    renderNoData: function () {
        DailyWinLoss2hManager.tb.find("tr:not(.header)").remove();
        DailyWinLoss2hManager.tb.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="9" class="b c" >There is no information available.</td></tr>');
        DailyWinLoss2hManager.me.removeClass('hidden');
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },
    eightrmFooter: function (dwlRow, wldate) {
        var html = "";
        if (dwlRow) {
            html = '<tr style="height:18px;" align="right" class="evenrow">\
                                <td class="longtext-wrapper r b">' + dwlRow.AliasName + '</td>\
                                <td colspan="3"></td>\
                                <td nowrap class="longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotal) + '</td>\
                                <td nowrap class="longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotalVirtual) + '</td>\
                                <td colspan="5"></td>\
                   </tr>';
        }
        return html;
    },
    barmFooter: function (dwlRow, wldate) {
        var html = "";
        if (dwlRow) {
            html = '<tr style="height:18px;" align="right" class="evenrow">\
                                <td class="longtext-wrapper r b">' + dwlRow.AliasName + '</td>\
                                <td colspan="3"></td>\
                                <td nowrap class="longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotal) + '</td>\
                                <td nowrap class="longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotalVirtual) + '</td>\
                                <td colspan="5"></td>\
                   </tr>';
        }
        return html;
    },
    renderReportFooter: function (dailyWLReport) {
        var arr = [];     
        arr.push(DailyWinLoss2hManager.renderGrandTotalRF(dailyWLReport.ReportTotal.GrandTotal));
        arr.push(DailyWinLoss2hManager.renderCommentRF());
        return arr.join('');
    },
    renderAllTotalRF: function (total) {
        var html;
        if (total) {
            html = '<tr class="evenrow">\
                        <td>\
                            <div class="col160 r b">Virtual All Total:</div>\
                        </td>\
                        <td>\
                            <div class="col100 r" style="font-weight: normal;">' + StringFormatter.getDisplayColorOfValue(total.WinLoss) + '</div>\
                        </td>\
                        <td>\
                            <div class="col90 r" style="font-weight: normal;">' + StringFormatter.formatNumber(total.Position) + '</div>\
                        </td>\
                         <td>\
                            <div class="col80 r" style="font-weight: normal;">' + StringFormatter.formatNumber(total.Commission) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r b">' + StringFormatter.getDisplayColorOfValue(total.LineTotal) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r b">' + StringFormatter.getDisplayColorOfValue(total.LineTotalVirtual) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r b">' + StringFormatter.getDisplayColorOfValue(total.Variance) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r b">' + StringFormatter.formatNumber(total.Turnover) + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r b">' + StringFormatter.formatNumber(total.TurnoverVirtual) + '</div>\
                        </td>\
                        <td>\
                            <div class="col80 r ' + (total.Turnover.toFixed(2) != 0 && total.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' +
                                (total.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(total.Margin) + '%' : 'N/A') + '</div>\
                        </td>\
                        <td>\
                            <div class="col80 r ' + (total.TurnoverVirtual.toFixed(2) != 0 && total.MarginVirtual.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' +
                                (total.TurnoverVirtual.toFixed(2) != 0 ? StringFormatter.formatNumber(total.MarginVirtual) + '%' : 'N/A') + '</div>\
                        </td>\
                    </tr>';
        }
        return html;
    },
    renderTotalIBCRF: function (allIBCTotal) {
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

        var html = '<tr style="height:18px;" align="right">\
                                <td class="b">' + ibcMsg + '</td>\
                                <td colspan="3"></td>\
                                <td nowrap class="b longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(allIBCTotal) + '</td>\
                                <td nowrap class="b longtext-wrapper">' + StringFormatter.getDisplayColorOfValue(allIBCTotal) + '</td>\
                                <td colspan="5"></td>\
                   </tr>';
        return html;
    },
    renderGrandTotalRF: function (grandTotal) {
        var arr = [];
        if (grandTotal) {
            html = '<tr>\
                        <td>\
                            <div class="col160 r b">GRAND TOTAL:</div>\
                        </td>\
                        <td>\
                            <div class="col100 r"></div>\
                        </td>\
                        <td>\
                            <div class="col90 r"></div>\
                        </td>\
                        <td>\
                            <div class="col80 r"></div>\
                        </td>\
                         <td>\
                            <div class="col100 r" style="font-weight: normal;">' + StringFormatter.getDisplayColorOfValue(grandTotal.Real) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r" style="font-weight: normal;">' + StringFormatter.getDisplayColorOfValue(grandTotal.Virtual) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r" style="font-weight: normal;">' + StringFormatter.getDisplayColorOfValue(grandTotal.Variance) + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r"></div>\
                         </td>\
                        <td>\
                            <div class="col100 r"></div>\
                         </td>\
                         <td>\
                            <div class="col80 r"></div>\
                        </td>\
                         <td>\
                            <div class="col80 r"></div>\
                        </td>\
                    </tr>';
        }
        arr.push(html);
        return arr.join("");
    },
    renderCommentRF: function () {
        var footerMsg = "* Virtual All Total: Number Game + Mix Parlay + System Parlay + Other Sports + Soccer";
        footerMsg += "<br />";
        footerMsg += "* Grand Total (Virtual): Virtual All Total + 8RM + BARM + Virtual Sports + Racing + Casino + Live Casino + Bingo + P2P";

        var html = '<tr>\
                        <td id="tdNoInfomation" colspan="11" class="l" style="background-color: #E7EAEA;" >\
                            <div style="font-weight: bold;padding: 20px 0 15px;color: #565656;">' + footerMsg + '</div>\
                        </td>\
                    </tr>';
        return html;
    },
    openSportDetail: function (sportId, wldate) {
        //        alert(sportId + " " + wldate);
    },
    disableReportButton: function () {
        $("#txtfromdate").datepicker('disable');
        $('#btnReport').attr('disabled', true);
        $('#btnToday').attr('disabled', true);
        $('#btnYesterday').attr('disabled', true);
        $('#btnExport').attr('disabled', true);
    },
    enableReportButton: function () {
        $('#btnReport').removeAttr('disabled');
        $('#btnToday').removeAttr('disabled');
        $('#btnYesterday').removeAttr('disabled');
        $('#btnExport').removeAttr('disabled');
        $("#txtfromdate").datepicker('enable');
    }
}

$(document).ready(function () {
    if (DailyWinLoss2hManager.isFirstLoad) {
        DailyWinLoss2hManager.initGrid();
        DailyWinLoss2hManager.isFirstLoad = false;
    }
}); 