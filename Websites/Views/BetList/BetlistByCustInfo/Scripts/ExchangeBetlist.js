var ExchangeBetlist = {
    param: {
        fromDate: null,
        toDate: null,
        custId: null,
        pageIndex: 1,
        pageSize: 100
    },

    URL: null,
    gridOpts: {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        isShowPager: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 970, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 120,
        freezeWidth: 0, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 0
    },
    eventGridOpts: {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 675, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 550,
        freezeWidth: 0, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 0
    },
    pagerOpts: {
        callback: null,
        start_per_page: 10,
        step_per_page: 10,
        item_per_page_arr: [50, 100, 150, 200],
        max_items_per_page: 200,
        items_per_page: 50,
        current_page: 1
    },

    getDetails: function (betId, type) {
        var data = {
            betId: betId,
            type: type,
            custId: _custID
        };
        var url = window.rootUrl + "ExchangeBetList/GetDetails?" + $.param(data);
        var width = 600;
        var height = 500;

        parent.OpenNextPopup("Details", url, "Details", width, height, true, false, window, 0, null, null);
    },

    getData: function () {
        var fromDateTmp = $('#exchange_fromdate').val();
        var toDateTmp = $('#exchange_todate').val();
        if (ExchangeBetlist.param.fromDate != fromDateTmp
            && ExchangeBetlist.param.toDate != toDateTmp) {
            ExchangeBetlist.param.pageSize = 100;
            ExchangeBetlist.param.pageIndex = 1;
        }

        ExchangeBetlist.param.fromDate = fromDateTmp;
        ExchangeBetlist.param.toDate = toDateTmp;
        $.ajax({
            type: "POST",
            url: window.rootUrl + "ExchangeBetList/GetData",
            data: {
                fromDate: ExchangeBetlist.param.fromDate,
                toDate: ExchangeBetlist.param.toDate,
                pageSize: ExchangeBetlist.param.pageSize,
                pageIndex: ExchangeBetlist.param.pageIndex,
                custId: _custID
            },
            success: function (result) {
                $("#betListExchangeContent").html(result);

                if (typeof _length != "undefined") {
                    ExchangeBetlist.gridOpts.isShowPager = ((_length == 0 || _length == 1) ? false : true);
                    $("#ExchangeBetlist").initGrid(ExchangeBetlist.gridOpts);
                    $('#ExchangeBetlist').removeClass("display-none").gridScroller(ExchangeBetlist.gridOpts);


                    ExchangeBetlist.initPager(_length,
                    ExchangeBetlist.param.pageSize,
                    ExchangeBetlist.param.pageIndex - 1);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },



    initPager: function (total_items, items_per_page, current_page) {
        ExchangeBetlist.pagerOpts.callback = ExchangeBetlist.pageSelectCallback;
        ExchangeBetlist.pagerOpts.items_per_page = items_per_page;
        ExchangeBetlist.pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, ExchangeBetlist.pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_length / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;

        ExchangeBetlist.param.pageIndex = page_index + 1;
        ExchangeBetlist.param.pageSize = _items_per_page;
        ExchangeBetlist.getData();
        return false;
    }
};



