function viewBetListByIP(fromDate, ip) {
    var parentIndex = 1;
    parent[parentIndex].location = window.rootUrl + 'TarusBetList/BetListByIP?IsLink=true&FromDate=' + fromDate + '&IP=' + ip;

    var betListPopUp = parent.$(".ui-dialog");
    if (betListPopUp && betListPopUp.length) {
        betListPopUp.dialog('close');
        betListPopUp.dialog('destroy');
    }

    var detailPopUp = parent.$('div[id^="dialog-modal"][id$="Details"]');
    detailPopUp.dialog('close');
    detailPopUp.dialog('destroy');
}

var JackpotDetailsManager = function () {
    var self = this;
    var gridContainer, gridOpts, formData;

    function initGridOpts() {
        return {
            tblContentId: 'tblContent',
            manualCopyHeader: true,
            isShowPager: true,
            fullWidth: true,
            fullHeight: true,
            gridWidth: 930,
            gridHeight: 20,
            freezeWidth: 0,
            numberFreezeCol: 0,
        };
    }

    self.initPage = function () {
        gridOpts = initGridOpts();
        formData = {
            transId: transId,
            custId: custId,
            betType: betType,
        };

        gridContainer = $("#jackpot-details");
        gridContainer.initGrid(gridOpts);
        gridContainer.removeClass("display-none").gridScroller(gridOpts);

        $("#btnExport").click(function () {
            var exportData = {
                transId: formData.transId,
                custId: formData.custId,
                betType: formData.betType
            };
            var url = window.rootUrl + "TarusBetList/ExportJackpotDetails?" + $.param(exportData);
            $("#frmExportExcel").attr("src", url);
        });
    };
};

var jackpotDetailsManager = new JackpotDetailsManager();