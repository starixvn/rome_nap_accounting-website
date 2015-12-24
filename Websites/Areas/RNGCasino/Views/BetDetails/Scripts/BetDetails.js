function viewBetListByIP(fromDate, ip) {
    var parentIndex = 1;
    parent[parentIndex].location = window.rootUrl + 'TarusBetList/BetListByIP?IsLink=true&FromDate=' + fromDate + '&IP=' + ip;

    var betListPopUp = parent.$(".ui-dialog");
    if (betListPopUp && betListPopUp.length) {
        betListPopUp.dialog('close');
        betListPopUp.remove();
    }

    var productName = "RNGCasino";
    var detailPopUp = parent.$("#dialog-modal" + productName + "Details");
    detailPopUp.dialog('close');
    detailPopUp.remove();
}

var BetDetailsManager = function () {
    var self = this;
    self.initPage = function () {
        var gridContainer = $("#betlist-details");
        var gridOpts = {
            tblContentId: 'tblContent',
            manualCopyHeader: true,
            isShowPager: true,
            fullWidth: true,
            fullHeight: true,
            gridWidth: 930,
            gridHeight: 20,
            freezeWidth: 0,
            numberFreezeCol: 0
        };

        gridContainer.initGrid(gridOpts);
        gridContainer.removeClass("display-none").gridScroller(gridOpts);
    };
};

var betDetailsManager = new BetDetailsManager();
