$(document).ready(function () {
    reportManager.InitPage();
});

ReportManager = function () {
    var self = this;

    FormatConstant = {
        INT_NUMBER: '0,0',
        TWO_DECIMAL_NUMBER: '0,0.00'
    };
    self.Tables = {
        btnGenerate: null,
        errorMessage: null,
        txtFromDate: null,
        txtTodate: null,
        datePickers: null
    };
    self.Params = {
        reportParams: {
            Fromdate: null,
            Todate: null
        }
    };

    self.Options = {
        specificPeriodIndex: 7,
        report: {
            isShowPager: true,
            tblContentId: 'numberGameMonthlyWinLossReport_Content',
            manualCopyHeader: true,
            fullHeight: true,
            fullWidth: true,
            //            gridWidth: 710,
            columns: [100, 120, 100, 80, 100, 100, 80],
            gridHeight: 280,
            freezeWidth: 100,
            freezeHeight: 150,
            numberFreezeCol: 1
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

    self.InitPage = function () {
        self.RenderDatePicker();
        self.InitGridScrollerTables();
        self.CachedDOMElements();
        self.BindingEvent();
    };

    self.RenderDatePicker = function () {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "txtFromDate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                self.Tables.datePickers.not(this).datepicker("option", option, date);
            }
        };

        $("#txtFromDate").datepicker(defaults);
        $("#txtTodate").datepicker(defaults);
        self.Tables.datePickers = $("#txtFromDate,#txtTodate");
        self.Tables.txtFromDate = $("#txtFromDate");
        self.Tables.txtTodate = $("#txtTodate");

        self.Tables.txtFromDate.datepicker("option", "maxDate", today);
        self.Tables.txtTodate.datepicker("option", "maxDate", today);
        self.Tables.txtFromDate.datepicker("option", "minDate", minDate);
        self.Tables.txtTodate.datepicker("option", "minDate", minDate);

        self.Tables.txtFromDate.datepicker("setDate", fromDate);
        self.Tables.txtTodate.datepicker("setDate", toDate);
    };
    self.InitGridScrollerTables = function () {
        $("#numberGameMonthlyWinLossReport").initGrid({
            isShowPager: false
        });
    };
    self.CachedDOMElements = function () {
        self.Tables.btnGenerate = $("#btnReport");
        self.Tables.errorMessage = $("#ErrorMessage");

        self.Tables.report = $("#numberGameMonthlyWinLossReport");
        self.Tables.reportTopLeft = self.Tables.report.find(".free-top-left");
        self.Tables.reportTopLeftHeader = self.Tables.report.find("#numberGameMonthlyWinLossReport_RowHeader");
        self.Tables.reportContent_Container = self.Tables.report.find("#numberGameMonthlyWinLossReport_Content");
        self.Tables.reportContent = self.Tables.report.find("#numberGameMonthlyWinLossReport_Content tbody");
        self.Tables.reportTitle = self.Tables.report.find("#numberGameMonthlyWinLossReport_Title");
        self.Tables.reportExportExcel = self.Tables.report.find("#numberGameMonthlyWinLossReport_ExportExcel");
        self.Tables.reportRowHeader = self.Tables.reportTopLeftHeader.find("tbody");
        self.Tables.reportRowFooter = self.Tables.report.find(".row_footer_content thead");
        self.Tables.reportColFooter = self.Tables.report.find(".col_footer_content thead");
        self.Tables.exportFrame = $('#frm');
    };
    self.BindingEvent = function () {
        self.Tables.btnGenerate.click(function () {
            self.GetReport();
        });

        self.Tables.reportExportExcel.click(function () {
            self.ExportReport();
        });
    };
    self.GetParams = function () {
        self.Params.reportParams.Fromdate = self.Tables.txtFromDate.val();
        self.Params.reportParams.Todate = self.Tables.txtTodate.val();
    };
    self.GetReport = function () {
        try { parent.BlockUI(); } catch (e) { }
        self.GetParams();
        self.Tables.errorMessage.empty().addClass("hidden");
        $.ajax({
            type: "POST",
            url: window.rootUrl + "NumberGameMonthlyWinLossReportV4/GetReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(self.Params.reportParams),
            beforeSend: function () {
            },
            success: function (rs) {
                self.RenderReport(rs);
            },
            error: function (event, jqXHR, ajaxSettings) {
                self.RenderReport(null);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    };
    self.ResetTempHtml = function () {
        self.TempHtml.frozenHtml = '';
        self.TempHtml.contentHtml = '';
        self.TempHtml.footerFrozenHtml = '';
        self.TempHtml.footerContentHtml = '';
    };
    self.RemoveSpaceIE9TempHtml = function () {
        self.TempHtml.frozenHtml = stringformat.removeSpaceIE9(self.TempHtml.frozenHtml);
        self.TempHtml.contentHtml = stringformat.removeSpaceIE9(self.TempHtml.contentHtml);
        self.TempHtml.footerFrozenHtml = stringformat.removeSpaceIE9(self.TempHtml.footerFrozenHtml);
        self.TempHtml.footerContentHtml = stringformat.removeSpaceIE9(self.TempHtml.footerContentHtml);
    };
    self.RenderReport = function (report) {
        var reportTitle = "Number Game Monthly Win Loss - From " + self.Params.reportParams.Fromdate +
                                                        " To " + self.Params.reportParams.Todate;
        self.Tables.reportTitle.html(reportTitle);
        if (report && report.ChildsInfo.length > 0) {
            self.Tables.reportExportExcel.removeClass("hidden");
            self.ResetTempHtml();
            var length = report.ChildsInfo.length;
            var frozenHtml = '';
            var info = null;
            for (var i = 0; i < length; i++) {
                info = report.ChildsInfo[i].GrandTotal;
                frozenHtml = '<td><div class="col100 c">' + info.WinLossDate_Str + '</div></td>';

                self.TempHtml.frozenHtml += '<tr>' + frozenHtml + ' </tr>';
                self.TempHtml.contentHtml += '<tr>' +
                                                    frozenHtml +
                                                    '<td><div class="col120 r">' + self.FormatNumberColor(info.MemberWinLoss, true, false) + '</div></td>' +
                                                    '<td><div class="col100 r">' + self.FormatNumberColor(info.Position) + '</div></td>' +
                                                    '<td><div class="col80 r">' + self.FormatNumberColor(info.Commission) + '</div></td>' +
                                                    '<td><div class="col100 r">' + self.FormatNumberColor(info.LineTotal, true, false) + '</div></td>' +
                                                    '<td><div class="col100 r">' + self.FormatNumberColor(info.Turnover) + '</div></td>' +
                                                    '<td><div class="col80 r">' + self.FormatNumberColor(info.Margin_Str, true, true) + '</div></td>' +
                                              '</tr>';
            }

            info = report.GrandTotal;
            frozenHtml = '<th><div class="col100 c">' + info.Name + '</div></td>';
            self.TempHtml.footerFrozenHtml += '<tr">' + frozenHtml + ' </tr>';
            self.TempHtml.footerContentHtml += '<tr>' +
                                                    frozenHtml +
                                                    '<th><div class="col120 r">' + self.FormatNumberColor(info.MemberWinLoss, true, false) + '</div></th>' +
                                                    '<th><div class="col100 r">' + self.FormatNumberColor(info.Position) + '</div></th>' +
                                                    '<th><div class="col80 r">' + self.FormatNumberColor(info.Commission) + '</div></th>' +
                                                    '<th><div class="col100 r">' + self.FormatNumberColor(info.LineTotal, true, false) + '</div></th>' +
                                                    '<th><div class="col100 r">' + self.FormatNumberColor(info.Turnover) + '</div></th>' +
                                                    '<th><div class="col80 r">' + self.FormatNumberColor(info.Margin_Str, true, true) + '</div></th>' +
                                               '</tr>';

            self.RemoveSpaceIE9TempHtml();
            self.Tables.reportRowHeader.empty().append(self.TempHtml.frozenHtml);
            self.Tables.reportContent.empty().append(self.TempHtml.contentHtml);
            self.Tables.reportRowFooter.empty().append(self.TempHtml.footerFrozenHtml);
            self.Tables.reportColFooter.empty().append(self.TempHtml.footerContentHtml);
        }
        else {
            self.Tables.reportExportExcel.addClass("hidden");
            var frezeeContent = "<td><div>&nbsp</div></td>";
            self.Tables.reportRowHeader.empty().append("<tr>" + frezeeContent + "</tr>");
            self.Tables.reportContent.empty().append(
                "<tr>" +
                    frezeeContent +
                    "<td colspan='6' class='b c'>There is no data.</td>\
                 </tr>");
            self.Tables.reportRowFooter.empty();
            self.Tables.reportColFooter.empty();
        }

        self.Tables.report.removeClass("hidden")
                          .gridScroller(self.Options.report);
    };
    self.FormatNumberColor = function (value, isPositive, isPercent) {
        var isNumber = $.isNumeric(value);
        var number = 0;

        if (isNumber) {
            number = value;
        } else {
            number = value.replace(',', '');
        }

        if (isPercent) {
            number = number * 100;
        }

        var formattedNumber = numeral(number).format(FormatConstant.TWO_DECIMAL_NUMBER);
        if (isPositive && number > 0) {
            if (isPercent) {
                return '<div class="color-red">' + formattedNumber + '%</div>';
            }

            return '<div class="color-red">' + formattedNumber + '</div>';
        }

        return formattedNumber;
    };
    self.ExportReport = function () {
        var url = window.rootUrl + "NumberGameMonthlyWinLossReportV4/ExportReport?" + $.param(self.Params.reportParams);
        self.Tables.exportFrame.attr('src', url);
    };
};

var reportManager = new ReportManager();