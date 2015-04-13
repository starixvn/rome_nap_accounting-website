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
        gridWidth: 1600,
        gridHeight: 40,
        freezeWidth: 365, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 5
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
                        AgentList.updateDoubleComm(id, isAllow, isMultiple);
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
                            AgentList.updateDoubleComm(id, true, isMultiple);
                        }
                        else {
                            AgentList.updateDoubleComm(id, false, isMultiple);
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
        AgentList.paramsUpdate.userId = _masterId;
        AgentList.paramsUpdate.custDetail = id.toString();

        AgentList.paramsUpdate.isDoubleComm = isAllow;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerList/UpdateDoubleComm",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(AgentList.paramsUpdate),
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
                            var error = '<tr><td><span class="username">' + ResultCustomerlist[i].UserName + '</span><span class="fail"> :' + (ResultCustomerlist[i].Result.ErrorMessage != "" ? ResultCustomerlist[i].Result.ErrorMessage : " Update fail .") + '</span></tr></tr>';
                            arr.push(error)
                            errorFlag = true;
                        }
                    }

                    if (!errorFlag) {
                        setTimeout('$("#tblconfirm").dialog("close")', 2000);

                        var htmlsuccess = '';
                        htmlsuccess = '<tr><td><span class="username"></span><span class="successful"> Update successful</span></tr></tr>';
                        var title = "";
                        if (!isMultiple) {
                            title = "Edit Single Double Commission for Losing tickets";
                        }
                        else {
                            title = "Edit Multiple Double Commission for Losing tickets";
                        }

                        //                        $('#tblconfirm').empty().append(htmlsuccess);
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
                                AgentList.getAgentList();
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
    OpenEdit: function (masterid, custid, uname) {
        var urlEdit = "";
        if (_IsShowCustomerSettingRenovation == 3) {
            urlEdit = "EditAgent/EditAgentPage";
        }
        else {

            urlEdit = "Customer/v2/EditAgent.aspx";
        }
        var url = urlEdit + "?custid=" + custid + "&masterId=" + masterid + "&uname=" + uname;
        var pageIndex = "1";
        if (location.href.indexOf("#") > -1) {
            pageIndex = location.href.split("#")[1];
        }
        showPopup("Update Agent Setting", url, "", "", 1000, 650, false, 2000, function () { callAjax(getParams(pageIndex)); });
    },
    editSetting: function (custId, roleId) {
        showPopup("Poker"
                , "Customer/PT/Poker/AgentUpdate.aspx?custId=" + custId + "&roleId=" + roleId
                , 0, 0, 640, 480, false, 2000, null);
    },
    liveCasinoSetting: function (custId, exchangeId, uplineId) {
        showPopup("Live Casino"
                , "LiveCasino/PTSetting/EditAgent.aspx?custId=" + custId + "&uplineId=" + uplineId
                , 0, 0, 640, 480, false, 2000, null);
    },
    virtualSportsSetting: function (custId, uplineId, roleId) {
        showPopup("Virtual Sport"
                , "VirtualSportAgentPT/EditAgent?custId=" + custId + "&uplineId=" + uplineId + "&roleId=" + roleId
                , 0, 0, 860, 480, false, 2000, null);
    },
    kenoSetting: function (custId) {
        showPopup(""
                , "customersetting/KenoSetting/kenosetting/EditAgent?custId=" + custId + "&userId = " + _userId, 0, 0, 900, 700, false, 2000, null);
    },
    casino: function (custId, uplineId) {
        showPopup(""
                , "CustomerSetting/Casino/Agent/EditAgent/" + custId + "/ " + uplineId, 0, 0, 1024, 700, false, 2000, null);
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
