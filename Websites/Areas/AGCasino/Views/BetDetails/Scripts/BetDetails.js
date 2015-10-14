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
                 