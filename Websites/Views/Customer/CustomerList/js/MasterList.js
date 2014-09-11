var masterList = {
    params: {
        superId: '',
        pageSize: 50,
        pageIndex: 1,
        searchText: "",
        selectAlpha: "",
        selectCurreny: "",
        dComm: "",
        status: "",
        synccasinoUpline: "",
        doubleCommSuper: ""
    },
    paramsUpdate: {
        custDetail: "",
        userId: 0,
        isDoubleComm: false
    },
    gridOption: {
        tblContentId: 'tbmasterlist',
        manualCopyHeader: true,  //totalColumnWidth = 1380  + totalColumn = 18 
        isShowPager: true, //=totalColumnWidth + totalColumn*4 +totalColumn
        fullWidth: true,
        fullHeight: true,
        gridWidth: 1600,
        gridHeight: 40,
        freezeWidth: 365, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 5
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

            var synccasinoUpline = gup("isSynccasino");
            if (synccasinoUpline != "") {
                masterList.params.synccasinoUpline = "";
            }
            masterList.params.doubleCommSuper = "";
        }
        else {
            masterList.params.doubleCommSuper = _superDcom.toLowerCase();
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
                        masterList.updateDoubleComm(id, isAllow, isMultiple);
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
                            masterList.updateDoubleComm(id, true, isMultiple);
                        }
                        else {
                            masterList.updateDoubleComm(id, false, isMultiple);
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
        masterList.paramsUpdate.userId = _superId;

        masterList.paramsUpdate.custDetail = id.toString();

        masterList.paramsUpdate.isDoubleComm = isAllow;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerList/UpdateDoubleComm",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(masterList.paramsUpdate),
            success: function (ResultCustomerlist) {

                if (ResultCustomerlist && ResultCustomerlist.ErrorCode == 1008) {
                    $('#tblconfirm').empty().append(ResultCustomerlist.ErrorMessage).css({color: 'red'});
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
                            var error = '<tr><td><span class="username">' + ResultCustomerlist[i].UserName + '</span><span class="fail"> :' + (ResultCustomerlist[i].Result.ErrorMessage != "" ? ResultCustomerlist[i].Result.ErrorMessage : " Update fail .") + '</span></tr></tr>';
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
                                masterList.getMasterList();
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
            urlEdit = "EditMaster/EditMasterPage";
        }
        else {

            urlEdit = "Customer/v2/EditMaster.aspx";
        }
        var url = urlEdit + "?custid=" + custid + "&superId=" + superid + "&uname=" + uname;
        var pageIndex = "1";
        if (location.href.indexOf("#") > -1) {
            pageIndex = location.href.split("#")[1];
        }
        showPopup("Edit Master Setting", url, "", "", 1000, 650, false, 2000, function () { callAjax(getParams(pageIndex)); });
    },
    editSetting: function (custId, roleId) {
        showPopup("Poker"
                , "Customer/PT/Poker/MasterUpdate.aspx?custId=" + custId + "&roleId=" + roleId
                , 0, 0, 640, 480, false, 2000, null);
    },
    liveCasinoSetting: function (custId, exchangeId, uplineId) {
        showPopup("Live Casino"
                , window.rootUrl + "LiveCasinoSetting/LiveCSSetting/EditSetting?uplineId=" + uplineId + "&directUplineId=" + uplineId + "&uplineRoleId=4&adminId=" + _adminID + "&targetCustId=" + custId + "&targetRoleId=3"
                , 0, 0, 880, 580, false, 2000, null);
    },
    virtualSportsSetting: function (custId, uplineId, roleId) {
        showPopup("Virtual Sport"
                , "VirtualSportMasterPT/EditMaster?custId=" + custId + "&uplineId=" + uplineId + "&roleId=" + roleId
                , 0, 0, 860, 480, false, 2000, null);
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
