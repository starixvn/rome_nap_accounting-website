var leagueWL = {
    tile: '',
    BARM: {
        IsActive: false,
        DisplayName: ''
    },
    getLeagueWLSporttype: function (isGetNew) {
        $('#leaguewlbettype').addClass('hidden');
        $('#leaguewlbettypeNonelive').addClass('hidden');
        $('#err').html('');

        var data = { 'winlostdate': $('#date').val(), 'isGetNew': isGetNew };
        try { BlockUI(); } catch (e) { }
        //try { parent.BlockUI(); } catch (e) { };
        $.ajax({
            type: "POST",
            url: "Service/LeagueWinLossService.svc/SelLeagueWLBySports",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs) {

                    $("#leaguewlbettype").addClass("hidden");
                    $("#leaguewlbettypeNoneLive").addClass("hidden");

                    leagueWL.tile = 'League Win Loss Report';
                    $('#gridtitle').html(leagueWL.tile + ' - ' + $('#date').val());
                    var arr = [];
                    if (rs.Items && rs.GrandTotal8RM && rs.LineTotal && rs.GrandTotal) {
                        for (var i = 0; i < rs.Items.length; i++) {
                            var item = rs.Items[i];
                            var sportId = item.SportId;
                            var sportName = item.SportName;

                            arr.push(leagueWL.renderSportRow(sportId, sportName));
                            //league detail
                            var totalitems = item.Items.length;
                            for (var j = 0; j < totalitems; j++) {

                                arr.push(leagueWL.renderLeagueWLDetail(item.Items[j], sportId, sportName, (j % 2 == 0 ? "bgeven" : "")));
                            }
                            //total by sporttype
                            arr.push(leagueWL.renderLeagueWLGrandTotal(item.Total, sportId, 'total'));
                        }
                        //footer
                        var arrFooter = [];
                        arrFooter.push(leagueWL.renderLeagueWLGrandTotal(rs.GrandTotal, 0, 'footer'));
                        arrFooter.push(leagueWL.renderLeagueWLFooter("8RM Total", rs.GrandTotal8RM, 'footer'));

                        if (leagueWL.BARM.IsActive == 'True') {
                            arrFooter.push(leagueWL.renderLeagueWLFooter(leagueWL.BARM.DisplayName + " Total", rs.GrandTotalBARM, 'footer'));
                        }

                        arrFooter.push(leagueWL.renderLeagueWLFooter("Grand Total", rs.LineTotal, 'footer'));
                        //------------------
                        $("#tblscroll").empty().html(arr.join(''));
                        $("#tblfooterscroll").empty().html(arrFooter.join(''));
                        //------------------
                        $("#leaguewlgrid").removeClass("hidden").reRenderScroller(921, 541, true);
                    }
                    else {
                        $("#leaguewlgrid").addClass("hidden");
                        $("#leaguewlbettype").addClass("hidden");
                        $("#leaguewlbettypeNoneLive").addClass("hidden");
                        showAlertMessage("err", "There is no data.", 1);
                    }
                }
                else {
                    $("#leaguewlgrid").addClass("hidden");
                    $("#leaguewlbettype").addClass("hidden");
                    $("#leaguewlbettypeNoneLive").addClass("hidden");
                    showAlertMessage("err", "There is no data.", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderSportRow: function (sportId, sportName) {
        var html = '<tr class="totaltitle">\
                    <td colspan="7">\
                        <div class="col918 l b"><span class="tree-expanded" onclick="leagueWL.toggleLeagueWLDetail(this,\'' + (sportId || '') + '\')"></span>' + (sportName || '') + '</div>\
                    </td>\
                </tr>';
        return html;
    },
    renderLeagueWLDetail: function (row, sportId, sportName, cssClass) {
        var html = '<tr class="s_' + sportId + ' ' + cssClass + '">\
                        <td>\
                            <div class="col300 l blue" onclick="leagueWL.openLeagueWLBettype(' + sportId + ', ' + row.LeagueId + ',\'' + (sportName + ' - ' + row.LeagueName) + '\')"><span class="indent" style="margin-right:6px;"></span>' + row.LeagueName + '</div>\
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
    renderLeagueWLGrandTotal: function (row, sport, cssClass) {
        var html = '<tr class="s_' + sport + ' ' + cssClass + '">\
                        <td>\
                            <div class="col300 r"><span class="indent" style="margin-right:6px;"></span>' + row.LeagueName + '</div>\
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
    toggleLeagueWLDetail: function (obj, sportId) {
        if ($(obj).hasClass("tree-expand")) {
            $(obj).removeClass("tree-expand");
            $("#leaguewlgrid tr.s_" + sportId).removeClass("hidden");
        }
        else {
            $("#leaguewlgrid tr.s_" + sportId).addClass("hidden");
            $(obj).addClass("tree-expand");
        }
        $("#leaguewlgrid").removeClass("hidden").reRenderScroller(921, 541, true);
    },
    leagueWLExportExcel: function () {
        var url = 'LeagueWLSporttype_New.aspx?isexport=true&rtype=1&rtitle=' + leagueWL.tile + '&wldate=' + $('#date').val();
        $("#frm").attr('src', url);
    },
    openLeagueWLBettype: function (sportId, leagueId, leagueName) {
        leagueWLBettype.getLeagueWLBettype(sportId, leagueId, leagueName);
    }
};

