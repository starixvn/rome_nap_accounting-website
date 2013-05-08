var BingoWLMonthly = {
    month: '',
    year: '',
    GetBingoWLMonthly: function () {
        var selectedMonth = $("#cbMonth").val();
        var date = selectedMonth.split("_");
        BingoWLMonthly.month = date[0];
        BingoWLMonthly.year = date[1];
        var arr = [];
        if (date.length > 1) {
            $.ajax({
                type: "Post",
                url: "GetBingoWLByMonth",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"month":' + date[0] + ', "year":' + date[1] + '}',
                success: function (rs) {
                    arr.push(BingoWLMonthly.renderRow(rs));
                    $("#tblBingoMonthly").html(arr.join(''));
                    $("#BingoMonthly").removeClass("hidden").reRenderScroller(618, 550, true);
                }
            });
        }

    },

    renderRow: function (row) {
        var html = '';
        html = '<tr>';
        html += '<td><div class="col100 c ' + (parseFloat(row.Winlost) < 0 ? 'red' : '0') + '">' + row.Winlost + '</div></td>';
        html += '<td><div class="col100 c ' + (parseFloat(row.Position) < 0 ? 'red' : '0') + '">' + row.Position + '</div></td>';
        html += '<td><div class="col100 c ' + (parseFloat(row.Commission) < 0 ? 'red' : '0') + '">' + row.Commission + '</div></td>';
        html += '<td><div class="col100 c ' + (parseFloat(row.LineTotal) < 0 ? 'red' : '0') + '">' + row.LineTotal + '</div></td>';
        html += '<td><div class="col100 c ' + (parseFloat(row.Turnover) < 0 ? 'red' : '0') + '">' + row.Turnover + '</div></td>';
        html += '<td><div class="col100 c ' + (parseFloat(row.Margin) < 0 ? 'red' : '0') + '">' + row.Margin + '</div></td>';
        return html;
    },

    exportExcelBingoMonthlyWL: function () {
        var url = 'Index?IsExport=1&month=' + BingoWLMonthly.month + '&year=' + BingoWLMonthly.year;
        $('#frm').attr('src', url);
    }

   
};