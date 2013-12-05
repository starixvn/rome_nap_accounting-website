var SuperList = {
    params: {
        pageSize: 50,
        pageIndex: 1,
        searchText: "",
        selectAlpha: "",
        selectCurreny: "",
        dComm: "",
        status: ""
    },
    paramsUpdate: {
        custDetail: "",
        userId: 0,
        isDoubleComm: false
    },
    gridOption: {
        tblContentId: 'tbSuperList',
        manualCopyHeader: true,  //totalColumnWidth = 1380  + totalColumn = 18 
        isShowPager: true, //=totalColumnWidth + totalColumn*4 +totalColumn
        fullWidth: true,
        fullHeight: true,
        gridWidth: 1560,
        gridHeight: 40,
        freezeWidth: 365, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 5
    },
    getSuperList: function (isSearch) {
        try { parent.BlockUI(); } catch (e) { }
        SuperList.getParams(isSearch);
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerList/SuperList",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(SuperList.params),
            success: function (result) {
                if ($.browser.msie) {
                    var ie = parseInt($.browser.version, 10);
                    if (ie == 9) {
                        result = result.replace(expr, '><');
                    }
                }
                SuperList.reCalculateGrid();
                $('#SuperListMain').html(result);
                $('#SuperListgrid').initGrid(SuperList.gridOption);
                SuperList.initPager(_totalChildren, SuperList.params.pageSize, SuperList.params.pageIndex - 1);
                $('#SuperListgrid').removeClass("hidden").gridScroller(SuperList.gridOption);
                $("#SuperListMain").show();
            },
            error: function () {
                $("#SuperListMain").hide();

            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = SuperList.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = _totalPages;
        page_index = page_index < max_page ? page_index : max_page - 1;
        SuperList.params.pageIndex = page_index + 1;
        if (_items_per_page != SuperList.params.pageSize) {
            SuperList.params.pageIndex = 1;
        }
        SuperList.params.pageSize = _items_per_page;
        SuperList.getSuperList();
        return false;
    },
    getParams: function (isSearch) {
        if (isSearch) {
            SuperList.params.pageIndex = 1;
        }
        var search = $("#txtSearch");
        if (search.length > 0 && search.val() != "All" && SuperList.params.searchText != search.val()) {
            SuperList.params.searchText = search.val();
        }

        var firstLetter = $("#listAlpha");
        if (firstLetter.length > 0 && firstLetter.val() != "All" && SuperList.params.selectAlpha != firstLetter.val()) {
            SuperList.params.selectAlpha = firstLetter.val();
        }

        var currrency = $("#listCurrency")
        if (currrency.length > 0 && currrency.val() != "-1" && SuperList.params.selectCurreny != currrency.val()) {
            SuperList.params.selectCurreny = currrency.val();
        }
        else { SuperList.params.selectCurreny = ""; }

        var dcomm = $("#DComm");
        if (dcomm.length > 0 && SuperList.params.dComm != dcomm.val()) {
            SuperList.params.dComm = dcomm.val();
        }

        var status = $("#listType");
        if (status.length > 0 && status.val() != "" && SuperList.params.status != status.val()) {
            SuperList.params.status = status.val();
        }
    },
    confirmUpdateDComm: function (isMultiple, id, isAllow) {
        var bCconfirm;
        if (!isMultiple) {
            var s = "'s";
            if (isAllow) {

                bCconfirm = '<tr><td> Are you sure you want to allow Double Commission feature for Sportsbook' + s + ' Losing tickets on this customer?</tr></tr>';
            }
            else {
                bCconfirm = '<tr><td> Are you sure you want to disallow Double Commission feature for Sportsbook' + s + ' Losing tickets on this customer?<br>\
                Warning, all members under this agent will be disallowed as well!</tr></tr>';
            }
            $('#tblconfirm').empty().append(bCconfirm);
            $("#tblconfirm").dialog({
                title: "Edit Single Double Commission for Losing tickets",
                autoOpen: false,
                height: 180,
                width: 400,
                modal: true,
                buttons: {
                    "OK": function () {
                        SuperList.updateDoubleComm(id, isAllow, isMultiple);
                        //                        $(this).dialog("close");
                    }
                },
                close: function (event, ui) {
                }
            });
            $('a.ui-dialog-titlebar-close').show();
            $("#tblconfirm").dialog("open");
        }
        else {
            id = CustomerList.getSelectedID();
            if (!id) return false;
            bCconfirm = '<tr><td><input type="radio" id="rdbAllow" name="type" value="0" checked="checked" /></td><td class="b"> Allow Double Commission</td></tr>\
            <td><input type="radio" id="rdbDisAllow" name="type" value="1"/><td class="b"> Disallow Double Commission</td></tr>';
            $('#tblconfirm').empty().append(bCconfirm);
            $("#tblconfirm").dialog({
                title: "Edit Multiple Double Commission for Losing tickets",
                autoOpen: false,
                height: 180,
                width: 400,
                modal: true,
                buttons: {
                    "Submit": function () {
                        var temp = $('input[name=type]:checked').val();
                        if (temp == "0") {
                            SuperList.updateDoubleComm(id, true, isMultiple);
                        }
                        else {
                            SuperList.updateDoubleComm(id, false, isMultiple);
                        }
                    }
                },
                close: function (event, ui) {
                }
            });
            $('a.ui-dialog-titlebar-close').show();
            $("#tblconfirm").dialog("open");
        }


    },
    updateDoubleComm: function (id, isAllow, isMultiple) {
       SuperList.paramsUpdate.custDetail = id.toString();
       
        SuperList.paramsUpdate.isDoubleComm = isAllow;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerList/UpdateDoubleComm",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(SuperList.paramsUpdate),
            success: function (ResultCustomerlist) {
                if (ResultCustomerlist && ResultCustomerlist.ErrorCode == 1008) {
                    $('#tblconfirm').empty().append(ResultCustomerlist.ErrorMessage).css({ color: 'red' });
                    $("#tblconfirm").dialog({
                        title: "Confirm Update Fail",
                        autoOpen: false,
                        height: 180,
                        width: 400,
                        modal: true,
                        buttons: {
                            "OK": function () {
                                $(this).dialog("close");
                            }
                        },
                        close: function (event, ui) {
                        }
                    });
                    $('a.ui-dialog-titlebar-close').show();
                    $("#tblconfirm").dialog("open");
                    return;
                }

                if (ResultCustomerlist.length > 0) {
                    var errorFlag = false;
                    var length = ResultCustomerlist.length;
                    var arr = [];
                    for (var i = 0; i < length; i++) {
                        if (ResultCustomerlist[i].Result.ErrorCode != 0) {
                            var error = '<tr><td><span class="username">' + ResultCustomerlist[i].UserName + '</span><span class="fail"> :'+ (ResultCustomerlist[i].Result.ErrorMessage != "" ? ResultCustomerlist[i].Result.ErrorMessage : " Update fail .") +'</span></tr></tr>';
                            arr.push(error)
                            errorFlag = true;
                        }
                    }

                    if (!errorFlag) {
                        setTimeout('$("#tblconfirm").dialog("close")', 2000);

                        var htmlsuccess = '';
                            htmlsuccess = '<tr><td><span class="successful">Update successful</span></tr></tr>';

                        var title = "";
                        if (!isMultiple) {
                            title = "Edit Single Double Commission for Losing tickets";
                        }
                        else {
                            title = "Edit Multiple Double Commission for Losing tickets";
                        }
                        $("#tblconfirm").dialog({
                            title: title,
                            autoOpen: false,
                            height: 100,
                            width: 400,
                            modal: true,
                            buttons: {
                                "OK": function () {
                                    $(this).dialog("close");
                                }
                            },
                            close: function (event, ui) {
                                SuperList.getSuperList();
                            }
                        });
                        $('div.ui-dialog-buttonpane').append(htmlsuccess);
                        $('a.ui-dialog-titlebar-close').show();
                        $("#tblconfirm").dialog("open");

                    }
                    else {
                        $('#tblconfirm').empty().append(arr.join(''));
                        $("#tblconfirm").dialog({
                            title: "Confirm Update Fail",
                            autoOpen: false,
                            height: 180,
                            width: 400,
                            modal: true,
                            buttons: {
                                "OK": function () {
                                    $(this).dialog("close");
                                }
                            },
                            close: function (event, ui) {
                            }
                        });
                        $('a.ui-dialog-titlebar-close').show();
                        $("#tblconfirm").dialog("open");
                    }
                }
            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    OpenEdit: function (superid, custid, uname) {
        var urlEdit = "";
        if (_IsShowCustomerSettingRenovation == 3) {
            urlEdit = "EditSuper/EditSuperPage";
        }
        else {

            urlEdit = "Customer/v2/AddNewSuper.aspx";
        }
        var url = urlEdit + "?custid=" + custid + "&superId=" + superid + "&uname=" + uname;
        var pageIndex = "1";
        if (location.href.indexOf("#") > -1) {
            pageIndex = location.href.split("#")[1];
        }
        showPopup("Update Super Setting", url, "", "", 1000, 650, false, 2000, function () { callAjax(getParams(pageIndex)); });
    },
    editSetting: function (custId, roleId) {
        showPopup(""
                , "Customer/PT/Poker/SuperUpdate.aspx?custId=" + custId + "&roleId=" + roleId
                , 0, 0, 640, 480, false, 2000, null);
    },
    liveCasinoSetting: function (custId, exchangeId, uplineId) {
        showPopup(""
                , "LiveCasino/PTSetting/EditSuper.aspx?custId=" + custId + "&uplineId=" + uplineId
                , 0, 0, 640, 480, false, 2000, null);
    },
    virtualSportsSetting: function (custId, uplineId, roleId) {
        showPopup(""
                , "VirtualSportSuperPT/EditSuper?custId=" + custId + "&uplineId=" + uplineId + "&roleId=" + roleId
                , 0, 0, 860, 480, false, 2000, null);
    },
    bettingSetting: function (custId, uplineId, accId, groupName) {
        showPopup(" Edit Betting Limit"
                , "customersetting/betsetting/sportsbook/index/" + custId + "/" + uplineId + "/" + accId + "/" + groupName
                , 0, 0, 900, 680, false, 2000, null);
    },
    reCalculateGrid: function () {
        var initWidth = 1385;
        if (_IsVisibleMaxExposure == "True") {
            initWidth += 85;
        }
        if (_IsVisibleVirtualRate == "True") {
            initWidth += 65;
        }
        if (_enableNumberGame == "True") {
            initWidth += 75;
        }
        SuperList.gridOption.gridWidth = initWidth;
    }
};


head.ready(function (event) {
    try { parent.BlockUI(); } catch (e) { }
    SuperList.reCalculateGrid();
    $('#filter').removeClass("hidden");
    SuperList.gridOption.gridHeight = 70;

    $('#SuperListgrid').initGrid(SuperList.gridOption);
    SuperList.initPager(_totalChildren, SuperList.params.pageSize, SuperList.params.pageIndex - 1);
    $('#SuperListgrid').removeClass("hidden").gridScroller(SuperList.gridOption);
    $("#SuperListMain").show();
    try { parent.jQuery.unblockUI(); } catch (e) { }
});
