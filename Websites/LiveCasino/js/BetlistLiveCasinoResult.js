/*--End global variable declaretion--*/
$(document).ready(function () {
    resultbetlist.getNumberGameResult();
});


var resultbetlist = {
    matchId: "",
    title: "",
    getNumberGameResult: function () {
        resultbetlist.matchId = matchId;
        if (matchId != null) {
            var param = { matchId: resultbetlist.matchId, bettype: bettype };
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                type: "POST",
                url: "../Report/Service/LiveCasinoReportService.svc/GetBetlistResultLiveCasino",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                success: function (rs) {
                    var bettypeName = '';
                    if (parseInt(bettype) == 1101)
                        bettypeName = "Baccarat"
                    else if (parseInt(bettype) == 1102)
                        bettypeName = "Live Roulette"
                    resultbetlist.title = bettypeName + " " + rs[0].GameType + " - Result";
                    $("#gridtitle").html(resultbetlist.title);
                    var arr = [];
                    if (rs != null && rs.length > 0) {
                        arr.push(resultbetlist.renderLeagueRow(rs[0]));
                        var numberitems = rs.length;
                        for (var i = 0; i < numberitems; i++) {
                            arr.push(resultbetlist.renderMainRow(rs[i]));
                        }

                        $("#tblLiveCasino").empty().html(arr.join(''));

                    }
                    else {
                        $("#tblLiveCasino").empty().html('<tr><td colspan="10"><div class="c" style="width:715px">There is no information available.</div></td></tr>');
                        $("#tblFooter").empty();
                    }

                    $("#LiveCasinoDetails").removeClass("hidden").reRenderScroller(715, 342, true);

                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
    },
    renderLeagueRow: function (row) {
        var finalscore = "";
        var html = "";
        if (parseInt(bettype) == 1101)
            finalscore = "P - B";
        html = '<tr>\
                <td colspan="5" style="background-color:#B4B4B4"><span style="margin-left:143px; font-weight:bold">' + league + '</span><span style="margin-left:127px; font-weight:bold;">' + finalscore + '</span></div></td>\
            </tr>';
        return html;
    },
    renderMainRow: function (row) {
        var html = '';
        //var gametype = bettypeName + ' ' + row.GameType;
        var FinalScore = '';
        var arr = [];
        if (parseInt(bettype) == 1101) {
            FinalScore = row.FinalHomeScore + ' - ' + row.FinalAwayScore;
            html = '<tr>\
                <td><div class="col140 c ">' + row.KickoffTime + '</div></td>\
                <td><div class="col200 l ">' + row.GameType + '</div></td>\
                <td><div class="col100 c">' + FinalScore + '</div></td>\
                <td><div class="col180 c">' + row.Results + '</div></td>\
                <td><div class="col80 c">' + row.Status + '</div></td>\
            </tr>';
        }
        else if (parseInt(bettype) == 1102) {
            FinalScore = row.FinalHomeScore;
            var html = '';
            var result = '';
            html = '<tr>';
            html += '<td><div class="col140 c ">' + row.KickoffTime + '</div></td>';
            html += '<td><div class="col200 l ">' + row.GameType + '</div></td>';
            html += '<td><div class="col100 c">' + FinalScore + '</div></td>';
            if (row.Results != null && row.Results != '') {
                arr = row.Results.split(';');
                for (var i = 0; i < arr.length; i++) {
                    if (i > 0 && i % 3 == 0) {
                        result += (i == arr.length - 1) ? '<br/>' + arr[i] : '<br/>' + arr[i] + ';&nbsp;';
                    }
                    else if (i == 0 || i % 3 != 0) {
                        result += (i == arr.length - 1) ? arr[i] : arr[i] + ';&nbsp;';
                    }
                }
            }
            else {
                result = '-';
            }
            html += '<td><div class="col180 c">' + result + '</div></td>';
            html += '<td><div class="col80 c">' + row.Status + '</div></td>';
            html += '</tr>';

        }

        return html;
    }
}


