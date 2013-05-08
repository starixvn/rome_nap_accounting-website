var DailyWLMP = {
    winlossDate: '',
    title: '',
    GetMainDetailMP: function () {
        DailyWLMP.winlossDate = $('#fdate').val();
        var d1 = new Date(DailyWLMP.winlossDate);
        var d2 = new Date(datewinloss);
        if (d1 < d2) {
            var url = "DailyWL_MixParlay.aspx?&fdate=" + DailyWLMP.winlossDate + "&path=Mix Parlay&db=Rep&bettype=9&ispopup=false";
            $(location).attr('href', url);
        }
        else 
            DailyWLMP.GetDetailMP(DailyWLMP.winlossDate);
    },
    GetDetailMP: function (fdate) {
        // DailyWLMP.winlossDate = $('#fdate').val();
        var param = { wldate: fdate, bettype: 9 };
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ReportService.svc/GetMixParlayDetailDailyWl",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                var Items = rs.ListLiveType;
                DailyWLMP.title = 'MIX PARLAY PRODUCT WIN/LOSS REPORT FOR ' + fdate;
                $('#gridtitle').html(DailyWLMP.title);
                if (Items != null & Items.length > 0) {
                    var arr = [];
                    var arrFooter = [];

                    /* arr.push(DailyWLMP.RenderRow(rs.ListLiveType[1], "live"));
                    arr.push(DailyWLMP.RenderRow(rs.ListLiveType[0], "nonlive"));
                    arr.push(DailyWLMP.RenderRow(rs.ListLiveType[2], "mix"));*/
                    /*for (var i = 0; i < rs.ListLiveType.length; i++) {
                    if(i != 1)
                    arr.push(DailyWLMP.RenderRow(rs.ListLiveType[i]));
                    }*/
                    if (Items.length > 2) {

                        arr.push(DailyWLMP.RenderRow(rs.ListLiveType[1]));
                        arr.push(DailyWLMP.RenderRow(rs.ListLiveType[0]));
                        arr.push(DailyWLMP.RenderRow(rs.ListLiveType[2]));
                    }
                    else if (Items.length > 1) {
                        if (rs.ListLiveType[1].LiveName == "Live" && rs.ListLiveType[0].LiveName == "Non Live") {

                            arr.push(DailyWLMP.RenderRow(rs.ListLiveType[1]));
                            arr.push(DailyWLMP.RenderRow(rs.ListLiveType[0]));
                        }
                        else {

                            arr.push(DailyWLMP.RenderRow(rs.ListLiveType[0]));
                            arr.push(DailyWLMP.RenderRow(rs.ListLiveType[1]));
                        }
                    }
                    else if (Items.length > 0) {

                        arr.push(DailyWLMP.RenderRow(rs.ListLiveType[0]));
                    }
                    arrFooter.push(DailyWLMP.renderGrandTotal(rs.GrandTotal));
                    $("#tblMixparlay").empty().html(arr.join(''));
                    $("#tblFooter").empty().html(arrFooter.join(''));

                }
                else {
                    $("#tblMixparlay").empty().html('<tr><td colspan="9"><div class="c" style="width:1104px">There is no information available.</div></td></tr>');
                    $("#tblFooter").empty();
                }
                $("#MixparlayDetails").removeClass('hidden').reRenderScroller(1107, 600, true);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    RenderRow: function (row) {
        var html = '';
        var totalLive = row.ListSportType.length;
        var totalItems = row.TotalItems;
        html = '<tr>';
        var css = '';
        if (row.LiveName == 'Live')
            css = "live";
        else if (row.LiveName == 'Non Live')
            css = "nonlive"
        else
            css = "mix";
        html += '<td rowspan="' + totalItems + '" class="' + css + '"><div class="col120 c b">' + row.LiveName + '</div></td>'
        for (var i = 0; i < totalLive; i++) {
            var item = row.ListSportType[i];
            if (i > 0)
                html += '<tr>';

            html += '<td rowspan="' + item.ListWlDetail.length + '"><div class="col120 c b">' + item.SportName + '</div></td>';
            for (var j = 0; j < item.ListWlDetail.length; j++) {
                if (j > 0)
                    html += '<tr>';
                html += '<td><div class="col120 l">' + item.ListWlDetail[j].Status + '</div></td>\
                        <td><div class="col120 r ' + (parseFloat(item.ListWlDetail[j].WinLoss) > 0 ? 'red' : '0') + '">' + item.ListWlDetail[j].WinLoss + '</div></td>\
                        <td><div class="col120 r">' + item.ListWlDetail[j].Position + '</div></td>\
                        <td><div class="col120 r">' + item.ListWlDetail[j].Comm + '</div></td>\
                        <td><div class="col120 r ' + (parseFloat(item.ListWlDetail[j].LineTotal) > 0 ? 'red' : '0') + '">' + item.ListWlDetail[j].LineTotal + '</div></td>\
                        <td><div class="col120 r">' + item.ListWlDetail[j].Turnover + '</div></td>\
                        <td><div class="col120 r ' + (parseFloat(item.ListWlDetail[j].Margin) > 0 ? 'red' : '0') + '">' + item.ListWlDetail[j].Margin + '%</div></td>';

                html += '</tr>';
            }
            html += '<tr>\
                        <td><div class="col120 c">&nbsp;</div></td>\
                        <td style="background-color:#C4BD97"><div class="col120 c b" >Sub Total</div></td>\
                        <td style="background-color:#C4BD97"><div class="col120 r b ' + (parseFloat(item.SubTotal.WinLoss) > 0 ? 'red' : '0') + '" >' + item.SubTotal.WinLoss + '</div></td>\
                        <td style="background-color:#C4BD97"><div class="col120 r b">' + item.SubTotal.Position + '</div></td>\
                        <td style="background-color:#C4BD97"><div class="col120 r b">' + item.SubTotal.Comm + '</div></td>\
                        <td style="background-color:#C4BD97"><div class="col120 r b ' + (parseFloat(item.SubTotal.LineTotal) > 0 ? 'red' : '0') + '">' + item.SubTotal.LineTotal + '</div></td>\
                        <td style="background-color:#C4BD97"><div class="col120 r b">' + item.SubTotal.Turnover + '</div></td>\
                        <td style="background-color:#C4BD97"><div class="col120 r b ' + (parseFloat(item.SubTotal.Margin) > 0 ? 'red' : '0') + '">' + item.SubTotal.Margin + '%</div></td>\
                    </tr>';

        }
        html += '<tr>\
                        <td><div class="col120 c">&nbsp;</div></td>\
                        <td style="background-color:#215967"><div class="col120 c" >&nbsp;</div></td>\
                        <td style="background-color:#215967"><div class="col120 c b white">Total</div></td>\
                        <td style="background-color:#215967"><div class="col120 r b ' + (parseFloat(row.Total.WinLoss) > 0 ? 'red' : 'white') + '">' + row.Total.WinLoss + '</div></td>\
                        <td style="background-color:#215967"><div class="col120 r b white">' + row.Total.Position + '</div></td>\
                        <td style="background-color:#215967"><div class="col120 r b white">' + row.Total.Comm + '</div></td>\
                        <td style="background-color:#215967"><div class="col120 r b ' + (parseFloat(row.Total.LineTotal) > 0 ? 'red' : 'white') + '">' + row.Total.LineTotal + '</div></td>\
                        <td style="background-color:#215967"><div class="col120 r b white">' + row.Total.Turnover + '</div></td>\
                        <td style="background-color:#215967"><div class="col120 r b ' + (parseFloat(row.Total.Margin) > 0 ? 'red' : 'white') + '">' + row.Total.Margin + '%</div></td>\
                    </tr>';
        return html;
    },
    renderGrandTotal: function (row) {
        var html = '';
        html = '<tr>\
                        <td colspan="3" style="background-color:#D8E4BC"><div class="c b" style="width:366px">Grand Total</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col120 r b ' + (parseFloat(row.WinLoss) > 0 ? 'red' : '0') + '">' + row.WinLoss + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col120 r b">' + row.Position + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col120 r b">' + row.Comm + '</div></td>\
                        <td style="background-color:#B5C4D0"><div class="col120 r b ' + (parseFloat(row.LineTotal) > 0 ? 'red' : '0') + '">' + row.LineTotal + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col120 r b ">' + row.Turnover + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col120 r b ' + (parseFloat(row.Margin) > 0 ? 'red' : '0') + '">' + row.Margin + '%</div></td>\
                    </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWL_MixParlayAdvance.aspx?isExport=true&wldate=' + DailyWLMP.winlossDate + '&title=' + DailyWLMP.title + '&bettype=9';
        $("#frm").attr('src', url);
    },

    viewReportYT: function (date) {
        var fdate = '';
        if (date == 1)
            fdate = $('#btnToday').val();
        else
            fdate = $('#btnYesterday').val();
        $('#fdate').val(fdate);
        DailyWLMP.GetDetailMP(fdate);

    }

}