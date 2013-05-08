
var matchWinLoss2aV2 = {
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

        //        $('#btnReport').click(function () {
        //            matchWinLoss2aV2.viewReport(0);
        //        });
        //        $('#btnExport').click(function () {
        //            matchWinLoss2aV2.exportExcel('AllMainReports');
        //        });

        var gridOptsWLSummaryByBetTypeReport = {
            tblContentId: 'wlSummaryByBetTypeReport_Container',
            isShowPager: false
        };
        var gridOptsWLSummaryBySportReport = {
            tblContentId: 'wlSummaryBySportTypeReport_Container',
            isShowPager: false
        };
        //        $("#wlSummaryByBetTypeReport").initGrid(gridOptsWLSummaryByBetTypeReport);
        //        $("#wlSummaryBySportTypeReport").initGrid(gridOptsWLSummaryBySportReport);

        //        $("#wlSummaryByBetTypeReport").find(".free-cell-top-right div").append('<input id="btnExportWLSummaryByBetTypeReport" name="btnExport" type="button" value="" class="btnExcelReport" title="Export to Excel" style="" />');
        //        $("#wlSummaryBySportTypeReport").find(".free-cell-top-right div").append('<input id="btnExportWLSummaryBySportTypeReport" name="btnExport" type="button" value="" class="btnExcelReport" title="Export to Excel" style="" />');

        //        $('#btnExportWLSummaryByBetTypeReport').click(function () {
        //            matchWinLoss2aV2.exportExcel('WLSummaryByBetTypeReport');
        //        });
        //        $('#btnExportWLSummaryBySportTypeReport').click(function () {
        //            matchWinLoss2aV2.exportExcel('WLSummaryBySportTypeReport');
        //        });

        //        var reportHeight = ($(window).height() - 80) / 2;
        var reportHeight = 400;
        wlSummaryByBetTypeV2.reportHeight = reportHeight;
        matchWinLossDetailV2.reportHeight = reportHeight;
    },
    initGrid: function () {
        if (matchWinLoss2aV2.isFirstLoad) {
            matchWinLoss2aV2.firstLoadInitialize();
            matchWinLoss2aV2.isFirstLoad = false;
        }
    },
    viewReport: function (viewType) {
        switch (viewType) {
            case 0:
                matchWinLoss2aV2.lastReportDate = StringFormatter.get_MMDDYYYY_String(new Date($('#txtFromDate').val()));
                break;
            case 1:
                matchWinLoss2aV2.lastReportDate = StringFormatter.get_MMDDYYYY_String(new Date($('#btnToday').val()));
                $("#txtFromDate").datepicker("setDate", new Date($('#btnToday').val()));
                break;
            case 2:
                matchWinLoss2aV2.lastReportDate = $('#btnYesterday').val();
                $("#txtFromDate").datepicker("setDate", new Date($('#btnYesterday').val()));
                break;
        }
        wlSummaryByBetTypeV2.viewSummaryReportByBetType();
        matchWinLossDetailV2.viewSummaryReportBySportType();
    },
    exportExcel: function (exportType) {
        var reportDate = '';
        switch (exportType) {
            case 'AllMainReports':
                reportDate = matchWinLoss2aV2.lastReportDate;
                break;
            default:
                reportDate = matchWinLoss2aV2.lastReportDate;
                break;
        }

        var url = '../WLSummaryByBetType/MatchWinLoss2a?ExportType=' + exportType + '&ReportDate=' + reportDate;
        $('#frm').attr('src', url);
    }
}