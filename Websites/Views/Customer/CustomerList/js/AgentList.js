var AgentList = {
    params: {
        masterId: '',
        pageSize: 50,
        pageIndex: 1,
        doubleCommMaster: "",
        synccasinoUpline: false
    },
    paramsUpdate: {
        custDetail: "",
        userId: 0,
        isDoubleComm: false
    },
    gridOption: {
        tblContentId: 'tbAgentList',
        manualCopyHeader: true,  //totalColumnWidth = 1380  + totalColumn = 18 
        isShowPager: true, //=totalColumnWidth + totalColumn*4 +totalColumn
        fullWidth: true,
        fullHeight: true,
        gridWidth: 1470,
        gridHeight: 40,
        freezeWidth: 235, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 3
    },
    getAgentList: function (isSearch) {
        try { parent.BlockUI(); } catch (e) { }
        AgentList.getParams(isSearch);
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerList/AgentList",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(AgentList.params),
            success: function (result) {
                if ($.browser.msie) {
                    var ie = parseInt($.browser.version, 10);
                    if (ie == 9) {
                        result = result.replace(expr, '><');
                    }
                }
                AgentList.reCalculateGrid();
                $('#AgentListMain').html(result);
                $('#AgentListgrid').initGrid(AgentList.gridOption);
                AgentList.initPager(_totalChildren, AgentList.params.pageSize, AgentList.params.pageIndex - 1);
                $('#AgentListgrid').removeClass("hidden").gridScroller(AgentList.gridOption);
                $("#AgentListMain").show();
            },
            error: function () {
                $("#AgentListMain").hide();

            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = AgentList.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = _totalPages;
        page_index = page_index < max_page ? page_index : max_page - 1;
        AgentList.params.pageIndex = page_index + 1;
        if (_items_per_page != AgentList.params.pageSize) {
            AgentList.params.pageIndex = 1;
        }
        AgentList.params.pageSize = _items_per_page;
        AgentList.getAgentList();
        return false;
    },
    getParams: function (isSearch) {
        AgentList.params.masterId = _masterId;
        if (isSearch) {
            AgentList.params.pageIndex = 1;
        }
        if (_masterId == 0) {

            AgentList.params.doubleCommMaster = "";
        }
        else {
            AgentList.params.doubleCommMaster = _masterDcom.toLowerCase();
        }

        var synccasinoUpline = gup("isSynccasino");
        if (synccasinoUpline != "") {
            AgentList.params.synccasinoUpline = "";
        }
    },  
    reCalculateGrid: function () {
        var initWidth = 1200;
        if (_IsVisibleMaxExposure == "True") {
            initWidth += 85;
        }
        if (_IsVisibleVirtualRate == "True") {
            initWidth += 65;
        }   
        AgentList.gridOption.gridWidth = initWidth;
    }
};


head.ready(function (event) {
    try { parent.BlockUI(); } catch (e) { }
    AgentList.reCalculateGrid();
    $('#AgentListgrid').initGrid(AgentList.gridOption);
    AgentList.initPager(_totalChildren, AgentList.params.pageSize, AgentList.params.pageIndex - 1);
    $('#AgentListgrid').removeClass("hidden").gridScroller(AgentList.gridOption);
    $("#AgentListMain").show();
    try { parent.jQuery.unblockUI(); } catch (e) { }
});
