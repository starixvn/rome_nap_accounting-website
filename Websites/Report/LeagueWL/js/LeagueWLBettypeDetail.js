var leagueWLBettypeDetail = {
    title: '',
    sportId: 0,
    leagueId: 0,
    bettypeId: 0,
    isLive: false,
    isHedge: false,
    wlDate: '',
    BARM: {
        IsActive: false,
        DisplayName: ''
    },
    getLeagueWLBettypeDetail: function (leagueId, sportId, bettype, iLive, isHedge, wldate, title) {
        try { BlockUI(); } catch (e) { }
        leagueWLBettypeDetail.sportId = sportId;
        leagueWLBettypeDetail.leagueId = leagueId;
        leagueWLBettypeDetail.bettypeId = bettype;
        leagueWLBettypeDetail.isLive = iLive;
        leagueWLBettypeDetail.isHedge = isHedge;
        leagueWLBettypeDetail.wlDate = wldate;
        //try { parent.BlockUI(); } catch (e) { };
        var data = { 'winlostDate': wldate, 'sportId': sportId, 'leagueId': leagueId, 'bettypeId': bettype, 'isLive': iLive, 'isHedge': isHedge };
        $.ajax({
            type: "POST",
            url: "Service/LeagueWinLossService.svc/SelLeagueWLDetailByBettypes",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs) {
                    leagueWLBettypeDetail.title = title;
                    $('#leaguewldetailgridtitle').html(leagueWLBettypeDetail.title);
                    $("#dialogLeagueWLDetail").dialog('option', 'title', 'League Winloss Report');
                    var arr = [];
                    var totalitems = rs.Items.length;
                    for (var j = 0; j < totalitems; j++) {
                        var row = rs.Items[j];
                        arr.push(leagueWLBettypeDetail.renderLeagueWLDetail(row, (j % 2 == 0 ? "bgeven" : "")));
                    }
                    //footer
                    //grand total 1
                    var arrFooter = [];
                    arrFooter.push(leagueWLBettypeDetail.renderLeagueWLFooterDetail(rs.GrandTotal, 'footer'));
                    arrFooter.push(leagueWLBettypeDetail.renderLeagueWLFooter('8RM Total', rs.GrandTotal8RM, 'footer'));

                    if (leagueWLBettypeDetail.BARM.IsActive == 'True') {
                        arrFooter.push(leagueWLBettypeDetail.renderLeagueWLFooter(leagueWLBettypeDetail.BARM.DisplayName + ' Total', rs.GrandTotalBARM, 'footer'));
                    }

                    arrFooter.push(leagueWLBettypeDetail.renderLeagueWLFooter('Grand Total', rs.LineTotal, 'footer'));
                    //------------------
                    $("#tblleaguewldetail").empty().html(arr.join(''));
                    $("#tblfooterleaguewldetail").empty().html(arrFooter.join(''));
                    //------------------
                    $("#dialogLeagueWLDetail").removeClass("hidden");
                    $("#dialogLeagueWLDetail").dialog("open");
                    //------------------
                    $("#leaguewldetailgrid").reRenderScroller(924, 541, true);
                }
                else {

                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderLeagueWLDetail: function (row, cssClass) {

        var html = '<tr class="' + cssClass + '">\
                        <td>\
                            <div class="col100 c">' + row.Code + '</div>\
                        </td>\
                        <td>\
                            <div class="col200 c">' + row.Event + '</div>\
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
    renderLeagueWLFooterDetail: function (row, cssClass) {

        var html = '<tr class="' + cssClass + '">\
                        <td colspan="2">\
                            <div class="col303 r">' + row.Event + '</div>\
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
                        <td colspan="5">\
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
    exportExcel: function () {
        var url = 'LeagueWLSporttype_New.aspx?isexport=true&rtype=3&rtitle=' + leagueWLBettypeDetail.title +
        '&wldate=' + leagueWLBettypeDetail.wlDate + '&sportId=' + leagueWLBettype.sportId + '&leagueId=' + leagueWLBettype.leagueId +
            '&bettypeId=' + leagueWLBettypeDetail.bettypeId + '&isLive=' + leagueWLBettypeDetail.isLive + '&isHedge=' + leagueWLBettypeDetail.isHedge;
        $("#frm").attr('src', url);
    }
};
