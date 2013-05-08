
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

        var reportHeight = ($(window).height() - 100) / 2;
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
        wlSummaryByBetType.viewSummaryReportByBetType();
        matchWinLossDetail.viewSummaryReportBySportType();
    },
    exportExcel: function (exportType) {
        var reportDate = '';
        switch (exportType) {
            case 'AllMainReports':
                reportDate = matchWinLoss2a.lastReportDate;
                break;
            default:
                reportDate = matchWinLoss2a.lastReportDate;
                break;
        }

        var url = 'MatchWinLoss2a?ExportType=' + exportType + '&ReportDate=' + reportDate;
        $('#frm').attr('src', url);
    }
}

$(document).ready(function () {
    matchWinLoss2a.initGrid();
}); 
