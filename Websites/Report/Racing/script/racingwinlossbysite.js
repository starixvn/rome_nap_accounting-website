var _isHideIBCPTRacing = false;

$(function () {
    racingwinloss.getSite();
    var racinggrid = $("#racinggridPT");
    racinggrid.scroller();
    $("#dialogBetList").dialog({
        title: '',
        autoOpen: false,
        height: 635,
        width: racingwinloss.IsApply50PercentRacingReport == 'True' ? 1152 : 1072,
        modal: true
    });
});

function GetRacingWinLossData() {

    var hasPt = false;
    $("#ddlsite option:selected").each(function () {
        hasPt = ($(this).attr('pt') == 'true');
    });

    $('#spSite').html($('#ddlsite').val());

    if (racingwinloss.IsApply50PercentRacingReport == 'True') {
        racingwinlossPT.getDataReport();
    }
    else {
        if (hasPt) {
            racingwinlossPT.getDataReport();
        }
        else {
            racingwinloss.getDataReport();
        }
    }
}

var racingwinloss = {
    date: '',
    site: '',
    reportTitle: '',
    IsApply50PercentRacingReport: true,
    getSite: function () {
        $.ajax({
            type: "Post",
            url: "Services/ServiceReportRacing.svc/WinlostRacingLicensee_SiteSel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                var arr = [];
                if (rs != null && rs.SiteItems != null) {
                    var len = rs.SiteItems.length;
                    for (var i = 0; i < len; i++) {
                        var item = rs.SiteItems[i];
                        arr.push('<option pt="' + item.HasPt + '" value="' + item.Name + '">' + item.Name + '</option>');
                    }
                }
                $("#ddlsite").empty().html(arr.join(''));
            }
        });
    },
    getDataReport: function () {
        try { parent.BlockUI(); } catch (e) { }
        $("#err").empty();
        var arr = [];
        racingwinloss.date = $("#date").val();
        racingwinloss.site = $("#ddlsite").val();


        $.ajax({
            type: "Post",
            url: "Services/ServiceReportRacing.svc/WinlostRacingLicensee_Details",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"winlossdate":"' + racingwinloss.date + '","site":"' + racingwinloss.site + '"}',
            success: function (rs) {
                if (rs.Items != null && rs.Items.length > 0) {
                    racingwinloss.reportTitle = title + ' (' + racingwinloss.site + ') - Date ' + racingwinloss.date;
                    $("#gridtitle").html(racingwinloss.reportTitle);
                    var tblscroll = $("#tblscroll");

                    var items = rs.Items;
                    var total = items.length;
                    //Loop Currency
                    for (var i = 0; i < total; i++) {
                        if (items[i].Items != null) {
                            var items1 = items[i].Items;
                            var total1 = items1.length;
                            //Loop Meeting
                            for (var j = 0; j < total1; j++) {
                                var items2 = items1[j].Items;
                                var total2 = items2.length;
                                //Loop Race
                                for (k = 0; k < total2; k++) {
                                    //Render Race
                                    arr.push(racingwinloss.renderRow(k, total2, items2[k]));
                                }
                                //Render Meeting Total
                                arr.push(racingwinloss.renderTotal(items1[j].Total, "meetingtotal"));
                                if (j < (total1 - 1)) {
                                    arr.push(racingwinloss.renderEmptyRow(9));
                                }
                            }
                        }
                        //Render Currency Total
                        var currencytotalitems = items[i].Totals;
                        var currencytotal = currencytotalitems.length;
                        for (var n = 0; n < currencytotal; n++) {
                            arr.push(racingwinloss.renderCurrencyTotal(n, currencytotal, currencytotalitems[n]));
                        }
                        arr.push(racingwinloss.renderEmptyRow(9));
                    }

                    //render grandtotal

                    $("#tblscroll").empty("").html(arr.join(''));
                    $("#tblfooterscroll").html(racingwinloss.renderGrandTotal(rs.Total, "grandtotal"));
                    $("#racinggridPT").addClass("hidden");
                    $("#racinggrid").removeClass("hidden").reRenderScroller(1087, 562, true); //width:1060+9*1+9*2|height:605px
                }
                else {
                    $("#racinggrid").addClass("hidden");
                    $("#racinggridPT").addClass("hidden");
                    showAlertMessage("err", "There is no data.", 1);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }


        });
    },
    renderRow: function (index, total, row) {
        var html = "";
        var firstcol = '';
        if (index == 0) {
            firstcol = '<td rowspan="' + total + '">\
                        <div class="b col260 c">' + row.Name + " - " + row.CurrencyName + ' </div>\
                     </td>';
        }
        html = '<tr>' + firstcol +
                '<td>\
                    <div class="col100  c"><span class="canchange" onclick="betlist.initPopupBetlist(' + row.Id + ',' + row.Race + ',' + row.CurrencyId + ',\'' + row.Name + " - " + row.CurrencyName + '\')">' + row.Race + '</span></div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.WinTO + '</div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.PlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.WinPlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.TotalTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.DividendWon + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.CommIncome + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.Amountdueto + '</div>\
                </td>\
            </tr>';
        return html;
    },
    renderTotal: function (row, cssclass) {
        var html = "";
        html = '<tr class="' + cssclass + '">\
                    <td colspan="2">\
                        <div class="c b">' + row.Name + " Total" + '</div>\
                    </td>\
                     <td>\
                        <div class="col100  r">' + row.WinTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100  r">' + row.PlaceTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + row.WinPlaceTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + row.TotalTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + row.DividendWon + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + row.CommIncome + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + row.Amountdueto + '</div>\
                    </td>\
            </tr>';
        return html;
    },
    renderGrandTotal: function (row, cssclass) {
        var html = "";

        html = '<tr class="' + cssclass + '">\
                    <td colspan="2">\
                        <div class="c b" style="width:363px;">' + row.Name + '</div>\
                    </td>\
                     <td>\
                        <div class="col100  r">' + row.WinTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100  r">' + row.PlaceTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + row.WinPlaceTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + row.TotalTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + row.DividendWon + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + row.CommIncome + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r">' + row.Amountdueto + '</div>\
                    </td>\
            </tr>';

        return html;
    },
    renderCurrencyTotal: function (index, total, row) {
        var html = "";
        var firstcol = '';
        if (index == 0) {
            firstcol = '<td rowspan="' + total + '">\
                        <div class="b col260 c">' + row.Name + ' </div>\
                     </td>';
        }
        html = '<tr class="currencytotal">' + firstcol +
                '<td>\
                    <div class="col100  c">' + row.CurrencyName + '</div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.WinTO + '</div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.PlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.WinPlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.TotalTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.DividendWon + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.CommIncome + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.Amountdueto + '</div>\
                </td>\
            </tr>';
        return html;
    },
    renderEmptyRow: function (col) {
        var html = "";
        html = '<tr>\
                <td colspan="' + col + '">\
                    <div>&nbsp;</div>\
                </td>\
            </tr>';
        return html;
    },
    ExportToExcel: function () {
        var url = "RacingExport.aspx?type=export&reporttype=reportbysite&fdate=" + this.date + "&site=" + this.site + "&reportporttitle=" + this.reportTitle;
        $("#frm").attr("src", url);
    }
};

// define object racingwinlossPT

var racingwinlossPT = {
    date: '',
    site: '',
    reportTitle: '',
    IsApply50PercentRacingReport: true,
    getSite: function () {
        $.ajax({
            type: "Post",
            url: "Services/ServiceReportRacing.svc/WinlostRacingLicensee_SiteSel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                var arr = [];
                if (rs != null && rs.SiteItems != null) {
                    var len = rs.SiteItems.length;
                    for (var i = 0; i < len; i++) {
                        var item = rs.SiteItems[i];
                        arr.push('<option value="' + item.Name + '">' + item.Name + '</option>');
                    }
                }
                $("#ddlsite").empty().html(arr.join(''));
            }
        });
    },
    getDataReport: function () {
        try { parent.BlockUI(); } catch (e) { }
        $("#err").empty();
        var arr = [];
        var arrfrozen = [];
        racingwinlossPT.date = $("#date").val();
        racingwinlossPT.site = $("#ddlsite").val();

        var colspan1 = racingwinloss.IsApply50PercentRacingReport == 'True' ? 14 : 12;
        var colspan2 = racingwinloss.IsApply50PercentRacingReport == 'True' ? 4 : 2;

        _isHideIBCPTRacing = $('#IsHideIBCPTRacing').val() == 'True';

        $.ajax({
            type: "Post",
            url: "Services/ServiceReportRacing.svc/RacingWinLossByLicenseeHavingPt",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"winlossdate":"' + racingwinlossPT.date + '","site":"' + racingwinlossPT.site + '"}',
            success: function (rs) {
                if (rs.Items != null && rs.Items.length > 0) {
                    racingwinlossPT.reportTitle = title + ' (' + racingwinlossPT.site + ') - Date ' + racingwinlossPT.date;
                    $("#gridtitlePT").html(racingwinlossPT.reportTitle);
                    var tblscroll = $("#tblbodyscrollPT");

                    var items = rs.Items;
                    var total = items.length;
                    //Loop Currency
                    for (var i = 0; i < total; i++) {
                        if (items[i].Items != null) {
                            var items1 = items[i].Items;
                            var total1 = items1.length;
                            //Loop Meeting
                            for (var j = 0; j < total1; j++) {
                                var items2 = items1[j].Items;
                                var total2 = items2.length;
                                //Loop Race
                                for (k = 0; k < total2; k++) {
                                    //Render Race
                                    arr.push(racingwinlossPT.renderRow(items2[k], ""));
                                    arrfrozen.push(racingwinlossPT.renderRowfrozen(k, total2, items2[k]));
                                }
                                //Render Meeting Total
                                arr.push(racingwinlossPT.renderRow(items1[j].Total, "meetingtotal"));
                                arrfrozen.push(racingwinlossPT.renderTotalfrozen(items1[j].Total, "meetingtotal"));

                                if (j < (total1 - 1)) {
                                    arr.push(racingwinlossPT.renderEmptyRow(colspan1));
                                    arrfrozen.push(racingwinlossPT.renderEmptyRow(colspan2));
                                }
                            }
                        }
                        //Render Currency Total
                        var currencytotalitems = items[i].Totals;
                        var currencytotal = currencytotalitems.length;

                        for (var n = 0; n < currencytotal; n++) {
                            arr.push(racingwinlossPT.renderRow(currencytotalitems[n], "currencytotal"));
                            arrfrozen.push(racingwinlossPT.renderCurrencyTotalfrozen(n, currencytotal, currencytotalitems[n]));
                        }

                        arr.push(racingwinlossPT.renderEmptyRow(colspan1));
                        arrfrozen.push(racingwinlossPT.renderEmptyRow(colspan2));
                    }

                    //render grandtotal

                    $("#tblbodyscrollPT").html(arr.join(''));
                    $("#tblbodyfrozenPT").html(arrfrozen.join(''));

                    $("#tblfooterPT").html(racingwinlossPT.renderRow(rs.Total, "grandtotal"));
                    $("#tblfooterfrozen").html(racingwinlossPT.renderGrandTotalfrozen(rs.Total));
                    $("#racinggrid").addClass("hidden");
                    $("#racinggridPT").removeClass("hidden").reRenderScroller(720, 562, false); //width:1060+9*1+9*2|height:605px

                    if (_isHideIBCPTRacing) {
                        $('.hideIBC').addClass('hide');
                        $('.colspanIBC').attr('colspan', 1);
                        $('.hideIBCBetlist').removeClass('canchange').attr('onclick', '');
                        $('.hideIBCExcel').addClass('hide');
                    }
                }
                else {
                    $("#racinggridPT").addClass("hidden");
                    showAlertMessage("err", "There is no data.", 1);
                    $("#racinggrid").addClass("hidden");
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }


        });
    },
    renderRowfrozen: function (index, total, row) {
        var fristcol = '';
        var html = "";
        if (index == 0) {
            firstcol = '<td rowspan="' + total + '">\
                        <div class="b col260 c">' + row.Name + " - " + row.CurrencyName + ' </div>\
                     </td>';
            html = '<tr>' + firstcol +
                '<td>\
                    <div class="col100 c"><span class="canchange hideIBCBetlist" onclick="betlist.initPopupBetlist(' + row.Id + ',' + row.Race + ',' + row.CurrencyId + ',\'' + row.Name + " - " + row.CurrencyName + '\')">' + row.Race + '</span></div>\
                 </td>\
                 </tr>';
        }
        else {
            html = '<tr>\
                        <td>\
                            <div class="col100 c"><span class="canchange hideIBCBetlist" onclick="betlist.initPopupBetlist(' + row.Id + ',' + row.Race + ',' + row.CurrencyId + ',\'' + row.Name + " - " + row.CurrencyName + '\')">' + row.Race + '</span></div>\
                        </td>\
                    </tr>';
        }

        return html;
    },

    renderRow: function (row, cssclass) {
        var html = "";
        var clsHide = racingwinloss.IsApply50PercentRacingReport == 'True' ? '' : 'hide';
        html = '<tr class="' + cssclass + '">\
                <td>\
                    <div class="col100  r">' + row.WinTO + '</div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.PlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.WinPlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.TotalTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.DividendWon + '</div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.AgentWinTO + '</div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.AgentPlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r" >' + row.AgentWinPlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r" >' + row.AgentTotalTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.AgentPtWinLoss + '</div>\
                </td>\
                <td>\
                    <div class="col100 r" >' + row.CommIncome + '</div>\
                </td>\
                <td class="hideIBC ' + clsHide + '">\
                    <div class="col100 r" >' + row.IBCPTWL + '</div>\
                </td>\
                <td class="' + clsHide + '">\
                    <div class="col100 r" >' + row.IBCCommIncomm + '</div>\
                </td>\
                <td class="hideIBC">\
                    <div class="col100 r" >' + row.Amountdueto + '</div>\
                </td>\
            </tr>';
        return html;
    },
    renderTotalfrozen: function (row, cssclass) {
        var html = "";
        html = '<tr class="' + cssclass + '">\
                    <td colspan="2">\
                        <div class = "c b">' + row.Name + " Total" + '</div>\
                    </td>\
                </tr>';
        return html;
    },
    renderGrandTotalfrozen: function (row) {
        var html = "";
        html = '<tr class="grandtotal">\
                    <td colspan="2">\
                        <div class = "c b" style="width:363px;">' + row.Name + '</div>\
                    </td>\
                </tr>';
        return html;

    },

    renderCurrencyTotalfrozen: function (index, total, row) {
        var firstcol = '';
        var html = "";
        if (index == 0) {
            firstcol = '<td rowspan="' + total + '">\
                        <div class="b col260 c">' + row.Name + ' </div>\
                     </td>';
            html = '<tr class="currencytotal">' + firstcol +
                      '<td>\
                        <div class="col100  c">' + row.CurrencyName + '</div>\
                      </td>\
                   </tr>';
        }
        else {
            html = '<tr class="currencytotal">\
                      <td>\
                        <div class="col100  c">' + row.CurrencyName + '</div>\
                      </td>\
                   </tr>';
        }
        return html;
    },
    renderEmptyRow: function (col) {
        var html = "";
        html = '<tr>\
                <td colspan="' + col + '">\
                    <div>&nbsp;</div>\
                </td>\
            </tr>';
        return html;
    },
    ExportToExcel: function () {
        var url = "RacingExport.aspx?type=export&reporttype=reportbysitept&fdate=" + this.date + "&site=" + this.site + "&reportporttitle=" + this.reportTitle;
        $("#frm").attr("src", url);
    }
};


var betlist = {
    initPopupBetlist: function (meetingid, raceNo, currency, meetingCurrency) {
        try { parent.BlockUI(); } catch (e) { }
        var site = $('#ddlsite').val();
        var date = $('#date').val();
        var title = '';

        site = $('#ddlsite').val();
        date = $('#date').val();
        if (site == '12Bet' || racingwinloss.IsApply50PercentRacingReport == 'True') {
            title = racingwinlossPT.reportTitle;
        }
        else {
            title = racingwinloss.reportTitle;
        }




        var arr = [];
        var data = {

            'meetingid': meetingid,
            'raceNo': raceNo,
            'currency': currency,
            'winlossdate': date,
            'site': site
        };
        $.ajax({
            type: "Post",
            url: "Services/ServiceReportRacing.svc/WinlostRacingLicensee_BetList_Details",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs.Items != null && rs.Items.length > 0) {

                    $("#dialogBetList").dialog('option', 'title', 'Tote Odds Racing (' + site + ') Detail Bet List');

                    var betlisttitlearr = title.split(" - ");
                    var betlisttitle = betlisttitlearr[0] + " - " + meetingCurrency + " - " + betlisttitlearr[1];
                    $("#gridtitlebetlist").html(betlisttitle);
                    var total = rs.Items.length;

                    for (var i = 0; i < total; i++) {
                        arr.push(betlist.renderRow(rs.Items[i]));
                    }

                    $("#tblbodyscrollbetlist").html(arr.join(''));
                    $("#tblfooterscrollbetlist").html(betlist.renderTotal(rs.Total));
                    $("#dialogBetList").removeClass("hidden");
                    $("#dialogBetList").dialog("open");

                    var width = racingwinloss.IsApply50PercentRacingReport == 'True' ? 1116 : 1033;

                    $("#gridbetlist").reRenderScroller(width, 485, true); //1033+12*1+12*2
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },
    renderRow: function (row) {
        var html = '';
        var clsHide = racingwinloss.IsApply50PercentRacingReport == 'True' ? '' : 'hide';
        if (row.IsCombineBettype) {
            html = '<tr>\
                    <td><div class="col20 c">' + row.Index + '</div></td>\
                    <td><div class="col100 c" >' + row.Member + '</div></td>\
                    <td><div class="col100 c" >' + row.UserName2 + '</div></td>\
                    <td><div class="col120 c">Ref No: <b>' + row.TransId + '</b><br>' + row.TranTime + '</div></td>\
                    <td><div class="col180 r">\
                        <span class="bl-choice">' + row.HorseBettype + '</span><br>\
                            <span class="bl-bettype">' + row.Bettype + '</span>\
                        </div></td>\
                    <td><div class="col80 r"><b>Win: </b>' + row.WinOdds + '<br><b>Place: </b>' + row.PlaceOdds + '</div></td>\
                    <td><div class="col80 r">' + row.Stake + (row.IsShowOtExRate ? '<br><span class="exrate2">' + row.ActualRate + '</span><br><span class="exrate2">' + row.ExRate2 + '</span>' : '') + '</div></td>\
                    <td><div class="col80 r">' + row.Winloss + '<br><span class="exrate2 b">' + row.Comm + '</span></div></td>\
                    <td><div class="col80 r">' + row.PTWLLicensee + '<br><span class="exrate2 b">' + row.AComm + '</span></div></td>\
                    <td class="' + clsHide + '"><div class="col80 r">' + row.IBCPTWL + '<br><span class="exrate2 b">' + row.IBCCommIncomm + '</span></div></td>\
                    <td><div class="col80 c"><b>Win: </b>' + row.WinStatus + '<br><b>Place: </b>' + row.PlaceStatus + '</div></td>\
                    <td><div class="col80 r">' + row.Dividend + '</div></td>\
               </tr>';
        }
        else {
            html = '<tr>\
                    <td><div class="col20 c">' + row.Index + '</div></td>\
                    <td><div class="col100 c" >' + row.Member + '</div></td>\
                    <td><div class="col100 c" >' + row.UserName2 + '</div></td>\
                    <td><div class="col120 c">Ref No: <b>' + row.TransId + '</b><br>' + row.TranTime + '</div></td>\
                    <td><div class="col180 r">\
                        <span class="bl-choice">' + row.HorseBettype + '</span><br>\
                            <span class="bl-bettype">' + row.Bettype + '</span>\
                        </div></td>\
                    <td><div class="col80 r">' + row.Odds + '</div></td>\
                    <td><div class="col80 r">' + row.Stake + (row.IsShowOtExRate ? '<br><span class="exrate2">' + row.ActualRate + '</span><br><span class="exrate2">' + row.ExRate2 + '</span>' : '') + '</div></td>\
                    <td><div class="col80 r">' + row.Winloss + '<br><span class="exrate2 b">' + row.Comm + '</span></div></td>\
                    <td><div class="col80 r">' + row.PTWLLicensee + '<br><span class="exrate2 b">' + row.AComm + '</span></div></td>\
                    <td class="' + clsHide + '"><div class="col80 r">' + row.IBCPTWL + '<br><span class="exrate2 b">' + row.IBCCommIncomm + '</span></div></td>\
                    <td><div class="col80 c">' + row.Status + '</div></td>\
                    <td><div class="col80 r">' + row.Dividend + '</div></td>\
               </tr>';
        }
        return html;
    },
    renderTotal: function (row) {
        var html = '';
        var clsHide = racingwinloss.IsApply50PercentRacingReport == 'True' ? '' : 'hide';
        //width:535px: total columns width + (totalcolum-1)*border + (totalcolum-1)*(paddingleft+paddingright)
        html = '<tr><td colspan="6" class="r"><div style="width:615px;">Total:</div></td>\
                    <td><div class="col80 r">' + row.Stake + '</div></td>\
                    <td><div class="col80 r">' + row.Winloss + '<br><span class="exrate2">' + row.Comm + '</span></div></td>\
                    <td><div class="col80 r">' + row.PTWLLicensee + '<br><span class="exrate2">' + row.AComm + '</span></div></td>\
                    <td class="' + clsHide + '"><div class="col80 r">' + row.IBCPTWL + '<br><span class="exrate2 b">' + row.IBCCommIncomm + '</span></div></td>\
                    <td><div class="col80 r">&nbsp;</div></td>\
                    <td><div class="col80 r">' + row.Dividend + '</div></td>\
                </tr>';
        return html;
    }
};

