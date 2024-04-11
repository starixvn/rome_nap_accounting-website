var MemberList = {
    params: {
        agentId: '',
        pageSize: 50,
        pageIndex: 1,
        type: 0 
    },  
    gridOption: {
        tblContentId: 'tbMemberList',
        manualCopyHeader: true,  //totalColumnWidth = 1270
        isShowPager: true, //=totalColumnWidth + totalColumn*4 +totalColumn
        fullWidth: true,
        fullHeight: true,
        gridWidth: 1500,
        gridHeight: 40,
        freezeWidth: 235, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 3
    },
    getMemberList: function (isSearch) {
        try { parent.BlockUI(); } catch (e) { }
        MemberList.getParams(isSearch);
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerList/MemberList",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(MemberList.params),
            success: function (result) {
                if ($.browser.msie) {
                    var ie = parseInt($.browser.version, 10);
                    if (ie == 9) {
                        result = result.replace(expr, '><');
                    }
                }
                MemberList.reCalculateGrid();
                $('#MemberListMain').html(result);
                $('#MemberListgrid').initGrid(MemberList.gridOption);
                MemberList.initPager(_totalChildren, MemberList.params.pageSize, MemberList.params.pageIndex - 1);
                $('#MemberListgrid').removeClass("hidden").gridScroller(MemberList.gridOption);
                $("#MemberListMain").show();
            },
            error: function () {
                $("#MemberListMain").hide();

            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = MemberList.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = _totalPages;
        page_index = page_index < max_page ? page_index : max_page - 1;
        MemberList.params.pageIndex = page_index + 1;
        if (_items_per_page != MemberList.params.pageSize) {
            MemberList.params.pageIndex = 1;
        }
        MemberList.params.pageSize = _items_per_page;
        MemberList.getMemberList();
        return false;
    },
    getParams: function (isSearch) {
        MemberList.params.agentId = _agentId;
        if (isSearch) {
            MemberList.params.pageIndex = 1;
        }
    },  
    virtualSportsSetting: function (custId, uplineId, roleId) {
        showPopup("Virtual Sport"
                , "VirtualSportMemberPT/EditMember?custId=" + custId + "&uplineId=" + uplineId + "&roleId=" + roleId
                , 0, 0, 860, 480, false, 2000, null);
    },  
    reCalculateGrid: function () {
        var initWidth = 1255;
        if (_IsVisibleVirtualRate == "True") {
            initWidth += 65;
        }    
       
        MemberList.gridOption.gridWidth = initWidth;
    }
};

head.ready(function (event) {
    try { parent.BlockUI(); } catch (e) { }
    MemberList.reCalculateGrid();
    $('#MemberListgrid').initGrid(MemberList.gridOption);
    MemberList.initPager(_totalChildren, MemberList.params.pageSize, MemberList.params.pageIndex - 1);
    $('#MemberListgrid').removeClass("hidden").gridScroller(MemberList.gridOption);
    $("#MemberListMain").show();
    try { parent.jQuery.unblockUI(); } catch (e) { }
});
