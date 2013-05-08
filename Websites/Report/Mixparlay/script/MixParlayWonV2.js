
var pagerOpts = {
    callback: null,
    start_per_page: 10,
    step_per_page: 10,
    max_items_per_page: 100,
    items_per_page: 50,
    current_page: 1,
    is_show_detail: false
};

var mixparlayWonReport = {
    pageIndex: 1,
    pageSize: 50,
    totalItem: 0,
    fromcache: false,
    date: '',
    isSoccer: false,
    sortDesc: 0,
    initPager: function (total_item, items_per_page, current_page) {
        pagerOpts.callback = mixparlayWonReport.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_item, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(mixparlayWonReport.totalItem / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        mixparlayWonReport.pageIndex = page_index + 1;
        mixparlayWonReport.pageSize = _items_per_page;
        try { parent.BlockUI(); } catch (e) { }
        mixparlayWonReport.renderMixparlayWon();
        return false;
    },
    getMixparlayWonReport: function () {
        mixparlayWonReport.pageIndex = 1;
        mixparlayWonReport.fromcache = false;
        mixparlayWonReport.date = $("#date").val();
        if ($('input[@name="sportstype"]:checked').val() == 'soccer') {
            mixparlayWonReport.isSoccer = true;
        }
        else {
            mixparlayWonReport.isSoccer = false;
        }
        try { parent.BlockUI(); } catch (e) { }
        $('#mixparlaywon').addClass('hidden');
        mixparlayWonReport.renderMixparlayWon();
    },
    renderMixparlayWon: function () {
        $("#err").empty();
        var mixparlaywon = $("#mixparlaywon");
        //
        var arr = [];
        $.ajax({
            type: "POST",
            url: "../Service/ReportService.svc/GetMixparlayReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"isSoccer":' + mixparlayWonReport.isSoccer + ',"date":"' + mixparlayWonReport.date +
                '","pageIndex":' + mixparlayWonReport.pageIndex + ',"pageSize":' + mixparlayWonReport.pageSize +
                ',"fromcache":' + mixparlayWonReport.fromcache + ',"sortType":' + mixparlayWonReport.sortDesc + '}',
            success: function (rs) {
                mixparlayWonReport.totalItem = rs.total;
                if (rs.rows.length > 0) {
                    $("#gridtitle").html("Mix Parlay Won - Date " + mixparlayWonReport.date);

                    var tbl = $("#tblmixparlay");
                    tbl.empty();
                    var cssClassEven = "";
                    $.each(rs.rows, function (i, row) {
                        cssClassEven = i % 2 == 0 ? "" : "bgeven";
                        arr.push(mixparlayWonReport.renderRow(row, cssClassEven));
                    });
                    tbl.html(arr.join(''));

                    mixparlayWonReport.assignTotal(rs.footer[0]);
                    mixparlaywon.removeClass('hidden').reRenderScroller(910, 400, true);

                    mixparlayWonReport.initPager(mixparlayWonReport.totalItem, mixparlayWonReport.pageSize, mixparlayWonReport.pageIndex - 1);

                }
                else {
                    showAlertMessage("err", "There is no data.", 1);
                    mixparlaywon.addClass('hidden');
                }


            },
            error: function () { },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });

    },
    renderRow: function (row, cssClassEven) {
        var strrow = "";
        strrow += '<tr>';
        strrow += '<td class="' + cssClassEven + '"><div class="col80">' + row.TransID + '</div></td>';
        strrow += '<td class="' + cssClassEven + '"><div class="col160">' + row.UserName + '</div></td>';
        strrow += '<td class="' + cssClassEven + '"><div class="col80">' + row.Currency + '</div></td>';
        strrow += '<td class="' + cssClassEven + '"><div class="col80 r">' + row.Exchange + '</div></td>';
        strrow += '<td class="' + cssClassEven + '"><div class="col80 r">' + row.Stake + '</div></td>';
        strrow += '<td class="' + cssClassEven + '"><div class="col80 r">' + row.Winloss + '</div></td>';
        strrow += '<td class="bgbase"><div class="col80 r">' + row.BaseStake + '</div></td>';
        strrow += '<td class="bgbase"><div class="col80 r">' + row.BaseWinloss + '</div></td>';
        strrow += '<td class="' + cssClassEven + '"><div class="col80 r">' + row.Selection + '</div></td>';
        strrow += '<td class="' + cssClassEven + '"><div class="col80 r">' + row.WinOdds + '</div></td>';
        strrow += '</tr>';
        return strrow;
    },
    assignTotal: function (row) {
        $("#totalbasestake").html(row.BaseStake);
        $("#totalbasewinlost").html(row.BaseWinloss);
    },
    exportToExcel: function () {
        if ($('input[@name="sportstype"]:checked').val() == 'soccer') {
            mixparlayWonReport.isSoccer = true;
        }
        else {
            mixparlayWonReport.isSoccer = false;
        }
        var params = 'type=export&fdate=' + mixparlayWonReport.date + '&issoccer=' + mixparlayWonReport.isSoccer + '&sorttype=' + mixparlayWonReport.sortDesc;
        $('#frm').attr('src', 'MixparlayWon.aspx?' + params);
    },
    baseCurrenySort: function () {
        if (mixparlayWonReport.sortDesc == 0) {
            mixparlayWonReport.sortDesc = 1;
        }

        if (mixparlayWonReport.sortDesc == 1) {
            $('#wlsort').removeClass('desc');
            $('#wlsort').addClass('asc');
            mixparlayWonReport.sortDesc = 2;
        }
        else {
            $('#wlsort').removeClass('asc');
            $('#wlsort').addClass('desc');
            mixparlayWonReport.sortDesc = 1;
        }
        mixparlayWonReport.pageIndex = 1;
        try { parent.BlockUI(); } catch (e) { }
        mixparlayWonReport.renderMixparlayWon();
    }
}