head.ready(function (event) {
    var gridOpts_Soccer = {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        isShowPager: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 980, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 10,
        freezeWidth: 510, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 3
    };

    $("#BetlistStatement").initGrid(gridOpts_Soccer);
    $('#BetlistStatement').removeClass("display-none").gridScroller(gridOpts_Soccer);

});

