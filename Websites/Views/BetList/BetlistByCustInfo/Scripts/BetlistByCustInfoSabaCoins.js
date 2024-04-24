var BetlistByCustInfoSabaCoins = {
    paramGetBetBetlist: {
        fromDate: null,
        toDate: null,
        custId: null,
        betType: 0,
        pageIndex: 1,
        pageSize: 100,
        language: null
    },
    paramGetBetBetlistbyRefno: {
        customerId: null,
        transactionId: null,
        language: null
    },
    URL: null,
    gridOpts: {
        tblContentId: 'tblContent-sabacoins',
        manualCopyHeader: true,
        isShowPager: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 970, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 180,
        freezeWidth: 0, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 0
    },
    eventGridOpts: {
        tblContentId: 'tblContent-sabacoins',
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

    BuildTicketStatusDropdownlist: function () {
        var multiselectData = [];
        for (var i = 0; i < ticketStatus.length; i++) {
            multiselectData.push({
                Id: "selecteStatus" + ticketStatus[i].Value,
                Value: ticketStatus[i].Value,
                Text: ticketStatus[i].Text,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }
        $("#ddlStatus-sabacoins").multiSelect({
            minWidth: 150,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: "All",
            listData: multiselectData,
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All',
            noneSelectedText: "Select Statuses",
            onCheck: function () {
                if ($("#ddlStatus-sabacoins").multiSelect("getCheckedItems").length === 0) {
                    showAlertMessage("err", "Please choose Status!", 1);
                }
                else {
                    $("#err-sabacoins").empty();
                }
            },
            noItemText: "No Status found."
        })
    },
    getBetListByCustInfo: function () {
        var params = BetlistByCustInfoSabaCoins.getParams();
        if (params.statusIds === "") {
            return;
        }
        var isByDateRange = $('input[name=type-sabacoins]:checked').val() == "-2";
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + BetlistByCustInfoSabaCoins.URL,
            data: params,
            success: function (result) {
                $("#betListSabaCoinsContent").html(result);

                if (typeof _length != "undefined") {
                    BetlistByCustInfoSabaCoins.gridOpts.isShowPager = ((_length == 0 || _length == 1) ? false : true);
                    $('#betListSabaCoinsContent > #BetlistByCustInfo').initGrid(BetlistByCustInfoSabaCoins.gridOpts);
                    $('#betListSabaCoinsContent > #BetlistByCustInfo').removeClass("display-none").gridScroller(BetlistByCustInfoSabaCoins.gridOpts);
                    if (!isByDateRange) {
                        $("#betListSabaCoinsContent > a:[id^='hidden']").each(function () {
                            this.click();
                            $(this).removeAttr("href");
                        });
                    }

                    BetlistByCustInfoSabaCoins.initPager(_length,
                        BetlistByCustInfoSabaCoins.paramGetBetBetlist.pageSize,
                        BetlistByCustInfoSabaCoins.paramGetBetBetlist.pageIndex - 1);
                }

                $('#betListSabaCoinsContent > #lang-container').width($('#betListSabaCoinsContent > #BetlistByCustInfo').width())
            },
            error: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    getSelectedStatus: function () {
        return $("#ddlStatus-sabacoins").multiSelect("getCheckedItems").join(",");
    },

    getParams: function () {
        if ($('input[name=type-sabacoins]:checked').val() == "-2") {
            var fromDateTmp = $('#txtfromdate-sabacoins').val();
            var toDateTmp = $('#txttodate-sabacoins').val();
            var bettypeTmp = $('#ddlBettype-sabacoins').val() == null ? 0 : $('#ddlBettype-sabacoins').val();
            var statusIds = BetlistByCustInfoSabaCoins.getSelectedStatus();
            if (BetlistByCustInfoSabaCoins.paramGetBetBetlist.fromDate != fromDateTmp ||
                BetlistByCustInfoSabaCoins.paramGetBetBetlist.toDate != toDateTmp ||
                BetlistByCustInfoSabaCoins.paramGetBetBetlist.betType != bettypeTmp ||
                BetlistByCustInfoSabaCoins.paramGetBetBetlist.statusIds != statusIds) {
                BetlistByCustInfoSabaCoins.paramGetBetBetlist.pageIndex = 1;
                BetlistByCustInfoSabaCoins.paramGetBetBetlist.pageSize = 100;
            }

            BetlistByCustInfoSabaCoins.paramGetBetBetlist.fromDate = fromDateTmp;
            BetlistByCustInfoSabaCoins.paramGetBetBetlist.toDate = toDateTmp;
            BetlistByCustInfoSabaCoins.paramGetBetBetlist.custId = _shadowcustId;
            BetlistByCustInfoSabaCoins.paramGetBetBetlist.betType = bettypeTmp;
            BetlistByCustInfoSabaCoins.paramGetBetBetlist.statusIds = BetlistByCustInfoSabaCoins.getSelectedStatus();
            BetlistByCustInfoSabaCoins.paramGetBetBetlist.language = $('#languageCodeHidden-sabacoins').val();
            BetlistByCustInfoSabaCoins.URL =  "TarusBetList/GetBetListByCustInfo_DateRange";
            return BetlistByCustInfoSabaCoins.paramGetBetBetlist;
        }
        else {
            BetlistByCustInfoSabaCoins.paramGetBetBetlistbyRefno.customerId = _shadowcustId;
            BetlistByCustInfoSabaCoins.paramGetBetBetlistbyRefno.transactionId = $('#txtRefNo-sabacoins').val();
            BetlistByCustInfoSabaCoins.paramGetBetBetlistbyRefno.language = $('#languageCodeHidden-sabacoins').val();

            BetlistByCustInfoSabaCoins.URL =  "BetList/GetBetListByCustInfo_Refno";
            return BetlistByCustInfoSabaCoins.paramGetBetBetlistbyRefno;
        }
    },
    trim: function (str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
    },
    getBettyes: function () {
        var paramGetBetTypes = {
            FromDate: $('#txtfromdate-sabacoins').val(),
            ToDate: $('#txttodate-sabacoins').val(),
            CustID: _shadowcustId
        };
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "BetList/GetBetTypes",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(paramGetBetTypes),
            success: function (rs) {
                if (rs.length > 1) {
                    $('#divBettype-sabacoins').removeClass("hidden");
                    $('#ddlBettype-sabacoins').empty();
                    $.each(rs, function (i, val) {
                        $('#ddlBettype-sabacoins').append('<option value="' + val.BetTypeID + '">' + val.BetTypeName + '</option>');
                    });
                }
                else {
                    $('#divBettype-sabacoins').addClass("hidden");
                }
            },
            error: function (event, jqXHR, ajaxSettings) {

                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    disabledFilter: function () {
        var $infoMsg = $('#info-msg-sabacoins');
        if ($('input[name=type-sabacoins]:checked').val() == "-2") {
            $('#txtRefNo-sabacoins').attr("disabled", "disabled");

            $('#txttodate-sabacoins').removeAttr("disabled", "disabled");
            $('#txtfromdate-sabacoins').removeAttr("disabled", "disabled");
            $('#ddlBettype-sabacoins').removeAttr("disabled", "disabled");
            BetlistByCustInfoSabaCoins.BuildTicketStatusDropdownlist();
            $infoMsg.addClass("hide");
        }
        else {
            $('#txttodate-sabacoins').attr("disabled", "disabled");
            $('#txtfromdate-sabacoins').attr("disabled", "disabled");
            $('#ddlBettype-sabacoins').attr("disabled", "disabled");
            $('#ddlStatus-sabacoins').multiSelect({
                "disabled": true
            });

            $('#txtRefNo-sabacoins').removeAttr("disabled", "disabled");
            $infoMsg.removeClass("hide");
            $('#err-sabacoins').empty();
        }
    },
    initPager: function (total_items, items_per_page, current_page) {
        BetlistByCustInfoSabaCoins.pagerOpts.callback = BetlistByCustInfoSabaCoins.pageSelectCallback;
        BetlistByCustInfoSabaCoins.pagerOpts.items_per_page = items_per_page;
        BetlistByCustInfoSabaCoins.pagerOpts.current_page = current_page;

        $("#betlistMain-sabacoins #pagination-sabacoins").pagination(total_items, BetlistByCustInfoSabaCoins.pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_length / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;

        BetlistByCustInfoSabaCoins.paramGetBetBetlist.pageIndex = page_index + 1;
        BetlistByCustInfoSabaCoins.paramGetBetBetlist.pageSize = _items_per_page;
        BetlistByCustInfoSabaCoins.getBetListByCustInfo(false);
        return false;
    },
};

head.ready(function (event) {
    Helper.renderCalendarFromTo("#txtfromdate-sabacoins, #txttodate-sabacoins");
    Helper.range_date();
    BetlistByCustInfoSabaCoins.BuildTicketStatusDropdownlist();
    BetlistByCustInfoSabaCoins.getBetListByCustInfo();
    BetlistByCustInfoSabaCoins.getBettyes();
    $('#txtRefNo-sabacoins').attr("disabled", "disabled");
});
