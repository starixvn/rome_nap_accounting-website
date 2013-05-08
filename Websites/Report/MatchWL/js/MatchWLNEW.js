var dateReport;
function ExportToExcel() {
    try { parent.BlockUI() } catch (e) { }
    $("#frm").attr('src', 'MatchWLNEW.aspx?IsExport=1&dateReport=' + dateReport + '&fdate=' + $('#fdate').val());
    try { parent.jQuery.unblockUI() } catch (e) { };
}
function ViewReport(date) {
    matchWL2a.currIndex = 0;
    $("#tblMatchWLDetail").empty();
    matchWL2a.getWLByBettype(date);
    matchWL2a.getMatchWL(date);
}
var matchWL2a = {
    currIndex: 0,
    NextBtypeIndex: 0,
    isActiveBARM: false,
    displayNameBARM: '',
    getWLByBettype: function (date) {
        dateReport = date;
        var d;
        if (date == 1) {
            d = $('#btnToday').val();

        }
        else if (date == 2) {
            d = $('#btnYesterday').val();

        }
        else {
            d = $('#fdate').val();
        }
        $('#fdate').val(d);
        var data = {
            'wldate': d
        };
        //try{parent.BlockUI();}catch(e){};
        $.ajax({
            type: "POST",
            url: "../Service/ReportService.svc/SelWLbyBettype",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null) {
                    $("#tbWLByBettype").html(matchWL2a.renderWLByBettypeRows(rs.Items, ""));
                    $("#tblfooterWLByBettype").html(matchWL2a.renderWLByBettypeRows(new Array(rs.Total), ""));
                    $("#WLByBettype").removeClass("hidden").reRenderScroller(1036, 300, true);
                }
                else {
                    $("#WLByBettype").addClass("hidden");
                    showAlertMessage("err", "There is no data.", 1);
                }
                //try{parent.jQuery.unblockUI();}catch(e){}
            }
        });
    },
    getMatchWL: function (date) {
        var d;
        if (date == 1) {
            d = $('#btnToday').val();
        }
        else if (date == 2) {
            d = $('#btnYesterday').val();
        }
        else {
            d = $('#fdate').val();
        }

        if (matchWL2a.currIndex != -1) {
            var data = {
                'wldate': d,
                'IsFromCache': false,
                'currIndex': matchWL2a.currIndex,
                'currIndexBtype': matchWL2a.NextBtypeIndex
            };
            if (matchWL2a.currIndex == 0 && matchWL2a.NextBtypeIndex == 0) {
                try { parent.BlockUI(); } catch (e) { };
            }
            $.ajax({
                type: "POST",
                url: "../Service/ReportService.svc/SelMatchWLBySport",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    var arr = [];
                    if (matchWL2a.currIndex == 0 && matchWL2a.NextBtypeIndex == 0) {
                        matchWL2a.getMatchWLFooter(d);
                    }
                    if (rs.Item != null) {
                        var len = rs.Item.Items.length;
                        for (var i = 0; i < len; i++) {
                            var league = rs.Item.Items[i];
                            for (var j = 0; j < league.Items.length; j++) {
                                var live = league.Items[j];
                                for (var k = 0; k < live.Items.length; k++) {
                                    var row = live.Items[k];
                                    arr.push(matchWL2a.renderMatchWLRow1(row));
                                }
                                arr.push(matchWL2a.renderTotalRow(live.Total, "total1"));
                            }
                        }

                        var t = setTimeout(function () {
                            if (rs.Item.Total != null) {
                                arr.push(matchWL2a.renderTotalRow(rs.Item.Total, "total2"));
                            }
                            $("#tblMatchWLDetail").append(arr.join(''));
                            if (rs.NextIndex != -1) {
                                var r = setTimeout(function () { matchWL2a.getMatchWL(date); }, 100);
                            }
                            else {
                                try { parent.jQuery.unblockUI(); } catch (e) { };

                            }

                        }, 100);
                        if (rs.NextIndex == 0) {
                            try { parent.jQuery.unblockUI(); } catch (e) { };
                            $("#MatchWL").removeClass("hidden");

                        }
                        var t = setTimeout(function () {
                            $("#MatchWL").reRenderScroller(1056, 340, true, false);
                        }, 500);

                        matchWL2a.currIndex = rs.NextIndex;
                        matchWL2a.NextBtypeIndex = rs.NextBtypeIndex;
                    }
                    else {
                        try { parent.jQuery.unblockUI(); } catch (e) { };
                    }

                }

            });
        }
    },
    getMatchWLFooter: function (date) {
        var data = {
            'wldate': date
        };
        try { parent.BlockUI(); } catch (e) { };
        $.ajax({
            type: "POST",
            url: "../Service/ReportService.svc/SelMatchWl3RdFooter",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null) {
                    var tbl3rd = $("#tblMatchWL3rd");
                    tbl3rd.html(matchWL2a.renderTotalRow(rs.GrandTotal1, "grandtotal"));
                    tbl3rd.append(matchWL2a.render8RMRow(rs.wl8rm, rs.wlBARM));
                    tbl3rd.append(matchWL2a.renderMatchWL3rdRows(rs.Items));
                    $("#MatchWLGrandTotal").html(matchWL2a.renderGrandTotalRow(rs.GrandTotal2));
                    $("#MatchWL").removeClass("hidden").reRenderScroller(1056, 390, true);

                }
                try { parent.jQuery.unblockUI() } catch (e) { };
            }
        });
    },
    renderWLByBettypeRow: function (row) {
        var tpl = '<tr><td><div class="c col120">' + row.bettype + '</div></td><td><div class="r col80">' + row.LineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.LineTotal_soccer + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer + '</div></td><td><div class="r col80">' + row.LineTotal_others_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others_live + '</div></td><td><div class="r col80">' + row.LineTotal_others + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others + '</div></td><td><div class="r col80">' + row.LineTotalReal + '</div></td><td><div class="r col80">' + row.LineTotalVirtual + '</div></td><td><div class="r col80">' + row.Variance + '</div></td></tr>';
        $("#tbWLByBettype").append(tpl);
    },
    renderWLByBettypeRows: function (rows, cssClass) {

        var len = rows.length;
        var arr = [];
        for (var i = 0; i < len; i++) {
            var row = rows[i];
            var tpl = '<tr class="' + cssClass + '"><td><div class="c col120">' + row.bettype + '</div></td><td><div class="r col80">' + row.LineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.LineTotal_soccer + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer + '</div></td><td><div class="r col80">' + row.LineTotal_others_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others_live + '</div></td><td><div class="r col80">' + row.LineTotal_others + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others + '</div></td><td><div class="r col80">' + row.LineTotalReal + '</div></td><td><div class="r col80">' + row.LineTotalVirtual + '</div></td><td><div class="r col80">' + row.Variance + '</div></td></tr>';
            if (parseInt(row.bettypeId) == 1201 || parseInt(row.bettypeId) == 1203 || parseInt(row.bettypeId) == 1204 || parseInt(row.bettypeId) == 1205 || parseInt(row.bettypeId) == 1206 || parseInt(row.bettypeId) == 1224)
                tpl = '<tr class="' + cssClass + '"><td><div class="c col120">' + row.bettype + '<br/>(Virtual Sports)</div></td><td><div class="r col80">' + row.LineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.LineTotal_soccer + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer + '</div></td><td><div class="r col80">' + row.LineTotal_others_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others_live + '</div></td><td><div class="r col80">' + row.LineTotal_others + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others + '</div></td><td><div class="r col80">' + row.LineTotalReal + '</div></td><td><div class="r col80">' + row.LineTotalVirtual + '</div></td><td><div class="r col80">' + row.Variance + '</div></td></tr>';
            if (row.bettype.trim() == "Mix Parlay") {
                var dateChoose = $('#fdate').val();
                var d1 = new Date(dateChoose);
                var d2 = new Date(dateWL);
                var url = '';
                if (d1.getTime() >= d2.getTime()) {
                    // chon template MixParlay_Advance
                    //var tpl = '<tr class="' + cssClass + '"><td><div class="c col120"><a href="../DailyWL_MixParlayAdvance.aspx?&fdate=' + dateChoose + '&path=' + row.bettype + '&db=Rep&bettype=9">' + row.bettype + '</a></div></td><td><div class="r col80">' + row.LineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.LineTotal_soccer + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer + '</div></td><td><div class="r col80">' + row.LineTotal_others_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others_live + '</div></td><td><div class="r col80">' + row.LineTotal_others + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others + '</div></td><td><div class="r col80">' + row.LineTotalReal + '</div></td><td><div class="r col80">' + row.LineTotalVirtual + '</div></td><td><div class="r col80">' + row.Variance + '</div></td></tr>';
                    url = '../DailyWL_MixParlayAdvance.aspx?&fdate=' + dateChoose + '&path=' + row.bettype + '&db=Rep&bettype=9&ispopup=true';
                }
                else {
                    //var tpl = '<tr class="' + cssClass + '"><td><div class="c col120"><a href="../DailyWL_MixParlay.aspx?fdate=' + dateChoose + '&path=' + row.bettype + '&db=Rep&bettype=9">' + row.bettype + '</a></div></td><td><div class="r col80">' + row.LineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.LineTotal_soccer + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer + '</div></td><td><div class="r col80">' + row.LineTotal_others_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others_live + '</div></td><td><div class="r col80">' + row.LineTotal_others + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others + '</div></td><td><div class="r col80">' + row.LineTotalReal + '</div></td><td><div class="r col80">' + row.LineTotalVirtual + '</div></td><td><div class="r col80">' + row.Variance + '</div></td></tr>';
                    url = '../DailyWL_MixParlay.aspx?&fdate=' + dateChoose + '&path=' + row.bettype + '&db=Rep&bettype=9&ispopup=true';
                }
                tpl = '<tr class="' + cssClass + '"><td><div class="c col120"><span style="color:#0066CC; cursor:pointer" onclick="matchWL2a.MixparlayDetail(\'' + dateChoose + '\',\'' + url + '\')">' + row.bettype + '</span></div></td><td><div class="r col80">' + row.LineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.LineTotal_soccer + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer + '</div></td><td><div class="r col80">' + row.LineTotal_others_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others_live + '</div></td><td><div class="r col80">' + row.LineTotal_others + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others + '</div></td><td><div class="r col80">' + row.LineTotalReal + '</div></td><td><div class="r col80">' + row.LineTotalVirtual + '</div></td><td><div class="r col80">' + row.Variance + '</div></td></tr>';
            }
            //tpl = '<tr class="' + cssClass + '"><td><div class="c col120">' + row.bettype + '</div></td><td><div class="r col80">' + row.LineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer_live + '</div></td><td><div class="r col80">' + row.LineTotal_soccer + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_soccer + '</div></td><td><div class="r col80">' + row.LineTotal_others_live + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others_live + '</div></td><td><div class="r col80">' + row.LineTotal_others + '</div></td><td><div class="r col80">' + row.VirtualLineTotal_others + '</div></td><td><div class="r col80">' + row.LineTotalReal + '</div></td><td><div class="r col80">' + row.LineTotalVirtual + '</div></td><td><div class="r col80">' + row.Variance + '</div></td></tr>';
            arr.push(tpl);
        }
        return arr.join('');
    },
    renderMatchWLRow: function (row) {
        var tpl = '<tr><td><div class="c col120">' + (row.liveindicator ? '<b>!Live</b> - ' : '') + row.bettype + ' ' + row.matchcode + '</div></td><td><div class="col80 c">' + row.eventdate + '</div></td><td><div class="col120 r">' + row.homename + (row.IsNeutral ? '(N)' : '') + '</div></td><td><div class="col20 c">-vs-</div></td><td><div class="col120 l">' + row.awayname + '</div></td><td><div class="col80 r">' + row.winlost + '</div></td><td><div class="col80 r">' + row.pos + '</div></td><td><div class="col80 r">' + row.comm + '</div></td><td><div class="col80 r">' + row.VirtualComm + '</div></td><td><div class="col80 r b">' + row.LineTotal + '</div></td><td><div class="col80 r b">' + row.VirtualLineTotal + '</div></td><td><div class="col80 r">' + row.variance + '</div></td></tr>';
        return tpl;
    },
    renderMatchWLRow1: function (row) {
        var tpl = '<tr><td><div class="c col120">' + (row.liveindicator ? '<b>!Live</b> - ' : '') + row.bettype + ' ' + row.matchcode + '</div></td><td><div class="col80 c">' + row.eventdate + '</div></td><td><div class="col120 r">' + row.homename + (row.IsNeutral ? '(N)' : '') + '</div></td><td><div class="col20 c">-vs-</div></td><td><div class="col120 l">' + row.awayname + '</div></td><td><div class="col80 r">' + row.winlost + '</div></td><td><div class="col80 r">' + row.pos + '</div></td><td><div class="col80 r">' + row.comm + '</div></td><td><div class="col80 r">' + row.VirtualComm + '</div></td><td><div class="col80 r b">' + row.LineTotal + '</div></td><td><div class="col80 r b">' + row.VirtualLineTotal + '</div></td><td><div class="col80 r">' + row.variance + '</div></td></tr>';
        return tpl;
    },
    renderMatchWL3rdRows: function (rows) {
        var len = rows.length;
        var casinolink = "";
        var gaminglink = "";
        var arr = [];
        for (var i = 0; i < len; i++) {
            var row = rows[i];
            gaminglink = row.bettype != "P2P" ? '<a target="_blank" href="../CasinoDetail_1.aspx?wldate=' + row.eventdate + '&bettype=' + row.bettype + '">' + row.bettype + '</a>' : row.bettype;
            casinolink = row.type3rd == 2 ? gaminglink : row.bettype;


            var tpl = '<tr><td><div class="c col120">' + casinolink + '</div></td><td><div class="col80 c">' + row.eventdate + '</div></td><td colspan="3"><div class="" style="width:266px;"></div></td><td><div class="col80 r">' + row.winlost + '</div></td><td><div class="col80 r">' + row.pos + '</div></td><td><div class="col80 r">' + row.comm + '</div></td><td><div class="col80 r">' + row.VirtualComm + '</div></td><td><div class="col80 r b">' + row.LineTotal + '</div></td><td><div class="col80 r b">' + row.VirtualLineTotal + '</div></td><td><div class="col80 r">' + row.variance + '</div></td></tr>';
            arr.push(tpl);
        }
        return arr.join('');
    },
    render8RMRow: function (value8RM, valueBARM) {
        var tpl = '<tr class="total8RM"><td colspan="9"><div class="r b" style="width:804px;">Total 8RM:</div></td><td><div class="col80 r b">' + value8RM + '</div></td><td><div class="col80 r b">' + value8RM + '</div></td><td><div class="col80 r">0.00</div></td></tr>';
        if (matchWL2a.isActiveBARM == 'True') {
            tpl += '<tr class="total8RM"><td colspan="9"><div class="r b" style="width:804px;">Total ' + matchWL2a.displayNameBARM + ':</div></td><td><div class="col80 r b">' + valueBARM + '</div></td><td><div class="col80 r b">' + valueBARM + '</div></td><td><div class="col80 r">0.00</div></td></tr>';
        }
        return tpl;
    },
    renderTotalRow: function (row, cssClass) {
        if (row == null) return '';
        var tpl = '<tr class="' + cssClass + '"><td colspan="7"><div class="r b" style="width:638px;">' + row.homename + '</div></td><td><div class="col80 r">' + row.comm + '</div></td><td><div class="col80 r">' + row.VirtualComm + '</div></td><td><div class="col80 r b">' + row.LineTotal + '</div></td><td><div class="col80 r b">' + row.VirtualLineTotal + '</div></td><td><div class="col80 r">' + row.variance + '</div></td></tr>';
        return tpl;
    },
    renderGrandTotalRow: function (row) {
        var tpl = '<tr><td colspan="7"><div class="r b" style="width:638px;">' + row.homename + '</div></td><td><div class="col80 r" style="font-weight:normal">' + row.comm + '</div></td><td><div class="col80 r" style="font-weight:normal">' + row.VirtualComm + '</div></td><td><div class="col80 r b">' + row.LineTotal + '</div></td><td><div class="col80 r b">' + row.VirtualLineTotal + '</div></td><td><div class="col80 r" style="font-weight:normal">' + row.variance + '</div></td></tr>';
        return tpl;
    },

    MixparlayDetail: function (date, url) {
        try { parent.BlockUI(); } catch (e) { }
        var title = '';
        var url = url;
        var width = 1155;
        var height = 650;
        OpenPopup(url, title, width, height, false, false, window, 0, null, null);
        try { parent.jQuery.unblockUI(); } catch (e) { };
    }

};


//********
/// Queue handler
//********
var queue = null, tInterval = null, TInterval = 10;
var Queue = function (interval) {
    this.QueueItems = [];
    window.clearInterval(tInterval);
}
Queue.prototype = {
    receive: function (data) {
        if (typeof data != undefined) {
            this.QueueItems.push(data);
        }
    },
    process: function (callBack) {
        tInterval = window.setInterval(function () {
            if (queue.QueueItems.length > 0) {
                var data = queue.QueueItems.pop();
                if (callBack && data) {
                    try { callBack(data); } catch (e) { }
                }
            }
        }, TInterval);
    }
};

$(document).ready(function () {
    var limitFromDate = {
        minDate: minDay,
        maxDate: maxDay
    };
    var options = {
        showOtherMonths: true,
        selectOtherMonths: true,
        checkMinMax: true
    };
    calendarUtils.renderCalendar("#fdate", limitFromDate, "#fdate", "", fDay, options);

    var strtoday = '<input type="button" onclick="ViewReport(1)" id="btnToday" class="btn"/>';
    var stryesterday = '<input type="button" onclick="ViewReport(2)" id="btnYesterday" class="btn"/>';
    $('#spMoreElement').append(strtoday);
    $('#spMoreElement').append(stryesterday);
    $('#btnToday').val(today);
    $('#btnYesterday').val(yesterday);
});
