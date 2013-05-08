var date = '';
var obj = {};
var detailtitle = '';
var _poolCurrency = '';
var _partnerId = '';
var _meetingName = '';
var _isHideIBCPTRacing = false; 

var betlists = {
    Items: [
        {
            No: "1",
            Member: "M31SD00001",
            RefNo: "2337396547",
            TransTime: "3/26/2011 1:20:12 PM",
            HorseBet: "3 - DEN'S GIFT",
            Bettype: "Win",
            Odds: "-",
            Stake: "1,800.00",
            ExRate: "0.45",
            ExRateName: "Exrate 2",
            MemberWL: "-1,800.00",
            MemberCom: "0.00",
            AgencyWL: "-1,800.00",
            AgencyCom: "0.00",
            IBCWL: "-1,800.00",
            IBCCom: "0.00",
            Status: "LOSE",
            Dividend: "0.00"
        },
        {
            No: "1",
            Member: "M31SD00001",
            RefNo: "2337396547",
            TransTime: "3/26/2011 1:20:12 PM",
            HorseBet: "3 - DEN'S GIFT",
            Bettype: "Win",
            Odds: "-",
            Stake: "1,800.00",
            ExRate: "0.45",
            ExRateName: "Exrate 2",
            MemberWL: "-1,800.00",
            MemberCom: "0.00",
            AgencyWL: "-1,800.00",
            AgencyCom: "0.00",
            IBCWL: "-1,800.00",
            IBCCom: "0.00",
            Status: "LOSE",
            Dividend: "0.00"
        }
    ],
    Totals: null
};
$(function () {
    $("#racingwinloss").scroller();
    $("#racingwinlossdetail").scroller();
    $("#gridbetlist").scroller();

    $("#dialogComparision").dialog({
        title: "Tote Comparision",
        autoOpen: false,
        height: 200,
        width: 600,
        modal: true
    });
    $("#dialogBetList").dialog({
        title: detailtitle,
        autoOpen: false,
        height: 635,
        width: 972,
        modal: true
    });

    $('.btnHelp').CreateBubblePopup();

    //set customized mouseover event for each button
    /*if (reporttype == 1) {
    $('#btnAmountHelp, #btnAmountDueHelpDetail').mouseover(function () {
    //show the bubble popup with new options
    showPopup($(this), "= [Total TO Partner] + [Tote Fee] - [Dividend Won] - [Comm Income IBC]");
           
    });
    $('#btnNetIncomeHelp, #btnNetIncomeHelpDetail').mouseover(function () {
    //show the bubble popup with new options
    showPopup($(this), "= [PT Win/Loss IBC] + [Comm Income IBC] -  [Comm Income Agency]");
           
    });
    }
    else {
    $('#btnAmountHelp, #btnAmountDueHelpDetail').mouseover(function () {
    //show the bubble popup with new options
    showPopup($(this), "= [Total TO Partner]  - [Dividend Won] - [Comm Income IBC]");
    });
    $('#btnNetIncomeHelp, #btnNetIncomeHelpDetail').mouseover(function () {
    //show the bubble popup with new options
    showPopup($(this), "= [PT Win/Loss IBC] + [Comm Income IBC] -  [Comm Income Agency] - [Total Fee]");
    });
    }*/
    if (reporttype == 1) {
        $('#btnNetIncomeHelp, #btnNetIncomeHelpDetail').mouseover(function () {
            //show the bubble popup with new options
            showPopup($(this), "= [PT Win/Loss IBC] + [Comm Income IBC] -  [Comm Income Agency]");

        });
    }
    else {
        $('#btnNetIncomeHelp, #btnNetIncomeHelpDetail').mouseover(function () {
            //show the bubble popup with new options
            showPopup($(this), "= [PT Win/Loss IBC] + [Comm Income IBC] -  [Comm Income Agency] - [Tote Fee]");
        });
    }

    if (reporttype == 3) {
        $('#btnAmountHelp, #btnAmountDueHelpDetail').mouseover(function () {
            //show the bubble popup with new options
            showPopup($(this), "= [Total TO Partner]  - [Dividend Won] - [Comm Income IBC]");
        });
    }
    else if (reporttype == 1) {
        $('#btnAmountHelp, #btnAmountDueHelpDetail').mouseover(function () {
            //show the bubble popup with new options
            showPopup($(this), "= [Total TO Partner] - [Tote Fee] - [Dividend Won] - [Comm Income IBC]");

        });
    }
    else {

        $('#btnAmountDueHelp, #btnAmountDueHelpDetail').mouseover(function () {
            //show the bubble popup with new options
            showPopup($(this), "= [Total TO Partner] + [Tote Fee] - [Dividend Won] - [Comm Income IBC]");

        });
    }
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

function ExportToExcel(reporttype) {
    //try{parent.BlockUI();}catch(e){}
    alert('export');
    var params = 'type=export';
    params += '&date=' + $('#date').val();
    params += "reporttype=" + reporttype;
    $("#frm").attr('src', 'HorseRacingDailyReportforUK.aspx?' + params);
    //parent.jQuery.unblockUI();
}
function ExportToExcel(type, bettype, isLive, matchId) {
    //try{parent.BlockUI();}catch(e){}
    var params = 'type=' + type;
    params += '&date=' + $('#date').val();
    params += '&isLive=' + isLive;
    params += '&matchid=' + matchId;
    params += '&bettype=' + bettype;

    $("#frm").attr('src', 'MatchWinloss.aspx?' + params);
    //parent.jQuery.unblockUI();
}
var racingwinloss = {

    GetDataReport: function () {
        parent.BlockUI();
        $("#err").empty();
        var racinggriddetail = $("#racingwinlossdetail");
        racinggriddetail.addClass("hidden");
        $("#btnbacktomain").addClass("hidden");
        var arr_frozen = [];
        var arr_scroll = [];
        var arr_footerfrozen = [];
        var arr_footerscroll = [];

        _isHideIBCPTRacing = $('#IsHideIBCPTRacing').val() == 'True';

        date = $("#date").val();
        $.ajax({
            type: "Post",
            url: "../../Report/Racing/Services/ServiceReportRacing.svc/GetWinlossRacingReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"date":"' + date + '","reporttype":' + reporttype + '}',
            success: function (rs) {
                if (rs.Items != null && rs.Items.length > 0) {
                    obj = rs;
                    $("#gridtitle").html(title + " - Date " + date);
                    var tblfrozen = $("#tblfrozen");
                    var tblscroll = $("#tblscroll");

                    var items = rs.Items;
                    var totalMeetings = items.length;

                    for (var i = 0; i < totalMeetings; i++) {
                        if (items[i].Items != null) {
                            var totalPartners = items[i].Items.length;
                            for (var j = 0; j < totalPartners; j++) {
                                //Render row
                                var row = items[i].Items[j];
                                arr_frozen.push(racingwinloss.renderFrozenRow(j, totalPartners, row));
                                arr_scroll.push(racingwinloss.renderScrollRow(row));
                            }
                            //rendder total
                            arr_frozen.push(racingwinloss.renderTotalFrozen(items[i].Totals, "total"));
                            arr_frozen.push(racingwinloss.renderEmptyRow(2));
                            arr_scroll.push(racingwinloss.renderTotalScroll(items[i].Totals, "total"));
                            arr_scroll.push(racingwinloss.renderEmptyRow(23));
                        }
                    }

                    tblfrozen.html(arr_frozen.join(''));
                    tblscroll.html(arr_scroll.join(''));
                    //render Comparision
                    if (rs.IsGotDumpFile) {
                        arr_footerfrozen.push(racingwinloss.renderEmptyRows(2, 2, "whitebg"));
                        arr_footerscroll.push(racingwinloss.renderComparisionRows("whitebg"));
                    }
                    //render grandtotal
                    arr_footerfrozen.push(racingwinloss.renderGrandTotalFrozen(rs.Totals, ""));
                    arr_footerscroll.push(racingwinloss.renderTotalScroll(rs.Totals, ""));

                    $("#tblfooterfrozen").html(arr_footerfrozen.join(''));
                    $("#tblfooterscroll").html(arr_footerscroll.join(''));

                    var racinggrid = $("#racingwinloss");
                    racinggrid.removeClass("hidden");
                    racinggrid.reRenderScroller(721, 563, false); //width:7*100+7*1+7*2|height:605px

                    if (_isHideIBCPTRacing) {
                        $('.hideIBC').addClass('hide');
                        $('.colspanIBC').attr('colspan', 7);
                        $('.hideIBCExcel').addClass('hide');
                        $('.colspanIBCCompare').attr('colspan', 16);
                    }
                }
                else {
                    $("#racingwinloss").addClass("hidden");
                    showAlertMessage("err", "There is no data.", 1);
                    $("#err").removeClass("hidden");
                }
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }


        });
    },
    renderFrozenRow: function (index, totalrow, row) {
        var html = "";
        if (index == 0) {
            html = '<tr>\
                    <td rowspan="' + totalrow + '">\
                        <div class="col160">' + row.Name + '</div>\
                    </td>\
                    <td >\
                        <div class="col60 c">' + row.Currency + '</div>\
                    </td>\
                </tr>';
        }
        else {
            html = '<tr>\
                    <td >\
                        <div class="col60 c">' + row.Currency + '</div>\
                    </td>\
                </tr>';
        }
        return html;
    },
    renderScrollRow: function (row) {
        var html = "";
        html = '<tr>\
                <!--Agency-->\
                <td>\
                    <div class="col100 r">' + row.AgencyWinTO + ' </div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.AgencyPlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.AgencyWPTO + '</div>\
                </td>\
                <td>\
                    <div class="col100  r">' + row.AgencyTotalTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.AgencyCommIncome + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.AgencyPTWL + '</div>\
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
                    <div class="col100 r">' + row.IBCTotalTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.IBCCommIncome + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.IBCToteFee + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.IBCCommExpense + '</div>\
                </td>\
                <td class="hideIBC">\
                    <div class="col100 r">' + row.IBCPTWL + '</div>\
                </td>\
                <td class="hideIBC">\
                    <div class="col100 r">' + row.IBCNetIncome + '</div>\
                </td>\
                <!--Partner-->\
                <td>\
                    <div class="col100 r">' + row.PartnerWinTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PartnerPlaceTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PartnerWPTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PartnerTotalTO + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PartnerDividendWon + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PartnerCommExpense + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PartnerToteFee + '</div>\
                </td>\
                <td>\
                    <div class="col100 r">' + row.PartnerAmountDue + '</div>\
                </td>\
            </tr>';
        return html;
    },
    renderTotalFrozen: function (rows, cssclass) {
        var html = "";
        for (i = 0; i < rows.length; i++) {
            if (i == 0) {
                html += '<tr class="' + cssclass + '">\
                <td rowspan="' + rows.length + '">\
                    <div class="col160 blue b" onclick="racingwinlossdetail.GetDataReportDetail(\'' + rows[i].CurrencyId + '\',\'' + rows[i].Id + '\',\'' + rows[i].Name.replace(' Total', '') + '\');">' + rows[i].Name + '</div>\
                </td>\
                <td>\
                    <div class="col60 c b">' + (rows[i].Currency == "" ? "&nbsp;" : rows[i].Currency) + '</div>\
                </td>\
            </tr>';
            }
            else {
                html += '<tr class="' + cssclass + '">\
                <td>\
                    <div class="col60 c b">' + (rows[i].Currency == "" ? "&nbsp;" : rows[i].Currency) + '</div>\
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
                        <div class="col160 b">' + rows[i].Name + '</div>\
                    </td>\
                    <td>\
                        <div class="col60 c b">' + (rows[i].Currency == "" ? "&nbsp;" : rows[i].Currency) + '</div>\
                    </td>\
                </tr>';
            }
            else {
                html += '<tr class="' + cssclass + '">\
                    <td>\
                        <div class="col60 c b">' + (rows[i].Currency == "" ? "&nbsp;" : rows[i].Currency) + '</div>\
                    </td>\
                </tr>';
            }
        }
        return html;
    },
    renderTotalScroll_2: function (rows, cssclass) {
        var html = "";
        var color = "#DDEBFF";
        for (i = 0; i < rows.length; i++) {
            var row = rows[i];
            var style = '';
            if (i < rows.length - 2) style = ' style="background-color:' + color + '"';
            html += '<tr class="' + cssclass + '">\
                    <!--Agency-->\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.AgencyWinTO + ' </div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100  r b">' + row.AgencyPlaceTO + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100  r b">' + row.AgencyWPTO + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100  r b">' + row.AgencyTotalTO + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.AgencyCommIncome + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.AgencyPTWL + '</div>\
                    </td>\
                    <!--IBC-->\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.IBCWinTO + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.IBCPlaceTO + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.IBCWPTO + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.IBCTotalTO + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.IBCCommIncome + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.IBCToteFee + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.IBCCommExpense + '</div>\
                    </td>\
                    <td class="hideIBC"' + style + '>\
                        <div class="col100 r b">' + row.IBCPTWL + '</div>\
                    </td>\
                    <td class="hideIBC"' + style + '>\
                        <div class="col100 r b">' + row.IBCNetIncome + '</div>\
                    </td>\
                    <!--Partner-->\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.PartnerWinTO + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.PartnerPlaceTO + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.PartnerWPTO + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.PartnerTotalTO + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.PartnerDividendWon + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.PartnerCommExpense + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.PartnerToteFee + '</div>\
                    </td>\
                    <td' + style + '>\
                        <div class="col100 r b">' + row.PartnerAmountDue + '</div>\
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
                        <div class="col100 r b">' + row.AgencyWinTO + ' </div>\
                    </td>\
                    <td>\
                        <div class="col100  r b">' + row.AgencyPlaceTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100  r b">' + row.AgencyWPTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100  r b">' + row.AgencyTotalTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.AgencyCommIncome + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.AgencyPTWL + '</div>\
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
                        <div class="col100 r b">' + row.IBCTotalTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.IBCCommIncome + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.IBCToteFee + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.IBCCommExpense + '</div>\
                    </td>\
                    <td class="hideIBC">\
                        <div class="col100 r b">' + row.IBCPTWL + '</div>\
                    </td>\
                    <td class="hideIBC">\
                        <div class="col100 r b">' + row.IBCNetIncome + '</div>\
                    </td>\
                    <!--Partner-->\
                    <td>\
                        <div class="col100 r b">' + row.PartnerWinTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PartnerPlaceTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PartnerWPTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PartnerTotalTO + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PartnerDividendWon + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PartnerCommExpense + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PartnerToteFee + '</div>\
                    </td>\
                    <td>\
                        <div class="col100 r b">' + row.PartnerAmountDue + '</div>\
                    </td>\
                </tr>';
        }
        return html;
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
    renderEmptyRows: function (row, col, cssclass) {
        var html = "";
        for (var i = 0; i < row; i++) {
            html += '<tr>\
                    <td colspan="' + col + '" style="border-right:none 0px;" class="' + cssclass + '">\
                        <div>&nbsp;</div>\
                    </td>\
                </tr>';
        }
        return html;
    },
    renderComparisionRows: function (cssclass) {
        var isToteMatched = true;
        var isDividendMatched = true;

        for (i = 0; i < obj.Comparision.length; i++) {
            if (obj.Comparision[i].IsMatchTO == false) {
                isToteMatched = false;
            }
            if (obj.Comparision[i].IsMatchDividend == false) {
                isDividendMatched = false;
            }
        }
        var html = "";
        html += '<tr >\
                    <!--Agency-->\
                    <td colspan="18" class="colspanIBCCompare ' + cssclass + '">\
                        <div>&nbsp;</div>\
                    </td>\
                    <td colspan="2" class="comparision">\
                        <div class="c" >Tote Comparision</div>\
                    </td>\
                    <td colspan="3" class="' + cssclass + '">\
                        <div>&nbsp;</div>\
                    </td>\
                </tr>';
        html += '<tr>\
                    <!--Agency-->\
                    <td colspan="18" class="colspanIBCCompare ' + cssclass + '">\
                        <div>&nbsp;</div>\
                    </td>\
                    <td class="' + cssclass + '">\
                        <div class="col100 c">' + (isToteMatched ? '<span class="match">OK<\span>' : '<span class="notmatch" onclick="racingwinloss.initPopupComparision();">Not Matched<\span>') + '</div>\
                    </td>\
                    <td class="' + cssclass + '">\
                        <div class="col100 c">' + (isDividendMatched ? '<span class="match">OK<\span>' : '<span class="notmatch" onclick="racingwinloss.initPopupComparision();">Not Matched<\span>') + '</div>\
                    </td>\
                    <td colspan="3" class="' + cssclass + '">\
                        <div>&nbsp;</div>\
                    </td>\
                </tr>';
        return html;
    },
    initPopupComparision: function () {
        racingwinloss.renderComparisionData();

    },
    renderComparisionData: function () {
        var arr = [];
        var html = '';
        if (obj.Comparision != null) {
            for (var i = 0; i < obj.Comparision.length; i++) {
                var item = obj.Comparision[i];
                html = '<tr class="comparision">\
                        <td>' + item.currency + '</td>\
                        <td ' + (item.IsMatchTO ? '' : 'class="red"') + '>' + item.partnerTO + '</td>\
                        <td ' + (item.IsMatchDividend ? '' : 'class="red"') + '>' + item.partnerDividendWon + '</td>\
                        <td ' + (item.IsMatchTO ? '' : 'class="red"') + '>' + item.ibcTO + '</td>\
                        <td ' + (item.IsMatchDividend ? '' : 'class="red"') + '>' + item.ibcDividendWon + '</td>\
                    </tr>';
                arr.push(html);
            }
            $("#tblComparision .comparision").remove();
            $("#tblComparision").append($(arr.join('')));
            $("#dialogComparision").removeClass("hidden");
            $("#dialogComparision").dialog("open");
        }
    },
    ExportToExcel: function (reporttype) {
        var params = 'type=export';
        params += '&date=' + $('#date').val();
        params += "&reporttype=" + reporttype;
        $("#frm").attr('src', 'HorseRacingDailyReportforUK.aspx?' + params);
    }
};
var racingwinlossdetail = {
    GetDataReportDetail: function (poolCurr, partnerId, meetingName) {
        parent.BlockUI();
        $("#err").empty();
        var racinggrid = $("#racingwinloss");
        racinggrid.addClass("hidden");

        var arr_frozen = [];
        var arr_scroll = [];
        var arr_footerfrozen = [];
        var arr_footerscroll = [];
        if (date == '') date = $("#date").val();
        $.ajax({
            type: "Post",
            url: "../../Report/Racing/Services/ServiceReportRacing.svc/GetWinlossRacingReportDetails",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"winlossDate":"' + date + '","poolCurrency":"' + poolCurr + '","partnerId":"' + partnerId + '"}',
            success: function (rs) {
                if (rs.Items != null && rs.Items.length > 0) {
                    obj = rs;
                    detailtitle = title + ' - ' + meetingName + ' - Date ' + date;
                    $("#gridtitledetail").html(detailtitle);
                    var tblfrozen = $("#tblfrozendetail");
                    var tblscroll = $("#tblscrolldetail");

                    var items1 = rs.Items;
                    var total1 = items1.length;

                    for (var i = 0; i < total1; i++) {
                        if (items1[i].Items != null) {
                            var total2 = items1[i].Items.length;
                            for (var j = 0; j < total2; j++) {
                                //Render row
                                var rows = items1[i].Items[j].Items;
                                total3 = rows.length;
                                for (k = 0; k < total3; k++) {
                                    arr_frozen.push(racingwinlossdetail.renderFrozenRow(k, total3, rows[k]));
                                    arr_scroll.push(racingwinloss.renderScrollRow(rows[k]));
                                }
                                //rendder total
                                arr_frozen.push(racingwinlossdetail.renderTotal1Frozen(items1[i].Items[j].Totals[0], "total1"));
                                //arr_frozen.push(renderEmptyRow(2));
                                arr_scroll.push(racingwinloss.renderTotalScroll(items1[i].Items[j].Totals, "total1"));
                                //arr_scroll.push(renderEmptyRow(23));
                            }
                            //rendder total
                            arr_frozen.push(racingwinlossdetail.renderTotalFrozen(items1[i].Totals, "total"));
                            arr_frozen.push(racingwinloss.renderEmptyRow(2));
                            arr_scroll.push(racingwinloss.renderTotalScroll(items1[i].Totals, "total"));
                            arr_scroll.push(racingwinloss.renderEmptyRow(23));
                        }
                    }

                    tblfrozen.html(arr_frozen.join(''));
                    tblscroll.html(arr_scroll.join(''));

                    //render grandtotal
                    arr_footerfrozen.push(racingwinlossdetail.renderGrandTotalFrozen(rs.Totals, "grouptotal"));
                    arr_footerfrozen.push(racingwinloss.renderGrandTotalFrozen(rs.GrandTotals, "grandtotal"));
                    arr_footerscroll.push(racingwinloss.renderTotalScroll(rs.Totals, "grouptotal"));
                    arr_footerscroll.push(racingwinloss.renderTotalScroll(rs.GrandTotals, "grandtotal"));
                    $("#tblfooterfrozendetail").html(arr_footerfrozen.join(''));
                    $("#tblfooterscrolldetail").html(arr_footerscroll.join(''));

                    var detailgrid = $("#racingwinlossdetail");
                    detailgrid.removeClass("hidden");
                    detailgrid.reRenderScroller(721, 563, false); //7*100+7*1+7*2

                    if (_isHideIBCPTRacing) {
                        $('.hideIBCBetlist').removeClass('blue').attr('onclick', '');
                        $('.hideIBC').addClass('hide');
                    }
                }
                else {
                    $("#racingwinlossdetail").addClass("hidden");
                    $("#btnbacktomain").removeClass("hidden");
                    showAlertMessage("err", "There is no data.", 1);
                    $("#err").removeClass("hidden");
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
        _meetingName = meetingName;
        _partnerId = partnerId;
        _poolCurrency = poolCurr;
    },
    renderFrozenRow: function (index, totalrow, row) {
        var html = "";
        if (index == 0) {
            html = '<tr>\
                        <td rowspan="' + totalrow + '">\
                            <div class="col160">' + row.Name + '</div>\
                        </td>\
                        <td >\
                            <div class="col60 c blue hideIBCBetlist" onclick="betlist.initPopupBetlist(\'' + row.Id + '\',\'' + row.RaceNo + '\',\'' + row.CurrencyId + '\',\'' + row.Currency + '\');">' + row.RaceNo + '</div>\
                        </td>\
                    </tr>';
        }
        else {
            html = '<tr>\
                        <td >\
                            <div class="col60 c blue hideIBCBetlist" onclick="betlist.initPopupBetlist(\'' + row.Id + '\',\'' + row.RaceNo + '\',\'' + row.CurrencyId + '\',\'' + row.Currency + '\');">' + row.RaceNo + '</div>\
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
                    <div class="col160 b">' + rows[i].Name + '</div>\
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
                    <div class="b">' + row.Name + '</div>\
                </td>\
            </tr>';
        return html;
    },
    backToMain: function () {
        $("#err").addClass("hidden");
        $("#racingwinlossdetail").addClass("hidden");
        $("#racingwinloss").removeClass("hidden");
    },
    ExportToExcel: function (reporttype) {
        var params = 'type=export&getdetail=1&reporttype=' + reporttype;
        params += '&date=' + $('#date').val();
        params += "&meeting=" + _meetingName;
        params += "&partnerid=" + _partnerId;
        params += "&poolCurr=" + _poolCurrency;
        $("#frm").attr('src', 'HorseRacingDailyReportforUK.aspx?' + params);
    },
    renderGrandTotalFrozen: function (rows, cssclass) {
        var html = "";
        for (var i = 0; i < rows.length; i++) {
            html += '<tr class="' + cssclass + '">\
                    <td >\
                        <div class="col160 b">' + rows[i].Name + '</div>\
                    </td>\
                    <td>\
                        <div class="col60 c b">' + (rows[i].Currency == "" ? "&nbsp;" : rows[i].Currency) + '</div>\
                    </td>\
                </tr>';
        }
        return html;
    }
};

var betlist = {
    initPopupBetlist: function (leagueid, raceNo, currency, currencyName) {

        parent.BlockUI();

        var arr_scroll = [];
        var arr_footerscroll = [];
        var data = {
            'winlossdate': date,
            'leagueid': leagueid,
            'raceNo': raceNo,
            'currency': currency
        };
        $.ajax({
            type: "Post",
            url: "../../Report/Racing/Services/ServiceReportRacing.svc/GetWinlossRacingBetlist",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: $.toJSON(data),
            success: function (rs) {
                if (rs.Items) {
                    $("#dialogBetList").dialog('option', 'title', detailtitle);
                    var arrTitle = detailtitle.split('-');
                    var strTtitle = arrTitle[0] + ' - ' + arrTitle[1] + ' - ' + arrTitle[2] + ' - ' + raceNo + ' - ' + arrTitle[3];

                    $("#gridtitlebetlist").html(strTtitle);
                    var total = rs.Items.length;
                    for (var i = 0; i < total; i++) {
                        arr_scroll.push(betlist.renderRow(rs.Items[i]));
                    }

                    $("#tblbodyscrollbetlist").html(arr_scroll.join(''));
                    $("#tblfooterscrollbetlist").html(betlist.renderTotal(rs.Totals));
                    $("#dialogBetList").removeClass("hidden");
                    $("#dialogBetList").dialog("open");

                    $("#gridbetlist").reRenderScroller(933, 485, true); //7*100+7*1+7*2
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },
    renderRow: function (row) {
        var html = '';
        var status = row.Status;
        var odd = row.Odds;
        if (row.BettypeId == "33") {
            status = "Win:<b>" + row.WinStatus + "</b><br>Place:<b>" + row.PlaceStatus + "</b>";
            odd = "Win:<b>" + row.winOdds + "</b><br>Place:<b>" + row.placeOdds + "</b>";
        }


        html = '<tr>\
                    <td><div class="col40 c">' + row.No + '</div></td>\
                    <td><div class="col100 c" >' + row.Member + '</div></td>\
                    <td><div class="col120 c">Ref No: ' + row.RefNo + '<br>' + row.TransTime + '</div></td>\
                    <td><div class="col120 r">\
                        <span class="bl-choice">' + row.HorseBet + '</span><br>\
                            <span class="bl-bettype">' + row.Bettype + '</span>\
                        </div></td>\
                    <td><div class="col60 r">' + odd + '</div></td>\
                    <td><div class="col80 r">' + row.Stake + (row.IsShowOtExRate ? '<br><span class="exrate2">' + row.ExRate + '</span><br><span class="exrate2">' + row.ExRateName + '</span>' : '') + '</div></td>\
                    <td><div class="col80 r">' + row.MemberWL + '<br><span class="exrate2">' + row.MemberCom + '</span></div></td>\
                    <td><div class="col80 r">' + row.AgencyWL + '<br><span class="exrate2">' + row.AgencyCom + '</span></div></td>\
                    <td><div class="col80 r">' + row.IBCWL + '<br><span class="exrate2">' + row.IBCCom + '</span></div></td>\
                    <td><div class="col60 c">' + status + '</div></td>\
                    <td><div class="col80 c">' + row.Dividend + '</div></td>\
               </tr>';
        return html;
    },
    renderTotal: function (row) {
        var html = '';
        //width:492px: total columns width + (totalcolum-1)*border + (totalcolum-1)*(paddingleft+paddingright)
        html = '<tr><td colspan="5" class="r"><div style="width:452px;">Total:</div></td>\
                    <td><div class="col80 r">' + row.Stake + '</div></td>\
                    <td><div class="col80 r">' + row.MemberWL + '<br><span class="exrate2">' + row.MemberCom + '</span></div></td>\
                    <td><div class="col80 r">' + row.AgencyWL + '<br><span class="exrate2">' + row.AgencyCom + '</span></div></td>\
                    <td><div class="col80 r">' + row.IBCWL + '<br><span class="exrate2">' + row.IBCCom + '</span></div></td>\
                    <td><div class="col60 r">&nbsp;</div></td>\
                    <td><div class="col80 r">' + row.Dividend + '</div></td>\
                </tr>'
        return html;
    }
};


