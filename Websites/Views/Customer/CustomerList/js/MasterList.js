var masterList = {
    params: {
        superId: '',
        pageSize: 50,
        pageIndex: 1,
        searchText: "",
        selectAlpha: "",
        selectCurreny: "",
        status: "",
    },   
    gridOption: {
        tblContentId: 'tbmasterlist',
        manualCopyHeader: true,  //totalColumnWidth = 1380  + totalColumn = 18 
        isShowPager: true, //=totalColumnWidth + totalColumn*4 +totalColumn
        fullWidth: true,
        fullHeight: true,
        gridWidth: 1490,
        gridHeight: 40,
        freezeWidth: 235, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 3
    },
    getMasterList: function (isSearch) {
        try { parent.BlockUI(); } catch (e) { }
        masterList.getParams(isSearch);
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerList/MasterAgentList",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(masterList.params),
            success: function (result) {
                if ($.browser.msie) {
                    var ie = parseInt($.browser.version, 10);
                    if (ie == 9) {
                        result = result.replace(expr, '><');
                    }
                }
                $('#masterlistMain').empty().html(result);
                $('#masterlistgrid').initGrid(masterList.gridOption);
                masterList.initPager(_totalChildren, masterList.params.pageSize, masterList.params.pageIndex - 1);
                $('#masterlistgrid').removeClass("hidden").gridScroller(masterList.gridOption);
                $("#masterlistMain").show();
            },
            error: function () {
                $("#masterlistMain").hide();
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = masterList.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = _totalPages;
        page_index = page_index < max_page ? page_index : max_page - 1;
        masterList.params.pageIndex = page_index + 1;
        if (_items_per_page != masterList.params.pageSize) {
            masterList.params.pageIndex = 1;
        }
        masterList.params.pageSize = _items_per_page;
        masterList.getMasterList();
        return false;
    },
    getParams: function (isSearch) {
        masterList.params.superId = _superId;
        if (isSearch) {
            masterList.params.pageIndex = 1;
        }
        if (_superId == 0) {
            masterList.params.hr = "";

            var search = $("#txtSearch");
            if (search.length > 0 && search.val() != "All") {
                masterList.params.searchText = $("#txtSearch").val();
            }
            else { masterList.params.searchText = ""; }

            var firstLetter = $("#listAlpha");
            if (firstLetter.length > 0 && firstLetter.val() != "All") {
                masterList.params.selectAlpha = firstLetter.val();
            }
            else { masterList.params.selectAlpha = ""; }

            var currrency = $("#listCurrency")
            if (currrency.length > 0 && currrency.val() != "-1") {
                masterList.params.selectCurreny = currrency.val();
            }
            else { masterList.params.selectCurreny = ""; }

            var dcomm = $("#DComm");
            if (dcomm.length > 0) {
                masterList.params.dComm = dcomm.val();
            }

            var status = $("#listType");
            if (status.length > 0 && status.val() != "") {
                masterList.params.status = status.val();
            }
            else { masterList.params.status = ""; }
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
        masterList.gridOption.gridWidth = initWidth;
    }
};

head.ready(function (event) {
    try { parent.BlockUI(); } catch (e) { }
    masterList.reCalculateGrid();
    if (_superId == 0) {
        $('#filter').removeClass("hidden");
        masterList.gridOption.gridHeight = 70;
    }
    $('#masterlistgrid').initGrid(masterList.gridOption);
    masterList.initPager(_totalChildren, masterList.params.pageSize, masterList.params.pageIndex - 1);
    $('#masterlistgrid').removeClass("hidden").gridScroller(masterList.gridOption);
    $("#masterlistMain").show();
    try { parent.jQuery.unblockUI(); } catch (e) { }
});
