$(function () {
    $("#racingDailyForTote").scroller();
    $('.btnHelp').CreateBubblePopup();

    //set customized mouseover event for each button
    $('#btnAmountDueHelp').mouseover(function () {
        //show the bubble popup with new options
        showPopup($(this), "= [Total TO Partner] + [PGI/Amtote Fee] - [Dividend Won] - [Comm Income IBC]");
    });
    $('#btnNetIncomeHelp').mouseover(function () {
        //show the bubble popup with new options
        showPopup($(this), "= [PT Win/Loss IBC] + [Comm Income IBC] - [Comm Expense] - [PGI/Amtote Fee]");
    });
  
});

function showPopup(obj, content) {
    obj.ShowBubblePopup({
        innerHtml: content,
        alwaysVisible: false,
        closingDelay: 200,
        themeName: 'azure',
        themePath: '../../js/jQuery/Bubblepopup/images/jquerybubblepopup-theme'
    });
}

//function ExportToExcel(reporttype) {
//    //try{parent.BlockUI();}catch(e){}
//    alert('export');
//    var params = 'type=export';
//    params += '&date=' + $('#date').val();
//    params += "reporttype=" + reporttype;
//    $("#frm").attr('src', 'HorseRacingDailyReportforUK.aspx?' + params);
//    //parent.jQuery.unblockUI();
//}
//function ExportToExcel(type, bettype, isLive, matchId) {
//    //try{parent.BlockUI();}catch(e){}
//    var params = 'type=' + type;
//    params += '&date=' + $('#date').val();
//    params += '&isLive=' + isLive;
//    params += '&matchid=' + matchId;
//    params += '&bettype=' + bettype;

//    $("#frm").attr('src', 'MatchWinloss.aspx?' + params);
//    //parent.jQuery.unblockUI();
//}

var racingDailyForTote = {
    date: '',
    title: '',
    GetDataReport: function () {
        try { parent.BlockUI(); } catch (e) { }
        $("#err").empty();
        var racinggrid = $("#racingDailyForTote");

        var arr_frozen = [],
            arr_scroll = [],
            arr_footerfrozen = [],
            arr_footerscroll = [];
        racingDailyForTote.date = $("#date").val();
        racingDailyForTote.title = title + ' - Date ' + racingDailyForTote.date;
        $.ajax({
            type: "Post",
            url: "../../Report/Racing/Services/ServiceReportRacing.svc/SelWinlostHR_Details",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"wldate":"' + racingDailyForTote.date + '"}',
            success: function (rs) {
                if (rs.Items != null && rs.Items.length > 0) {
                    obj = rs;

                    $("#gridtitle").html(racingDailyForTote.title);
                    var tblfrozen = $("#tblfrozen"),
                        tblscroll = $("#tblscroll"),
                        items1 = rs.Items,
                        total1 = items1.length;

                    for (var i = 0; i < total1; i++) {
                        if (items1[i].Items != null) {
                            var total2 = items1[i].Items.length;
                            for (var j = 0; j < total2; j++) {
                                //Render row
                                var rows = items1[i].Items[j].Items;
                                total3 = rows.length;
                                for (k = 0; k < total3; k++) {
                                    arr_frozen.push(racingDailyForTote.renderFrozenRow(k, total3, rows[k]));
                                    arr_scroll.push(racingDailyForTote.renderScrollRow(rows[k]));
                                }
                                //rendder total
                                arr_frozen.push(racingDailyForTote.renderTotal1Frozen(items1[i].Items[j].Totals[0], "total1"));
                                arr_scroll.push(racingDailyForTote.renderTotalScroll(items1[i].Items[j].Totals, "total1"));
                            }
                            //rendder total
                            arr_frozen.push(racingDailyForTote.renderTotalFrozen(items1[i].Totals, "total"));
                            arr_frozen.push(racingDailyForTote.renderEmptyRow(2));
                            arr_scroll.push(racingDailyForTote.renderTotalScroll(items1[i].Totals, "total"));
                            arr_scroll.push(racingDailyForTote.renderEmptyRow(23));

                            if (items1[i].GroupTotals.length > 0) {
                                arr_frozen.push(racingDailyForTote.renderGroupTotalFrozen(items1[i].GroupTotals, "grouptotal"));
                                arr_frozen.push(racingDailyForTote.renderEmptyRow(2));
                                arr_scroll.push(racingDailyForTote.renderTotalScroll(items1[i].GroupTotals, "grouptotal"));
                                arr_scroll.push(racingDailyForTote.renderEmptyRow(23));
                            }
                        }
                    }

                    tblfrozen.html(arr_frozen.join(''));
                    tblscroll.html(arr_scroll.join(''));

                    //render grandtotal
                    arr_footerfrozen.push(racingDailyForTote.renderGrandTotalFrozen(rs.Totals, ""));
                    arr_footerscroll.push(racingDailyForTote.renderTotalScroll(rs.Totals, ""));
                    $("#tblfooterfrozen").html(arr_footerfrozen.join(''));
                    $("#tblfooterscroll").html(arr_footerscroll.join(''));

                    racinggrid.removeClass("hidden").reRenderScroller(721, 563, false); //7*100+7*1+7*2
                }
                else {
                    $("#racingDailyForTote").addClass("hidden");
                    showAlertMessage("err", "There is no data.", 1);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderEmptyRow: function (col) {
        var html = "";
        html = '<tr>\
                <td colspan="' + col + '" style="border-right:none 0px;">\
                    <div>&nbsp;</div>\
                </td>\
            </tr>';
        return html;
    },
    renderScrollRow: function (row) {
        var html = "";
        html = '<tr>\
                <!--Agency-->\
                <td>\
                    <div class="col100 r">' + row.AgWinTO + ' </div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.AgPlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.AgWPTO + '</div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.AgTotal + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.AgCommIncome + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.AgPTWL + '</div>\
                </td>\
                <!--IBC-->\
                <td>\
                    <div class="col100 r">' + row.IBCWinTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.IBCPlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.IBCWPTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.IBCTotal + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.IBCCommIncome + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.IBCAmountFee + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.IBCCommExpense + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.IBCPT + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.IBCNetIncome + '</div>\
                </td>\
                <!--Partner-->\
                <td>\
                    <div class="col100 r">' + row.PhuWinTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PhuPlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PhuWPTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PhuTotal + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PhuDividendWon + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PhuCommExpense + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PhuAmountFee + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PhuAmountDue + '</div>\
                </td>\
            </tr>';
        return html;
    },
    renderFrozenRow: function (index, totalrow, row) {
        var html = "";
        if (index == 0) {
            html = '<tr>\
                        <td rowspan="' + totalrow + '">\
                            <div class="col160 c">' + row.Meeting + ' - ' + row.Currency  + '</div>\
                        </td>\
                        <td >\
                            <div class="col60 c blue" onclick="ViewBetList(\'' + row.MeetingId + '\',\'' + row.Race + '\',\'' + row.CurrencyId + '\',\'' + row.Currency + '\',\'' + racingDailyForTote.date + '\',\'rep\',\'\');">' + row.Race + '</div>\
                        </td>\
                    </tr>';
        }
        else {
            html = '<tr>\
                        <td >\
                            <div class="col60 c blue" onclick="ViewBetList(\'' + row.MeetingId + '\',\'' + row.Race + '\',\'' + row.CurrencyId + '\',\'' + row.Currency + '\',\'' + racingDailyForTote.date + '\',\'rep\',\'\');">' + row.Race + '</div>\
                        </td>\
                    </tr>';
        }
        return html;
    },
    renderTotalScroll: function (rows, cssclass) {
        var html = "";
        for (i = 0; i < rows.length; i++) {
            var row = rows[i];
            html += '<tr class="' + cssclass + '">\
                    <!--Agency-->\
                    <td>\
                        <div class="col100 r b">' + row.AgWinTO + ' </div>\
                    </td>\
                    <td>\
                        <div class="col100  r b">' + row.AgPlaceTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100  r b">' + row.AgWPTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100  r b">' + row.AgTotal + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.AgCommIncome + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.AgPTWL + '</div>\
                    </td>\
                    <!--IBC-->\
                    <td>\
                        <div class="col100 r b">' + row.IBCWinTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.IBCPlaceTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.IBCWPTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.IBCTotal + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.IBCCommIncome + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.IBCCommExpense + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.IBCAmountFee + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.IBCPT + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.IBCNetIncome + '</div>\
                    </td>\
                    <!--Partner-->\
                    <td>\
                        <div class="col100 r b">' + row.PhuWinTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PhuPlaceTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PhuWPTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PhuTotal + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PhuDividendWon + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PhuCommExpense + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PhuAmountFee + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PhuAmountDue + '</div>\
                    </td>\
                </tr>';
        }
        return html;
    },
    renderTotalFrozen: function (rows, cssclass) {
        var html = "";
        for (i = 0; i < rows.length; i++) {
            if (i == 0) {
                html += '<tr class="' + cssclass + '">\
                <td rowspan="' + rows.length + '">\
                    <div class="col160 b c">Total</div>\
                </td>\
                <td>\
                    <div class="col60 c b">' + rows[i].Currency + '</div>\
                </td>\
            </tr>';
            }
            else {
                html += '<tr class="' + cssclass + '">\
                <td>\
                    <div class="col60 c b">' + rows[i].Currency + '</div>\
                </td>\
            </tr>';
            }
        }

        return html;
    },
    renderGroupTotalFrozen: function (rows, cssclass) {
        var html = "";
        for (i = 0; i < rows.length; i++) {
            if (i == 0) {
                html += '<tr class="' + cssclass + '">\
                <td rowspan="' + rows.length + '">\
                    <div class="col160 b c">' + rows[i].Currency + ' Group Total</div>\
                </td>\
                <td>\
                    <div class="col60 c b">' + rows[i].Currency + '</div>\
                </td>\
            </tr>';
            }
            else {
                html += '<tr class="' + cssclass + '">\
                <td>\
                    <div class="col60 c b">' + rows[i].Currency + '</div>\
                </td>\
            </tr>';
            }
        }

        return html;
    },
    renderGrandTotalFrozen: function (rows, cssclass) {
        var html = "";
        for (i = 0; i < rows.length; i++) {
            if (i == 0) {
                html += '<tr class="' + cssclass + '">\
                <td rowspan="' + rows.length + '">\
                    <div class="col160 b c">Grand Total</div>\
                </td>\
                <td>\
                    <div class="col60 c b">' + rows[i].Currency + '</div>\
                </td>\
            </tr>';
            }
            else {
                html += '<tr class="' + cssclass + '">\
                <td>\
                    <div class="col60 c b">' + rows[i].Currency + '</div>\
                </td>\
            </tr>';
            }
        }

        return html;
    },
    renderTotal1Frozen: function (row, cssclass) {
        var html = "";
        html = '<tr class="' + cssclass + '">\
                <td colspan="2">\
                    <div class="c b">' + row.Meeting + ' - ' + row.Currency + ' Total</div>\
                </td>\
            </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'RacingExport.aspx?type=export&reporttype=racingdailyreportfortote&fdate=' + racingDailyForTote.date + '&title=' + racingDailyForTote.title;
        $("#frm").attr("src", url);
    }
};
function ViewBetList(leagueid, raceNo, currencyID, currencyName, wldate, db, WL) 
    {
        var url = '../ToteHRBL.aspx?leagueid=' + leagueid;
        url += '&raceNo=' + raceNo;
        url += '&currencyID=' + currencyID;
        url += '&currencyName=' + currencyName;
        url += '&wldate=' + wldate;
        url += '&db=' + db;
        url += '&WL=' + WL;
                     
        var w = 1100;
        var h = 800;
        PopUpWindow(url, w, h);
    }
