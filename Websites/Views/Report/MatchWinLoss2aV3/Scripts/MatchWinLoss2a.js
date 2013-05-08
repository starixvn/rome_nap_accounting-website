
var matchWinLoss2a = {
    isFirstLoad: true,
    isBARMActive: false,
    isEightActive: false,
    displayNameBARM: '',
    displayNameEightRM: '',
    lastReportDate: '',
    currentPopup: null,
    firstLoadInitialize: function () {
        var limitFromDate = {
            minDate: minDate,
            maxDate: maxDate
        };
        var options = {
            showOtherMonths: true,
            selectOtherMonths: true
        };
        calendarUtils.renderCalendar("#txtFromDate", limitFromDate, "#txtFromDate", "", toDay, options);
        var strToday = '<input type="button" onclick="matchWinLoss2a.viewReport(1)" id="btnToday" class="btn"/>';
        var strYesterday = '<input type="button" onclick="matchWinLoss2a.viewReport(2)" id="btnYesterday" class="btn"/>';
        $('#spMoreElement').append(strToday);
        $('#spMoreElement').append(strYesterday);
        $('#btnToday').val(toDay);
        $('#btnYesterday').val(yesterday);

        $('#btnReport').click(function () {
            matchWinLoss2a.viewReport(0);
        });
        $('#btnExport').click(function () {
            matchWinLoss2a.exportExcel('AllMainReports');
        });

        var gridOptsWLSummaryByBetTypeReport = {
            tblContentId: 'wlSummaryByBetTypeReport_Container',
            isShowPager: false
        };
        var gridOptsWLSummaryBySportReport = {
            tblContentId: 'wlSummaryBySportTypeReport_Container',
            isShowPager: false
        };
        $("#wlSummaryByBetTypeReport").initGrid(gridOptsWLSummaryByBetTypeReport);
        $("#wlSummaryBySportTypeReport").initGrid(gridOptsWLSummaryBySportReport);

        $("#wlSummaryByBetTypeReport").find(".free-cell-top-right div").append('<input id="btnExportWLSummaryByBetTypeReport" name="btnExport" type="button" value="" class="btnExcelReport" title="Export to Excel" style="" />');
        $("#wlSummaryBySportTypeReport").find(".free-cell-top-right div").append('<input id="btnExportWLSummaryBySportTypeReport" name="btnExport" type="button" value="" class="btnExcelReport" title="Export to Excel" style="" />');

        $('#btnExportWLSummaryByBetTypeReport').click(function () {
            matchWinLoss2a.exportExcel('WLSummaryByBetTypeReport');
        });
        $('#btnExportWLSummaryBySportTypeReport').click(function () {
            matchWinLoss2a.exportExcel('WLSummaryBySportTypeReport');
        });

        //        var reportHeight = ($(window).height() - 80) / 2;
        var reportHeight = 400;
        wlSummaryByBetType.reportHeight = reportHeight;
        matchWinLossDetail.reportHeight = reportHeight;
    },
    initGrid: function () {
        if (matchWinLoss2a.isFirstLoad) {
            matchWinLoss2a.firstLoadInitialize();
            matchWinLoss2a.isFirstLoad = false;
        }
    },
    viewReport: function (viewType) {
        switch (viewType) {
            case 0:
                matchWinLoss2a.lastReportDate = StringFormatter.get_MMDDYYYY_String(new Date($('#txtFromDate').val()));
                break;
            case 1:
                matchWinLoss2a.lastReportDate = StringFormatter.get_MMDDYYYY_String(new Date($('#btnToday').val()));
                $("#txtFromDate").datepicker("setDate", new Date($('#btnToday').val()));
                break;
            case 2:
                matchWinLoss2a.lastReportDate = $('#btnYesterday').val();
                $("#txtFromDate").datepicker("setDate", new Date($('#btnYesterday').val()));
                break;
        }

        if (new Date(balanceUpDay) <= new Date(matchWinLoss2a.lastReportDate)) {
            matchWinLoss2a.getReport();
        }
        else {
            matchWinLoss2aV2.viewReport(viewType);
        }
    },
    getReport: function () {
        var data = {
            'reportDate': matchWinLoss2a.lastReportDate
        };

        matchWinLoss2aUtils.blockUI();
        ClosePopup();
        matchWinLoss2aUtils.disableReportButton();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "MatchWinLoss2aV3/SelMainReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs) {
                    wlSummaryByBetType.renderWLSummaryByBetTypeReport(rs.SummaryReportByBetType,
                    data.reportDate);

                    matchWinLossDetail.renderWLSummaryBySportTypeReport(rs.SummaryReportBySportType,
                    matchWinLoss2a.lastReportDate);
                }
                else {
                    wlSummaryByBetType.renderWLSummaryByBetTypeReport_NoData();
                    matchWinLossDetail.renderWLSummaryBySportTypeReport_NoData();
                }
            },
            error: function () {
                wlSummaryByBetType.renderWLSummaryByBetTypeReport_NoData();
                matchWinLossDetail.renderWLSummaryBySportTypeReport_NoData();
            },
            complete: function () {
                matchWinLoss2aUtils.nonBorderLastRow("wlSummaryByBetTypeReport_RowHeader");
                matchWinLoss2aUtils.nonBorderLastRow("wlSummaryByBetTypeReport_Container");
                matchWinLoss2aUtils.nonBorderLastRow("wlSummaryBySportTypeReport_RowHeader");
                matchWinLoss2aUtils.nonBorderLastRow("wlSummaryBySportTypeReport_Container");

                var gridOptsWLSummaryByBetTypeReport = {
                    tblContentId: 'wlSummaryByBetTypeReport_Container',
                    manualCopyHeader: true,
                    fullWidth: true,
                    fullHeight: false,
                    gridHeight: wlSummaryByBetType.reportHeight,
                    freezeWidth: 120,
                    freezeHeight: 200,
                    numberFreezeCol: 1
                };
                $("#wlSummaryByBetTypeReport").removeClass("hidden");
                $("#wlSummaryByBetTypeReport").gridScroller(gridOptsWLSummaryByBetTypeReport);
                wlSummaryByBetType.make2TableHeaderSameHeight($("#wlSummaryByBetTypeReport").find(".free-cell-top-left"), $("#wlSummaryByBetTypeReport").find(".row_header_content"));

                var gridOptsWLSummaryBySportReport = {
                    tblContentId: 'wlSummaryBySportTypeReport_Container',
                    manualCopyHeader: true,
                    fullWidth: true,
                    fullHeight: false,
                    gridHeight: matchWinLossDetail.reportHeight,
                    gridWidth: 845,
                    freezeWidth: 170,
                    freezeHeight: 183,
                    numberFreezeCol: 1
                };
                $("#wlSummaryBySportTypeReport").removeClass("hidden");
                $("#wlSummaryBySportTypeReport").gridScroller(gridOptsWLSummaryBySportReport);

                matchWinLoss2aUtils.enableReportButton();
                matchWinLoss2aUtils.unblockUI();
            }
        });
    },
    exportExcel: function (exportType) {
        if (new Date(balanceUpDay) > new Date(matchWinLoss2a.lastReportDate)) {
            matchWinLoss2aV2.exportExcel(exportType);
            return;
        }

        var reportDate = '';
        switch (exportType) {
            case 'AllMainReports':
                reportDate = matchWinLoss2a.lastReportDate;
                break;
            default:
                reportDate = matchWinLoss2a.lastReportDate;
                break;
        }

        var url = 'Index?ExportType=' + exportType + '&ReportDate=' + reportDate;
        $('#frm').attr('src', url);
    }
}

$(document).ready(function () {
    matchWinLoss2a.initGrid();
    matchWinLoss2aV2.initGrid();
}); 
