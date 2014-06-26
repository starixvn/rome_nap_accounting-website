
$(document).ready(function () {
    reportManager.initPage();
});

ReportManager = function () {
    var self = this;

    FormatConstant = {
        INT_NUMBER: '0,0',
        TWO_DECIMAL_NUMBER: '0,0.00'
    };
    var _tables = {
        btnGenerate: null,
        datepickerplugin: null,
        btnFromDate: null,
        btnToDate: null,
        ddwLicensee: null,
        ddwCurrency: null,
        ddwGameType: null,
        ddwGameTable: null,
        errorMessage: null,

        reportContent: null,
        reportContent_Container: null,
        reportTitle: null,
        reportExportExcel: null,
        reportRowHeader: null,
        reportRowFooter: null,
        reportColFooter: null,
        reportTopLeft: null,
        colGameLevy: null,
        exportFrame: null
    };
    var _params = {
        getGameTables: {
            GameTypeID: null
        },

        reportParams: {
            licensee: '',
            LicenseeName: '',
            datestart: '',
            dateend: '',
            currency: '',
            CurrencyName: '',
            gametype: '',
            GameTypeName: '',
            gametable: '',
            GameTableName: ''
        }
    };

    var _options = {
        specificPeriodIndex: 7,
        report: {
            isShowPager: true,
            tblContentId: 'financialReport_Content',
            manualCopyHeader: true,
            fullHeight: true,
            fullWidth: true,
            gridWidth: 1150,
            columns: [120, 80, 80, 100],
            gridHeight: 280,
            freezeWidth: 290,
            freezeHeight: 150,
            numberFreezeCol: 3
        }
    };

    self.TempHtml = {
        frozenHtml: '',
        contentHtml: '',
        footerFrozenHtml: '',
        footerContentHtml: ''
    };
    var _cachedData = {
        fromDate: null,
        toDate: null
    };

    self.initPage = function () {
        renderDatePicker();
        getLicensees();
        getCurrencies();
        getGameTypes();
        initGridScrollerTables();
        cachedDOMElements();
        bindingEvent();
    };
    var renderDatePicker = function () {
        var lastdate = '';
        lastdate = lastMonth + '/' + lastDay + '/' + lastYear;
        $('#spMonth').monPickr({
            today: lastdate,
            maxDate: _today,
            minDate: _minDate,
            showDateRange: true,
            fromDayId: 'spFrom',
            toDayId: 'spTo',
            log: function (msg) {
                console.warn(msg);
            }
        });

        _tables.datepickerplugin = $('#spMonth');
        _tables.btnFromDate = $('#spFrom');
        _tables.btnToDate = $('#spTo');
    };
    var cachedDOMElements = function () {
        _tables.btnGenerate = $("#btnReport");
        _tables.errorMessage = $("#ErrorMessage");

        _tables.report = $("#financialReport");
        _tables.reportTopLeft = _tables.report.find(".free-top-left");
        _tables.reportTopLeftHeader = _tables.report.find("#financialReport_RowHeader");
        _tables.reportContent_Container = _tables.report.find("#financialReport_Content");
        _tables.reportContent = _tables.report.find("#financialReport_Content tbody");
        _tables.reportTitle = _tables.report.find("#financialReport_Title");
        _tables.reportExportExcel = _tables.report.find("#financialReport_ExportExcel");
        _tables.reportRowHeader = _tables.reportTopLeftHeader.find("tbody");
        _tables.reportRowFooter = _tables.report.find(".row_footer_content thead");
        _tables.reportColFooter = _tables.report.find(".col_footer_content thead");
        _tables.colGameLevy = _tables.report.find(".colGameLevy");
        _tables.exportFrame = $('#frm');
    };
    var bindingEvent = function () {
        _tables.btnGenerate.click(function () {
            self.getReport();
        });

        _tables.reportExportExcel.click(function () {
            self.exportReport();
        });
    };
    var getLicensees = function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "LiveCasinoGameFinancialReport/GetLicensees",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                renderDdwLicensee(rs);
            },
            error: function () {
                renderDdwLicensee([]);
            },
            complete: function () {
            }
        });
    };
    var renderDdwLicensee = function (result) {
        var id = "ddwLicensee";
        var length = result.length;
        var licensees = [];
        for (var i = 0; i < length; i++) {
            var info = result[i];
            licensees.push({
                Id: id + info.Id.toString(),
                Value: info.Id,
                Text: info.AliasName,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }

        var noItemText = '';
        if (licensees.length == 0) {
            noItemText = 'No licensee found.';
        }

        $("#ddwLicensee").multiSelect(
        {
            minWidth: 170,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All Licensees',
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All Licensees',
            noItemText: noItemText,
            listData: licensees
        });

        _tables.ddwLicensee = $("#ddwLicensee");
    };
    var getCurrencies = function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "LiveCasinoGameFinancialReport/GetCurrencies",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                renderDdwCurrency(rs);
            },
            error: function () {
                renderDdwCurrency([]);
            },
            complete: function () {
            }
        });
    };
    var renderDdwCurrency = function (result) {
        var id = "ddwCurrency";
        var length = result.length;
        var currencies = [];
        for (var i = 0; i < length; i++) {
            var info = result[i];
            currencies.push({
                Id: id + info.Id.toString(),
                Value: info.Id,
                Text: info.Name,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }

        var noItemText = '';
        if (currencies.length == 0) {
            noItemText = 'No currency found.';
        }

        $("#ddwCurrency").multiSelect(
        {
            minWidth: 170,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All Currencies',
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All Currencies',
            noItemText: noItemText,
            listData: currencies
        });

        _tables.ddwCurrency = $("#ddwCurrency");
    };
    var getGameTypes = function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "LiveCasinoGameFinancialReport/GetGameTypes",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                renderDdwGameType(rs);
            },
            error: function () {
                renderDdwGameType([]);
            },
            complete: function () {
                getGameTables();
            }
        });
    };
    var renderDdwGameType = function (result) {
        var id = "ddwGameType";
        var length = result.length;
        var gameTypes = [];
        for (var i = 0; i < length; i++) {
            var info = result[i];
            gameTypes.push({
                Id: id + info.Id.toString(),
                Value: info.Id,
                Text: info.Name,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }

        var noItemText = '';
        if (gameTypes.length == 0) {
            noItemText = 'No game type found.';
        }

        $("#ddwGameType").multiSelect(
        {
            minWidth: 150,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All Game Types',
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All Game Types',
            noItemText: noItemText,
            listData: gameTypes
        });

        _tables.ddwGameType = $("#ddwGameType");
    };
    var getGameTables = function () {
        _params.getGameTables.GameTypeID = _tables.ddwGameType != null ?
                                    _tables.ddwGameType.multiSelect('getCheckedItems').join(",") :
                                    "";

        $.ajax({
            type: "POST",
            url: window.rootUrl + "LiveCasinoGameFinancialReport/GetGameTables",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(_params.getGameTables),
            success: function (rs) {
                renderDdwGameTable(rs);
            },
            error: function () {
                renderDdwGameTable([]);
            },
            complete: function () {
            }
        });
    };
    var renderDdwGameTable = function (result) {
        var id = "ddwGameTable";
        var length = result.length;
        var gameTables = [];
        for (var i = 0; i < length; i++) {
            var info = result[i];
            gameTables.push({
                Id: id + info.Id.toString(),
                Value: info.Id,
                Text: info.Name,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }

        var noItemText = '';
        if (gameTables.length == 0) {
            noItemText = 'No game table found.';
        }

        $("#ddwGameTable").multiSelect(
        {
            minWidth: 170,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All Game Tables',
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All Game Tables',
            noItemText: noItemText,
            listData: gameTables
        });

        _tables.ddwGameTable = $("#ddwGameTable");
    };
    self.getReport = function () {
        try { parent.BlockUI(); } catch (e) { }
        getParams();
        if (validateParams() == true) {
            _tables.errorMessage.empty().addClass("hidden");
            $.ajax({
                type: "POST",
                url: window.rootUrl + "LiveCasinoGameFinancialReport/GetFinancialReport",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(_params.reportParams),
                beforeSend: function () {
                },
                success: function (rs) {
                    self.renderReport(rs);
                },
                error: function (event, jqXHR, ajaxSettings) {
                    self.renderReport(null);
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
        else {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    };
    var getParams = function () {
        _params.reportParams.licensee = _tables.ddwLicensee.multiSelect('getCheckedItems').join(",");
        _params.reportParams.LicenseeName = _tables.ddwLicensee.multiSelect('getCheckedItemsByText').join(",");
        _params.reportParams.currency = _tables.ddwCurrency.multiSelect('getCheckedItems').join(",");
        _params.reportParams.CurrencyName = _tables.ddwCurrency.multiSelect('getCheckedItemsByText').join(",");
        _params.reportParams.gametype = _tables.ddwGameType.multiSelect('getCheckedItems').join(",");
        _params.reportParams.GameTypeName = _tables.ddwGameType.multiSelect('getCheckedItemsByText').join(",");
        _params.reportParams.gametable = _tables.ddwGameTable.multiSelect('getCheckedItems').join(",");
        _params.reportParams.GameTableName = _tables.ddwGameTable.multiSelect('getCheckedItemsByText').join(",");

        _params.reportParams.datestart = _tables.datepickerplugin.monPickr("getFromDate", "MM/dd/yyyy");
        _params.reportParams.dateend = _tables.datepickerplugin.monPickr("getToDate", "MM/dd/yyyy");
    };
    var validateParams = function () {
        if (_params.reportParams.licensee.length == 0) {
            showAlertMessage("ErrorMessage", "Please select at least one licensee!", 1);
            _tables.errorMessage.removeClass("hidden");
            _tables.errorMessage.focus();
            return false;
        }

        if (_params.reportParams.currency.length == 0) {
            showAlertMessage("ErrorMessage", "Please select at least one currency!", 1);
            _tables.errorMessage.removeClass("hidden");
            _tables.errorMessage.focus();
            return false;
        }


        if (_params.reportParams.gametype.length == 0) {
            showAlertMessage("ErrorMessage", "Please select at least one game type!", 1);
            _tables.errorMessage.removeClass("hidden");
            _tables.errorMessage.focus();
            return false;
        }


        if (_params.reportParams.gametable.length == 0) {
            showAlertMessage("ErrorMessage", "Please select at least one game table!", 1);
            _tables.errorMessage.removeClass("hidden");
            _tables.errorMessage.focus();
            return false;
        }

        return true;
    };
    self.renderReport = function (report) {
        if (report && report.ChildInfo.length > 0) {
            _tables.reportExportExcel.removeClass("hidden");
            renderReport_FinancialReport(report);
        }
        else {
            renderReport_NoData();
        }

        _tables.report.removeClass("hidden")
                      .gridScroller(_options.report);
        make2TableHeaderSameHeight();
    };
    var renderReport_NoData = function () {
        _tables.reportExportExcel.addClass("hidden");
        _tables.colGameLevy.html("Game Levy");
        var frezeeContent = "<td colspan='3'><div class=''>&nbsp</div></td>";
        _tables.reportRowHeader.empty().append("<tr>" + frezeeContent + "</tr>");
        _tables.reportContent.empty().append(
                "<tr>" +
                    frezeeContent +
                    "<td colspan='8' class='b c'>There is no data.</td>\
                 </tr>");
        _tables.reportRowFooter.empty();
        _tables.reportColFooter.empty();
    };
    var renderReport_FinancialReport = function (report) {
        self.resetTempHtml();
        _tables.colGameLevy.html("Game Levy (" + parseInt(report.GameLevy * 100, 0) + "%)");

        var length = report.ChildInfo.length;
        var frozenHtml = '';
        var info = null;
        for (var i = 0; i < length; i++) {
            info = report.ChildInfo[i];
            frozenHtml = '<td><div class="col120 c">' + info.Name + '</div></td>';
            frozenHtml += '<td><div class="col80 r">' + numeral(info.PlayerCount).format(FormatConstant.INT_NUMBER) + '</div></td>';
            frozenHtml += '<td><div class="col80 r">' + numeral(info.GameCount).format(FormatConstant.INT_NUMBER) + '</div></td>';

            self.TempHtml.frozenHtml += '<tr class="">' + frozenHtml + ' </tr>';
            self.TempHtml.contentHtml += '<tr class="">' +
                                                        frozenHtml +
                                                        '<td><div class="col100 r">' + formatNumberColor(info.TotalBets) + '</div></td>' +
                                                        '<td><div class="col100 r">' + formatNumberColor(info.TotalWins) + '</div></td>' +
                                                        '<td><div class="col100 r">' + formatNumberColor(info.GrossIncome) + '</div></td>' +
                                                        '<td><div class="col100 r">' + formatNumberColor(info.Commission) + '</div></td>' +
                                                        '<td><div class="col100 r">' + formatNumberColor(info.NetIncome) + '</div></td>' +
                                                        '<td><div class="col100 r">' + formatNumberColor(info.GamePayout * 100) + '</div></td>' +
                                                        '<td><div class="col100 r">' + formatNumberColor(info.GameLevy) + '</div></td>' +
                                                        '<td><div class="col120 r">' + formatNumberColor(info.GameLevyToPay) + '</div></td>' +
                                                        '</tr>';
        }

        info = report.GrandTotal;
        frozenHtml = '<th><div class="col120 c">' + info.Name + '</div></td>';
        frozenHtml += '<th><div class="col80 r">' + numeral(info.PlayerCount).format(FormatConstant.INT_NUMBER) + '</div></th>';
        frozenHtml += '<th><div class="col80 r">' + numeral(info.GameCount).format(FormatConstant.INT_NUMBER) + '</div></th>';

        self.TempHtml.footerFrozenHtml += '<tr class="">' + frozenHtml + ' </tr>';
        self.TempHtml.footerContentHtml += '<tr class="">' +
                                                        frozenHtml +
                                                        '<th><div class="col100 r">' + formatNumberColor(info.TotalBets) + '</div></th>' +
                                                        '<th><div class="col100 r">' + formatNumberColor(info.TotalWins) + '</div></th>' +
                                                        '<th><div class="col100 r">' + formatNumberColor(info.GrossIncome) + '</div></th>' +
                                                        '<th><div class="col100 r">' + formatNumberColor(info.Commission) + '</div></th>' +
                                                        '<th><div class="col100 r">' + formatNumberColor(info.NetIncome) + '</div></th>' +
                                                        '<th><div class="col100 r">' + formatNumberColor(info.GamePayout * 100) + '</div></th>' +
                                                        '<th><div class="col100 r">' + formatNumberColor(info.GameLevy) + '</div></th>' +
                                                        '<th><div class="col120 r">' + formatNumberColor(info.GameLevyToPay) + '</div></th>' +
                                                        '</tr>';

        self.removeSpaceIE9TempHtml();
        _tables.reportRowHeader.empty().append(self.TempHtml.frozenHtml);
        _tables.reportContent.empty().append(self.TempHtml.contentHtml);
        _tables.reportRowFooter.empty().append(self.TempHtml.footerFrozenHtml);
        _tables.reportColFooter.empty().append(self.TempHtml.footerContentHtml);
    };
    var initGridScrollerTables = function () {
        $("#financialReport").initGrid({
            isShowPager: false
        });
    };
    self.resetTempHtml = function () {
        self.TempHtml.frozenHtml = '';
        self.TempHtml.contentHtml = '';
        self.TempHtml.footerFrozenHtml = '';
        self.TempHtml.footerContentHtml = '';
    };
    self.removeSpaceIE9TempHtml = function () {
        self.TempHtml.frozenHtml = stringformat.removeSpaceIE9(self.TempHtml.frozenHtml);
        self.TempHtml.contentHtml = stringformat.removeSpaceIE9(self.TempHtml.contentHtml);
        self.TempHtml.footerFrozenHtml = stringformat.removeSpaceIE9(self.TempHtml.footerFrozenHtml);
        self.TempHtml.footerContentHtml = stringformat.removeSpaceIE9(self.TempHtml.footerContentHtml);
    };
    var formatNumberColor = function (value, isPercent) {
        var html = '';
        if (value < 0) {
            html = '<div class="color-red">' + numeral(value).format(FormatConstant.TWO_DECIMAL_NUMBER) + '</div>';
        }
        else {
            html = numeral(value).format(FormatConstant.TWO_DECIMAL_NUMBER);
        }

        return html;
    };
    var make2TableHeaderSameHeight = function () {
        var version = parseInt($.browser.version);
        if ($.browser.msie && version > 8) {
            _tables.reportTopLeft.find(".row-span-2").css("height", "32px");
            _tables.reportTopLeftHeader.find(".row-span-2").css("height", "32px");
        }
        else if ($.browser.msie) {
            _tables.reportTopLeft.find(".row-span-2").css("height", "31px");
            _tables.reportTopLeftHeader.find(".row-span-2").css("height", "31px");
        }
        else {
            var height = _tables.reportContent_Container.find("th:first").height();
            _tables.reportTopLeft.find(".row-span-2").css("height", height + "px");
            _tables.reportTopLeftHeader.find(".row-span-2").css("height", height + "px");
        }
    };
    self.exportReport = function () {
        var url = window.rootUrl + "LiveCasinoGameFinancialReport/ExportFinancialReport?" + $.param(_params.reportParams);
        _tables.exportFrame.attr('src', url);
    }
};

var reportManager = new ReportManager();