var grid = {
    width: 1180,
    refundWidth: 100,
    noPlayerWidth: 90,
    currencyWidth: 90,
    totalCol: 12,
    getGridWidth: function (width, totalCol) {
        return width + totalCol * 3;
    },
    getObj: function () {
        return $("#gridgamestatistic");
    },
    getBody: function () {
        return grid.getObj().find("#tblbodygamestatistic");
    },
    getFooter: function () {
        return grid.getObj().find("#tblfootergamestatistic");
    },
    initScroller: function () {
        var gWidth = grid.width;
        var totalCol = grid.totalCol;

        var reportType = gameStatistics.params.reporttype;
        if (reportType == 2) {
            gWidth = gWidth - grid.noPlayerWidth;
            totalCol -= 1;
        }
        if (reportType == 6) {
            gWidth = gWidth - grid.currencyWidth;
            totalCol -= 1;
        }
        if (!($('#chkShowRefund').attr('checked'))) {
            gWidth = gWidth - grid.refundWidth;
            totalCol -= 1;
        }

        gameStatistics.initPager(gameStatistics.params.totalItems, gameStatistics.params.pagesize, gameStatistics.params.pageindex - 1);
        grid.getObj().removeClass("hidden").reRenderScroller(grid.getGridWidth(gWidth, totalCol), 500, true);
        $('.maincontent_box').css({ width: $('#gridgamestatistic').width() - 3 });
    }
};

var gameStatistics = {
    params: {
        fromdate: '',
        todate: '',
        movebetsday: '',
        balanceupday: '',
        username: '',
        excludetest: false,
        site: 0,
        gametype: 1101,
        gametable: '',
        currency: 0,
        reporttype: 3,
        betLess: '0',
        betMore: '0',
        winLess: '0',
        winMore: '0',
        totalItems: 0,
        pagesize: 100,
        pageindex: 1,
        fromcache: false,
        showrefund: false,
        sort: { SortType: '', SortCol: '', IsDecimal: false, IsDate: false }
    },
    filterInfo: [],
    defaultDate: '',
    specialReport: 0,
    specificPeriodIndex: 8,
    getReportData: function (fromCache) {
        $('#err').html('');

        gameStatistics.setParams();
        gameStatistics.params.fromcache = fromCache;

        if ((parseFloat(gameStatistics.params.betLess) < parseFloat(gameStatistics.params.betMore)) &&
            (gameStatistics.params.betLess.length > 0 && gameStatistics.params.betMore.length > 0)) {
            showAlertMessage("err", "The value of Total bet less than must greater than the value of Total bet more than!", 1);
            return false;
        }
        if ((parseFloat(gameStatistics.params.winLess) < parseFloat(gameStatistics.params.winMore)) &&
            (gameStatistics.params.winLess.length > 0 && gameStatistics.params.winMore.length > 0)) {
            showAlertMessage("err", "The value of Total win less than must greater than the value of Total win more than!", 1);
            return false;
        }

        try { parent.BlockUI(); } catch (e) { }
        //render report header
        $('#tbreportheader').html('');
        $('#tbreportheader').append(gameStatistics.renderHeaderRow(gameStatistics.params.reporttype));
        if (gameStatistics.params.reporttype == 0 || gameStatistics.params.reporttype == 1)
            gameStatistics.params.sort.IsDate = true;
        else
            gameStatistics.params.sort.IsDate = false;

        //get report data & assign to grid
        $.ajax({
            type: "POST",
            url: "Service/LiveCasinoReportService.svc/GetGameStatistic",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ dbparams: gameStatistics.params }),
            success: function (rs) {

                if (rs && rs.Items && rs.Items.length > 0) {

                    var arr = [];
                    var arrTotal = [];
                    var total = rs.Items.length;
                    gameStatistics.params.totalItems = rs.total;
                    //detail

                    if (gameStatistics.params.reporttype == 2 | gameStatistics.params.reporttype == 6) {
                        for (var i = 0; i < total; i++) {
                            var item = rs.Items[i];
                            arr.push(gameStatistics.renderDetailRow(item));
                        }
                    }
                    else {
                        for (var i = 0; i < total; i++) {
                            var item = rs.Items[i];
                            arr.push(gameStatistics.renderDailyRow(item));
                        }
                    }

                    //sub total by currency
                    var totalTotal = rs.TotalItems.length;
                    for (var j = 0; j < totalTotal; j++) {
                        arrTotal.push(gameStatistics.renderTotalRow(rs.TotalItems[j], 'total'));
                    }
                    //footer
                    for (var k = 0; k < rs.GrandTotal.length; k++) {

                        arrTotal.push(gameStatistics.renderTotalRow(rs.GrandTotal[k], ''));
                    }

                    //-----------
                    grid.getBody().html(arr.join(''));
                    grid.getFooter().html(arrTotal.join(''));
                    grid.initScroller();

                    if (fromCache)
                        gameStatistics.addSortingIconAfterPaging();

                } else {
                    if (rs && rs.total && rs.total == -100)
                        showAlertMessage("err", timeoutErrorMessage, 1);
                    else
                        showAlertMessage("err", "There is no data.", 1);
                }
            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    btnSpecialDateClick: function (id) {
        switch (id) {
            case 1:
                gameStatistics.specificPeriodIndex = 0;
                break;
            case 2:
                gameStatistics.specificPeriodIndex = 1;
                break;
        }
        $("#ddlTimeperiod").attr('selectedIndex', gameStatistics.specificPeriodIndex);
        gameStatisticsHelper.setDateFilter(gameStatistics.specificPeriodIndex + 1);
        gameStatistics.getReportDataSpecialDate(id);
    },
    getReportDataSpecialDate: function (id) {
        gameStatistics.specialReport = id;
        gameStatistics.getReportData(false);
    },
    renderHeaderRow: function (reportType) {

        var playerCol = reportType == 2 ? '' : '<td style="border-bottom: none 0px;">\
                                                    <div id="divHNoPlayers" class="w90 c sorter " onclick="gameStatistics.sorting(this, \'NoPlayers\')" >No.Players</div>\
                                                </td>';
        var currencyCol = reportType == 6 ? '' : '<td style="border-bottom: none 0px;">\
                                                        <div id="divHCurrency"  class="w90 c sorter " onclick="gameStatistics.sorting(this, \'CurrencyName\')" >Currency</div>\
                                                    </td>';
        var refundCol = !($('#chkShowRefund').attr('checked')) ? '' : '<td style="border-bottom: none 0px;">\
                                                                    <div id="divHRefund" class="w100 c sorter " onclick="gameStatistics.sorting(this, \'TotalRefund\')">Total refund</div>\
                                                                </td>';
        var html = '<tr>\
                    <td style="border-bottom: none 0px;">\
                        <div id="divHReporttype" class="w100 c sorter " onclick="gameStatistics.sorting(this, \'ReportType\')" >' + gameStatistics.getReportTypeTitle(reportType) + '</div>\
                    </td>\
                    ' + playerCol + '\
                    <td style="border-bottom: none 0px;">\
                        <div id="divHNoGames" class="w90 c sorter " onclick="gameStatistics.sorting(this, \'NoGames\')">No.Games</div>\
                    </td>\
                    ' + currencyCol + '\
                    <td style="border-bottom: none 0px;">\
                        <div id="divHTotalBet" class="w100 c sorter " onclick="gameStatistics.sorting(this, \'TotalBets\')">Total bets</div>\
                    </td>\
                    <td style="border-bottom: none 0px;">\
                        <div id="divHTotalWin" class="w100 c sorter " onclick="gameStatistics.sorting(this, \'TotalWins\')">Total wins</div>\
                    </td>\
                    <td style="border-bottom: none 0px;">\
                        <div id="divHGameIncome" class="w110 c sorter " onclick="gameStatistics.sorting(this, \'GameIncome\')">Game income</div>\
                    </td>\
                    <td style="border-bottom: none 0px;">\
                        <div id="divHGamePayout" class="w70 c sorter " onclick="gameStatistics.sorting(this, \'GamePayout\')">Game payout</div>\
                    </td>\
                    ' + refundCol + '\
                    <td style="border-bottom: none 0px;">\
                        <div id="divHCompanyTO" class="w110 c sorter " onclick="gameStatistics.sorting(this, \'CompanyTO\')">Company T/O</div>\
                    </td>\
                    <td style="border-bottom: none 0px;">\
                        <div id="divHCompanyComm" class="w120 c sorter " onclick="gameStatistics.sorting(this, \'CompanyComm\')">Company Comm</div>\
                    </td>\
                    <td style="border-bottom: none 0px;">\
                        <div id="divHCompany" class="w100 c sorter " onclick="gameStatistics.sorting(this, \'Company\')">Company net income</div>\
                    </td>\
                </tr>';
        return html;
    },
    getReportTypeTitle: function (id) {

        var title = '';
        switch (parseInt(id)) {
            case 0:
                title = 'Date';
                break;
            case 1:
                title = 'Month';
                break;
            case 2:
                title = 'Username';
                break;
            case 3:
                title = 'Game';
                break;
            case 4:
                title = 'Table';
                break;
            case 5:
                title = 'Company';
                break;
            case 6:
                title = 'Currency';
                break;
            default:
        }
        return title;
    },
    renderDailyRow: function (item) {
        var total = item.Items.length;
        var arr = [];
        for (var i = 0; i < total; i++) {
            var row = item.Items[i];
            var firstCol = '';
            if (i == 0) {
                firstCol = '<td rowspan=' + total + '>\
                                <div class="w100 c" >' + item.ReportType + '</div>\
                            </td>';
            }

            var refundCol = !($('#chkShowRefund').attr('checked')) ? '' : '<td>\
                                                                    <div class="w100 r ' + (parseFloat(row.TotalRefund) < 0 ? 'negativeNumber' : '') + '">' + row.TotalRefund + '</div>\
                                                                </td>';
            var html = '<tr>' + firstCol + '\
                        <td>\
                            <div class="w90 r">' + row.NoPlayers + '</div>\
                        </td>\
                        <td>\
                            <div class="w90 r">' + row.NoGames + '</div>\
                        </td>\
                        <td>\
                            <div class="w90 c">' + row.CurrencyName + '</div>\
                        </td>\
                        <td >\
                            <div class="w100 r ' + (parseFloat(row.TotalBets) < 0 ? 'negativeNumber' : '') + '">' + row.TotalBets + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.TotalWins) < 0 ? 'negativeNumber' : '') + '">' + row.TotalWins + '</div>\
                        </td>\
                        <td>\
                            <div class="w110 r ' + (parseFloat(row.GameIncome) < 0 ? 'negativeNumber' : '') + '">' + row.GameIncome + '</div>\
                        </td>\
                        <td>\
                            <div class="w70 r ' + (parseFloat(row.GamePayout) < 0 ? 'negativeNumber' : '') + '">' + row.GamePayout + '</div>\
                        </td>\
                        ' + refundCol + '\
                        <td>\
                            <div class="w110 r ' + (parseFloat(row.CompanyTO) < 0 ? 'negativeNumber' : '') + '">' + row.CompanyTO + '</div>\
                        </td>\
                        <td>\
                            <div class="w120 r ' + (parseFloat(row.CompanyComm) < 0 ? 'negativeNumber' : '') + '">' + row.CompanyComm + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.Company) < 0 ? 'negativeNumber' : '') + '">' + row.Company + '</div>\
                        </td>\
                    </tr>';

            arr.push(html);
        }
        return arr.join('');
    },
    renderDetailRow: function (item) {
        var reportType = gameStatistics.params.reporttype;
        var playerCol = reportType == 2 ? '' : '<td>\
                                                    <div class="w90 r ">' + item.NoPlayers + '</div>\
                                                </td>';
        var currencyCol = reportType == 6 ? '' : '<td>\
                                                    <div class="w90 c">' + item.CurrencyName + '</div>\
                                                </td>';
        var refundCol = !($('#chkShowRefund').attr('checked')) ? '' : '<td>\
                                                                    <div class="w100 r ' + (parseFloat(item.TotalRefund) < 0 ? 'negativeNumber' : '') + '">' + item.TotalRefund + '</div>\
                                                                </td>';
        var html = '<tr>\
                        <td>\
                                <div class="w100 c" >' + item.ReportType + '</div>\
                            </td>\
                        ' + playerCol + '\
                        <td>\
                            <div class="w90 r">' + item.NoGames + '</div>\
                        </td>\
                        ' + currencyCol + '\
                        <td >\
                            <div class="w100 r ' + (parseFloat(item.TotalBets) < 0 ? 'negativeNumber' : '') + '">' + item.TotalBets + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(item.TotalWins) < 0 ? 'negativeNumber' : '') + '">' + item.TotalWins + '</div>\
                        </td>\
                        <td>\
                            <div class="w110 r ' + (parseFloat(item.GameIncome) < 0 ? 'negativeNumber' : '') + '">' + item.GameIncome + '</div>\
                        </td>\
                        <td>\
                            <div class="w70 r ' + (parseFloat(item.GamePayout) < 0 ? 'negativeNumber' : '') + '">' + item.GamePayout + '</div>\
                        </td>\
                        ' + refundCol + '\
                        <td>\
                            <div class="w110 r ' + (parseFloat(item.CompanyTO) < 0 ? 'negativeNumber' : '') + '">' + item.CompanyTO + '</div>\
                        </td>\
                        <td>\
                            <div class="w120 r ' + (parseFloat(item.CompanyComm) < 0 ? 'negativeNumber' : '') + '">' + item.CompanyComm + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(item.Company) < 0 ? 'negativeNumber' : '') + '">' + item.Company + '</div>\
                        </td>\
                    </tr>';

        return html;
    },
    renderTotalRow: function (row, cssName) {

        var reportType = gameStatistics.params.reporttype;
        var playerCol = reportType == 2 ? '' : '<td style="">\
                                                    <div class="w90 r">' + row.NoPlayers + '</div>\
                                                </td>';
        var currencyCol = reportType == 6 ? '' : '<td style="">\
                                                        <div class="w90 c">' + row.CurrencyName + '</div>\
                                                    </td>';
        var refundCol = !($('#chkShowRefund').attr('checked')) ? '' : '<td style="">\
                                                                    <div class="w100 r ' + (parseFloat(row.TotalRefund) < 0 ? 'negativeNumber' : '') + '">' + row.TotalRefund + '</div>\
                                                                </td>';

        var html = '<tr class="' + cssName + '">\
                        <td>\
                            <div class="w100 c">' + row.ReportType + '</div>\
                        </td>\
                        ' + playerCol + '\
                        <td>\
                            <div class="w90 r">' + row.NoGames + '</div>\
                        </td>\
                        ' + currencyCol + '\
                        <td >\
                            <div class="w100 r ' + (parseFloat(row.TotalBets) < 0 ? 'negativeNumber' : '') + '">' + row.TotalBets + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.TotalWins) < 0 ? 'negativeNumber' : '') + '">' + row.TotalWins + '</div>\
                        </td>\
                        <td>\
                            <div class="w110 r ' + (parseFloat(row.GameIncome) < 0 ? 'negativeNumber' : '') + '">' + row.GameIncome + '</div>\
                        </td>\
                        <td>\
                            <div class="w70 r ' + (parseFloat(row.GamePayout) < 0 ? 'negativeNumber' : '') + '">' + row.GamePayout + '</div>\
                        </td>\
                        ' + refundCol + '\
                        <td>\
                            <div class="w110 r ' + (parseFloat(row.CompanyTO) < 0 ? 'negativeNumber' : '') + '">' + row.CompanyTO + '</div>\
                        </td>\
                        <td>\
                            <div class="w120 r ' + (parseFloat(row.CompanyComm) < 0 ? 'negativeNumber' : '') + '">' + row.CompanyComm + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.Company) < 0 ? 'negativeNumber' : '') + '">' + row.Company + '</div>\
                        </td>\
                    </tr>';
        return html;
    },
    exportExcel: function (fromCache) {
        if (!fromCache) {
            gameStatistics.specialReport = 0;
            gameStatistics.setParams();
        }

        var p = gameStatistics.params;
        var url = 'ExportDataToExcel.aspx?IsExport=true&fdate=' + p.fromdate + '&tdate=' + p.todate +
            '&username=' + p.username + '&site=' + p.site + '&gType=' + p.gametype + '&gTable=' + p.gametable +
            '&currency=' + p.currency + '&isExcludeTest=' + p.excludetest + '&reporttype=' + p.reporttype +
            '&fromcache=' + fromCache + '&showrefund=' + p.showrefund + '&winless=' + p.winLess + '&winmore=' + p.winMore +
            '&betless=' + p.betLess + '&betmore=' + p.betMore;
        $('#frm').attr('src', url);
    },
    getGameFilterInfo: function () {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/LiveCasinoReportService.svc/GetGameFilterInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                if (rs) {
                    gameStatistics.filterInfo = rs;
                    //company info
                    gameStatistics.assignDataToDropDownBox($('#ddlCompanies'), rs.Company);
                    //game type info
                    gameStatistics.assignDataToDropDownBox($('#ddlGameType'), rs.GameType);
                    gameStatistics.assignDataToDropDownBox($('#ddlCurrency'), rs.Currency);
                    gameStatistics.renderGameTable(0);
                    $("#ddlGameType").bind("change", function () {
                        gameStatistics.renderGameTable($(this).val());
                    });
                    //                    //report type
                    gameStatistics.renderReportOption(rs.ReportType);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    assignDataToDropDownBox: function (obj, data) {
        obj.children().remove().end();
        obj.append('<option value="0">All</option>');
        if (data) {
            for (var j = 0; j < data.length; j++) {
                var item = data[j];
                if (item.Selected)
                    obj.append('<option selected value="' + item.Id + '">' + item.Name + '</option>');
                else
                    obj.append('<option value="' + item.Id + '">' + item.Name + '</option>');
            }
        }
    },
    renderReportOption: function (data) {
        $('#tdReportFilter').html('');
        if (data) {
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                $('#tdReportFilter').append('<input type="radio" id="rdo' + item.Id + '" name="reportby" value="' + item.Id + '" ' + (item.Selected ? ' checked="checked"' : '') + ' />' + item.Name + '&nbsp;');
            }
        }
    },
    renderGameTable: function (gameTypeId) {
        $.ajax({
            type: "POST",
            url: "Service/LiveCasinoReportService.svc/GetGameTableByGameType",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"gametype": ' + gameTypeId + '}',
            success: function (rs) {
                if (rs) {
                    gameStatistics.assignDataToDropDownBox($('#ddlGameTable'), rs);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    changeDateOption: function () {
        if ($('#txtfromdate').val() != gameStatistics.defaultDate ||
            $('#txttodate').val() != gameStatistics.defaultDate) {
            $("#ddlTimeperiod").attr('selectedIndex', gameStatistics.specificPeriodIndex);
        }
    },
    showhideSearchCondition: function (id) {
        if ($(id).hasClass('upCondition')) {
            $(id).removeClass('upCondition');
            $(id).addClass('downCondition');
            $('.maincontent').addClass('hidden');
            $("#gridgamestatistic").css("margin-top", "32px");
        }
        else {
            $(id).removeClass('downCondition');
            $(id).addClass('upCondition');
            $('.maincontent').removeClass('hidden');
            $("#gridgamestatistic").css("margin-top", "6px");
        }
    },
    setParams: function () {

        gameStatistics.setDateFilter();
        gameStatistics.params.username = $('#txtUsername').val().toLowerCase() == 'all' ? '' : $('#txtUsername').val();
        gameStatistics.params.excludetest = $('#chkExcludeTestAcc').attr('checked');
        gameStatistics.params.site = $('#ddlCompanies').val();
        gameStatistics.params.gametype = $('#ddlGameType').val();
        gameStatistics.params.gametable = $('#ddlGameTable').val();
        gameStatistics.params.currency = $('#ddlCurrency').val();
        gameStatistics.params.betLess = $('#txtTotalBetLessThan').val();
        gameStatistics.params.betMore = $('#txtTotalBetMoreThan').val();
        gameStatistics.params.winLess = $('#txtTotalWinLessThan').val();
        gameStatistics.params.winMore = $('#txtTotalWinMoreThan').val();
        gameStatistics.params.reporttype = $('input[name=reportby]:checked').val();
        gameStatistics.params.showrefund = $('#chkShowRefund').attr('checked');


    },
    setDateFilter: function () {
        var selectValue = $('#ddlTimeperiod').val();

        var dt = new Date(_today);
        var fDate = new Date(_today);
        var tDate = new Date(_today);

        if (gameStatistics.specialReport == 1) {
            fDate = new Date(_today);
            tDate = new Date(_today);
        }
        else if (gameStatistics.specialReport == 2) {

            fDate = new Date($('#btnYesterday').val());
            tDate = new Date($('#btnYesterday').val());
        }
        else {
            switch (parseInt(selectValue)) {
                case 1:
                    fDate = dt;
                    tDate = dt;
                    break;
                case 2:
                    fDate = new Date(_yesterday);
                    //fDate.setDate(fDate.getDate() - 1);
                    tDate = new Date(_yesterday);
                    //tDate.setDate(tDate.getDate() - 1);
                    break;
                case 3:
                    var dateRange = gameStatisticsHelper.getDateRangeOfWeek();
                    fDate = dateRange[0];
                    tDate = dateRange[1];
                    break;
                case 4:
                    var dateRange = gameStatisticsHelper.getDateRangeOfCurrentMonth();
                    fDate = dateRange[0];
                    tDate = dateRange[1];
                    break;
                case 5:
                    var dateRange = gameStatisticsHelper.getDateRangeOfMonths(1);
                    fDate = dateRange[0];
                    tDate = dateRange[1];
                    break;
                case 6:
                    fDate = new Date(dt.getFullYear(), dt.getMonth() - 1, 1);
                    tDate = dt;
                    break;
                case 7:

                    var dateRange = gameStatisticsHelper.getDateRangeOfMonths(3);
                    fDate = dateRange[0];
                    tDate = dateRange[1];

                    break;
                case 8:

                    var dateRange = gameStatisticsHelper.getDateRangeOfMonths(6);
                    fDate = dateRange[0];
                    tDate = dateRange[1];
                    break;
                case 0:
                    fDate = new Date($('#txtfromdate').val());
                    tDate = new Date($('#txttodate').val());
                    break;
                default:
            }
        }

        gameStatistics.params.fromdate = (fDate.getMonth() + 1) + '/' + fDate.getDate() + '/' + fDate.getFullYear();
        gameStatistics.params.todate = (tDate.getMonth() + 1) + '/' + tDate.getDate() + '/' + tDate.getFullYear();
    },
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = gameStatistics.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(gameStatistics.params.totalItems / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        gameStatistics.params.pageindex = page_index + 1;
        gameStatistics.params.pagesize = _items_per_page;
        gameStatistics.getReportData(true);
        return false;
    },
    sorting: function (obj, sortCol) {
        if (gameStatistics.params.sort.SortType == '') {
            gameStatistics.params.sort.SortType = 'desc';
        }

        if (gameStatistics.params.sort.SortType == 'desc') {
            $(obj).removeClass("desc");
            $(obj).addClass("asc");
            gameStatistics.params.sort.SortType = 'asc';
        }
        else {
            $(obj).removeClass("asc");
            $(obj).addClass("desc");
            gameStatistics.params.sort.SortType = 'desc';
        }
        //--replace sort icon
        $('#tbreportheader tr:eq(0) td').each(function () {
            if ($(this).find('div').attr('id') != $(obj).attr('id')) {
                $(this).find('div').removeClass('asc');
                $(this).find('div').removeClass('desc');
                $(this).find('div').addClass('sorter');
            }
        });
        //-------

        gameStatistics.params.sort.SortCol = sortCol;
        if (sortCol == 'ReportType' | sortCol == 'CurrencyName')
            gameStatistics.params.sort.IsDecimal = false;
        else
            gameStatistics.params.sort.IsDecimal = true;
        gameStatistics.renderDataSorted();
    },
    renderDataSorted: function () {
        $('#err').html('');
        gameStatistics.params.fromcache = true;
        $.ajax({
            type: "POST",
            url: "Service/LiveCasinoReportService.svc/GetGameStatistic",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ dbparams: gameStatistics.params }),
            success: function (rs) {

                if (rs && rs.Items && rs.Items.length > 0) {

                    var arr = [];
                    var arrTotal = [];
                    var total = rs.Items.length;
                    gameStatistics.params.totalItems = rs.total;
                    //detail
                    if (gameStatistics.params.reporttype == 2 | gameStatistics.params.reporttype == 6) {
                        for (var i = 0; i < total; i++) {
                            var item = rs.Items[i];
                            arr.push(gameStatistics.renderDetailRow(item));
                        }
                    }
                    else {
                        for (var i = 0; i < total; i++) {
                            var item = rs.Items[i];
                            arr.push(gameStatistics.renderDailyRow(item));
                        }
                    }

                    //sub total by currency
                    var totalTotal = rs.TotalItems.length;
                    for (var j = 0; j < totalTotal; j++) {
                        arrTotal.push(gameStatistics.renderTotalRow(rs.TotalItems[j], 'total'));
                    }
                    //footer
                    for (var k = 0; k < rs.GrandTotal.length; k++) {
                        arrTotal.push(gameStatistics.renderTotalRow(rs.GrandTotal[k], ''));
                    }

                    //-----------
                    grid.getBody().html(arr.join(''));
                    grid.getFooter().html(arrTotal.join(''));
                    grid.initScroller();
                } else {
                    showAlertMessage("err", "There is no data.", 1);
                }
            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    setDefaultFilter: function () {
        $('#gridgamestatistic').addClass('hidden');
        gameStatistics.params.sort.SortType = '';
        gameStatistics.params.sort.SortCol = '';
        gameStatistics.params.pageindex = 1;
    },
    addSortingIconAfterPaging: function () {
        var sorttype = '';
        var sortcol = '';
        if (gameStatistics.params.sort.SortCol == 'ReportType')
            sortcol = 'divHReporttype';
        else if (gameStatistics.params.sort.SortCol == 'CurrencyName')
            sortcol = 'divHCurrency';
        else if (gameStatistics.params.sort.SortCol == 'TotalBets')
            sortcol = 'divHTotalBet';
        else if (gameStatistics.params.sort.SortCol == 'TotalWins')
            sortcol = 'divHTotalWin';
        else
            sortcol = 'divH' + gameStatistics.params.sort.SortCol;

        if (gameStatistics.params.sort.SortType == '')
            return;
        else
            sorttype = gameStatistics.params.sort.SortType;

        $('#' + sortcol).addClass(sorttype);
    }
};

var pagerOpts = {
    callback: null,
    start_per_page: 50,
    step_per_page: 50,
    max_items_per_page: 200,
    items_per_page: 50,
    current_page: 1
};

var gameStatisticsHelper = {
    getDateRangeOfWeek: function () {
        var _date = new Date(_today);
        var _lastDate = new Date(_today);

        var day = _date.getDay(),
        diff = _date.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday
        var startDate = new Date(_date.setDate(diff));
        startDate = _date.getMonth() < _lastDate.getMonth() ? new Date(_lastDate.getFullYear(), _lastDate.getMonth(), 1) : startDate;

        return [startDate, _lastDate];
    },
    getDateRangeOfCurrentMonth: function () {
        var today = new Date(_today);
        var startday = new Date(_today);
        startday.setDate(1);
        startday.setMonth(today.getMonth());
        var endday = new Date(today.setMonth(today.getMonth()));
        return [startday, endday];
    },
    getDateRangeOfMonths: function (totalMonths) {
        var cDate = new Date(_today);
        var currentMonth = cDate.getMonth();
        var currentYear = cDate.getFullYear();
        var lastYear = cDate.getFullYear() - 1;

        var startDate = new Date();
        var endDate = new Date();
//        if (totalMonths - (currentMonth - 1) > 0) {

//            var month = 12 - (totalMonths - (currentMonth));
//            startDate = new Date(lastYear, month, 1);
//            endDate = new Date(currentYear, (cDate.getMonth() - 1), (gameStatisticsHelper.getTotalDayOfMonth((currentMonth), currentYear)));
//        }
//        else if (totalMonths - (currentMonth - 1) == 0) {
//            startDate = new Date(lastYear, 12, 1);
//            endDate = new Date(lastYear, 12, 31);
//        }
//        else {
//            var month = 12 - (totalMonths - (currentMonth));
//            startDate = new Date(lastYear, month, 1);
//            endDate = new Date(currentYear, (cDate.getMonth() - 1), (gameStatisticsHelper.getTotalDayOfMonth((currentMonth), currentYear)));
        //        }
        if (currentMonth == 0 && totalMonths == 1) {
            var fromMonth = 12 - totalMonths;
            startDate = new Date(lastYear, fromMonth, 1);
            endDate = new Date(lastYear, fromMonth, 31);
        }
        else {

            var month = currentMonth - totalMonths;
            if (month >= 0) {
                startDate = new Date(currentYear, month, 1);
                endDate = new Date(currentYear,
                                    currentMonth - 1,
                                    gameStatisticsHelper.getTotalDayOfMonth(currentMonth, currentYear)
                                    );
            }
            else {
                startDate = new Date(lastYear, 12 + month, 1);
                var tempYear = (currentMonth == 0 ? lastYear : currentYear);
                var tempMonth = (currentMonth == 0 ? 11 : (currentMonth - 1))
                endDate = new Date(tempYear,
                                    tempMonth,
                                    gameStatisticsHelper.getTotalDayOfMonth(tempMonth + 1, tempYear)
                                    );
            }
        }

        return [startDate, endDate];
    },
    getCurrentWeekNumber: function () {
        var currentDate = new Date(_today);
        var firstDay = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1).getDay();
        return Math.ceil((currentDate.getDate() + firstDay) / 7);
    },
    getTotalDayOfMonth: function (month, year) {
        var m = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (month != 2) return m[month - 1];
        if (year % 4 != 0) return m[1];
        if (year % 100 == 0 && year % 400 != 0) return m[1];
        return m[1] + 1;
    },
    renderCalendarFromTo: function (ids) {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
                gameStatistics.specificPeriodIndex = 8;
                gameStatistics.changeDateOption();
            }
        };
        var dates = $(ids).datepicker(defaults);
        var dt = new Date(_today);
        var fdate = $(ids.split(',')[0]);
        var tdate = $(ids.split(',')[1]);
        fdate.datepicker("setDate", dt);
        tdate.datepicker("setDate", dt);
    },
    on_Blur: function () {
        var inputValue = 'All';
        var oldName = '';
        var newName = $('#txtUsername').val();

        if (oldName != '' && newName != '') {
            if (newName != oldName);
            inputValue = newName;
        }
        else if (oldName != '' && newName == '') {
            inputValue = oldName;
        }
        else if (newName != '')
            inputValue = newName;
        else
            inputValue = 'All';

        $('#txtUsername').val(inputValue);
    },
    on_Focus: function () {
        if ($('#txtUsername').val().length > 0)
            $('#txtUsername').val('');
    },
    setDateFilter: function (selectValue) {
        var dt = new Date(_today);
        var fDate = new Date(_today);
        var tDate = new Date(_today);

        switch (parseInt(selectValue)) {
            case 1:
                fDate = dt;
                tDate = dt;
                break;
            case 2:
                fDate = new Date(_yesterday);
                tDate = new Date(_yesterday);
                break;
            case 3:
                var dateRange = gameStatisticsHelper.getDateRangeOfWeek();
                fDate = dateRange[0];
                tDate = dateRange[1];
                break;
            case 4:
                var dateRange = gameStatisticsHelper.getDateRangeOfCurrentMonth();
                fDate = dateRange[0];
                tDate = dateRange[1];
                break;
            case 5:
                var dateRange = gameStatisticsHelper.getDateRangeOfMonths(1);
                fDate = dateRange[0];
                tDate = dateRange[1];
                break;
            case 6:
                fDate = new Date(dt.getFullYear(), dt.getMonth() - 1, 1);
                tDate = dt;
                break;
            case 7:
                var dateRange = gameStatisticsHelper.getDateRangeOfMonths(3);
                fDate = dateRange[0];
                tDate = dateRange[1];
                break;
            case 8:
                var dateRange = gameStatisticsHelper.getDateRangeOfMonths(6);
                fDate = dateRange[0];
                tDate = dateRange[1];
                break;
            case 0:
                fDate = new Date($('#txtfromdate').val());
                tDate = new Date($('#txttodate').val());
                break;
            default:
        }
        $("#txttodate").datepicker("option", "minDate", fDate);
        $("#txtfromdate").datepicker("option", "maxDate", tDate);
        $("#txttodate").datepicker("setDate", tDate);
        $("#txtfromdate").datepicker("setDate", fDate);
    }
};

$(document).ready(function () {
    gameStatisticsHelper.renderCalendarFromTo("#txtfromdate, #txttodate");
    gameStatistics.getGameFilterInfo();
    var dt = new Date(_today);
    var yDt = new Date(_yesterday);

    var month = ((dt.getMonth() + 1) > 9 ? "" : "0") + (dt.getMonth() + 1);
    var yMonth = ((yDt.getMonth() + 1) > 9 ? "" : "0") + (yDt.getMonth() + 1);
    var year = dt.getFullYear();
    var yYear = yDt.getFullYear();

    var currentDate = month + '/' + ((dt.getDate() > 9 ? "" : "0") + dt.getDate()) + '/' + year;
    var yesterday = yMonth + '/' + (((yDt.getDate()) > 9 ? "" : "0")) + (yDt.getDate()) + '/' + yYear;

    gameStatistics.defaultDate = currentDate;
    $('#btnToday').val(currentDate);
    $('#btnYesterday').val(yesterday);
    $('.numbersOnly').keyup(function () {
        this.value = this.value.replace(/[^0-9\.]/g, '');
    });
    //----------auto complete
    var autosearch;
    var isonauto = false;
    var txtusername = $('#txtUsername');
    autosearch = txtusername.autocomplete({ serviceUrl: '../../Handlers/QueryUserName.ashx', minChars: 2 });
    $('.autocomplete').mouseover(function () {
        isonauto = true;
    }).mouseout(function () {
        isonauto = false;
    });
    txtusername.blur(function () {
        if (!isonauto) {
            $('#Autocomplete_1').css('display', 'none');
        }
        else {
            txtusername.focus();
        }

    });
    //-----------
});