var _pageIndex = 1;
var _pageSize = 50;
var _totalItem = 0;
$(document).ready(function () {
    var gridOpts = {
        tblContentId: 'tbWinloss',
        isShowPager: true
    };
    $("#winlossmain").initGrid(gridOpts);

    licenseeReport.renderCalendarFromTo("#fdate, #tdate");
});

$(".txtdate").datepicker({
    onSelect: function () {
        if ($('#fdate').val() != licenseeReport.defaultDate | $('#tdate').val() != licenseeReport.defaultDate) {
            $("#dlDate").attr('selectedIndex', 7);
        }
    }
});

function getLicensees() {
    var param = {
        IsAll: false
    };
    $.ajax({
        type: "POST",
        url: "Service/LicenseeService.svc/SelLicensees",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(param),
        success: function (rs) {
            var dlSite = $('#dlSite');
            for (var i = 0; i < rs.length; i++) {
                var brandName = rs[i].BrandName;
                var licenseeName = rs[i].Name;
                if (brandName != null && brandName != "" && brandName != licenseeName) {
                    dlSite.append("<option enableBA='" + rs[i].EnableBA + "' level='" + rs[i].Level + "' value='" + rs[i].Id + "'>" + licenseeName + " (" + brandName + ")</option>");
                }
                else {
                    dlSite.append("<option enableBA='" + rs[i].EnableBA + "' level='" + rs[i].Level + "' value='" + rs[i].Id + "'>" + licenseeName + "</option>");
                }
            }
        }
    });
}

function ResetValue() {
    _pageIndex = 1;
    _pageSize = 50;
    _totalItem = 0;
}

var tmplCurrency = '<option value="0">All</option><option value="-3">Base Currency-USD</option><option value="-2">Base Currency-RM</option><option value="-6">Base Currency-EUR</option>';

function SelCurrency() {
    var enableBA = $("#dlSite").find("option:selected").attr("enableBA");
    if (enableBA == "false")
        $('#ddlBA').attr('disabled', 'disabled');
    else
        $('#ddlBA').removeAttr('disabled');
    var data = {
        siteId: $('#dlSite').val()
    };
    if ($('#dlSite').val() == '0')
        return;
    $("#ErrMsg").html("");

    LicenseeReportHelper.CheckLicenseeOfferRNGCasino(data.siteId);

    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "Service/LicenseeService.svc/SelCurrency",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {
            var dlCurrency = $('#dlCurrency');
            dlCurrency.empty().append(tmplCurrency);
            for (i = 0; i < rs.currency.length; i++) {
                var item = rs.currency[i];
                dlCurrency.append('<option value="' + item.currencyId + '_' + item.custId + '">' + item.currency + '</option>');
            }
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function GetDateTime(vl) {
    if (vl != 8) {
        var data = {
            dateOption: vl
        };
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/LicenseeService.svc/GetDatetimeOption",
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
}

function DoReset() {
    obj('dlSite').selectedIndex = 0;
    obj('dlDate').selectedIndex = 0;
    obj('dlCurrency').selectedIndex = 0;
    obj('ddltype').selectedIndex = 0;
    GetDateTime(0);
}

function ExportToExcel() {
    try { parent.BlockUI(); } catch (e) { }

    var reportType, baseCurrency;
    switch ($('#dlCurrency').val()) {
        case "0": //default
            baseCurrency = 0;
            break;
        case "-1":
            baseCurrency = 3; //USD
            break;
        case "-2":
            baseCurrency = 2; //RM
            break;
        case "-6":
            baseCurrency = 6; //EUR
            break;
        default:
            baseCurrency = 0;
            break;
    }

    reportType = $("input[name='RdReportBy']:checked").val();
    $("#frm").attr('src', 'Deposit.aspx?IsExport=1&site=' + $('#dlSite').val() + '&level=' + $('#level').val() + '&currency=' + $('#currency').val() + '&baseCurrency=' + baseCurrency
        + '&fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val() + '&reportType=' + reportType + '&baseCurrency=' + baseCurrency);

    try { parent.jQuery.unblockUI(); } catch (e) { }
}

var licenseeReport = {
    defaultDate: '',
    SelectedDate: '',
    funcMain: null,
    funcDetail12Bet: null,
    funcMasterDetail: null,
    funcSuperDetail: null,
    level: '',
    isDetail: false,
    fromDate: '',
    toDate: '',
    sportID: '',
    licenseeId: 0,
    currencyId: 0,
    reportType: 0,
    drawFilter: 0,
    baseCurrency: 0,
    custId: 0,
    reportSelected: 0,
    excludeTesting: 0,
    ShowBA: 0,
    ShowNG: 0,
    ShowSB: 0,
    ShowRB: 0,
    ShowLC: 0,
    ShowVS: 0,
    ShowCS: 0,
    ReportLevel: 0,
    getTitle: function () {
        //return "Deposit report for " + $('#dlSite option:selected').text() + " from " + $("#fdate").val() + " to " + $("#tdate").val()
        return "Deposit report for " + $('#dlSite option:selected').text() + " from " + licenseeReport.fromDate + " to " + licenseeReport.toDate
    },
    renderMainRow: function (rows, cssClass) {
        var html = '';
        var itemByDate = rows.LicenseeByDateList.length;
        html = '<tr class="' + cssClass + '">';
        html += '<td rowspan="' + itemByDate + '"><div class="l" style="width:100px">' + rows.DateTime + '</div></td>';
        for (var i = 0; i < itemByDate; i++) {
            var row = rows.LicenseeByDateList[i];
            if (i > 0) {
                html += '<tr class="' + cssClass + '">';
            }
            html += '<td><div class="c link" style="width:60px; height:17px;" onclick="ResetValue();licenseeReport.ViewSuperDetailReport(' + row.CurrencyId + ', 0 ,\'' + rows.DateTime + '\')">' + row.CurrencyName + '</div></td>';
            html += '<td><div class="r" style="width:100px; height:17px;">' + row.Turnover + '</div></td>';
            html += '<td><div class="r" style="width:70px; height:17px;">' + row.BetCount + '</div></td>';
            html += '<td><div class="r" style="width:100px; height:17px;">' + row.CustomerWL + '</div></td>';
            html += '<td><div class="r" style="width:100px; height:17px;">' + row.CustomerComm + '</div></td>';
            html += '<td><div class="r" style="width:100px; height:17px;">' + row.CustomerWLTotal + '</div></td>'
            html += '<td><div class="r" style="width:120px; height:17px;">' + row.LicenseeWL + '</div></td>';
            html += '<td><div class="r" style="width:80px; height:17px;">' + row.BA + '</div></td>';
            html += '<td><div class="r" style="width:100px; height:17px;">' + row.WinlossRB + '</div></td>';
            html += '<td><div class="r" style="width:100px; height:17px;">' + row.Racing + '</div></td>';
            html += '<td><div class="r" style="width:100px; height:17px;">' + row.Total + '</div></td>';
            html += '<td><div class="r" style="width:100px; height:17px;">' + row.AmtDuetoBA + '</div></td>';
            html += '<td><div class="r" style="width:100px; height:17px;">' + row.AmtDuetoRB + '</div></td>';
            html += '<td><div class="r" style="width:120px; height:17px;">' + row.Company + '</div></td>';

            html += '</tr>'
        }

        return html;
    },
    renderMainFirstCols: function (rows, cssClass) {
        var html = '';
        var itemByDate = rows.LicenseeByDateList.length;
        html = '<tr class="' + cssClass + '">';
        html += '<td rowspan="' + itemByDate + '"><div class="l" style="width:100px">' + rows.DateTime + '</div></td>';
        for (var i = 0; i < itemByDate; i++) {
            var row = rows.LicenseeByDateList[i];
            if (i > 0) {
                html += '<tr class="' + cssClass + '">';
            }
            html += '<td><div class="c link" style="width:60px; height:17px;" onclick="ResetValue();licenseeReport.ViewSuperDetailReport(' + row.CurrencyId + ', 0 ,\'' + rows.DateTime + '\')">' + row.CurrencyName + '</div></td>';
            html += '<td><div class="r" style="width:100px; height:17px;">' + row.Turnover + '</div></td>';
            html += '<td><div class="r" style="width:70px; height:17px;">' + row.BetCount + '</div></td>';
            html += '</tr>'
        }

        return html;
    },
    renderSuperDetailRow: function (row, cssClass, date) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="l link" style="width:100px; height:17px;" onclick="ResetValue();licenseeReport.ViewMasterDetailReport(' + row.CurrencyId + ',0,' + row.CustId + ',\'' + date + '\')">' + (row.Username != null ? row.Username : "") + '</div></td>\
                <td><div class="c" style="width:60px; height:17px;">' + row.CurrencyName + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Turnover + '</div></td>\
                <td><div class="r" style="width:70px; height:17px;">' + row.BetCount + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.CustomerWL + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.CustomerComm + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.CustomerWLTotal + '</div></td>\
                <td><div class="r" style="width:120px; height:17px;">' + row.LicenseeWL + '</div></td>\
                <td><div class="r" style="width:80px; height:17px;">' + row.BA + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.WinlossRB + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Racing + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Total + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.AmtDuetoBA + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.AmtDuetoRB + '</div></td>\
                <td><div class="r" style="width:120px; height:17px;">' + row.Company + '</div></td>\
            </tr>';
        return html;
    },
    renderSuperDetailFirstCols: function (row, cssClass, date) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="l link" style="width:100px; height:17px;" onclick="ResetValue();licenseeReport.ViewMasterDetailReport(' + row.CurrencyId + ',0,' + row.CustId + ',\'' + date + '\')">' + (row.Username != null ? row.Username : "") + '</div></td>\
                <td><div class="c" style="width:60px; height:17px;">' + row.CurrencyName + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Turnover + '</div></td>\
                <td><div class="r" style="width:70px; height:17px;">' + row.BetCount + '</div></td>\
            </tr>';
        return html;
    },
    renderMasterDetailRow: function (row, cssClass, date) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="l link" style="width:100px; height:17px;" onclick="ResetValue();licenseeReport.renderDetail12BetReport(' + row.CurrencyId + ',0,' + row.CustId + ',\'' + date + '\')">' + (row.Username != null ? row.Username : "") + '</div></td>\
                <td><div class="c" style="width:60px; height:17px;">' + row.CurrencyName + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Turnover + '</div></td>\
                <td><div class="r" style="width:70px; height:17px;">' + row.BetCount + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.CustomerWL + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.CustomerComm + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.CustomerWLTotal + '</div></td>\
                <td><div class="r" style="width:120px; height:17px;">' + row.LicenseeWL + '</div></td>\
                <td><div class="r" style="width:80px; height:17px;">' + row.BA + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.WinlossRB + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Racing + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Total + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.AmtDuetoBA + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.AmtDuetoRB + '</div></td>\
                <td><div class="r" style="width:120px; height:17px;">' + row.Company + '</div></td>\
            </tr>';
        return html;
    },
    renderMasterDetailFirstCols: function (row, cssClass, date) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="l link" style="width:100px; height:17px;" onclick="ResetValue();licenseeReport.renderDetail12BetReport(' + row.CurrencyId + ',0,' + row.CustId + ',\'' + date + '\')">' + (row.Username != null ? row.Username : "") + '</div></td>\
                <td><div class="c" style="width:60px; height:17px;">' + row.CurrencyName + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Turnover + '</div></td>\
                <td><div class="r" style="width:70px; height:17px;">' + row.BetCount + '</div></td>\
            </tr>';
        return html;
    },
    renderDetailRow: function (row, cssClass, date) {

        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="l link" style="width:100px; height:17px;" onclick="licenseeReport.viewBetList(' + row.CustId + ',\'' + (row.Username != null ? row.Username : "") + '\',\'' + licenseeReport.baseCurrency + '\',' + licenseeReport.ShowBA + ', \'' + date + '\')">' + (row.Username != null ? row.Username : "") + '</div></td>\
                <td><div class="c" style="width:60px; height:17px;">' + row.CurrencyName + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Turnover + '</div></td>\
                <td><div class="r" style="width:70px; height:17px;">' + row.BetCount + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.CustomerWL + '</div></td>\
                <td><div class="r"  style="width:100px; height:17px;">' + row.CustomerComm + '</div></td>\
                <td><div class="r"  style="width:100px; height:17px;">' + row.CustomerWLTotal + '</div></td>\
                <td><div class="r" style="width:120px; height:17px;">' + row.LicenseeWL + '</div></td>\
                <td><div class="r" style="width:80px; height:17px;">' + row.BA + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.WinlossRB + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Racing + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Total + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.AmtDuetoBA + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.AmtDuetoRB + '</div></td>\
                <td><div class="r" style="width:120px; height:17px;">' + row.Company + '</div></td>\
            </tr>';
        return html;
    },
    renderDetailFirstCols: function (row, cssClass, date) {

        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="l link" style="width:100px; height:17px;" onclick="licenseeReport.viewBetList(' + row.CustId + ',\'' + (row.Username != null ? row.Username : "") + '\',\'' + licenseeReport.baseCurrency + '\',' + licenseeReport.ShowBA + ', \'' + date + '\')">' + (row.Username != null ? row.Username : "") + '</div></td>\
                <td><div class="c" style="width:60px; height:17px;">' + row.CurrencyName + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Turnover + '</div></td>\
                <td><div class="r" style="width:70px; height:17px;">' + row.BetCount + '</div></td>\
            </tr>';
        return html;
    },
    renderDetail12BetRow: function (row, cssClass, date) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="l link" style="width:100px; height:17px;" onclick="ResetValue();licenseeReport.renderDetailReport(' + row.CurrencyId + ',0,' + row.CustId + ',\'' + date + '\')">' + (row.Username != null ? row.Username : "") + '</div></td>\
                <td><div class="c" style="width:60px; height:17px;">' + row.CurrencyName + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Turnover + '</div></td>\
                <td><div class="r" style="width:70px; height:17px;">' + row.BetCount + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.CustomerWL + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.CustomerComm + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.CustomerWLTotal + '</div></td>\
                <td><div class="r" style="width:120px; height:17px;">' + row.LicenseeWL + '</div></td>\
                <td><div class="r" style="width:80px; height:17px;">' + row.BA + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.WinlossRB + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Racing + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Total + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.AmtDuetoBA + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.AmtDuetoRB + '</div></td>\
                <td><div class="r" style="width:120px; height:17px;">' + row.Company + '</div></td>\
            </tr>';
        return html;
    },
    renderDetail12BetFristCols: function (row, cssClass, date) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="l link" style="width:100px; height:17px;" onclick="ResetValue();licenseeReport.renderDetailReport(' + row.CurrencyId + ',0,' + row.CustId + ',\'' + date + '\')">' + (row.Username != null ? row.Username : "") + '</div></td>\
                <td><div class="c" style="width:60px; height:17px;">' + row.CurrencyName + '</div></td>\
                <td><div class="r" style="width:100px; height:17px;">' + row.Turnover + '</div></td>\
                <td><div class="r" style="width:70px; height:17px;">' + row.BetCount + '</div></td>\
            </tr>';
        return html;
    },
    renderTotalRow: function (row, cssClass, total) {
        var html = '';
        var totalhtml = '<td><div class="c b" style="width:60px; height:17px;">' + row.CurrencyName + '</div></td>';
        if (total > 0) {
            totalhtml = '<td rowspan="' + total + '"><div class="b c" style="width:100px; height:17px;">Total</div></td><td><div class="col60 b c">' + row.CurrencyName + '</div></td>';
        }
        html = '<tr class="' + cssClass + '">' + totalhtml + '\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.Turnover + '</div></td>\
                    <td><div class="r b" style="width:70px; height:17px;">' + (row.BetCount ? row.BetCount : '') + '</div></td>\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.CustomerWL + '</div></td>\
                    <td><div class="r b"  style="width:100px; height:17px;">' + row.CustomerComm + '</div></td>\
                    <td><div class="r b"  style="width:100px; height:17px;">' + row.CustomerWLTotal + '</div></td>\
                    <td><div class="r b" style="width:120px; height:17px;">' + row.LicenseeWL + '</div></td>\
                    <td><div class="r b" style="width:80px; height:17px;">' + row.BA + '</div></td>\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.WinlossRB + '</div></td>\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.Racing + '</div></td>\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.Total + '</div></td>\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.AmtDuetoBA + '</div></td>\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.AmtDuetoRB + '</div></td>\
                    <td><div class="r b" style="width:120px; height:17px;">' + row.Company + '</div></td>\
                </tr>';
        return html;
    },
    renderTotalFirstCols: function (row, cssClass, total) {

        var html = '';
        var totalhtml = '<td><div class="c b" style="width:60px;">' + row.CurrencyName + '</div></td>';
        if (total > 0) {
            totalhtml = '<td rowspan="' + total + '"><div class="b c" style="width:100px; height:17px;">Total</div></td><td><div class="col60 b c">' + row.CurrencyName + '</div></td>';
        }
        html = '<tr class="' + cssClass + '">' + totalhtml + '\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.Turnover + '</div></td>\
                    <td><div class="r b" style="width:70px; height:17px;">' + (row.BetCount ? row.BetCount : '') + '</div></td>\
                </tr>';
        return html;
    },
    renderGrandTotalRow: function (row, cssClass, total) {
        var html = '';
        var totalhtml = '<td><div class="c b" style="width:60px; height:17px;">' + row.CurrencyName + '</div></td>';
        if (total > 0) {
            totalhtml = '<td rowspan="' + total + '" style="border-bottom:none 0px;"><div class="b c" style="width:100px; height:17px;">Total</div></td><td><div class="col60 b c">' + row.CurrencyName + '</div></td>';
        }
        var totalbethtml = '';
        if (total > 0) {
            totalbethtml = '<td rowspan="' + total + '" style="border-bottom:0px none;"><div class="r b" style="width:70px; height:17px;">' + (row.BetCount ? row.BetCount : '') + '</div></td>';
        }
        html = '<tr class="' + cssClass + '">' + totalhtml + '\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.Turnover + '</div></td>' + totalbethtml + '<td>\
                    <div class="r b"  style="width:100px; height:17px;">' + row.CustomerWL + '</div></td>\
                    <td><div class="r b"  style="width:100px; height:17px;">' + row.CustomerComm + '</div></td>\
                    <td><div class="r b"  style="width:100px; height:17px;">' + row.CustomerWLTotal + '</div></td>\
                    <td><div class="r b" style="width:120px; height:17px;">' + row.LicenseeWL + '</div></td>\
                    <td><div class="r b" style="width:80px; height:17px;">' + row.BA + '</div></td>\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.WinlossRB + '</div></td>\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.Racing + '</div></td>\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.Total + '</div></td>\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.AmtDuetoBA + '</div></td>\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.AmtDuetoRB + '</div></td>\
                    <td><div class="r b" style="width:120px; height:17px;">' + row.Company + '</div></td>\
                </tr>';
        return html;

    },
    renderGrandTotalFirstCols: function (row, cssClass, total) {
        var html = '';
        var totalhtml = '<td><div class="c b" style="width:60px; height:17px;">' + row.CurrencyName + '</div></td>';
        if (total > 0) {
            totalhtml = '<td rowspan="' + total + '" style="border-bottom:none 0px;"><div class="b c" style="width:100px; height:17px;">Total</div></td><td><div class="col60 b c">' + row.CurrencyName + '</div></td>';
        }
        var totalbethtml = '';
        if (total > 0) {
            totalbethtml = '<td rowspan="' + total + '" style="border-bottom:0px none;"><div class="r b" style="width:70px; height:17px;">' + (row.BetCount ? row.BetCount : '') + '</div></td>';
        }
        html = '<tr class="' + cssClass + '">' + totalhtml + '\
                    <td><div class="r b" style="width:100px; height:17px;">' + row.Turnover + '</div></td>' + totalbethtml + '\
                </tr>';
        return html;

    },
    initPagerMain: function (total_item, items_per_page, current_page) {
        var opt = { callback: licenseeReport.pageSelectCallbackMain };
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    pageSelectCallbackMain: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_totalItem / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        _pageIndex = page_index + 1;
        _pageSize = _items_per_page;
        licenseeReport.renderMainReport(1);
        return false;
    },
    initPagerSuper: function (total_item, items_per_page, current_page) {
        var opt = { callback: licenseeReport.pageSelectCallbackSuper };
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    pageSelectCallbackSuper: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_totalItem / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        _pageIndex = page_index + 1;
        _pageSize = _items_per_page;
        licenseeReport.ViewSuperDetailReport(licenseeReport.currencyId, 1, licenseeReport.fromDate);
        return false;
    },
    initPagerMaster: function (total_item, items_per_page, current_page) {
        var opt = { callback: licenseeReport.pageSelectCallbackMaster };
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    pageSelectCallbackMaster: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_totalItem / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        _pageIndex = page_index + 1;
        _pageSize = _items_per_page;
        licenseeReport.ViewMasterDetailReport(licenseeReport.currencyId, 1, licenseeReport.custId, licenseeReport.fromDate);
        return false;
    },

    initPagerDetail: function (total_item, items_per_page, current_page) {
        var opt = { callback: licenseeReport.pageSelectCallbackDetail };
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    pageSelectCallbackDetail: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_totalItem / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        _pageIndex = page_index + 1;
        _pageSize = _items_per_page;
        licenseeReport.renderDetailReport(licenseeReport.currencyId, 1, licenseeReport.custId, licenseeReport.fromDate);
        return false;
    },
    initPagerDetail12Bet: function (total_item, items_per_page, current_page) {
        var opt = { callback: licenseeReport.pageSelectCallbackDetail12Bet };
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    pageSelectCallbackDetail12Bet: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_totalItem / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        _pageIndex = page_index + 1;
        _pageSize = _items_per_page;

        licenseeReport.renderDetail12BetReport(licenseeReport.currencyId, 1, licenseeReport.baseCurrency, licenseeReport.fromDate);
        return false;
    },
    setPostedData: function (fromCache) {
        var data = {
            LicenseeID: licenseeReport.licenseeId,
            Currency: licenseeReport.currencyId,
            baseCurrency: licenseeReport.baseCurrency,
            CustId: licenseeReport.custId,
            FromDate: licenseeReport.fromDate,
            ToDate: licenseeReport.toDate,
            showSB: licenseeReport.ShowSB,
            ShowRB: licenseeReport.ShowRB,
            ShowNG: licenseeReport.ShowNG,
            ShowBA: licenseeReport.ShowBA,
            ShowLC: licenseeReport.ShowLC,
            ShowCS: licenseeReport.ShowCS,
            ShowVS: licenseeReport.ShowVS,
            ReportType: licenseeReport.reportType,
            drawfilter: licenseeReport.drawFilter,
            excludeTest: licenseeReport.excludeTesting,
            pageIndex: _pageIndex,
            pageSize: _pageSize,
            fromCache: fromCache
        };

        LicenseeReportHelper.displayLicensee();

        return data;
    },
    renderMainReport: function (fromCache) {
        licenseeReport.ReportLevel = 0;
        this.level = $("#dlSite").find("option:selected").attr("level");
        this.isDetail = false;
        this.funcMain = function () {
            this.renderMainReport(1);
        }
        $("#btnBackToMain").hide();
        this.reportSelected = 0;
        licenseeReport.fromDate = $("#fdate").val();
        licenseeReport.toDate = $("#tdate").val();
        licenseeReport.licenseeId = parseInt($("#dlSite").val());
        licenseeReport.currencyId = parseInt($("#dlCurrency").val());
        licenseeReport.baseCurrency = parseInt($('#dlCurrency').val()) < 0 ? parseInt($('#dlCurrency').val()) * (-1) : 0;
        licenseeReport.drawFilter = parseInt($("#ddltype").val());
        licenseeReport.reportType = parseInt($("input[name=RdReportBy]:checked").val());
        //licenseeReport.sportID = $('#dlSport').val();
        licenseeReport.excludeTesting = parseInt($("input[name=rdExcludeTesting]:checked").val());
        if ($("#chbShowBA").attr("checked"))
            licenseeReport.ShowBA = 1;
        else
            licenseeReport.ShowBA = 0;
        if ($("#chbSport").attr("checked"))
            licenseeReport.ShowSB = 1;
        else
            licenseeReport.ShowSB = 0;
        if ($("#chbRacing").attr("checked"))
            licenseeReport.ShowRB = 1;
        else
            licenseeReport.ShowRB = 0;
        if ($("#chbNG").attr("checked"))
            licenseeReport.ShowNG = 1;
        else
            licenseeReport.ShowNG = 0;

        if ($("#chbLiveCasino").attr("checked"))
            licenseeReport.ShowLC = 1;
        else
            licenseeReport.ShowLC = 0;

        if ($("#chbVS").attr("checked"))
            licenseeReport.ShowVS = 1;
        else
            licenseeReport.ShowVS = 0;

        if ($("#chbRNGCasino").attr("checked"))
            licenseeReport.ShowCS = 1;
        else
            licenseeReport.ShowCS = 0;

        if (licenseeReport.licenseeId == 0) {
            showAlertMessage("ErrMsg", "Please choose a Licensee.", 1);
            return;
        }

        $("#ErrMsg").html("");
        var mainreport = $("#winlossmain");

        var data = licenseeReport.setPostedData(fromCache);
        this.currencyId = data.Currency;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/LicenseeService.svc/SelReportByLicenseeV2",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                $("#gridtitle").html(licenseeReport.getTitle);
                if (data.ReportType == 3) {
                    $("#colReportBy").html("Sport");
                }
                else {
                    $("#colReportBy").html("Date");
                }
                var arr = [];
                var arrfirst = [];
                var arr_total = [];
                var arr_totalFirstcols = [];
                var arr_granttotal = [];
                var arr_granttotalFirstcols = [];
                var items = rs.Items;
                if (items != null && items.length > 0) {

                    var numberitems = items.length;

                    for (var i = 0; i < numberitems; i++) {
                        arr.push(licenseeReport.renderMainRow(items[i], (i % 2 == 0 ? "bgeven" : "")));
                        arrfirst.push(licenseeReport.renderMainFirstCols(items[i], (i % 2 == 0 ? "bgeven" : "")));
                    }

                    var totalitems = rs.Totals;
                    var numbertotalitems = totalitems.length;
                    for (var j = 0; j < numbertotalitems; j++) {
                        if (j == 0) {
                            arr_total.push(licenseeReport.renderTotalRow(totalitems[j], "total", numbertotalitems));
                            arr_totalFirstcols.push(licenseeReport.renderTotalFirstCols(totalitems[j], "total", numbertotalitems));

                        }
                        else {
                            arr_total.push(licenseeReport.renderTotalRow(totalitems[j], "total", 0));
                            arr_totalFirstcols.push(licenseeReport.renderTotalFirstCols(totalitems[j], "total", 0));
                        }
                    }

                    var grandtotalitems = rs.TotalsBaseCurrency;
                    var numbergrandtotalitems = grandtotalitems.length;
                    var totalbet = 0;
                    for (var k = 0; k < numbergrandtotalitems; k++) {
                        if (k == 0) {
                            arr_granttotal.push(licenseeReport.renderGrandTotalRow(grandtotalitems[k], "grandtotal", numbergrandtotalitems));
                            arr_granttotalFirstcols.push(licenseeReport.renderGrandTotalFirstCols(grandtotalitems[k], "grandtotal", numbergrandtotalitems));
                        }
                        else {
                            arr_granttotal.push(licenseeReport.renderGrandTotalRow(grandtotalitems[k], "grandtotal", 0));
                            arr_granttotalFirstcols.push(licenseeReport.renderGrandTotalFirstCols(grandtotalitems[k], "grandtotal", 0));
                        }
                        totalbet += parseInt(grandtotalitems[k].BetCount);
                    }

                    mainreport.resetGrid("tbWinloss");
                    $("#tbWinloss tbody").empty().append(arr.join('')).append(arr_total.join(''));
                    $("#tbwinlossrowheader tbody").empty().append(arrfirst.join('')).append(arr_totalFirstcols.join(''));
                    mainreport.find(".row_footer_content thead").empty().append(arr_granttotalFirstcols.join(''));
                    mainreport.find(".col_footer_content thead").empty().append(arr_granttotal.join(''));

                    _totalItem = rs.totalrow;
                    licenseeReport.initPagerMain(_totalItem, _pageSize, _pageIndex - 1);


                }
                else {
                    $("#tbWinloss tbody").empty().html('<tr><td colspan="15"><div class="c" >There is no information available.</div></td></tr>');
                    $("#tbwinlossrowheader tbody").empty().html('<tr><td colspan="4" style="background:none repeat scroll 0 0 #CDDEF3"><div style="width:330px">&nbsp;</div></td></tr>');
                    mainreport.find(".row_footer_content thead").empty()
                    mainreport.find(".col_footer_content thead").empty()
                    $("#pagination").html('&nbsp;');
                }

                var gridOpts = {
                    tblContentId: 'tbWinloss',
                    manualCopyHeader: true,
                    fullWidth: true,
                    fullHeight: true,
                    gridHeight: 160,
                    freezeWidth: 345,
                    numberFreezeCol: 4
                };
                mainreport.removeClass("hidden");
                mainreport.gridScroller(gridOpts);

            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    ViewSuperDetailReport: function (currencyId, fromCache, date) {
        licenseeReport.ReportLevel = 1;
        var mainreport = $("#winlossmain");
        this.reportSelected = 1;
        this.isDetail = false;
        this.currencyId = currencyId;
        licenseeReport.funcSuperDetail = function () {
            licenseeReport.ViewSuperDetailReport(currencyId, 1, date);
        };
        if (parseInt(licenseeReport.reportType) == 1) {
            licenseeReport.fromDate = date;
            licenseeReport.toDate = date;
        }

        if (parseInt(licenseeReport.reportType) == 2) {

            var dt = date.split('/');
            var startD = $("#fdate").val().split('/');
            var endD = $("#tdate").val().split('/');

            if (parseInt(dt[0]) == parseInt(startD[0])) {
                licenseeReport.fromDate = dt[0] + "/" + startD[1] + "/" + dt[1];
            }
            else {
                licenseeReport.fromDate = dt[0] + "/01/" + dt[1];
            }

            if (parseInt(dt[0]) == parseInt(endD[0])) {
                licenseeReport.toDate = dt[0] + "/" + endD[1] + "/" + dt[1];

            }
            else {
                licenseeReport.toDate = dt[0] + "/" + daysInMonth(parseInt(dt[0]), parseInt(dt[1])) + "/" + dt[1];
            }

        }
        var data = licenseeReport.setPostedData(fromCache);
        this.currencyId = data.Currency;
        $("#btnBackToMain").show();
        if (licenseeReport.licenseeId == 0) {
            showAlertMessage("ErrMsg", "Please choose a site.", 1);
            return;
        }
        $("#ErrMsg").html("");
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/LicenseeService.svc/SelReportLicenseeDetail_SAgV2",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                $("#gridtitle").html(licenseeReport.getTitle);
                $("#colReportBy").html("Username");
                var arr = [];
                var arrfirst = [];
                var arr_total = [];
                var arr_totalFirstcols = [];

                var items = rs.Items;

                if (items != null && items.length > 0) {

                    var numberitems = items.length;
                    for (var i = 0; i < numberitems; i++) {
                        arr.push(licenseeReport.renderSuperDetailRow(items[i], (i % 2 == 0 ? "bgeven" : ""), date));
                        arrfirst.push(licenseeReport.renderSuperDetailFirstCols(items[i], (i % 2 == 0 ? "bgeven" : ""), date));
                    }

                    var totalitems = rs.Totals;
                    var numbertotalitems = totalitems.length;
                    for (var j = 0; j < numbertotalitems; j++) {
                        if (j == 0) {
                            arr_total.push(licenseeReport.renderTotalRow(totalitems[j], "total", numbertotalitems));
                            arr_totalFirstcols.push(licenseeReport.renderTotalFirstCols(totalitems[j], "total", numbertotalitems));
                        }
                        else {
                            arr_total.push(licenseeReport.renderTotalRow(totalitems[j], "total", 0));
                            arr_totalFirstcols.push(licenseeReport.renderTotalFirstCols(totalitems[j], "total", 0));
                        }
                    }
                    //$("#tblwinlossmain").empty().html(arr.join(''));
                    var grandtotalitems = rs.TotalsBaseCurrency;
                    var numbergrandtotalitems = grandtotalitems.length;
                    for (var k = 0; k < numbergrandtotalitems; k++) {
                        if (k == 0) {
                            arr_total.push(licenseeReport.renderGrandTotalRow(grandtotalitems[k], "grandtotal", numbergrandtotalitems));
                            arr_totalFirstcols.push(licenseeReport.renderGrandTotalFirstCols(grandtotalitems[k], "grandtotal", numbergrandtotalitems));
                        }
                        else {
                            arr_total.push(licenseeReport.renderGrandTotalRow(grandtotalitems[k], "grandtotal", 0));
                            arr_totalFirstcols.push(licenseeReport.renderGrandTotalFirstCols(grandtotalitems[k], "grandtotal", 0));
                        }
                    }


                    mainreport.resetGrid("tbWinloss");
                    $("#tbWinloss tbody").empty().append(arr.join(''));
                    $("#tbwinlossrowheader tbody").empty().append(arrfirst.join(''));
                    mainreport.find(".row_footer_content thead").empty().append(arr_totalFirstcols.join(''));
                    mainreport.find(".col_footer_content thead").empty().append(arr_total.join(''));

                    _totalItem = rs.totalrow;
                    licenseeReport.initPagerSuper(_totalItem, _pageSize, _pageIndex - 1);
                }
                else {
                    $("#tbWinloss tbody").empty().html('<tr><td colspan="15"><div class="c">There is no information available.</div></td></tr>');
                    $("#tbwinlossrowheader tbody").empty().html('<tr><td colspan="4" style="background:none repeat scroll 0 0 #CDDEF3"><div style="width:330px">&nbsp;</div></td></tr>');
                    mainreport.find(".row_footer_content thead").empty()
                    mainreport.find(".col_footer_content thead").empty()
                    $("#pagination").html('&nbsp;');
                }

                var gridOpts = {
                    tblContentId: 'tbWinloss',
                    manualCopyHeader: true,
                    fullWidth: true,
                    fullHeight: true,
                    gridHeight: 160,
                    freezeWidth: 345,
                    numberFreezeCol: 4
                };

                mainreport.removeClass("hidden");
                mainreport.gridScroller(gridOpts);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    ViewMasterDetailReport: function (currencyId, fromCache, custId, date) {
        licenseeReport.ReportLevel = 2;
        this.isDetail = true;
        var mainreport = $("#winlossmain");
        $("#btnBackToMain").show();
        this.reportSelected = 2;
        licenseeReport.custId = custId;
        licenseeReport.currencyId = currencyId;
        licenseeReport.funcMasterDetail = function () {
            licenseeReport.ViewMasterDetailReport(currencyId, 1, custId, date);
        };
        if (parseInt(licenseeReport.reportType) == 1) {
            licenseeReport.fromDate = date;
            licenseeReport.toDate = date;
        }
        if (parseInt(licenseeReport.reportType) == 2) {

            var dt = date.split('/');
            var startD = $("#fdate").val().split('/');
            var endD = $("#tdate").val().split('/');

            if (parseInt(dt[0]) == parseInt(startD[0])) {
                licenseeReport.fromDate = dt[0] + "/" + startD[1] + "/" + dt[1];
            }
            else {
                licenseeReport.fromDate = dt[0] + "/01/" + dt[1];
            }

            if (parseInt(dt[0]) == parseInt(endD[0])) {
                licenseeReport.toDate = dt[0] + "/" + endD[1] + "/" + dt[1];
            }
            else {
                licenseeReport.toDate = dt[0] + "/" + daysInMonth(parseInt(dt[0]), parseInt(dt[1])) + "/" + dt[1];
            }

        }

        if (licenseeReport.licenseeId == 0) {
            showAlertMessage("ErrMsg", "Please choose a site.", 1);
            return;
        }
        var data = licenseeReport.setPostedData(fromCache);
        this.currencyId = data.Currency;
        $("#ErrMsg").html("");
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/LicenseeService.svc/SelReportLicenseeDetail_MAgV2",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                $("#gridtitle").html(licenseeReport.getTitle);
                $("#colReportBy").html("Username");

                var arr = [];
                var arrfirst = [];
                var arr_total = [];
                var arr_totalFirstcols = [];
                var items = rs.Items;

                if (items != null && items.length > 0) {

                    var numberitems = items.length;

                    for (var i = 0; i < numberitems; i++) {
                        arr.push(licenseeReport.renderMasterDetailRow(items[i], (i % 2 == 0 ? "bgeven" : ""), date));
                        arrfirst.push(licenseeReport.renderMasterDetailFirstCols(items[i], (i % 2 == 0 ? "bgeven" : ""), date));
                    }

                    var totalitems = rs.Totals;
                    var numbertotalitems = totalitems.length;
                    for (var j = 0; j < numbertotalitems; j++) {
                        if (j == 0) {
                            arr_total.push(licenseeReport.renderTotalRow(totalitems[j], "total", numbertotalitems));
                            arr_totalFirstcols.push(licenseeReport.renderTotalFirstCols(totalitems[j], "total", numbertotalitems));
                        }
                        else {
                            arr_total.push(licenseeReport.renderTotalRow(totalitems[j], "total", 0));
                            arr_totalFirstcols.push(licenseeReport.renderTotalFirstCols(totalitems[j], "total", 0));
                        }
                    }
                    //$("#tblwinlossmain").empty().html(arr.join(''));
                    var grandtotalitems = rs.TotalsBaseCurrency;
                    var numbergrandtotalitems = grandtotalitems.length;
                    for (var k = 0; k < numbergrandtotalitems; k++) {
                        if (k == 0) {
                            arr_total.push(licenseeReport.renderGrandTotalRow(grandtotalitems[k], "grandtotal", numbergrandtotalitems));
                            arr_totalFirstcols.push(licenseeReport.renderGrandTotalFirstCols(grandtotalitems[k], "grandtotal", numbergrandtotalitems));
                        }
                        else {
                            arr_total.push(licenseeReport.renderGrandTotalRow(grandtotalitems[k], "grandtotal", 0));
                            arr_totalFirstcols.push(licenseeReport.renderGrandTotalFirstCols(grandtotalitems[k], "grandtotal", 0));
                        }
                    }

                    mainreport.resetGrid("tbWinloss");
                    $("#tbWinloss tbody").empty().append(arr.join(''));
                    $("#tbwinlossrowheader tbody").empty().append(arrfirst.join(''));
                    mainreport.find(".row_footer_content thead").empty().append(arr_totalFirstcols.join(''));
                    mainreport.find(".col_footer_content thead").empty().append(arr_total.join(''));

                    _totalItem = rs.totalrow;
                    licenseeReport.initPagerMaster(_totalItem, _pageSize, _pageIndex - 1);
                }
                else {
                    $("#tbWinloss tbody").empty().html('<tr><td colspan="15"><div class="c">There is no information available.</div></td></tr>');
                    $("#tbwinlossrowheader tbody").empty().html('<tr><td colspan="4" style="background:none repeat scroll 0 0 #CDDEF3"><div style="width:330px">&nbsp;</div></td></tr>');
                    mainreport.find(".row_footer_content thead").empty()
                    mainreport.find(".col_footer_content thead").empty()
                    $("#pagination").html('&nbsp;');
                }

                var gridOpts = {
                    tblContentId: 'tbWinloss',
                    manualCopyHeader: true,
                    fullWidth: true,
                    fullHeight: true,
                    gridHeight: 160,
                    freezeWidth: 345,
                    numberFreezeCol: 4
                };
                mainreport.removeClass("hidden");
                mainreport.gridScroller(gridOpts);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    ViewDetail: function (currencyId, fromCache, custId, date) {
        licenseeReport.currencyId = currencyId;
        //licenseeReport.baseCurrency = baseCurrency;
        licenseeReport.SelectedDate = date;

        if (parseInt(licenseeReport.reportType) == 1) {
            licenseeReport.fromDate = date;
            licenseeReport.toDate = date;
        }
        //        switch (SportName) {
        //            case "Sportsbook": //Sport Book
        //                licenseeReport.sportID = "0";
        //                break;
        //            case "Racing": //Horse Racing
        //                licenseeReport.sportID = "151";
        //                break;
        //            case "Number Game": //NumberGame
        //                licenseeReport.sportID = "161";
        //                break;
        //            default:
        //                break;
        //        }
        if (this.level == "3") {
            licenseeReport.renderDetail12BetReport(currencyId, fromCache, custId, date);
        }
        else {
            licenseeReport.renderDetailReport(currencyId, fromCache, custId, date);
        }
    },
    renderDetailReport: function (currencyId, fromCache, custId, date) {
        licenseeReport.ReportLevel = 4;
        this.isDetail = true;
        var mainreport = $("#winlossmain");
        $("#btnBackToMain").show();
        this.reportSelected = 2;
        licenseeReport.custId = custId;
        licenseeReport.currencyId = currencyId;

        if (parseInt(licenseeReport.reportType) == 1) {
            licenseeReport.fromDate = date;
            licenseeReport.toDate = date;
        }
        if (parseInt(licenseeReport.reportType) == 2) {
            //mm/dd/yyyy


            var startD = $("#fdate").val().split('/');
            var endD = $("#tdate").val().split('/');
            var _y;
            var dt = date.split('/');
            if (dt.length == 2) {
                _y = dt[1];
            }
            if (dt.length == 3) {
                _y = dt[2];
            }
            else {
                _y = startD[2];
            }


            if (parseInt(dt[0]) == parseInt(startD[0])) {
                licenseeReport.fromDate = dt[0] + "/" + startD[1] + "/" + _y;
            }
            else {
                licenseeReport.fromDate = dt[0] + "/01/" + _y;
            }

            if (parseInt(dt[0]) == parseInt(endD[0])) {
                licenseeReport.toDate = dt[0] + "/" + endD[1] + "/" + _y;
            }
            else {
                licenseeReport.toDate = dt[0] + "/" + daysInMonth(parseInt(dt[0]), parseInt(_y)) + "/" + _y;
            }
        }
        if (licenseeReport.licenseeId == 0) {
            showAlertMessage("ErrMsg", "Please choose a site.", 1);
            return;
        }
        var data = licenseeReport.setPostedData(fromCache);
        this.currencyId = data.Currency;
        $("#ErrMsg").html("");
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/LicenseeService.svc/SelReportLicenseeDetailV2",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                $("#gridtitle").html(licenseeReport.getTitle);
                $("#colReportBy").html("Username");

                var arr = [];
                var arrfirst = [];
                var arr_total = [];
                var arr_totalFirstcols = [];
                var items = rs.Items;

                if (items != null && items.length > 0) {

                    var numberitems = items.length;

                    for (var i = 0; i < numberitems; i++) {
                        arr.push(licenseeReport.renderDetailRow(items[i], (i % 2 == 0 ? "bgeven" : ""), date));
                        arrfirst.push(licenseeReport.renderDetailFirstCols(items[i], (i % 2 == 0 ? "bgeven" : ""), date));
                    }

                    var totalitems = rs.Totals;
                    var numbertotalitems = totalitems.length;
                    for (var j = 0; j < numbertotalitems; j++) {
                        if (j == 0) {
                            arr_total.push(licenseeReport.renderTotalRow(totalitems[j], "total", numbertotalitems));
                            arr_totalFirstcols.push(licenseeReport.renderTotalFirstCols(totalitems[j], "total", numbertotalitems));
                        }
                        else {
                            arr_total.push(licenseeReport.renderTotalRow(totalitems[j], "total", 0));
                            arr_totalFirstcols.push(licenseeReport.renderTotalFirstCols(totalitems[j], "total", 0));
                        }
                    }
                    //$("#tblwinlossmain").empty().html(arr.join(''));
                    var grandtotalitems = rs.TotalsBaseCurrency;
                    var numbergrandtotalitems = grandtotalitems.length;
                    for (var k = 0; k < numbergrandtotalitems; k++) {
                        if (k == 0) {
                            arr_total.push(licenseeReport.renderGrandTotalRow(grandtotalitems[k], "grandtotal", numbergrandtotalitems));
                            arr_totalFirstcols.push(licenseeReport.renderGrandTotalFirstCols(grandtotalitems[k], "grandtotal", numbergrandtotalitems));
                        }
                        else {
                            arr_total.push(licenseeReport.renderGrandTotalRow(grandtotalitems[k], "grandtotal", 0));
                            arr_totalFirstcols.push(licenseeReport.renderGrandTotalFirstCols(grandtotalitems[k], "grandtotal", 0));
                        }
                    }

                    mainreport.resetGrid("tbWinloss");
                    $("#tbWinloss tbody").empty().append(arr.join(''));
                    $("#tbwinlossrowheader tbody").empty().append(arrfirst.join(''));
                    mainreport.find(".row_footer_content thead").empty().append(arr_totalFirstcols.join(''));
                    mainreport.find(".col_footer_content thead").empty().append(arr_total.join(''));

                    _totalItem = rs.totalrow;
                    licenseeReport.initPagerDetail(_totalItem, _pageSize, _pageIndex - 1);
                }
                else {
                    $("#tbWinloss tbody").empty().html('<tr><td colspan="15"><div class="c">There is no information available.</div></td></tr>');
                    $("#tbwinlossrowheader tbody").empty().html('<tr><td colspan="4" style="background:none repeat scroll 0 0 #CDDEF3"><div style="width:330px">&nbsp;</div></td></tr>');
                    mainreport.find(".row_footer_content thead").empty()
                    mainreport.find(".col_footer_content thead").empty()
                    $("#pagination").html('&nbsp;');
                }

                var gridOpts = {
                    tblContentId: 'tbWinloss',
                    manualCopyHeader: true,
                    fullWidth: true,
                    fullHeight: true,
                    gridHeight: 160,
                    freezeWidth: 345,
                    numberFreezeCol: 4
                };
                mainreport.removeClass("hidden");
                mainreport.gridScroller(gridOpts);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDetail12BetReport: function (currencyId, fromCache, custId, date) {
        licenseeReport.ReportLevel = 3;
        var mainreport = $("#winlossmain");
        this.reportSelected = 1;
        this.isDetail = true;
        this.currencyId = currencyId;
        this.custId = custId;
        licenseeReport.funcDetail12Bet = function () {
            licenseeReport.renderDetail12BetReport(currencyId, 1, custId, date);
        };
        if (parseInt(licenseeReport.reportType) == 1) {
            licenseeReport.fromDate = date;
            licenseeReport.toDate = date;
        }

        if (parseInt(licenseeReport.reportType) == 2) {

            var dt = date.split('/');
            var startD = $("#fdate").val().split('/');
            var endD = $("#tdate").val().split('/');

            if (parseInt(dt[0]) == parseInt(startD[0])) {
                licenseeReport.fromDate = dt[0] + "/" + startD[1] + "/" + dt[1];
            }
            else {
                licenseeReport.fromDate = dt[0] + "/01/" + dt[1];
            }

            if (parseInt(dt[0]) == parseInt(endD[0])) {
                licenseeReport.toDate = dt[0] + "/" + endD[1] + "/" + dt[1];

            }
            else {
                licenseeReport.toDate = dt[0] + "/" + daysInMonth(parseInt(dt[0]), parseInt(dt[1])) + "/" + dt[1];
            }

        }
        var data = licenseeReport.setPostedData(fromCache);
        this.currencyId = data.Currency;
        $("#btnBackToMain").show();
        if (licenseeReport.licenseeId == 0) {
            showAlertMessage("ErrMsg", "Please choose a site.", 1);
            return;
        }
        $("#ErrMsg").html("");
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/LicenseeService.svc/SelReportLicenseeDetail_AgV2",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                $("#gridtitle").html(licenseeReport.getTitle);
                $("#colReportBy").html("Username");
                var arr = [];
                var arrfirst = [];
                var arr_total = [];
                var arr_totalFirstcols = [];

                var items = rs.Items;

                if (items != null && items.length > 0) {

                    var numberitems = items.length;
                    for (var i = 0; i < numberitems; i++) {
                        arr.push(licenseeReport.renderDetail12BetRow(items[i], (i % 2 == 0 ? "bgeven" : ""), date));
                        arrfirst.push(licenseeReport.renderDetail12BetFristCols(items[i], (i % 2 == 0 ? "bgeven" : ""), date));
                    }

                    var totalitems = rs.Totals;
                    var numbertotalitems = totalitems.length;
                    for (var j = 0; j < numbertotalitems; j++) {
                        if (j == 0) {
                            arr_total.push(licenseeReport.renderTotalRow(totalitems[j], "total", numbertotalitems));
                            arr_totalFirstcols.push(licenseeReport.renderTotalFirstCols(totalitems[j], "total", numbertotalitems));
                        }
                        else {
                            arr_total.push(licenseeReport.renderTotalRow(totalitems[j], "total", 0));
                            arr_totalFirstcols.push(licenseeReport.renderTotalFirstCols(totalitems[j], "total", 0));
                        }
                    }
                    //$("#tblwinlossmain").empty().html(arr.join(''));
                    var grandtotalitems = rs.TotalsBaseCurrency;
                    var numbergrandtotalitems = grandtotalitems.length;
                    for (var k = 0; k < numbergrandtotalitems; k++) {
                        if (k == 0) {
                            arr_total.push(licenseeReport.renderGrandTotalRow(grandtotalitems[k], "grandtotal", numbergrandtotalitems));
                            arr_totalFirstcols.push(licenseeReport.renderGrandTotalFirstCols(grandtotalitems[k], "grandtotal", numbergrandtotalitems));
                        }
                        else {
                            arr_total.push(licenseeReport.renderGrandTotalRow(grandtotalitems[k], "grandtotal", 0));
                            arr_totalFirstcols.push(licenseeReport.renderGrandTotalFirstCols(grandtotalitems[k], "grandtotal", 0));
                        }
                    }


                    mainreport.resetGrid("tbWinloss");
                    $("#tbWinloss tbody").empty().append(arr.join(''));
                    $("#tbwinlossrowheader tbody").empty().append(arrfirst.join(''));
                    mainreport.find(".row_footer_content thead").empty().append(arr_totalFirstcols.join(''));
                    mainreport.find(".col_footer_content thead").empty().append(arr_total.join(''));

                    _totalItem = rs.totalrow;
                    licenseeReport.initPagerDetail12Bet(_totalItem, _pageSize, _pageIndex - 1);
                }
                else {
                    $("#tbWinloss tbody").empty().html('<tr><td colspan="15"><div class="c">There is no information available.</div></td></tr>');
                    $("#tbwinlossrowheader tbody").empty().html('<tr><td colspan="4" style="background:none repeat scroll 0 0 #CDDEF3"><div style="width:330px">&nbsp;</div></td></tr>');
                    mainreport.find(".row_footer_content thead").empty()
                    mainreport.find(".col_footer_content thead").empty()
                    $("#pagination").html('&nbsp;');
                }

                var gridOpts = {
                    tblContentId: 'tbWinloss',
                    manualCopyHeader: true,
                    fullWidth: true,
                    fullHeight: true,
                    gridHeight: 160,
                    freezeWidth: 345,
                    numberFreezeCol: 4,
                    marginRight: 4
                };

                mainreport.removeClass("hidden");
                mainreport.gridScroller(gridOpts);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    viewBetList: function (custId, username, baseCurrency, ShowBA, date) {
        var ShowSB = 1;
        var ShowRB = 1;
        var ShowNG = 1;
        var ShowVS = 0;
        var ShowCS = 0;
        var currencyOption = $('#dlCurrency').val();
        //var excludeTest = parseInt($("input[name=rdExcludeTesting]:checked").val());
        var site = licenseeReport.licenseeId;
        if ($("#chbSport").attr("checked"))
            ShowSB = 1;
        else
            ShowSB = 0;
        if ($("#chbRacing").attr("checked"))
            ShowRB = 1;
        else
            ShowRB = 0;
        if ($("#chbNG").attr("checked"))
            ShowNG = 1;
        else
            ShowNG = 0;

        if ($("#chbLiveCasino").attr("checked"))
            ShowLC = 1;
        else
            ShowLC = 0;

        if ($("#chbVS").attr("checked"))
            ShowVS = 1;

        if ($("#chbRNGCasino").attr("checked"))
            ShowCS = 1;

        if (parseInt(licenseeReport.reportType) == 1) {
            licenseeReport.fromDate = date;
            licenseeReport.toDate = date;

        }
        if (parseInt(licenseeReport.reportType) == 2) {
            var dt = date.split('/');
            var startD = $("#fdate").val().split('/');
            var endD = $("#tdate").val().split('/');

            var _y;
            var dt = date.split('/');
            if (dt.length == 2) {
                _y = dt[1];
            }
            if (dt.length == 3) {
                _y = dt[2];
            }
            else {
                _y = startD[2];
            }

            if (parseInt(dt[0]) == parseInt(startD[0])) {
                licenseeReport.fromDate = dt[0] + "/" + startD[1] + "/" + _y;
            }
            else {
                licenseeReport.fromDate = dt[0] + "/01/" + _y;
            }

            if (parseInt(dt[0]) == parseInt(endD[0])) {
                licenseeReport.toDate = dt[0] + "/" + endD[1] + "/" + _y;

            }
            else {
                licenseeReport.toDate = dt[0] + "/" + daysInMonth(parseInt(dt[0]), parseInt(_y)) + "/" + _y;
            }
        }

        if (parseInt(licenseeReport.reportType) == 3) {
            licenseeReport.fromDate = $("#fdate").val();
            licenseeReport.toDate = $("#tdate").val();
        }


        var baseCurrency = $('#dlCurrency').val();
        if (currencyOption == '-3') //base usd
            baseCurrency = 'usd';
        else if (currencyOption == '-2') //base rm
            baseCurrency = 'myr';
        else if (currencyOption == '-6') //base rm
            baseCurrency = 'eur';
        else
            baseCurrency = '';

        var url = '../../BetList/BetListDepositReportV2.aspx?custid=' + custId + '&username=' + username + '&fdate=' + licenseeReport.fromDate + '&tdate=' + licenseeReport.toDate + '&showSB=' + ShowSB + '&showRB=' + ShowRB + '&showNG=' + ShowNG + '&showLC=' + ShowLC
        + '&showVS=' + ShowVS + '&showCS=' + ShowCS
        + '&showBA=' + licenseeReport.ShowBA + '&drawFilter=' + licenseeReport.drawFilter + '&baseCurrency=' + baseCurrency + '&site=' + site;

        var NewWindow = window.open(url, 'BetList', ' scrollbars=yes, toolbar=no, menubar=no,location=no,status=no,resizable=yes');
        if (NewWindow) NewWindow.focus();
    },
    goBack: function () {
        ResetValue();
        if (licenseeReport.isDetail) {
            if (licenseeReport.ReportLevel == 2 && licenseeReport.funcSuperDetail() != null) {
                licenseeReport.funcSuperDetail();
            }
            else if (licenseeReport.ReportLevel == 3 && licenseeReport.funcMasterDetail() != null) {
                licenseeReport.funcMasterDetail();
            }
            else if (licenseeReport.ReportLevel == 4 && licenseeReport.funcDetail12Bet() != null) {
                licenseeReport.funcDetail12Bet();
            }

        }
        else {
            if (licenseeReport.funcMain != null) {
                licenseeReport.funcMain();
            }
        }

    },
    exportExcel: function () {
        try { parent.BlockUI(); } catch (e) { }
        var reportTitle = this.getTitle();
        var getData = this.setPostedData(true);
        var baseCurrency;
        switch ($('#dlCurrency').val()) {
            case "0": //default
                baseCurrency = 0;
                break;
            case "-3":
                baseCurrency = 3; //USD
                break;
            case "-2":
                baseCurrency = 2; //RM
                break;
            case "-6":
                baseCurrency = 6; //EUR
                break;
            default:
                baseCurrency = 0;
                break;
        }
        var param = {
            IsExport: 1,
            reportTitle: reportTitle,
            licenseeId: getData.LicenseeID,
            currency: getData.Currency,
            fromDate: getData.FromDate,
            toDate: getData.ToDate,
            showSb: getData.showSB,
            showRb: getData.ShowRB,
            showNg: getData.ShowNG,
            showLc: getData.ShowLC,
            showBa: getData.ShowBA,
            showVS: getData.ShowVS,
            showCS: getData.ShowCS,
            baseCurrency: baseCurrency,
            reportType: getData.ReportType,
            drawFilter: getData.drawfilter,
            excludeTest: getData.excludeTest,
            pageIndex: getData.pageIndex,
            pageSize: getData.pageSize,
            fromCache: getData.fromCache,
            reportSelected: this.reportSelected,
            custid: this.custId,
            ReportLevel: licenseeReport.ReportLevel
        };

        var url = 'WLByLicensee_new.aspx?' + $.param(param);

        $('#frm').attr('src', url);
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },
    renderCalendarFromTo: function (ids) {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "fdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
                licenseeReport.changeDateOption();
            }
        };
        var dates = $(ids).datepicker(defaults);
        var fd = new Date(_fdate);
        var td = new Date(_tdate);
        var fdate = $(ids.split(',')[0]);
        var tdate = $(ids.split(',')[1]);
        fdate.datepicker("setDate", fd);
        tdate.datepicker("setDate", td);
    },
    changeDateOption: function () {
        if ($('#txtfromdate').val() != licenseeReport.defaultDate | $('#txttodate').val() != licenseeReport.defaultDate) {
            $("#dlDate").attr('selectedIndex', 7);
        }
    }

};

function daysInMonth(iMonth, iYear) {
    return 32 - new Date(iYear, iMonth - 1, 32).getDate();
}
function change(obj) {
    var checked = $(obj).attr("checked");
    $(".chksport").attr("checked", checked);
}

function changeSport() {
    var checked = true;
    $(".chksport").each(function () {
        if (!$(this).attr("checked")) {
            checked = false;
            return;
        }
    });
    $("#chbAll").attr("checked", checked);
}

