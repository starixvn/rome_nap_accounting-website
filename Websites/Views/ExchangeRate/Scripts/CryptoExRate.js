const fromDatePicker="txtCryptoExRateFromDate";
const toDatePicker="txtCryptoExRateToDate";
var CryptoExRate = {
    fdate: '',
    tdate: '',
    renderCalendar: function (ids, opts) {
        var defaults = {
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true
        };
        var new_opts = $.extend(true, defaults, opts);
        var calendar = $("#" + ids);
        var dates = calendar.datepicker(new_opts);
        calendar.datepicker("setDate", new Date(dates));
    },
    setupCalendar: function (id, toDate) {
        var limitDate = {
            maxDate: toDate
        };
        CryptoExRate.renderCalendar(id, limitDate);
        $('#' + id).click(function () {
            $('#' + id).datepicker("show");
        });
    },
    renderRow: function (rows, endRow) {
        var strrow = '';
        var items = rows.ExchangeDailyList;
        strrow += '<tr>';
        if (endRow)
            strrow += '<td rowspan="' + items.length + '" style="border-bottom:0px none"><div class="col120 c">' + rows.ExchangeDate + '</div></td>';
        else
            strrow += '<td rowspan="' + items.length + '"><div class="col120 c">' + rows.ExchangeDate + '</div></td>';
        for (var i = 0; i < items.length; i++) {
            var listBaseCurr = items[i].BaseCurrencies;
            if (i == 0) {
                strrow += '<td><div class="col100 c">' + items[i].CurrencyCode + '</div></td>';
                for (var j = 0; j < listBaseCurr.length; j++) {
                    strrow += '<td><div class="col80 r">' + listBaseCurr[j].FormatedValue + '</div></td>';
                }
                strrow += '</tr>';
            }
            else {
                strrow += '<tr>';
                strrow += '<td><div class="col100 c">' + items[i].CurrencyCode + '</div></td>';
                for (var j = 0; j < listBaseCurr.length; j++) {
                    strrow += '<td><div class="col80 r">' + listBaseCurr[j].FormatedValue + '</div></td>';
                }
                strrow += '</tr>';
            }
` `
        }
        return strrow;

    },
    loadDataCryptoExRate: function () {
        let arr = [];
        let endRow = false;
        CryptoExRate.fdate = $("#"+fromDatePicker).val();
        CryptoExRate.tdate = $("#"+toDatePicker).val();
        const params = { fdate: CryptoExRate.fdate, tdate: CryptoExRate.tdate };
        try { parent.BlockUI(); } catch (e) {
            console.log("error:",e) }
        
        $.ajax({
            type: "POST",
            url: "GetExchangeDailyCrypto",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (rs) {
                console.log("data",rs)
                var width = 243 + (80 + 2) * baseCryptoExRateCurrencyLength;
                var height = 550;
                if (rs.length > 0) {
                    for (var i = 0; i < rs.length; i++) {
                        if (i == rs.length - 1)
                            endRow = true;
                        arr.push(CryptoExRate.renderRow(rs[i], endRow));
                    }
                    $("#tblCryptoExRate").html(arr.join(''));
                    $("#CryptoExRate").removeClass("hidden").reRenderScroller(width, height, true);
                }
                else {
                    $("#tblCryptoExRate").html('<tr><td colspan="' + (baseCryptoExRateCurrencyLength + 2) + '" style="border-right:0px none"><div class="c b" style="width:' + width + 'px">There is no information available</div></td></tr>');
                    $("#CryptoExRate").removeClass("hidden").reRenderScroller(width, height, true);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    exportExcelCryptoExRate: function () {
        var url = 'Index?IsExport=1&fdate=' + CryptoExRate.fdate + '&tdate=' + CryptoExRate.tdate +'&typeExport=Crypto';
        $('#frm').attr('src', url);
    },
    
};

$(document).ready(function () {
    CryptoExRate.setupCalendar(fromDatePicker, today);
    CryptoExRate.setupCalendar(toDatePicker, today);
}); 