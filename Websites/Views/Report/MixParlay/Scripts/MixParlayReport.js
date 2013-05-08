

var mixParlayReport = {
    reportDate: null,
    betType: null,
    betTypeId: null,
    reportWidth: 0,
    isFirstLoad: true,
    initPage: function (reportDate, betTypeId, betType) {
        mixParlayReport.reportDate = reportDate;
        mixParlayReport.betTypeId = betTypeId;
        mixParlayReport.betType = betType;

        $("#advanceMixParlayWLReport_ExportExcel").click(function () {
            advanceMixParlayReport.exportExcel();
        });

        $("#normalMixParlayWLReport_ExportExcel").click(function () {
            normalMixParlayReport.exportExcel();
        });

        var gridOptsAdvanceMixParlayReport = {
            tblContentId: 'advanceMixParlayWLReport_Container',
            isShowPager: false
        };
        var gridOptsNormalMixParlayReport = {
            tblContentId: 'normalMixParlayWLReport_Container',
            isShowPager: false
        };
        $("#advanceMixParlayWLReport").initGrid(gridOptsAdvanceMixParlayReport);
        $("#normalMixParlayWLReport").initGrid(gridOptsNormalMixParlayReport);

        var dReportDate = new Date(reportDate);
        var dBeginSupportMPDate = new Date(beginSupportMPDate);
        if (dReportDate >= dBeginSupportMPDate) {
            $("#normalMixParlayWLReport").addClass("hidden");
            mixParlayReport.reportWidth = advanceMixParlayReport.reportWidth;
            mixParlayReport.resizePopupWidth(mixParlayReport.reportWidth);
            advanceMixParlayReport.initPage(reportDate, betTypeId, betType);
        }
        else {
            $("#advanceMixParlayWLReport").addClass("hidden");
            mixParlayReport.reportWidth = normalMixParlayReport.reportWidth;
            mixParlayReport.resizePopupWidth(mixParlayReport.reportWidth);
            normalMixParlayReport.initPage(reportDate, betTypeId, betType);
        }
    },
    resizePopupWidth: function (width) {
        if (window.parent != null && $(window.parent).width() <= 1024 && width >= advanceMixParlayReport.reportWidth) {
            $(".ui-dialog", parent.document).css("left", "10px");
            $("#innerFrame", parent.document).css("width", width + 25 + "px");
            $(".ui-dialog", parent.document).css("width", width + 40 + "px");
        } else {
            $("#innerFrame", parent.document).css("width", width + 25 + "px");
            $(".ui-dialog", parent.document).css("width", width + 40 + "px");
        }
    }
}

$(document).ready(function () {
    if (mixParlayReport.isFirstLoad) {
        mixParlayReport.initPage(reportDate, betTypeId, betType);
        mixParlayReport.isFirstLoad = false;
    }
}); 
