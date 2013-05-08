var numbergameMonthlyWinloss = {
    fromDate: '',
    toDate: '',
    title: '',
    gridOpts: {
        tblContentId: 'tblContent',
        isShowPager: false,
        manualCopyHeader: true,
        fullWidth: true,
        fullHeight: true,
        gridHeight: 100,
        freezeWidth: 436,
        numberFreezeCol: 4
    },
    init: function () {
        this.renderCalendar("#txtfromdate, #txttodate");
        //#region assign events
        $('#btnReport').on('click', function () {
            numbergameMonthlyWinloss.getWinLossReport();
        });
        
        //#endregion
    },
    renderCalendar: function (ids) {
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

        var dates = $(ids).datepicker(defaults);
        var dt = new Date();
        var fdate = $(ids.split(',')[0]);
        var tdate = $(ids.split(',')[1]);
        fdate.datepicker("setDate", fromdate);
        tdate.datepicker("setDate", todate);
    },
    getWinLossReport: function () {
        $('#err').hide();

        this.fromDate = $('#txtfromdate').val();
        this.toDate = $('#txttodate').val();
        this.title = 'Number Game Financial Report - From ' + this.fromDate + ' To ' + this.toDate;

        var params = 'fromDate=' + this.fromDate + '&toDate=' + this.toDate + '&reqTime=' + new Date().getMilliseconds();
        try { parent.BlockUI(); } catch (e) { };
        $.ajax({
            type: "GET",
            url: "../NumberGameMonthlyWinLoss/WinLossPage/?" + params,
            dataType: "html",
            success: function (evt) {
                $('#mainreport').html(evt);
            },
            error: function (req, status, error) {
                numbergameMonthlyWinloss.showErrorMessage(error);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderWinLossReport: function () {
        $('#reporttile').html(this.title);

        var arrFrozen = [];
        var arrScroll = [];

        var totalGroupByDate = wlDetail.length;
        for (var i = 0; i < totalGroupByDate; i++) {
            var dateInfo = wlDetail[i];
            //group by date
            var groupByDateId = 'groupByDateId' + i;
            arrFrozen.push(numbergameMonthlyWinloss.renderWlReportFrozenGroupByDateRow(dateInfo, groupByDateId, groupByDateId));
            arrScroll.push(numbergameMonthlyWinloss.renderWlReportScrollGroupByDateRow(dateInfo));
            //group by status
            var totalByStatus = dateInfo.WinLossDetail.length;
            for (var j = 0; j < totalByStatus; j++) {
                var statusInfo = dateInfo.WinLossDetail[j];
                var groupByStatusId = groupByDateId + '_' + j;
                var groupByStatusClass = groupByDateId + ' ' + groupByStatusId;

                arrFrozen.push(numbergameMonthlyWinloss.renderWlReportFrozenGroupByStatusRow(statusInfo, groupByStatusId, groupByStatusClass));
                arrScroll.push(numbergameMonthlyWinloss.renderWlReportScrollGroupByStatusRow(statusInfo, groupByDateId));

                var totalRows = statusInfo.WinLossDetail.length;
                for (var k = 0; k < totalRows; k++) {
                    var wlInfo = statusInfo.WinLossDetail[k];
                    arrFrozen.push(numbergameMonthlyWinloss.renderWlReportFrozenRow(wlInfo, groupByStatusClass));
                    arrScroll.push(numbergameMonthlyWinloss.renderWlReportScrollRow(wlInfo, groupByStatusClass));
                }
            }
        }

        var htmlScrollContent = arrScroll.join('');
        var htmlFrozenContent = arrFrozen.join('');
        if ($.browser.msie && parseInt($.browser.version, 10) === 9) {
            htmlScrollContent = numbergameMonthlyWinloss.removeSpecialCharactersForIE9(htmlScrollContent);
            htmlFrozenContent = numbergameMonthlyWinloss.removeSpecialCharactersForIE9(htmlFrozenContent);
        }
        $('#tblContent tbody').empty().append(htmlScrollContent);
        $('#tblContentHeader tbody').empty().append(htmlFrozenContent);

        var gridOpts = {
            tblContentId: 'tblContent',
            isShowPager: false,
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 100,
            freezeWidth: 436,
            numberFreezeCol: 4
        };
        $('#mainreportgrid').gridScroller(gridOpts);
    },
    renderWlReportFrozenGroupByDateRow: function (row, id, style) {

        var html = '<tr>\
                        <td><div class="l b" style="width: 140px; height: 17px"><span onclick="numbergameMonthlyWinloss.collapseExpandRows(\'' + id + '\',\'' + (style) + '\')" class="tree-expand" id="' + id + '"></span>' + row.Title + '</div></td>\
                        <td><div class="r b" style="width: 80px; height: 17px">' + row.NoPlayers + '</div></td>\
                        <td><div class="r b" style="width: 80px; height: 17px">' + row.NoGames + '</div></td>\
                        <td><div class="r b" style="width: 120px; height: 17px">' + row.TotalBets + '</div></td>\
                    </tr>';

        return html;
    },
    renderWlReportScrollGroupByDateRow: function (row) {
        var gameIncomeColor = numbergameMonthlyWinloss.negativeNumberColor(row.GameIncome);
        var html = '<tr>\
                        <td><div class="l" style="width: 140px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r" style="width: 80px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r" style="width: 80px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r" style="width: 120px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r b" style="width: 120px; height: 17px">' + row.TotalWins + '</div></td>\
                        <td><div class="r b ' + gameIncomeColor + '" style="width: 120px; height: 17px">' + row.GameIncome + '</div></td>\
                        <td><div class="r b ' + gameIncomeColor + '" style="width: 80px; height: 17px">' + row.Payout + '%</div></td>\
                    </tr>';

        return html;
    },
    renderWlReportFrozenGroupByStatusRow: function (row, id, style) {

        var html = '<tr class="' + (style.split(" ")[0]) + '">\
                        <td><div class="l" style="width: 140px; height: 17px">&nbsp;&nbsp;&nbsp;&nbsp;<span onclick="numbergameMonthlyWinloss.collapseExpandRows(\'' + id + '\',\'' + (style.split(" ")[1]) + '\')" class="tree-expand" id="' + id + '"></span>' + row.Title + '</div></td>\
                        <td><div class="r" style="width: 80px; height: 17px">' + row.NoPlayers + '</div></td>\
                        <td><div class="r" style="width: 80px; height: 17px">' + row.NoGames + '</div></td>\
                        <td><div class="r" style="width: 120px; height: 17px">' + row.TotalBets + '</div></td>\
                    </tr>';

        return html;
    },
    renderWlReportScrollGroupByStatusRow: function (row, style) {
        var gameIncomeColor = numbergameMonthlyWinloss.negativeNumberColor(row.GameIncome);
        var html = '<tr class="' + style + '">\
                        <td><div class="l" style="width: 140px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r" style="width: 80px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r" style="width: 80px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r" style="width: 120px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r" style="width: 120px; height: 17px">' + row.TotalWins + '</div></td>\
                        <td><div class="r ' + gameIncomeColor + '" style="width: 120px; height: 17px">' + row.GameIncome + '</div></td>\
                        <td><div class="r ' + gameIncomeColor + '" style="width: 80px; height: 17px">' + row.Payout + '%</div></td>\
                    </tr>';

        return html;
    },
    renderWlReportFrozenRow: function (row, style) {
        var html = '<tr class="' + style + '">\
                        <td><div class="l" style="width: 140px; height: 17px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + row.Title + '</div></td>\
                        <td><div class="r" style="width: 80px; height: 17px">' + row.NoPlayers + '</div></td>\
                        <td><div class="r" style="width: 80px; height: 17px">' + row.NoGames + '</div></td>\
                        <td><div class="r" style="width: 120px; height: 17px">' + row.TotalBets + '</div></td>\
                    </tr>';

        return html;
    },
    renderWlReportScrollRow: function (row, style) {
        var gameIncomeColor = numbergameMonthlyWinloss.negativeNumberColor(row.GameIncome);
        var html = '<tr class="' + style + '">\
                        <td><div class="l" style="width: 140px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r" style="width: 80px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r" style="width: 80px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r" style="width: 120px; height: 17px">&nbsp;</div></td>\
                        <td><div class="r" style="width: 120px; height: 17px">' + row.TotalWins + '</div></td>\
                        <td><div class="r ' + gameIncomeColor + '" style="width: 120px; height: 17px">' + row.GameIncome + '</div></td>\
                        <td><div class="r ' + gameIncomeColor + '" style="width: 80px; height: 17px">' + row.Payout + '%</div></td>\
                    </tr>';

        return html;
    },
    showErrorMessage: function (message) {
        $('#err').show();
        $('#err .alertboxerr div').html(message);
    },
    collapseExpandRows: function (parentId, childClass) {

        if ($('#' + parentId).hasClass('tree-expand')) {
            $('#' + parentId).removeClass('tree-expand').addClass('tree-collapse');
            $('#tblContent .' + childClass).addClass('hidden');
            $('#tblContentHeader .' + childClass).addClass('hidden');
        }
        else {
            $('#' + parentId).removeClass('tree-collapse').addClass('tree-expand');
            $('#tblContent .' + childClass).removeClass('hidden');
            $('#tblContentHeader .' + childClass).removeClass('hidden');

            if (parentId.split("_").length == 1) {
                $('#' + parentId + "_0").removeClass('tree-collapse').addClass('tree-expand');
                $('#' + parentId + "_1").removeClass('tree-collapse').addClass('tree-expand');
            }
        }

        $('#mainreportgrid').gridScroller(numbergameMonthlyWinloss.gridOpts);
    },
    negativeNumberColor: function (number) {
        if (parseFloat(number) < 0) {
            return 'red';
        }
        return '';
    },
    exportExcel: function () {
        var request = '/?fromDate=' + this.fromDate + '&toDate=' + this.toDate + '&reportTitle='+ this.title + '&reqTime=' + new Date().getMilliseconds();
        $("#frameExportExcel").attr("src", linkExportExcel + request);
    },
    removeSpecialCharactersForIE9: function (content) {
        var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
        var html = content.replace(expr, '><');
        return html;
    }
};

$(document).ready(function () {
    numbergameMonthlyWinloss.init();
});