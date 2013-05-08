
var pokerReport = {
    mainData: null,
    SelectedDate: '',
    funcMain: null,
    fromDate: '',
    toDate: '',
    sportID: '',
    title: '',
    detailtitle: '',
    selectedCurrency: 0,
    renderMainRow: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
               <td><div class="col100 c">' + row.Date + '</div></td>\
                <td><div class="col100 c link" onclick="pokerReport.renderDetailReport(\'' + row.CurrencyName + '\',' + row.Currency + ',\'' + row.Date + '\')">' + row.CurrencyName + '</div></td>\
                <td><div class="col80 r">' + row.GrossComm + '</div></td>\
                <td><div class="col80 r">' + row.MWinLoss + '</div></td>\
                <td><div class="col80 r">' + row.MComm + '</div></td>\
                <td><div class="col80 r">' + row.MTotal + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.AgentComm) < 0 ? 'red' : '0') + '">' + row.AgentComm + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.MasterComm) < 0 ? 'red' : '0') + '">' + row.MasterComm + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.SuperComm) < 0 ? 'red' : '0') + '">' + row.SuperComm + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.CompanyComm) < 0 ? 'red' : '0') + '">' + row.CompanyComm + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.GrossCommBase) < 0 ? 'red' : '0') + '">' + row.GrossCommBase + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.CompanyCommBase) < 0 ? 'red' : '0') + '">' + row.CompanyCommBase + '</div></td>\
            </tr>';
        return html;
    },
    renderDetailRow: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
               <td><div class="col100 c">' + row.UserName + '</div></td>\
                <td><div class="col80 r">' + row.GrossComm + '</div></td>\
                <td><div class="col80 r">' + row.MWinLoss + '</div></td>\
                <td><div class="col80 r">' + row.MComm + '</div></td>\
                <td><div class="col80 r">' + row.MTotal + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.AgentComm) < 0 ? 'red' : '0') + '">' + row.AgentComm + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.MasterComm) < 0 ? 'red' : '0') + '">' + row.MasterComm + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.SuperComm) < 0 ? 'red' : '0') + '">' + row.SuperComm + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.CompanyComm) < 0 ? 'red' : '0') + '">' + row.CompanyComm + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.GrossCommBase) < 0 ? 'red' : '0') + '">' + row.GrossCommBase + '</div></td>\
                <td><div class="col80 r ' + (parseFloat(row.CompanyCommBase) < 0 ? 'red' : '0') + '">' + row.CompanyCommBase + '</div></td>\
            </tr>';
        return html;
    },
    renderTotalRow: function (row, cssClass, total) {
        var html = '';
        var totalhtml = '<td><div class="col100 c b">' + row.CurrencyName + '</div></td>';
        if (total > 0) {
            totalhtml = '<td rowspan="' + total + '" style="border-bottom:none 0px;"><div class="col100 b c">Total</div></td><td><div class="col100 b c">' + row.CurrencyName + '</div></td>';
        }
        html = '<tr class="' + cssClass + '">' + totalhtml + '\
                   <td><div class="col80 r">' + row.GrossComm + '</div></td>\
                    <td><div class="col80 r">' + row.MWinLoss + '</div></td>\
                    <td><div class="col80 r">' + row.MComm + '</div></td>\
                    <td><div class="col80 r">' + row.MTotal + '</div></td>\
                    <td><div class="col80 r ' + (parseFloat(row.AgentComm) < 0 ? 'red' : '0') + '">' + row.AgentComm + '</div></td>\
                    <td><div class="col80 r ' + (parseFloat(row.MasterComm) < 0 ? 'red' : '0') + '">' + row.MasterComm + '</div></td>\
                    <td><div class="col80 r ' + (parseFloat(row.SuperComm) < 0 ? 'red' : '0') + '">' + row.SuperComm + '</div></td>\
                    <td><div class="col80 r ' + (parseFloat(row.CompanyComm) < 0 ? 'red' : '0') + '">' + row.CompanyComm + '</div></td>\
                    <td><div class="col80 r ' + (parseFloat(row.GrossCommBase) < 0 ? 'red' : '0') + '">' + row.GrossCommBase + '</div></td>\
                    <td><div class="col80 r ' + (parseFloat(row.CompanyCommBase) < 0 ? 'red' : '0') + '">' + row.CompanyCommBase + '</div></td>\
                </tr>';
        return html;
    },
    renderGrandTotal: function (row) {
        var html = '<tr>\
                        <td colspan="10"><div class="r" style="width:867px">Grand Total</div></td>\
                        <td><div class="col80 r ' + (parseFloat(row.GrossCommBase) < 0 ? 'red' : '0') + '">' + row.GrossCommBase + '</div></td>\
                        <td><div class="col80 r ' + (parseFloat(row.CompanyCommBase) < 0 ? 'red' : '0') + '">' + row.CompanyCommBase + '</div></td>\
                    </tr>';
        return html;
    },
    renderGrandTotalDetail: function (row) {
        var html = '<tr>\
                        <td colspan="9"><div class="r" style="width:764px">Grand Total</div></td>\
                        <td><div class="col80 r ' + (parseFloat(row.GrossCommBase) < 0 ? 'red' : '0') + '">' + row.GrossCommBase + '</div></td>\
                        <td><div class="col80 r ' + (parseFloat(row.CompanyCommBase) < 0 ? 'red' : '0') + '">' + row.CompanyCommBase + '</div></td>\
                    </tr>';
        return html;
    },
    renderMainReport: function (date) {
        if (date != '') {
            $("#fdate").val(date);
            $("#tdate").val(date);
        }
        pokerReport.fromDate = $("#fdate").val();
        pokerReport.toDate = $("#tdate").val();
        $("#winlossdetail").addClass("hidden");
        $(".maincontent_box").css("width", "1049px");

        var winlossmain = $("#winlossmain");

        $("#ErrMsg").html("");

        var data = {
            FromDate: pokerReport.fromDate,
            ToDate: pokerReport.toDate
        };
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "service/PokerService.svc/SelWinlostP2P",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                pokerReport.title = "P2P Win/Loss From " + pokerReport.fromDate + " To " + pokerReport.toDate;
                $("#gridtitle").html(pokerReport.title);
                var arr = [];
                var items = rs.Items;
                if (items != null && items.length > 0) {

                    var numberitems = items.length;
                    for (var i = 0; i < numberitems; i++) {
                        arr.push(pokerReport.renderMainRow(items[i], (i % 2 == 0 ? "bgeven" : "")));
                    }

                    var totalitems = rs.Total;
                    var numbertotalitems = totalitems.length;
                    for (var j = 0; j < numbertotalitems; j++) {
                        if (j == 0) {
                            arr.push(pokerReport.renderTotalRow(totalitems[j], "total", numbertotalitems));
                        }
                        else {
                            arr.push(pokerReport.renderTotalRow(totalitems[j], "total", 0));
                        }
                    }
                    $("#tblwinlossmain").empty().html(arr.join(''));
                    $("#tblFooter").empty().html(pokerReport.renderGrandTotal(rs.GrandTotal));

                    winlossmain.removeClass("hidden").reRenderScroller(1036, 542, true); //100*10+10+10*2
                }
                else {
                    $("#tblwinlossmain").empty().html('<tr><td colspan="12"><div class="c" style="width:1033px">There is no information available.</div></td></tr>');
                    $("#tblFooter").empty();
                    winlossmain.removeClass("hidden").reRenderScroller(1036, 541, true);
                }

            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDetailReport: function (currencyName, currency, date) {
        pokerReport.SelectedDate = date;
        pokerReport.selectedCurrency = currency;
        $("#winlossmain").addClass("hidden");
        $(".maincontent_box").css("width", "946px");
        var winlossdetail = $("#winlossdetail");

        $("#ErrMsg").html("");

        var data = {
            currency: pokerReport.selectedCurrency,
            wldate: pokerReport.SelectedDate
        };

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "service/PokerService.svc/SelWinlostP2PDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                pokerReport.detailtitle = "P2P Win/Loss Detail - " + currencyName + " - " + date;
                $("#gridtitledetail").html(pokerReport.detailtitle);
                var arr = [];
                var items = rs.Items;
                if (items != null && items.length > 0) {
                    pokerReport.mainData = rs;
                    var numberitems = items.length;

                    for (var i = 0; i < numberitems; i++) {
                        arr.push(pokerReport.renderDetailRow(items[i], (i % 2 == 0 ? "bgeven" : "")));
                    }

                    $("#tblwinlossdetail").empty().html(arr.join(''));
                    $("#tblFooterDetail").empty().html(pokerReport.renderGrandTotalDetail(rs.GrandTotal));
                    winlossdetail.removeClass("hidden").reRenderScroller(933, 542, true); //100*10+10+10*2
                }
                else {
                    $("#tblwinlossdetail").empty().html('<tr><td colspan="12"><div class="c" style="width:990px">There is no information available.</div></td></tr>');
                    $("#tblFooterDetail").empty();
                    winlossdetail.removeClass("hidden").reRenderScroller(933, 541, true);
                }

            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    goBack: function () {
        $(".maincontent_box").css("width", "1049px");
        $("#winlossmain").removeClass("hidden");
        $("#winlossdetail").addClass("hidden");
    },
    exportExcel: function () {
        var url = 'PokerWL.aspx?isexport=1&rptype=1&rptitle=' + pokerReport.title + '&fdate=' + pokerReport.fromDate + '&tdate=' + pokerReport.toDate;
        $('#frm').attr('src', url);

    }
    ,
    exportExcelDetail: function () {
        var url = 'PokerWL.aspx?isexport=1&rptype=2&rptitle=' + pokerReport.detailtitle + '&sdate=' + pokerReport.SelectedDate + '&currency=' + pokerReport.selectedCurrency;
        $('#frm').attr('src', url);

    }

};

function daysInMonth(iMonth, iYear) {
    return 32 - new Date(iYear, iMonth - 1, 32).getDate();
}

function GetDateTime(vl) {
    var data = {
        dateOption: vl
    };
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../Licensee/Service/LicenseeService.svc/GetDatetimeOption",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {
            $('#fdate').val(rs.fdate);
            $('#tdate').val(rs.tdate);
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}
$(document).ready(function () {
    var options = {
        showOtherMonths: true,
        selectOtherMonths: true,
        checkMinMax: true
    };
    calendarUtils.renderCalendar("#fdate", null, "#fdate", "", fDay, options);
    calendarUtils.renderCalendar("#tdate", null, "#fdate", "#tdate", tDay, options);
    if (showOnload) {
        pokerReport.renderMainReport('');
    }
});

