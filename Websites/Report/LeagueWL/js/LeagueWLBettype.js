var leagueWLBettype = {
    title: '',
    sportId: 0,
    leagueId: 0,
    BARM: {
        IsActive: false,
        DisplayName: ''
    },
    getLeagueWLBettype: function (sportId, leagueId, leagueName) {
        $('#err').html('');
        $("#leaguewlgrid").addClass("hidden");
        $("#leaguewlbettype").addClass("hidden");
        $("#leaguewlbettypenoNeLive").addClass("hidden");
        $("#tbWLByBettype").empty();
        $("#tbWLByBettypeNoneLive").empty();
        $("#tblfooterWLByBettype").empty();
        $("#tblfooterWLByBettypeNoneLive").empty();
        try { BlockUI(); } catch (e) { }
        //try { parent.BlockUI(); } catch (e) { };
        leagueWLBettype.sportId = sportId;
        leagueWLBettype.leagueId = leagueId;
        var data = { 'winlostDate': $('#date').val(), 'sportId': sportId, 'leagueId': leagueId };
        $.ajax({
            type: "POST",
            url: "Service/LeagueWinLossService.svc/SelLeagueWLByBettypes",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs) {
                    leagueWLBettype.title = leagueName;
                    for (var i = 0; i < rs.Items.length; i++) {
                        var item = rs.Items[i];
                        var subfix = item.IsLive ? '' : 'NoneLive';
                        leagueWLBettype.title = leagueName;
                        $('#girdWLBettypeTitle' + subfix).html(leagueName + (item.IsLive ? ' - LIVE' : ' - NONE LIVE'));
                        var arr = [];
                        var totalitems = item.Items.length;
                        for (var j = 0; j < totalitems; j++) {
                            var row = item.Items[j];
                            arr.push(leagueWLBettype.renderLeagueWLDetail(row, sportId, leagueId, item.IsLive, (j % 2 == 0 ? "bgeven" : ""), false));
                        }

                        //footer
                        //grand total 1
                        var arrFooter = [];
                        arrFooter.push(leagueWLBettype.renderLeagueWLDetail(item.GrandTotal, 0, 0, item.IsLive, 'footer', true));
                        arrFooter.push(leagueWLBettype.renderLeagueWLFooter("8RM Total", item.GrandTotal8RM, 'footer'));

                        if (leagueWLBettype.BARM.IsActive == 'True') {
                            arrFooter.push(leagueWLBettype.renderLeagueWLFooter(leagueWLBettype.BARM.DisplayName + " Total", item.GrandTotalBARM, 'footer'));
                        }

                        arrFooter.push(leagueWLBettype.renderLeagueWLFooter("Grand Total", item.LineTotal, 'footer'));
                        //------------------
                        $("#tbWLByBettype" + subfix).empty().html(arr.join(''));
                        $("#tblfooterWLByBettype" + subfix).empty().html(arrFooter.join(''));
                        //------------------
                        $("#leaguewlbettype" + subfix).removeClass("hidden").reRenderScroller(741, 300, true);
                    }
                }
                else {
                    $("#leaguewlgrid").addClass("hidden");
                    $("#leaguewlbettype").addClass("hidden");
                    $("#leaguewlbettypeNoneLive").addClass("hidden");
                    showAlertMessage("err", "There is no data.", 1);
                }
            },
            error: function () {
                $("#leaguewlgrid").addClass("hidden");
                $("#leaguewlbettype").addClass("hidden");
                $("#leaguewlbettypeNoneLive").addClass("hidden");
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderLeagueWLDetail: function (row, sportId, leagueId, isLive, cssClass, isTotal) {
        var titleDetal = leagueWLBettype.title + ' - ' + row.BettypeName;

        var alink = isTotal === true ? '<div class="col120 l"><span class="indent" style="margin-right:6px;"></span>' +
            row.BettypeName + '</div>' : '<div class="col120 l blue" onclick="leagueWLBettype.getleagueWLDetail(' + leagueId + ', ' + sportId + ', ' + row.BettypeId + ', ' + isLive + ', ' + (row.IsHedge.length > 0 ? 1 : 0) + ', \'' + $('#date').val() + '\', \'' + titleDetal + '\')"><span class="indent" style="margin-right:6px;"></span>' + (row.BettypeName + (' ' + row.IsHedge)) + '</div>';

        var html = '<tr class="' + cssClass + '">\
                        <td>\
                            <div>' + alink + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r ' + (parseFloat(row.WinLoss) > 0 ? 'red' : '') + '">' + row.WinLoss + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r ">' + row.Position + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r ">' + row.Comm + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r ' + (parseFloat(row.LineTotal) > 0 ? 'red' : '') + '">' + row.LineTotal + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r ">' + row.Turnover + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r ' + (parseFloat(row.Margin) > 0 ? 'red' : '') + '">' + row.Margin + '%</div>\
                        </td>\
                </tr>';
        return html;
    },
    renderLeagueWLFooter: function (name, value, cssClass) {
        var html = '<tr class="' + cssClass + '">\
                        <td colspan="4">\
                            <div class=" r">' + name + '</div>\
                        </td>\
                        <td>\
                            <div class="r ' + (parseFloat(value) > 0 ? 'red' : '') + '">' + value + '</div>\
                        </td>\
                        <td colspan="2">\
                            <div class=" r">&nbsp;</div>\
                        </td>\
                </tr>';
        return html;
    },
    leagueWLExportExcel: function () {
        var url = 'LeagueWLSporttype_New.aspx?isexport=true&rtype=2&rtitle=' + leagueWLBettype.title +
        '&wldate=' + $('#date').val() + '&sportId=' + leagueWLBettype.sportId + '&leagueId=' + leagueWLBettype.leagueId;
        $("#frm").attr('src', url);
    },
    leagueWLGoBack: function () {
        //leagueWL.getLeagueWLSporttype(false);
        $("#leaguewlgrid").removeClass("hidden");
        $("#leaguewlbettype").addClass("hidden");
        $("#leaguewlbettypeNoneLive").addClass("hidden");
    },
    getleagueWLDetail: function (leagueId, sporttype, bettype, iLive, isHedge, wldate, title) {
        leagueWLBettypeDetail.getLeagueWLBettypeDetail(leagueId, sporttype, bettype, iLive, isHedge, wldate, title);

    }
};

$(function () {
    $("#dialogLeagueWLDetail").dialog({
        title: "League Winloss Report",
        autoOpen: false,
        height: 550,
        width: 960,
        modal: true
    });
});