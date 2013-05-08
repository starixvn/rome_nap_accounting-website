var _pageIndex = 1;
var _pageSize = 50;
var _totalItem = 0;
function getLicensees() {
    var param = {
        IsAll: true
    };
    $.ajax({
        type: "POST",
        url: "../Service/LicenseeService.svc/SelLicensees",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(param),
        success: function (rs) {
            var dlSite = $('#dlSite');
            for (i = 0; i < rs.length; i++) {
                if(rs[i].EnableBA)
                dlSite.append("<option level='" + rs[i].Level + "' value='" + rs[i].Id + "'>" + rs[i].Name + "</option>")
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
    var data = {
        siteId: $('#dlSite').val()
    };
    if ($('#dlSite').val() == '0')
        return;
    $("#ErrMsg").html("");
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../Service/LicenseeService.svc/SelCurrency",
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
            url: "../Service/LicenseeService.svc/GetDatetimeOption",
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
    obj('dlDate').selectedIndex = 0;
    obj('dlSport').selectedIndex = 0;
    obj('dlCurrency').selectedIndex = 0;
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
    funcMain: null,
    funcDetail12Bet: null,
    level: '',
    isDetail: false,
    fromDate: '',
    toDate: '',
    sportID: '',
    licenseeId: 0,
    currencyId: 0,
    reportType: 0,
    drawFilter: 0,
    excludeTest:0,
    baseCurrency: 0,
    custId: 0,
    reportSelected: 0,
    getTitle: function () {
        return "BA Win Loss Report for " + $('#dlSite option:selected').text() + " from " + licenseeReport.fromDate + " to " + licenseeReport.toDate
    },
    renderMainRow: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="col100 l">' + row.ReportTypeId + '</div></td>\
                 <td><div class="col100 c link" onclick="ResetValue();licenseeReport.viewDetail(' + row.CurrencyId + ', ' + licenseeReport.baseCurrency + ',\'' + row.ReportTypeId + '\')">' + row.CurrencyName + '</div></td>\
                <td><div class="col100 r">' + row.Turnover + '</div></td>\
                <td><div class="col100 r">' + row.BetCount + '</div></td>\
                <td><div class="col100 r">' + row.CustomerWL + '</div></td>\
                <td><div class="col100 r">' + row.LicenseeWL + '</div></td>\
                <td><div class="col100 r">' + row.BA + '</div></td>\
                <td><div class="col100 r">' + row.RB + '</div></td>\
                <td><div class="col100 r">' + row.Total + '</div></td>\
                <td><div class="col100 r">' + row.AmtDuetoBA + '</div></td>\
                <td><div class="col100 r">' + row.AmtDuetoRB + '</div></td>\
            </tr>';
        return html;
    },
    renderDetailRow: function (row, cssClass, date) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
               <td><div class="col100 l link" onclick="licenseeReport.viewBetList(' + row.CustId + ',\'' + (row.Username != null ? row.Username : "") + '\',' + licenseeReport.baseCurrency + ', \'' + date + '\')">' + (row.Username != null ? row.Username : "") + '</div></td>\
                <td><div class="col100 c">' + row.CurrencyName + '</div></td>\
                <td><div class="col100 r">' + row.Turnover + '</div></td>\
                <td><div class="col100 r">' + row.BetCount + '</div></td>\
                <td><div class="col100 r">' + row.CustomerWL + '</div></td>\
                <td><div class="col100 r">' + row.LicenseeWL + '</div></td>\
                <td><div class="col100 r">' + row.BA + '</div></td>\
                <td><div class="col100 r">' + row.RB + '</div></td>\
                <td><div class="col100 r">' + row.Total + '</div></td>\
                <td><div class="col100 r">' + row.AmtDuetoBA + '</div></td>\
                <td><div class="col100 r">' + row.AmtDuetoRB + '</div></td>\
            </tr>';
        return html;
    },
    renderDetail12BetRow: function (row, cssClass, date) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="col100 l link" onclick="ResetValue();licenseeReport.renderDetailReport(' + row.CurrencyId + ',0,' + row.CustId + ',\'' + date + '\')">' + (row.Username != null ? row.Username : "") + '</div></td>\
                <td><div class="col100 c">' + row.CurrencyName + '</div></td>\
                <td><div class="col100 r">' + row.Turnover + '</div></td>\
                <td><div class="col100 r">' + row.BetCount + '</div></td>\
                <td><div class="col100 r">' + row.CustomerWL + '</div></td>\
                <td><div class="col100 r">' + row.LicenseeWL + '</div></td>\
                <td><div class="col100 r">' + row.BA + '</div></td>\
                <td><div class="col100 r">' + row.RB + '</div></td>\
                <td><div class="col100 r">' + row.Total + '</div></td>\
                <td><div class="col100 r">' + row.AmtDuetoBA + '</div></td>\
                <td><div class="col100 r">' + row.AmtDuetoRB + '</div></td>\
            </tr>';
        return html;
    },
    renderTotalRow: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                    <td colspan="2"><div class="c b" style="width:203px;" >' + row.CurrencyName + '</div></td>\
                    <td><div class="col100 r b">' + row.Turnover + '</div></td>\
                    <td><div class="col100 r b">' + (row.BetCount ? row.BetCount : '') + '</div></td>\
                    <td><div class="col100 r b">' + row.CustomerWL + '</div></td>\
                    <td><div class="col100 r b">' + row.LicenseeWL + '</div></td>\
                    <td><div class="col100 r b">' + row.BA + '</div></td>\
                    <td><div class="col100 r b">' + row.RB + '</div></td>\
                    <td><div class="col100 r b">' + row.Total + '</div></td>\
                    <td><div class="col100 r b">' + row.AmtDuetoBA + '</div></td>\
                    <td><div class="col100 r b">' + row.AmtDuetoRB + '</div></td>\
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

        licenseeReport.renderDetailReport(licenseeReport.currencyId, 1);
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

        licenseeReport.renderDetail12BetReport(licenseeReport.currencyId, 1);
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
            showSB: 1,
            ShowRB: 0,
            ShowNG: 1,
            ShowBA: 1,
            ReportType: licenseeReport.reportType,
            drawfilter: licenseeReport.drawFilter,
            excludeTest: licenseeReport.excludeTest,
            pageIndex: _pageIndex,
            pageSize: _pageSize,
            fromCache: fromCache
        };
        switch (licenseeReport.sportID) {
            case "0": //SportBook
                data.showSB = 1;
               // data.ShowRB = 0;
                data.ShowNG = 0;
                break;
//            case "151": //RaceBook
//                data.showSB = 0;
//                data.ShowNG = 1;
//                data.ShowRB = 0;
//                break;
            case "161": //NumberGame
                data.showSB = 0;
                //data.ShowRB = 0;
                data.ShowNG = 1;
                break;
            default:
                break;
        }
        return data;
    },
    renderMainReport: function (fromCache) {
        this.level = $("#dlSite").find("option:selected").attr("level");
        this.isDetail = false;
        this.funcMain = function () {
            this.renderMainReport(1);
        }
        $("#btnBackToMain").hide();
        this.reportSelected = 0;
        licenseeReport.fromDate = $("#fdate").val(),
        licenseeReport.toDate = $("#tdate").val(),
        licenseeReport.licenseeId = parseInt($("#dlSite").val());
        licenseeReport.currencyId = parseInt($("#dlCurrency").val());
        licenseeReport.baseCurrency = parseInt($('#dlCurrency').val()) < 0 ? parseInt($('#dlCurrency').val()) * (-1) : 0;
        licenseeReport.drawFilter = $('#ddltype').val();
 
        licenseeReport.reportType = parseInt($("input[name=RdReportBy]:checked").val());
        licenseeReport.excludeTest = parseInt($("input[name=rdExcludeTesting]:checked").val());
        licenseeReport.sportID = $('#dlSport').val();
        if (licenseeReport.licenseeId == 0) {
            showAlertMessage("ErrMsg", "Please choose a Licensee.", 1);
            return;
        }
        $("#ErrMsg").html("");
        var data = licenseeReport.setPostedData(fromCache);
        licenseeReport.currencyId = data.Currency;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "../Service/LicenseeService.svc/SelReportByLicenseeV2",
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
                var winlossmain = $("#winlossmain");
                var arr = [];
                var arr_total = [];
                var items = rs.Items;
                if (items != null && items.length > 0) {

                    var numberitems = items.length;

                    for (var i = 0; i < numberitems; i++) {
                        arr.push(licenseeReport.renderMainRow(items[i], (i % 2 == 0 ? "bgeven" : "")));
                    }

                    var totalitems = rs.Totals;
                    var numbertotalitems = totalitems.length;
                    for (var j = 0; j < numbertotalitems; j++) {
                        arr.push(licenseeReport.renderTotalRow(totalitems[j], "total"));
                    }
                    $("#tblwinlossmain").empty().html(arr.join(''));
                    var grandtotalitems = rs.TotalsBaseCurrency;
                    var numbergrandtotalitems = grandtotalitems.length;
                    var totalbet=0;
                    for (var k = 0; k < numbergrandtotalitems; k++) {
                        arr_total.push(licenseeReport.renderGrandTotalRow(k, grandtotalitems[k], "grandtotal"));
                    }
                    $("#tblFooter").empty().html(arr_total.join(''));
                    _totalItem = rs.totalrow;
                    licenseeReport.initPagerMain(_totalItem, _pageSize, _pageIndex - 1); winlossmain.removeClass("hidden").reRenderScroller(1030, 542, true); //100*10+10+10*2
                }
                else {
                    $("#tblwinlossmain").empty().html('<tr><td colspan="10"><div class="c" style="width:1027px">There is no information available.</div></td></tr>');
                    $("#tblFooter").empty();
                    $("#pagination").html('&nbsp;');
                }

                winlossmain.removeClass("hidden").reRenderScroller(1133, 541, true);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    viewDetail: function (currencyId, baseCurrency, date) {
       licenseeReport.currencyId = currencyId;
        licenseeReport.baseCurrency = baseCurrency;
        licenseeReport.selectedDate = date;
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
            licenseeReport.renderDetail12BetReport(currencyId, 0, baseCurrency, date);
        }
        else {
            licenseeReport.renderDetailReport(currencyId, 0, 0, date);
        }
    },
    renderDetailReport: function (currencyId, fromCache, custId, date) {
        this.isDetail = true;
        $("#btnBackToMain").show();
        this.reportSelected = 2;
        licenseeReport.custId = custId;
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

        if (licenseeReport.licenseeId == 0) {
            showAlertMessage("ErrMsg", "Please choose a site.", 1);
            return;
        }
        $("#ErrMsg").html("");
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "../Service/LicenseeService.svc/SelReportByLicenseeDetailV2",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                $("#gridtitle").html(licenseeReport.getTitle);
                $("#colReportBy").html("Username");
                var winlossmain = $("#winlossmain");
                var arr = [];
                var arr_total = [];
                var items = rs.Items;

                if (items != null && items.length > 0) {

                    var numberitems = items.length;

                    for (var i = 0; i < numberitems; i++) {
                        arr.push(licenseeReport.renderDetailRow(items[i], (i % 2 == 0 ? "bgeven" : ""), date));
                    }

                    var totalitems = rs.Totals;
                    var numbertotalitems = totalitems.length;
                    for (var j = 0; j < numbertotalitems; j++) {
                        arr.push(licenseeReport.renderTotalRow(totalitems[j], "total"));
                    }
                    $("#tblwinlossmain").empty().html(arr.join(''));
                    var grandtotalitems = rs.TotalsBaseCurrency;
                    var numbergrandtotalitems = grandtotalitems.length;
                    for (var k = 0; k < numbergrandtotalitems; k++) {
                        arr_total.push(licenseeReport.renderTotalRow(grandtotalitems[k], "grandtotal"));
                    }
                    $("#tblFooter").empty().html(arr_total.join(''));
                    _totalItem = rs.totalrow;
                    licenseeReport.initPagerDetail(_totalItem, _pageSize, _pageIndex - 1); winlossmain.removeClass("hidden").reRenderScroller(1030, 542, true); //100*10+10+10*2
                }
                else {
                    $("#tblwinlossmain").empty().html('<tr><td colspan="10"><div class="c" style="width:1027px">There is no information available.</div></td></tr>');
                    $("#tblFooter").empty();
                    $("#pagination").html('&nbsp;');
                }

                winlossmain.removeClass("hidden").reRenderScroller(1133, 541, true);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDetail12BetReport: function (currencyId, fromCache, baseCurrency, date) {
        this.reportSelected = 1;
        this.isDetail = false;
        this.funcDetail12Bet = function () {
            licenseeReport.renderDetail12BetReport(currencyId, 1, baseCurrency, date);
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
        $("#btnBackToMain").show();

        if (licenseeReport.licenseeId == 0) {
            showAlertMessage("ErrMsg", "Please choose a site.", 1);
            return;
        }
        $("#ErrMsg").html("");
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "../Service/LicenseeService.svc/SelReportByLicenseeDetail_AgV2",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                $("#gridtitle").html(licenseeReport.getTitle);
                $("#colReportBy").html("Username");
                var winlossmain = $("#winlossmain");
                var arr = [];
                var arr_total = [];
                var items = rs.Items;

                if (items != null && items.length > 0) {

                    var numberitems = items.length;

                    for (var i = 0; i < numberitems; i++) {
                        arr.push(licenseeReport.renderDetail12BetRow(items[i], (i % 2 == 0 ? "bgeven" : ""), date));
                    }

                    var totalitems = rs.Totals;
                    var numbertotalitems = totalitems.length;
                    for (var j = 0; j < numbertotalitems; j++) {
                        arr.push(licenseeReport.renderTotalRow(totalitems[j], "total"));
                    }
                    $("#tblwinlossmain").empty().html(arr.join(''));
                    var grandtotalitems = rs.TotalsBaseCurrency;
                    var numbergrandtotalitems = grandtotalitems.length;
                    for (var k = 0; k < numbergrandtotalitems; k++) {
                        arr_total.push(licenseeReport.renderTotalRow(grandtotalitems[k], "grandtotal"));
                    }
                    $("#tblFooter").empty().html(arr_total.join(''));
                    _totalItem = rs.totalrow;
                    licenseeReport.initPagerDetail12Bet(_totalItem, _pageSize, _pageIndex - 1); winlossmain.removeClass("hidden").reRenderScroller(1030, 542, true); //100*10+10+10*2
                }
                else {
                    $("#tblwinlossmain").empty().html('<tr><td colspan="10"><div class="c" style="width:1027px">There is no information available.</div></td></tr>');
                    $("#tblFooter").empty();
                    $("#pagination").html('&nbsp;');
                }

                winlossmain.removeClass("hidden").reRenderScroller(1133, 541, true);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    viewBetList: function (custId, username, baseCurrency, date) {
       var ShowSB = 1;
        var ShowRB = 1;
        var ShowNG = 1;
        var currencyOption = $('#dlCurrency').val();
        var site = licenseeReport.licenseeId;
        var excludeTesting = licenseeReport.excludeTest;

        //Get ShowSB, ShowHR
        switch (licenseeReport.sportID) {
            case "0": //Sport Book
                ShowSB = 1;
                ShowRB = 0;
                ShowNG = 0;
                break;
//            case "151": //Horse Racing
//                ShowSB = 0;
//                ShowRB = 1;
//                ShowNG = 0;
//                break;
            case "161": //Horse Racing
                ShowSB = 0;
                ShowRB = 0;
                ShowNG = 1;
                break;
            default:
                break;
        }

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
        
        var baseCurrency = $('#dlCurrency').val();
        if (currencyOption == '-3') //base usd
            baseCurrency = 'usd';
        else if (currencyOption == '-2') //base rm
            baseCurrency = 'myr';
        else if (currencyOption == '-6') //base rm
            baseCurrency = 'eur';
        else
            baseCurrency = 'none';

        var url = '../../BA/BetList/BABetListDepositReportV2.aspx?custid=' + custId + '&username=' + username + '&fdate=' + licenseeReport.fromDate + '&tdate=' + licenseeReport.toDate+ '&showSB=' + ShowSB + '&showNG=' + ShowNG + '&ShowBA=' + 1 + '&drawFilter=' + licenseeReport.drawFilter + '&baseCurrency=' + baseCurrency + '&site=' + site;

        var NewWindow = window.open(url, 'BetList', ' scrollbars=yes, toolbar=no, menubar=no,location=no,status=no,resizable=yes');
        if (NewWindow) NewWindow.focus();
    },
    goBack: function () {
        ResetValue();
        if (licenseeReport.level == "3" && licenseeReport.isDetail) {
            if (licenseeReport.funcDetail12Bet != null) {
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
        var url = 'WLByLicenseeV2.aspx?IsExport=1&reportTitle=' + reportTitle + '&licenseeId=' + getData.LicenseeID + '&currency=' +
            getData.Currency + '&fromDate=' + getData.FromDate + '&toDate=' + getData.ToDate + '&showSb=' + getData.showSB +
            '&showRb=' + getData.ShowRB + '&reportType=' + getData.ReportType + '&drawFilter=' + getData.drawfilter + '&excludeTest='+ getData.excludeTest +  '&pageIndex=' +
            getData.pageIndex + '&pageSize=' + getData.pageSize + '&reportSelected=' + this.reportSelected + '&custid=' + this.custId;

        $('#frm').attr('src', url);
        try { parent.jQuery.unblockUI(); } catch (e) { }
    }

};
 
$(window).load(function () {
     try { parent.jQuery.unblockUI(); } catch (e) { }  
});