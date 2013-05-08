var dailyWLCasino = {
    winlostdate: '',
    title: '',
    GetDetailDailyWLLiveCasino: function () {
        dailyWLCasino.winlostdate = $('#fdate').val();
        var param = { winlostdate: dailyWLCasino.winlostdate };
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/LiveCasinoReportService.svc/GetDailyWLDirect",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                dailyWLCasino.title = "LIVE CASINO PRODUCT WIN/LOSS REPORT"
                $("#gridtitle").html(dailyWLCasino.title);
                var Items = rs.Items;
                var arr = [];
                var arr_total = [];
                if (Items != null && Items.length > 0) {
                    var numberitems = Items.length;
                    for (var i = 0; i < numberitems; i++) {
                        arr.push(dailyWLCasino.renderMainRow(Items[i], (i % 2 == 0 ? "bgeven" : "")));
                    }
                    arr.push(dailyWLCasino.addLine());
                    arr.push(dailyWLCasino.renderTotalRow(rs.Total));
                    $("#tblLiveCasino").empty().html(arr.join(''));

                }
                else {
                    $("#tblLiveCasino").empty().html('<tr><td colspan="10"><div class="c" style="width:1050px">There is no information available.</div></td></tr>');
                    $("#tblFooter").empty();
                }

                $("#LiveCasinoDetails").removeClass("hidden").reRenderScroller(861, 342, true);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    renderMainRow: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="col120 l b">' + row.bettype + '</div></td>\
                <td><div class="col120 r ' + (parseFloat(row.winloss) > 0 ? 'red' : '') + '">' + row.winloss + '</div></td>\
                <td><div class="col120 r">' + row.position + '</div></td>\
                <td><div class="col120 r">' + row.comm + '</div></td>\
                <td><div class="col120 r ' + (parseFloat(row.linetotal) > 0 ? 'red' : '') + '">' + row.linetotal + '</div></td>\
                <td><div class="col120 r">' + row.turnover + '</div></td>\
                <td><div class="col120 r ' + (parseFloat(row.margin) > 0 ? 'red' : '') + '">' + row.margin + '%</div></td>\
            </tr>';
        return html;
    },
    addLine: function () {
        var html = '';
        html = '<tr>\
                    <td colspan="7">&nbsp;</td>\
                </tr>';
        return html;
    },
    renderTotalRow: function (row) {
        var html = '';
        var margin = row.margin;
        if (row.margin.trim() != "N/A") {
            margin = row.margin + "%";
        }
        html = '<tr>\
                <td><div class="col120 r b">All Total</div></td>\
                <td><div class="col120 r b ' + (parseFloat(row.winloss) > 0 ? 'red' : '') + '">' + row.winloss + '</div></td>\
                <td><div class="col120 r b">' + row.position + '</div></td>\
                <td><div class="col120 r b">' + row.comm + '</div></td>\
                <td style="background-color:#B5C4D0"><div class="col120 r b ' + (parseFloat(row.linetotal) > 0 ? 'red' : '') + '">' + row.linetotal + '</div></td>\
                <td><div class="col120 r b">' + row.turnover + '</div></td>\
                <td><div class="col120 r b ' + (parseFloat(row.margin) > 0 ? 'red' : '') + '">' + margin + '</div></td>\
            </tr>';
        return html;
    },
    exportToExcel: function () {
        var url = 'DailyWL_LiveCasino.aspx?isExport=true&wldate=' + dailyWLCasino.winlostdate + '&reportTitle=' + dailyWLCasino.title;
        $("#frm").attr('src', url);
    },
    viewReportYT: function (date) {
        var fdate = '';
        if (date == 1)
            fdate = $('#btnToday').val();
        else
            fdate = $('#btnYesterday').val();
        $('#fdate').val(fdate);
        dailyWLCasino.GetDetailDailyWLLiveCasino();

    }

}