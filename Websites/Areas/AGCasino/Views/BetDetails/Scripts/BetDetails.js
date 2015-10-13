function showAGCasinoDetails(refno, winlossDate, custId, transType) {
    var params = $.param({
        Refno: refno,
        WinLossDate: winlossDate,
        TransType: transType,
        CustId: custId
    });

    parent.OpenNextPopup(
        "AGCasinoDetails",
        'AGCasinoBetList/BetDetails?' + params,
        'AG Casino Details',
        900,
        600,
        true,
        false,
        window,
        0,
        null,
        null);
}

var AGCasinoBetDetailsManager = function () {
    var self = this;
    self.initPage = function () {
        var gridContainer = $("#betlist-details");
        var gridOpts = {
            tblContentId: 'tblContent',
            manualCopyHeader: true,
            isShowPager: true,
            fullWidth: true,
            fullHeight: true,
            gridWidth: 765,
            gridHeight: 20,
            freezeWidth: 0,
            numberFreezeCol: 0
        };

        gridContainer.initGrid(gridOpts);
        gridContainer.removeClass("display-none").gridScroller(gridOpts);
    };
};

var agCasinoBetDetailsManager = new AGCasinoBetDetailsManager();
                 