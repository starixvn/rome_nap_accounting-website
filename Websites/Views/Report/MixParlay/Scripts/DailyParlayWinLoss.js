var isInitedMonthlyCalendar = false;
$(document).ready(function () {
    parent.$('#nav li').removeClass('mcurrent');
    parent.$('#nav #pId2').addClass('mcurrent');
    helper.renderCalendarFromTo('#txtfromdate,#txttodate');
    $('#btnReport_today').val(currentDay);
    $('#btnReport_yesterday').val(yesterday);
    $("#txtTitle").html('Data is available ' + movebetday + ' onward. For more historical data, please click to ');
    if (isredirect == "True") {
        $('#pId0').removeClass('mcurrent');
        $('#pId2').addClass('mcurrent');
        $('#txtCurrentReport').val('Product');
        $('#spParlayCombinationWL').removeClass('on').addClass('off');
        $('#spParalyWinLossByProduct').removeClass('off').addClass('on');
        var isViewDW = false;

        if (new Date(fromdate) < new Date(movebetday)) {
            $('#tblDate1').hide();
            $('#tblDate2').show();
            if (!isInitedMonthlyCalendar) {
                isInitedMonthlyCalendar = true;
                $('#spMonth').monPickr({
                    today: today,
                    minDate: '01/01/2011',
                    maxDate: beforeyesterday,
                    showDateRange: true,
                    fromDayId: 'spFrom',
                    toDayId: 'spTo',
                    log: function (msg) {
                        console.warn(msg);
                    }
                });
                $('#spMonth').monPickr("setFromDay", new Date(fromdate).getDate());
            }
            isViewDW = true;
        }
        else {
            helper.renderCalendarFromTo('#txtfromdate,#txttodate');
            $('#tblDate1').show();
            $('#tblDate2').hide();
        }
        helper.callReport(null, isViewDW);
    }
});

var helper = {
    renderCalendarFromTo: function (ids) {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     minDate
                date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
            }
        };
        var dates = $(ids).datepicker(defaults);
        var dt = new Date();
        var fdate = $(ids.split(',')[0]);
        var tdate = $(ids.split(',')[1]);

        fdate.datepicker("option", "minDate", movebetday);
        fdate.datepicker("option", "maxDate", dt);

        tdate.datepicker("option", "minDate", movebetday);
        tdate.datepicker("option", "maxDate", dt);

        fdate.datepicker("setDate", fromdate);
        tdate.datepicker("setDate", today);


    },
    initCalendar: function (yesterday) {
        $('#spMonth').monPickr({
            today: yesterday,
            minDate: '01/01/2011',
            showDateRange: true,
            fromDayId: 'spFrom',
            toDayId: 'spTo',
            log: function (msg) {
                console.warn(msg);
            }
        });

    },
    format: function (num) {
        if (num == null)
            return '';
        var n = num;
        var IsMargin = num.indexOf('%') > 0;
        n = num.replace('%', '');
        if (parseFloat(n) > 0) {
            return '<span style="color:red">' + n + (IsMargin ? '%' : '') + '</span>'
        }
        return num;
    },

    getbgColor: function (name) {
        var color = '';
        if (name.indexOf('2X') > -1) {
            color = 'c2x';
        }
        else if (name.indexOf('3X') > -1) {
            color = 'c3x';
        }
        else if (name.indexOf('4X') > -1) {
            color = 'c4x';
        }
        else if (name.indexOf('5X') > -1) {
            color = 'c5x';
        }
        else if (name.indexOf('6X') > -1) {
            color = 'c6x';
        }
        else if (name == 'Live') {
            color = 'bgLive';
        }
        else if (name == 'Non Live') {
            color = 'bgNoneLive';
        }
        else if (name == 'Mix Live and Non Live') {
            color = 'bgMixLive';
        }
        return color;
    },

    changelink: function (obj) {
        if ($(obj).hasClass('on'))
            return;
        $('.menu').each(function (i) {
            if (this.className.indexOf('on') > -1) {
                this.className = 'off menu';
            }
            else {
                this.className = 'on menu';
            }
        });

        var value = $(obj).attr('value');
        if (value == 'Combination') {
            $('#AdvanceParlayWL').addClass('hide');
        }
        else {
            $('#DailyParlayWL').addClass('hide');
        }
        $('#txtCurrentReport').val(value);

    },

    callReport: function (obj, isViewDW) {
        var value = $('#txtCurrentReport').val();
        switch (value) {
            case 'Combination':
                $('#DailyParlayWL').removeClass('hide');
                $('#AdvanceParlayWL').addClass('hide');
                dailyParlayWinLoss.dailyParlayWinLoss(obj, isViewDW);
                break;
            case 'Product':
                $('#DailyParlayWL').addClass('hide');
                $('#AdvanceParlayWL').removeClass('hide');
                advanceParlayWL.advanceParlayWinLoss(obj, isViewDW);
                break;
        }
        var fromdate = obj == null ? $('#txtfromdate').val() : obj.value;
        var todate = obj == null ? $('#txttodate').val() : obj.value;
        if (isViewDW) {
            fromdate = $('#spMonth').monPickr("getFromDate", "MM/dd/yyyy");
            todate = $('#spMonth').monPickr("getToDate", "MM/dd/yyyy");
        }
        $('#txtfromdate').val(fromdate);
        $('#txttodate').val(todate);
        helper.setDateTitle(fromdate, todate);
    },

    setDateTitle: function (fromdate, todate) {
        $('#spDateTitleAdvanceParlayWL').html('From ' + fromdate + ' To ' + todate);
        $('#spDateTitleDailyParlayWL').html('From ' + fromdate + ' To ' + todate);
    }
};

var dailyParlayWinLoss = {
    fromDate: '',
    toDate: '',
    isViewDW: false,
    dailyParlayWinLoss: function (obj, isViewDW) {
        dailyParlayWinLoss.fromDate = obj == null ? $('#txtfromdate').val() : obj.value;
        dailyParlayWinLoss.toDate = obj == null ? $('#txttodate').val() : obj.value;
        dailyParlayWinLoss.isViewDW = isViewDW;
        if (isViewDW) {
            dailyParlayWinLoss.fromDate = $('#spMonth').monPickr("getFromDate", "MM/dd/yyyy");
            dailyParlayWinLoss.toDate = $('#spMonth').monPickr("getToDate", "MM/dd/yyyy");
        }
        var param = {
            fromDate: dailyParlayWinLoss.fromDate,
            toDate: dailyParlayWinLoss.toDate,
            isViewDW: dailyParlayWinLoss.isViewDW
        };
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: 'SelDailyParlayWinLoss',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                dailyParlayWinLoss.renderReport(rs);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    renderReport: function (rs) {
        $("#bodyDailyParlayWL").html(dailyParlayWinLoss.genBody(rs));
        $('#footerDailyParlayWL').html(dailyParlayWinLoss.genFooter(rs));
        $("#DailyParlayWL").reRenderScroller(1215, 640, true, false);
        $('#mainreport').removeClass('hide');
    },

    assignData: function (className, row) {
        var html = [];

        html.push('<td class="col100" style="height: 17px;"><div class="' + className + '">' + row.Name + '</div></td>')
        html.push('<td class="col100"><div class="r">' + helper.format(row.WinLoss) + '</div></td>');
        html.push('<td><div class="col100 r">' + row.MemberTurnover + '</div></td>');
        html.push('<td class="col100"><div class="r">' + row.BetCount + '</div></td>');
        html.push('<td class="col100"><div class="r">' + row.Position + '</div></td>');
        html.push('<td class="col100"><div class="r">' + row.Comm + '</div></td>');
        html.push('<td class="col100"><div class="r">' + helper.format(row.LineTotal) + '</div></td>');
        html.push('<td class="col100"><div class="r">' + row.Turnover + '</div></td>');
        html.push('<td class="col100"><div class="r">' + helper.format(row.Margin) + '</div></td>');

        return html.join('');
    },


    genBody: function (rs) {
        var html = [];
        var cntCombinations = rs.Rows.length;
        if (cntCombinations > 0) {
            for (var i = 0; i < cntCombinations; i++) {//Combinations
                var rowCombinations = rs.Rows[i];

                var cntLiveTypes = rowCombinations.Rows.length;
                for (var j = 0; j < cntLiveTypes; j++) {//Live Types
                    var rowLiveType = rowCombinations.Rows[j];


                    var cntBettypes = rowLiveType.Rows.length;
                    for (var k = 0; k < cntBettypes; k++) {//Bettype
                        var rowBettype = rowLiveType.Rows[k];
                        html.push('<tr>');

                        if (j == 0 && k == 0) {
                            html.push('<td class="c b col140 ' + helper.getbgColor(rowCombinations.ParentName) + '" rowspan="' + (rowCombinations.rowspan + 1) + '">' + rowCombinations.ParentName + '</td>');
                        }

                        if (k == 0) {
                            html.push('<td class="c b col140" rowspan="' + (rowLiveType.rowspan + 1) + '">' + rowLiveType.ParentName + '</td>');
                        }

                        html.push(dailyParlayWinLoss.assignData('l', rowBettype));
                        html.push('</tr>');
                    }

                    //Sub Total
                    html.push('<tr class="bgSubTotal">');
                    html.push(dailyParlayWinLoss.assignData('c', rowLiveType));
                    html.push('</tr>');
                }

                // Total Combination
                html.push('<tr class="bgCombinationTotal">');
                html.push('<td></td>');
                html.push(dailyParlayWinLoss.assignData('c', rowCombinations));
                html.push('</tr>');
            }

        }


        return html.join('');
    },

    genFooter: function (row) {
        var html = [];
        if (row != null) {
            html.push('<tr><td colspan="3" style="width:386px" class="c"><div class="bgGrandTotal">Grand Total</div></td>');
            html.push('<td class="col100"><div class="r bgGrandTotal">' + helper.format(row.WinLoss) + '</div></td>');
            html.push('<td><div class="col100 r bgGrandTotal">' + row.MemberTurnover + '</div></td>');
            html.push('<td class="col100"><div class="r bgGrandTotal">' + row.BetCount + '</div></td>');
            html.push('<td class="col100"><div class="r bgGrandTotal">' + row.Position + '</div></td>');
            html.push('<td class="col100"><div class="r bgGrandTotal">' + row.Comm + '</div></td>');
            html.push('<td class="col100"><div class="r bgLineTotal">' + helper.format(row.LineTotal) + '</div></td>');
            html.push('<td class="col100"><div class="r bgGrandTotal">' + row.Turnover + '</div></td>');
            html.push('<td class="col100"><div class="r bgGrandTotal">' + helper.format(row.Margin) + '</div></td></tr>');
        }
        return html.join('');
    },

    exportExcel: function () {
        var url = 'DailyParlayWinLoss?IsExport=1&reportType=dailyParlayWinLoss';
        url += '&fromDate=' + dailyParlayWinLoss.fromDate + '&toDate=' + dailyParlayWinLoss.toDate + '&isViewDW=' + dailyParlayWinLoss.isViewDW;
        $('#frm').attr('src', url);
    }
};

var advanceParlayWL = {
    fromDate: '',
    toDate: '',
    isViewDW: false,
    advanceParlayWinLoss: function (obj, isViewDW) {
        advanceParlayWL.fromDate = obj == null ? $('#txtfromdate').val() : obj.value;
        advanceParlayWL.toDate = obj == null ? $('#txttodate').val() : obj.value;
        advanceParlayWL.isViewDW = isViewDW
        if (advanceParlayWL.isViewDW) {
            advanceParlayWL.fromDate = $('#spMonth').monPickr("getFromDate", "MM/dd/yyyy");
            advanceParlayWL.toDate = $('#spMonth').monPickr("getToDate", "MM/dd/yyyy");
        }

        var param = {
            fromDate: advanceParlayWL.fromDate,
            toDate: advanceParlayWL.toDate,
            isViewDW: advanceParlayWL.isViewDW
        };

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: 'SelAdvanceParlayWinLoss',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                advanceParlayWL.renderReport(rs);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    renderReport: function (rs) {
        $("#bodyAdvanceParlayWL").html(advanceParlayWL.genBody(rs));
        $('#footerAdvanceParlayWL').html(advanceParlayWL.genFooter(rs));
        $("#AdvanceParlayWL").reRenderScroller(1010, 640, true, false);
        $('#AdvanceParlayWL').removeClass('hide');

    },

    assignData: function (className, row) {
        var html = [];

        html.push('<td class="col100" style="height: 17px;"><div class="' + className + '">' + row.Name + '</div></td>')
        html.push('<td class="col100"><div class="r">' + helper.format(row.WinLoss) + '</div></td>');
        html.push('<td class="col100"><div class="r">' + row.Position + '</div></td>');
        html.push('<td class="col100"><div class="r">' + row.Comm + '</div></td>');
        html.push('<td class="col100"><div class="r">' + helper.format(row.LineTotal) + '</div></td>');
        html.push('<td class="col100"><div class="r">' + row.Turnover + '</div></td>');
        html.push('<td class="col100"><div class="r">' + helper.format(row.Margin) + '</div></td>');

        return html.join('');
    },

    genBody: function (rs) {
        var html = [];

        var cntLiveType = rs.Rows.length;
        if (cntLiveType > 0) {
            for (var i = 0; i < cntLiveType; i++) {//LiveType
                var rowLiveType = rs.Rows[i];

                var cntSportType = rowLiveType.Rows.length;
                for (var j = 0; j < cntSportType; j++) {//Sport Type
                    var rowSportType = rowLiveType.Rows[j];


                    var cntBettype = rowSportType.Rows.length;
                    for (var k = 0; k < cntBettype; k++) {//Bettype
                        var rowBettype = rowSportType.Rows[k];
                        html.push('<tr>');

                        if (j == 0 && k == 0) {
                            html.push('<td class="c b col140 ' + helper.getbgColor(rowLiveType.ParentName) + '" rowspan="' + (rowLiveType.rowspan + 1) + '">' + rowLiveType.ParentName + '</td>');
                        }

                        if (k == 0) {
                            html.push('<td class="c b col140" rowspan="' + (rowSportType.rowspan + 1) + '">' + rowSportType.ParentName + '</td>');
                        }

                        html.push(advanceParlayWL.assignData('l', rowBettype));
                        html.push('</tr>');
                    }

                    //Sub Total Sport
                    html.push('<tr class="bgSubTotal">');
                    html.push(advanceParlayWL.assignData('c', rowSportType));
                    html.push('</tr>');
                }

                // Total Live Type
                html.push('<tr class="bgCombinationTotal">');
                html.push('<td></td>');
                html.push(advanceParlayWL.assignData('c', rowLiveType));
                html.push('</tr>');
            }

        }


        return html.join('');
    },

    genFooter: function (row) {
        var html = [];
        if (row != null) {
            html.push('<tr><td colspan="3" style="width:386px" class="c"><div class="bgGrandTotal">Grand Total</div></td>');
            html.push('<td class="col100"><div class="r bgGrandTotal">' + helper.format(row.WinLoss) + '</div></td>');
            html.push('<td class="col100"><div class="r bgGrandTotal">' + row.Position + '</div></td>');
            html.push('<td class="col100"><div class="r bgGrandTotal">' + row.Comm + '</div></td>');
            html.push('<td class="col100"><div class="r bgLineTotal">' + helper.format(row.LineTotal) + '</div></td>');
            html.push('<td class="col100"><div class="r bgGrandTotal">' + row.Turnover + '</div></td>');
            html.push('<td class="col100"><div class="r bgGrandTotal">' + helper.format(row.Margin) + '</div></td></tr>');
        }
        return html.join('');
    },

    exportExcel: function () {
        var url = 'DailyParlayWinLoss?IsExport=1&reportType=advanceParlayWinLoss';
        url += '&fromDate=' + advanceParlayWL.fromDate + '&toDate=' + advanceParlayWL.toDate + '&isViewDW=' + advanceParlayWL.isViewDW;
        $('#frm').attr('src', url);
    }
};
function SwitchCalendarType(type) {
    $('#AdvanceParlayWL').addClass('hide');
    $('#DailyParlayWL').addClass('hide');
    switch (type) {
        case 1:
            $('#tblDate1').hide();
            $('#tblDate2').show();
            if (!isInitedMonthlyCalendar) {
                isInitedMonthlyCalendar = true;
                helper.initCalendar(beforeyesterday);
            }
            break;
        case 2:
            $('#tblDate2').hide();
            $('#tblDate1').show();
            fromdate = currentDay;
            today = currentDay;
            helper.renderCalendarFromTo('#txtfromdate,#txttodate');

            break;
    }
}