function renderCalendar(ids, opts, chosenDate) {
    var defaults = {
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true
    };
    var new_opts = $.extend(true, defaults, opts);
    var dates = $("#" + ids).datepicker(new_opts);
    $("#" + ids).datepicker("setDate", chosenDate);
}


function setupCalendar(id, chosenDate, mindate, maxdate) {

    var limitDate = {
        minDate: mindate,
        maxDate: maxdate
    };
    renderCalendar(id, limitDate, chosenDate);
    $('#' + id).click(function () {
        $('#' + id).datepicker("show");
    });

}

var pagerOpts = {
    callback: null,
    start_per_page: 50,
    step_per_page: 50,
    max_items_per_page: 200,
    items_per_page: 50,
    current_page: 1
};

var parlayWonReport = {
    date: '',
    title: '',
    params: {
        winlossDate: '',
        betType: 9,
        isSoccer: false,
        balanceUpDate: '',
        moveBetsDay: '',
        fromCache: false,
        sortDirection: '',
        sortBy: 0,
        pageSize: 50,
        pageIndex: 1,
        totalItems: 0
    },
    gridOption: {
        tblContentId: 'gridDetail',
        manualCopyHeader: true,
        isShowPager: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 1230,
        gridHeight: 66,
        freezeWidth: 475,
        numberFreezeCol: 4
    },
    viewReport: function (fromCache) {
        //        try { parent.BlockUI(); } catch (e) { }
        var tplFullRows = [], tplFreezeRows = [], tplFooterFull = [], tplFooterFreeze = [];
        parlayWonReport.getParams(fromCache);
        if (!fromCache) {
            $('#baseWinlost').removeClass("desc");
            $('#baseWinlost').removeClass("asc");
            $('#companyWinlost').removeClass("desc");
            $('#companyWinlost').removeClass("asc");
            parlayWonReport.params.pageIndex = 1;
        }
        $('#err').hide();
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "ParlayWon/GetData",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(parlayWonReport.params),
            success: function (result) {
                var len = result.ListParlayWon.length;
                parlayWonReport.params.totalItems = result.Count;
                if (len == 0) {
                    $(".contentBox").hide();
                    showAlertMessage("err", "There is no data.", 1);
                    $("#err").show();
                }
                else {
                    $(".contentBox").show();
                    $("#parlayWonGrid").show();

                    if (parlayWonReport.params.betType == "9") {
                        $("#subtitle").empty().append(" Mix Parlay Won - " + parlayWonReport.params.winlossDate);
                    }
                    else {
                        $("#subtitle").empty().append(" System Parlay Won - " + parlayWonReport.params.winlossDate);
                    }
                    for (var i = 0; i < len; i++) {
                        var row = result.ListParlayWon[i];
                        var css = "";
                        if (i % 2 == 1) {
                            css = "evenrow";
                        }
                        tplFullRows.push(parlayWonReport.tplRowFull(row, css));
                        tplFreezeRows.push(parlayWonReport.tplRowFreeze(row, css));
                    }
                    $("#gridRowHeader").find('tbody').empty().append(tplFreezeRows.join(''));
                    $("#gridDetail").find('tbody').empty().append(tplFullRows.join(''));

                    tplFooterFull.push(parlayWonReport.tplFooterFull(result));
                    tplFooterFreeze.push(parlayWonReport.tplFooterFreeze(result));
                    $("#GridFooterRow").find('thead').empty().append(tplFooterFreeze.join(''));
                    $("#GridFooterCol").find('thead').empty().append(tplFooterFull.join(''));

                    parlayWonReport.initPager(parlayWonReport.params.totalItems, parlayWonReport.params.pageSize, parlayWonReport.params.pageIndex - 1);
                    $('#parlayWonGrid').removeClass('hidden').gridScroller(parlayWonReport.gridOption);

                    $('#filter').css({ width: $('#parlayWonGrid').outerWidth() - 6 });
                }
            },
            error: function () {
                $(".contentBox").hide();
                showAlertMessage("err", "There is no data.", 1);
                $("#err").show();
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },
    tplRowFull: function (row, css) {
        var tpl = '<tr class=' + css + '><td><div style="height: 17px; width: 140px;">&nbsp;</div></td>\
        <td><div style="height: 17px; width: 160px;">&nbsp;</div></td>\
        <td><div style="height: 17px; width: 80px;">&nbsp;</div></td>\
        <td><div style="height: 17px; width: 80px;">&nbsp;</div></td>\
        <td><div style="height: 17px; width: 80px;" class="r">' + row.OriginalStake + '</div></td>\
        <td><div style="height: 17px; width: 80px;" class="r">' + row.OriginalWinlost + '</div></td>\
        <td class=" base"><div style="height: 17px; width: 80px;" class="r ">' + row.BaseStake + '</div></td>\
        <td class=" base"><div style="height: 17px; width: 80px;" class="r ">' + row.BaseWinlost + '</div></td>\
        <td class=" company"><div style="height: 17px; width: 100px;" class="r ">&nbsp;' + row.CompanyStake + '</div> </td>\
        <td class=" company"><div style="height: 17px; width: 130px;" class="r">&nbsp;' + row.CompanyWinlost + '</div></td>\
        <td><div style="height: 17px; width: 80px;" class="r">' + row.Selection + '</div></td>\
        <td><div style="height: 17px; width: 80px;" class="r">' + row.WinOdds + '</div></td></tr>';
        return tpl;
    },
    tplRowFreeze: function (row, css) {
        var tpl = '<tr class=' + css + '><td><div style="height: 17px; width: 140px;" class="c">' + row.RefNo + '</div></td>\
        <td><div style="height: 17px; width: 160px;">' + row.UserName + '</div> </td>\
        <td><div style="height: 17px; width: 80px;" class="c">' + row.Currency + '</div></td>\
        <td><div style="height: 17px; width: 80px;" class="r">' + row.Exchange + '</div></td></tr>';
        return tpl;
    },
    tplFooterFull: function (rs) {
        var tpl = '<tr><th><div style="height: 17px; width: 140px;">&nbsp;</div></th>\
        <th><div style="height: 17px; width: 160px;" >&nbsp;</div></th>\
        <th><div style="height: 17px; width: 80px;">&nbsp;</div></th>\
        <th><div style="height: 17px; width: 80px;">&nbsp;</div></th>\
        <th><div style="height: 17px; width: 80px;" >&nbsp;</div></th>\
        <th><div style="height: 17px; width: 80px;">&nbsp;</div></th>\
        <th><div style="height: 17px; width: 80px;" class="r ">' + rs.BaseStakeTotal + '</div></th>\
        <th><div style="height: 17px; width: 80px;" class="r ">' + rs.BaseWinlostTotal + '</div></th>\
        <th><div style="height: 17px; width: 100px;" class="r ">' + rs.CompanyStakeTotal + '</div></th>\
        <th><div style="height: 17px; width: 130px;" class="r">' + rs.CompanyWinlostTotal + '</div>\</th>\
        <th><div style="height: 17px; width: 80px;" >&nbsp;</div></th>\
        <th><div style="height: 17px; width: 80px;">&nbsp;</div></th></tr>';
        return tpl;
    },
    tplFooterFreeze: function (rs) {
        var tpl = '<tr><th><div style="height: 17px; width: 140px;" class="c">&nbsp;</div></th>\
        <th><div style="height: 17px; width: 160px;" class="c b"> Total</div></th>\
        <th><div style="height: 17px; width: 80px;" >&nbsp;</div></th>\
        <th><div style="height: 17px; width: 80px;" &nbsp;></div></th></tr>';
        return tpl;
    },
    getParams: function (fromCache) {
        parlayWonReport.params.balanceUpDate = _balanceUpDate;
        parlayWonReport.params.moveBetsDay = _moveBetsDay;
        if ($('input[name=sportstype]:checked').val() == 'soccer') {
            parlayWonReport.params.isSoccer = true;
        }
        else {
            parlayWonReport.params.isSoccer = false;
        }

        parlayWonReport.params.betType = $('input[name=reporttype]:checked').val();
        parlayWonReport.params.winlossDate = $('#txtfromdate').val();
        parlayWonReport.params.fromCache = fromCache;
    },
    sorting: function (fromCache, sortBy) {
        if (sortBy == 0) {
            $('#companyWinlost').removeClass("asc");
            $('#companyWinlost').removeClass("desc");
            var sortDirection = $('#baseWinlost').attr('class');
            if (sortDirection.search('asc') != -1) {
                parlayWonReport.params.sortDirection = 'ASC';
                $('#baseWinlost').addClass("desc");
                $('#baseWinlost').removeClass("asc");
            }
            else {
                parlayWonReport.params.sortDirection = 'DESC';
                $('#baseWinlost').addClass("asc");
                $('#baseWinlost').removeClass("desc");
            }
        }
        else {

            $('#baseWinlost').removeClass("asc");
            $('#baseWinlost').removeClass("desc");
            var sortDirection = $('#companyWinlost').attr('class');
            if (sortDirection.search('asc') != -1) {
                parlayWonReport.params.sortDirection = 'ASC';
                $('#companyWinlost').addClass("desc");
                $('#companyWinlost').removeClass("asc");
            }
            else {
                parlayWonReport.params.sortDirection = 'DESC';
                $('#companyWinlost').addClass("asc");
                $('#companyWinlost').removeClass("desc");
            }
        }
        parlayWonReport.params.sortBy = sortBy;
        parlayWonReport.viewReport(fromCache);
    },
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = parlayWonReport.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, pagerOpts);
        //        $("#pagination").pagination(total_items, pagerOpts);
        //        if (total_items < items_per_page) {
        //            $("#pagination").addClass("hidden");
        //        }
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(parlayWonReport.params.totalItems / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        parlayWonReport.params.pageIndex = page_index + 1;
        parlayWonReport.params.pageSize = _items_per_page;
        parlayWonReport.viewReport(true);
        return false;
    },
    exportExcel: function () {
        var p = parlayWonReport.params;
        var url = 'ParlayWonReport?IsExport=true&winlostDate=' + p.winlossDate + '&betType=' + p.betType + '&isSoccer=' + p.isSoccer + '&moveBetsDay=' + p.moveBetsDay +
        '&balanceUpdate=' + p.balanceUpDate + '&fromCache=' + true;
        $('#frm').attr('src', url);
    }

};

$(document).ready(function () {
    setupCalendar("txtfromdate", chosenDate, mindate, currdate);
    $('#parlayWonGrid').initGrid(parlayWonReport.gridOption);


});
