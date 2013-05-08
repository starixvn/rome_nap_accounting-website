var duplicateticket = {
    btnReport: function () {
        try { parent.BlockUI(); } catch (e) { }
     
        var duplicateticketgrid = $("#duplicateticket");
        var arr = [];

        $.ajax({
            type: "POST",
            url: "Services/OthersReportService.svc/SelDuplicateTickets",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"wldate":"' + $("#date").val() + '"}',
            success: function (rs) {
                var tbl = $("#tblduplicateticket");
                if (rs.Items.length > 0) {
                   

                    $.each(rs.Items, function (i, row) {
                        arr.push(duplicateticket.renderRow(row));
                    });
                    tbl.html(arr.join(''));
                    duplicateticketgrid.removeClass("hidden").reRenderScroller(1199, 542, true);
                  
                }
                else 
                {
                    tbl.html('<tr><td><div class="c b" style="width:1196px;">There is no data.</div></td></tr>');
                    duplicateticketgrid.removeClass("hidden").reRenderScroller(1199, 542, true);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderRow: function (row) {
        var htmlRow = '';
        htmlRow = '<tr>\
                <td>\
                    <div class="col80 c">' + row.Username + '</div>\
                </td>\
                <td >\
                    <div class="col80 c">' + row.CustId + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + row.Bettype + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + row.WinLostDate + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + row.BalanceBeforePlaceBet + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + row.Stake + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + row.Odds + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + row.Status + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + row.RefNo1stTicket + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + row.RefNo2ndTicket + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.BetTime1stTicket + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.BetTime2ndTicket + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.TimeDiff + '</div>\
                </td>\
            </tr>';
        return htmlRow;
    }
};
