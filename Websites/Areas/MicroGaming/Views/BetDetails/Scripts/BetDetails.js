var MicroGamingBetDetailsManager = function () {
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

var microGamingBetDetailsManager = new MicroGamingBetDetailsManager();
                 