var _pageIndex = 1;
var _pageSize = 50;
var _totalItem = 0;
var _isSoccer = false;
var fromcache = false;
var _date = "";

function GetReport() {
    fromcache = false;
    _date = $("#date").val();
    if ($('input[@name="sportstype"]:checked').val() == 'soccer') {
        _isSoccer = true;
    }
    else {
        _isSoccer = false;
    }
    RenderMixparlayWon();
}

function GetReportPager() {
    fromcache = true;
    try { parent.BlockUI(); } catch (e) { }
    var arr = [];

    $.ajax({
        type: "POST",
        url: "../Service/ReportService.svc/GetCombinationMixparlayReport",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"isSoccer":' + _isSoccer + ',"date":"' + _date + '","pageIndex":' + _pageIndex + ',"pageSize":' + _pageSize + ',"fromcache":' + fromcache + '}',
        success: function (rs) {
            if (rs.rows.length > 0) {
                var tbl = $("#tblmixparlay");
                tbl.empty();
                var cssClassEven = "";
                $.each(rs.rows, function (i, row) {
                    cssClassEven = i % 2 == 0 ? "" : "bgeven";
                    arr.push(RenderRow(row, cssClassEven));
                });
                tbl.html(arr.join(''));
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        }
    });
}

function RenderMixparlayWon() {
    try { parent.BlockUI(); } catch (e) { }
    var mixparlaywon = $("#mixparlaywon");
    var arr = [];
    $.ajax({
        type: "POST",
        url: "../Service/ReportService.svc/GetCombinationMixparlayReport",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"isSoccer":' + _isSoccer + ',"date":"' + _date + '","pageIndex":' + _pageIndex + ',"pageSize":' + _pageSize + ',"fromcache":' + fromcache + '}',
        success: function (rs) {
            _totalItem = rs.total;
            if (rs.rows.length > 0) {
                $("#err").addClass("hidden");
                $("#gridtitle").html("Combination Mix Parlay Won - Date " + _date);

                var tbl = $("#tblmixparlay");
                tbl.empty();
                var cssClassEven = "";
                $.each(rs.rows, function (i, row) {
                    cssClassEven = i % 2 == 0 ? "" : "bgeven";
                    arr.push(RenderRow(row, cssClassEven));
                });
                tbl.html(arr.join(''));
                assignTotal(rs.footer[0]);
                mixparlaywon.show();
                mixparlaywon.reRenderScroller(910, 663, true);
                initPager(_totalItem, 50, 0);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            else {
                showAlertMessage("err", "There is no data.", 1);
                mixparlaywon.hide();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        }
    });
}

function RenderRow(row, cssClassEven) {
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
}

function assignTotal(row) {
    $("#totalbasestake").html(row.BaseStake);
    $("#totalbasewinlost").html(row.BaseWinloss);
}

function initPager(total_item, items_per_page, current_page) {
    var opt = { callback: pageselectCallback };
    // Collect options from the text fields - the fields are named like their option counterparts
    opt["items_per_page"] = items_per_page;
    opt["current_page"] = current_page;
    $("#pagination").pagination(total_item, opt);
}


function pageselectCallback(page_index, _items_per_page, jq) {
    var max_page = Math.ceil(_totalItem / _items_per_page);
    page_index = page_index < max_page ? page_index : max_page - 1;

    _pageIndex = page_index + 1;
    _pageSize = _items_per_page;

    GetReportPager();
    initPager(_totalItem, _items_per_page, page_index);

    return false;
}

function ExportToExcel() {
    _date = $("#date").val();
    if ($('input[@name="sportstype"]:checked').val() == 'soccer') {
        _isSoccer = true;
    }
    else {
        _isSoccer = false;
    }
    var params = 'type=export&fdate=' + _date + '&issoccer=' + _isSoccer;
    $('#frm').attr('src', 'CombinationMixparlayWon.aspx?' + params);
}
